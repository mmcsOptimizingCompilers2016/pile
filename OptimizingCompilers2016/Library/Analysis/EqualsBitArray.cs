using System;
using System.Collections;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class EqualsBitArray : ICloneable
    {
        public int Length => Bits.Length;

        public BitArray Bits { get; set; }

        public EqualsBitArray(BitArray bits)
        {
            this.Bits = bits;
        }

        public EqualsBitArray(int count, bool value)
        {
            this.Bits = new BitArray(count, value);
        }

        public EqualsBitArray Or(EqualsBitArray other)
        {
            return new EqualsBitArray(this.Bits.Or(other.Bits));
        }

        public EqualsBitArray And(EqualsBitArray other)
        {
            return new EqualsBitArray(this.Bits.And(other.Bits));
        }

        public EqualsBitArray Xor(EqualsBitArray other)
        {
            return new EqualsBitArray(this.Bits.Xor(other.Bits));
        }

        public EqualsBitArray Not()
        {
            return new EqualsBitArray(this.Bits.Not());
        }
        public void Set(int index, bool value)
        {
            Bits.Set(index, value);
        }

        public bool Get(int index)
        {
            return Bits.Get(index);
        }

        public override bool Equals(object obj)
        {
            EqualsBitArray other = obj as EqualsBitArray;
            if (this.Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < this.Bits.Length; i++)
            {
                if (this.Bits[i] != other.Bits[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            String fullString = "";
            foreach (var bit in Bits)
            {
                fullString += bit.ToString() == "True" ? "1" : "0";
            }
            return fullString;
        }

        public object Clone()
        {
            return new EqualsBitArray(Bits.Clone() as BitArray);
        }
    }
}
