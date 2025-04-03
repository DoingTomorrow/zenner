// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.MiConConnector
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib.NFC
{
  public class MiConConnector
  {
    private NfcSubunitCommands mySubCmds;
    private NfcDeviceCommands myCmds;
    public string MiConConnectorIdentification;

    public MiConConnector(NfcDeviceCommands myCommands)
    {
      this.myCmds = myCommands;
      this.mySubCmds = myCommands.mySubunitCommands;
    }

    public async Task<NfcCouplerCurrents> GetNFC_CouplerCurrent(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      NfcCouplerCurrents couplerCurrents = new NfcCouplerCurrents();
      ushort valueCounts = 250;
      byte[] numArray1 = await this.mySubCmds.StartCouplerCurrentMeasurement((ushort) 20, (ushort) 2, valueCounts, progress, cancelToken);
      await Task.Delay(10);
      byte[] numArray2 = await this.mySubCmds.SetRfOffAsync(progress, cancelToken);
      await Task.Delay(450);
      byte[] numArray3 = await this.mySubCmds.SetRfOnAsync(progress, cancelToken);
      await Task.Delay(450);
      DeviceIdentification deviceIdentification = await this.myCmds.ReadVersionAsync(progress, cancelToken);
      await Task.Delay(750);
      byte[] numArray4 = await this.mySubCmds.SetRfOffAsync(progress, cancelToken);
      await Task.Delay(1500);
      List<ushort> currentSamples = new List<ushort>();
      while (currentSamples.Count < (int) valueCounts)
      {
        ushort[] samples = await this.mySubCmds.GetCouplerCurrentValues(progress, cancelToken);
        if (samples.Length != 0)
        {
          currentSamples.AddRange((IEnumerable<ushort>) samples);
          samples = (ushort[]) null;
        }
        else
          break;
      }
      int i = currentSamples.Count;
      couplerCurrents.CurrentSamples = new float[currentSamples.Count];
      while (i-- > 0)
      {
        float item = (float) (1.0 - (double) currentSamples[i] / 4096.0);
        couplerCurrents.CurrentSamples[i] = item;
      }
      i = 0;
      int j = 0;
      while (i < currentSamples.Count - 1)
      {
        couplerCurrents.NfcFieldOffCurrent += (double) couplerCurrents.CurrentSamples[i];
        ++j;
        ++i;
        if ((short) ((double) couplerCurrents.CurrentSamples[i] * 100.0) > (short) 0)
          break;
      }
      couplerCurrents.NfcFieldOffCurrent /= (double) j;
      j = 0;
      while (i < currentSamples.Count - 1)
      {
        couplerCurrents.NfcFieldOnCurrent += (double) couplerCurrents.CurrentSamples[i];
        ++j;
        ++i;
        if ((short) ((double) couplerCurrents.CurrentSamples[i] * 100.0) == (short) 0)
          break;
      }
      couplerCurrents.NfcFieldOnCurrent /= (double) j;
      j = 0;
      while (i < currentSamples.Count - 1)
      {
        couplerCurrents.StandbyCurrent += (double) couplerCurrents.CurrentSamples[i];
        ++j;
        ++i;
        if ((short) ((double) couplerCurrents.CurrentSamples[i] * 100.0) > (short) 0)
          break;
      }
      couplerCurrents.StandbyCurrent /= (double) j;
      NfcCouplerCurrents nfcCouplerCurrent = couplerCurrents;
      couplerCurrents = (NfcCouplerCurrents) null;
      currentSamples = (List<ushort>) null;
      return nfcCouplerCurrent;
    }

    public async Task ResetDevice(ProgressHandler progress, CancellationToken cancelToken)
    {
      await this.mySubCmds.MiConConnector_Reset(progress, cancelToken);
    }

    public async Task ReadMemory(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = await this.mySubCmds.ReadNdcMemory_Async(addressRange.StartAddress, addressRange.ByteSize, progress, cancelToken);
      deviceMemory.GarantMemoryAvailable(addressRange);
      deviceMemory.SetData(addressRange.StartAddress, data);
      data = (byte[]) null;
    }

    public async Task WriteMemory(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] writeData = deviceMemory.GetData(addressRange);
      byte[] numArray = await this.mySubCmds.SubUnit_WriteMemory_Async(addressRange.StartAddress, writeData, progress, cancelToken);
      writeData = (byte[]) null;
    }
  }
}
