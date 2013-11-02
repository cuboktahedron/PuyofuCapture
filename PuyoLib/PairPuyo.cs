using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo.Common
{
    public interface PairPuyo
    {
        PuyoType this[int index] { get; set; }
        Direction Dir { get; set; }
        int Pos { get; set; }
        bool IsOjama { get; }
        int OjamaRow { get; set; }
        BitArray OjamaBit { get; set; }
        int OjamaAt(int pos);
    }

    public class ColorPairPuyo : PairPuyo
    {
        public PuyoType Pivot { get; set; }
        public PuyoType Satellite { get; set; }
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

        public Direction Dir { get; set; }
        public int Pos { get; set; }

        public bool IsOjama
        {
            get
            {
                return false;
            }
        }

        public int OjamaRow
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public BitArray OjamaBit
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public int OjamaAt(int pos)
        {
            throw new NotSupportedException();
        }

        public override bool Equals(object obj)
        {
            //objがnullか、型が違うときは、等価でない
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

        public override int GetHashCode()
        {
            return (int)Pivot + (int)Satellite + (int)Dir + Pos;
        }
    }

    public class OjamaPairPuyo : PairPuyo
    {
        public PuyoType this[int index]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public Direction Dir
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public int Pos
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public bool IsOjama
        {
            get
            {
                return true;
            }
        }

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

        private BitArray ojamaBit = new BitArray(FieldConst.FIELD_X);
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

        public int OjamaAt(int pos)
        {
            if (pos < 0 || pos >= FieldConst.FIELD_X)
            {
                throw new ArgumentException("pos '" + pos + "' is out of field.");
            }

            return OjamaRow + (OjamaBit[pos] ? 1 : 0);
        }

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

        public override int GetHashCode()
        {
            return OjamaRow + OjamaBit.GetHashCode();
        }
    }
}
