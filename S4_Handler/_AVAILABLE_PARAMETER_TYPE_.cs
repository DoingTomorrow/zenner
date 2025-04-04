// Decompiled with JetBrains decompiler
// Type: S4_Handler._AVAILABLE_PARAMETER_TYPE_
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace S4_Handler
{
  internal class _AVAILABLE_PARAMETER_TYPE_
  {
    internal byte index;
    internal uint varPtr;
    internal byte group;
    internal S4_CommunicationManager.ParamPermission permission;
    internal _VAR_TYPE_ srcType;
    internal _MBUS_INFO_TYPE_ mbusInfo;
    private uint MBusInfoLength;

    internal void LoadFromBytes(byte[] managedParamBytes, uint MBusInfoLength, ref int offset)
    {
      this.MBusInfoLength = MBusInfoLength;
      this.varPtr = BitConverter.ToUInt32(managedParamBytes, offset);
      offset += 4;
      this.group = managedParamBytes[offset++];
      this.permission = (S4_CommunicationManager.ParamPermission) ((int) managedParamBytes[offset] & 128);
      this.srcType = (_VAR_TYPE_) ((int) managedParamBytes[offset++] & (int) sbyte.MaxValue);
      offset += 2;
      this.mbusInfo = new _MBUS_INFO_TYPE_();
      this.mbusInfo.LoadFromBytes(managedParamBytes, MBusInfoLength, ref offset);
    }

    internal void GetBytes(List<byte> byteDestination)
    {
      byteDestination.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.varPtr));
      byteDestination.Add(this.group);
      byteDestination.Add((byte) (this.permission + (int) this.srcType));
      byteDestination.Add((byte) 0);
      byteDestination.Add((byte) 0);
      this.mbusInfo.GetBytes(byteDestination, this.MBusInfoLength);
    }

    internal string GetParameterName() => this.mbusInfo.GetParameterName();

    public override string ToString()
    {
      return this.index.ToString() + "; AGroup=" + this.group.ToString() + "; " + this.permission.ToString() + "; SrcT:" + this.srcType.ToString() + "; " + this.mbusInfo.ToString();
    }
  }
}
