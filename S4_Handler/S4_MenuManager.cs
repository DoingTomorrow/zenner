// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_MenuManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using S4_Handler.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler
{
  internal class S4_MenuManager
  {
    internal const string CustomizedMenu = "Customized menu";
    internal const string NotDefinedView = "Not defined";
    internal const string DefaultVolumeScaleText = "Default volume scale";
    internal _UNIT_SCALE_ DefaultUnitScale;
    private S4_DeviceMemory DeviceMemory;
    private const int maxMenuIndex = 10;
    public static string[] AllMenuNames;
    public static string[] AllMenuResolutions;
    public static string[] AllBaseUnits;
    private static string[] PoolSelectionList;
    internal static string[,] DispPool_0 = new string[24, 2]
    {
      {
        "0mL",
        "0.000m\u00B3/h"
      },
      {
        "0.00L",
        "0.000m\u00B3/h"
      },
      {
        "0.0L",
        "0.000m\u00B3/h"
      },
      {
        "0.000m\u00B3",
        "0.000m\u00B3/h"
      },
      {
        "0.00m\u00B3",
        "0.00m\u00B3/h"
      },
      {
        "0.0m\u00B3",
        "0.0m\u00B3/h"
      },
      {
        "0m\u00B3",
        "0m\u00B3/h"
      },
      {
        "0m\u00B3x10",
        "0m\u00B3/h"
      },
      {
        "0m\u00B3x100",
        "0m\u00B3/h"
      },
      {
        "0.000US_GAL",
        "0.00US_GAL/min"
      },
      {
        "0.00US_GAL",
        "0.00US_GAL/min"
      },
      {
        "0.0US_GAL",
        "0.0US_GAL/min"
      },
      {
        "0US_GAL",
        "0US_GAL/min"
      },
      {
        "0US_GALx10",
        "0US_GAL/min"
      },
      {
        "0US_GALx100",
        "0US_GAL/min"
      },
      {
        "0.000ft\u00B3",
        "0.00ft\u00B3/min"
      },
      {
        "0.00ft\u00B3",
        "0.00ft\u00B3/min"
      },
      {
        "0.0ft\u00B3",
        "0.0ft\u00B3/min"
      },
      {
        "0ft\u00B3",
        "0ft\u00B3/min"
      },
      {
        "0ft\u00B3x10",
        "0ft\u00B3/min"
      },
      {
        "0ft\u00B3x100",
        "0ft\u00B3/min"
      },
      {
        "Segment test",
        ""
      },
      {
        "Date",
        "Time"
      },
      {
        "Test current",
        "Time"
      }
    };
    internal static string[] DispPool_1 = new string[11]
    {
      "0.000m\u00B3/h",
      "0.00m\u00B3/h",
      "0.0m\u00B3/h",
      "0m\u00B3/h",
      "0.00US_GAL/min",
      "0.0US_GAL/min",
      "0US_GAL/min",
      "0.00ft\u00B3/min",
      "0.0ft\u00B3/min",
      "0ft\u00B3/min",
      "Time"
    };
    private static SortedList<S4_BaseUnits, _UNIT_SCALE_> TestResolutions;
    internal static Dictionary<S4_MenuManager.DS0, S4_MenuManager.SelectionData0> DSel0;
    internal static Dictionary<S4_MenuManager.DS1, S4_MenuManager.SelectionData1> DSel1;
    internal static Dictionary<S4_MenuManager.DST, S4_MenuManager.SelectionDataT> DSelT;
    internal static Dictionary<S4_MenuManager.DS0, S4_MenuManager.ResolutionInfo> All_Resolutions;

    internal S4_MenuManager(S4_DeviceMemory deviceMemory)
    {
      this.DeviceMemory = deviceMemory;
      this.DefaultUnitScale = (_UNIT_SCALE_) this.DeviceMemory.GetParameterValue<ushort>(S4_Params.DeviceDefaultUnitScale);
    }

    internal List<ViewDefinition> GetMenuDefinition()
    {
      _UNIT_SCALE_ defaultUnitScale = (_UNIT_SCALE_) this.DeviceMemory.GetParameterValue<ushort>(S4_Params.DeviceDefaultUnitScale);
      S4_MenuManager.DS0 selection = S4_MenuManager.DSel0.Values.First<S4_MenuManager.SelectionData0>((Func<S4_MenuManager.SelectionData0, bool>) (x => x.UnitScale == defaultUnitScale)).Selection;
      List<ViewDefinition> menuDefinition = new List<ViewDefinition>();
      uint num = this.DeviceMemory.GetParameterValue<uint>(S4_Params.Disp_MenuSize);
      if (num > 10U)
        num = 10U;
      uint parameterAddress = this.DeviceMemory.GetParameterAddress(S4_Params.Disp_Menu);
      for (int index = 0; (long) index < (long) num; ++index)
      {
        byte[] data = this.DeviceMemory.GetData(parameterAddress, 4U);
        parameterAddress += 4U;
        S4_MenuManager.DS0 s0 = selection == S4_MenuManager.DS0.DefaultVolUnitScale || data[0] != (byte) 31 ? (S4_MenuManager.DS0) data[0] : selection;
        S4_MenuManager.DS1 s1 = (S4_MenuManager.DS1) data[1];
        S4_MenuManager.DST timeout = (S4_MenuManager.DST) data[2];
        S4_MenuManager.DSF flags = (S4_MenuManager.DSF) data[3];
        if (new FirmwareVersion(this.DeviceMemory.FirmwareVersion) <= (object) "0.3.7 IUW")
        {
          s1 = S4_MenuManager.DS1.Time;
          timeout = S4_MenuManager.DST.NoTimeout;
          flags = S4_MenuManager.DSF.None;
        }
        menuDefinition.Add(new ViewDefinition(s0, s1, timeout, flags));
      }
      return menuDefinition;
    }

    internal List<ViewSetup> GetMenuSetup(List<ViewDefinition> viewDefList)
    {
      S4_MenuManager.DS0 defaultVolumeView = this.DefaultUnitScale != _UNIT_SCALE_.NotSet ? S4_MenuManager.Get_DS0_From_UnitScale(this.DefaultUnitScale) : viewDefList[0].S0;
      List<ViewSetup> menuSetup = new List<ViewSetup>();
      string[] mainSelectionList = (string[]) null;
      for (int index = 0; index < viewDefList.Count; ++index)
      {
        ViewDefinition viewDef = viewDefList[index];
        if (index == 0)
          mainSelectionList = S4_MenuManager.EnsureValueInList("Not defined", S4_MenuManager.GetMainSelectionList(viewDef.S0, this.DeviceMemory.FirmwareVersion));
        menuSetup.Add(new ViewSetup(this.DeviceMemory.FirmwareVersion, mainSelectionList, viewDef, defaultVolumeView));
      }
      menuSetup.Add(new ViewSetup(this.DeviceMemory.FirmwareVersion, mainSelectionList));
      return menuSetup;
    }

    internal void SetMenuDefinition(List<ViewDefinition> viewsList)
    {
      byte[] data = new byte[4];
      uint theValue = 0;
      uint parameterAddress = this.DeviceMemory.GetParameterAddress(S4_Params.Disp_Menu);
      if (viewsList.Count == 0)
        return;
      S4_MenuManager.DS0 s0 = viewsList[0].S0;
      this.DefaultUnitScale = S4_MenuManager.DSel0[s0].UnitScale;
      this.DeviceMemory.SetParameterValue<ushort>(S4_Params.DeviceDefaultUnitScale, (ushort) this.DefaultUnitScale);
      foreach (ViewDefinition views in viewsList)
      {
        data[0] = views.S0 != s0 ? (byte) views.S0 : (byte) 31;
        data[1] = (byte) views.S1;
        data[2] = (byte) views.Timeout;
        data[3] = (byte) views.Flags;
        this.DeviceMemory.GarantMemoryAvailable(new AddressRange(parameterAddress, (uint) data.Length));
        this.DeviceMemory.SetData(parameterAddress, data);
        parameterAddress += 4U;
        ++theValue;
      }
      this.DeviceMemory.SetParameterValue<uint>(S4_Params.Disp_MenuSize, theValue);
    }

    internal string GetBaseUnitString()
    {
      return this.DefaultUnitScale == _UNIT_SCALE_.NotSet ? S4_MenuManager.GetBaseUnitString(this.GetResolution()) : S4_MenuManager.GetBaseUnitString(this.DefaultUnitScale);
    }

    internal S4_BaseUnits GetBaseUnit() => S4_MenuManager.GetBaseUnit(this.GetBaseUnitString());

    internal string GetResolution(List<ViewDefinition> menuSelections = null)
    {
      if (menuSelections == null)
        menuSelections = this.GetMenuDefinition();
      return this.DefaultUnitScale != _UNIT_SCALE_.NotSet && menuSelections[0].S0 == S4_MenuManager.DS0.DefaultVolUnitScale ? S4_DifVif_Parameter.BaseUnitScale[this.DefaultUnitScale].DisplayString : S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[menuSelections[0].S0].UnitScale].DisplayString;
    }

    internal S4_MenuManager.DS0 GetMainViewDS0()
    {
      List<ViewDefinition> menuDefinition = this.GetMenuDefinition();
      return this.DefaultUnitScale != _UNIT_SCALE_.NotSet && menuDefinition[0].S0 == S4_MenuManager.DS0.DefaultVolUnitScale ? S4_MenuManager.DSel0.First<KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>>((Func<KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>, bool>) (x => x.Value.UnitScale == this.DefaultUnitScale)).Key : menuDefinition[0].S0;
    }

    internal string SetMenuSetup(string baseUnitString, string resolution, string menuName)
    {
      if (menuName == null)
        throw new ArgumentNullException(nameof (menuName));
      if (resolution == null)
        resolution = this.GetResolution();
      resolution = S4_MenuManager.GarantResolutonForBaseUnitString(baseUnitString, resolution);
      this.SetMenuDefinition(SupportedMenu.GetViewsList(resolution, menuName));
      return resolution;
    }

    private static string GarantResolutonForBaseUnitString(string baseUnitString, string resolution)
    {
      if (string.IsNullOrEmpty(resolution))
        throw new ArgumentNullException(nameof (resolution));
      if (baseUnitString != null && baseUnitString != S4_MenuManager.GetBaseUnitString(resolution))
      {
        S4_BaseUnits baseUnit = S4_MenuManager.GetBaseUnit(baseUnitString);
        _UNIT_SCALE_ unitScale = S4_DifVif_Parameter.BaseUnitScale.Values.First<BaseUnitScaleValues>((Func<BaseUnitScaleValues, bool>) (x => x.DisplayString == resolution)).ScaleUnit;
        S4_MenuManager.DS0 selection = S4_MenuManager.DSel0.Values.First<S4_MenuManager.SelectionData0>((Func<S4_MenuManager.SelectionData0, bool>) (x => x.UnitScale == unitScale)).Selection;
        S4_MenuManager.ResolutionInfo allResolution = S4_MenuManager.All_Resolutions[selection];
        switch (baseUnit)
        {
          case S4_BaseUnits.Qm:
            resolution = S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[allResolution.Qm_ChangeSelector].UnitScale].DisplayString;
            break;
          case S4_BaseUnits.US_GAL:
            resolution = S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[allResolution.GAL_ChangeSelector].UnitScale].DisplayString;
            break;
          case S4_BaseUnits.Qft:
            resolution = S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[allResolution.Qft_ChangeSelector].UnitScale].DisplayString;
            break;
        }
      }
      return resolution;
    }

    internal string SetMenuSetup(string resolution, string baseUnitString, List<ViewSetup> vSet)
    {
      string resolution1 = this.GetResolution();
      S4_MenuManager.GetBaseUnitString(resolution1);
      S4_MenuManager.DS0 mainViewDs0 = this.GetMainViewDS0();
      if (resolution == null && baseUnitString == null)
        resolution = resolution1;
      else if (baseUnitString != null)
        resolution = resolution != null ? S4_MenuManager.GarantResolutonForBaseUnitString(baseUnitString, resolution) : S4_MenuManager.GarantResolutonForBaseUnitString(baseUnitString, resolution1);
      _UNIT_SCALE_ unitScale = S4_DifVif_Parameter.BaseUnitScale.Values.First<BaseUnitScaleValues>((Func<BaseUnitScaleValues, bool>) (x => x.DisplayString == resolution)).ScaleUnit;
      S4_MenuManager.DS0 selection = S4_MenuManager.DSel0.Values.First<S4_MenuManager.SelectionData0>((Func<S4_MenuManager.SelectionData0, bool>) (x => x.UnitScale == unitScale)).Selection;
      List<ViewDefinition> viewsList = new List<ViewDefinition>();
      for (int index = 0; index < vSet.Count; ++index)
      {
        ViewSetup v = vSet[index];
        if (v.MainSelectionText == null || v.MainSelectionText == "Not defined")
        {
          vSet.RemoveAt(index);
          --index;
        }
        else
        {
          if (vSet[index].MainSelection == mainViewDs0)
            vSet[index].MainSelection = selection;
          viewsList.Add(v.GetViewDefinition());
        }
      }
      this.SetMenuDefinition(viewsList);
      return resolution;
    }

    static S4_MenuManager()
    {
      S4_MenuManager.DS1[] l1_Selections = new S4_MenuManager.DS1[7]
      {
        S4_MenuManager.DS1.CurrentFlow,
        S4_MenuManager.DS1.VolumeFlowDirection,
        S4_MenuManager.DS1.VolumeReturnDirection,
        S4_MenuManager.DS1.VolumeLastMonth,
        S4_MenuManager.DS1.VolumeLastHalfMonth,
        S4_MenuManager.DS1.VolumeKeyDate,
        S4_MenuManager.DS1.Time
      };
      S4_MenuManager.DSel0 = new Dictionary<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>();
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.mL_0, _UNIT_SCALE_.U0_001L, new S4_MenuManager.DS1[1]
      {
        S4_MenuManager.DS1.CurrentFlow
      });
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qm_0_0000, _UNIT_SCALE_.U0_1L, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qm_0_000, _UNIT_SCALE_.U0_001qm, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qm_0_00, _UNIT_SCALE_.U0_01qm, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qm_0_0, _UNIT_SCALE_.U0_1qm, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qm_0, _UNIT_SCALE_.U1_0qm, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qm_0x10, _UNIT_SCALE_.U10_0qm, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qm_0x100, _UNIT_SCALE_.U100_0qm, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.US_GAL_0_EXP_MIN_6, _UNIT_SCALE_.U0_000001USgal, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.US_GAL_0_0000, _UNIT_SCALE_.U0_0001USgal, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.US_GAL_0_000, _UNIT_SCALE_.U0_001USgal, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.US_GAL_0_00, _UNIT_SCALE_.U0_01USgal, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.US_GAL_0_0, _UNIT_SCALE_.U0_1USgal, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.US_GAL_0, _UNIT_SCALE_.U1_0USgal, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.US_GAL_0x10, _UNIT_SCALE_.U10_0USgal, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.US_GAL_0x100, _UNIT_SCALE_.U100_0USgal, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qft_0_EXP_MIN_6, _UNIT_SCALE_.U0_000001cft, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qft_0_0000, _UNIT_SCALE_.U0_0001cft, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qft_0_000, _UNIT_SCALE_.U0_001cft, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qft_0_00, _UNIT_SCALE_.U0_01cft, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qft_0_0, _UNIT_SCALE_.U0_1cft, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qft_0, _UNIT_SCALE_.U1_0cft, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qft_0x10, _UNIT_SCALE_.U10_0cft, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Qft_0x100, _UNIT_SCALE_.U100_0cft, l1_Selections);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.SegmentTest, _UNIT_SCALE_.U_segment_test, new S4_MenuManager.DS1[0]);
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Date, _UNIT_SCALE_.U_date, new S4_MenuManager.DS1[2]
      {
        S4_MenuManager.DS1.Time,
        S4_MenuManager.DS1.KeyDate
      });
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.TestCurrent, _UNIT_SCALE_.U_system_current, new S4_MenuManager.DS1[1]
      {
        S4_MenuManager.DS1.TestCurrent
      });
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.Identification, _UNIT_SCALE_.U_identification, new S4_MenuManager.DS1[3]
      {
        S4_MenuManager.DS1.SerialNumber,
        S4_MenuManager.DS1.DevEUI_Low,
        S4_MenuManager.DS1.DevEUI_High
      });
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.SpecialFlow, _UNIT_SCALE_.U_specialflow, new S4_MenuManager.DS1[12]
      {
        S4_MenuManager.DS1.FlowMaxThisHour,
        S4_MenuManager.DS1.FlowMinThisHour,
        S4_MenuManager.DS1.FlowMaxLastHour,
        S4_MenuManager.DS1.FlowMinLastHour,
        S4_MenuManager.DS1.FlowMaxToday,
        S4_MenuManager.DS1.FlowMinToday,
        S4_MenuManager.DS1.FlowMaxYesterday,
        S4_MenuManager.DS1.FlowMinYesterday,
        S4_MenuManager.DS1.FlowMaxThisMonth,
        S4_MenuManager.DS1.FlowMinThisMonth,
        S4_MenuManager.DS1.FlowMaxLastMonth,
        S4_MenuManager.DS1.FlowMinLastMonth
      });
      S4_MenuManager.SetDisplaySelection0(S4_MenuManager.DS0.SystemState, _UNIT_SCALE_.U_sys_state, new S4_MenuManager.DS1[0]);
      S4_MenuManager.DSel1 = new Dictionary<S4_MenuManager.DS1, S4_MenuManager.SelectionData1>();
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.Time, "Device clock time");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.CurrentFlow, "Current flow");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.VolumeFlowDirection, "Volume flow dir. (LCD:FL)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.VolumeReturnDirection, "Volume return dir. (LCD:rE)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.VolumeLastMonth, "Volume last month (LCD:L0)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.VolumeLastHalfMonth, "Volume last half month (LCD:Lh0)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.VolumeKeyDate, "Volume on key date (LCD:K)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.KeyDate, "Key date (LCD:K)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.TestCurrent, "Test current (LCD:Curr)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.SerialNumber, "Serial number (LCD:Snr)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.DevEUI_Low, "LoDevEUI lower bytes (LCD:dEu)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.DevEUI_High, "DevEUI higher byteds (LCD:EuH)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMaxThisHour, "Flow max. this hour (LCD:FLo h  ¯¯)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMinThisHour, "Flow min. this hour (LCD:FLo h  __)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMaxLastHour, "Flow max. last hour (LCD:FLo h2 ¯¯)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMinLastHour, "Flow min. last hour (LCD:FLo h2 __)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMaxToday, "Flow max. today (LCD:FLo d  ¯¯)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMinToday, "Flow min. today (LCD:FLo d  __)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMaxYesterday, "Flow max. yesterday (LCD:FLo d2 --)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMinYesterday, "Flow min. yesterday (LCD:FLo d2 __)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMaxThisMonth, "Flow max. this month (LCD:FLo nn --)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMinThisMonth, "Flow min. this month (LCD:FLo nn __)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMaxLastMonth, "Flow max. last month (LCD:FLo nn2¯¯)");
      S4_MenuManager.SetDisplaySelection1(S4_MenuManager.DS1.FlowMinLastMonth, "Flow min. last month (LCD:FLo nn2__)");
      S4_MenuManager.DSelT = new Dictionary<S4_MenuManager.DST, S4_MenuManager.SelectionDataT>();
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.NoTimeout, "0");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Seconds_3, "3 Seconds");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Seconds_5, "5 Seconds");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Seconds_10, "10 Seconds");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Seconds_30, "30 Seconds");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Minutes_1, "1 Minute");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Minutes_3, "3 Minutes");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Minutes_10, "10 Minutes");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Hours_1, "1 Hour");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Hours_3, "3 Hours");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Hours_10, "10 Hour");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Days_1, "1 Day");
      S4_MenuManager.SetTimeoutSelection(S4_MenuManager.DST.Days_2, "2 Days");
      S4_MenuManager.All_Resolutions = new Dictionary<S4_MenuManager.DS0, S4_MenuManager.ResolutionInfo>();
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.mL_0, true, S4_BaseUnits.Qm, false, S4_MenuManager.DS0.mL_0, S4_MenuManager.DS0.US_GAL_0_EXP_MIN_6, S4_MenuManager.DS0.Qft_0_EXP_MIN_6);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qm_0_0000, true, S4_BaseUnits.Qm, true, S4_MenuManager.DS0.Qm_0_0000, S4_MenuManager.DS0.US_GAL_0_0000, S4_MenuManager.DS0.Qft_0_0000);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qm_0_000, true, S4_BaseUnits.Qm, true, S4_MenuManager.DS0.Qm_0_000, S4_MenuManager.DS0.US_GAL_0_000, S4_MenuManager.DS0.Qft_0_000);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qm_0_00, true, S4_BaseUnits.Qm, true, S4_MenuManager.DS0.Qm_0_00, S4_MenuManager.DS0.US_GAL_0_00, S4_MenuManager.DS0.Qft_0_00);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qm_0_0, true, S4_BaseUnits.Qm, true, S4_MenuManager.DS0.Qm_0_0, S4_MenuManager.DS0.US_GAL_0_0, S4_MenuManager.DS0.Qft_0_0);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qm_0, true, S4_BaseUnits.Qm, true, S4_MenuManager.DS0.Qm_0, S4_MenuManager.DS0.US_GAL_0, S4_MenuManager.DS0.Qft_0);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qm_0x10, true, S4_BaseUnits.Qm, true, S4_MenuManager.DS0.Qm_0x10, S4_MenuManager.DS0.US_GAL_0x10, S4_MenuManager.DS0.Qft_0x10);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qm_0x100, true, S4_BaseUnits.Qm, true, S4_MenuManager.DS0.Qm_0x100, S4_MenuManager.DS0.US_GAL_0x100, S4_MenuManager.DS0.Qft_0x100);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.US_GAL_0_EXP_MIN_6, true, S4_BaseUnits.US_GAL, false, S4_MenuManager.DS0.mL_0, S4_MenuManager.DS0.US_GAL_0_EXP_MIN_6, S4_MenuManager.DS0.Qft_0_EXP_MIN_6);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.US_GAL_0_0000, true, S4_BaseUnits.US_GAL, true, S4_MenuManager.DS0.Qm_0_0000, S4_MenuManager.DS0.US_GAL_0_0000, S4_MenuManager.DS0.Qft_0_0000);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.US_GAL_0_000, true, S4_BaseUnits.US_GAL, true, S4_MenuManager.DS0.Qm_0_000, S4_MenuManager.DS0.US_GAL_0_000, S4_MenuManager.DS0.Qft_0_000);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.US_GAL_0_00, true, S4_BaseUnits.US_GAL, true, S4_MenuManager.DS0.Qm_0_00, S4_MenuManager.DS0.US_GAL_0_00, S4_MenuManager.DS0.Qft_0_00);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.US_GAL_0_0, true, S4_BaseUnits.US_GAL, true, S4_MenuManager.DS0.Qm_0_0, S4_MenuManager.DS0.US_GAL_0_0, S4_MenuManager.DS0.Qft_0_0);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.US_GAL_0, true, S4_BaseUnits.US_GAL, true, S4_MenuManager.DS0.Qm_0, S4_MenuManager.DS0.US_GAL_0, S4_MenuManager.DS0.Qft_0);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.US_GAL_0x10, true, S4_BaseUnits.US_GAL, true, S4_MenuManager.DS0.Qm_0x10, S4_MenuManager.DS0.US_GAL_0x10, S4_MenuManager.DS0.Qft_0x10);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.US_GAL_0x100, true, S4_BaseUnits.US_GAL, true, S4_MenuManager.DS0.Qm_0x100, S4_MenuManager.DS0.US_GAL_0x100, S4_MenuManager.DS0.Qft_0x100);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qft_0_EXP_MIN_6, true, S4_BaseUnits.Qft, false, S4_MenuManager.DS0.mL_0, S4_MenuManager.DS0.US_GAL_0_EXP_MIN_6, S4_MenuManager.DS0.Qft_0_EXP_MIN_6);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qft_0_0000, true, S4_BaseUnits.Qft, true, S4_MenuManager.DS0.Qm_0_0000, S4_MenuManager.DS0.US_GAL_0_0000, S4_MenuManager.DS0.Qft_0_0000);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qft_0_000, true, S4_BaseUnits.Qft, true, S4_MenuManager.DS0.Qm_0_000, S4_MenuManager.DS0.US_GAL_0_000, S4_MenuManager.DS0.Qft_0_000);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qft_0_00, true, S4_BaseUnits.Qft, true, S4_MenuManager.DS0.Qm_0_00, S4_MenuManager.DS0.US_GAL_0_00, S4_MenuManager.DS0.Qft_0_00);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qft_0_0, true, S4_BaseUnits.Qft, true, S4_MenuManager.DS0.Qm_0_0, S4_MenuManager.DS0.US_GAL_0_0, S4_MenuManager.DS0.Qft_0_0);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qft_0, true, S4_BaseUnits.Qft, true, S4_MenuManager.DS0.Qm_0, S4_MenuManager.DS0.US_GAL_0, S4_MenuManager.DS0.Qft_0);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qft_0x10, true, S4_BaseUnits.Qft, true, S4_MenuManager.DS0.Qm_0x10, S4_MenuManager.DS0.US_GAL_0x10, S4_MenuManager.DS0.Qft_0x10);
      S4_MenuManager.SetResolutionInfo(S4_MenuManager.DS0.Qft_0x100, true, S4_BaseUnits.Qft, true, S4_MenuManager.DS0.Qm_0x100, S4_MenuManager.DS0.US_GAL_0x100, S4_MenuManager.DS0.Qft_0x100);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      for (int index = 0; index < SupportedMenu.AllSupportedMenus.Count; ++index)
      {
        SupportedMenu allSupportedMenu = SupportedMenu.AllSupportedMenus[index];
        string displayString = S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[allSupportedMenu.ViewsList[0].S0].UnitScale].DisplayString;
        if (!stringList2.Contains(displayString))
          stringList2.Add(displayString);
        if (!stringList1.Contains(allSupportedMenu.MenuName))
          stringList1.Add(allSupportedMenu.MenuName);
      }
      S4_MenuManager.AllMenuNames = stringList1.ToArray();
      S4_MenuManager.AllMenuResolutions = stringList2.ToArray();
      S4_MenuManager.AllBaseUnits = new string[3]
      {
        "m\u00B3",
        "US_GAL",
        "ft\u00B3"
      };
      S4_MenuManager.PoolSelectionList = new string[S4_MenuManager.DispPool_0.GetLength(0)];
      for (int index = 0; index < S4_MenuManager.PoolSelectionList.Length; ++index)
        S4_MenuManager.PoolSelectionList[index] = S4_MenuManager.DispPool_0[index, 0];
      S4_MenuManager.TestResolutions = new SortedList<S4_BaseUnits, _UNIT_SCALE_>();
      S4_MenuManager.TestResolutions.Add(S4_BaseUnits.Qm, _UNIT_SCALE_.U0_000001qmh);
      S4_MenuManager.TestResolutions.Add(S4_BaseUnits.US_GAL, _UNIT_SCALE_.U0_0001USgal);
      S4_MenuManager.TestResolutions.Add(S4_BaseUnits.Qft, _UNIT_SCALE_.U0_0001cft);
    }

    internal static string GetBaseUnitString(string resolution)
    {
      if (string.IsNullOrEmpty(resolution) || resolution[0] != '0')
        return (string) null;
      if (resolution.Contains("m\u00B3"))
        return "m\u00B3";
      if (resolution.Contains("US_GAL"))
        return "US_GAL";
      return resolution.Contains("ft\u00B3") ? "ft\u00B3" : "m\u00B3";
    }

    internal static string GetBaseUnitString(_UNIT_SCALE_ unitScale)
    {
      if (unitScale < ~_UNIT_SCALE_.NotSet)
        throw new Exception("Base unit not defined");
      if (unitScale <= _UNIT_SCALE_.U100_0qm)
        return "m\u00B3";
      if (unitScale <= _UNIT_SCALE_.U100_0USgal)
        return "US_GAL";
      if (unitScale <= _UNIT_SCALE_.U100_0cft)
        return "ft\u00B3";
      throw new Exception("Base unit not a volume definition");
    }

    internal static S4_BaseUnits GetBaseUnit(_UNIT_SCALE_ unitScale)
    {
      return S4_MenuManager.GetBaseUnit(S4_MenuManager.GetBaseUnitString(unitScale));
    }

    internal static S4_BaseUnits GetBaseUnit(string baseUnitString)
    {
      switch (baseUnitString)
      {
        case "m\u00B3":
          return S4_BaseUnits.Qm;
        case "US_GAL":
          return S4_BaseUnits.US_GAL;
        case "ft\u00B3":
          return S4_BaseUnits.Qft;
        default:
          throw new Exception("Illegal base unit");
      }
    }

    internal static string[] GetMainSelectionList(S4_MenuManager.DS0 mainSelection, uint firmware)
    {
      FirmwareVersion firmwareVersion = new FirmwareVersion(firmware);
      List<string> stringList = new List<string>();
      if (firmwareVersion >= (object) "1.1.1 IUW")
        stringList.Add("Default volume scale");
      foreach (KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0> keyValuePair in S4_MenuManager.DSel0)
        stringList.Add(S4_DifVif_Parameter.BaseUnitScale[keyValuePair.Value.UnitScale].DisplayString);
      if (firmwareVersion <= (object) "0.3.7 IUW")
      {
        for (int index1 = 0; index1 < stringList.Count; ++index1)
        {
          bool flag = false;
          for (int index2 = 0; index2 < S4_MenuManager.DispPool_0.GetLength(0); ++index2)
          {
            if (S4_MenuManager.DispPool_0[index2, 0] == stringList[index1])
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            stringList.RemoveAt(index1);
            --index1;
          }
        }
      }
      return stringList.ToArray();
    }

    internal static string[] GetSecondSelectionList(S4_MenuManager.DS0 mainSelection)
    {
      string[] secondSelectionList = new string[S4_MenuManager.DSel0[mainSelection].L1_Selections.Length];
      for (int index = 0; index < S4_MenuManager.DSel0[mainSelection].L1_Selections.Length; ++index)
        secondSelectionList[index] = S4_MenuManager.DSel1[S4_MenuManager.DSel0[mainSelection].L1_Selections[index]].DisplayText;
      return secondSelectionList;
    }

    internal static string[] GetTimeoutSelectionList()
    {
      string[] timeoutSelectionList = new string[S4_MenuManager.DSelT.Count];
      for (int index = 0; index < S4_MenuManager.DSelT.Count; ++index)
        timeoutSelectionList[index] = S4_MenuManager.DSelT.ElementAt<KeyValuePair<S4_MenuManager.DST, S4_MenuManager.SelectionDataT>>(index).Value.DisplayText;
      return timeoutSelectionList;
    }

    private static void SetDisplaySelection0(
      S4_MenuManager.DS0 selection,
      _UNIT_SCALE_ unitScale,
      S4_MenuManager.DS1[] l1_Selections)
    {
      S4_MenuManager.DSel0.Add(selection, new S4_MenuManager.SelectionData0(selection, unitScale, l1_Selections));
    }

    internal static S4_MenuManager.DS0 Get_DS0_From_UnitScale(_UNIT_SCALE_ unitScale)
    {
      return S4_MenuManager.DSel0.Values.First<S4_MenuManager.SelectionData0>((Func<S4_MenuManager.SelectionData0, bool>) (x => x.UnitScale == unitScale)).Selection;
    }

    private static void SetDisplaySelection1(S4_MenuManager.DS1 selection, string displayText)
    {
      S4_MenuManager.DSel1.Add(selection, new S4_MenuManager.SelectionData1(selection, displayText));
    }

    private static void SetTimeoutSelection(S4_MenuManager.DST selection, string displayText)
    {
      S4_MenuManager.DSelT.Add(selection, new S4_MenuManager.SelectionDataT(selection, displayText));
    }

    private static void SetResolutionInfo(
      S4_MenuManager.DS0 unitSelector,
      bool isUnit,
      S4_BaseUnits baseUnit,
      bool canBeMainView,
      S4_MenuManager.DS0 Qm_changeSelector,
      S4_MenuManager.DS0 GAL_changeSelector,
      S4_MenuManager.DS0 Qft_changeSelector)
    {
      S4_MenuManager.ResolutionInfo resolutionInfo = new S4_MenuManager.ResolutionInfo(unitSelector, isUnit, baseUnit, canBeMainView, Qm_changeSelector, GAL_changeSelector, Qft_changeSelector);
      S4_MenuManager.All_Resolutions.Add(unitSelector, resolutionInfo);
    }

    public static string[] EnsureValueInList(string value, string[] list)
    {
      if (Array.IndexOf<string>(list, value) >= 0)
        return list;
      string[] destinationArray = new string[list.Length + 1];
      destinationArray[0] = value;
      Array.Copy((Array) list, 0, (Array) destinationArray, 1, list.Length);
      return destinationArray;
    }

    internal enum DS0
    {
      mL_0,
      L_0_00,
      Qm_0_0000,
      Qm_0_000,
      Qm_0_00,
      Qm_0_0,
      Qm_0,
      Qm_0x10,
      Qm_0x100,
      US_GAL_0_000,
      US_GAL_0_00,
      US_GAL_0_0,
      US_GAL_0,
      US_GAL_0x10,
      US_GAL_0x100,
      Qft_0_000,
      Qft_0_00,
      Qft_0_0,
      Qft_0,
      Qft_0x10,
      Qft_0x100,
      SegmentTest,
      Date,
      TestCurrent,
      US_GAL_0_EXP_MIN_6,
      US_GAL_0_0000,
      Qft_0_EXP_MIN_6,
      Qft_0_0000,
      Identification,
      SpecialFlow,
      SystemState,
      DefaultVolUnitScale,
    }

    internal enum DS1
    {
      Time,
      CurrentFlow,
      VolumeFlowDirection,
      VolumeReturnDirection,
      VolumeLastMonth,
      VolumeLastHalfMonth,
      VolumeKeyDate,
      KeyDate,
      TestCurrent,
      SerialNumber,
      DevEUI_Low,
      DevEUI_High,
      FlowMaxThisHour,
      FlowMinThisHour,
      FlowMaxLastHour,
      FlowMinLastHour,
      FlowMaxToday,
      FlowMinToday,
      FlowMaxYesterday,
      FlowMinYesterday,
      FlowMaxThisMonth,
      FlowMinThisMonth,
      FlowMaxLastMonth,
      FlowMinLastMonth,
    }

    internal enum DST
    {
      NoTimeout,
      Seconds_3,
      Seconds_5,
      Seconds_10,
      Seconds_30,
      Minutes_1,
      Minutes_3,
      Minutes_10,
      Hours_1,
      Hours_3,
      Hours_10,
      Days_1,
      Days_2,
    }

    [Flags]
    internal enum DSF
    {
      None = 0,
      StatusOn = 1,
      LeadingZeros = 2,
      FlashingSegmentTest = 4,
      DecimalFrameOn = 8,
      TestView = 16, // 0x00000010
      HideFlow = 32, // 0x00000020
    }

    internal class SelectionData0
    {
      internal S4_MenuManager.DS0 Selection;
      internal _UNIT_SCALE_ UnitScale;
      internal S4_MenuManager.DS1[] L1_Selections;

      internal SelectionData0(
        S4_MenuManager.DS0 selection,
        _UNIT_SCALE_ unitScale,
        S4_MenuManager.DS1[] l1_Selections)
      {
        this.Selection = selection;
        this.UnitScale = unitScale;
        this.L1_Selections = l1_Selections;
      }

      public override string ToString() => this.Selection.ToString();
    }

    internal class SelectionData1
    {
      internal S4_MenuManager.DS1 Selection;
      internal string DisplayText;

      internal SelectionData1(S4_MenuManager.DS1 selection, string displayText)
      {
        this.Selection = selection;
        this.DisplayText = displayText;
      }

      public override string ToString() => this.Selection.ToString();
    }

    internal class SelectionDataT
    {
      internal S4_MenuManager.DST Selection;
      internal string DisplayText;

      internal SelectionDataT(S4_MenuManager.DST selection, string displayText)
      {
        this.Selection = selection;
        this.DisplayText = displayText;
      }

      public override string ToString() => this.Selection.ToString();
    }

    internal class ResolutionInfo
    {
      internal S4_MenuManager.DS0 UnitSelector;
      internal bool IsUnit;
      internal S4_BaseUnits BaseUnit;
      internal bool CanBeMainView;
      internal S4_MenuManager.DS0 Qm_ChangeSelector;
      internal S4_MenuManager.DS0 GAL_ChangeSelector;
      internal S4_MenuManager.DS0 Qft_ChangeSelector;

      internal ResolutionInfo(
        S4_MenuManager.DS0 unitSelector,
        bool isUnit,
        S4_BaseUnits baseUnit,
        bool canBeMainView,
        S4_MenuManager.DS0 Qm_changeSelector,
        S4_MenuManager.DS0 GAL_changeSelector,
        S4_MenuManager.DS0 Qft_changeSelector)
      {
        this.UnitSelector = unitSelector;
        this.IsUnit = isUnit;
        this.BaseUnit = baseUnit;
        this.CanBeMainView = canBeMainView;
        this.Qm_ChangeSelector = Qm_changeSelector;
        this.GAL_ChangeSelector = GAL_changeSelector;
        this.Qft_ChangeSelector = Qft_changeSelector;
      }
    }
  }
}
