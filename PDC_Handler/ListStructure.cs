// Decompiled with JetBrains decompiler
// Type: PDC_Handler.ListStructure
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace PDC_Handler
{
  internal sealed class ListStructure
  {
    internal ushort LogChannelIndexA { get; set; }

    internal ushort LogChannelIndexB { get; set; }

    internal MBusList Channel_A_ListA { get; set; }

    internal MBusList Channel_A_ListB { get; set; }

    internal MBusList Channel_A_ListC { get; set; }

    internal MBusList Channel_A_ListInstall { get; set; }

    internal MBusList Channel_B_ListA { get; set; }

    internal MBusList Channel_B_ListB { get; set; }

    internal MBusList Channel_B_ListC { get; set; }

    internal MBusList Channel_B_ListInstall { get; set; }

    internal MBusList Channel_C_ListA { get; set; }

    internal MBusList Channel_C_ListB { get; set; }

    internal MBusList Channel_C_ListC { get; set; }

    internal MBusList Channel_C_ListInstall { get; set; }

    internal static ListStructure Parse(byte[] buffer, ushort addr)
    {
      ushort addr1 = buffer != null ? BitConverter.ToUInt16(buffer, (int) addr) : throw new NullReferenceException(nameof (buffer));
      addr += (ushort) 2;
      ushort uint16_1 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_2 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_3 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_4 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_5 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_6 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_7 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_8 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_9 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_10 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ushort uint16_11 = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      ListStructure listStructure = new ListStructure();
      listStructure.LogChannelIndexA = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      listStructure.LogChannelIndexB = BitConverter.ToUInt16(buffer, (int) addr);
      addr += (ushort) 2;
      listStructure.Channel_A_ListA = MBusList.Parse(buffer, addr1);
      listStructure.Channel_A_ListB = MBusList.Parse(buffer, uint16_1);
      listStructure.Channel_A_ListC = MBusList.Parse(buffer, uint16_2);
      listStructure.Channel_A_ListInstall = MBusList.Parse(buffer, uint16_3);
      listStructure.Channel_B_ListA = MBusList.Parse(buffer, uint16_4);
      listStructure.Channel_B_ListB = MBusList.Parse(buffer, uint16_5);
      listStructure.Channel_B_ListC = MBusList.Parse(buffer, uint16_6);
      listStructure.Channel_B_ListInstall = MBusList.Parse(buffer, uint16_7);
      listStructure.Channel_C_ListA = MBusList.Parse(buffer, uint16_8);
      listStructure.Channel_C_ListB = MBusList.Parse(buffer, uint16_9);
      listStructure.Channel_C_ListC = MBusList.Parse(buffer, uint16_10);
      listStructure.Channel_C_ListInstall = MBusList.Parse(buffer, uint16_11);
      return listStructure;
    }

    internal byte[] CreateMemory(ushort address)
    {
      List<byte> buffer = new List<byte>();
      buffer.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.LogChannelIndexA));
      buffer.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.LogChannelIndexB));
      byte[] memory1 = this.Channel_A_ListA == null ? (byte[]) null : this.Channel_A_ListA.CreateMemory();
      byte[] memory2 = this.Channel_A_ListB == null ? (byte[]) null : this.Channel_A_ListB.CreateMemory();
      byte[] memory3 = this.Channel_A_ListC == null ? (byte[]) null : this.Channel_A_ListC.CreateMemory();
      byte[] memory4 = this.Channel_A_ListInstall == null ? (byte[]) null : this.Channel_A_ListInstall.CreateMemory();
      byte[] memory5 = this.Channel_B_ListA == null ? (byte[]) null : this.Channel_B_ListA.CreateMemory();
      byte[] memory6 = this.Channel_B_ListB == null ? (byte[]) null : this.Channel_B_ListB.CreateMemory();
      byte[] memory7 = this.Channel_B_ListC == null ? (byte[]) null : this.Channel_B_ListC.CreateMemory();
      byte[] memory8 = this.Channel_B_ListInstall == null ? (byte[]) null : this.Channel_B_ListInstall.CreateMemory();
      byte[] memory9 = this.Channel_C_ListA == null ? (byte[]) null : this.Channel_C_ListA.CreateMemory();
      byte[] memory10 = this.Channel_C_ListB == null ? (byte[]) null : this.Channel_C_ListB.CreateMemory();
      byte[] memory11 = this.Channel_C_ListC == null ? (byte[]) null : this.Channel_C_ListC.CreateMemory();
      byte[] memory12 = this.Channel_C_ListInstall == null ? (byte[]) null : this.Channel_C_ListInstall.CreateMemory();
      if (memory1 != null)
        buffer.AddRange((IEnumerable<byte>) memory1);
      if (memory2 != null)
        buffer.AddRange((IEnumerable<byte>) memory2);
      if (memory3 != null)
        buffer.AddRange((IEnumerable<byte>) memory3);
      if (memory4 != null)
        buffer.AddRange((IEnumerable<byte>) memory4);
      if (memory5 != null)
        buffer.AddRange((IEnumerable<byte>) memory5);
      if (memory6 != null)
        buffer.AddRange((IEnumerable<byte>) memory6);
      if (memory7 != null)
        buffer.AddRange((IEnumerable<byte>) memory7);
      if (memory8 != null)
        buffer.AddRange((IEnumerable<byte>) memory8);
      if (memory9 != null)
        buffer.AddRange((IEnumerable<byte>) memory9);
      if (memory10 != null)
        buffer.AddRange((IEnumerable<byte>) memory10);
      if (memory11 != null)
        buffer.AddRange((IEnumerable<byte>) memory11);
      if (memory12 != null)
        buffer.AddRange((IEnumerable<byte>) memory12);
      ushort offset = (ushort) ((uint) address + 28U);
      ListStructure.AddAddress(buffer, memory1, 0, ref offset);
      ListStructure.AddAddress(buffer, memory2, 2, ref offset);
      ListStructure.AddAddress(buffer, memory3, 4, ref offset);
      ListStructure.AddAddress(buffer, memory4, 6, ref offset);
      ListStructure.AddAddress(buffer, memory5, 8, ref offset);
      ListStructure.AddAddress(buffer, memory6, 10, ref offset);
      ListStructure.AddAddress(buffer, memory7, 12, ref offset);
      ListStructure.AddAddress(buffer, memory8, 14, ref offset);
      ListStructure.AddAddress(buffer, memory9, 16, ref offset);
      ListStructure.AddAddress(buffer, memory10, 18, ref offset);
      ListStructure.AddAddress(buffer, memory11, 20, ref offset);
      ListStructure.AddAddress(buffer, memory12, 22, ref offset);
      return buffer.ToArray();
    }

    private static void AddAddress(
      List<byte> buffer,
      byte[] list,
      int insertIndex,
      ref ushort offset)
    {
      if (list != null)
      {
        buffer.InsertRange(insertIndex, (IEnumerable<byte>) BitConverter.GetBytes((ushort) ((uint) offset + 2U)));
        offset += (ushort) list.Length;
      }
      else
        buffer.InsertRange(insertIndex, (IEnumerable<byte>) new byte[2]
        {
          byte.MaxValue,
          byte.MaxValue
        });
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("Lists:");
      sb.AppendLine("LogChannelIndexA = " + this.LogChannelIndexA.ToString());
      sb.AppendLine("LogChannelIndexB = " + this.LogChannelIndexB.ToString());
      this.PrintList(sb, "Channel_A_ListA: ", this.Channel_A_ListA);
      this.PrintList(sb, "Channel_A_ListB: ", this.Channel_A_ListB);
      this.PrintList(sb, "Channel_A_ListC: ", this.Channel_A_ListC);
      this.PrintList(sb, "Channel_A_ListInstall: ", this.Channel_A_ListInstall);
      this.PrintList(sb, "Channel_B_ListA: ", this.Channel_B_ListA);
      this.PrintList(sb, "Channel_B_ListB: ", this.Channel_B_ListB);
      this.PrintList(sb, "Channel_B_ListC: ", this.Channel_B_ListC);
      this.PrintList(sb, "Channel_B_ListInstall: ", this.Channel_B_ListInstall);
      this.PrintList(sb, "Channel_C_ListA: ", this.Channel_C_ListA);
      this.PrintList(sb, "Channel_C_ListB: ", this.Channel_C_ListB);
      this.PrintList(sb, "Channel_C_ListC: ", this.Channel_C_ListC);
      this.PrintList(sb, "Channel_C_ListInstall: ", this.Channel_C_ListInstall);
      return sb.ToString();
    }

    private void PrintList(StringBuilder sb, string name, MBusList list)
    {
      sb.AppendLine(name).AppendLine(list != null ? list.ToString() : "NULL");
    }
  }
}
