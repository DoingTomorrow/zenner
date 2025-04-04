// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.GenericReturnCodeWidcommSocketException`1
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal abstract class GenericReturnCodeWidcommSocketException<T> : WidcommSocketException where T : IConvertible
  {
    private const string SzName_ret = "_ret";
    protected readonly int m_ret;
    protected string m_retName;

    internal GenericReturnCodeWidcommSocketException(int errorCode, T ret, string location)
      : base(errorCode, location)
    {
      if (!typeof (T).IsEnum)
        throw new InvalidOperationException("Internal error -- The generic parameter must be an Enum type.");
      this.m_ret = ret.ToInt32((IFormatProvider) CultureInfo.InvariantCulture);
      this.SetEnum();
    }

    protected void SetEnum() => this.m_retName = Enum.Format(typeof (T), (object) this.m_ret, "G");

    protected override string ErrorCodeAndDescription
    {
      get
      {
        return string.Format((IFormatProvider) CultureInfo.InvariantCulture, typeof (T).Name + "={0}=0x{1:X}", (object) this.m_retName, (object) this.m_ret);
      }
    }

    protected GenericReturnCodeWidcommSocketException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
      this.m_ret = info.GetInt32("_ret");
      this.SetEnum();
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("_ret", this.m_ret);
    }
  }
}
