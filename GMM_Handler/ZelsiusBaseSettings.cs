// Decompiled with JetBrains decompiler
// Type: GMM_Handler.ZelsiusBaseSettings
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;

#nullable disable
namespace GMM_Handler
{
  public class ZelsiusBaseSettings
  {
    public bool complete;
    public double PulsValueInLiterPerImpuls;
    public string EnergyUnit;
    public string VolumeUnit;
    public string VisibleEnergyUnit;
    public string VisibleVolumeUnit;
    internal double PulsValueInLiterPerImpulsUsed;
    public int VolumeUnitIndex;
    public int VolumeLinearUnitIndex;
    internal int ZelsiusVolPulsValue;
    public sbyte Vol_SumExpo;
    public int EnergyUnitIndex;
    public int EnergyLinearUnitIndex;
    public byte Energy_SumExpo;
    internal int ZelsiusEnergyPulsValue;
    internal int PowerUnitIndex;
    internal int PowerLinearUnitIndex;
    public string Input1Unit;
    public double Input1PulsValue;
    public string Input2Unit;
    public double Input2PulsValue;
    public int Input1UnitIndex;
    public int Input2UnitIndex;
    public short MBusVolumeVIF = -1;
    public short MBusFlowVIF = -1;
    public short MBusEnergieVIF = -1;
    public short MBusPowerVIF = -1;
    public short MBusInput1VIF = -1;
    public short MBusInput2VIF = -1;
    public string BaseConfig;
    internal int BaseConfigIndex;
    public ArrayList Frames;
    public SortedList MeterBaseParameter;

    public ZelsiusBaseSettings()
    {
      this.PulsValueInLiterPerImpuls = -1.0;
      this.EnergyUnit = "?";
      this.VolumeUnit = "?";
      this.Input1Unit = "?";
      this.Input2Unit = "?";
      this.VisibleEnergyUnit = "?";
      this.VisibleVolumeUnit = "?";
      this.BaseConfig = "nil";
      this.Vol_SumExpo = (sbyte) 0;
      this.Energy_SumExpo = (byte) 0;
    }

    public ZelsiusBaseSettings(bool UseDefaultValues)
    {
      if (!UseDefaultValues)
        return;
      this.PulsValueInLiterPerImpuls = 10.0;
      this.EnergyUnit = "0.000MWH";
      this.VolumeUnit = "0.000m\u00B3";
      this.Input1Unit = "m\u00B3";
      this.Input2Unit = "m\u00B3";
      this.VisibleEnergyUnit = "?";
      this.VisibleVolumeUnit = "?";
      this.BaseConfig = "nil";
      this.Vol_SumExpo = (sbyte) 0;
      this.Energy_SumExpo = (byte) 0;
    }

    public ZelsiusBaseSettings Clone()
    {
      return new ZelsiusBaseSettings(false)
      {
        complete = this.complete,
        PulsValueInLiterPerImpuls = this.PulsValueInLiterPerImpuls,
        EnergyUnit = this.EnergyUnit,
        VolumeUnit = this.VolumeUnit,
        VisibleEnergyUnit = this.VisibleEnergyUnit,
        VisibleVolumeUnit = this.VisibleVolumeUnit,
        PulsValueInLiterPerImpulsUsed = this.PulsValueInLiterPerImpulsUsed,
        VolumeUnitIndex = this.VolumeUnitIndex,
        VolumeLinearUnitIndex = this.VolumeLinearUnitIndex,
        ZelsiusVolPulsValue = this.ZelsiusVolPulsValue,
        Vol_SumExpo = this.Vol_SumExpo,
        EnergyUnitIndex = this.EnergyUnitIndex,
        EnergyLinearUnitIndex = this.EnergyLinearUnitIndex,
        Energy_SumExpo = this.Energy_SumExpo,
        ZelsiusEnergyPulsValue = this.ZelsiusEnergyPulsValue,
        PowerUnitIndex = this.PowerUnitIndex,
        PowerLinearUnitIndex = this.PowerLinearUnitIndex,
        Input1Unit = this.Input1Unit,
        Input1PulsValue = this.Input1PulsValue,
        Input2Unit = this.Input2Unit,
        Input2PulsValue = this.Input2PulsValue,
        Input1UnitIndex = this.Input1UnitIndex,
        Input2UnitIndex = this.Input2UnitIndex,
        MBusVolumeVIF = this.MBusVolumeVIF,
        MBusFlowVIF = this.MBusFlowVIF,
        MBusEnergieVIF = this.MBusEnergieVIF,
        MBusPowerVIF = this.MBusPowerVIF,
        MBusInput1VIF = this.MBusInput1VIF,
        MBusInput2VIF = this.MBusInput2VIF,
        BaseConfig = this.BaseConfig,
        BaseConfigIndex = this.BaseConfigIndex
      };
    }
  }
}
