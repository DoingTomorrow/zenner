// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.IrDAAddress
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace InTheHand.Net
{
  public sealed class IrDAAddress : IComparable, IFormattable
  {
    private const int AddressBytesLength = 4;
    private byte[] data;
    public static readonly IrDAAddress None = new IrDAAddress();

    internal IrDAAddress() => this.data = new byte[4];

    public IrDAAddress(byte[] address)
    {
      if (address == null)
        throw new ArgumentNullException(nameof (address));
      this.data = address.Length == 4 ? (byte[]) address.Clone() : throw new ArgumentException("Address bytes array must be four bytes in size.");
    }

    public IrDAAddress(int address)
    {
      this.data = new byte[4];
      BitConverter.GetBytes(address).CopyTo((Array) this.data, 0);
    }

    public int ToInt32() => BitConverter.ToInt32(this.data, 0);

    public byte[] ToByteArray() => (byte[]) this.data.Clone();

    public static bool TryParse(string s, out IrDAAddress result)
    {
      try
      {
        result = IrDAAddress.Parse(s);
        return true;
      }
      catch
      {
        result = (IrDAAddress) null;
        return false;
      }
    }

    public static IrDAAddress Parse(string irdaString)
    {
      if (irdaString == null)
        throw new ArgumentNullException(nameof (irdaString));
      IrDAAddress irDaAddress;
      if (irdaString.IndexOf(":", StringComparison.Ordinal) > -1)
      {
        if (irdaString.Length != 11)
          throw new FormatException("irdaString is not a valid IrDA address.");
        byte[] address = new byte[4];
        string[] strArray = irdaString.Split(':');
        for (int index = 0; index < address.Length; ++index)
          address[index] = byte.Parse(strArray[3 - index], NumberStyles.HexNumber);
        irDaAddress = new IrDAAddress(address);
      }
      else if (irdaString.IndexOf(".", StringComparison.Ordinal) > -1)
      {
        if (irdaString.Length != 11)
          throw new FormatException("irdaString is not a valid IrDA address.");
        byte[] address = new byte[4];
        string[] strArray = irdaString.Split('.');
        for (int index = 0; index < address.Length; ++index)
          address[index] = byte.Parse(strArray[3 - index], NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
        irDaAddress = new IrDAAddress(address);
      }
      else
        irDaAddress = !(irdaString.Length < 4 | irdaString.Length > 8) ? new IrDAAddress(int.Parse(irdaString, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture)) : throw new FormatException("irdaString is not a valid IrDA address.");
      return irDaAddress;
    }

    public override string ToString() => this.ToString("N");

    public string ToString(string format)
    {
      string str;
      if (format == null || format.Length == 0)
      {
        str = string.Empty;
      }
      else
      {
        switch (format.ToUpper())
        {
          case "N":
            str = string.Empty;
            break;
          case "C":
            str = ":";
            break;
          case "P":
            str = ".";
            break;
          default:
            throw new FormatException("Invalid format specified - must be either \"N\", \"C\", \"P\", \"\" or null.");
        }
      }
      StringBuilder stringBuilder = new StringBuilder(18);
      stringBuilder.Append(this.data[3].ToString("X2") + str);
      stringBuilder.Append(this.data[2].ToString("X2") + str);
      stringBuilder.Append(this.data[1].ToString("X2") + str);
      stringBuilder.Append(this.data[0].ToString("X2"));
      return stringBuilder.ToString();
    }

    public override bool Equals(object obj)
    {
      IrDAAddress irDaAddress = obj as IrDAAddress;
      return irDaAddress != (IrDAAddress) null ? this == irDaAddress : base.Equals(obj);
    }

    public override int GetHashCode() => this.ToInt32();

    public static bool operator ==(IrDAAddress x, IrDAAddress y)
    {
      return (object) x == null && (object) y == null || (object) x != null && (object) y != null && x.ToInt32() == y.ToInt32();
    }

    public static bool operator !=(IrDAAddress x, IrDAAddress y) => !(x == y);

    int IComparable.CompareTo(object obj)
    {
      IrDAAddress irDaAddress = obj as IrDAAddress;
      return irDaAddress != (IrDAAddress) null ? this.ToInt32().CompareTo(irDaAddress.ToInt32()) : -1;
    }

    public string ToString(string format, IFormatProvider formatProvider) => this.ToString(format);
  }
}
