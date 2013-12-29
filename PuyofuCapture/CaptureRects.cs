/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using System;
using System.Drawing;

namespace Cubokta.Puyo
{
    /// <summary>
    /// スクリーンキャプチャ領域を表すクラス
    /// </summary>
    public class CaptureRects
    {
        /// <summary>計算の基準となる画面の横幅</summary>
        private const int STANDARD_SCREEN_WIDTH = 460;

        /// <summary>計算の基準となる画面の高さ</summary>
        private const int STANDARD_SCREEN_HEIGHT = 330;

        /// <summary>画面の横幅に対するフィールドセル1つあたりの横幅の比</summary>
        private const float CELL_W_RATIO = 25.0f / STANDARD_SCREEN_WIDTH;

        /// <summary>画面の高さに対するフィールドセル1つあたりの高さの比</summary>
        private const float CELL_H_RATIO = 27.5f / STANDARD_SCREEN_HEIGHT;

        /// <summary>画面の横幅に対するフィールド１つあたりの幅の比</summary>
        private const float FIELD_W_RATIO = CELL_W_RATIO * 6;

        /// <summary>画面の高さに対するフィールド１つあたりの高さの比</summary>
        private const float FIELD_H_RATIO = 1; // CELL_H_RATIO * 12

        /// <summary>画面の高さに対する第一ネクスト領域の上端の位置の比</summary>
        private const float NEXT_TOP_RATIO = 53.0f / STANDARD_SCREEN_HEIGHT;

        /// <summary>画面の幅に対する1Pフィールドの第一ネクスト領域の左端の位置の比</summary>
        private const float NEXT1_LEFT_RATIO = 176.0f / STANDARD_SCREEN_WIDTH;

        /// <summary>画面の幅に対する2Pフィールドの第一ネクスト領域の左端の位置の比</summary>
        private const float NEXT2_LEFT_RATIO = 259.0f / STANDARD_SCREEN_WIDTH;

        /// <summary>画面の幅に対する2Pフィールドの左端の位置の比</summary>
        private const float FIELD2_LEFT_RATIO = 309.0f / STANDARD_SCREEN_WIDTH;

        /// <summary>キャプチャ領域</summary>
        public Rectangle CaptureRect { get; private set; }

        private Rectangle[] fieldRects = new Rectangle[2];
        private Rectangle[] nextRects = new Rectangle[2];

        /// <summary>キャプチャ領域から各種個別のキャプチャ領域を計算する</summary>
        /// <param name="rect">キャプチャ領域</param>
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

        /// <summary>キャプチャ領域から各種個別のキャプチャ領域を計算する</summary>
        /// <param name="begin">キャプチャ領域の始点</param>
        /// <param name="begin">キャプチャ領域の終点</param>
        public void CalculateRects(Point begin, Point end)
        {
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

        /// <summary>
        /// フィールドキャプチャ領域を取得する
        /// </summary>
        /// <param name="fieldNo">フィールド番号(0オリジン)</param>
        /// <returns>フィールドキャプチャ領域</returns>
        public Rectangle GetFieldRect(int fieldNo)
        {
            return fieldRects[fieldNo];
        }

        /// <summary>
        /// 第一ネクスト領域を取得する
        /// </summary>
        /// <param name="fieldNo">フィールド番号(0オリジン)</param>
        /// <returns>第一ネクスト領域</returns>
        public Rectangle GetNextRect(int fieldNo)
        {
            return nextRects[fieldNo];
        }
    }
}
