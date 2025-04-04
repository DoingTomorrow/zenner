// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.StructureNodeDTOExtensions
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Structures;
using MSS.DTO.Meters;
using MSS.DTO.Minomat;
using MSS.DTO.Structures;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS_Client.Utils
{
  public static class StructureNodeDTOExtensions
  {
    public static BitmapImage SetImageNode(
      this StructureNodeDTO node,
      bool existsMeterWithSameSerialNumberInCurrentStructure = false)
    {
      string[] files = new string[2]
      {
        node.NodeType.IconPath,
        "pack://application:,,,/Styles;component/Images/StructureIcons/exclamation2.png"
      };
      BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/replace-meter4.png"));
      switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), node.NodeType.Name, true))
      {
        case StructureNodeTypeEnum.Location:
          return node.Entity != null ? new BitmapImage(new Uri(node.NodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files);
        case StructureNodeTypeEnum.Tenant:
          return node.Entity != null ? new BitmapImage(new Uri(node.NodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files);
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          if (node.Entity is MeterDTO entity1 && entity1.MBusStateEnum.HasValue)
            return StructuresHelper.GetImageForMeterNode(entity1.MBusStateEnum, node.NodeType, entity1.IsConfigured, entity1.SerialNumber, node.StructureType, existsMeterWithSameSerialNumberInCurrentStructure);
          StructureTypeEnum? structureType = node.StructureType;
          StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Fixed;
          if (structureType.GetValueOrDefault() == structureTypeEnum1 && structureType.HasValue)
          {
            string str1 = "pack://application:,,,/Styles;component/Images/StructureIcons/meter.png";
            string str2 = "pack://application:,,,/Styles;component/Images/StructureIcons/replace-meter4.png";
            string str3 = "pack://application:,,,/Styles;component/Images/StructureIcons/exclamation2.png";
            string str4 = "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png";
            string str5 = "pack://application:,,,/Styles;component/Images/Universal/checkmark-green.png";
            List<string> stringList = new List<string>();
            if (entity1 != null)
            {
              stringList.Add(entity1.IsReplaced ? str2 : node.NodeType.IconPath);
              if (entity1.IsError || !entity1.IsReceived)
              {
                stringList.Add(str3);
              }
              else
              {
                bool? isConfigured = entity1.IsConfigured;
                bool flag = false;
                if (isConfigured.GetValueOrDefault() == flag && isConfigured.HasValue)
                  stringList.Add(str4);
                else
                  stringList.Add(str5);
              }
            }
            else
            {
              stringList.Add(str1);
              stringList.Add(str3);
            }
            return ImageHelper.Instance.GetBitmapImageFromFiles(stringList.ToArray());
          }
          structureType = node.StructureType;
          StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Physical;
          int num;
          if ((structureType.GetValueOrDefault() == structureTypeEnum2 ? (structureType.HasValue ? 1 : 0) : 0) == 0)
          {
            structureType = node.StructureType;
            StructureTypeEnum structureTypeEnum3 = StructureTypeEnum.Logical;
            num = structureType.GetValueOrDefault() == structureTypeEnum3 ? (structureType.HasValue ? 1 : 0) : 0;
          }
          else
            num = 1;
          if (num == 0)
            return entity1 != null ? new BitmapImage(new Uri(node.NodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files);
          if (entity1 != null && entity1.IsReplaced)
            return bitmapImage;
          return entity1 != null ? new BitmapImage(new Uri(node.NodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files);
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          if (node.Entity == null)
            return new BitmapImage(new Uri(node.NodeType.IconPath));
          return !(node.Entity is MinomatSerializableDTO entity2) ? ((node.Entity as MinomatDTO).IsMaster ? new BitmapImage(new Uri(node.NodeType.IconPath)) : new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/minomats.png"))) : (entity2.IsMaster ? new BitmapImage(new Uri(node.NodeType.IconPath)) : new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/minomats.png")));
        default:
          return new BitmapImage(new Uri(node.NodeType.IconPath));
      }
    }
  }
}
