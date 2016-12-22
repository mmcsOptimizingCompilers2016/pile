using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class EqualsBitArray : ICloneable
    {
        public BitArray bits { get; set; }

        public EqualsBitArray(BitArray bits)
        {
            this.bits = bits;
        }

        public EqualsBitArray(int count, bool value)
        {
            this.bits = new BitArray(count, value);
        }

        public int length()
        {
            return bits.Length;
        }

        public EqualsBitArray Or(EqualsBitArray other)
        {
            return new EqualsBitArray(this.bits.Or(other.bits));
        }
        public EqualsBitArray And(EqualsBitArray other)
        {
            return new EqualsBitArray(this.bits.And(other.bits));
        }
        public EqualsBitArray Xor(EqualsBitArray other)
        {
            return new EqualsBitArray(this.bits.Xor(other.bits));
        }
        public EqualsBitArray Not()
        {
            return new EqualsBitArray(this.bits.Not());
        }
        public void Set(int index, bool value)
        {
            bits.Set(index, value);
        }
        public bool Get(int index)
        {
            return bits.Get(index);
        }

        public override bool Equals(object obj)
        {
            EqualsBitArray other = obj as EqualsBitArray;
            if (this.length() != other.length())
            {
                return false;
            }

            for (int i = 0; i < this.bits.Length; i++)
            {
                if (this.bits[i] != other.bits[i])
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
            foreach (var bit in bits)
            {
                fullString += bit.ToString() == "True" ? "1" : "0";
            }
            return fullString;
        }

        public object Clone()
        {
            return new EqualsBitArray(bits.Clone() as BitArray);
        }
    }
}
