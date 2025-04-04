// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceElement
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class ServiceElement
  {
    private const bool _strictStringDecoding = true;
    public const string ErrorMsgNotUuidType = "Element is not of type UUID.";
    public const string ErrorMsgNotTextStringType = "Not TextString type.";
    public const string ErrorMsgNotUrlType = "Not Url type.";
    public const string ErrorMsgNotSeqAltType = "Not Element Sequence or Alternative type.";
    public const string ErrorMsgSeqAltTypeNeedElementArray = "ElementType Sequence or Alternative needs an array of ServiceElement.";
    public const string ErrorMsgFmtCreateNumericalGivenNonNumber = "Not a numerical type ({0}).";
    public const string ErrorMsgListContainsNotElement = "The list contains a element which is not a ServiceElement.";
    private ElementType m_type;
    private ElementTypeDescriptor m_etd;
    private object m_rawValue;

    public ServiceElement(ElementType type, object value)
      : this(ServiceElement.GetEtdForType(type), type, value)
    {
    }

    private static ElementTypeDescriptor GetEtdForType(ElementType type)
    {
      return ServiceRecordParser.GetEtdForType(type);
    }

    public ServiceElement(ElementType type, IList<ServiceElement> childElements)
      : this(type, (object) childElements)
    {
    }

    public ServiceElement(ElementType type, params ServiceElement[] childElements)
      : this(ServiceElement.checkTypeSuitsElementParamsArray(type, childElements), (object) childElements)
    {
    }

    private static ElementType checkTypeSuitsElementParamsArray(
      ElementType typePassThru,
      ServiceElement[] childElements)
    {
      return childElements == null || typePassThru == ElementType.ElementSequence || typePassThru == ElementType.ElementAlternative ? typePassThru : throw new ArgumentException("ElementType Sequence or Alternative needs an array of ServiceElement.");
    }

    internal ServiceElement(ElementTypeDescriptor etd, ElementType type, object value)
    {
      ServiceRecordParser.VerifyTypeMatchesEtd(etd, type);
      this.m_type = type;
      this.m_etd = etd;
      this.SetValue(value);
    }

    internal void SetValue(object value)
    {
      ElementTypeDescriptor etd = this.m_etd;
      ElementType type = this.m_type;
      if (value == null)
      {
        if (etd == ElementTypeDescriptor.ElementSequence || etd == ElementTypeDescriptor.ElementAlternative)
          throw new ArgumentNullException(nameof (value), "Type DataElementSequence and DataElementAlternative need an list of AttributeValue.");
        if (etd != ElementTypeDescriptor.Nil && etd != ElementTypeDescriptor.Unknown)
          throw new ArgumentNullException(nameof (value), "Null not valid for type: '" + (object) type + "'.");
      }
      else
      {
        IList<ServiceElement> serviceElementList = value as IList<ServiceElement>;
        if (etd == ElementTypeDescriptor.ElementSequence || etd == ElementTypeDescriptor.ElementAlternative)
        {
          if (serviceElementList == null)
            throw new ArgumentException("Type ElementSequence and ElementAlternative need an list of ServiceElement.");
        }
        else if (serviceElementList != null)
          throw new ArgumentException("Type ElementSequence and ElementAlternative must be used for an list of ServiceElement.");
        bool flag1;
        bool flag2;
        if (type == ElementType.Nil)
          flag1 = value == null;
        else if (etd == ElementTypeDescriptor.UnsignedInteger || etd == ElementTypeDescriptor.TwosComplementInteger)
        {
          switch (type)
          {
            case ElementType.UInt8:
              flag1 = value is byte;
              break;
            case ElementType.UInt16:
              flag1 = value is ushort;
              break;
            case ElementType.UInt32:
              flag1 = value is uint;
              break;
            case ElementType.UInt64:
              flag1 = value is ulong;
              break;
            case ElementType.UInt128:
            case ElementType.Int128:
              if (value is byte[] numArray && numArray.Length == 16)
              {
                flag1 = true;
                break;
              }
              flag2 = false;
              throw new ArgumentException("Element type '" + (object) type + "' needs a length 16 byte array.");
            case ElementType.Int8:
              flag1 = value is sbyte;
              break;
            case ElementType.Int16:
              flag1 = value is short;
              break;
            case ElementType.Int32:
              flag1 = value is int;
              break;
            case ElementType.Int64:
              flag1 = value is long;
              break;
            default:
              flag1 = false;
              break;
          }
        }
        else
        {
          switch (type)
          {
            case ElementType.Uuid16:
              flag2 = value is ushort;
              flag1 = value is ushort;
              break;
            case ElementType.Uuid32:
              int num;
              switch (value)
              {
                case ushort _:
                case short _:
                case uint _:
                  num = 1;
                  break;
                default:
                  num = value is int ? 1 : 0;
                  break;
              }
              flag1 = num != 0;
              break;
            case ElementType.Uuid128:
              flag1 = value is Guid;
              break;
            case ElementType.TextString:
              flag1 = value is byte[] || value is string;
              break;
            case ElementType.Boolean:
              flag1 = value is bool;
              break;
            case ElementType.ElementSequence:
            case ElementType.ElementAlternative:
              flag1 = serviceElementList != null;
              break;
            default:
              flag1 = value is byte[] || (object) (value as Uri) != null || value is string;
              break;
          }
        }
        if (!flag1)
          throw new ArgumentException("CLR type '" + value.GetType().Name + "' not valid type for element type '" + (object) type + "'.");
      }
      this.m_rawValue = value;
    }

    public static ServiceElement CreateNumericalServiceElement(
      ElementType elementType,
      object value)
    {
      ElementTypeDescriptor etdForType = ServiceElement.GetEtdForType(elementType);
      switch (etdForType)
      {
        case ElementTypeDescriptor.UnsignedInteger:
        case ElementTypeDescriptor.TwosComplementInteger:
          object obj = ServiceElement.ConvertNumericalValue(elementType, value);
          return new ServiceElement(elementType, obj);
        default:
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Not a numerical type ({0}).", (object) etdForType));
      }
    }

    private static object ConvertNumericalValue(ElementType elementType, object value)
    {
      Exception innerException;
      switch (value)
      {
        case byte _:
        case short _:
        case int _:
        case long _:
        case sbyte _:
        case ushort _:
        case uint _:
        case ulong _:
        case Enum _:
          try
          {
            IConvertible convertible = (IConvertible) value;
            IFormatProvider invariantCulture = (IFormatProvider) CultureInfo.InvariantCulture;
            object obj;
            switch (elementType)
            {
              case ElementType.UInt8:
                obj = (object) convertible.ToByte(invariantCulture);
                break;
              case ElementType.UInt16:
                obj = (object) convertible.ToUInt16(invariantCulture);
                break;
              case ElementType.UInt32:
                obj = (object) convertible.ToUInt32(invariantCulture);
                break;
              case ElementType.UInt64:
                obj = (object) convertible.ToUInt64(invariantCulture);
                break;
              case ElementType.Int8:
                obj = (object) convertible.ToSByte(invariantCulture);
                break;
              case ElementType.Int16:
                obj = (object) convertible.ToInt16(invariantCulture);
                break;
              case ElementType.Int64:
                obj = (object) convertible.ToInt64(invariantCulture);
                break;
              default:
                obj = (object) convertible.ToInt32(invariantCulture);
                break;
            }
            return obj;
          }
          catch (OverflowException ex)
          {
            innerException = (Exception) ex;
            break;
          }
        default:
          innerException = (Exception) null;
          break;
      }
      throw ServiceRecordParser.new_ArgumentOutOfRangeException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Value '{1}'  of type '{2}' not valid for element type {0}.", (object) elementType, value, (object) value.GetType()), innerException);
    }

    public ElementType ElementType
    {
      [DebuggerStepThrough] get => this.m_type;
    }

    public ElementTypeDescriptor ElementTypeDescriptor
    {
      [DebuggerStepThrough] get => this.m_etd;
    }

    public object Value
    {
      [DebuggerStepThrough] get => this.m_rawValue;
    }

    public IList<ServiceElement> GetValueAsElementList()
    {
      if (this.m_etd != ElementTypeDescriptor.ElementSequence && this.m_etd != ElementTypeDescriptor.ElementAlternative)
        throw new InvalidOperationException("Not Element Sequence or Alternative type.");
      return (IList<ServiceElement>) this.m_rawValue;
    }

    public ServiceElement[] GetValueAsElementArray()
    {
      ServiceElement[] array = new ServiceElement[this.GetValueAsElementList().Count];
      this.GetValueAsElementList().CopyTo(array, 0);
      return array;
    }

    public Uri GetValueAsUri()
    {
      if (this.m_type != ElementType.Url)
        throw new InvalidOperationException("Not Url type.");
      Uri valueAsUri = this.m_rawValue as Uri;
      if (valueAsUri == (Uri) null)
        valueAsUri = new Uri(!(this.m_rawValue is byte[] rawValue) ? (string) this.m_rawValue : ServiceRecordParser.CreateUriStringFromBytes(rawValue));
      return valueAsUri;
    }

    public Guid GetValueAsUuid()
    {
      if (this.m_etd != ElementTypeDescriptor.Uuid)
        throw new InvalidOperationException("Element is not of type UUID.");
      if (this.m_type == ElementType.Uuid16)
        return BluetoothService.CreateBluetoothUuid((ushort) this.Value);
      return this.m_type == ElementType.Uuid32 ? BluetoothService.CreateBluetoothUuid((uint) this.Value) : (Guid) this.Value;
    }

    public string GetValueAsString(Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (this.m_type != ElementType.TextString)
        throw new InvalidOperationException("Not TextString type.");
      if (this.m_rawValue is string rawValue1)
        return rawValue1;
      byte[] rawValue2 = (byte[]) this.m_rawValue;
      string valueAsString = encoding.GetString(rawValue2, 0, rawValue2.Length);
      if (valueAsString.Length > 0 && valueAsString[valueAsString.Length - 1] == char.MinValue)
        valueAsString = valueAsString.Substring(0, valueAsString.Length - 1);
      return valueAsString;
    }

    public string GetValueAsString(LanguageBaseItem languageBase)
    {
      Encoding encoding = languageBase != null ? (Encoding) languageBase.GetEncoding().Clone() : throw new ArgumentNullException(nameof (languageBase));
      encoding.DecoderFallback = (DecoderFallback) new DecoderExceptionFallback();
      encoding.EncoderFallback = (EncoderFallback) new EncoderExceptionFallback();
      return this.GetValueAsString(encoding);
    }

    public string GetValueAsStringUtf8()
    {
      return this.GetValueAsString((Encoding) new UTF8Encoding(false, true));
    }
  }
}
