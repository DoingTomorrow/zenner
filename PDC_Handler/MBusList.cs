// Decompiled with JetBrains decompiler
// Type: PDC_Handler.MBusList
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace PDC_Handler
{
  public sealed class MBusList
  {
    public ushort UniqueID { get; set; }

    public List<MBusParameter> Parameters { get; set; }

    public MBusList() => this.Parameters = new List<MBusParameter>();

    internal static MBusList Parse(byte[] buffer, ushort addr)
    {
      if (addr == ushort.MaxValue)
        return (MBusList) null;
      MBusList mbusList = new MBusList();
      mbusList.UniqueID = BitConverter.ToUInt16(buffer, (int) addr - 2);
      for (ushort uint16 = BitConverter.ToUInt16(buffer, (int) addr); uint16 != (ushort) 0 && uint16 != ushort.MaxValue; uint16 = BitConverter.ToUInt16(buffer, (int) addr))
      {
        MBusParameter mbusParameter = MBusParameter.Parse(buffer, ref addr);
        if (mbusParameter != null)
          mbusList.Parameters.Add(mbusParameter);
      }
      return mbusList;
    }

    internal byte[] CreateMemory()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.UniqueID));
      foreach (MBusParameter parameter in this.Parameters)
        byteList.AddRange((IEnumerable<byte>) parameter.CreateMemory());
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) 0));
      return byteList.ToArray();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("UniqueID: ").AppendLine(this.UniqueID.ToString());
      foreach (MBusParameter parameter in this.Parameters)
        stringBuilder.AppendLine(parameter.ToString());
      return stringBuilder.ToString();
    }
  }
}
