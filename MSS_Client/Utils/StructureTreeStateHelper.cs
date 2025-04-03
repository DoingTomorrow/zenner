// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.StructureTreeStateHelper
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeListView;

#nullable disable
namespace MSS_Client.Utils
{
  public static class StructureTreeStateHelper
  {
    private static List<TreeListViewRow> GetExpandedState(RadTreeListView radTreeListView)
    {
      List<TreeListViewRow> expandedState = new List<TreeListViewRow>();
      if (radTreeListView == null)
        return expandedState;
      expandedState.AddRange(radTreeListView.ChildrenOfType<TreeListViewRow>().Where<TreeListViewRow>((Func<TreeListViewRow, bool>) (item => item.IsExpanded)));
      return expandedState;
    }

    private static void SetExpandedState(
      List<TreeListViewRow> expandedItems,
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(node))
        {
          foreach (TreeListViewRow expandedItem in expandedItems)
          {
            if (descendant.Id == ((StructureNodeDTO) expandedItem.Item).Id)
              descendant.IsExpanded = true;
          }
        }
      }
    }

    public static void MaintainExpandedState(
      RadTreeListView radTreeListView,
      ObservableCollection<StructureNodeDTO> structureNodeCollection)
    {
      StructureTreeStateHelper.SetExpandedState(StructureTreeStateHelper.GetExpandedState(radTreeListView), structureNodeCollection);
    }
  }
}
