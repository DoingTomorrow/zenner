// Decompiled with JetBrains decompiler
// Type: GMM_Handler.LoggerParameterData
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using DeviceCollector;

#nullable disable
namespace GMM_Handler
{
  public class LoggerParameterData
  {
    public int ParameterSize;
    public Parameter.BaseParameterFormat ParameterFormat;
    public string ZDF_ParameterID;
    public string ParameterUnit;
    public int UnitExponent;
    public DataBaseAccess.PValueDescription PValueDescription;

    public LoggerParameterData(Parameter BaseParameter, Meter TheMeter)
    {
      this.ParameterSize = BaseParameter.Size;
      this.ParameterFormat = BaseParameter.ParameterFormat;
      long difVifs = BaseParameter.DifVifs;
      short difVifSize = BaseParameter.DifVifSize;
      if (MBusDevice.GetZR_MBusLoggerDivVif(ref difVifs, ref difVifSize))
      {
        MBusDevice.GetZR_MBusParameterID(BaseParameter.DifVifs, BaseParameter.DifVifSize, out this.ZDF_ParameterID, out this.ParameterUnit, out this.UnitExponent);
        this.PValueDescription = TheMeter.MyHandler.MyDataBaseAccess.GetValueDescriptions(TheMeter, this.ZDF_ParameterID);
      }
      else
      {
        this.ZDF_ParameterID = "NoZDF_Param";
        this.ParameterUnit = "_";
        this.UnitExponent = 0;
      }
      if (this.PValueDescription != null)
        return;
      this.PValueDescription = new DataBaseAccess.PValueDescription();
      this.PValueDescription.ValueName = BaseParameter.NameTranslated;
      this.PValueDescription.ValueDescription = "-";
      this.PValueDescription.Unit = this.ParameterUnit;
    }
  }
}
