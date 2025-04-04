// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceRecordUtilities
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.AttributeIds;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public static class ServiceRecordUtilities
  {
    private static char[] s_lotsOfSpaces = new string(' ', 100).ToCharArray();

    public static string DumpRaw(ServiceRecord record)
    {
      using (StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        ServiceRecordUtilities.DumpRaw((TextWriter) writer, record);
        writer.Close();
        return writer.ToString();
      }
    }

    public static void DumpRaw(TextWriter writer, ServiceRecord record)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (record == null)
        throw new ArgumentNullException(nameof (record));
      bool flag = true;
      foreach (ServiceAttribute serviceAttribute in record)
      {
        if (!flag)
          writer.WriteLine();
        writer.WriteLine("AttrId: 0x{0:X4}", (object) (ushort) serviceAttribute.Id);
        ServiceRecordUtilities.DumpRawElement(writer, 0, serviceAttribute.Value);
        flag = false;
      }
    }

    private static void DumpRawElement(TextWriter writer, int depth, ServiceElement elem)
    {
      ServiceRecordUtilities.WritePrefix(writer, depth);
      if (elem.ElementType == ElementType.ElementSequence || elem.ElementType == ElementType.ElementAlternative)
      {
        writer.WriteLine("{0}", (object) elem.ElementType);
        foreach (ServiceElement valueAsElement in (IEnumerable<ServiceElement>) elem.GetValueAsElementList())
          ServiceRecordUtilities.DumpRawElement(writer, depth + 1, valueAsElement);
      }
      else if (elem.ElementType == ElementType.Nil)
        writer.WriteLine("Nil:");
      else if (elem.ElementType == ElementType.TextString || elem.ElementType == ElementType.Boolean || elem.ElementType == ElementType.Url)
        writer.WriteLine("{0}: {1}", (object) elem.ElementType, elem.Value);
      else if (elem.ElementType == ElementType.Uuid128)
        writer.WriteLine("{0}: {1}", (object) elem.ElementType, elem.Value);
      else if (elem.ElementType == ElementType.UInt128 || elem.ElementType == ElementType.Int128)
      {
        string str = BitConverter.ToString((byte[]) elem.Value);
        writer.WriteLine("{0}: {1}", (object) elem.ElementType, (object) str);
      }
      else
        writer.WriteLine("{0}: 0x{1:X}", (object) elem.ElementType, elem.Value);
    }

    public static string Dump(ServiceRecord record, params Type[] attributeIdEnumDefiningTypes)
    {
      using (StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        ServiceRecordUtilities.Dump((TextWriter) writer, record, attributeIdEnumDefiningTypes);
        writer.Close();
        return writer.ToString();
      }
    }

    public static void Dump(
      TextWriter writer,
      ServiceRecord record,
      params Type[] attributeIdEnumDefiningTypes)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      Type[] recordSpecificAttributeIdEnumDefiningTypes = record != null ? ServiceRecordUtilities.GetServiceClassSpecificAttributeIdEnumDefiningType(record) : throw new ArgumentNullException(nameof (record));
      Type[] attributeIdDefiningClasses = ServiceRecordUtilities.CombineAttributeIdEnumDefiningTypes(attributeIdEnumDefiningTypes, recordSpecificAttributeIdEnumDefiningTypes);
      LanguageBaseItem[] languageBaseList = record.GetLanguageBaseList();
      bool flag = true;
      foreach (ServiceAttribute serviceAttribute in record)
      {
        if (!flag)
          writer.WriteLine();
        ServiceAttributeId id = serviceAttribute.Id;
        LanguageBaseItem applicableLangBase;
        string name = AttributeIdLookup.GetName(id, attributeIdDefiningClasses, languageBaseList, out applicableLangBase);
        if (name == null)
          writer.WriteLine("AttrId: 0x{0:X4}", (object) (ushort) serviceAttribute.Id);
        else
          writer.WriteLine("AttrId: 0x{0:X4} -- {1}", (object) (ushort) serviceAttribute.Id, (object) name);
        if (serviceAttribute.Value.ElementType == ElementType.TextString)
          ServiceRecordUtilities.DumpString(writer, 0, serviceAttribute.Value, applicableLangBase);
        else
          ServiceRecordUtilities.DumpElement(writer, 0, serviceAttribute.Value);
        if (id == (ServiceAttributeId) 4)
          ServiceRecordUtilities.DumpProtocolDescriptorList(writer, 0, serviceAttribute.Value);
        if (id == (ServiceAttributeId) 13)
          ServiceRecordUtilities.DumpAdditionalProtocolDescriptorLists(writer, 0, serviceAttribute.Value);
        flag = false;
      }
    }

    private static Type[] CombineAttributeIdEnumDefiningTypes(
      Type[] attributeIdEnumDefiningTypes,
      Type[] recordSpecificAttributeIdEnumDefiningTypes)
    {
      if (attributeIdEnumDefiningTypes == null)
        attributeIdEnumDefiningTypes = new Type[0];
      Type[] typeArray = new Type[1 + attributeIdEnumDefiningTypes.Length + recordSpecificAttributeIdEnumDefiningTypes.Length];
      typeArray[0] = typeof (UniversalAttributeId);
      attributeIdEnumDefiningTypes.CopyTo((Array) typeArray, 1);
      recordSpecificAttributeIdEnumDefiningTypes.CopyTo((Array) typeArray, 1 + attributeIdEnumDefiningTypes.Length);
      return typeArray;
    }

    private static void DumpAdditionalProtocolDescriptorLists(
      TextWriter writer,
      int depth,
      ServiceElement element)
    {
      foreach (ServiceElement valueAsElement in (IEnumerable<ServiceElement>) element.GetValueAsElementList())
        ServiceRecordUtilities.DumpProtocolDescriptorList(writer, depth + 1, valueAsElement);
    }

    private static void DumpProtocolDescriptorList(
      TextWriter writer,
      int depth,
      ServiceElement element)
    {
      if (element.ElementType == ElementType.ElementAlternative)
      {
        foreach (ServiceElement valueAsElement in (IEnumerable<ServiceElement>) element.GetValueAsElementList())
          ServiceRecordUtilities.DumpProtocolDescriptorListList(writer, depth + 1, valueAsElement);
      }
      else
        ServiceRecordUtilities.DumpProtocolDescriptorListList(writer, depth, element);
    }

    private static void DumpProtocolDescriptorListList(
      TextWriter writer,
      int depth,
      ServiceElement element)
    {
      ServiceRecordUtilities.WritePrefix(writer, depth);
      writer.Write("( ");
      bool flag = true;
      foreach (ServiceElement valueAsElement in (IEnumerable<ServiceElement>) element.GetValueAsElementList())
      {
        ServiceElement[] valueAsElementArray = valueAsElement.GetValueAsElementArray();
        int index1 = 0;
        string protoStr;
        ServiceRecordUtilities.HackProtocolId hackProtocolId = ServiceRecordUtilities.GuidToHackProtocolId(valueAsElementArray[index1].GetValueAsUuid(), out protoStr);
        int index2 = index1 + 1;
        writer.Write("{0}( {1}", flag ? (object) string.Empty : (object) ", ", (object) protoStr);
        switch (hackProtocolId)
        {
          case ServiceRecordUtilities.HackProtocolId.Rfcomm:
            if (index2 < valueAsElementArray.Length)
            {
              byte num = (byte) valueAsElementArray[index2].Value;
              ++index2;
              writer.Write(", ChannelNumber={0}", (object) num);
              break;
            }
            break;
          case ServiceRecordUtilities.HackProtocolId.L2Cap:
            if (index2 < valueAsElementArray.Length)
            {
              ServiceRecordUtilities.HackProtocolServiceMultiplexer serviceMultiplexer = (ServiceRecordUtilities.HackProtocolServiceMultiplexer) (ushort) valueAsElementArray[index2].Value;
              ++index2;
              writer.Write(", PSM={0}", (object) ServiceRecordUtilities.Enum_ToStringNameOrHex((Enum) serviceMultiplexer));
              break;
            }
            break;
        }
        if (index2 < valueAsElementArray.Length)
          writer.Write(", ...");
        writer.Write(" )");
        flag = false;
      }
      writer.WriteLine(" )");
    }

    private static string Enum_ToStringNameOrHex(Enum value)
    {
      string text = value.ToString();
      if (ServiceRecordUtilities.HackIsNumeric(text))
      {
        Type underlyingType = Enum.GetUnderlyingType(value.GetType());
        text = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "0x{0:X}", ((IConvertible) value).ToType(underlyingType, (IFormatProvider) null));
      }
      return text;
    }

    private static bool HackIsNumeric(string text) => char.IsDigit(text[0]) || text[0] == '-';

    private static ServiceRecordUtilities.HackProtocolId GuidToHackProtocolId(
      Guid protocolGuid,
      out string protoStr)
    {
      ServiceRecordUtilities.HackProtocolId? nullable1 = new ServiceRecordUtilities.HackProtocolId?();
      if (protocolGuid == BluetoothService.BnepProtocol)
        nullable1 = new ServiceRecordUtilities.HackProtocolId?(ServiceRecordUtilities.HackProtocolId.Bnep);
      else if (protocolGuid == BluetoothService.L2CapProtocol)
        nullable1 = new ServiceRecordUtilities.HackProtocolId?(ServiceRecordUtilities.HackProtocolId.L2Cap);
      else if (protocolGuid == BluetoothService.ObexProtocol)
        nullable1 = new ServiceRecordUtilities.HackProtocolId?(ServiceRecordUtilities.HackProtocolId.Obex);
      else if (protocolGuid == BluetoothService.RFCommProtocol)
        nullable1 = new ServiceRecordUtilities.HackProtocolId?(ServiceRecordUtilities.HackProtocolId.Rfcomm);
      else if (protocolGuid == BluetoothService.SdpProtocol)
        nullable1 = new ServiceRecordUtilities.HackProtocolId?(ServiceRecordUtilities.HackProtocolId.Sdp);
      if (nullable1.HasValue)
      {
        protoStr = nullable1.ToString();
        return nullable1.Value;
      }
      nullable1 = !ServiceRecordUtilities.IsUuid16Value(protocolGuid) ? new ServiceRecordUtilities.HackProtocolId?((ServiceRecordUtilities.HackProtocolId) 0) : new ServiceRecordUtilities.HackProtocolId?(ServiceRecordUtilities.GetAsUuid16Value(protocolGuid));
      if (Enum.IsDefined(typeof (ServiceRecordUtilities.HackProtocolId), (object) nullable1))
      {
        protoStr = ServiceRecordUtilities.Enum_ToStringNameOrHex((Enum) (ValueType) nullable1);
      }
      else
      {
        string name = BluetoothService.GetName(protocolGuid);
        if (name != null && name.EndsWith("Protocol", StringComparison.Ordinal))
        {
          protoStr = name.Remove(name.Length - "Protocol".Length, "Protocol".Length);
        }
        else
        {
          ServiceRecordUtilities.HackProtocolId? nullable2 = nullable1;
          int num = nullable2.GetValueOrDefault() != (ServiceRecordUtilities.HackProtocolId) 0 ? 1 : (!nullable2.HasValue ? 1 : 0);
          protoStr = num == 0 ? protocolGuid.ToString() : ServiceRecordUtilities.Enum_ToStringNameOrHex((Enum) (ValueType) nullable1);
        }
      }
      return nullable1.Value;
    }

    internal static ushort GetAsUuid16Value_(Guid protocolGuid)
    {
      return ServiceRecordUtilities.IsUuid16Value(protocolGuid) ? checked ((ushort) BitConverter.ToInt32(protocolGuid.ToByteArray(), 0)) : throw new ArgumentException("Guid is not a Bluetooth UUID.");
    }

    internal static ServiceRecordUtilities.HackProtocolId GetAsUuid16Value(Guid protocolGuid)
    {
      return (ServiceRecordUtilities.HackProtocolId) ServiceRecordUtilities.GetAsUuid16Value_(protocolGuid);
    }

    internal static bool IsUuid16Value(Guid protocolGuid)
    {
      byte[] byteArray = protocolGuid.ToByteArray();
      byteArray[0] = byteArray[1] = (byte) 0;
      return new Guid(byteArray).Equals(BluetoothService.BluetoothBase);
    }

    internal static bool IsUuid32Value(Guid protocolGuid)
    {
      byte[] byteArray = protocolGuid.ToByteArray();
      byteArray[0] = byteArray[1] = byteArray[2] = byteArray[3] = (byte) 0;
      return new Guid(byteArray).Equals(BluetoothService.BluetoothBase);
    }

    internal static uint GetAsUuid32Value(Guid protocolGuid)
    {
      return ServiceRecordUtilities.IsUuid32Value(protocolGuid) ? BitConverter.ToUInt32(protocolGuid.ToByteArray(), 0) : throw new ArgumentException("Guid is not a Bluetooth UUID.");
    }

    private static void DumpString(
      TextWriter writer,
      int depth,
      ServiceElement element,
      LanguageBaseItem langBase)
    {
      if (langBase != null)
      {
        try
        {
          string valueAsString = element.GetValueAsString(langBase);
          writer.WriteLine("{0}: [{1}] '{2}'", (object) element.ElementType, (object) langBase.NaturalLanguage, (object) valueAsString);
        }
        catch (NotSupportedException ex)
        {
          writer.WriteLine("{0}: Failure: {1}", (object) element.ElementType, (object) ex.Message);
        }
      }
      else
      {
        try
        {
          string valueAsStringUtf8 = element.GetValueAsStringUtf8();
          if (valueAsStringUtf8.IndexOf(char.MinValue) != -1)
            throw new DecoderFallbackException("EEEEE contains nulls!  UTF-16?!");
          writer.WriteLine("{0} (guessing UTF-8): '{1}'", (object) element.ElementType, (object) valueAsStringUtf8);
        }
        catch (DecoderFallbackException ex)
        {
          writer.WriteLine("{0} (Unknown/bad encoding):", (object) element.ElementType);
          byte[] numArray = (byte[]) element.Value;
          string str = BitConverter.ToString(numArray);
          ServiceRecordUtilities.WritePrefix(writer, depth);
          writer.WriteLine("Length: {0}, >>{1}<<", (object) numArray.Length, (object) str);
        }
      }
    }

    private static void DumpElement(TextWriter writer, int depth, ServiceElement elem)
    {
      ServiceRecordUtilities.WritePrefix(writer, depth);
      if (elem.ElementType == ElementType.ElementSequence || elem.ElementType == ElementType.ElementAlternative)
      {
        writer.WriteLine("{0}", (object) elem.ElementType);
        foreach (ServiceElement valueAsElement in (IEnumerable<ServiceElement>) elem.GetValueAsElementList())
          ServiceRecordUtilities.DumpElement(writer, depth + 1, valueAsElement);
      }
      else if (elem.ElementType == ElementType.Nil)
        writer.WriteLine("Nil:");
      else if (elem.ElementType == ElementType.TextString)
        ServiceRecordUtilities.DumpString(writer, depth, elem, (LanguageBaseItem) null);
      else if (elem.ElementType == ElementType.Boolean || elem.ElementType == ElementType.Url)
      {
        writer.WriteLine("{0}: {1}", (object) elem.ElementType, elem.Value);
      }
      else
      {
        string str1 = (string) null;
        string str2 = (string) null;
        if (elem.ElementTypeDescriptor == ElementTypeDescriptor.Uuid)
        {
          if (elem.ElementType == ElementType.Uuid16)
            str1 = BluetoothService.GetName((ushort) elem.Value);
          else if (elem.ElementType == ElementType.Uuid32)
          {
            str1 = BluetoothService.GetName((uint) elem.Value);
          }
          else
          {
            str1 = BluetoothService.GetName((Guid) elem.Value);
            str2 = ((Guid) elem.Value).ToString();
          }
        }
        if (str2 == null)
        {
          if (elem.ElementTypeDescriptor == ElementTypeDescriptor.Unknown)
            str2 = "unknown";
          else if (elem.ElementType == ElementType.UInt128 || elem.ElementType == ElementType.Int128)
            str2 = BitConverter.ToString((byte[]) elem.Value);
          else
            str2 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "0x{0:X}", elem.Value);
        }
        if (str1 == null)
          writer.WriteLine("{0}: {1}", (object) elem.ElementType, (object) str2);
        else
          writer.WriteLine("{0}: {1} -- {2}", (object) elem.ElementType, (object) str2, (object) str1);
      }
    }

    private static Type[] GetServiceClassSpecificAttributeIdEnumDefiningType(ServiceRecord record)
    {
      return new MapServiceClassToAttributeIdList().GetAttributeIdEnumTypes(record);
    }

    private static void WritePrefix(TextWriter writer, int depth)
    {
      int count = Math.Min(depth * 4, ServiceRecordUtilities.s_lotsOfSpaces.Length);
      writer.Write(ServiceRecordUtilities.s_lotsOfSpaces, 0, count);
    }

    internal enum HackProtocolServiceMultiplexer : short
    {
      Sdp = 1,
      Rfcomm = 3,
      Bnep = 15, // 0x000F
      HidControl = 17, // 0x0011
      HidInterrupt = 19, // 0x0013
    }

    internal enum HackProtocolId : short
    {
      Sdp = 1,
      Rfcomm = 3,
      Obex = 8,
      Bnep = 15, // 0x000F
      Hidp = 17, // 0x0011
      L2Cap = 256, // 0x0100
    }
  }
}
