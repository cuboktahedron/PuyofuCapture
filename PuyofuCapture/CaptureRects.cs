using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo
{
    public class CaptureRects
    {
        private const float CELL_W_RATIO = 25.0f / 460;
        private const float CELL_H_RATIO = 27.5f / 330;
        private const float FIELD_W_RATIO = CELL_W_RATIO * 6;
        private const float FIELD_H_RATIO = 1; // CELL_H_RATIO * 12
        private const float NEXT_TOP_RATIO = 53.0f / 330;
        private const float NEXT1_LEFT_RATIO = 176.0f / 460;
        private const float NEXT2_LEFT_RATIO = 259.0f / 460;
        private const float FIELD2_LEFT_RATIO = 309.0f / 460;

        public Rectangle CaptureRect { get; private set; }
        private Rectangle[] fieldRects = new Rectangle[2];
        private Rectangle[] nextRects = new Rectangle[2];

        public void CalculateRects(Rectangle rect)
        {
            this.CaptureRect = rect;

            fieldRects[0] = new Rectangle()
            {
                X = rect.X,
                Y = rect.Y,
                Width = (int)(rect.Width * FIELD_W_RATIO),
                Height = rect.Height
            };

            fieldRects[1] = new Rectangle()
            {
                X = rect.X + (int)(rect.Width * FIELD2_LEFT_RATIO),
                Y = rect.Y,
                Width = (int)(rect.Width * FIELD_W_RATIO),
                Height = rect.Height
            };

            nextRects[0] = new Rectangle()
            {
                X = rect.X + (int)(rect.Width * NEXT1_LEFT_RATIO),
                Y = rect.Y + (int)(rect.Height * NEXT_TOP_RATIO),
                Width = (int)(rect.Width * CELL_W_RATIO),
                Height = (int)(rect.Height * CELL_H_RATIO * 2),
            };

            nextRects[1] = new Rectangle()
            {
                X = rect.X + (int)(rect.Width * NEXT2_LEFT_RATIO),
                Y = rect.Y + (int)(rect.Height * NEXT_TOP_RATIO),
                Width = (int)(rect.Width * CELL_W_RATIO),
                Height = (int)(rect.Height * CELL_H_RATIO * 2),
            };
        }

        public void CalculateRects(Point begin, Point end) {
            int width = Math.Abs(end.X - begin.X);
            int height = Math.Abs(end.Y - begin.Y);
            int left = Math.Min(begin.X, end.X);
            int top = Math.Min(begin.Y, end.Y);

            CalculateRects(new Rectangle()
            {
                X = left,
                Y = top,
                Width = width,
                Height = height
            });
        }

        public Rectangle GetFieldRect(int fieldNo)
        {
            return fieldRects[fieldNo];
        }

        public Rectangle GetNextRect(int fieldNo)
        {
            return nextRects[fieldNo];
        }
    }
}
