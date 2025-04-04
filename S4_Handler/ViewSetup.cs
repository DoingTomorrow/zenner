// Decompiled with JetBrains decompiler
// Type: S4_Handler.ViewSetup
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler
{
  internal class ViewSetup
  {
    private string[] MainSelectionList;
    private bool IsMainView = false;
    private bool DataDefined;
    private uint FirmwareVersionValue;
    internal S4_MenuManager.DS0 MainSelection;
    internal S4_MenuManager.DS1 SecondSelection;
    internal S4_MenuManager.DST TimeoutSelection;
    internal S4_MenuManager.DSF ViewControlFlags;

    internal string MainSelectionText
    {
      get
      {
        return this.DataDefined && S4_MenuManager.DSel0.ContainsKey(this.MainSelection) ? S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[this.MainSelection].UnitScale].DisplayString : "Not defined";
      }
    }

    internal string[] MainSelectionsList => this.MainSelectionList;

    internal string SecondSelectionText
    {
      get
      {
        return this.MainSelection == S4_MenuManager.DS0.SegmentTest || S4_MenuManager.DSel0[this.MainSelection].L1_Selections.Length == 0 || new FirmwareVersion(this.FirmwareVersionValue) <= (object) "0.3.7 IUW" || !S4_MenuManager.DSel1.ContainsKey(this.SecondSelection) ? (string) null : S4_MenuManager.DSel1[this.SecondSelection].DisplayText;
      }
    }

    internal string[] SecondSelectionsList
    {
      get
      {
        return new FirmwareVersion(this.FirmwareVersionValue) <= (object) "0.3.7 IUW" ? (string[]) null : S4_MenuManager.GetSecondSelectionList(this.MainSelection);
      }
    }

    internal string TimeoutSelectionText
    {
      get
      {
        return new FirmwareVersion(this.FirmwareVersionValue) <= (object) "0.3.7 IUW" ? (string) null : S4_MenuManager.DSelT[this.TimeoutSelection].DisplayText;
      }
    }

    internal string[] TimeoutSelectionsList
    {
      get
      {
        return new FirmwareVersion(this.FirmwareVersionValue) <= (object) "0.3.7 IUW" ? (string[]) null : S4_MenuManager.GetTimeoutSelectionList();
      }
    }

    internal ViewSetup(uint firmwareVersion, string[] mainSelectionList)
    {
      this.FirmwareVersionValue = firmwareVersion;
      this.MainSelectionList = mainSelectionList;
      this.DataDefined = false;
    }

    internal ViewSetup(uint firmwareVersion, ViewDefinition viewDefinition)
    {
      this.FirmwareVersionValue = firmwareVersion;
      this.MainSelection = viewDefinition.S0;
      this.SecondSelection = viewDefinition.S1;
      this.TimeoutSelection = viewDefinition.Timeout;
      this.ViewControlFlags = viewDefinition.Flags;
      this.DataDefined = true;
    }

    internal ViewSetup(
      uint firmwareVersion,
      string[] mainSelectionList,
      ViewDefinition viewDefinition,
      S4_MenuManager.DS0 defaultVolumeView)
    {
      this.FirmwareVersionValue = firmwareVersion;
      this.MainSelectionList = mainSelectionList;
      this.MainSelection = viewDefinition.S0 != S4_MenuManager.DS0.DefaultVolUnitScale ? viewDefinition.S0 : defaultVolumeView;
      this.SecondSelection = viewDefinition.S1;
      this.TimeoutSelection = viewDefinition.Timeout;
      this.ViewControlFlags = viewDefinition.Flags;
      this.DataDefined = true;
    }

    internal ViewSetup(
      uint firmwareVersion,
      int viewIndexInMenu,
      string mainSelection,
      string secondSelection,
      string timeoutSelection,
      _UNIT_SCALE_ unitScale,
      string menuName)
    {
      try
      {
        this.FirmwareVersionValue = firmwareVersion;
        if (mainSelection == null || mainSelection == "Not defined")
        {
          this.DataDefined = false;
        }
        else
        {
          S4_MenuManager.SelectionData0 selectionData0 = unitScale == _UNIT_SCALE_.NotSet || !(mainSelection == "Default volume scale") ? S4_MenuManager.DSel0.First<KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>>((Func<KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>, bool>) (x => S4_DifVif_Parameter.BaseUnitScale[x.Value.UnitScale].DisplayString == mainSelection)).Value : S4_MenuManager.DSel0.First<KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>>((Func<KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>, bool>) (x => x.Value.UnitScale == unitScale)).Value;
          this.MainSelection = selectionData0.Selection;
          if (viewIndexInMenu == 0)
          {
            this.IsMainView = true;
            this.ViewControlFlags = menuName == null ? (S4_MenuManager.All_Resolutions[selectionData0.Selection].BaseUnit != S4_BaseUnits.Qm ? S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.LeadingZeros | S4_MenuManager.DSF.FlashingSegmentTest : S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.FlashingSegmentTest | S4_MenuManager.DSF.DecimalFrameOn) : (!(menuName == S4_SupportedMenues.USA.ToString()) ? S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.FlashingSegmentTest | S4_MenuManager.DSF.DecimalFrameOn : S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.LeadingZeros | S4_MenuManager.DSF.FlashingSegmentTest);
          }
          else
            this.ViewControlFlags = S4_MenuManager.DSF.None;
          if (new FirmwareVersion(this.FirmwareVersionValue) > (object) "0.3.7 IUW")
          {
            this.SecondSelection = this.MainSelection == S4_MenuManager.DS0.SegmentTest ? S4_MenuManager.DS1.CurrentFlow : (secondSelection != null ? S4_MenuManager.DSel1.First<KeyValuePair<S4_MenuManager.DS1, S4_MenuManager.SelectionData1>>((Func<KeyValuePair<S4_MenuManager.DS1, S4_MenuManager.SelectionData1>, bool>) (x => x.Value.DisplayText == secondSelection)).Value.Selection : (selectionData0.L1_Selections.Length != 0 && selectionData0.L1_Selections.Length != 0 ? selectionData0.L1_Selections[0] : S4_MenuManager.DS1.CurrentFlow));
            if (this.IsMainView)
              this.TimeoutSelection = S4_MenuManager.DST.NoTimeout;
            else if (timeoutSelection == null)
            {
              this.TimeoutSelection = this.MainSelection != S4_MenuManager.DS0.SegmentTest ? S4_MenuManager.DST.Minutes_3 : S4_MenuManager.DST.Seconds_30;
            }
            else
            {
              this.TimeoutSelection = S4_MenuManager.DSelT.First<KeyValuePair<S4_MenuManager.DST, S4_MenuManager.SelectionDataT>>((Func<KeyValuePair<S4_MenuManager.DST, S4_MenuManager.SelectionDataT>, bool>) (x => x.Value.DisplayText == timeoutSelection)).Value.Selection;
              if ((this.TimeoutSelection == S4_MenuManager.DST.Days_1 || this.TimeoutSelection == S4_MenuManager.DST.Days_2) && S4_MenuManager.GetBaseUnit(unitScale) == S4_BaseUnits.Qm)
                this.ViewControlFlags |= S4_MenuManager.DSF.TestView;
            }
          }
          this.DataDefined = true;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Unknown menu view selection", ex);
      }
    }

    internal ViewDefinition GetViewDefinition()
    {
      if (!(new FirmwareVersion(this.FirmwareVersionValue) <= (object) "0.3.7 IUW"))
        return new ViewDefinition(this.MainSelection, this.SecondSelection, this.TimeoutSelection, this.ViewControlFlags);
      string str = (string) null;
      int s1 = -1;
      for (int index = 0; index < S4_MenuManager.DispPool_0.GetLength(0); ++index)
      {
        if (S4_MenuManager.DispPool_0[index, 0] == this.MainSelectionText)
        {
          str = S4_MenuManager.DispPool_0[index, 1];
          break;
        }
      }
      if (str != null)
      {
        for (int index = 0; index < S4_MenuManager.DispPool_1.Length; ++index)
        {
          if (S4_MenuManager.DispPool_1[index] == str)
          {
            s1 = index;
            break;
          }
        }
      }
      if (s1 < 0)
        s1 = 0;
      return this.IsMainView ? new ViewDefinition(this.MainSelection, (S4_MenuManager.DS1) s1, S4_MenuManager.DST.NoTimeout, S4_MenuManager.DSF.None) : new ViewDefinition(this.MainSelection, (S4_MenuManager.DS1) s1, S4_MenuManager.DST.Seconds_3, S4_MenuManager.DSF.None);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(this.MainSelectionText);
      if (this.SecondSelectionText != null)
        stringBuilder.Append(" + " + this.SecondSelectionText);
      if (this.TimeoutSelectionText != null)
        stringBuilder.Append(" ; " + this.TimeoutSelectionText);
      return stringBuilder.ToString();
    }
  }
}
