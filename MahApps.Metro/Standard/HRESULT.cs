// Decompiled with JetBrains decompiler
// Type: Standard.HRESULT
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [StructLayout(LayoutKind.Explicit)]
  internal struct HRESULT
  {
    [FieldOffset(0)]
    private readonly uint _value;
    public static readonly HRESULT S_OK = new HRESULT(0);
    public static readonly HRESULT S_FALSE = new HRESULT(1);
    public static readonly HRESULT E_PENDING = new HRESULT(2147483658U);
    public static readonly HRESULT E_NOTIMPL = new HRESULT(2147500033U);
    public static readonly HRESULT E_NOINTERFACE = new HRESULT(2147500034U);
    public static readonly HRESULT E_POINTER = new HRESULT(2147500035U);
    public static readonly HRESULT E_ABORT = new HRESULT(2147500036U);
    public static readonly HRESULT E_FAIL = new HRESULT(2147500037U);
    public static readonly HRESULT E_UNEXPECTED = new HRESULT(2147549183U);
    public static readonly HRESULT STG_E_INVALIDFUNCTION = new HRESULT(2147680257U);
    public static readonly HRESULT OLE_E_ADVISENOTSUPPORTED = new HRESULT(2147745795U);
    public static readonly HRESULT DV_E_FORMATETC = new HRESULT(2147745892U);
    public static readonly HRESULT DV_E_TYMED = new HRESULT(2147745897U);
    public static readonly HRESULT DV_E_CLIPFORMAT = new HRESULT(2147745898U);
    public static readonly HRESULT DV_E_DVASPECT = new HRESULT(2147745899U);
    public static readonly HRESULT REGDB_E_CLASSNOTREG = new HRESULT(2147746132U);
    public static readonly HRESULT DESTS_E_NO_MATCHING_ASSOC_HANDLER = new HRESULT(2147749635U);
    public static readonly HRESULT DESTS_E_NORECDOCS = new HRESULT(2147749636U);
    public static readonly HRESULT DESTS_E_NOTALLCLEARED = new HRESULT(2147749637U);
    public static readonly HRESULT E_ACCESSDENIED = new HRESULT(2147942405U);
    public static readonly HRESULT E_OUTOFMEMORY = new HRESULT(2147942414U);
    public static readonly HRESULT E_INVALIDARG = new HRESULT(2147942487U);
    public static readonly HRESULT INTSAFE_E_ARITHMETIC_OVERFLOW = new HRESULT(2147942934U);
    public static readonly HRESULT COR_E_OBJECTDISPOSED = new HRESULT(2148734498U);
    public static readonly HRESULT WC_E_GREATERTHAN = new HRESULT(3222072867U);
    public static readonly HRESULT WC_E_SYNTAX = new HRESULT(3222072877U);
    public static readonly HRESULT WINCODEC_ERR_GENERIC_ERROR = HRESULT.E_FAIL;
    public static readonly HRESULT WINCODEC_ERR_INVALIDPARAMETER = HRESULT.E_INVALIDARG;
    public static readonly HRESULT WINCODEC_ERR_OUTOFMEMORY = HRESULT.E_OUTOFMEMORY;
    public static readonly HRESULT WINCODEC_ERR_NOTIMPLEMENTED = HRESULT.E_NOTIMPL;
    public static readonly HRESULT WINCODEC_ERR_ABORTED = HRESULT.E_ABORT;
    public static readonly HRESULT WINCODEC_ERR_ACCESSDENIED = HRESULT.E_ACCESSDENIED;
    public static readonly HRESULT WINCODEC_ERR_VALUEOVERFLOW = HRESULT.INTSAFE_E_ARITHMETIC_OVERFLOW;
    public static readonly HRESULT WINCODEC_ERR_WRONGSTATE = HRESULT.Make(true, Facility.WinCodec, 12036);
    public static readonly HRESULT WINCODEC_ERR_VALUEOUTOFRANGE = HRESULT.Make(true, Facility.WinCodec, 12037);
    public static readonly HRESULT WINCODEC_ERR_UNKNOWNIMAGEFORMAT = HRESULT.Make(true, Facility.WinCodec, 12039);
    public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDVERSION = HRESULT.Make(true, Facility.WinCodec, 12043);
    public static readonly HRESULT WINCODEC_ERR_NOTINITIALIZED = HRESULT.Make(true, Facility.WinCodec, 12044);
    public static readonly HRESULT WINCODEC_ERR_ALREADYLOCKED = HRESULT.Make(true, Facility.WinCodec, 12045);
    public static readonly HRESULT WINCODEC_ERR_PROPERTYNOTFOUND = HRESULT.Make(true, Facility.WinCodec, 12096);
    public static readonly HRESULT WINCODEC_ERR_PROPERTYNOTSUPPORTED = HRESULT.Make(true, Facility.WinCodec, 12097);
    public static readonly HRESULT WINCODEC_ERR_PROPERTYSIZE = HRESULT.Make(true, Facility.WinCodec, 12098);
    public static readonly HRESULT WINCODEC_ERR_CODECPRESENT = HRESULT.Make(true, Facility.WinCodec, 12099);
    public static readonly HRESULT WINCODEC_ERR_CODECNOTHUMBNAIL = HRESULT.Make(true, Facility.WinCodec, 12100);
    public static readonly HRESULT WINCODEC_ERR_PALETTEUNAVAILABLE = HRESULT.Make(true, Facility.WinCodec, 12101);
    public static readonly HRESULT WINCODEC_ERR_CODECTOOMANYSCANLINES = HRESULT.Make(true, Facility.WinCodec, 12102);
    public static readonly HRESULT WINCODEC_ERR_INTERNALERROR = HRESULT.Make(true, Facility.WinCodec, 12104);
    public static readonly HRESULT WINCODEC_ERR_SOURCERECTDOESNOTMATCHDIMENSIONS = HRESULT.Make(true, Facility.WinCodec, 12105);
    public static readonly HRESULT WINCODEC_ERR_COMPONENTNOTFOUND = HRESULT.Make(true, Facility.WinCodec, 12112);
    public static readonly HRESULT WINCODEC_ERR_IMAGESIZEOUTOFRANGE = HRESULT.Make(true, Facility.WinCodec, 12113);
    public static readonly HRESULT WINCODEC_ERR_TOOMUCHMETADATA = HRESULT.Make(true, Facility.WinCodec, 12114);
    public static readonly HRESULT WINCODEC_ERR_BADIMAGE = HRESULT.Make(true, Facility.WinCodec, 12128);
    public static readonly HRESULT WINCODEC_ERR_BADHEADER = HRESULT.Make(true, Facility.WinCodec, 12129);
    public static readonly HRESULT WINCODEC_ERR_FRAMEMISSING = HRESULT.Make(true, Facility.WinCodec, 12130);
    public static readonly HRESULT WINCODEC_ERR_BADMETADATAHEADER = HRESULT.Make(true, Facility.WinCodec, 12131);
    public static readonly HRESULT WINCODEC_ERR_BADSTREAMDATA = HRESULT.Make(true, Facility.WinCodec, 12144);
    public static readonly HRESULT WINCODEC_ERR_STREAMWRITE = HRESULT.Make(true, Facility.WinCodec, 12145);
    public static readonly HRESULT WINCODEC_ERR_STREAMREAD = HRESULT.Make(true, Facility.WinCodec, 12146);
    public static readonly HRESULT WINCODEC_ERR_STREAMNOTAVAILABLE = HRESULT.Make(true, Facility.WinCodec, 12147);
    public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT = HRESULT.Make(true, Facility.WinCodec, 12160);
    public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDOPERATION = HRESULT.Make(true, Facility.WinCodec, 12161);
    public static readonly HRESULT WINCODEC_ERR_INVALIDREGISTRATION = HRESULT.Make(true, Facility.WinCodec, 12170);
    public static readonly HRESULT WINCODEC_ERR_COMPONENTINITIALIZEFAILURE = HRESULT.Make(true, Facility.WinCodec, 12171);
    public static readonly HRESULT WINCODEC_ERR_INSUFFICIENTBUFFER = HRESULT.Make(true, Facility.WinCodec, 12172);
    public static readonly HRESULT WINCODEC_ERR_DUPLICATEMETADATAPRESENT = HRESULT.Make(true, Facility.WinCodec, 12173);
    public static readonly HRESULT WINCODEC_ERR_PROPERTYUNEXPECTEDTYPE = HRESULT.Make(true, Facility.WinCodec, 12174);
    public static readonly HRESULT WINCODEC_ERR_UNEXPECTEDSIZE = HRESULT.Make(true, Facility.WinCodec, 12175);
    public static readonly HRESULT WINCODEC_ERR_INVALIDQUERYREQUEST = HRESULT.Make(true, Facility.WinCodec, 12176);
    public static readonly HRESULT WINCODEC_ERR_UNEXPECTEDMETADATATYPE = HRESULT.Make(true, Facility.WinCodec, 12177);
    public static readonly HRESULT WINCODEC_ERR_REQUESTONLYVALIDATMETADATAROOT = HRESULT.Make(true, Facility.WinCodec, 12178);
    public static readonly HRESULT WINCODEC_ERR_INVALIDQUERYCHARACTER = HRESULT.Make(true, Facility.WinCodec, 12179);

    public HRESULT(uint i) => this._value = i;

    public HRESULT(int i) => this._value = (uint) i;

    public static explicit operator int(HRESULT hr) => (int) hr._value;

    public static HRESULT Make(bool severe, Facility facility, int code)
    {
      return new HRESULT((uint) ((severe ? int.MinValue : 0) | (int) facility << 16 | code));
    }

    public Facility Facility => HRESULT.GetFacility((int) this._value);

    public static Facility GetFacility(int errorCode) => (Facility) (errorCode >> 16 & 8191);

    public int Code => HRESULT.GetCode((int) this._value);

    public static int GetCode(int error) => error & (int) ushort.MaxValue;

    public override string ToString()
    {
      foreach (FieldInfo field in typeof (HRESULT).GetFields(BindingFlags.Static | BindingFlags.Public))
      {
        if (field.FieldType == typeof (HRESULT) && (HRESULT) field.GetValue((object) null) == this)
          return field.Name;
      }
      if (this.Facility == Facility.Win32)
      {
        foreach (FieldInfo field in typeof (Win32Error).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
          if (field.FieldType == typeof (Win32Error) && (HRESULT) (Win32Error) field.GetValue((object) null) == this)
            return "HRESULT_FROM_WIN32(" + field.Name + ")";
        }
      }
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "0x{0:X8}", new object[1]
      {
        (object) this._value
      });
    }

    public override bool Equals(object obj)
    {
      try
      {
        return (int) ((HRESULT) obj)._value == (int) this._value;
      }
      catch (InvalidCastException ex)
      {
        return false;
      }
    }

    public override int GetHashCode() => this._value.GetHashCode();

    public static bool operator ==(HRESULT hrLeft, HRESULT hrRight)
    {
      return (int) hrLeft._value == (int) hrRight._value;
    }

    public static bool operator !=(HRESULT hrLeft, HRESULT hrRight) => !(hrLeft == hrRight);

    public bool Succeeded => (int) this._value >= 0;

    public bool Failed => (int) this._value < 0;

    public void ThrowIfFailed() => this.ThrowIfFailed((string) null);

    public void ThrowIfFailed(string message)
    {
      if (this.Failed)
      {
        if (string.IsNullOrEmpty(message))
          message = this.ToString();
        Exception exception = Marshal.GetExceptionForHR((int) this._value, new IntPtr(-1));
        if (exception.GetType() == typeof (COMException))
        {
          exception = this.Facility != Facility.Win32 ? (Exception) new COMException(message, (int) this._value) : (Exception) new Win32Exception(this.Code, message);
        }
        else
        {
          ConstructorInfo constructor = exception.GetType().GetConstructor(new Type[1]
          {
            typeof (string)
          });
          if ((ConstructorInfo) null != constructor)
            exception = constructor.Invoke(new object[1]
            {
              (object) message
            }) as Exception;
        }
        throw exception;
      }
    }

    public static void ThrowLastError() => ((HRESULT) Win32Error.GetLastError()).ThrowIfFailed();
  }
}
