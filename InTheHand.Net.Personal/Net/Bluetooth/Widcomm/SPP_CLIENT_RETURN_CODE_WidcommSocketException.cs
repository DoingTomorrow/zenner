// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.SPP_CLIENT_RETURN_CODE_WidcommSocketException
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [Serializable]
  internal class SPP_CLIENT_RETURN_CODE_WidcommSocketException : 
    GenericReturnCodeWidcommSocketException<WidcommSppClient.SPP_CLIENT_RETURN_CODE>
  {
    internal SPP_CLIENT_RETURN_CODE_WidcommSocketException(
      int errorCode,
      WidcommSppClient.SPP_CLIENT_RETURN_CODE ret,
      string location)
      : base(errorCode, ret, location)
    {
    }

    protected SPP_CLIENT_RETURN_CODE_WidcommSocketException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
    }
  }
}
