// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceRecordParser
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public class ServiceRecordParser
  {
    internal const int ElementTypeDescriptorOffset = 3;
    internal const int SizeIndexMask = 7;
    public const string ErrorMsgFormatUnknownType = "Unknown ElementType '{0}'.";
    public const string ErrorMsgFormatTypeNotTypeDSubtype = "ElementType '{1}' is not of given TypeDescriptor '{0}'.";
    public const string ErrorMsgSizeIndexNotSuitTypeD = "SizeIndex is not value for TypeDescriptor.";
    public const string ErrorMsgServiceRecordBytesZeroLength = "ServiceRecord byte array must be at least one byte long.";
    public const string ErrorMsgFormatInvalidHeaderBytes = "Invalid header bytes at index {0}.";
    public const string ErrorMsgFormatTruncated = "Header truncated from index {0}.";
    public const string ErrorMsgFormatNotSupportFull32bitSized = "No support for full sized 32bit length values (index {0}).";
    public const string ErrorMsgTypeNotAsExpected = "Element Type not as expected.";
    public const string ErrorMsgTopElementNotSequence = "The top element must be a Element Sequence type.";
    public const string ErrorMsgMultiSeqChildElementNotSequence = "In a multi-record sequence each element must be an Element Sequence.";
    public const string ErrorMsgSequenceOverruns = "Element Sequence overruns the data, from index {0}.";
    public const string ErrorMsgElementOverrunsBuffer_WithLengths = "Element overruns buffer section, from index {0}, item length is {1} but remaining length is only {2}.";
    public const string ErrorMsgElementOverrunsBuffer = "Element overruns buffer section, from index {0}.";
    public const string ErrorMsgElementOverrunsBufferPrefix = "Element overruns buffer section, from index ";
    public const string ErrorMsgAttributePairFirstMustUint16 = "The Attribute Id at index {0} is not of type Uint16.";
    private bool m_allowAnySizeForUnknownTypeDescriptorElements = true;

    public bool SkipUnhandledElementTypes { get; set; }

    public bool LazyUrlCreation { get; set; }

    public ServiceRecord Parse(byte[] buffer)
    {
      return buffer != null ? this.Parse(buffer, 0, buffer.Length) : throw new ArgumentNullException(nameof (buffer));
    }

    public ServiceRecord Parse(byte[] buffer, int offset, int length)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (length <= 0)
        throw ServiceRecordParser.new_ArgumentOutOfRangeException(nameof (buffer), "ServiceRecord byte array must be at least one byte long.");
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (buffer.Length - offset < length)
        throw new ArgumentException("offset is past the end of the data.");
      if (ServiceRecordParser.GetElementTypeDescriptor(buffer[offset]) != ElementTypeDescriptor.ElementSequence)
        throw ServiceRecordParser.CreateInvalidException("The top element must be a Element Sequence type.", offset);
      int elementLength = ServiceRecordParser.GetElementLength(buffer, offset, length, out int _, out int _);
      if (elementLength > length)
        throw ServiceRecordParser.CreateInvalidExceptionOverruns(offset, elementLength, length);
      List<ServiceAttribute> serviceAttributeList = new List<ServiceAttribute>();
      ServiceRecordParser.SequenceItemParser<ServiceAttribute> itemParser = new ServiceRecordParser.SequenceItemParser<ServiceAttribute>(this.ParseAttributeElementPair);
      ServiceRecordParser.ParseSeqOrAlt<ServiceAttribute>(buffer, offset, elementLength, (IList<ServiceAttribute>) serviceAttributeList, itemParser);
      ServiceRecord serviceRecord = new ServiceRecord((IList<ServiceAttribute>) serviceAttributeList);
      if (offset == 0)
        serviceRecord.SetSourceBytes(buffer);
      return serviceRecord;
    }

    private ServiceAttribute ParseAttributeElementPair(
      byte[] buffer,
      int offset,
      int length,
      out int readLength)
    {
      int contentLength;
      ServiceRecordParser.GetElementLength(buffer, offset, length, out int _, out contentLength);
      if (ServiceRecordParser.GetElementTypeDescriptor(buffer[offset]) != ElementTypeDescriptor.UnsignedInteger || contentLength != 2)
        throw ServiceRecordParser.CreateInvalidException("The Attribute Id at index {0} is not of type Uint16.", offset);
      readLength = 0;
      int readLength1;
      ushort id = ServiceRecordParser.ReadElementUInt16(buffer, offset, length, out readLength1);
      readLength += readLength1;
      offset += readLength1;
      length -= readLength1;
      ServiceElement serviceElement = this.ParseInternal(buffer, offset, length, out readLength1);
      readLength += readLength1;
      return new ServiceAttribute(id, serviceElement);
    }

    private ServiceElement ParseInternal(
      byte[] buffer,
      int offset,
      int length,
      out int readLength)
    {
      ElementTypeDescriptor etd;
      SizeIndex sizeIndex;
      ServiceRecordParser.SplitHeaderByte(buffer[offset], out etd, out sizeIndex);
      ServiceRecordParser.VerifyAllowedSizeIndex(etd, sizeIndex, this.m_allowAnySizeForUnknownTypeDescriptorElements);
      int contentOffset;
      int contentLength;
      readLength = ServiceRecordParser.GetElementLength(buffer, offset, length, out contentOffset, out contentLength);
      if (readLength > length)
        throw ServiceRecordParser.CreateInvalidExceptionOverruns(offset, readLength, length);
      object obj = (object) null;
      ElementType type = ElementType.Unknown;
      switch (etd)
      {
        case ElementTypeDescriptor.Nil:
          type = ElementType.Nil;
          obj = (object) null;
          break;
        case ElementTypeDescriptor.UnsignedInteger:
        case ElementTypeDescriptor.TwosComplementInteger:
          switch (contentLength)
          {
            case 1:
              byte num1 = ServiceRecordParser.ReadElementUInt8(buffer, offset, length, out readLength);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt8;
                obj = (object) num1;
                break;
              }
              type = ElementType.Int8;
              obj = (object) (sbyte) num1;
              break;
            case 2:
              ushort num2 = ServiceRecordParser.ReadElementUInt16(buffer, offset, length, out readLength);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt16;
                obj = (object) num2;
                break;
              }
              type = ElementType.Int16;
              obj = (object) (short) num2;
              break;
            case 4:
              uint num3 = ServiceRecordParser.ReadElementUInt32(buffer, offset, length, out readLength);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt32;
                obj = (object) num3;
                break;
              }
              type = ElementType.Int32;
              obj = (object) (int) num3;
              break;
            case 8:
              ulong num4 = ServiceRecordParser.ReadElementUInt64(buffer, offset, length, out readLength);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt64;
                obj = (object) num4;
                break;
              }
              type = ElementType.Int64;
              obj = (object) (long) num4;
              break;
            case 16:
              byte[] numArray1 = ServiceRecordParser.ReadArrayContent(buffer, offset, length, out readLength);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt128;
                obj = (object) numArray1;
                break;
              }
              type = ElementType.Int128;
              obj = (object) numArray1;
              break;
          }
          break;
        case ElementTypeDescriptor.Uuid:
          switch (contentLength)
          {
            case 2:
              ushort num5 = ServiceRecordParser.ReadFieldUInt16(buffer, offset + contentOffset, length - contentOffset);
              type = ElementType.Uuid16;
              obj = (object) num5;
              break;
            case 4:
              uint num6 = ServiceRecordParser.ReadFieldUInt32(buffer, offset + contentOffset, length - contentOffset);
              type = ElementType.Uuid32;
              obj = (object) num6;
              break;
            case 16:
              offset += contentOffset;
              Guid guid = new Guid((int) ServiceRecordParser.ReadFieldUInt32(buffer, offset, contentLength), (short) ServiceRecordParser.ReadFieldUInt16(buffer, offset + 4, contentLength - 4), (short) ServiceRecordParser.ReadFieldUInt16(buffer, offset + 6, contentLength - 6), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 8, contentLength - 8), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 9, contentLength - 9), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 10, contentLength - 10), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 11, contentLength - 11), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 12, contentLength - 12), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 13, contentLength - 13), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 14, contentLength - 14), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 15, contentLength - 15));
              type = ElementType.Uuid128;
              obj = (object) guid;
              break;
          }
          break;
        case ElementTypeDescriptor.TextString:
          byte[] numArray2 = ServiceRecordParser.ReadArrayContent(buffer, offset, length, out int _);
          type = ElementType.TextString;
          obj = (object) numArray2;
          break;
        case ElementTypeDescriptor.Boolean:
          obj = (object) (ServiceRecordParser.ReadFieldUInt8(buffer, offset + contentOffset, length - contentOffset) != (byte) 0);
          type = ElementType.Boolean;
          break;
        case ElementTypeDescriptor.ElementSequence:
        case ElementTypeDescriptor.ElementAlternative:
          List<ServiceElement> children = new List<ServiceElement>();
          ServiceRecordParser.SequenceItemParser<ServiceElement> itemParser = new ServiceRecordParser.SequenceItemParser<ServiceElement>(this.ParseInternal);
          ServiceRecordParser.ParseSeqOrAlt<ServiceElement>(buffer, offset, readLength, (IList<ServiceElement>) children, itemParser);
          type = etd == ElementTypeDescriptor.ElementSequence ? ElementType.ElementSequence : ElementType.ElementAlternative;
          obj = (object) children;
          break;
        case ElementTypeDescriptor.Url:
          byte[] valueArray = ServiceRecordParser.ReadArrayContent(buffer, offset, length, out int _);
          obj = !this.LazyUrlCreation ? (object) ServiceRecordParser.CreateUriStringFromBytes(valueArray) : (object) valueArray;
          type = ElementType.Url;
          break;
      }
      if (type == ElementType.Unknown)
      {
        string message = "Element type: " + (object) etd + ", SizeIndex: " + (object) sizeIndex + ", at offset: " + (object) offset + ".";
        if (!this.SkipUnhandledElementTypes)
          throw ServiceRecordParser.new_NotImplementedException(message);
        type = ElementType.Unknown;
        etd = ElementTypeDescriptor.Unknown;
        obj = (object) null;
        MiscUtils.Trace_WriteLine("Unhandled SDP Parse " + message);
      }
      return new ServiceElement(etd, type, obj);
    }

    private static void ParseSeqOrAlt<T>(
      byte[] buffer,
      int offset,
      int length,
      IList<T> children,
      ServiceRecordParser.SequenceItemParser<T> itemParser)
    {
      int elementTypeDescriptor = (int) ServiceRecordParser.GetElementTypeDescriptor(buffer[offset]);
      int contentOffset;
      int contentLength;
      ServiceRecordParser.GetElementLength(buffer, offset, length, out contentOffset, out contentLength);
      int readLength;
      for (int offset1 = offset + contentOffset; offset1 < offset + length && contentLength > 0; contentLength -= readLength)
      {
        T obj = itemParser(buffer, offset1, contentLength, out readLength);
        children.Add(obj);
        offset1 += readLength;
      }
    }

    public static byte[][] SplitSearchAttributeResult(byte[] multiRecord)
    {
      if (multiRecord == null)
        throw new ArgumentNullException(nameof (multiRecord));
      if (multiRecord.Length == 0)
        return new byte[0][];
      byte[] numArray = multiRecord;
      int index1 = 0;
      int length1 = multiRecord.Length;
      if (ServiceRecordParser.GetElementTypeDescriptor(numArray[index1]) != ElementTypeDescriptor.ElementSequence)
        throw ServiceRecordParser.CreateInvalidException("The top element must be a Element Sequence type.", index1);
      int contentOffset1;
      int contentLength1;
      int elementLength1 = ServiceRecordParser.GetElementLength(numArray, index1, length1, out contentOffset1, out contentLength1);
      if (elementLength1 > length1)
        throw ServiceRecordParser.CreateInvalidExceptionOverruns(index1, elementLength1, length1);
      List<byte[]> numArrayList = new List<byte[]>();
      int index2 = index1 + contentOffset1;
      int length2;
      for (int length3 = contentLength1; length3 > 0; length3 -= length2)
      {
        if (ServiceRecordParser.GetElementTypeDescriptor(numArray[index2]) != ElementTypeDescriptor.ElementSequence)
          throw ServiceRecordParser.CreateInvalidException("In a multi-record sequence each element must be an Element Sequence.", index2);
        int contentOffset2;
        int contentLength2;
        int elementLength2 = ServiceRecordParser.GetElementLength(numArray, index2, length3, out contentOffset2, out contentLength2);
        if (elementLength2 > length3)
          throw ServiceRecordParser.CreateInvalidExceptionOverruns(index2, elementLength2, length3);
        length2 = contentOffset2 + contentLength2;
        byte[] destinationArray = new byte[length2];
        Array.Copy((Array) numArray, index2, (Array) destinationArray, 0, length2);
        numArrayList.Add(destinationArray);
        index2 += length2;
      }
      return numArrayList.ToArray();
    }

    private static byte ReadElementUInt8(
      byte[] buffer,
      int offset,
      int length,
      out int readLength)
    {
      int contentOffset;
      readLength = ServiceRecordParser.GetElementLength(buffer, offset, length, out contentOffset, out int _);
      int elementTypeDescriptor = (int) ServiceRecordParser.GetElementTypeDescriptor(buffer[offset]);
      return ServiceRecordParser.ReadFieldUInt8(buffer, offset + contentOffset, length - contentOffset);
    }

    private static ushort ReadElementUInt16(
      byte[] buffer,
      int offset,
      int length,
      out int readLength)
    {
      int contentOffset;
      readLength = ServiceRecordParser.GetElementLength(buffer, offset, length, out contentOffset, out int _);
      int elementTypeDescriptor = (int) ServiceRecordParser.GetElementTypeDescriptor(buffer[offset]);
      return ServiceRecordParser.ReadFieldUInt16(buffer, offset + contentOffset, length - contentOffset);
    }

    private static uint ReadElementUInt32(
      byte[] buffer,
      int offset,
      int length,
      out int readLength)
    {
      int contentOffset;
      readLength = ServiceRecordParser.GetElementLength(buffer, offset, length, out contentOffset, out int _);
      int elementTypeDescriptor = (int) ServiceRecordParser.GetElementTypeDescriptor(buffer[offset]);
      return ServiceRecordParser.ReadFieldUInt32(buffer, offset + contentOffset, length - contentOffset);
    }

    private static ulong ReadElementUInt64(
      byte[] buffer,
      int offset,
      int length,
      out int readLength)
    {
      int contentOffset;
      readLength = ServiceRecordParser.GetElementLength(buffer, offset, length, out contentOffset, out int _);
      int elementTypeDescriptor = (int) ServiceRecordParser.GetElementTypeDescriptor(buffer[offset]);
      return ServiceRecordParser.ReadFieldUInt64(buffer, offset + contentOffset, length - contentOffset);
    }

    private static byte[] ReadArrayContent(
      byte[] buffer,
      int offset,
      int length,
      out int readLength)
    {
      int contentOffset;
      int contentLength;
      readLength = ServiceRecordParser.GetElementLength(buffer, offset, length, out contentOffset, out contentLength);
      byte[] destinationArray = new byte[contentLength];
      Array.Copy((Array) buffer, offset + contentOffset, (Array) destinationArray, 0, contentLength);
      return destinationArray;
    }

    internal ServiceElement ParseContent(
      bool networkOrderInteger,
      bool networkOrderUuid,
      byte[] buffer,
      int offset,
      int length,
      ref int _readLength,
      ElementTypeDescriptor etd,
      SizeIndex dbgSizeIndex,
      int contentLength,
      int contentOffset)
    {
      object obj = (object) null;
      ElementType type = ElementType.Unknown;
      switch (etd)
      {
        case ElementTypeDescriptor.Nil:
          type = ElementType.Nil;
          obj = (object) null;
          break;
        case ElementTypeDescriptor.UnsignedInteger:
        case ElementTypeDescriptor.TwosComplementInteger:
          switch (contentLength)
          {
            case 1:
              byte num1 = ServiceRecordParser.ReadFieldUInt8_Content(networkOrderInteger, buffer, offset, length);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt8;
                obj = (object) num1;
                break;
              }
              type = ElementType.Int8;
              obj = (object) (sbyte) num1;
              break;
            case 2:
              ushort num2 = ServiceRecordParser.ReadFieldUInt16_Content(networkOrderInteger, buffer, offset, length);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt16;
                obj = (object) num2;
                break;
              }
              type = ElementType.Int16;
              obj = (object) (short) num2;
              break;
            case 4:
              uint num3 = ServiceRecordParser.ReadFieldUInt32_Content(networkOrderInteger, buffer, offset, length);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt32;
                obj = (object) num3;
                break;
              }
              type = ElementType.Int32;
              obj = (object) (int) num3;
              break;
            case 8:
              ulong num4 = ServiceRecordParser.ReadFieldUInt64_Content(networkOrderInteger, buffer, offset, length);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt64;
                obj = (object) num4;
                break;
              }
              type = ElementType.Int64;
              obj = (object) (long) num4;
              break;
            case 16:
              byte[] numArray1 = ServiceRecordParser.ReadArray_Content(buffer, offset, length);
              if (!networkOrderInteger)
                Array.Reverse((Array) numArray1);
              if (etd == ElementTypeDescriptor.UnsignedInteger)
              {
                type = ElementType.UInt128;
                obj = (object) numArray1;
                break;
              }
              type = ElementType.Int128;
              obj = (object) numArray1;
              break;
          }
          break;
        case ElementTypeDescriptor.Uuid:
          switch (contentLength)
          {
            case 2:
              ushort num5 = ServiceRecordParser.ReadFieldUInt16_Content(networkOrderUuid, buffer, offset + contentOffset, length - contentOffset);
              type = ElementType.Uuid16;
              obj = (object) num5;
              break;
            case 4:
              uint num6 = ServiceRecordParser.ReadFieldUInt32_Content(networkOrderUuid, buffer, offset + contentOffset, length - contentOffset);
              type = ElementType.Uuid32;
              obj = (object) num6;
              break;
            case 16:
              offset += contentOffset;
              Guid guid = new Guid((int) ServiceRecordParser.ReadFieldUInt32(buffer, offset, contentLength), (short) ServiceRecordParser.ReadFieldUInt16(buffer, offset + 4, contentLength - 4), (short) ServiceRecordParser.ReadFieldUInt16(buffer, offset + 6, contentLength - 6), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 8, contentLength - 8), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 9, contentLength - 9), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 10, contentLength - 10), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 11, contentLength - 11), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 12, contentLength - 12), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 13, contentLength - 13), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 14, contentLength - 14), ServiceRecordParser.ReadFieldUInt8(buffer, offset + 15, contentLength - 15));
              type = ElementType.Uuid128;
              obj = (object) guid;
              break;
          }
          break;
        case ElementTypeDescriptor.TextString:
          byte[] numArray2 = ServiceRecordParser.ReadArray_Content(buffer, offset, length);
          type = ElementType.TextString;
          obj = (object) numArray2;
          break;
        case ElementTypeDescriptor.Boolean:
          obj = (object) (ServiceRecordParser.ReadFieldUInt8(buffer, offset + contentOffset, length - contentOffset) != (byte) 0);
          type = ElementType.Boolean;
          break;
        case ElementTypeDescriptor.ElementSequence:
        case ElementTypeDescriptor.ElementAlternative:
          throw new InvalidOperationException("INTERNAL ERROR: Not supported here!!");
        case ElementTypeDescriptor.Url:
          byte[] valueArray = ServiceRecordParser.ReadArray_Content(buffer, offset, length);
          obj = !this.LazyUrlCreation ? (object) ServiceRecordParser.CreateUriStringFromBytes(valueArray) : (object) valueArray;
          type = ElementType.Url;
          break;
      }
      if (type == ElementType.Unknown)
      {
        string message = "Element type: " + (object) etd + ", SizeIndex: " + (object) dbgSizeIndex + ", at contentLength: " + (object) contentLength + ", at offset: " + (object) offset + ".";
        if (!this.SkipUnhandledElementTypes)
          throw ServiceRecordParser.new_NotImplementedException(message);
        type = ElementType.Unknown;
        etd = ElementTypeDescriptor.Unknown;
        obj = (object) null;
        MiscUtils.Trace_WriteLine("Unhandled SDP Parse " + message);
      }
      return new ServiceElement(etd, type, obj);
    }

    private static byte ReadFieldUInt8_Content(
      bool networkOrder,
      byte[] buffer,
      int offset,
      int length)
    {
      return ServiceRecordParser.ReadFieldUInt8(buffer, offset, length);
    }

    private static ushort ReadFieldUInt16_Content(
      bool networkOrder,
      byte[] buffer,
      int offset,
      int length)
    {
      return ServiceRecordParser.ReadFieldUInt16(networkOrder, buffer, offset, length);
    }

    private static uint ReadFieldUInt32_Content(
      bool networkOrder,
      byte[] buffer,
      int offset,
      int length)
    {
      return ServiceRecordParser.ReadFieldUInt32(networkOrder, buffer, offset, length);
    }

    private static ulong ReadFieldUInt64_Content(
      bool networkOrder,
      byte[] buffer,
      int offset,
      int length)
    {
      return ServiceRecordParser.ReadFieldUInt64(networkOrder, buffer, offset, length);
    }

    private static byte[] ReadArray_Content(byte[] buffer, int offset, int length)
    {
      int num = 0;
      int length1 = length;
      byte[] destinationArray = new byte[length1];
      Array.Copy((Array) buffer, offset + num, (Array) destinationArray, 0, length1);
      return destinationArray;
    }

    protected static int GetElementLength(
      byte[] buffer,
      int index,
      int length,
      out int contentOffset,
      out int contentLength)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (length <= 0)
        throw new ArgumentOutOfRangeException(nameof (length));
      if (index < 0 || index == int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (length > buffer.Length - index)
        throw new ArgumentException("length and index overruns buffer.");
      ElementTypeDescriptor etd;
      SizeIndex sizeIndex;
      ServiceRecordParser.SplitHeaderByte(buffer[index], out etd, out sizeIndex);
      if (etd == ElementTypeDescriptor.Nil)
      {
        if (sizeIndex != SizeIndex.LengthOneByteOrNil)
          throw ServiceRecordParser.CreateInvalidException("SizeIndex is not value for TypeDescriptor.", index);
        contentOffset = 1;
        contentLength = 0;
        return 1;
      }
      switch (sizeIndex)
      {
        case SizeIndex.LengthOneByteOrNil:
          contentOffset = 1;
          return ServiceRecordParser.FixupLength(1U, contentOffset, out contentLength, index);
        case SizeIndex.LengthTwoBytes:
          contentOffset = 1;
          return ServiceRecordParser.FixupLength(2U, contentOffset, out contentLength, index);
        case SizeIndex.LengthFourBytes:
          contentOffset = 1;
          return ServiceRecordParser.FixupLength(4U, contentOffset, out contentLength, index);
        case SizeIndex.LengthEightBytes:
          contentOffset = 1;
          return ServiceRecordParser.FixupLength(8U, contentOffset, out contentLength, index);
        case SizeIndex.LengthSixteenBytes:
          contentOffset = 1;
          return ServiceRecordParser.FixupLength(16U, contentOffset, out contentLength, index);
        case SizeIndex.AdditionalUInt8:
          ServiceRecordParser.CheckParseLength(index, length, 2);
          contentOffset = 2;
          return ServiceRecordParser.FixupLength((uint) ServiceRecordParser.ReadFieldUInt8(buffer, index + 1, length - 1), contentOffset, out contentLength, index);
        case SizeIndex.AdditionalUInt16:
          ServiceRecordParser.CheckParseLength(index, length, 3);
          contentOffset = 3;
          return ServiceRecordParser.FixupLength((uint) ServiceRecordParser.ReadFieldUInt16(buffer, index + 1, length - 1), contentOffset, out contentLength, index);
        default:
          ServiceRecordParser.CheckParseLength(index, length, 5);
          contentOffset = 5;
          return ServiceRecordParser.FixupLength(ServiceRecordParser.ReadFieldUInt32(buffer, index + 1, length - 1), contentOffset, out contentLength, index);
      }
    }

    private static int FixupLength(
      uint contentLength,
      int contentOffsetAlsoHeaderBytesLength,
      out int outContentLength,
      int index)
    {
      long num1 = (long) contentLength + (long) contentOffsetAlsoHeaderBytesLength;
      int num2 = num1 <= (long) int.MaxValue ? checked ((int) num1) : throw new ProtocolViolationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "No support for full sized 32bit length values (index {0}).", (object) index));
      outContentLength = num2 - contentOffsetAlsoHeaderBytesLength;
      return num2;
    }

    private static void CheckParseLength(int index, int length, int requiredLength)
    {
      if (requiredLength > length)
        throw ServiceRecordParser.CreateInvalidException("Header truncated from index {0}.", index);
    }

    private static byte ReadFieldUInt8(byte[] bytes, int index, int length)
    {
      ServiceRecordParser.CheckParseLength(index, length, 1);
      return bytes[index];
    }

    private static ushort ReadFieldUInt16(byte[] bytes, int index, int length)
    {
      return ServiceRecordParser.ReadFieldUInt16(true, bytes, index, length);
    }

    private static ushort ReadFieldUInt16(bool networkOrder, byte[] bytes, int index, int length)
    {
      ServiceRecordParser.CheckParseLength(index, length, 2);
      short int16 = BitConverter.ToInt16(bytes, index);
      return !networkOrder ? (ushort) int16 : (ushort) IPAddress.NetworkToHostOrder(int16);
    }

    private static uint ReadFieldUInt32(byte[] bytes, int index, int length)
    {
      return ServiceRecordParser.ReadFieldUInt32(true, bytes, index, length);
    }

    private static uint ReadFieldUInt32(bool networkOrder, byte[] bytes, int index, int length)
    {
      ServiceRecordParser.CheckParseLength(index, length, 4);
      int int32 = BitConverter.ToInt32(bytes, index);
      return !networkOrder ? (uint) int32 : (uint) IPAddress.NetworkToHostOrder(int32);
    }

    private static ulong ReadFieldUInt64(byte[] bytes, int index, int length)
    {
      return ServiceRecordParser.ReadFieldUInt64(true, bytes, index, length);
    }

    private static ulong ReadFieldUInt64(bool networkOrder, byte[] bytes, int index, int length)
    {
      ServiceRecordParser.CheckParseLength(index, length, 8);
      long int64 = BitConverter.ToInt64(bytes, index);
      return !networkOrder ? (ulong) int64 : (ulong) IPAddress.NetworkToHostOrder(int64);
    }

    internal static string CreateUriStringFromBytes(byte[] valueArray)
    {
      return Encoding.ASCII.GetString(valueArray, 0, valueArray.Length);
    }

    private static Exception CreateInvalidException(string formatMessage, int index)
    {
      return (Exception) new ProtocolViolationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, formatMessage, (object) index));
    }

    private static Exception CreateInvalidExceptionOverruns(
      int index,
      int elementLength,
      int length)
    {
      Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Element overruns buffer section, from index {0}, item length is {1} but remaining length is only {2}.", (object) index, (object) elementLength, (object) length));
      return (Exception) new ProtocolViolationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Element overruns buffer section, from index {0}.", (object) index, (object) elementLength, (object) length));
    }

    public static void SplitHeaderByte(
      byte headerByte,
      out ElementTypeDescriptor etd,
      out SizeIndex sizeIndex)
    {
      etd = ServiceRecordParser.GetElementTypeDescriptor(headerByte);
      sizeIndex = ServiceRecordParser.GetSizeIndex(headerByte);
    }

    [DebuggerStepThrough]
    public static ElementTypeDescriptor GetElementTypeDescriptor(byte headerByte)
    {
      return (ElementTypeDescriptor) ((uint) headerByte >> 3);
    }

    [DebuggerStepThrough]
    public static SizeIndex GetSizeIndex(byte headerByte) => (SizeIndex) ((int) headerByte & 7);

    internal static void VerifyTypeMatchesEtd(ElementTypeDescriptor etd, ElementType type)
    {
      if (!ServiceRecordParser.TypeMatchesEtd(etd, type))
        throw new ProtocolViolationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "ElementType '{1}' is not of given TypeDescriptor '{0}'.", (object) etd, (object) type));
    }

    public static bool TypeMatchesEtd(ElementTypeDescriptor etd, ElementType type)
    {
      bool flag = false;
      switch (etd)
      {
        case ElementTypeDescriptor.Unknown:
          flag = type == ElementType.Unknown;
          break;
        case ElementTypeDescriptor.Nil:
          flag = type == ElementType.Nil;
          break;
        case ElementTypeDescriptor.UnsignedInteger:
          flag = type == ElementType.UInt8 || type == ElementType.UInt16 || type == ElementType.UInt32 || type == ElementType.UInt64 || type == ElementType.UInt128;
          break;
        case ElementTypeDescriptor.TwosComplementInteger:
          flag = type == ElementType.Int8 || type == ElementType.Int16 || type == ElementType.Int32 || type == ElementType.Int64 || type == ElementType.Int128;
          break;
        case ElementTypeDescriptor.Uuid:
          flag = type == ElementType.Uuid16 || type == ElementType.Uuid32 || type == ElementType.Uuid128;
          break;
        case ElementTypeDescriptor.TextString:
          flag = type == ElementType.TextString;
          break;
        case ElementTypeDescriptor.Boolean:
          flag = type == ElementType.Boolean;
          break;
        case ElementTypeDescriptor.ElementSequence:
          flag = type == ElementType.ElementSequence;
          break;
        case ElementTypeDescriptor.ElementAlternative:
          flag = type == ElementType.ElementAlternative;
          break;
        case ElementTypeDescriptor.Url:
          flag = type == ElementType.Url;
          break;
      }
      return flag;
    }

    public static ElementTypeDescriptor GetEtdForType(ElementType type)
    {
      switch (type)
      {
        case ElementType.Nil:
          return ElementTypeDescriptor.Nil;
        case ElementType.UInt8:
        case ElementType.UInt16:
        case ElementType.UInt32:
        case ElementType.UInt64:
          return ElementTypeDescriptor.UnsignedInteger;
        case ElementType.Int8:
        case ElementType.Int16:
        case ElementType.Int32:
        case ElementType.Int64:
          return ElementTypeDescriptor.TwosComplementInteger;
        case ElementType.Uuid16:
        case ElementType.Uuid32:
        case ElementType.Uuid128:
          return ElementTypeDescriptor.Uuid;
        case ElementType.TextString:
          return ElementTypeDescriptor.TextString;
        case ElementType.Boolean:
          return ElementTypeDescriptor.Boolean;
        case ElementType.ElementSequence:
          return ElementTypeDescriptor.ElementSequence;
        case ElementType.ElementAlternative:
          return ElementTypeDescriptor.ElementAlternative;
        case ElementType.Url:
          return ElementTypeDescriptor.Url;
        default:
          throw ServiceRecordParser.new_ArgumentOutOfRangeException(nameof (type), string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Unknown ElementType '{0}'.", (object) type));
      }
    }

    internal static void VerifyAllowedSizeIndex(
      ElementTypeDescriptor etd,
      SizeIndex sizeIndex,
      bool allowAnySizeIndexForUnknownTypeDescriptorElements)
    {
      if (!ServiceRecordParser.IsAllowedSizeIndex(etd, sizeIndex, allowAnySizeIndexForUnknownTypeDescriptorElements))
        throw new ProtocolViolationException("SizeIndex is not value for TypeDescriptor.");
    }

    private static bool IsAllowedSizeIndex(
      ElementTypeDescriptor etd,
      SizeIndex sizeIndex,
      bool allowAnySizeIndexForUnknownTypeDescriptorElements)
    {
      bool flag;
      switch (etd)
      {
        case ElementTypeDescriptor.Nil:
        case ElementTypeDescriptor.Boolean:
          flag = sizeIndex == SizeIndex.LengthOneByteOrNil;
          break;
        case ElementTypeDescriptor.UnsignedInteger:
        case ElementTypeDescriptor.TwosComplementInteger:
          flag = sizeIndex == SizeIndex.LengthOneByteOrNil || sizeIndex == SizeIndex.LengthTwoBytes || sizeIndex == SizeIndex.LengthFourBytes || sizeIndex == SizeIndex.LengthEightBytes || sizeIndex == SizeIndex.LengthSixteenBytes;
          break;
        case ElementTypeDescriptor.Uuid:
          flag = sizeIndex == SizeIndex.LengthTwoBytes || sizeIndex == SizeIndex.LengthFourBytes || sizeIndex == SizeIndex.LengthSixteenBytes;
          break;
        case ElementTypeDescriptor.TextString:
        case ElementTypeDescriptor.ElementSequence:
        case ElementTypeDescriptor.ElementAlternative:
        case ElementTypeDescriptor.Url:
          flag = sizeIndex == SizeIndex.AdditionalUInt8 || sizeIndex == SizeIndex.AdditionalUInt16 || sizeIndex == SizeIndex.AdditionalUInt32;
          break;
        default:
          flag = allowAnySizeIndexForUnknownTypeDescriptorElements;
          break;
      }
      return flag;
    }

    internal static Exception new_NotImplementedException(string message)
    {
      return (Exception) new NotImplementedException(message);
    }

    internal static Exception new_ArgumentOutOfRangeException(string paramName, string message)
    {
      return (Exception) new ArgumentOutOfRangeException(paramName, message);
    }

    internal static Exception new_ArgumentOutOfRangeException(
      string message,
      Exception innerException)
    {
      return (Exception) new ArgumentOutOfRangeException(message, innerException);
    }

    private delegate T SequenceItemParser<T>(
      byte[] buffer,
      int offset,
      int length,
      out int readLength);
  }
}
