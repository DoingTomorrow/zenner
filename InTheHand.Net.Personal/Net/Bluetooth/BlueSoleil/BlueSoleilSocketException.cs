// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.BlueSoleilSocketException
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  [Serializable]
  internal class BlueSoleilSocketException : SocketException
  {
    private const string Key_BtSdkError = "BtSdkError";

    internal BlueSoleilSocketException(BtSdkError bsError, int socketErr)
      : base(socketErr)
    {
      this.Set((int) bsError);
    }

    internal BlueSoleilSocketException(BtSdkError bsError, SocketError socketErr)
      : this(bsError, (int) socketErr)
    {
    }

    private void Set(int iError) => this.Data.Add((object) "BtSdkError", (object) iError);

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    protected BlueSoleilSocketException(
      SerializationInfo serializationInfo,
      StreamingContext streamingContext)
      : base(serializationInfo, streamingContext)
    {
      this.Set(serializationInfo.GetInt32("BtSdkError"));
    }

    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("BtSdkError", this.BlueSoleilErrorCode);
    }

    public string BlueSoleilError
    {
      get
      {
        int blueSoleilErrorCode = this.BlueSoleilErrorCode;
        return Enum.IsDefined(typeof (BtSdkError), (object) blueSoleilErrorCode) ? ((BtSdkError) blueSoleilErrorCode).ToString() : "0x" + ((uint) blueSoleilErrorCode).ToString("X", (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    private BtSdkError BlueSoleilErrorCodeEnum => (BtSdkError) this.BlueSoleilErrorCode;

    public int BlueSoleilErrorCode => (int) this.Data[(object) "BtSdkError"];

    public override string Message
    {
      get
      {
        return base.Message + string.Format((IFormatProvider) CultureInfo.InvariantCulture, " (BlueSoleil: {0} (0x{1:X4})).", (object) this.BlueSoleilErrorCodeEnum, (object) (uint) this.BlueSoleilErrorCode);
      }
    }
  }
}
