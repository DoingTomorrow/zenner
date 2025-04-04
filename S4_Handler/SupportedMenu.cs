// Decompiled with JetBrains decompiler
// Type: S4_Handler.SupportedMenu
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace S4_Handler
{
  internal class SupportedMenu
  {
    internal string MenuName;
    internal List<ViewDefinition> ViewsList = new List<ViewDefinition>();
    internal static List<SupportedMenu> AllSupportedMenus = new List<SupportedMenu>();

    internal string Resolution
    {
      get
      {
        if (this.ViewsList == null || this.ViewsList.Count < 1)
          throw new Exception("Illegal menu definition");
        return S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[this.ViewsList[0].S0].UnitScale].DisplayString;
      }
    }

    public override string ToString() => this.MenuName + ": " + this.ViewsList[0].S0.ToString();

    internal static string GetDefaultMenuName(List<ViewDefinition> menuSelections)
    {
      string defaultMenuName = "Customized menu";
      using (List<SupportedMenu>.Enumerator enumerator = SupportedMenu.AllSupportedMenus.GetEnumerator())
      {
label_8:
        while (enumerator.MoveNext())
        {
          SupportedMenu current = enumerator.Current;
          if (current.ViewsList.Count == menuSelections.Count)
          {
            for (int index = 0; index < current.ViewsList.Count; ++index)
            {
              if (!current.ViewsList[index].IsEqual(menuSelections[index]))
                goto label_8;
            }
            defaultMenuName = current.MenuName;
            break;
          }
        }
      }
      return defaultMenuName;
    }

    internal static S4_MenuManager.DS0 GetMainViewSelection(List<ViewDefinition> menuSelections)
    {
      return menuSelections[0].S0;
    }

    internal static List<ViewDefinition> GetViewsList(string resolution, string menuName)
    {
      foreach (SupportedMenu allSupportedMenu in SupportedMenu.AllSupportedMenus)
      {
        if (allSupportedMenu.MenuName == menuName && allSupportedMenu.Resolution == resolution)
          return allSupportedMenu.ViewsList;
      }
      S4_MenuManager.ResolutionInfo allResolution;
      try
      {
        S4_MenuManager.DS0 selection = S4_MenuManager.DSel0.First<KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>>((Func<KeyValuePair<S4_MenuManager.DS0, S4_MenuManager.SelectionData0>, bool>) (x => S4_DifVif_Parameter.BaseUnitScale[x.Value.UnitScale].DisplayString == resolution)).Value.Selection;
        allResolution = S4_MenuManager.All_Resolutions[selection];
      }
      catch
      {
        throw new Exception("Selected volume resolution or unit not possible");
      }
      string displayString1 = S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[allResolution.Qm_ChangeSelector].UnitScale].DisplayString;
      foreach (SupportedMenu allSupportedMenu in SupportedMenu.AllSupportedMenus)
      {
        if (allSupportedMenu.MenuName == menuName && allSupportedMenu.Resolution == displayString1)
          return allSupportedMenu.ViewsList;
      }
      string displayString2 = S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[allResolution.GAL_ChangeSelector].UnitScale].DisplayString;
      foreach (SupportedMenu allSupportedMenu in SupportedMenu.AllSupportedMenus)
      {
        if (allSupportedMenu.MenuName == menuName && allSupportedMenu.Resolution == displayString2)
          return allSupportedMenu.ViewsList;
      }
      string displayString3 = S4_DifVif_Parameter.BaseUnitScale[S4_MenuManager.DSel0[allResolution.Qft_ChangeSelector].UnitScale].DisplayString;
      foreach (SupportedMenu allSupportedMenu in SupportedMenu.AllSupportedMenus)
      {
        if (allSupportedMenu.MenuName == menuName && allSupportedMenu.Resolution == displayString3)
          return allSupportedMenu.ViewsList;
      }
      throw new Exception("View not defined");
    }

    static SupportedMenu()
    {
      foreach (S4_MenuManager.DS0 key in S4_MenuManager.All_Resolutions.Keys)
      {
        if (S4_MenuManager.All_Resolutions[key].CanBeMainView)
        {
          S4_MenuManager.ResolutionInfo allResolution = S4_MenuManager.All_Resolutions[key];
          SupportedMenu supportedMenu1 = new SupportedMenu();
          supportedMenu1.MenuName = S4_SupportedMenues.Europe.ToString();
          supportedMenu1.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.NoTimeout, S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.FlashingSegmentTest | S4_MenuManager.DSF.DecimalFrameOn));
          supportedMenu1.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.VolumeFlowDirection, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu1.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.VolumeReturnDirection, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu1.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.Date, S4_MenuManager.DS1.Time, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu1.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.SegmentTest, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu1.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.SystemState, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          if (allResolution.BaseUnit == S4_BaseUnits.Qm)
            supportedMenu1.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.mL_0, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.TestView));
          else if (allResolution.BaseUnit == S4_BaseUnits.US_GAL)
            supportedMenu1.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.US_GAL_0_0000, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.None));
          else
            supportedMenu1.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.Qft_0_0000, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.None));
          SupportedMenu.AllSupportedMenus.Add(supportedMenu1);
          SupportedMenu supportedMenu2 = new SupportedMenu();
          supportedMenu2.MenuName = S4_SupportedMenues.USA.ToString();
          supportedMenu2.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.NoTimeout, S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.LeadingZeros | S4_MenuManager.DSF.FlashingSegmentTest | S4_MenuManager.DSF.DecimalFrameOn));
          supportedMenu2.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.VolumeFlowDirection, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu2.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.VolumeReturnDirection, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu2.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.Date, S4_MenuManager.DS1.Time, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu2.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.SegmentTest, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu2.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.SystemState, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          if (allResolution.BaseUnit == S4_BaseUnits.Qm)
            supportedMenu2.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.mL_0, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.TestView));
          else if (allResolution.BaseUnit == S4_BaseUnits.US_GAL)
            supportedMenu2.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.US_GAL_0_0000, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.None));
          else
            supportedMenu2.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.Qft_0_0000, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.None));
          SupportedMenu.AllSupportedMenus.Add(supportedMenu2);
          SupportedMenu supportedMenu3 = new SupportedMenu();
          supportedMenu3.MenuName = S4_SupportedMenues.Small.ToString();
          if (allResolution.BaseUnit == S4_BaseUnits.Qm)
            supportedMenu3.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.NoTimeout, S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.FlashingSegmentTest | S4_MenuManager.DSF.DecimalFrameOn));
          else
            supportedMenu3.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.NoTimeout, S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.LeadingZeros | S4_MenuManager.DSF.FlashingSegmentTest | S4_MenuManager.DSF.DecimalFrameOn));
          supportedMenu3.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.Date, S4_MenuManager.DS1.Time, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu3.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.SegmentTest, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu3.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.SystemState, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          if (allResolution.BaseUnit == S4_BaseUnits.Qm)
            supportedMenu3.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.mL_0, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.TestView));
          else if (allResolution.BaseUnit == S4_BaseUnits.US_GAL)
            supportedMenu3.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.US_GAL_0_0000, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.None));
          else
            supportedMenu3.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.Qft_0_0000, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.None));
          SupportedMenu.AllSupportedMenus.Add(supportedMenu3);
          SupportedMenu supportedMenu4 = new SupportedMenu();
          supportedMenu4.MenuName = S4_SupportedMenues.NoCurrentFlow.ToString();
          supportedMenu4.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.NoTimeout, S4_MenuManager.DSF.StatusOn | S4_MenuManager.DSF.FlashingSegmentTest | S4_MenuManager.DSF.DecimalFrameOn | S4_MenuManager.DSF.HideFlow));
          supportedMenu4.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.VolumeFlowDirection, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu4.ViewsList.Add(new ViewDefinition(key, S4_MenuManager.DS1.VolumeReturnDirection, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu4.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.Date, S4_MenuManager.DS1.Time, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu4.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.SegmentTest, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          supportedMenu4.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.SystemState, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Minutes_1, S4_MenuManager.DSF.None));
          if (allResolution.BaseUnit == S4_BaseUnits.Qm)
            supportedMenu4.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.mL_0, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.TestView));
          else if (allResolution.BaseUnit == S4_BaseUnits.US_GAL)
            supportedMenu4.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.US_GAL_0_0000, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.None));
          else
            supportedMenu4.ViewsList.Add(new ViewDefinition(S4_MenuManager.DS0.Qft_0_0000, S4_MenuManager.DS1.CurrentFlow, S4_MenuManager.DST.Days_1, S4_MenuManager.DSF.None));
          SupportedMenu.AllSupportedMenus.Add(supportedMenu4);
        }
      }
    }
  }
}
