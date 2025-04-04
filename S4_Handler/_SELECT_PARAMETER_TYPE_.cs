// Decompiled with JetBrains decompiler
// Type: S4_Handler._SELECT_PARAMETER_TYPE_
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System.Collections.Generic;

#nullable disable
namespace S4_Handler
{
  internal class _SELECT_PARAMETER_TYPE_
  {
    internal const int SelectedParameterSize = 20;
    internal byte group;
    internal byte paramIndex;
    internal _MBUS_INFO_TYPE_ mbusInfo;
    private uint MBusInfoLength;

    internal void LoadFromBytes(byte[] selectedParamBytes, uint MBusInfoLength, ref int offset)
    {
      this.MBusInfoLength = MBusInfoLength;
      this.group = selectedParamBytes[offset++];
      this.paramIndex = selectedParamBytes[offset++];
      offset += 2;
      this.mbusInfo = new _MBUS_INFO_TYPE_();
      this.mbusInfo.LoadFromBytes(selectedParamBytes, MBusInfoLength, ref offset);
    }

    internal void GetBytes(List<byte> byteDestination)
    {
      byteDestination.Add(this.group);
      byteDestination.Add(this.paramIndex);
      byteDestination.Add((byte) 0);
      byteDestination.Add((byte) 0);
      this.mbusInfo.GetBytes(byteDestination, this.MBusInfoLength);
    }

    internal string GetParameterName() => this.mbusInfo.GetParameterName();

    public override string ToString()
    {
      return "; SGroup=" + this.group.ToString() + "; PIndex:" + this.paramIndex.ToString() + "; " + this.mbusInfo.ToString();
    }
  }
}
