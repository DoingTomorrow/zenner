// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.REM_DEV_INFO_RETURN_CODE_WidcommSocketException
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [Serializable]
  internal class REM_DEV_INFO_RETURN_CODE_WidcommSocketException : 
    GenericReturnCodeWidcommSocketException<REM_DEV_INFO_RETURN_CODE>
  {
    internal REM_DEV_INFO_RETURN_CODE_WidcommSocketException(
      int errorCode,
      REM_DEV_INFO_RETURN_CODE ret,
      string location)
      : base(errorCode, ret, location)
    {
    }

    protected REM_DEV_INFO_RETURN_CODE_WidcommSocketException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
    }
  }
}
