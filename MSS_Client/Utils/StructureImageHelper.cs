// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.StructureImageHelper
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Modules.OrdersManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Structures;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS_Client.Utils
{
  public class StructureImageHelper
  {
    public static void SetImageIconPath(
      ObservableCollection<StructureNodeDTO> nodesCollection)
    {
      foreach (StructureNodeDTO nodes in (Collection<StructureNodeDTO>) nodesCollection)
      {
        if (nodes != null)
        {
          BitmapImage bitmapImage = new BitmapImage(new Uri(nodes.NodeType.IconPath));
          bitmapImage.Freeze();
          nodes.Image = bitmapImage;
          if (StructuresHelper.Descendants(nodes).Any<StructureNodeDTO>())
          {
            foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(nodes))
            {
              StructureNodeDTO child = descendant;
              bool existsMeterWithSameSerialNumberInCurrentStructure = false;
              string name1 = child.NodeType.Name;
              StructureNodeTypeEnum structureNodeTypeEnum = StructureNodeTypeEnum.Meter;
              string str1 = structureNodeTypeEnum.ToString();
              int num;
              if (!(name1 == str1))
              {
                string name2 = child.NodeType.Name;
                structureNodeTypeEnum = StructureNodeTypeEnum.RadioMeter;
                string str2 = structureNodeTypeEnum.ToString();
                num = name2 == str2 ? 1 : 0;
              }
              else
                num = 1;
              if (num != 0)
                existsMeterWithSameSerialNumberInCurrentStructure = nodesCollection.Any<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.EntityDto != null && child.EntityDto != null && n.EntityDto.SerialNumber == child.EntityDto.SerialNumber));
              child.Image = child.SetImageNode(existsMeterWithSameSerialNumberInCurrentStructure);
              child.Image.Freeze();
            }
          }
        }
      }
    }

    public static void SetImageExecutingState(
      StructureNodeDTO node,
      ExecutingStateEnum executingStateEnum)
    {
      switch (executingStateEnum)
      {
        case ExecutingStateEnum.OK:
          string[] files1 = new string[2]
          {
            node.NodeType.IconPath,
            "pack://application:,,,/Styles;component/Images/Settings/light-green.png"
          };
          node.Image = ImageHelper.Instance.GetBitmapImageFromFiles(files1);
          break;
        case ExecutingStateEnum.NotOK:
          string[] files2 = new string[2]
          {
            node.NodeType.IconPath,
            "pack://application:,,,/Styles;component/Images/Settings/light-red.png"
          };
          node.Image = ImageHelper.Instance.GetBitmapImageFromFiles(files2);
          break;
      }
    }
  }
}
