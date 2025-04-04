// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.BitSet
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Text;

#nullable disable
namespace Antlr.Runtime
{
  internal class BitSet : ICloneable
  {
    protected internal const int BITS = 64;
    protected internal const int LOG_BITS = 6;
    protected internal static readonly int MOD_MASK = 63;
    protected internal ulong[] bits;

    public BitSet()
      : this(64)
    {
    }

    public BitSet(ulong[] bits_) => this.bits = bits_;

    public BitSet(IList items)
      : this(64)
    {
      for (int index = 0; index < items.Count; ++index)
        this.Add((int) items[index]);
    }

    public BitSet(int nbits) => this.bits = new ulong[(nbits - 1 >> 6) + 1];

    public static BitSet Of(int el)
    {
      BitSet bitSet = new BitSet(el + 1);
      bitSet.Add(el);
      return bitSet;
    }

    public static BitSet Of(int a, int b)
    {
      BitSet bitSet = new BitSet(Math.Max(a, b) + 1);
      bitSet.Add(a);
      bitSet.Add(b);
      return bitSet;
    }

    public static BitSet Of(int a, int b, int c)
    {
      BitSet bitSet = new BitSet();
      bitSet.Add(a);
      bitSet.Add(b);
      bitSet.Add(c);
      return bitSet;
    }

    public static BitSet Of(int a, int b, int c, int d)
    {
      BitSet bitSet = new BitSet();
      bitSet.Add(a);
      bitSet.Add(b);
      bitSet.Add(c);
      bitSet.Add(d);
      return bitSet;
    }

    public virtual BitSet Or(BitSet a)
    {
      if (a == null)
        return this;
      BitSet bitSet = (BitSet) this.Clone();
      bitSet.OrInPlace(a);
      return bitSet;
    }

    public virtual void Add(int el)
    {
      int index = BitSet.WordNumber(el);
      if (index >= this.bits.Length)
        this.GrowToInclude(el);
      this.bits[index] |= BitSet.BitMask(el);
    }

    public virtual void GrowToInclude(int bit)
    {
      ulong[] destinationArray = new ulong[Math.Max(this.bits.Length << 1, this.NumWordsToHold(bit))];
      Array.Copy((Array) this.bits, 0, (Array) destinationArray, 0, this.bits.Length);
      this.bits = destinationArray;
    }

    public virtual void OrInPlace(BitSet a)
    {
      if (a == null)
        return;
      if (a.bits.Length > this.bits.Length)
        this.SetSize(a.bits.Length);
      for (int index = Math.Min(this.bits.Length, a.bits.Length) - 1; index >= 0; --index)
        this.bits[index] |= a.bits[index];
    }

    public virtual bool Nil
    {
      get
      {
        for (int index = this.bits.Length - 1; index >= 0; --index)
        {
          if (this.bits[index] != 0UL)
            return false;
        }
        return true;
      }
    }

    public virtual object Clone()
    {
      BitSet bitSet;
      try
      {
        bitSet = (BitSet) this.MemberwiseClone();
        bitSet.bits = new ulong[this.bits.Length];
        Array.Copy((Array) this.bits, 0, (Array) bitSet.bits, 0, this.bits.Length);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Unable to clone BitSet", ex);
      }
      return (object) bitSet;
    }

    public virtual int Count
    {
      get
      {
        int count = 0;
        for (int index1 = this.bits.Length - 1; index1 >= 0; --index1)
        {
          ulong bit = this.bits[index1];
          if (bit != 0UL)
          {
            for (int index2 = 63; index2 >= 0; --index2)
            {
              if (((long) bit & 1L << index2) != 0L)
                ++count;
            }
          }
        }
        return count;
      }
    }

    public virtual bool Member(int el)
    {
      if (el < 0)
        return false;
      int index = BitSet.WordNumber(el);
      return index < this.bits.Length && ((long) this.bits[index] & (long) BitSet.BitMask(el)) != 0L;
    }

    public virtual void Remove(int el)
    {
      int index = BitSet.WordNumber(el);
      if (index >= this.bits.Length)
        return;
      this.bits[index] &= ~BitSet.BitMask(el);
    }

    public virtual int NumBits() => this.bits.Length << 6;

    public virtual int LengthInLongWords() => this.bits.Length;

    public virtual int[] ToArray()
    {
      int[] array = new int[this.Count];
      int num = 0;
      for (int el = 0; el < this.bits.Length << 6; ++el)
      {
        if (this.Member(el))
          array[num++] = el;
      }
      return array;
    }

    public virtual ulong[] ToPackedArray() => this.bits;

    private static int WordNumber(int bit) => bit >> 6;

    public override string ToString() => this.ToString((string[]) null);

    public virtual string ToString(string[] tokenNames)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = ",";
      bool flag = false;
      stringBuilder.Append('{');
      for (int el = 0; el < this.bits.Length << 6; ++el)
      {
        if (this.Member(el))
        {
          if (el > 0 && flag)
            stringBuilder.Append(str);
          if (tokenNames != null)
            stringBuilder.Append(tokenNames[el]);
          else
            stringBuilder.Append(el);
          flag = true;
        }
      }
      stringBuilder.Append('}');
      return stringBuilder.ToString();
    }

    public override bool Equals(object other)
    {
      if (other == null || !(other is BitSet))
        return false;
      BitSet bitSet = (BitSet) other;
      int num = Math.Min(this.bits.Length, bitSet.bits.Length);
      for (int index = 0; index < num; ++index)
      {
        if ((long) this.bits[index] != (long) bitSet.bits[index])
          return false;
      }
      if (this.bits.Length > num)
      {
        for (int index = num + 1; index < this.bits.Length; ++index)
        {
          if (this.bits[index] != 0UL)
            return false;
        }
      }
      else if (bitSet.bits.Length > num)
      {
        for (int index = num + 1; index < bitSet.bits.Length; ++index)
        {
          if (bitSet.bits[index] != 0UL)
            return false;
        }
      }
      return true;
    }

    public override int GetHashCode() => base.GetHashCode();

    private static ulong BitMask(int bitNumber) => 1UL << (bitNumber & BitSet.MOD_MASK);

    private void SetSize(int nwords)
    {
      ulong[] destinationArray = new ulong[nwords];
      int length = Math.Min(nwords, this.bits.Length);
      Array.Copy((Array) this.bits, 0, (Array) destinationArray, 0, length);
      this.bits = destinationArray;
    }

    private int NumWordsToHold(int el) => (el >> 6) + 1;
  }
}
