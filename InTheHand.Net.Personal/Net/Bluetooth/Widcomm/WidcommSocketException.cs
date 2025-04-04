// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommSocketException
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [Serializable]
  internal abstract class WidcommSocketException : SocketException
  {
    private const string SzName_location = "_location";
    private readonly string m_location;

    protected WidcommSocketException(int errorCode, string location)
      : base(errorCode)
    {
      this.m_location = location;
    }

    public override string Message
    {
      get
      {
        return this.ErrorCodeAndDescription + (this.m_location == null ? (string) null : "; " + this.m_location);
      }
    }

    protected abstract string ErrorCodeAndDescription { get; }

    protected WidcommSocketException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.m_location = info.GetString("_location");
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("_location", (object) this.m_location);
    }
  }
}
