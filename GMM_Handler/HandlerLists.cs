// Decompiled with JetBrains decompiler
// Type: GMM_Handler.HandlerLists
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections.Generic;

#nullable disable
namespace GMM_Handler
{
  internal class HandlerLists
  {
    internal static SortedList<string, byte> ConsumationDataParameters;
    internal static SortedList<string, byte> InputIdentData;
    internal static SortedList<string, byte> MBusIdentData;

    internal static void GarantVarsListExists()
    {
      if (HandlerLists.ConsumationDataParameters != null)
        return;
      HandlerLists.ConsumationDataParameters = new SortedList<string, byte>();
      HandlerLists.ConsumationDataParameters.Add("DefaultFunction.In1Display", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("DefaultFunction.In1DisplayHelp", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("DefaultFunction.In2Display", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("DefaultFunction.In2DisplayHelp", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("DefaultFunction.Kaelte_EnergSum", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("DefaultFunction.Vol_VolSum", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("DefaultFunction.Waerme_EnergSum", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("Energ_KaelteEnergDisplay", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("Energ_WaermeEnergDisplay", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("In1Counter", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("In2Counter", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("MaxFlowAndPower.LastHourEnergy", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("MaxFlowAndPower.LastHourVolume", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("MaxFlowAndPower.MaxFlow", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("MaxFlowAndPower.MaxFlowAbs", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("MaxFlowAndPower.MaxFlowTimePoint", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("MaxFlowAndPower.MaxPower", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("MaxFlowAndPower.MaxPowerAbs", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("MaxFlowAndPower.MaxPowerTimePoint", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("OldIn1Counter", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("OldIn2Counter", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("Out1Counter", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("Out2Counter", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("StichtagKomplett.Input1CounterAmStichtag", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("StichtagKomplett.Input2CounterAmStichtag", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("StichtagKomplett.KaelteEnergieAmStichtag", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("StichtagKomplett.VolumenAmStichtag", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("StichtagKomplett.WaermeEnergieAmStichtag", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("TAR_EnergyDisplayTar0", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("TAR_EnergyDisplayTar1", (byte) 0);
      HandlerLists.ConsumationDataParameters.Add("Vol_VolumenDisplay", (byte) 0);
      HandlerLists.InputIdentData = new SortedList<string, byte>();
      HandlerLists.InputIdentData.Add("Inp1_IdentNumber.Inp1DeviceType", (byte) 0);
      HandlerLists.InputIdentData.Add("Inp1_IdentNumber.Inp1SerialNumber", (byte) 0);
      HandlerLists.InputIdentData.Add("Inp2_IdentNumber.Inp2DeviceType", (byte) 0);
      HandlerLists.InputIdentData.Add("Inp2_IdentNumber.Inp2SerialNumber", (byte) 0);
      HandlerLists.InputIdentData.Add("Kundennummer.KundenNr", (byte) 0);
      HandlerLists.MBusIdentData = new SortedList<string, byte>();
      HandlerLists.MBusIdentData.Add("DefaultFunction.MBu_Address", (byte) 0);
    }
  }
}
