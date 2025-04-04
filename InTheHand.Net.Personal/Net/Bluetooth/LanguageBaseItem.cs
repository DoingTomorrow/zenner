// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.LanguageBaseItem
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class LanguageBaseItem
  {
    public const ServiceAttributeId PrimaryLanguageBaseAttributeId = (ServiceAttributeId) 256;
    public const short Utf8EncodingId = 106;
    public const string ErrorMsgLangBaseListParseNotU16 = "Element in LanguageBaseAttributeIdList not type UInt16.";
    public const string ErrorMsgLangBaseListParseBaseInvalid = "Base element in LanguageBaseAttributeIdList has unacceptable value.";
    public const string ErrorMsgLangBaseListParseNotSequence = "LanguageBaseAttributeIdList elementSequence not an ElementSequence.";
    public const string ErrorMsgLangBaseListParseNotInThrees = "LanguageBaseAttributeIdList must contain items in groups of three.";
    public const string ErrorMsgFormatUnrecognizedEncodingId = "Unrecognized character encoding ({0}); add to LanguageBaseItem mapping table.";
    public const string ErrorMsgLangMustAsciiTwoChars = "A language code must be a two byte ASCII string.";
    private readonly ushort m_naturalLanguage;
    private readonly ServiceAttributeId m_baseAttrId;
    private readonly ushort m_encodingId;
    private static readonly LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap[] s_IetfCharsetIdToDotNetEncodingNameTable = new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap[19]
    {
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 3, "us-ascii"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 4, "iso-8859-1"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 5, "iso-8859-2"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 6, "iso-8859-3"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 7, "iso-8859-4"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 8, "iso-8859-5"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 9, "iso-8859-6"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 10, "iso-8859-7"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 11, "iso-8859-8"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 12, "iso-8859-9"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 13, "iso-8859-10"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 106, "UTF-8"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 109, "iso-8859-13"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 110, "iso-8859-14"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 111, "iso-8859-15"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 112, "iso-8859-16"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 1013, "unicodeFFFE"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 1014, "utf-16"),
      new LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap((ushort) 1015, "utf-16")
    };

    [CLSCompliant(false)]
    public LanguageBaseItem(ushort naturalLanguage, ushort encodingId, ushort baseAttributeId)
      : this(naturalLanguage, encodingId, (ServiceAttributeId) baseAttributeId)
    {
    }

    public LanguageBaseItem(short naturalLanguage, short encodingId, short baseAttributeId)
      : this((ushort) naturalLanguage, (ushort) encodingId, (ServiceAttributeId) baseAttributeId)
    {
    }

    [CLSCompliant(false)]
    public LanguageBaseItem(
      ushort naturalLanguage,
      ushort encodingId,
      ServiceAttributeId baseAttributeId)
    {
      if (baseAttributeId == (ServiceAttributeId) 0)
        throw new ArgumentOutOfRangeException(nameof (baseAttributeId));
      this.m_naturalLanguage = naturalLanguage;
      this.m_baseAttrId = baseAttributeId;
      this.m_encodingId = encodingId;
    }

    public LanguageBaseItem(
      short naturalLanguage,
      short encodingId,
      ServiceAttributeId baseAttributeId)
      : this((ushort) naturalLanguage, (ushort) encodingId, baseAttributeId)
    {
    }

    [CLSCompliant(false)]
    public LanguageBaseItem(
      string naturalLanguage,
      ushort encodingId,
      ServiceAttributeId baseAttributeId)
      : this(LanguageBaseItem.GetLanguageIdStringAsBytes(naturalLanguage), encodingId, baseAttributeId)
    {
    }

    public LanguageBaseItem(
      string naturalLanguage,
      short encodingId,
      ServiceAttributeId baseAttributeId)
      : this(LanguageBaseItem.GetLanguageIdStringAsBytes(naturalLanguage), (ushort) encodingId, baseAttributeId)
    {
    }

    private static ushort GetLanguageIdStringAsBytes(string language)
    {
      byte[] numArray = language.Length == 2 ? Encoding.UTF8.GetBytes(language) : throw new ArgumentException("A language code must be a two byte ASCII string.");
      return numArray.Length == 2 ? (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(numArray, 0)) : throw new ArgumentException("A language code must be a two byte ASCII string.");
    }

    private string GetLanguageIdBytesAsString()
    {
      byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) this.m_naturalLanguage));
      return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
    }

    public static LanguageBaseItem[] ParseListFromElementSequence(ServiceElement elementSequence)
    {
      IList<ServiceElement> serviceElementList = elementSequence.ElementType == ElementType.ElementSequence ? elementSequence.GetValueAsElementList() : throw new ArgumentException("LanguageBaseAttributeIdList elementSequence not an ElementSequence.");
      int count = serviceElementList.Count;
      if (count == 0 || count % 3 != 0)
        throw new ProtocolViolationException("LanguageBaseAttributeIdList must contain items in groups of three.");
      int length = count / 3;
      LanguageBaseItem[] fromElementSequence = new LanguageBaseItem[length];
      for (int index = 0; index < length; ++index)
      {
        ServiceElement serviceElement1 = serviceElementList[index * 3];
        ServiceElement serviceElement2 = serviceElementList[index * 3 + 1];
        ServiceElement serviceElement3 = serviceElementList[index * 3 + 2];
        if (serviceElement1.ElementType != ElementType.UInt16 || serviceElement2.ElementType != ElementType.UInt16 || serviceElement3.ElementType != ElementType.UInt16)
          throw new ProtocolViolationException("Element in LanguageBaseAttributeIdList not type UInt16.");
        if ((ushort) serviceElement3.Value == (ushort) 0)
          throw new ProtocolViolationException("Base element in LanguageBaseAttributeIdList has unacceptable value.");
        LanguageBaseItem languageBaseItem = new LanguageBaseItem((ushort) serviceElement1.Value, (ushort) serviceElement2.Value, (ushort) serviceElement3.Value);
        fromElementSequence[index] = languageBaseItem;
      }
      return fromElementSequence;
    }

    public static ServiceElement CreateElementSequenceFromList(LanguageBaseItem[] list)
    {
      IList<ServiceElement> childElements = (IList<ServiceElement>) new List<ServiceElement>();
      foreach (LanguageBaseItem languageBaseItem in list)
      {
        ushort languageAsUint16 = languageBaseItem.NaturalLanguageAsUInt16;
        childElements.Add(new ServiceElement(ElementType.UInt16, (object) languageAsUint16));
        childElements.Add(new ServiceElement(ElementType.UInt16, (object) languageBaseItem.EncodingId));
        childElements.Add(new ServiceElement(ElementType.UInt16, (object) (ushort) languageBaseItem.AttributeIdBase));
      }
      return new ServiceElement(ElementType.ElementSequence, childElements);
    }

    public static LanguageBaseItem CreateEnglishUtf8PrimaryLanguageItem()
    {
      return new LanguageBaseItem("en", (short) 106, (ServiceAttributeId) 256);
    }

    public string NaturalLanguage => this.GetLanguageIdBytesAsString();

    [CLSCompliant(false)]
    public ushort NaturalLanguageAsUInt16 => this.m_naturalLanguage;

    public short NaturalLanguageAsInt16 => (short) this.m_naturalLanguage;

    public ServiceAttributeId AttributeIdBase => this.m_baseAttrId;

    [CLSCompliant(false)]
    public ushort EncodingId => this.m_encodingId;

    public short EncodingIdAsInt16 => (short) this.m_encodingId;

    public Encoding GetEncoding()
    {
      if (this.m_encodingId >= (ushort) 2252 && this.m_encodingId <= (ushort) 2258)
        return Encoding.GetEncoding("windows-" + ((int) this.m_encodingId - 1000).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      foreach (LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap netEncodingNameMap in LanguageBaseItem.s_IetfCharsetIdToDotNetEncodingNameTable)
      {
        if ((int) netEncodingNameMap.IetfCharsetId == (int) this.m_encodingId)
          return Encoding.GetEncoding(netEncodingNameMap.DotNetEncodingName);
      }
      throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Unrecognized character encoding ({0}); add to LanguageBaseItem mapping table.", (object) this.m_encodingId));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static string TestAllDefinedEncodingMappingRows(
      out int numberSuccessful,
      out int numberFailed)
    {
      numberSuccessful = 0;
      numberFailed = 0;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (LanguageBaseItem.IetfCharsetIdToDotNetEncodingNameMap netEncodingNameMap in LanguageBaseItem.s_IetfCharsetIdToDotNetEncodingNameTable)
      {
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "id: {0}, name: {1}.  ", (object) netEncodingNameMap.IetfCharsetId, (object) netEncodingNameMap.DotNetEncodingName);
        try
        {
          Encoding.GetEncoding(netEncodingNameMap.DotNetEncodingName);
          stringBuilder.Append("Success");
          ++numberSuccessful;
        }
        catch (Exception ex)
        {
          stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "Failed with {0}:{1}", (object) ex.GetType().FullName, (object) ex.Message);
          ++numberFailed;
        }
        stringBuilder.Append("\r\n");
      }
      return stringBuilder.ToString();
    }

    private struct IetfCharsetIdToDotNetEncodingNameMap
    {
      public readonly ushort IetfCharsetId;
      public readonly string DotNetEncodingName;

      internal IetfCharsetIdToDotNetEncodingNameMap(
        ushort ietfCharsetId_,
        string DotNetEncodingName_)
      {
        this.IetfCharsetId = ietfCharsetId_;
        this.DotNetEncodingName = DotNetEncodingName_;
      }
    }
  }
}
