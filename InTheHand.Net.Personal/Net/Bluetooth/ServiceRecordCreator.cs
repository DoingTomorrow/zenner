// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceRecordCreator
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public class ServiceRecordCreator
  {
    public const string ErrorMsgSupportOnlyLength255 = "Only ServiceRecords shorter that 256 bytes are supported currently.";

    public int CreateServiceRecord(ServiceRecord record, byte[] buffer)
    {
      if (record == null)
        throw new ArgumentNullException(nameof (record));
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      int offset1 = 0;
      ServiceRecordCreator.HeaderWriteState headerState;
      int num = this.MakeVariableLengthHeader(buffer, offset1, ElementTypeDescriptor.ElementSequence, out headerState);
      int offset2 = offset1 + num;
      foreach (ServiceAttribute attr in record)
        this.WriteAttribute(attr, buffer, ref offset2);
      this.CompleteHeaderWrite(headerState, buffer, offset2, out int _);
      return offset2;
    }

    protected virtual void WriteAttribute(ServiceAttribute attr, byte[] buffer, ref int offset)
    {
      int attrId = this.CreateAttrId(attr.Id, buffer, offset);
      offset += attrId;
      int element = this.CreateElement(attr.Value, buffer, offset);
      offset += element;
    }

    public byte[] CreateServiceRecord(ServiceRecord record)
    {
      byte[] numArray = new byte[256];
      int serviceRecord = this.CreateServiceRecord(record, numArray);
      byte[] destinationArray = new byte[serviceRecord];
      Array.Copy((Array) numArray, 0, (Array) destinationArray, 0, serviceRecord);
      return destinationArray;
    }

    private static void VerifyWriteSpaceRemaining(int requiredLength, byte[] buffer, int offset)
    {
      int num = buffer.Length - offset;
      if (requiredLength > num)
        throw ExceptionFactory.ArgumentOutOfRangeException(nameof (buffer), "The record bytes are longer that the supplied byte array buffer.");
    }

    protected virtual int CreateAttrId(ServiceAttributeId attrId, byte[] buf, int offset)
    {
      return this.CreateElement(new ServiceElement(ElementType.UInt16, (object) (ushort) attrId), buf, offset);
    }

    protected virtual int CreateElement(ServiceElement element, byte[] buf, int offset)
    {
      int totalLength;
      if (element.ElementTypeDescriptor == ElementTypeDescriptor.ElementSequence || element.ElementTypeDescriptor == ElementTypeDescriptor.ElementAlternative)
      {
        ServiceRecordCreator.HeaderWriteState headerState;
        int num = this.MakeVariableLengthHeader(buf, offset, element.ElementTypeDescriptor, out headerState);
        offset += num;
        foreach (ServiceElement valueAsElement in (IEnumerable<ServiceElement>) element.GetValueAsElementList())
        {
          int element1 = this.CreateElement(valueAsElement, buf, offset);
          offset += element1;
        }
        this.CompleteHeaderWrite(headerState, buf, offset, out totalLength);
      }
      else if (element.ElementTypeDescriptor == ElementTypeDescriptor.UnsignedInteger || element.ElementTypeDescriptor == ElementTypeDescriptor.TwosComplementInteger)
      {
        switch (element.ElementType)
        {
          case ElementType.UInt8:
            this.WriteByte(element, (byte) element.Value, buf, ref offset, out totalLength);
            break;
          case ElementType.UInt16:
            this.WriteUInt16(element, (ushort) element.Value, buf, ref offset, out totalLength);
            break;
          case ElementType.UInt32:
            this.WriteUInt32(element, (uint) element.Value, buf, ref offset, out totalLength);
            break;
          case ElementType.UInt64:
            this.WriteUInt64(element, (ulong) element.Value, buf, ref offset, out totalLength);
            break;
          case ElementType.Int8:
            this.WriteSByte(element, (sbyte) element.Value, buf, ref offset, out totalLength);
            break;
          case ElementType.Int16:
            this.WriteInt16(element, (short) element.Value, buf, ref offset, out totalLength);
            break;
          case ElementType.Int32:
            this.WriteInt32(element, (int) element.Value, buf, ref offset, out totalLength);
            break;
          case ElementType.Int64:
            this.WriteInt64(element, (long) element.Value, buf, ref offset, out totalLength);
            break;
          default:
            totalLength = 0;
            break;
        }
      }
      else if (element.ElementTypeDescriptor == ElementTypeDescriptor.Uuid)
      {
        if (element.ElementType == ElementType.Uuid16)
          this.WriteUInt16(element, (ushort) element.Value, buf, ref offset, out totalLength);
        else if (element.ElementType == ElementType.Uuid32)
        {
          this.WriteUInt32(element, (uint) element.Value, buf, ref offset, out totalLength);
        }
        else
        {
          byte[] byteArray = BluetoothListener.HostToNetworkOrder((Guid) element.Value).ToByteArray();
          this.WriteFixedLength(element, byteArray, buf, ref offset, out totalLength);
        }
      }
      else if (element.ElementTypeDescriptor == ElementTypeDescriptor.Url)
      {
        byte[] bytes = Encoding.ASCII.GetBytes(element.GetValueAsUri().ToString());
        this.WriteVariableLength(element, bytes, buf, ref offset, out totalLength);
      }
      else if (element.ElementTypeDescriptor == ElementTypeDescriptor.TextString)
      {
        byte[] valueBytes = !(element.Value is string s) ? (byte[]) element.Value : Encoding.UTF8.GetBytes(s);
        this.WriteVariableLength(element, valueBytes, buf, ref offset, out totalLength);
      }
      else if (element.ElementTypeDescriptor == ElementTypeDescriptor.Nil)
        this.WriteFixedLength(element, new byte[0], buf, ref offset, out totalLength);
      else if (element.ElementTypeDescriptor == ElementTypeDescriptor.Boolean)
      {
        byte[] valueBytes = new byte[1]
        {
          (bool) element.Value ? (byte) 1 : (byte) 0
        };
        this.WriteFixedLength(element, valueBytes, buf, ref offset, out totalLength);
      }
      else
        totalLength = 0;
      return totalLength != 0 ? totalLength : throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Creation of element type '{0}' not implemented.", (object) element.ElementType));
    }

    protected virtual void WriteVariableLength(
      ServiceElement element,
      byte[] valueBytes,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      ServiceRecordCreator.HeaderWriteState headerState;
      int num = this.MakeVariableLengthHeader(buf, offset, element.ElementTypeDescriptor, out headerState);
      offset += num;
      ServiceRecordCreator.VerifyWriteSpaceRemaining(valueBytes.Length, buf, offset);
      valueBytes.CopyTo((Array) buf, offset);
      offset += valueBytes.Length;
      this.CompleteHeaderWrite(headerState, buf, offset, out totalLength);
    }

    protected virtual void WriteFixedLength(
      ServiceElement element,
      byte[] valueBytes,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      int num = this.WriteHeaderFixedLength(element.ElementTypeDescriptor, valueBytes.Length, buf, offset, out totalLength);
      offset += num;
      ServiceRecordCreator.VerifyWriteSpaceRemaining(valueBytes.Length, buf, offset);
      valueBytes.CopyTo((Array) buf, offset);
    }

    private void WriteUInt16(
      ServiceElement element,
      ushort value,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      short num = (short) value;
      this.WriteInt16(element, num, buf, ref offset, out totalLength);
    }

    private void WriteInt16(
      ServiceElement element,
      short value,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
      this.WriteFixedLength(element, bytes, buf, ref offset, out totalLength);
    }

    private void WriteByte(
      ServiceElement element,
      byte value,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      byte[] valueBytes = new byte[1]{ value };
      this.WriteFixedLength(element, valueBytes, buf, ref offset, out totalLength);
    }

    private void WriteSByte(
      ServiceElement element,
      sbyte value,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      byte num = (byte) value;
      this.WriteByte(element, num, buf, ref offset, out totalLength);
    }

    private void WriteUInt32(
      ServiceElement element,
      uint value,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      int num = (int) value;
      this.WriteInt32(element, num, buf, ref offset, out totalLength);
    }

    private void WriteInt32(
      ServiceElement element,
      int value,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
      this.WriteFixedLength(element, bytes, buf, ref offset, out totalLength);
    }

    private void WriteUInt64(
      ServiceElement element,
      ulong value,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      long num = (long) value;
      this.WriteInt64(element, num, buf, ref offset, out totalLength);
    }

    private void WriteInt64(
      ServiceElement element,
      long value,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
      this.WriteFixedLength(element, bytes, buf, ref offset, out totalLength);
    }

    private static SizeIndex FixedLengthToSizeIndex(int contentLength)
    {
      switch (contentLength)
      {
        case 0:
          return SizeIndex.LengthOneByteOrNil;
        case 1:
          return SizeIndex.LengthOneByteOrNil;
        case 2:
          return SizeIndex.LengthTwoBytes;
        case 4:
          return SizeIndex.LengthFourBytes;
        case 8:
          return SizeIndex.LengthEightBytes;
        default:
          return SizeIndex.LengthSixteenBytes;
      }
    }

    private int WriteHeaderFixedLength(
      ElementTypeDescriptor elementTypeDescriptor,
      int contentLength,
      byte[] buf,
      int offset,
      out int totalLength)
    {
      SizeIndex sizeIndex = ServiceRecordCreator.FixedLengthToSizeIndex(contentLength);
      return this.WriteHeaderFixedLength_(elementTypeDescriptor, contentLength, sizeIndex, buf, offset, out totalLength);
    }

    private int WriteHeaderFixedLength_(
      ElementTypeDescriptor elementTypeDescriptor,
      int contentLength,
      SizeIndex sizeIndex,
      byte[] buf,
      int offset,
      out int totalLength)
    {
      ServiceRecordParser.VerifyAllowedSizeIndex(elementTypeDescriptor, sizeIndex, false);
      ServiceRecordCreator.HeaderWriteState headerState = new ServiceRecordCreator.HeaderWriteState(elementTypeDescriptor, buf, offset, sizeIndex, 1);
      this.CompleteHeaderWrite(headerState, buf, offset + contentLength + headerState.HeaderLength, out totalLength);
      return headerState.HeaderLength;
    }

    protected virtual int MakeVariableLengthHeader(
      byte[] buf,
      int offset,
      ElementTypeDescriptor elementTypeDescriptor,
      out ServiceRecordCreator.HeaderWriteState headerState)
    {
      this.HackFxCopHintNonStaticMethod();
      headerState = new ServiceRecordCreator.HeaderWriteState(elementTypeDescriptor, buf, offset, SizeIndex.AdditionalUInt8, 2);
      return headerState.HeaderLength;
    }

    protected virtual void CompleteHeaderWrite(
      ServiceRecordCreator.HeaderWriteState headerState,
      byte[] buf,
      int offsetAtEndOfWritten,
      out int totalLength)
    {
      this.HackFxCopHintNonStaticMethod();
      byte headerByte = ServiceRecordCreator.CreateHeaderByte(headerState.Etd, headerState.SizeIndex);
      buf[headerState.HeaderOffset] = headerByte;
      if (headerState.SizeIndex == SizeIndex.LengthOneByteOrNil || headerState.SizeIndex == SizeIndex.LengthTwoBytes || headerState.SizeIndex == SizeIndex.LengthFourBytes || headerState.SizeIndex == SizeIndex.LengthEightBytes || headerState.SizeIndex == SizeIndex.LengthSixteenBytes)
      {
        totalLength = offsetAtEndOfWritten - headerState.HeaderOffset;
      }
      else
      {
        int num = offsetAtEndOfWritten - headerState.HeaderOffset - headerState.HeaderLength;
        if (headerState.SizeIndex == SizeIndex.AdditionalUInt8)
          buf[headerState.HeaderOffset + 1] = num <= (int) byte.MaxValue ? checked ((byte) num) : throw new NotSupportedException("Only ServiceRecords shorter that 256 bytes are supported currently.");
        totalLength = offsetAtEndOfWritten - headerState.HeaderOffset;
      }
    }

    private static byte CreateHeaderByte(ElementTypeDescriptor etd, SizeIndex sizeIndex)
    {
      return (byte) ((uint) (byte) ((uint) etd << 3) | (uint) (byte) sizeIndex);
    }

    private void HackFxCopHintNonStaticMethod()
    {
    }

    protected sealed class HeaderWriteState
    {
      public readonly int HeaderOffset;
      public readonly ElementTypeDescriptor Etd;
      public readonly SizeIndex SizeIndex;
      public readonly int HeaderLength;
      internal bool widcommNeedsStoring;

      internal HeaderWriteState(
        ElementTypeDescriptor elementTypeDescriptor,
        byte[] buf,
        int offset,
        SizeIndex sizeIndex,
        int headerLength)
      {
        this.Etd = elementTypeDescriptor;
        this.HeaderOffset = offset;
        this.SizeIndex = sizeIndex;
        this.HeaderLength = headerLength;
        ServiceRecordCreator.VerifyWriteSpaceRemaining(this.HeaderLength, buf, offset);
        ServiceRecordParser.VerifyAllowedSizeIndex(this.Etd, this.SizeIndex, false);
      }
    }
  }
}
