// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.BluetoothAddress
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace InTheHand.Net
{
  [Serializable]
  public sealed class BluetoothAddress : IComparable, IFormattable, IXmlSerializable, ISerializable
  {
    internal const int IacFirst = 10390272;
    internal const int IacLast = 10390335;
    public const int Liac = 10390272;
    public const int Giac = 10390323;
    [NonSerialized]
    private byte[] data;
    public static readonly BluetoothAddress None = new BluetoothAddress();

    internal BluetoothAddress() => this.data = new byte[8];

    public BluetoothAddress(long address)
      : this()
    {
      BitConverter.GetBytes(address).CopyTo((Array) this.data, 0);
    }

    public BluetoothAddress(byte[] address)
      : this()
    {
      if (address == null)
        throw new ArgumentNullException(nameof (address));
      if (address.Length != 6 && address.Length != 8)
        throw new ArgumentException("Address must be six bytes long.", nameof (address));
      System.Buffer.BlockCopy((Array) address, 0, (Array) this.data, 0, 6);
    }

    public static BluetoothAddress CreateFromBigEndian(byte[] address)
    {
      byte[] address1 = (byte[]) address.Clone();
      Array.Reverse((Array) address1);
      return new BluetoothAddress(address1);
    }

    public static BluetoothAddress CreateFromLittleEndian(byte[] address)
    {
      return new BluetoothAddress((byte[]) address.Clone());
    }

    public static bool TryParse(string bluetoothString, out BluetoothAddress result)
    {
      return BluetoothAddress.ParseInternal(bluetoothString, out result) == null;
    }

    public static BluetoothAddress Parse(string bluetoothString)
    {
      BluetoothAddress result;
      Exception exception = BluetoothAddress.ParseInternal(bluetoothString, out result);
      if (exception != null)
        throw exception;
      return result;
    }

    private static Exception ParseInternal(string bluetoothString, out BluetoothAddress result)
    {
      result = (BluetoothAddress) null;
      if (bluetoothString == null)
        return (Exception) new ArgumentNullException(nameof (bluetoothString));
      if (bluetoothString.IndexOf(":", StringComparison.Ordinal) > -1)
      {
        if (bluetoothString.Length != 17)
          return (Exception) new FormatException("bluetoothString is not a valid Bluetooth address.");
        try
        {
          byte[] address = new byte[8];
          string[] strArray = bluetoothString.Split(':');
          for (int index = 0; index < 6; ++index)
            address[index] = byte.Parse(strArray[5 - index], NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
          result = new BluetoothAddress(address);
          return (Exception) null;
        }
        catch (Exception ex)
        {
          return ex;
        }
      }
      else if (bluetoothString.IndexOf(".", StringComparison.Ordinal) > -1)
      {
        if (bluetoothString.Length != 17)
          return (Exception) new FormatException("bluetoothString is not a valid Bluetooth address.");
        try
        {
          byte[] address = new byte[8];
          string[] strArray = bluetoothString.Split('.');
          for (int index = 0; index < 6; ++index)
            address[index] = byte.Parse(strArray[5 - index], NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
          result = new BluetoothAddress(address);
          return (Exception) null;
        }
        catch (Exception ex)
        {
          return ex;
        }
      }
      else
      {
        if (bluetoothString.Length < 12 | bluetoothString.Length > 16)
          return (Exception) new FormatException("bluetoothString is not a valid Bluetooth address.");
        try
        {
          result = new BluetoothAddress(long.Parse(bluetoothString, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture));
          return (Exception) null;
        }
        catch (Exception ex)
        {
          return ex;
        }
      }
    }

    [CLSCompliant(false)]
    public uint Sap => BitConverter.ToUInt32(this.data, 0);

    [CLSCompliant(false)]
    public ushort Nap => BitConverter.ToUInt16(this.data, 4);

    public byte[] ToByteArray() => (byte[]) this.data.Clone();

    public byte[] ToByteArrayLittleEndian()
    {
      byte[] byteArray = this.ToByteArray();
      byte[] destinationArray = new byte[6];
      Array.Copy((Array) byteArray, (Array) destinationArray, destinationArray.Length);
      return destinationArray;
    }

    public byte[] ToByteArrayBigEndian()
    {
      byte[] arrayLittleEndian = this.ToByteArrayLittleEndian();
      Array.Reverse((Array) arrayLittleEndian);
      return arrayLittleEndian;
    }

    public long ToInt64() => BitConverter.ToInt64(this.data, 0);

    public override bool Equals(object obj)
    {
      BluetoothAddress bluetoothAddress = obj as BluetoothAddress;
      return bluetoothAddress != (BluetoothAddress) null ? this == bluetoothAddress : base.Equals(obj);
    }

    public override int GetHashCode() => this.ToInt64().GetHashCode();

    public static bool operator ==(BluetoothAddress x, BluetoothAddress y)
    {
      return (object) x == null && (object) y == null || (object) x != null && (object) y != null && x.ToInt64() == y.ToInt64();
    }

    public static bool operator !=(BluetoothAddress x, BluetoothAddress y) => !(x == y);

    public override string ToString() => this.ToString("N");

    public string ToString(string format)
    {
      string str;
      if (format == null || format == string.Empty)
      {
        str = string.Empty;
      }
      else
      {
        switch (format.ToUpper(CultureInfo.InvariantCulture))
        {
          case "8":
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
      if (format == "8")
      {
        stringBuilder.Append(this.data[7].ToString("X2") + str);
        stringBuilder.Append(this.data[6].ToString("X2") + str);
      }
      stringBuilder.Append(this.data[5].ToString("X2") + str);
      stringBuilder.Append(this.data[4].ToString("X2") + str);
      stringBuilder.Append(this.data[3].ToString("X2") + str);
      stringBuilder.Append(this.data[2].ToString("X2") + str);
      stringBuilder.Append(this.data[1].ToString("X2") + str);
      stringBuilder.Append(this.data[0].ToString("X2"));
      return stringBuilder.ToString();
    }

    int IComparable.CompareTo(object obj)
    {
      BluetoothAddress bluetoothAddress = obj as BluetoothAddress;
      return bluetoothAddress != (BluetoothAddress) null ? this.ToInt64().CompareTo(bluetoothAddress.ToInt64()) : -1;
    }

    public string ToString(string format, IFormatProvider formatProvider) => this.ToString(format);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      this.data = BluetoothAddress.Parse(reader.ReadElementContentAsString()).data;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer) => writer.WriteString(this.ToString("N"));

    [SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
    private BluetoothAddress(SerializationInfo info, StreamingContext context)
    {
      this.data = BluetoothAddress.Parse(info.GetString("dataString")).data;
    }

    [SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("dataString", (object) this.ToString("N"));
    }

    public object Clone()
    {
      return (object) new BluetoothAddress()
      {
        data = (byte[]) this.data.Clone()
      };
    }
  }
}
