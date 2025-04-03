// Decompiled with JetBrains decompiler
// Type: GMM_Handler.LoggerStore
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class LoggerStore : LinkBlock
  {
    internal LoggerStore(Meter MyMeterIn)
      : base(MyMeterIn, LinkBlockTypes.LoggerStore)
    {
    }

    internal bool InitialiseTheLoggerAreas()
    {
      int blockStartAddress = this.BlockStartAddress;
      foreach (IntervalAndLogger allIntervallCode in this.MyMeter.MyLinker.AllIntervallCodes)
      {
        if (allIntervallCode.MaxEntries < 0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Negative number of logger entries");
          return false;
        }
        if (allIntervallCode.MaxEntries > 0)
        {
          allIntervallCode.P_StartAddress.ValueEprom = (long) blockStartAddress;
          blockStartAddress += allIntervallCode.MaxEntries * allIntervallCode.EntrySize;
          allIntervallCode.P_EndAddress.ValueEprom = (long) blockStartAddress;
          if (!this.MyMeter.MyHandler.BaseTypeEditMode && blockStartAddress > this.MyMeter.MyIdent.extEEPSize)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Out of eeprom space");
            return false;
          }
        }
      }
      this.StartAddressOfNextBlock = blockStartAddress;
      this.MyMeter.MyIdent.extEEPUsed = this.StartAddressOfNextBlock;
      return true;
    }
  }
}
