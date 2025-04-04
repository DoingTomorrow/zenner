// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_ProtectionManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using HandlerLib.NFC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_ProtectionManager
  {
    private S4_DeviceMemory DeviceMemory = (S4_DeviceMemory) null;
    private NfcDeviceCommands Nfc;
    private SortedList<ulong, ulong> ProtectionRanges;
    private AddressRange NfcCommandProtectionRange;
    private AddressRange IrDaCommandListRange;
    private AddressRange IrDaCommandProtectionRange;

    internal S4_ProtectionManager(S4_DeviceMemory deviceMemory, NfcDeviceCommands nfc)
    {
      this.DeviceMemory = deviceMemory;
      this.Nfc = nfc;
    }

    internal bool IsWriteEnabled(AddressRange writeRange)
    {
      if (this.ProtectionRanges == null)
        this.LoadProtectionRanges();
      foreach (KeyValuePair<ulong, ulong> protectionRange in this.ProtectionRanges)
      {
        if ((ulong) writeRange.StartAddress >= protectionRange.Key && (ulong) writeRange.EndAddress <= protectionRange.Value)
          return true;
      }
      return false;
    }

    internal void CheckWriteEnabled(List<AddressRange> writeRanges)
    {
      if (this.ProtectionRanges == null)
        this.LoadProtectionRanges();
      foreach (AddressRange writeRange in writeRanges)
      {
        if (!this.IsWriteEnabled(writeRange))
          throw new Exception("Write premisson error on range: " + writeRange.ToString());
      }
    }

    private void LoadProtectionRanges()
    {
      this.ProtectionRanges = new SortedList<ulong, ulong>();
      if (!this.DeviceMemory.IsParameterAvailable(S4_Params.writeProtectionTable))
        return;
      uint address1 = this.DeviceMemory.GetParameterAddress(S4_Params.writeProtectionTable);
      while (this.DeviceMemory.AreDataAvailable(address1, 8U))
      {
        uint uint32_1 = BitConverter.ToUInt32(this.DeviceMemory.GetData(address1, 4U), 0);
        if (uint32_1 == 0U)
          break;
        uint address2 = address1 + 4U;
        uint uint32_2 = BitConverter.ToUInt32(this.DeviceMemory.GetData(address2, 4U), 0);
        address1 = address2 + 4U;
        if (uint32_2 < uint32_1)
          throw new Exception("Illegal write protection table. rangeEndAdr < rangeStartAdr");
        foreach (KeyValuePair<ulong, ulong> protectionRange in this.ProtectionRanges)
        {
          if ((ulong) uint32_1 >= protectionRange.Key && (ulong) uint32_1 <= protectionRange.Value)
            throw new Exception("Illegal combind protection range at rangeStartAdr: 0x" + uint32_1.ToString("x08"));
          if ((ulong) uint32_2 >= protectionRange.Key && (ulong) uint32_2 <= protectionRange.Value)
            throw new Exception("Illegal combind protection range at rangeEndAdr: 0x" + uint32_2.ToString("x08"));
        }
        this.ProtectionRanges.Add((ulong) uint32_1, (ulong) uint32_2);
      }
    }

    internal async Task ReadCommandProtectionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.NfcCommandProtectionRange = this.DeviceMemory.GetParameterAddressRange(S4_Params.cmd_attributes);
      this.DeviceMemory.GarantMemoryAvailable(this.NfcCommandProtectionRange);
      await this.Nfc.ReadMemoryAsync(this.NfcCommandProtectionRange, (HandlerLib.DeviceMemory) this.DeviceMemory, progress, cancelToken);
      if (!this.DeviceMemory.IsParameterInMap(S4_Params.CommandList))
        return;
      this.IrDaCommandListRange = this.DeviceMemory.GetParameterAddressRange(S4_Params.CommandList);
      this.IrDaCommandProtectionRange = this.DeviceMemory.GetParameterAddressRange(S4_Params.CommandAttributes);
      this.DeviceMemory.GarantMemoryAvailable(this.IrDaCommandListRange);
      this.DeviceMemory.GarantMemoryAvailable(this.IrDaCommandProtectionRange);
      await this.Nfc.ReadMemoryAsync(this.IrDaCommandListRange, (HandlerLib.DeviceMemory) this.DeviceMemory, progress, cancelToken);
      await this.Nfc.ReadMemoryAsync(this.IrDaCommandProtectionRange, (HandlerLib.DeviceMemory) this.DeviceMemory, progress, cancelToken);
    }

    internal string GetCommandProtectionText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (NfcCommands nfcCommands in (NfcCommands[]) Enum.GetValues(typeof (NfcCommands)))
      {
        if (nfcCommands < NfcCommands.ErrorCommand)
        {
          byte? nullable = this.DeviceMemory.GetByte((uint) ((ulong) this.NfcCommandProtectionRange.StartAddress + (ulong) nfcCommands));
          stringBuilder.Append(nfcCommands.ToString() + " = ");
          stringBuilder.Append("0x" + nullable.Value.ToString("x02") + " ");
          if (nullable.Value == (byte) 0)
            stringBuilder.Append("-");
          else
            stringBuilder.Append("protected");
          stringBuilder.AppendLine();
        }
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("IrDa commands");
      stringBuilder.AppendLine();
      if (this.DeviceMemory.IsParameterInMap(S4_Params.CommandList))
      {
        uint startAddress1 = this.IrDaCommandListRange.StartAddress;
        uint startAddress2 = this.IrDaCommandProtectionRange.StartAddress;
        while (startAddress1 < this.IrDaCommandListRange.EndAddress)
        {
          byte? nullable1 = this.DeviceMemory.GetByte(startAddress1);
          byte? nullable2 = this.DeviceMemory.GetByte(startAddress1 + 1U);
          byte? nullable3 = this.DeviceMemory.GetByte(startAddress2);
          if (nullable1.HasValue && Enum.IsDefined(typeof (Manufacturer_FC), (object) nullable1.Value))
          {
            Manufacturer_FC manufacturerFc = (Manufacturer_FC) nullable1.Value;
            string str;
            switch (manufacturerFc)
            {
              case Manufacturer_FC.CommonRadioCommands_0x2f:
                str = ((CommonRadioCommands_EFC) nullable2.Value).ToString();
                break;
              case Manufacturer_FC.CommonMBusCommands_0x34:
                str = ((CommonMBusCommands_EFC) nullable2.Value).ToString();
                break;
              case Manufacturer_FC.CommonLoRaCommands_0x35:
                str = ((CommonLoRaCommands_EFC) nullable2.Value).ToString();
                break;
              case Manufacturer_FC.SpecialCommands_0x36:
                str = ((SpecialCommands_EFC) nullable2.Value).ToString();
                break;
              case Manufacturer_FC.CommonNBIoTCommands_0x37:
                str = ((CommonNBIoTCommands_EFC) nullable2.Value).ToString();
                break;
              default:
                str = manufacturerFc.ToString();
                break;
            }
            if (str != null && nullable3.HasValue)
            {
              stringBuilder.Append(str + " = ");
              if (((uint) nullable3.Value & 2U) > 0U)
                stringBuilder.Append("-");
              else
                stringBuilder.Append("protected");
              stringBuilder.AppendLine();
            }
          }
          startAddress1 += 12U;
          ++startAddress2;
        }
      }
      return stringBuilder.ToString();
    }
  }
}
