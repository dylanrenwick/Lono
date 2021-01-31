using Lono.Core.Assets;
using Lono.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lono
{
    public interface IRenderWrapper
    {
        Vector2 CameraOffset { get; set; }

        void DrawLine(Vector2 start, Vector2 end, Color color);
        void DrawCircle(Vector2 origin, double radius, Color color);
        void DrawCircle(Vector2 origin, double radius, float thickness, Color color);
        void DrawSprite(SpriteAsset sprite, Vector2 pos);
        void DrawText(string text, Vector2 pos);
        void DrawText(string text, Vector2 pos, Vector2 bounds);

        void FillRect(Vector2 pos, Vector2 size, Color color);
    }
}
