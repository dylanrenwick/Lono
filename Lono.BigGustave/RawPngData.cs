namespace Lono.BigGustave
{
    using System;

    public class RawPngData
    {
        public readonly byte[] PixelData;
        public readonly int BytesPerPixel;
        public readonly int Width;
        public readonly int Height;
        public readonly Palette Palette;
        public readonly ColorType ColorType;
        public readonly int RowOffset;
        public readonly int BitDepth;

        /// <summary>
        /// Create a new <see cref="RawPngData"/>.
        /// </summary>
        /// <param name="data">The decoded pixel data as bytes.</param>
        /// <param name="bytesPerPixel">The number of bytes in each pixel.</param>
        /// <param name="palette">The palette for the image.</param>
        /// <param name="imageHeader">The image header.</param>
        public RawPngData(byte[] data, int bytesPerPixel, Palette palette, ImageHeader imageHeader)
        {
            if (Width < 0)
            {
                throw new ArgumentOutOfRangeException($"Width must be greater than or equal to 0, got {Width}.");
            }

            this.PixelData = data ?? throw new ArgumentNullException(nameof(data));
            this.BytesPerPixel = bytesPerPixel;
            this.Palette = palette;
            
            Width = imageHeader.Width;
            Height = imageHeader.Height;
            ColorType = imageHeader.ColorType;
            RowOffset = imageHeader.InterlaceMethod == InterlaceMethod.Adam7 ? 0 : 1;
            BitDepth = imageHeader.BitDepth;
        }

        public byte[] GetRawPixelData()
        {
            byte[] buffer = new byte[Width * Height * BytesPerPixel];
            for (int y = 0; y < Height; y++)
            {
                var bufferStartPixel = BytesPerPixel * Width * y;
                var rowStartPixel = bufferStartPixel + RowOffset + (RowOffset * y);
                Array.Copy(PixelData, rowStartPixel, buffer, bufferStartPixel, Width * BytesPerPixel);
            }
            return buffer;
        }

        public Pixel GetPixel(int x, int y)
        {
            if (Palette != null)
            {
                var pixelsPerByte = (8 / BitDepth);

                var bytesInRow = (1 + (Width / pixelsPerByte));

                var byteIndexInRow = x / pixelsPerByte;
                var paletteIndex = (1 + (y * bytesInRow)) + byteIndexInRow;

                var b = PixelData[paletteIndex];

                if (BitDepth == 8)
                {
                    return Palette.GetPixel(b);
                }

                var withinByteIndex = x % pixelsPerByte;
                var rightShift = 8 - ((withinByteIndex + 1) * BitDepth);
                var indexActual = (b >> rightShift) & ((1 << BitDepth) - 1);

                return Palette.GetPixel(indexActual);
            }

            var rowStartPixel = (RowOffset + (RowOffset * y)) + (BytesPerPixel * Width * y);

            var pixelStartIndex = rowStartPixel + (BytesPerPixel * x);

            var first = PixelData[pixelStartIndex];

            switch (BytesPerPixel)
            {
                case 1:
                    return new Pixel(first, first, first, 255, true);
                case 2:
                    switch (ColorType)
                    {
                        case ColorType.None:
                        {
                            byte second = PixelData[pixelStartIndex + 1];
                            var value = ToSingleByte(first, second);
                            return new Pixel(value, value, value, 255, true);

                        }
                        default:
                            return new Pixel(first, first, first, PixelData[pixelStartIndex + 1], true);
                    }

                case 3:
                    return new Pixel(first, PixelData[pixelStartIndex + 1], PixelData[pixelStartIndex + 2], 255, false);
                case 4:
                    switch (ColorType)
                    {
                        case ColorType.None | ColorType.AlphaChannelUsed:
                        {
                            var second = PixelData[pixelStartIndex + 1];
                            var firstAlpha = PixelData[pixelStartIndex + 2];
                            var secondAlpha = PixelData[pixelStartIndex + 3];
                            var gray = ToSingleByte(first, second);
                            var alpha = ToSingleByte(firstAlpha, secondAlpha);
                            return new Pixel(gray, gray, gray, alpha, true);
                        }
                        default:
                            return new Pixel(first, PixelData[pixelStartIndex + 1], PixelData[pixelStartIndex + 2], PixelData[pixelStartIndex + 3], false);
                    }
                case 6:
                    return new Pixel(first, PixelData[pixelStartIndex + 2], PixelData[pixelStartIndex + 4], 255, false);
                case 8:
                    return new Pixel(first, PixelData[pixelStartIndex + 2], PixelData[pixelStartIndex + 4], PixelData[pixelStartIndex + 6], false);
                default:
                    throw new InvalidOperationException($"Unreconized number of bytes per pixel: {BytesPerPixel}.");
            }
        }

        private static byte ToSingleByte(byte first, byte second)
        {
            var us = (first << 8) + second;
            var result = (byte)Math.Round((255 * us) / (double)ushort.MaxValue);
            return result;
        }
    }
}