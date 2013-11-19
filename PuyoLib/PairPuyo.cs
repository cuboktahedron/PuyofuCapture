using System;
using System.Collections;

namespace Cubokta.Puyo.Common
{
    /// <summary>
    /// 組ぷよインタフェース
    /// </summary>
    public interface PairPuyo
    {
        /// <summary>
        /// 組ぷよのぷよ種別を取得する
        /// </summary>
        /// <param name="index">ぷよ番号</param>
        /// <returns>ぷよ番号に対応するぷよ種別</returns>
        PuyoType this[int index] { get; set; }

        /// <summary>方向</summary>
        Direction4 Dir { get; set; }

        /// <summary>軸ぷよの設置位置</summary>
        int Pos { get; set; }

        /// <summary>お邪魔ぷよかどうか</summary>
        bool IsOjama { get; }

        /// <summary>お邪魔ぷよの落下段数</summary>
        int OjamaRow { get; set; }

        /// <summary>お邪魔ぷよの端数情報</summary>
        BitArray OjamaBit { get; set; }

        /// <summary>
        /// 指定列のお邪魔ぷよ落下数を取得する
        /// </summary>
        /// <param name="pos">列番号(0～)</param>
        /// <returns>お邪魔ぷよ落下数</returns>
        int OjamaAt(int pos);
    }

    /// <summary>
    /// 2つ組の組ぷよ
    /// </summary>
    public class ColorPairPuyo : PairPuyo
    {
        /// <summary>軸ぷよ</summary>
        public PuyoType Pivot { get; set; }

        /// <summary>衛星ぷよ</summary>
        public PuyoType Satellite { get; set; }

        /// <summary>
        /// 組ぷよのぷよ種別を取得する
        /// </summary>
        /// <param name="index">ぷよ番号</param>
        /// <returns>ぷよ番号に対応するぷよ種別</returns>
        public PuyoType this[int index]
        {
            get
            {
                switch (index) {
                    case 0: return Pivot;
                    case 1: return Satellite;
                    default: throw new ArgumentException("index must be 0 or 1.");
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        Pivot = value;
                        break;
                    case 1:
                        Satellite = value;
                        break;
                    default:
                        throw new ArgumentException("index must be 0 or 1.");
                }
            }
        }

        /// <summary>
        /// 軸ぷよから見た衛星ぷよの方向
        /// </summary>
        public Direction4 Dir { get; set; }

        /// <summary>
        /// 軸ぷよの設置位置
        /// </summary>
        public int Pos { get; set; }

        /// <summary>
        /// お邪魔ぷよかどうか
        /// 常にfalseです
        /// </summary>
        public bool IsOjama
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// お邪魔ぷよの落下段数
        /// このクラスではサポートしません
        /// </summary>
        /// <exception cref="NotSupportedException">常に発生します</exception>
        public int OjamaRow
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// お邪魔ぷよの端数情報
        /// このクラスではサポートしません
        /// </summary>
        /// <exception cref="NotSupportedException">常に発生します</exception>
        public BitArray OjamaBit
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// 指定列のお邪魔ぷよ落下数を取得する
        /// このクラスではサポートしません
        /// </summary>
        /// <param name="pos">列番号(0～)</param>
        /// <returns>お邪魔ぷよ落下数</returns>
        /// <exception cref="NotSupportedException">常に発生します</exception>
        public int OjamaAt(int pos)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 等価判定を行う
        /// </summary>
        /// <param name="obj">比較対象</param>
        /// <returns>オブジェクトの内容が等しいかどうか</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            ColorPairPuyo pp = (ColorPairPuyo)obj;
            return Pivot == pp.Pivot
                && Satellite == pp.Satellite
                && Dir == pp.Dir
                && Pos == pp.Pos;
        }

        /// <summary>
        /// このオブジェクトのハッシュ値を取得する
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            return (int)Pivot + (int)Satellite + (int)Dir + Pos;
        }
    }

    /// <summary>
    /// 落下お邪魔ぷよ
    /// </summary>
    public class OjamaPairPuyo : PairPuyo
    {
        /// <summary>
        /// 組ぷよのぷよ種別を取得する
        /// このクラスではサポートしません
        /// </summary>
        /// <param name="index">ぷよ番号</param>
        /// <returns>ぷよ番号に対応するぷよ種別</returns>
        /// <exception cref="NotSupportedException">常に発生します</exception>
        public PuyoType this[int index]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }


        /// <summary>
        /// 方向
        /// このクラスではサポートしません
        /// </summary>
        /// <exception cref="NotSupportedException">常に発生します</exception>
        public Direction4 Dir
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// 軸ぷよの設置位置
        /// このクラスではサポートしません
        /// </summary>
        /// <exception cref="NotSupportedException">常に発生します</exception>
        public int Pos
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// お邪魔ぷよかどうか
        /// 常にtrueです
        /// </summary>
        public bool IsOjama
        {
            get
            {
                return true;
            }
        }

        /// <summary>お邪魔ぷよの落下段数</summary>
        private int ojamaRow;
        public int OjamaRow {
            get
            {
                return ojamaRow;
            }

            set
            {
                if (value < 0 || value > FieldConst.OJAMA_ROW_MAX)
                {
                    throw new ArgumentException("OjamaRow must be between 0 and " + FieldConst.OJAMA_ROW_MAX);
                }

                ojamaRow = value;
            }
        }

        /// <summary>お邪魔ぷよの端数情報</summary>
        private BitArray ojamaBit = new BitArray(FieldConst.FIELD_X);

        /// <summary>お邪魔ぷよの端数情報</summary>
        public BitArray OjamaBit
        {
            get
            {
                return ojamaBit;
            }

            set
            {
                ojamaBit = value;
            }
        }

        /// <summary>
        /// 指定列のお邪魔ぷよ落下数を取得する
        /// </summary>
        /// <param name="pos">列番号(0～)</param>
        /// <returns>お邪魔ぷよ落下数</returns>
        public int OjamaAt(int pos)
        {
            if (pos < 0 || pos >= FieldConst.FIELD_X)
            {
                throw new ArgumentException("pos '" + pos + "' is out of field.");
            }

            return OjamaRow + (OjamaBit[pos] ? 1 : 0);
        }

        /// <summary>
        /// 等価判定を行う
        /// </summary>
        /// <param name="obj">比較対象</param>
        /// <returns>オブジェクトの内容が等しいかどうか</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            OjamaPairPuyo ojama = (OjamaPairPuyo)obj;
            if (OjamaRow != ojama.OjamaRow)
            {
                return false;
            }

            if (ojamaBit.Count != ojama.ojamaBit.Count)
            {
                return false;
            }

            for (int i = 0; i < OjamaBit.Length; i++)
            {
                if (ojamaBit[i] != ojama.OjamaBit[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// このオブジェクトのハッシュ値を取得する
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            return OjamaRow + OjamaBit.GetHashCode();
        }
    }
}
