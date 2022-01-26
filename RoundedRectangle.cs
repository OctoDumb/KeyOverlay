using System;
using SFML.Graphics;
using SFML.System;

namespace KeyOverlay
{
    class RoundedRectangle : ConvexShape
    {
        public RoundedRectangle(float width, float height, float radius)
        {
            radius = Math.Min(radius, Math.Min(width, height) / 2);

            uint points = ((uint)Math.Ceiling(radius)) * 2;

            SetPointCount(points * 4);

            float x = 0, y = 0;

            for(uint i = 0; i < points; i++)
            {
                x += radius / points;
                y = (float)Math.Sqrt(radius * radius - x * x);
                SetPoint(i, new Vector2f(x + width - radius, radius - y));
            }
            y = 0;
            for(uint i = 0; i < points; i++)
            {
                y += radius / points;
                x = (float)Math.Sqrt(radius * radius - y * y);
                SetPoint(points + i, new Vector2f(x + width - radius, y + height - radius));
            }
            x = 0;
            for(uint i = 0; i < points; i++)
            {
                x += radius / points;
                y = (float)Math.Sqrt(radius * radius - x * x);
                SetPoint(points * 2 + i, new Vector2f(radius - x, y + height - radius));
            }
            y = 0;
            for(uint i = 0; i < points; i++)
            {
                y += radius / points;
                x = (float)Math.Sqrt(radius * radius - y * y);
                SetPoint(points * 3 + i, new Vector2f(radius - x, radius - y));
            }
        }
    }
}