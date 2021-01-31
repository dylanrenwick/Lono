using System;
using System.Text;

using Lono.Core.Assets;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lono.MonoGame
{
    public class MonoGameRenderWrapper : IRenderWrapper
    {
        private SpriteBatch innerRenderer;
        private GraphicsDevice graphics;

        private SpriteFont defaultFont;

        public Lono.Data.Vector2 CameraOffset { get; set; }

        public MonoGameRenderWrapper(GraphicsDevice g, SpriteFont defaultFont, SpriteBatch sb = null)
        {
            graphics = g;
            innerRenderer = sb ?? new SpriteBatch(g);

            this.defaultFont = defaultFont;

            CameraOffset = Lono.Data.Vector2.Zero;
        }

        public void DrawLine(Lono.Data.Vector2 start, Lono.Data.Vector2 end, Lono.Data.Color color)
        {
            innerRenderer.DrawLine(LonoVectorToXNAVector(start + CameraOffset), LonoVectorToXNAVector(end + CameraOffset), new Color(color.R, color.G, color.B, color.A));
        }

        public void DrawCircle(Lono.Data.Vector2 origin, double radius, Lono.Data.Color color)
        {
            innerRenderer.DrawCircle(LonoVectorToXNAVector(origin + CameraOffset), (float)radius, (int)radius * 4, new Color(color.R, color.G, color.B, color.A));
        }
        public void DrawCircle(Lono.Data.Vector2 origin, double radius, float thickness, Lono.Data.Color color)
        {
            innerRenderer.DrawCircle(LonoVectorToXNAVector(origin + CameraOffset), (float)radius, (int)radius * 4, new Color(color.R, color.G, color.B, color.A), thickness);
        }

        public void DrawSprite(SpriteAsset sprite, Lono.Data.Vector2 pos)
        {
            innerRenderer.Draw(SpriteAssetToTexture2D(sprite), LonoVectorToXNAVector(pos + CameraOffset), Color.White);
        }

        public void DrawText(string text, Lono.Data.Vector2 pos)
        {
            innerRenderer.End();
            innerRenderer.Begin();
            innerRenderer.DrawString(defaultFont, text, LonoVectorToXNAVector(pos + CameraOffset), Color.White);
            innerRenderer.End();
            BeginCall();
        }
        public void DrawText(string text, Lono.Data.Vector2 pos, Lono.Data.Vector2 bounds)
        {
            var sb = new StringBuilder();

            int textOffset = 0;

            Vector2 textSize = defaultFont.MeasureString("");

            for (double y = 0; y < bounds.Y; y += textSize.Y)
            {
                textSize = defaultFont.MeasureString("");
                string snip = "";

                while (textSize.X < bounds.X)
                {
                    sb.Append(snip);
                    if (textOffset + sb.Length >= text.Length) break;

                    int nextSpace = text.IndexOf(' ', textOffset + sb.Length + 1);
                    int nextLine = text.IndexOf('\n', textOffset + sb.Length);

                    int nextBreak = nextLine < 0 ? (nextSpace < 0 ? text.Length : nextSpace) : Math.Min((nextSpace < 0 ? text.Length : nextSpace), nextLine);

                    snip = text.Substring(textOffset + sb.Length, nextBreak - (textOffset + sb.Length));

                    textSize = defaultFont.MeasureString(sb.ToString() + snip);

                    if (nextBreak == nextLine)
                    {
                        if (textSize.X < bounds.X)
                        {
                            sb.Append(snip);
                            textOffset++;
                        }
                        break;
                    }
                }

                innerRenderer.End();
                innerRenderer.Begin();
                innerRenderer.DrawString(defaultFont, sb.ToString().Trim(), LonoVectorToXNAVector(pos + CameraOffset + new Lono.Data.Vector2(0, y)), Color.White);
                innerRenderer.End();
                BeginCall();
                textOffset += sb.Length;
                sb.Clear();

                if (textOffset >= text.Length) break;
            }
        }

        public void FillRect(Lono.Data.Vector2 pos, Lono.Data.Vector2 size, Lono.Data.Color color)
        {
            innerRenderer.FillRectangle(RectangleFromLonoVectors(pos + CameraOffset, size), new Color(color.R, color.G, color.B, color.A));
        }

        public void BeginCall()
        {
            innerRenderer.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        }

        private Texture2D SpriteAssetToTexture2D(SpriteAsset asset)
        {
            Texture2D tex = new Texture2D(graphics, asset.Width, asset.Height, false, SurfaceFormat.ColorSRgb);
            tex.SetData(asset.Data);
            Color[] px = new Color[tex.Width * tex.Height];
            tex.GetData(px);
            return tex;
        }

        private Vector2 LonoVectorToXNAVector(Lono.Data.Vector2 pos)
        {
            return new Vector2((float)pos.X, (float)pos.Y);
        }

        private Rectangle RectangleFromLonoVectors(Lono.Data.Vector2 pos, Lono.Data.Vector2 size)
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }
    }
}
