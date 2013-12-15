/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/license/LICENSE-MIT.txt
 */
using Cubokta.Common.game;
using Cubokta.Puyo.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Cubokta.Puyo
{
    /// <summary>
    /// フィールド状態を表すクラス
    /// </summary>
    /// <remarks>
    /// フィールドのサイズは6×12
    /// フィールドの左下のセルの座標を(0, 0)とする。
    /// 
    /// </remarks>
    public class CaptureField
    {
        /// <summary>フィールドの幅</summary>
        public const int X_MAX = FieldConst.FIELD_X;

        /// <summary>フィールドの高さ</summary>
        public const int Y_MAX = FieldConst.FIELD_Y;

        /// <summary>1セルの辺の長さ(ピクセル)</summary>
        public const int UNIT = 32;

        /// <summary>フィールド情報</summary>
        private PuyoType[,] Types { get; set; }

        /// <summary>ネクスト情報</summary>
        public ColorPairPuyo Next { get; set; }

        /// <summary>
        /// 文字列表現用の変換テーブル
        /// </summary>
        private static readonly IDictionary<PuyoType, string> TYPE2CHAR = new Dictionary<PuyoType, string>()
        {
            { PuyoType.NONE, "□" },
            { PuyoType.AKA, "赤" },
            { PuyoType.MIDORI, "緑" },
            { PuyoType.AO, "青" },
            { PuyoType.KI, "黄" },
            { PuyoType.MURASAKI, "紫" },
        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CaptureField()
        {
            Types = new PuyoType[Y_MAX, X_MAX];
        }

        /// <summary>
        /// 指定した座標のぷよ種別を取得する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>ぷよ種別</returns>
        public PuyoType GetPuyoType(int x, int y)
        {
            return Types[(Y_MAX - 1) - y, x];
        }

        /// <summary>
        /// 指定した座標のぷよ種別を設定する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="type">設定するぷよ種別</param>
        public void SetPuyoType(int x, int y, PuyoType type)
        {
            Types[(Y_MAX - 1) - y, x] = type;
        }

        /// <summary>
        /// 1セルのキャプチャ範囲を取得する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>1セルのキャプチャ範囲</returns>
        public Rectangle GetRect(int x, int y)
        {
            return new Rectangle()
            {
                X = x * UNIT,
                Y = ((Y_MAX - 1) - y) * UNIT,
                Width = UNIT,
                Height = UNIT,
            };
        }

        /// <summary>
        /// ネクストツモのキャプチャ範囲を取得する
        /// </summary>
        /// <param name="no">フィールド番号</param>
        /// <param name="puyoNo">ぷよ番号(軸ぷよ：0、衛星ぷよ：1)</param>
        /// <returns>ネクストのキャプチャ範囲</returns>
        public Rectangle GetNextRect(int no, int puyoNo)
        {
            return new Rectangle()
            {
                X = 0,
                Y = (1 - puyoNo) * UNIT,
                Width = UNIT,
                Height = UNIT,
            };
        }

        /// <summary>
        /// キャプチャ範囲の差分からツモの設置情報を特定する
        /// </summary>
        /// <param name="f2">ツモを設置後のキャプチャフィールド</param>
        /// <param name="pp">設置したツモ</param>
        /// <returns>ツモの設置情報</returns>
        public ColorPairPuyo GetStepFromDiff(CaptureField f2, ColorPairPuyo pp)
        {
            CaptureField f1 = this;
            bool foundPivot = false;
            bool foundSatellite = false;
            ColorPairPuyo p2 = new ColorPairPuyo();
            Point pivotPt = new Point(-1, -1);
            Point satellitePt = new Point(-1, -1);

            for (int x = 0; x < X_MAX; x++)
            {
                for (int y = 0; y < Y_MAX; y++)
                {
                    PuyoType pt1 = f1.GetPuyoType(x, y);
                    PuyoType pt2 = f2.GetPuyoType(x, y);

                    if (pt1 != pt2)
                    {
                        if (!foundPivot && pp.Pivot == pt2)
                        {
                            p2.Pos = x;
                            p2.Pivot = pt2;
                            foundPivot = true;
                            pivotPt.X = x;
                            pivotPt.Y = y;
                        }
                        else if (pp.Satellite == pt2)
                        {
                            p2.Satellite = f2.GetPuyoType(x, y);
                            foundSatellite = true;
                            satellitePt.X = x;
                            satellitePt.Y = y;
                        }
                        else
                        {
                            return null;
                        }

                        if (foundPivot && foundSatellite)
                        {
                            if (pivotPt.X == satellitePt.X && pivotPt.Y < satellitePt.Y)
                            {
                                p2.Dir = Direction4.UP;
                            }
                            else if (pivotPt.X == satellitePt.X)
                            {
                                p2.Dir = Direction4.DOWN;
                            }
                            else if (pivotPt.X < satellitePt.X)
                            {
                                p2.Dir = Direction4.RIGHT;
                            }
                            else
                            {
                                p2.Dir = Direction4.LEFT;
                            }

                            p2.Pos++;
                            return p2;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// ツモを落下させフィールド情報を更新する
        /// </summary>
        /// <param name="pp">ツモ</param>
        public void Drop(ColorPairPuyo pp)
        {
            int pos = pp.Pos - 1;
            if (pp.Dir == Direction4.UP)
            {
                DropOne(pp.Pivot, pos);
                DropOne(pp.Satellite, pos);
            }
            else if (pp.Dir == Direction4.DOWN)
            {
                DropOne(pp.Satellite, pos);
                DropOne(pp.Pivot, pos);
            }
            else if (pp.Dir == Direction4.LEFT)
            {
                DropOne(pp.Pivot, pos);
                DropOne(pp.Satellite, pos - 1);
            }
            else
            {
                DropOne(pp.Pivot, pos);
                DropOne(pp.Satellite, pos + 1);
            }
        }

        /// <summary>
        /// １つのぷよを落下させフィールド情報を更新する
        /// </summary>
        /// <param name="type">ぷよ種別</param>
        /// <param name="pos">落下位置</param>
        private void DropOne(PuyoType type, int pos)
        {
            for (int y = 0; y < Y_MAX; y++)
            {
                if (GetPuyoType(pos, y) == PuyoType.NONE)
                {
                    SetPuyoType(pos, y, type);
                    return;
                }
            }
        }

        /// <summary>
        /// フィールド状態を補正する
        /// 中に浮いているぷよは除外する
        /// </summary>
        public void Correct()
        {
            for (int y = 1; y < Y_MAX; y++)
            {
                for (int x = 0; x < X_MAX; x++)
                {
                    if (GetPuyoType(x, y - 1) == PuyoType.NONE)
                    {
                        SetPuyoType(x, y, PuyoType.NONE);
                    }
                }
            }
        }

        /// <summary>
        /// このオブジェクトの文字列表現
        /// </summary>
        /// <returns>オブジェクトの文字列表現</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < Y_MAX; y++)
            {
                for (int x = 0; x < X_MAX; x++)
                {
                    sb.Append(TYPE2CHAR[Types[y, x]]);
                }

                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
