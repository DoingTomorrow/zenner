// Decompiled with JetBrains decompiler
// Type: NLog.Internal.StringBuilderExt
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.MessageTemplates;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Internal
{
  internal static class StringBuilderExt
  {
    private static readonly char[] charToInt = new char[10]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9'
    };

    public static void AppendFormattedValue(
      this StringBuilder builder,
      object value,
      string format,
      IFormatProvider formatProvider)
    {
      if (value is string && string.IsNullOrEmpty(format))
        builder.Append(value);
      else if (format == "@")
      {
        ValueFormatter.Instance.FormatValue(value, (string) null, CaptureType.Serialize, formatProvider, builder);
      }
      else
      {
        if (value == null)
          return;
        ValueFormatter.Instance.FormatValue(value, format, CaptureType.Normal, formatProvider, builder);
      }
    }

    public static void AppendInvariant(this StringBuilder builder, int value)
    {
      if (value < 0)
      {
        builder.Append('-');
        uint num = (uint) (-1 - value + 1);
        builder.AppendInvariant(num);
      }
      else
        builder.AppendInvariant((uint) value);
    }

    public static void AppendInvariant(this StringBuilder builder, uint value)
    {
      if (value == 0U)
      {
        builder.Append('0');
      }
      else
      {
        int repeatCount = 0;
        uint num = value;
        do
        {
          num /= 10U;
          ++repeatCount;
        }
        while (num > 0U);
        builder.Append('0', repeatCount);
        int length = builder.Length;
        for (; repeatCount > 0; --repeatCount)
        {
          --length;
          builder[length] = StringBuilderExt.charToInt[(int) (value % 10U)];
          value /= 10U;
        }
      }
    }

    public static void ClearBuilder(this StringBuilder builder) => builder.Clear();

    public static void CopyToStream(
      this StringBuilder builder,
      MemoryStream ms,
      Encoding encoding,
      char[] transformBuffer)
    {
      if (transformBuffer != null)
      {
        int maxByteCount = encoding.GetMaxByteCount(builder.Length);
        ms.SetLength(ms.Position + (long) maxByteCount);
        for (int sourceIndex = 0; sourceIndex < builder.Length; sourceIndex += transformBuffer.Length)
        {
          int num = Math.Min(builder.Length - sourceIndex, transformBuffer.Length);
          builder.CopyTo(sourceIndex, transformBuffer, 0, num);
          int bytes = encoding.GetBytes(transformBuffer, 0, num, ms.GetBuffer(), (int) ms.Position);
          ms.Position += (long) bytes;
        }
        if (ms.Position == ms.Length)
          return;
        ms.SetLength(ms.Position);
      }
      else
      {
        string s = builder.ToString();
        byte[] bytes = encoding.GetBytes(s);
        ms.Write(bytes, 0, bytes.Length);
      }
    }

    public static void CopyTo(this StringBuilder builder, StringBuilder destination)
    {
      int length = builder.Length;
      if (length <= 0)
        return;
      destination.EnsureCapacity(length + destination.Length);
      if (length < 8)
      {
        for (int index = 0; index < length; ++index)
          destination.Append(builder[index]);
      }
      else if (length < 512)
      {
        destination.Append(builder.ToString());
      }
      else
      {
        char[] destination1 = new char[256];
        for (int sourceIndex = 0; sourceIndex < length; sourceIndex += destination1.Length)
        {
          int num = Math.Min(length - sourceIndex, destination1.Length);
          builder.CopyTo(sourceIndex, destination1, 0, num);
          destination.Append(destination1, 0, num);
        }
      }
    }

    internal static void Append2DigitsZeroPadded(this StringBuilder builder, int number)
    {
      builder.Append((char) (number / 10 + 48));
      builder.Append((char) (number % 10 + 48));
    }

    internal static void Append4DigitsZeroPadded(this StringBuilder builder, int number)
    {
      builder.Append((char) (number / 1000 % 10 + 48));
      builder.Append((char) (number / 100 % 10 + 48));
      builder.Append((char) (number / 10 % 10 + 48));
      builder.Append((char) (number / 1 % 10 + 48));
    }

    internal static void AppendIntegerAsString(
      this StringBuilder sb,
      object value,
      TypeCode objTypeCode)
    {
      switch (objTypeCode)
      {
        case TypeCode.SByte:
          sb.AppendInvariant((int) (sbyte) value);
          break;
        case TypeCode.Byte:
          sb.AppendInvariant((int) (byte) value);
          break;
        case TypeCode.Int16:
          sb.AppendInvariant((int) (short) value);
          break;
        case TypeCode.UInt16:
          sb.AppendInvariant((int) (ushort) value);
          break;
        case TypeCode.Int32:
          sb.AppendInvariant((int) value);
          break;
        case TypeCode.UInt32:
          sb.AppendInvariant((uint) value);
          break;
        case TypeCode.Int64:
          long num1 = (long) value;
          if (num1 < (long) int.MaxValue && num1 > (long) int.MinValue)
          {
            sb.AppendInvariant((int) num1);
            break;
          }
          sb.Append(num1);
          break;
        case TypeCode.UInt64:
          ulong num2 = (ulong) value;
          if (num2 < (ulong) uint.MaxValue)
          {
            sb.AppendInvariant((uint) num2);
            break;
          }
          sb.Append(num2);
          break;
        default:
          sb.Append(XmlHelper.XmlConvertToString(value, objTypeCode));
          break;
      }
    }
  }
}
