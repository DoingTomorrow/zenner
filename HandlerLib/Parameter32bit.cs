// Decompiled with JetBrains decompiler
// Type: HandlerLib.Parameter32bit
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Runtime.InteropServices;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class Parameter32bit : IComparable<Parameter32bit>
  {
    public string Section { get; private set; }

    public string Name { get; private set; }

    public Type SystemType { get; private set; }

    public string Typ { get; private set; }

    public uint Address { get; private set; }

    public uint Size { get; private set; }

    public uint EndAddress => (uint) ((int) this.Address + (int) this.Size - 1);

    public AddressRange AddressRange => new AddressRange(this.Address, this.Size);

    public Parameter32bit(string section, string name, uint address, uint size, string typ = "")
    {
      this.Section = section;
      this.Name = name;
      this.Address = address;
      this.Size = size;
      this.Typ = typ;
    }

    public T GetValue<T>(DeviceMemory deviceMemory)
    {
      return (T) Parameter32bit.GetValue(typeof (T), this.Address, deviceMemory);
    }

    public static T GetValue<T>(uint address, DeviceMemory deviceMemory, bool toHEX = false)
    {
      return (T) Parameter32bit.GetValue(typeof (T), address, deviceMemory, toHEX);
    }

    public static object GetValue(Type type, uint address, DeviceMemory deviceMemory, bool toHEX = false)
    {
      if (!deviceMemory.AreDataAvailable(address, (uint) Marshal.SizeOf(type)))
        throw new Exception("No data available!");
      if (toHEX)
      {
        uint sizeOfType = Parameter32bit.GetSizeOfType(type);
        byte[] data = deviceMemory.GetData(address, sizeOfType);
        byte[] buffer = new byte[data.Length];
        for (int index = 0; index < data.Length; ++index)
          buffer[data.Length - 1 - index] = data[index];
        return (object) Utility.ByteArrayToHexString(buffer);
      }
      if (type == typeof (sbyte))
        return (object) Parameter32bit.GetValueSByte(address, deviceMemory);
      if (type == typeof (byte))
        return (object) Parameter32bit.GetValueByte(address, deviceMemory);
      if (type == typeof (short))
        return (object) Parameter32bit.GetValueShort(address, deviceMemory);
      if (type == typeof (ushort))
        return (object) Parameter32bit.GetValueUShort(address, deviceMemory);
      if (type == typeof (int))
        return (object) Parameter32bit.GetValueInt(address, deviceMemory);
      if (type == typeof (uint))
        return (object) Parameter32bit.GetValueUInt(address, deviceMemory);
      if (type == typeof (float))
        return (object) Parameter32bit.GetValueFloat(address, deviceMemory);
      if (type == typeof (double))
        return (object) Parameter32bit.GetValueDouble(address, deviceMemory);
      if (type == typeof (ulong))
        return (object) BitConverter.ToUInt64(deviceMemory.GetData(address, 8U), 0);
      if (type == typeof (long))
        return (object) BitConverter.ToInt64(deviceMemory.GetData(address, 8U), 0);
      if (type == typeof (byte[]))
        throw new Exception("use static method GetValueByteArray() instead !!!");
      throw new Exception("unsupported type");
    }

    public static void SetType(Type type, Parameter32bit param)
    {
      param.SystemType = type == typeof (sbyte) || type == typeof (byte) || type == typeof (short) || type == typeof (ushort) || type == typeof (int) || type == typeof (uint) || type == typeof (float) || type == typeof (double) || type == typeof (byte[]) || type == typeof (ushort) || type == typeof (short) || type == typeof (uint) || type == typeof (int) || type == typeof (ulong) || type == typeof (long) || type == typeof (short) || type == typeof (float) || type == typeof (double) || type == typeof (byte) || type == typeof (sbyte) ? type : throw new Exception("unsupported type");
      param.Typ = type.ToString();
    }

    public static byte GetValueByte(uint address, DeviceMemory deviceMemory)
    {
      return deviceMemory.GetData(address, 1U)[0];
    }

    public static sbyte GetValueSByte(uint address, DeviceMemory deviceMemory)
    {
      return (sbyte) deviceMemory.GetData(address, 1U)[0];
    }

    public static short GetValueShort(uint address, DeviceMemory deviceMemory)
    {
      return BitConverter.ToInt16(deviceMemory.GetData(address, 2U), 0);
    }

    public static ushort GetValueUShort(uint address, DeviceMemory deviceMemory)
    {
      return BitConverter.ToUInt16(deviceMemory.GetData(address, 2U), 0);
    }

    public static int GetValueInt(uint address, DeviceMemory deviceMemory)
    {
      return BitConverter.ToInt32(deviceMemory.GetData(address, 4U), 0);
    }

    public static uint GetValueUInt(uint address, DeviceMemory deviceMemory)
    {
      return BitConverter.ToUInt32(deviceMemory.GetData(address, 4U), 0);
    }

    public static float GetValueFloat(uint address, DeviceMemory deviceMemory)
    {
      return BitConverter.ToSingle(deviceMemory.GetData(address, 4U), 0);
    }

    public static double GetValueDouble(uint address, DeviceMemory deviceMemory)
    {
      return BitConverter.ToDouble(deviceMemory.GetData(address, 8U), 0);
    }

    public static byte[] GetValueByteArray(uint address, uint byteSize, DeviceMemory deviceMemory)
    {
      return deviceMemory.GetData(address, byteSize);
    }

    public void SetValue<T>(T theValue, DeviceMemory deviceMemory)
    {
      Parameter32bit.SetValue<T>(theValue, this.Address, deviceMemory);
    }

    public static void SetValue(Type T, object theValue, uint address, DeviceMemory deviceMemory)
    {
      if (T == typeof (byte))
        Parameter32bit.SetValue<byte>(Convert.ToByte(theValue), address, deviceMemory);
      else if (T == typeof (sbyte))
        Parameter32bit.SetValue<sbyte>(Convert.ToSByte(theValue), address, deviceMemory);
      else if (T == typeof (short))
        Parameter32bit.SetValue<short>(Convert.ToInt16(theValue), address, deviceMemory);
      else if (T == typeof (ushort))
        Parameter32bit.SetValue<ushort>(Convert.ToUInt16(theValue), address, deviceMemory);
      else if (T == typeof (int))
        Parameter32bit.SetValue<int>(Convert.ToInt32(theValue), address, deviceMemory);
      else if (T == typeof (uint))
        Parameter32bit.SetValue<uint>(Convert.ToUInt32(theValue), address, deviceMemory);
      else if (T == typeof (long))
        Parameter32bit.SetValue<long>(Convert.ToInt64(theValue), address, deviceMemory);
      else if (T == typeof (float))
      {
        Parameter32bit.SetValue<float>(Convert.ToSingle(theValue), address, deviceMemory);
      }
      else
      {
        if (!(T == typeof (double)))
          throw new Exception("unsupported type");
        Parameter32bit.SetValue<double>(Convert.ToDouble(theValue), address, deviceMemory);
      }
    }

    public static void SetValue<T>(T theValue, uint address, DeviceMemory deviceMemory)
    {
      if (typeof (T) == typeof (byte))
        Parameter32bit.SetValue((byte) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (sbyte))
        Parameter32bit.SetValue((sbyte) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (short))
        Parameter32bit.SetValue((short) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (ushort))
        Parameter32bit.SetValue((ushort) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (int))
        Parameter32bit.SetValue((int) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (uint))
        Parameter32bit.SetValue((uint) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (ulong))
        Parameter32bit.SetValue((ulong) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (long))
        Parameter32bit.SetValue((long) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (float))
        Parameter32bit.SetValue((float) (object) theValue, address, deviceMemory);
      else if (typeof (T) == typeof (double))
      {
        Parameter32bit.SetValue((double) (object) theValue, address, deviceMemory);
      }
      else
      {
        if (!(typeof (T) == typeof (byte[])))
          throw new Exception("unsupported type");
        Parameter32bit.SetValue((byte[]) (object) theValue, address, deviceMemory);
      }
    }

    public static void SetValue(byte theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, new byte[1]{ theValue });
    }

    public static void SetValue(sbyte theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, new byte[1]
      {
        (byte) theValue
      });
    }

    public static void SetValue(short theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, BitConverter.GetBytes(theValue));
    }

    public static void SetValue(ushort theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, BitConverter.GetBytes(theValue));
    }

    public static void SetValue(int theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, BitConverter.GetBytes(theValue));
    }

    public static void SetValue(uint theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, BitConverter.GetBytes(theValue));
    }

    public static void SetValue(ulong theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, BitConverter.GetBytes(theValue));
    }

    public static void SetValue(long theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, BitConverter.GetBytes(theValue));
    }

    public static void SetValue(float theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, BitConverter.GetBytes(theValue));
    }

    public static void SetValue(double theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, BitConverter.GetBytes(theValue));
    }

    public static void SetValue(byte[] theValue, uint address, DeviceMemory deviceMemory)
    {
      deviceMemory.SetData(address, theValue);
    }

    public static void CheckValueType(Type T, object theValue)
    {
      object obj;
      if (T == typeof (byte))
        obj = (object) Convert.ToByte(theValue);
      else if (T == typeof (sbyte))
        obj = (object) Convert.ToSByte(theValue);
      else if (T == typeof (short))
        obj = (object) Convert.ToInt16(theValue);
      else if (T == typeof (ushort))
        obj = (object) Convert.ToUInt16(theValue);
      else if (T == typeof (int))
        obj = (object) Convert.ToInt32(theValue);
      else if (T == typeof (uint))
        obj = (object) Convert.ToUInt32(theValue);
      else if (T == typeof (long))
        obj = (object) Convert.ToInt64(theValue);
      else if (T == typeof (float))
      {
        obj = (object) Convert.ToSingle(theValue);
      }
      else
      {
        if (!(T == typeof (double)))
          throw new Exception("unsupported type");
        obj = (object) Convert.ToDouble(theValue);
      }
    }

    public static uint GetSizeOfType(Type T)
    {
      return !(T == typeof (byte)) && !(T == typeof (byte)) && !(T == typeof (sbyte)) && !(T == typeof (sbyte)) && !(T == typeof (bool)) && !(T == typeof (bool)) ? (!(T == typeof (short)) && !(T == typeof (short)) && !(T == typeof (ushort)) && !(T == typeof (ushort)) && !(T == typeof (char)) && !(T == typeof (char)) ? (!(T == typeof (int)) && !(T == typeof (int)) && !(T == typeof (uint)) && !(T == typeof (uint)) && !(T == typeof (float)) && !(T == typeof (float)) ? (!(T == typeof (double)) && !(T == typeof (double)) && !(T == typeof (ulong)) && !(T == typeof (long)) ? (!(T == typeof (Decimal)) && !(T == typeof (Decimal)) ? 0U : 16U) : 8U) : 4U) : 2U) : 1U;
    }

    public override string ToString()
    {
      return string.Format("{0} 0x{1:X8}, {2} byte(s)", (object) this.Name, (object) this.Address, (object) this.Size);
    }

    public void AppandValueStrings(
      DeviceMemory deviceMemory,
      StringBuilder result,
      string leftSpaceString = "")
    {
      if (!deviceMemory.AreDataAvailable(this.Address, this.Size))
      {
        result.Append(" Data not available");
      }
      else
      {
        byte[] data = deviceMemory.GetData(this.AddressRange);
        if (this.SystemType != (Type) null)
        {
          uint sizeOfType = Parameter32bit.GetSizeOfType(this.SystemType);
          if (sizeOfType > 0U)
          {
            double num1 = (double) this.Size / (double) sizeOfType;
            uint num2 = (uint) num1;
            if ((double) num2 != num1)
            {
              result.Append("Byte array count don't fit to type array count");
            }
            else
            {
              if (num2 == 1U)
              {
                result.Append(" Value: ");
                result.Append(Parameter32bit.GetValue(this.SystemType, this.Address, deviceMemory));
                result.Append(" = 0x");
                result.Append(Parameter32bit.GetValue(this.SystemType, this.Address, deviceMemory, true));
                result.Append(" Data.Bytes: ");
                result.Append(Utility.ByteArrayToHexStringFormated(data, (string) null, 32));
                return;
              }
              result.Append(" Array.Count=" + num2.ToString());
              result.Append(" Array.Values: ");
              uint address = this.Address;
              for (int index = 0; (long) index < (long) num2; ++index)
              {
                result.AppendLine();
                result.Append(leftSpaceString);
                result.Append("[" + index.ToString("d03") + "] ");
                result.Append(Parameter32bit.GetValue(this.SystemType, address, deviceMemory));
                result.Append(" = 0x");
                result.Append(Parameter32bit.GetValue(this.SystemType, address, deviceMemory, true));
                address += sizeOfType;
              }
            }
          }
        }
        result.AppendLine();
        result.Append(leftSpaceString + "Data.Bytes: ");
        if (this.Size <= 32U)
        {
          result.Append(Utility.ByteArrayToHexStringFormated(data, (string) null, 32));
        }
        else
        {
          result.AppendLine();
          result.Append(Utility.ByteArrayToHexStringFormated(data, leftSpaceString, 32, new uint?(this.Address)));
        }
      }
    }

    public int CompareTo(Parameter32bit compareObject)
    {
      return this.Address.CompareTo(compareObject.Address);
    }
  }
}
