// Decompiled with JetBrains decompiler
// Type: S4_Handler.ScenarioEnergy
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using NLog;
using System;

#nullable disable
namespace S4_Handler
{
  public class ScenarioEnergy
  {
    internal static Logger S4_ScenarioEnergyLogger = LogManager.GetLogger("S4_ScenarioEnergy");
    private const double DaySeconds = 86400.0;
    public string PacketName;
    public double PacketBuilding_ms;
    public double SX1276_Setup_ms;
    public double Tx_Duration_ms;
    public double Rx_Duration_ms;
    public double PacketBuilding_mA;
    public double SX1276_Setup_mA;
    public double Tx_Duration_mA;
    public double Rx_Duration_mA;
    public double PacketsPerDay = 0.0;
    public ScenarioEnergy AdditionalPacket;

    public double CycleSeconds
    {
      get => 86400.0 / this.PacketsPerDay;
      set => this.PacketsPerDay = 86400.0 / value;
    }

    private ScenarioEnergy()
    {
    }

    public ScenarioEnergy(string name) => this.PacketName = name;

    public double GetYearly_mAs(out string message)
    {
      double yearlyMAs = (0.0 + this.PacketBuilding_ms * this.PacketBuilding_mA + this.SX1276_Setup_ms * this.SX1276_Setup_mA + this.Tx_Duration_ms * this.Tx_Duration_mA + this.Rx_Duration_ms * this.Rx_Duration_mA) / 3600000.0 * (this.PacketsPerDay * 365.0);
      message = "PacketsPerDay: " + this.PacketsPerDay.ToString("0.000") + Environment.NewLine + "; Energy: " + yearlyMAs.ToString("0.000") + " mAh/year";
      return yearlyMAs;
    }

    public ScenarioEnergy Clone(string name)
    {
      ScenarioEnergy scenarioEnergy = new ScenarioEnergy(name);
      scenarioEnergy.PacketName = this.PacketName;
      scenarioEnergy.PacketBuilding_ms = this.PacketBuilding_ms;
      scenarioEnergy.SX1276_Setup_ms = this.SX1276_Setup_ms;
      scenarioEnergy.Tx_Duration_ms = this.Tx_Duration_ms;
      scenarioEnergy.Rx_Duration_ms = this.Rx_Duration_ms;
      scenarioEnergy.PacketBuilding_mA = this.PacketBuilding_mA;
      scenarioEnergy.SX1276_Setup_mA = this.SX1276_Setup_mA;
      scenarioEnergy.Tx_Duration_mA = this.Tx_Duration_mA;
      scenarioEnergy.Rx_Duration_mA = this.Rx_Duration_mA;
      scenarioEnergy.PacketsPerDay = this.PacketsPerDay;
      if (this.AdditionalPacket != null)
        scenarioEnergy.AdditionalPacket = scenarioEnergy.AdditionalPacket.Clone((string) null);
      return scenarioEnergy;
    }
  }
}
