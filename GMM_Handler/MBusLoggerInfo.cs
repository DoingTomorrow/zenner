// Decompiled with JetBrains decompiler
// Type: GMM_Handler.MBusLoggerInfo
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

#nullable disable
namespace GMM_Handler
{
  public class MBusLoggerInfo
  {
    public string LoggerFunctionName = string.Empty;
    public byte LoggerNumberOfEntrys = 0;
    public int LoggerMaxNumberOfEntrys = 0;
    public int LoggerBytesPerTransmit = 0;
    public int MBusParameterLength = 0;
    public int LoggerDifVifBytesPerEntry = 0;
    public short FunctionNumber = -1;

    public MBusLoggerInfo Clone()
    {
      return new MBusLoggerInfo()
      {
        LoggerFunctionName = this.LoggerFunctionName,
        LoggerNumberOfEntrys = this.LoggerNumberOfEntrys,
        LoggerMaxNumberOfEntrys = this.LoggerMaxNumberOfEntrys,
        LoggerBytesPerTransmit = this.LoggerBytesPerTransmit,
        LoggerDifVifBytesPerEntry = this.LoggerDifVifBytesPerEntry,
        MBusParameterLength = this.MBusParameterLength,
        FunctionNumber = this.FunctionNumber
      };
    }
  }
}
