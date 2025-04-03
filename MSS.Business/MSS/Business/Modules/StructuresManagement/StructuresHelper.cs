// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.StructuresHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DTO.Meters;
using MSS.DTO.Minomat;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public static class StructuresHelper
  {
    public static void InitializeMappings()
    {
      Mapper.CreateMap<Meter, MeterSerializableDTO>().ForMember((Expression<Func<MeterSerializableDTO, object>>) (mDTO => (object) mDTO.RoomTypeId), (Action<IMemberConfigurationExpression<Meter>>) (x => x.MapFrom<Guid>((Expression<Func<Meter, Guid>>) (m => m.Room.Id)))).ForMember((Expression<Func<MeterSerializableDTO, object>>) (mDTO => (object) mDTO.ReadingUnitId), (Action<IMemberConfigurationExpression<Meter>>) (x => x.MapFrom<Guid>((Expression<Func<Meter, Guid>>) (m => m.ReadingUnit.Id)))).ForMember((Expression<Func<MeterSerializableDTO, object>>) (mDTO => (object) mDTO.ImpulsUnitId), (Action<IMemberConfigurationExpression<Meter>>) (x => x.MapFrom<Guid>((Expression<Func<Meter, Guid>>) (m => m.ImpulsUnit.Id)))).ForMember((Expression<Func<MeterSerializableDTO, object>>) (mDTO => (object) mDTO.ChannelId), (Action<IMemberConfigurationExpression<Meter>>) (x => x.MapFrom<Guid>((Expression<Func<Meter, Guid>>) (m => m.Channel.Id)))).ForMember((Expression<Func<MeterSerializableDTO, object>>) (mDTO => mDTO.RoomTypeCode), (Action<IMemberConfigurationExpression<Meter>>) (x => x.MapFrom<string>((Expression<Func<Meter, string>>) (m => m.Room.Code)))).ForMember((Expression<Func<MeterSerializableDTO, object>>) (mDTO => mDTO.ImpulsUnitCelestaCode), (Action<IMemberConfigurationExpression<Meter>>) (x => x.MapFrom<string>((Expression<Func<Meter, string>>) (m => m.ImpulsUnit.CelestaCode)))).ForMember((Expression<Func<MeterSerializableDTO, object>>) (mDTO => mDTO.ReadingUnitCelestaCode), (Action<IMemberConfigurationExpression<Meter>>) (x => x.MapFrom<string>((Expression<Func<Meter, string>>) (m => m.ReadingUnit.CelestaCode)))).ForMember((Expression<Func<MeterSerializableDTO, object>>) (mDTO => (object) mDTO.ConnectedDeviceTypeId), (Action<IMemberConfigurationExpression<Meter>>) (x => x.MapFrom<Guid>((Expression<Func<Meter, Guid>>) (m => m.ConnectedDeviceType.Id))));
      Mapper.CreateMap<Location, LocationSerializableDTO>().ForMember((Expression<Func<LocationSerializableDTO, object>>) (locDTO => (object) locDTO.ScenarioId), (Action<IMemberConfigurationExpression<Location>>) (x => x.MapFrom<Guid>((Expression<Func<Location, Guid>>) (loc => loc.Scenario.Id)))).ForMember((Expression<Func<LocationSerializableDTO, object>>) (locDTO => (object) locDTO.CountryId), (Action<IMemberConfigurationExpression<Location>>) (x => x.MapFrom<Guid>((Expression<Func<Location, Guid>>) (loc => loc.Country.Id))));
      Mapper.CreateMap<Tenant, TenantSerializableDTO>();
      Mapper.CreateMap<StructureNode, StructureNodeSerializableDTO>().ForMember((Expression<Func<StructureNodeSerializableDTO, object>>) (m => (object) m.NodeType), (Action<IMemberConfigurationExpression<StructureNode>>) (a => a.MapFrom<Guid>((Expression<Func<StructureNode, Guid>>) (e => e.NodeType.Id))));
      Mapper.CreateMap<StructureNodeLinks, StructureNodeLinksSerializableDTO>();
      Mapper.CreateMap<MSS.Core.Model.DataCollectors.Minomat, MinomatSerializableDTO>().ForMember((Expression<Func<MinomatSerializableDTO, object>>) (x => (object) x.ProviderId), (Action<IMemberConfigurationExpression<MSS.Core.Model.DataCollectors.Minomat>>) (x => x.MapFrom<Guid>((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, Guid>>) (y => y.Provider.Id))));
    }

    public static void LoadItemsInGroup(
      Group group,
      ObservableCollection<StructureNodeDTO> availableNodes)
    {
      foreach (StructureNodeDTO availableNode in (Collection<StructureNodeDTO>) availableNodes)
        group.Participants.Add(availableNode);
    }

    public static IEnumerable<StructureNodeDTO> Descendants(StructureNodeDTO root)
    {
      Stack<StructureNodeDTO> nodes = new Stack<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) new StructureNodeDTO[1]
      {
        root
      });
      while (nodes.Any<StructureNodeDTO>())
      {
        StructureNodeDTO node = nodes.Pop();
        yield return node;
        foreach (StructureNodeDTO n in (Collection<StructureNodeDTO>) node.SubNodes)
          nodes.Push(n);
        node = (StructureNodeDTO) null;
      }
    }

    public static ObservableCollection<StructureNodeDTO> GetTreeFromList(
      IList<StructureNodeType> structureNodeTypeList,
      IList<StructureNodeLinks> structureNodeLinksList,
      Dictionary<Guid, object> entitiesDictionary,
      List<string> duplicateMeterSerialNumbers = null,
      IList<MeterReplacementHistorySerializableDTO> meterReplacementHistoryList = null,
      bool loadOnDemand = false)
    {
      structureNodeLinksList = (IList<StructureNodeLinks>) structureNodeLinksList.OrderBy<StructureNodeLinks, int>((Func<StructureNodeLinks, int>) (l => l.OrderNr)).ToList<StructureNodeLinks>();
      Mapper.CreateMap<Meter, MeterDTO>();
      Mapper.CreateMap<Location, LocationDTO>();
      Mapper.CreateMap<Tenant, TenantDTO>();
      Mapper.CreateMap<MSS.Core.Model.DataCollectors.Minomat, MinomatSerializableDTO>().ForMember((Expression<Func<MinomatSerializableDTO, object>>) (x => (object) x.ProviderId), (Action<IMemberConfigurationExpression<MSS.Core.Model.DataCollectors.Minomat>>) (x => x.ResolveUsing((Func<MSS.Core.Model.DataCollectors.Minomat, object>) (y => y.Provider != null ? (object) y.Provider.Id : (object) Guid.Empty))));
      ObservableCollection<StructureNodeDTO> treeFromList = new ObservableCollection<StructureNodeDTO>();
      IList<StructureNodeLinks> list = (IList<StructureNodeLinks>) structureNodeLinksList.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (x => x.ParentNodeId == Guid.Empty && x.RootNode.Id == x.Node.Id)).ToList<StructureNodeLinks>();
      IDictionary<Guid, StructureNodeType> dictionary1 = (IDictionary<Guid, StructureNodeType>) structureNodeTypeList.ToDictionary<StructureNodeType, Guid, StructureNodeType>((Func<StructureNodeType, Guid>) (x => x.Id), (Func<StructureNodeType, StructureNodeType>) (x => x));
      Dictionary<Guid, \u003C\u003Ef__AnonymousType0<Guid, ILookup<Guid, StructureNodeLinks>>> dictionary2 = structureNodeLinksList.GroupBy((Func<StructureNodeLinks, Guid>) (p => p.RootNode.Id), (key, g) => new
      {
        TreeRootNode = key,
        TreeElements = g.ToLookup<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (x => x.ParentNodeId))
      }).ToDictionary(x => x.TreeRootNode);
      foreach (StructureNodeLinks structureNodeLink in (IEnumerable<StructureNodeLinks>) list)
      {
        StructureNodeDTO newNode = StructuresHelper.CreateNewNode(structureNodeLink, dictionary1, entitiesDictionary, meterReplacementHistoryList);
        newNode.RootNode = newNode;
        newNode.IsEmpty = false;
        treeFromList.Add(newNode);
        Guid id = newNode.Id;
        if (!loadOnDemand && dictionary2.ContainsKey(id))
        {
          ILookup<Guid, StructureNodeLinks> treeElements = dictionary2[id].TreeElements;
          StructuresHelper.FillChild(newNode, newNode.Id, treeElements, dictionary1, entitiesDictionary, duplicateMeterSerialNumbers, meterReplacementHistoryList);
        }
      }
      return treeFromList;
    }

    public static ObservableCollection<StructureNodeDTO> GetTreeFromList(
      IEnumerable<StructureNodeType> structureNodeTypeList,
      IEnumerable<StructureNodeLinks> structureNodeLinksList,
      IEnumerable<StructureNode> nodesToBeColored,
      Dictionary<Guid, object> entitiesDictionary,
      List<string> duplicateMeterSerialNumbers = null)
    {
      Mapper.CreateMap<Meter, MeterDTO>();
      Mapper.CreateMap<Location, LocationDTO>();
      Mapper.CreateMap<Tenant, TenantDTO>();
      ObservableCollection<StructureNodeDTO> treeFromList = new ObservableCollection<StructureNodeDTO>();
      IDictionary<Guid, StructureNodeType> structureNodeTypeDictionary = (IDictionary<Guid, StructureNodeType>) structureNodeTypeList.ToDictionary<StructureNodeType, Guid, StructureNodeType>((Func<StructureNodeType, Guid>) (x => x.Id), (Func<StructureNodeType, StructureNodeType>) (x => x));
      foreach (StructureNodeDTO structureNodeDto in structureNodeLinksList.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (structureNodeLink => structureNodeLink.ParentNodeId == Guid.Empty && structureNodeLink.RootNode.Id == structureNodeLink.Node.Id)).Select<StructureNodeLinks, StructureNodeDTO>((Func<StructureNodeLinks, StructureNodeDTO>) (structureNodeLink => StructuresHelper.CreateNewNode(structureNodeLink, structureNodeTypeDictionary, entitiesDictionary))))
      {
        StructureNodeDTO parentNode = structureNodeDto;
        if (nodesToBeColored.Any<StructureNode>((Func<StructureNode, bool>) (u => u.Id == parentNode.Id)))
          parentNode.BackgroundColor = (System.Windows.Media.Brush) System.Windows.Media.Brushes.LightGreen;
        parentNode.RootNode = parentNode;
        treeFromList.Add(parentNode);
        Guid id = parentNode.Id;
        StructuresHelper.FillChild(parentNode, id, parentNode.Id, structureNodeLinksList, structureNodeTypeDictionary, nodesToBeColored, entitiesDictionary, duplicateMeterSerialNumbers);
      }
      return treeFromList;
    }

    public static void FillChild(
      StructureNodeDTO parentNode,
      Guid parentNodeId,
      ILookup<Guid, StructureNodeLinks> currentTreeLookup,
      IDictionary<Guid, StructureNodeType> structureNodeTypeDictionary,
      Dictionary<Guid, object> entitiesDictionary,
      List<string> duplicateMeterSerialNumbers,
      IList<MeterReplacementHistorySerializableDTO> meterReplacementHistoryList = null)
    {
      if (!currentTreeLookup.Contains(parentNodeId))
        return;
      foreach (StructureNodeLinks structureNodeLink in currentTreeLookup[parentNodeId])
      {
        StructureNodeDTO childNode = StructuresHelper.CreateNewNode(structureNodeLink, structureNodeTypeDictionary, entitiesDictionary, meterReplacementHistoryList);
        if (childNode.NodeType.Name == StructureNodeTypeEnum.Tenant.ToString())
        {
          SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 237, (byte) 237, (byte) 237));
          solidColorBrush.Freeze();
          childNode.BackgroundColor = (System.Windows.Media.Brush) solidColorBrush;
        }
        if (duplicateMeterSerialNumbers != null && duplicateMeterSerialNumbers.Count > 0 && (childNode.NodeType.Name == "Meter" || childNode.NodeType.Name == "RadioMeter") && duplicateMeterSerialNumbers.Contains(childNode.Entity is MeterDTO entity ? entity.SerialNumber : (string) null))
          childNode.IsDuplicate = true;
        childNode.IsEmpty = !currentTreeLookup.Any<IGrouping<Guid, StructureNodeLinks>>((Func<IGrouping<Guid, StructureNodeLinks>, bool>) (v => v.Any<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (l => l.ParentNodeId == childNode.Id && !l.EndDate.HasValue))));
        parentNode.SubNodes.Add(childNode);
        StructuresHelper.FillChild(childNode, childNode.Id, currentTreeLookup, structureNodeTypeDictionary, entitiesDictionary, duplicateMeterSerialNumbers, meterReplacementHistoryList);
      }
    }

    public static void FillChild(
      StructureNodeDTO parentNode,
      Guid rootNodeId,
      Guid parentNodeId,
      IEnumerable<StructureNodeLinks> structureNodeLinksList,
      IDictionary<Guid, StructureNodeType> structureNodeTypeDictionary,
      IEnumerable<StructureNode> nodesToBeColored,
      Dictionary<Guid, object> entitiesDictionary,
      List<string> duplicateMeterSerialNumbers)
    {
      IEnumerable<StructureNodeLinks> source = structureNodeLinksList.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => s.ParentNodeId == parentNodeId && s.RootNode.Id == rootNodeId));
      if (!source.Any<StructureNodeLinks>())
        return;
      foreach (StructureNodeDTO structureNodeDto in source.Select<StructureNodeLinks, StructureNodeDTO>((Func<StructureNodeLinks, StructureNodeDTO>) (structureNode => StructuresHelper.CreateNewNode(structureNode, structureNodeTypeDictionary, entitiesDictionary))))
      {
        StructureNodeDTO childNode = structureNodeDto;
        if (nodesToBeColored.Any<StructureNode>((Func<StructureNode, bool>) (u => u.Id == childNode.Id)))
          childNode.BackgroundColor = (System.Windows.Media.Brush) System.Windows.Media.Brushes.LightGreen;
        string name1 = childNode.NodeType.Name;
        StructureNodeTypeEnum structureNodeTypeEnum = StructureNodeTypeEnum.Tenant;
        string str1 = structureNodeTypeEnum.ToString();
        if (name1 == str1)
        {
          SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 237, (byte) 237, (byte) 237));
          solidColorBrush.Freeze();
          childNode.BackgroundColor = (System.Windows.Media.Brush) solidColorBrush;
        }
        if (duplicateMeterSerialNumbers != null && duplicateMeterSerialNumbers.Count > 0)
        {
          string name2 = childNode.NodeType.Name;
          structureNodeTypeEnum = StructureNodeTypeEnum.Meter;
          string str2 = structureNodeTypeEnum.ToString();
          if (name2 == str2 && duplicateMeterSerialNumbers.Contains((childNode.Entity as MeterDTO).SerialNumber))
            childNode.IsDuplicate = true;
        }
        parentNode.SubNodes.Add(childNode);
        StructuresHelper.FillChild(childNode, rootNodeId, childNode.Id, structureNodeLinksList, structureNodeTypeDictionary, nodesToBeColored, entitiesDictionary, duplicateMeterSerialNumbers);
      }
    }

    public static StructureNodeDTO CreateNewNode(
      StructureNodeLinks structureNodeLink,
      IDictionary<Guid, StructureNodeType> structureNodeTypeDictionary,
      Dictionary<Guid, object> entitiesDictionary,
      IList<MeterReplacementHistorySerializableDTO> meterReplacementHistoryList = null)
    {
      StructureNodeDTO node = new StructureNodeDTO();
      if (structureNodeLink.Node != null)
      {
        node.Id = structureNodeLink.Node.Id;
        node.Name = structureNodeLink.Node.Name;
        node.Description = structureNodeLink.Node.Description;
        node.StructureType = new StructureTypeEnum?(structureNodeLink.StructureType);
        node.OrderNr = structureNodeLink.OrderNr;
        node.IsDuplicate = false;
        if (structureNodeTypeDictionary.ContainsKey(structureNodeLink.Node.NodeType.Id))
        {
          StructureNodeType structureNodeType = structureNodeTypeDictionary[structureNodeLink.Node.NodeType.Id];
          node.NodeType = structureNodeType;
          if (structureNodeLink.Node.EntityId != Guid.Empty && entitiesDictionary.ContainsKey(structureNodeLink.Node.EntityId))
          {
            object entities = entitiesDictionary[structureNodeLink.Node.EntityId];
            if (entities != null)
            {
              object entityDto = StructuresHelper.GetEntityDTO((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeType.Name, true), entities);
              if (entityDto is MeterDTO meterDto && meterReplacementHistoryList != null)
                StructuresHelper.UpdateMeterReplacementHistory(meterDto, meterReplacementHistoryList);
              node.Entity = entityDto;
            }
          }
        }
        BitmapImage imageForNode = StructuresHelper.GetImageForNode(node, node.Entity != null);
        imageForNode.Freeze();
        node.Image = imageForNode;
      }
      return node;
    }

    public static void UpdateMeterReplacementHistory(
      MeterDTO meterDto,
      IList<MeterReplacementHistorySerializableDTO> meterReplacementHistoryList)
    {
      if (!meterDto.IsReplaced)
        return;
      IEnumerable<MeterReplacementHistorySerializableDTO> source = meterReplacementHistoryList.Where<MeterReplacementHistorySerializableDTO>((Func<MeterReplacementHistorySerializableDTO, bool>) (m => m.CurrentMeterId == meterDto.Id));
      meterDto.MeterReplacementHistoryList = source.ToList<MeterReplacementHistorySerializableDTO>();
    }

    public static List<MeterReplacementHistorySerializableDTO> GetMeterReplacementHistorySerializableDTO(
      IList<MeterReplacementHistory> meterReplacementHistoryList)
    {
      List<MeterReplacementHistorySerializableDTO> historySerializableDto1 = new List<MeterReplacementHistorySerializableDTO>();
      foreach (MeterReplacementHistory replacementHistory in (IEnumerable<MeterReplacementHistory>) meterReplacementHistoryList)
      {
        MeterReplacementHistorySerializableDTO historySerializableDto2 = new MeterReplacementHistorySerializableDTO()
        {
          CurrentMeterId = replacementHistory.CurrentMeter.Id,
          ReplacedMeterId = replacementHistory.ReplacedMeter.Id,
          ReplacedMeterDeviceType = replacementHistory.ReplacedMeter.DeviceType,
          ReplacedMeterSerialNumber = replacementHistory.ReplacedMeter.SerialNumber,
          ReplacementDate = replacementHistory.ReplacementDate,
          ReplacedByUserId = replacementHistory.ReplacedByUser.Id,
          LastChangedOn = replacementHistory.LastChangedOn
        };
        historySerializableDto1.Add(historySerializableDto2);
      }
      return historySerializableDto1;
    }

    public static BitmapImage GetImageForNode(StructureNodeDTO node, bool hasEntity)
    {
      StructureNodeType nodeType = node.NodeType;
      string[] files1 = new string[2]
      {
        nodeType.IconPath,
        "pack://application:,,,/Styles;component/Images/StructureIcons/exclamation2.png"
      };
      BitmapImage imageForNode = new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/replace-meter4.png"));
      string[] files2 = new string[2]
      {
        "pack://application:,,,/Styles;component/Images/StructureIcons/replace-meter4.png",
        "pack://application:,,,/Styles;component/Images/StructureIcons/exclamation2.png"
      };
      switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), nodeType.Name, true))
      {
        case StructureNodeTypeEnum.Location:
          return hasEntity ? new BitmapImage(new Uri(nodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files1);
        case StructureNodeTypeEnum.Tenant:
          return hasEntity ? new BitmapImage(new Uri(nodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files1);
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          MeterDTO entity1 = node.Entity as MeterDTO;
          StructureTypeEnum? structureType = node.StructureType;
          StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Fixed;
          if (structureType.GetValueOrDefault() == structureTypeEnum1 && structureType.HasValue)
          {
            bool? isConfigured;
            int num1;
            if (hasEntity && entity1 != null && entity1.IsReplaced)
            {
              isConfigured = entity1.IsConfigured;
              bool flag = true;
              num1 = isConfigured.GetValueOrDefault() == flag ? (isConfigured.HasValue ? 1 : 0) : 0;
            }
            else
              num1 = 0;
            if (num1 != 0)
              return imageForNode;
            int num2;
            if (hasEntity && entity1 != null && entity1.IsReplaced)
            {
              isConfigured = entity1.IsConfigured;
              bool flag = false;
              num2 = isConfigured.GetValueOrDefault() == flag ? (isConfigured.HasValue ? 1 : 0) : 0;
            }
            else
              num2 = 0;
            if (num2 != 0)
              return ImageHelper.Instance.GetBitmapImageFromFiles(files2);
            int num3;
            if (hasEntity && entity1 != null)
            {
              isConfigured = entity1.IsConfigured;
              bool flag = true;
              num3 = isConfigured.GetValueOrDefault() == flag ? (isConfigured.HasValue ? 1 : 0) : 0;
            }
            else
              num3 = 0;
            return num3 != 0 ? new BitmapImage(new Uri(nodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files1);
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
            return !hasEntity || entity1 == null ? ImageHelper.Instance.GetBitmapImageFromFiles(files1) : new BitmapImage(new Uri(nodeType.IconPath));
          if (entity1 != null && entity1.IsReplaced)
            return imageForNode;
          return entity1 != null ? new BitmapImage(new Uri(node.NodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files1);
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          if (node.Entity == null)
            return new BitmapImage(new Uri(node.NodeType.IconPath));
          return !(node.Entity is MinomatSerializableDTO entity2) ? ((node.Entity as MinomatDTO).IsMaster ? new BitmapImage(new Uri(node.NodeType.IconPath)) : new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/minomats.png"))) : (entity2.IsMaster ? new BitmapImage(new Uri(node.NodeType.IconPath)) : new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/minomats.png")));
        default:
          return new BitmapImage(new Uri(nodeType.IconPath));
      }
    }

    public static BitmapImage Combine2Images(BitmapImage image, SolidColorBrush colorState)
    {
      if (colorState == null)
        return image;
      Bitmap secondImage = (Bitmap) null;
      if (colorState.Equals((object) System.Windows.Media.Brushes.LightGreen))
        secondImage = new Bitmap(Application.GetResourceStream(new Uri("pack://application:,,,/Styles;component/Images/Universal/checkmark-green.png", UriKind.RelativeOrAbsolute)).Stream);
      else if (colorState.Equals((object) System.Windows.Media.Brushes.OrangeRed))
        secondImage = new Bitmap(Application.GetResourceStream(new Uri("pack://application:,,,/Styles;component/Images/Universal/checkmark-red.png", UriKind.RelativeOrAbsolute)).Stream);
      else if (colorState.Equals((object) System.Windows.Media.Brushes.DeepSkyBlue))
        secondImage = new Bitmap(Application.GetResourceStream(new Uri("pack://application:,,,/Styles;component/Images/Universal/checkmark-blue.png", UriKind.RelativeOrAbsolute)).Stream);
      else if (colorState.Equals((object) System.Windows.Media.Brushes.Yellow))
        secondImage = new Bitmap(Application.GetResourceStream(new Uri("pack://application:,,,/Styles;component/Images/Universal/checkmark-yellow.png", UriKind.RelativeOrAbsolute)).Stream);
      return ImageHelper.Instance.Combine2Images(ImageHelper.Instance.BitmapImage2Bitmap(image), secondImage);
    }

    public static BitmapImage GetImageForNode(
      StructureNodeType nodeType,
      object entity,
      bool hasEntity)
    {
      string[] files = new string[2]
      {
        nodeType.IconPath,
        "pack://application:,,,/Styles;component/Images/StructureIcons/exclamation2.png"
      };
      switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), nodeType.Name, true))
      {
        case StructureNodeTypeEnum.Location:
          return hasEntity ? new BitmapImage(new Uri(nodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files);
        case StructureNodeTypeEnum.Tenant:
          return hasEntity ? new BitmapImage(new Uri(nodeType.IconPath)) : ImageHelper.Instance.GetBitmapImageFromFiles(files);
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          return new BitmapImage(new Uri(nodeType.IconPath));
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          if (entity == null)
            return new BitmapImage(new Uri(nodeType.IconPath));
          return !(entity is MinomatSerializableDTO minomatSerializableDto) ? ((entity as MinomatDTO).IsMaster ? new BitmapImage(new Uri(nodeType.IconPath)) : new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/minomats.png"))) : (minomatSerializableDto.IsMaster ? new BitmapImage(new Uri(nodeType.IconPath)) : new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/minomats.png")));
        default:
          return new BitmapImage(new Uri(nodeType.IconPath));
      }
    }

    public static BitmapImage GetImageForMeterNode(
      MeterMBusStateEnum? meterState,
      StructureNodeType nodeType,
      bool? isMeterConfigured,
      string meterSerialNumber,
      StructureTypeEnum? structureType,
      bool existsMeterWithSameSerialNumberInCurrentStructure,
      HashSet<string> metersMissingTranslationRules = null)
    {
      string[] files1 = new string[2]
      {
        nodeType.IconPath,
        "pack://application:,,,/Styles;component/Images/Universal/checkmark-red.png"
      };
      string[] files2 = new string[2]
      {
        nodeType.IconPath,
        "pack://application:,,,/Styles;component/Images/Universal/checkmark-green.png"
      };
      string[] files3 = new string[2]
      {
        nodeType.IconPath,
        "pack://application:,,,/Styles;component/Images/Universal/checkmark-yellow.png"
      };
      StructureTypeEnum? nullable = structureType;
      StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
      if (nullable.GetValueOrDefault() == structureTypeEnum && nullable.HasValue)
      {
        if (StructuresHelper.MeterExistsInAnotherStructureOrHasNoSerialNumber(meterState, meterSerialNumber) || isMeterConfigured.HasValue && !isMeterConfigured.Value)
          return ImageHelper.Instance.GetBitmapImageFromFiles(files1);
        return metersMissingTranslationRules != null && metersMissingTranslationRules.Contains(meterSerialNumber) ? ImageHelper.Instance.GetBitmapImageFromFiles(files3) : ImageHelper.Instance.GetBitmapImageFromFiles(files2);
      }
      return StructuresHelper.MeterExistsInAnotherStructureOrHasNoSerialNumber(meterState, meterSerialNumber) || existsMeterWithSameSerialNumberInCurrentStructure && meterState.HasValue && meterState.Value != MeterMBusStateEnum.Update ? ImageHelper.Instance.GetBitmapImageFromFiles(files1) : new BitmapImage(new Uri(nodeType.IconPath));
    }

    public static bool MeterExistsInAnotherStructureOrHasNoSerialNumber(
      MeterMBusStateEnum? meterState,
      string meterSerialNumber)
    {
      int num;
      if (!string.IsNullOrWhiteSpace(meterSerialNumber))
      {
        if (meterState.HasValue)
        {
          if (meterState.Value != MeterMBusStateEnum.ExistingMeterInAnotherStructure)
          {
            MeterMBusStateEnum? nullable = meterState;
            MeterMBusStateEnum meterMbusStateEnum = MeterMBusStateEnum.ExistingMeterInAnotherStructureAndCurrentStructure;
            num = nullable.GetValueOrDefault() == meterMbusStateEnum ? (nullable.HasValue ? 1 : 0) : 0;
          }
          else
            num = 1;
        }
        else
          num = 0;
      }
      else
        num = 1;
      return num != 0;
    }

    public static object GetEntityDTO(StructureNodeTypeEnum structureNodeTypeName, object entityObj)
    {
      object obj = new object();
      object entityDto = new object();
      switch (structureNodeTypeName)
      {
        case StructureNodeTypeEnum.Location:
          entityDto = (object) Mapper.Map<Location, LocationDTO>(entityObj as Location);
          break;
        case StructureNodeTypeEnum.Tenant:
          entityDto = (object) Mapper.Map<Tenant, TenantDTO>(entityObj as Tenant);
          break;
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          entityDto = (object) Mapper.Map<Meter, MeterDTO>(entityObj as Meter);
          break;
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          entityDto = (object) Mapper.Map<MSS.Core.Model.DataCollectors.Minomat, MinomatSerializableDTO>(entityObj as MSS.Core.Model.DataCollectors.Minomat);
          break;
      }
      return entityDto;
    }

    public static void GetEntityIdAndEntityType(
      StructureNodeDTO structureNodeDto,
      out Guid entityId,
      out StructureNodeTypeEnum entityType)
    {
      object entity = structureNodeDto.Entity;
      entityId = Guid.Empty;
      entityType = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeDto.NodeType.Name, true);
      switch (entityType)
      {
        case StructureNodeTypeEnum.Location:
          if (!(entity is Location location))
            break;
          entityId = location.Id;
          break;
        case StructureNodeTypeEnum.Tenant:
          if (!(entity is Tenant tenant))
            break;
          entityId = tenant.Id;
          break;
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          if (!(entity is Meter meter))
            break;
          entityId = meter.Id;
          break;
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          if (!(entity is MSS.Core.Model.DataCollectors.Minomat minomat))
            break;
          entityId = minomat.Id;
          break;
        default:
          entityId = Guid.Empty;
          break;
      }
    }

    public static IEnumerable<StructureNodeLinks> GetDescendantsForStructureNodeLink(
      IRepository<StructureNodeLinks> _structureNodeLinksRepository,
      StructureNodeLinks parentStructureNodeLink)
    {
      List<StructureNodeLinks> descendants = new List<StructureNodeLinks>();
      IEnumerable<StructureNodeLinks> nodesForRootNode = StructuresHelper.GetStructureNodesForRootNode(_structureNodeLinksRepository, parentStructureNodeLink.RootNode.Id);
      StructuresHelper.FindChildren(parentStructureNodeLink, nodesForRootNode, ref descendants);
      descendants.Add(parentStructureNodeLink);
      return (IEnumerable<StructureNodeLinks>) descendants;
    }

    private static void FindChildren(
      StructureNodeLinks parentStructureNodeLink,
      IEnumerable<StructureNodeLinks> structureNodeLinkCollection,
      ref List<StructureNodeLinks> descendants)
    {
      IEnumerable<StructureNodeLinks> source = structureNodeLinkCollection.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => s.ParentNodeId == parentStructureNodeLink.Node.Id));
      if (!source.Any<StructureNodeLinks>())
        return;
      foreach (StructureNodeLinks parentStructureNodeLink1 in source)
      {
        descendants.Add(parentStructureNodeLink1);
        StructuresHelper.FindChildren(parentStructureNodeLink1, structureNodeLinkCollection, ref descendants);
      }
    }

    public static IEnumerable<StructureNodeLinks> GetStructureNodesForRootNode(
      IRepository<StructureNodeLinks> _structureNodeLinksRepository,
      Guid rootNodeId)
    {
      return (IEnumerable<StructureNodeLinks>) _structureNodeLinksRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (s => s.RootNode.Id == rootNodeId)).ToList<StructureNodeLinks>();
    }

    public static IEnumerable<StructureNodeDTO> GetStructureNodeDTOForRootNode(
      Guid structureRootNodeId,
      IRepository<StructureNodeLinks> structureNodeLinksRepository,
      IRepository<StructureNode> structureNodeReporsitory,
      IRepository<StructureNodeType> structureNodeTypeRepository,
      ISession session,
      IList<MeterReplacementHistorySerializableDTO> meterReplacementHistory = null)
    {
      ObservableCollection<StructureNodeDTO> nodeDtoForRootNode = new ObservableCollection<StructureNodeDTO>();
      if (structureRootNodeId != Guid.Empty)
      {
        StructureNodeLinks structureNodeLinks = structureNodeLinksRepository.FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == structureRootNodeId));
        List<StructureNodeLinks> list = StructuresHelper.GetStructureNodesForRootNode(structureNodeLinksRepository, structureNodeLinks.Node.Id).Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => !s.EndDate.HasValue)).ToList<StructureNodeLinks>();
        List<Guid> nodeIDs = StructuresHelper.GetNodeIdList((IEnumerable<StructureNodeLinks>) list);
        Dictionary<Guid, object> entitiesDictionary = StructuresHelper.GetEntitiesDictionary(structureNodeReporsitory.SearchFor((Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id) && s.EndDate == new DateTime?())), session);
        nodeDtoForRootNode = StructuresHelper.GetTreeFromList(structureNodeTypeRepository.GetAll(), (IList<StructureNodeLinks>) list, entitiesDictionary, meterReplacementHistoryList: meterReplacementHistory);
      }
      return (IEnumerable<StructureNodeDTO>) nodeDtoForRootNode;
    }

    public static Dictionary<Guid, object> GetEntitiesDictionary(
      IList<StructureNode> structureNodeList,
      ISession session)
    {
      Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
      foreach (StructureNode structureNode in (IEnumerable<StructureNode>) structureNodeList)
      {
        if (structureNode.EntityId != Guid.Empty)
        {
          object entity = StructuresHelper.GetEntity((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNode.EntityName, true), structureNode, session);
          entitiesDictionary.Add(structureNode.EntityId, entity);
        }
      }
      return entitiesDictionary;
    }

    public static List<Guid> GetNodeIdList(IEnumerable<StructureNodeLinks> structureNodeLinks)
    {
      List<Guid> nodeIDs = new List<Guid>();
      foreach (StructureNodeLinks structureNodeLinks1 in structureNodeLinks.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (structureNode => !nodeIDs.Contains(structureNode.Node.Id))))
        nodeIDs.Add(structureNodeLinks1.Node.Id);
      return nodeIDs;
    }

    public static object GetEntity(
      StructureNodeTypeEnum structureNodeTypeName,
      StructureNode structureNode,
      ISession session)
    {
      object entity = new object();
      switch (structureNodeTypeName)
      {
        case StructureNodeTypeEnum.Location:
          entity = (object) StructuresHelper.GetEntity<Location>(structureNode.EntityId, session);
          break;
        case StructureNodeTypeEnum.Tenant:
          entity = (object) StructuresHelper.GetEntity<Tenant>(structureNode.EntityId, session);
          break;
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          entity = (object) StructuresHelper.GetEntity<Meter>(structureNode.EntityId, session);
          break;
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          entity = (object) StructuresHelper.GetEntity<MSS.Core.Model.DataCollectors.Minomat>(structureNode.EntityId, session);
          break;
      }
      return entity;
    }

    public static T GetEntity<T>(Guid entityId, ISession session)
    {
      return session.Get<T>((object) entityId);
    }

    public static OrderSerializableStructure GetSerializableStructure(Structure structure)
    {
      OrderSerializableStructure serializableStructure = new OrderSerializableStructure()
      {
        locationList = new List<LocationSerializableDTO>(),
        meterList = new List<MeterSerializableDTO>(),
        structureNodeList = new List<StructureNodeSerializableDTO>(),
        tenantList = new List<TenantSerializableDTO>(),
        minomatList = new List<MinomatSerializableDTO>(),
        structureNodesLinksList = new List<StructureNodeLinksSerializableDTO>(),
        meterReplacementHistoryList = new List<MeterReplacementHistorySerializableDTO>()
      };
      structure.Minomats.ForEach((Action<MSS.Core.Model.DataCollectors.Minomat>) (m =>
      {
        MinomatSerializableDTO minomatSerializableDto1 = new MinomatSerializableDTO();
        minomatSerializableDto1.Id = m.Id;
        minomatSerializableDto1.EndDate = m.EndDate;
        minomatSerializableDto1.IsDeactivated = m.IsDeactivated;
        minomatSerializableDto1.StartDate = m.StartDate;
        minomatSerializableDto1.AccessPoint = m.AccessPoint;
        minomatSerializableDto1.Challenge = m.Challenge;
        minomatSerializableDto1.CreatedBy = m.CreatedBy;
        minomatSerializableDto1.CreatedOn = m.CreatedOn;
        minomatSerializableDto1.GsmId = m.GsmId;
        minomatSerializableDto1.HostAndPort = m.HostAndPort;
        minomatSerializableDto1.IsInMasterPool = m.IsInMasterPool;
        minomatSerializableDto1.IsMaster = m.IsMaster;
        minomatSerializableDto1.LastUpdatedBy = m.LastUpdatedBy;
        minomatSerializableDto1.LastChangedOn = m.LastChangedOn;
        minomatSerializableDto1.RadioId = m.RadioId;
        MinomatSerializableDTO minomatSerializableDto2 = minomatSerializableDto1;
        Provider provider = m.Provider;
        Guid guid = provider != null ? provider.Id : Guid.Empty;
        minomatSerializableDto2.ProviderId = guid;
        minomatSerializableDto1.Polling = m.Polling;
        minomatSerializableDto1.ProviderName = m.ProviderName;
        minomatSerializableDto1.Registered = m.Registered;
        minomatSerializableDto1.SessionKey = m.SessionKey;
        minomatSerializableDto1.SimPin = m.SimPin;
        minomatSerializableDto1.Status = m.Status;
        serializableStructure.minomatList.Add(minomatSerializableDto1);
      }));
      structure.Links.ForEach((Action<StructureNodeLinks>) (l => serializableStructure.structureNodesLinksList.Add(new StructureNodeLinksSerializableDTO()
      {
        Id = l.Id,
        EndDate = l.EndDate,
        StartDate = l.StartDate,
        NodeId = l.Node.Id,
        ParentNodeId = l.ParentNodeId,
        RootNodeId = l.RootNode.Id,
        StructureType = l.StructureType,
        OrderNr = l.OrderNr,
        LastChangedOn = l.LastChangedOn
      })));
      structure.Nodes.ForEach((Action<StructureNode>) (s =>
      {
        StructureNodeSerializableDTO nodeSerializableDto1 = new StructureNodeSerializableDTO();
        nodeSerializableDto1.Id = s.Id;
        nodeSerializableDto1.Description = s.Description;
        nodeSerializableDto1.Name = s.Name;
        nodeSerializableDto1.EndDate = s.EndDate;
        nodeSerializableDto1.EntityId = s.EntityId;
        nodeSerializableDto1.EntityName = s.EntityName;
        StructureNodeSerializableDTO nodeSerializableDto2 = nodeSerializableDto1;
        StructureNodeType nodeType = s.NodeType;
        Guid guid = nodeType != null ? nodeType.Id : Guid.Empty;
        nodeSerializableDto2.NodeType = guid;
        nodeSerializableDto1.StartDate = s.StartDate;
        nodeSerializableDto1.LastChangedOn = s.LastChangedOn;
        serializableStructure.structureNodeList.Add(nodeSerializableDto1);
      }));
      structure.Locations.ForEach((Action<Location>) (l => serializableStructure.locationList.Add(new LocationSerializableDTO()
      {
        BuildingNr = l.BuildingNr,
        City = l.City,
        Description = l.Description,
        DueDate = l.DueDate,
        Generation = (int) l.Generation,
        HasMaster = l.HasMaster,
        Id = l.Id,
        ScenarioId = new Guid?(l.Scenario == null ? Guid.Empty : l.Scenario.Id),
        Street = l.Street,
        ZipCode = l.ZipCode,
        CountryId = l.Country != null ? l.Country.Id : Guid.Empty,
        LastChangedOn = l.LastChangedOn
      })));
      structure.Tenants.ForEach((Action<Tenant>) (t => serializableStructure.tenantList.Add(new TenantSerializableDTO()
      {
        ApartmentNr = t.ApartmentNr,
        CustomerTenantNo = t.CustomerTenantNo,
        Description = t.Description,
        Direction = t.Direction,
        FloorName = t.FloorName,
        FloorNr = t.FloorNr,
        Id = t.Id,
        IsDeactivated = t.IsDeactivated,
        Name = t.Name,
        TenantNr = t.TenantNr,
        Entrance = t.Entrance,
        LastChangedOn = t.LastChangedOn
      })));
      structure.Meters.ForEach((Action<Meter>) (m =>
      {
        MeterSerializableDTO meterSerializableDto1 = new MeterSerializableDTO();
        MeterSerializableDTO meterSerializableDto2 = meterSerializableDto1;
        Channel channel = m.Channel;
        Guid guid1 = channel != null ? channel.Id : Guid.Empty;
        meterSerializableDto2.ChannelId = guid1;
        meterSerializableDto1.CompletDevice = m.CompletDevice;
        meterSerializableDto1.ConnectedDeviceTypeId = m.ConnectedDeviceType == null ? Guid.Empty : m.ConnectedDeviceType.Id;
        meterSerializableDto1.DeviceType = m.DeviceType;
        meterSerializableDto1.Id = m.Id;
        meterSerializableDto1.EvaluationFactor = m.EvaluationFactor;
        meterSerializableDto1.ImpulsUnitCelestaCode = m.ImpulsUnit == null ? string.Empty : m.ImpulsUnit.CelestaCode;
        MeterSerializableDTO meterSerializableDto3 = meterSerializableDto1;
        MeasureUnit impulsUnit = m.ImpulsUnit;
        Guid guid2 = impulsUnit != null ? impulsUnit.Id : Guid.Empty;
        meterSerializableDto3.ImpulsUnitId = guid2;
        meterSerializableDto1.ImpulsValue = m.ImpulsValue;
        meterSerializableDto1.IsDeactivated = m.IsDeactivated;
        meterSerializableDto1.ReadingUnitCelestaCode = m.ReadingUnit == null ? string.Empty : m.ReadingUnit.CelestaCode;
        MeterSerializableDTO meterSerializableDto4 = meterSerializableDto1;
        MeasureUnit readingUnit = m.ReadingUnit;
        Guid guid3 = readingUnit != null ? readingUnit.Id : Guid.Empty;
        meterSerializableDto4.ReadingUnitId = guid3;
        meterSerializableDto1.RoomTypeCode = m.Room == null ? string.Empty : m.Room.Code;
        meterSerializableDto1.RoomTypeId = m.Room == null ? Guid.Empty : m.Room.Id;
        meterSerializableDto1.SerialNumber = m.SerialNumber;
        meterSerializableDto1.ShortDeviceNo = m.ShortDeviceNo;
        meterSerializableDto1.StartValue = m.StartValue;
        meterSerializableDto1.IsConfigured = m.IsConfigured;
        meterSerializableDto1.IsReplaced = m.IsReplaced;
        meterSerializableDto1.Manufacturer = m.Manufacturer;
        meterSerializableDto1.Medium = m.Medium;
        meterSerializableDto1.PrimaryAddress = m.PrimaryAddress;
        meterSerializableDto1.Generation = m.Generation;
        meterSerializableDto1.InputNumber = m.InputNumber;
        meterSerializableDto1.DeviceInfo = m.GMMAdditionalInfo;
        meterSerializableDto1.LastChangedOn = m.LastChangedOn;
        meterSerializableDto1.MbusRadioMeterId = m.MbusRadioMeter != null ? m.MbusRadioMeter.Id : Guid.Empty;
        meterSerializableDto1.City = m.MbusRadioMeter != null ? m.MbusRadioMeter.City : string.Empty;
        meterSerializableDto1.Street = m.MbusRadioMeter != null ? m.MbusRadioMeter.Street : string.Empty;
        meterSerializableDto1.HouseNumber = m.MbusRadioMeter != null ? m.MbusRadioMeter.HouseNumber : string.Empty;
        meterSerializableDto1.HouseNumberSupplement = m.MbusRadioMeter != null ? m.MbusRadioMeter.HouseNumberSupplement : string.Empty;
        meterSerializableDto1.ApartmentNumber = m.MbusRadioMeter != null ? m.MbusRadioMeter.ApartmentNumber : string.Empty;
        meterSerializableDto1.ZipCode = m.MbusRadioMeter != null ? m.MbusRadioMeter.ZipCode : string.Empty;
        meterSerializableDto1.FirstName = m.MbusRadioMeter != null ? m.MbusRadioMeter.FirstName : string.Empty;
        meterSerializableDto1.LastName = m.MbusRadioMeter != null ? m.MbusRadioMeter.LastName : string.Empty;
        meterSerializableDto1.Location = m.MbusRadioMeter != null ? m.MbusRadioMeter.Location : string.Empty;
        meterSerializableDto1.RadioSerialNumber = m.MbusRadioMeter != null ? m.MbusRadioMeter.RadioSerialNumber : string.Empty;
        meterSerializableDto1.ReadingEnabled = m.ReadingEnabled;
        serializableStructure.meterList.Add(meterSerializableDto1);
        if (!m.IsReplaced)
          return;
        TypeHelperExtensionMethods.ForEach<MeterReplacementHistorySerializableDTO>(structure.MeterReplacementHistory.Where<MeterReplacementHistorySerializableDTO>((Func<MeterReplacementHistorySerializableDTO, bool>) (h => h.CurrentMeterId == m.Id)), new Action<MeterReplacementHistorySerializableDTO>(serializableStructure.meterReplacementHistoryList.Add));
      }));
      return serializableStructure;
    }

    public static byte[] SerializeStructure(Structure structure)
    {
      OrderSerializableStructure serializableStructure = StructuresHelper.GetSerializableStructure(structure);
      XmlSerializer xmlSerializer = new XmlSerializer(serializableStructure.GetType());
      MemoryStream memoryStream = new MemoryStream();
      xmlSerializer.Serialize((Stream) memoryStream, (object) serializableStructure);
      return memoryStream.ToArray();
    }

    public static byte[] SerializeStructure(OrderSerializableStructure serializableStructure)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(serializableStructure.GetType());
      MemoryStream memoryStream = new MemoryStream();
      xmlSerializer.Serialize((Stream) memoryStream, (object) serializableStructure);
      return memoryStream.ToArray();
    }

    public static byte[] SerializeStructure(
      ObservableCollection<StructureNodeDTO> nodeCollection,
      byte[] structureBytes)
    {
      OrderSerializableStructure serializableStructure;
      if (structureBytes != null)
        serializableStructure = StructuresHelper.DeserializeStructure(structureBytes);
      else
        serializableStructure = new OrderSerializableStructure()
        {
          locationList = new List<LocationSerializableDTO>(),
          meterList = new List<MeterSerializableDTO>(),
          tenantList = new List<TenantSerializableDTO>(),
          minomatList = new List<MinomatSerializableDTO>(),
          structureNodeList = new List<StructureNodeSerializableDTO>(),
          structureNodesLinksList = new List<StructureNodeLinksSerializableDTO>(),
          meterReplacementHistoryList = new List<MeterReplacementHistorySerializableDTO>()
        };
      List<Guid> guidList = new List<Guid>();
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        StructureNodeDTO structureNodeDTO = node;
        Guid entityId = Guid.Empty;
        string entityName = string.Empty;
        StructuresHelper.GetEntityIdAndentityName(structureNodeDTO, out entityId, out entityName);
        StructureNodeSerializableDTO nodeSerializableDto1 = serializableStructure.structureNodeList.FirstOrDefault<StructureNodeSerializableDTO>((Func<StructureNodeSerializableDTO, bool>) (n => n.Id == structureNodeDTO.Id));
        if (nodeSerializableDto1 != null)
        {
          nodeSerializableDto1.Description = structureNodeDTO.Description;
          nodeSerializableDto1.Name = structureNodeDTO.Name;
          nodeSerializableDto1.EntityId = entityId;
          nodeSerializableDto1.EntityName = entityName;
          nodeSerializableDto1.NodeType = structureNodeDTO.NodeType == null ? Guid.Empty : structureNodeDTO.NodeType.Id;
          nodeSerializableDto1.LastChangedOn = structureNodeDTO.LastChangedOn;
        }
        else
        {
          StructureNodeSerializableDTO nodeSerializableDto2 = new StructureNodeSerializableDTO();
          nodeSerializableDto2.Id = structureNodeDTO.Id;
          nodeSerializableDto2.Description = structureNodeDTO.Description;
          nodeSerializableDto2.Name = structureNodeDTO.Name;
          nodeSerializableDto2.EntityId = entityId;
          nodeSerializableDto2.EntityName = entityName;
          StructureNodeSerializableDTO nodeSerializableDto3 = nodeSerializableDto2;
          StructureNodeType nodeType = structureNodeDTO.NodeType;
          Guid guid = nodeType != null ? nodeType.Id : Guid.Empty;
          nodeSerializableDto3.NodeType = guid;
          nodeSerializableDto2.StartDate = new DateTime?(DateTime.Now);
          StructureNodeSerializableDTO nodeSerializableDto4 = nodeSerializableDto2;
          serializableStructure.structureNodeList.Add(nodeSerializableDto4);
        }
        StructureNodeLinksSerializableDTO linksSerializableDto1 = serializableStructure.structureNodesLinksList.FirstOrDefault<StructureNodeLinksSerializableDTO>((Func<StructureNodeLinksSerializableDTO, bool>) (l => l.NodeId == structureNodeDTO.Id));
        StructureTypeEnum? structureType;
        if (linksSerializableDto1 != null)
        {
          linksSerializableDto1.StartDate = new DateTime?(DateTime.Now);
          linksSerializableDto1.NodeId = structureNodeDTO.Id;
          linksSerializableDto1.ParentNodeId = structureNodeDTO.ParentNode != null ? structureNodeDTO.ParentNode.Id : Guid.Empty;
          linksSerializableDto1.RootNodeId = structureNodeDTO.RootNode != null ? structureNodeDTO.RootNode.Id : Guid.Empty;
          StructureNodeLinksSerializableDTO linksSerializableDto2 = linksSerializableDto1;
          structureType = structureNodeDTO.StructureType;
          int num = (int) structureType.Value;
          linksSerializableDto2.StructureType = (StructureTypeEnum) num;
          linksSerializableDto1.OrderNr = structureNodeDTO.OrderNr;
        }
        else
        {
          StructureNodeLinksSerializableDTO linksSerializableDto3 = new StructureNodeLinksSerializableDTO();
          linksSerializableDto3.Id = Guid.NewGuid();
          linksSerializableDto3.StartDate = new DateTime?(DateTime.Now);
          linksSerializableDto3.NodeId = structureNodeDTO.Id;
          linksSerializableDto3.ParentNodeId = structureNodeDTO.ParentNode != null ? structureNodeDTO.ParentNode.Id : Guid.Empty;
          linksSerializableDto3.RootNodeId = structureNodeDTO.RootNode != null ? structureNodeDTO.RootNode.Id : Guid.Empty;
          StructureNodeLinksSerializableDTO linksSerializableDto4 = linksSerializableDto3;
          structureType = structureNodeDTO.StructureType;
          int num = (int) structureType.Value;
          linksSerializableDto4.StructureType = (StructureTypeEnum) num;
          linksSerializableDto3.OrderNr = structureNodeDTO.OrderNr;
          StructureNodeLinksSerializableDTO linksSerializableDto5 = linksSerializableDto3;
          serializableStructure.structureNodesLinksList.Add(linksSerializableDto5);
        }
        if (structureNodeDTO.Entity != null)
        {
          switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeDTO.NodeType.Name, true))
          {
            case StructureNodeTypeEnum.Location:
              LocationDTO locationDTO = (LocationDTO) structureNodeDTO.Entity;
              LocationSerializableDTO locationSerializableDto1 = serializableStructure.locationList.FirstOrDefault<LocationSerializableDTO>((Func<LocationSerializableDTO, bool>) (l => l.Id == locationDTO.Id));
              if (locationSerializableDto1 != null)
              {
                locationSerializableDto1.BuildingNr = locationDTO.BuildingNr;
                locationSerializableDto1.City = locationDTO.City;
                locationSerializableDto1.Description = locationDTO.Description;
                locationSerializableDto1.DueDate = locationDTO.DueDate;
                locationSerializableDto1.Generation = (int) locationDTO.Generation;
                locationSerializableDto1.HasMaster = locationDTO.HasMaster;
                locationSerializableDto1.Id = locationDTO.Id;
                locationSerializableDto1.ScenarioId = new Guid?(locationDTO.Scenario == null ? Guid.Empty : locationDTO.Scenario.Id);
                locationSerializableDto1.Street = locationDTO.Street;
                locationSerializableDto1.ZipCode = locationDTO.ZipCode;
                locationSerializableDto1.LastChangedOn = locationDTO.LastChangedOn;
                break;
              }
              LocationSerializableDTO locationSerializableDto2 = new LocationSerializableDTO()
              {
                BuildingNr = locationDTO.BuildingNr,
                City = locationDTO.City,
                Description = locationDTO.Description,
                DueDate = locationDTO.DueDate,
                Generation = (int) locationDTO.Generation,
                HasMaster = locationDTO.HasMaster,
                Id = locationDTO.Id,
                ScenarioId = new Guid?(locationDTO.Scenario == null ? Guid.Empty : locationDTO.Scenario.Id),
                Street = locationDTO.Street,
                ZipCode = locationDTO.ZipCode,
                CountryId = MSS.Business.Utils.AppContext.Current.LoggedUser.Country.Id
              };
              serializableStructure.locationList.Add(locationSerializableDto2);
              break;
            case StructureNodeTypeEnum.Tenant:
              TenantDTO tenantDTO = (TenantDTO) structureNodeDTO.Entity;
              TenantSerializableDTO tenantSerializableDto1 = serializableStructure.tenantList.FirstOrDefault<TenantSerializableDTO>((Func<TenantSerializableDTO, bool>) (t => t.Id == tenantDTO.Id));
              if (tenantSerializableDto1 != null)
              {
                tenantSerializableDto1.ApartmentNr = tenantDTO.ApartmentNr;
                tenantSerializableDto1.CustomerTenantNo = tenantDTO.CustomerTenantNo;
                tenantSerializableDto1.Description = tenantDTO.Description;
                tenantSerializableDto1.Direction = tenantDTO.Direction;
                tenantSerializableDto1.FloorName = tenantDTO.FloorName;
                tenantSerializableDto1.FloorNr = tenantDTO.FloorNr;
                tenantSerializableDto1.IsDeactivated = tenantDTO.IsDeactivated;
                tenantSerializableDto1.Name = tenantDTO.Name;
                tenantSerializableDto1.TenantNr = tenantDTO.TenantNr;
                tenantSerializableDto1.Entrance = tenantDTO.Entrance;
                tenantSerializableDto1.LastChangedOn = tenantDTO.LastChangedOn;
                break;
              }
              TenantSerializableDTO tenantSerializableDto2 = new TenantSerializableDTO()
              {
                ApartmentNr = tenantDTO.ApartmentNr,
                CustomerTenantNo = tenantDTO.CustomerTenantNo,
                Description = tenantDTO.Description,
                Direction = tenantDTO.Direction,
                FloorName = tenantDTO.FloorName,
                FloorNr = tenantDTO.FloorNr,
                Id = tenantDTO.Id,
                IsDeactivated = tenantDTO.IsDeactivated,
                Name = tenantDTO.Name,
                TenantNr = tenantDTO.TenantNr,
                Entrance = tenantDTO.Entrance
              };
              serializableStructure.tenantList.Add(tenantSerializableDto2);
              break;
            case StructureNodeTypeEnum.Meter:
            case StructureNodeTypeEnum.RadioMeter:
              MeterDTO meterDTO = (MeterDTO) structureNodeDTO.Entity;
              MeterSerializableDTO meterSerializableDto1 = serializableStructure.meterList.FirstOrDefault<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => m.Id == meterDTO.Id));
              if (meterSerializableDto1 != null)
              {
                meterSerializableDto1.ChannelId = meterDTO.Channel == null ? Guid.Empty : meterDTO.Channel.Id;
                meterSerializableDto1.CompletDevice = meterDTO.CompletDevice;
                meterSerializableDto1.ConnectedDeviceTypeId = meterDTO.ConnectedDeviceType == null ? Guid.Empty : meterDTO.ConnectedDeviceType.Id;
                meterSerializableDto1.DeviceType = meterDTO.DeviceType;
                meterSerializableDto1.Id = meterDTO.Id;
                meterSerializableDto1.EvaluationFactor = meterDTO.EvaluationFactor;
                meterSerializableDto1.ImpulsUnitCelestaCode = meterDTO.ImpulsUnit == null ? string.Empty : meterDTO.ImpulsUnit.CelestaCode;
                meterSerializableDto1.ImpulsUnitId = meterDTO.ImpulsUnit == null ? Guid.Empty : meterDTO.ImpulsUnit.Id;
                meterSerializableDto1.ImpulsValue = meterDTO.ImpulsValue;
                meterSerializableDto1.IsDeactivated = meterDTO.IsDeactivated;
                meterSerializableDto1.ReadingUnitCelestaCode = meterDTO.ReadingUnit == null ? string.Empty : meterDTO.ReadingUnit.CelestaCode;
                meterSerializableDto1.ReadingUnitId = meterDTO.ReadingUnit == null ? Guid.Empty : meterDTO.ReadingUnit.Id;
                meterSerializableDto1.RoomTypeCode = meterDTO.Room == null ? string.Empty : meterDTO.Room.Code;
                meterSerializableDto1.RoomTypeId = meterDTO.Room == null ? Guid.Empty : meterDTO.Room.Id;
                meterSerializableDto1.SerialNumber = meterDTO.SerialNumber;
                meterSerializableDto1.ShortDeviceNo = meterDTO.ShortDeviceNo;
                meterSerializableDto1.StartValue = meterDTO.StartValue;
                meterSerializableDto1.IsConfigured = meterDTO.IsConfigured;
              }
              else
              {
                MeterSerializableDTO meterSerializableDto2 = new MeterSerializableDTO();
                MeterSerializableDTO meterSerializableDto3 = meterSerializableDto2;
                Channel channel = meterDTO.Channel;
                Guid guid1 = channel != null ? channel.Id : Guid.Empty;
                meterSerializableDto3.ChannelId = guid1;
                meterSerializableDto2.CompletDevice = meterDTO.CompletDevice;
                MeterSerializableDTO meterSerializableDto4 = meterSerializableDto2;
                ConnectedDeviceType connectedDeviceType = meterDTO.ConnectedDeviceType;
                Guid guid2 = connectedDeviceType != null ? connectedDeviceType.Id : Guid.Empty;
                meterSerializableDto4.ConnectedDeviceTypeId = guid2;
                meterSerializableDto2.DeviceType = meterDTO.DeviceType;
                meterSerializableDto2.Id = meterDTO.Id;
                meterSerializableDto2.EvaluationFactor = meterDTO.EvaluationFactor;
                meterSerializableDto2.ImpulsUnitCelestaCode = meterDTO.ImpulsUnit == null ? string.Empty : meterDTO.ImpulsUnit.CelestaCode;
                MeterSerializableDTO meterSerializableDto5 = meterSerializableDto2;
                MeasureUnit impulsUnit = meterDTO.ImpulsUnit;
                Guid guid3 = impulsUnit != null ? impulsUnit.Id : Guid.Empty;
                meterSerializableDto5.ImpulsUnitId = guid3;
                meterSerializableDto2.ImpulsValue = meterDTO.ImpulsValue;
                meterSerializableDto2.IsDeactivated = meterDTO.IsDeactivated;
                meterSerializableDto2.ReadingUnitCelestaCode = meterDTO.ReadingUnit == null ? string.Empty : meterDTO.ReadingUnit.CelestaCode;
                MeterSerializableDTO meterSerializableDto6 = meterSerializableDto2;
                MeasureUnit readingUnit = meterDTO.ReadingUnit;
                Guid guid4 = readingUnit != null ? readingUnit.Id : Guid.Empty;
                meterSerializableDto6.ReadingUnitId = guid4;
                meterSerializableDto2.RoomTypeCode = meterDTO.Room == null ? string.Empty : meterDTO.Room.Code;
                MeterSerializableDTO meterSerializableDto7 = meterSerializableDto2;
                RoomType room = meterDTO.Room;
                Guid guid5 = room != null ? room.Id : Guid.Empty;
                meterSerializableDto7.RoomTypeId = guid5;
                meterSerializableDto2.SerialNumber = meterDTO.SerialNumber;
                meterSerializableDto2.ShortDeviceNo = meterDTO.ShortDeviceNo;
                meterSerializableDto2.StartValue = meterDTO.StartValue;
                meterSerializableDto2.IsConfigured = meterDTO.IsConfigured;
                meterSerializableDto2.IsReplaced = meterDTO.IsReplaced;
                meterSerializableDto2.LastChangedOn = meterDTO.LastChangedOn;
                MeterSerializableDTO meterSerializableDto8 = meterSerializableDto2;
                serializableStructure.meterList.Add(meterSerializableDto8);
              }
              Guid? replacedMeterId = meterDTO.ReplacedMeterId;
              if (replacedMeterId.HasValue)
              {
                Guid id = meterDTO.Id;
                replacedMeterId = meterDTO.ReplacedMeterId;
                Guid _replacedMeterId = replacedMeterId.Value;
                OrderSerializableStructure oldSerializableStructure = serializableStructure;
                List<MeterReplacementHistorySerializableDTO> historySerializableDtoList = StructuresHelper.UpdateMeterReplacementHistoryForReadingOrder(id, _replacedMeterId, oldSerializableStructure);
                serializableStructure.meterReplacementHistoryList = historySerializableDtoList;
                break;
              }
              break;
            case StructureNodeTypeEnum.MinomatMaster:
            case StructureNodeTypeEnum.MinomatSlave:
              MinomatSerializableDTO minomatDTO = (MinomatSerializableDTO) structureNodeDTO.Entity;
              MinomatSerializableDTO minomatSerializableDto = serializableStructure.minomatList.FirstOrDefault<MinomatSerializableDTO>((Func<MinomatSerializableDTO, bool>) (m => m.Id == minomatDTO.Id));
              if (minomatSerializableDto != null)
              {
                minomatSerializableDto.EndDate = minomatDTO.EndDate;
                minomatSerializableDto.IsDeactivated = minomatDTO.IsDeactivated;
                minomatSerializableDto.StartDate = minomatDTO.StartDate;
                minomatSerializableDto.AccessPoint = minomatDTO.AccessPoint;
                minomatSerializableDto.CelestaId = minomatDTO.CelestaId;
                minomatSerializableDto.Challenge = minomatDTO.Challenge;
                minomatSerializableDto.CreatedBy = minomatDTO.CreatedBy;
                minomatSerializableDto.CreatedOn = minomatDTO.CreatedOn;
                minomatSerializableDto.GsmId = minomatDTO.GsmId;
                minomatSerializableDto.HostAndPort = minomatDTO.HostAndPort;
                minomatSerializableDto.IsInMasterPool = minomatDTO.IsInMasterPool;
                minomatSerializableDto.IsMaster = minomatDTO.IsMaster;
                minomatSerializableDto.LastUpdatedBy = minomatDTO.LastUpdatedBy;
                minomatSerializableDto.LastChangedOn = minomatDTO.LastChangedOn;
                minomatSerializableDto.RadioId = minomatDTO.RadioId;
                minomatSerializableDto.ProviderId = minomatDTO.Id;
                minomatSerializableDto.Polling = minomatDTO.Polling;
                minomatSerializableDto.ProviderName = minomatDTO.ProviderName;
                minomatSerializableDto.Registered = minomatDTO.Registered;
                minomatSerializableDto.SessionKey = minomatDTO.SessionKey;
                minomatSerializableDto.SimPin = minomatDTO.SimPin;
                minomatSerializableDto.Status = minomatDTO.Status;
                break;
              }
              serializableStructure.minomatList.Add(minomatDTO);
              break;
          }
        }
        guidList.Add(structureNodeDTO.Id);
      }
      List<StructureNodeSerializableDTO> nodesToDelete = new List<StructureNodeSerializableDTO>();
      foreach (StructureNodeSerializableDTO structureNode in serializableStructure.structureNodeList)
      {
        if (!guidList.Contains(structureNode.Id))
          nodesToDelete.Add(structureNode);
      }
      StructuresHelper.RemoveNodesFromSerializableCollection(nodesToDelete, serializableStructure);
      structureBytes = StructuresHelper.SerializeStructure(serializableStructure);
      return structureBytes;
    }

    private static List<MeterReplacementHistorySerializableDTO> UpdateMeterReplacementHistoryForReadingOrder(
      Guid _currentMeterId,
      Guid _replacedMeterId,
      OrderSerializableStructure oldSerializableStructure)
    {
      List<MeterReplacementHistorySerializableDTO> replacementHistoryList = oldSerializableStructure.meterReplacementHistoryList;
      MeterSerializableDTO replacedMeterDTO = oldSerializableStructure.meterList.FirstOrDefault<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => m.Id == _replacedMeterId));
      if (replacedMeterDTO != null)
      {
        MeterReplacementHistorySerializableDTO historySerializableDto1 = new MeterReplacementHistorySerializableDTO()
        {
          CurrentMeterId = _currentMeterId,
          ReplacedMeterId = replacedMeterDTO.Id,
          ReplacedMeterDeviceType = replacedMeterDTO.DeviceType,
          ReplacedMeterSerialNumber = replacedMeterDTO.SerialNumber,
          ReplacementDate = DateTime.Now,
          ReplacedByUserId = MSS.Business.Utils.AppContext.Current.LoggedUser.Id
        };
        replacementHistoryList.Add(historySerializableDto1);
        foreach (MeterReplacementHistorySerializableDTO historySerializableDto2 in replacementHistoryList.Where<MeterReplacementHistorySerializableDTO>((Func<MeterReplacementHistorySerializableDTO, bool>) (m => m.CurrentMeterId == replacedMeterDTO.Id)))
          historySerializableDto2.CurrentMeterId = _currentMeterId;
      }
      return replacementHistoryList;
    }

    private static void RemoveNodesFromSerializableCollection(
      List<StructureNodeSerializableDTO> nodesToDelete,
      OrderSerializableStructure serializableStructure)
    {
      foreach (StructureNodeSerializableDTO nodeSerializableDto in nodesToDelete)
      {
        StructureNodeSerializableDTO structureNodeSerializableDto = nodeSerializableDto;
        serializableStructure.structureNodeList.Remove(structureNodeSerializableDto);
        StructureNodeLinksSerializableDTO linksSerializableDto = serializableStructure.structureNodesLinksList.FirstOrDefault<StructureNodeLinksSerializableDTO>((Func<StructureNodeLinksSerializableDTO, bool>) (l => l.NodeId == structureNodeSerializableDto.Id));
        if (linksSerializableDto != null)
          serializableStructure.structureNodesLinksList.Remove(linksSerializableDto);
        if (!string.IsNullOrEmpty(structureNodeSerializableDto.EntityName))
        {
          switch (structureNodeSerializableDto.EntityName)
          {
            case "MeterDTO":
              MeterSerializableDTO meterSerializableDto = serializableStructure.meterList.FirstOrDefault<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => m.Id == structureNodeSerializableDto.EntityId));
              serializableStructure.meterList.Remove(meterSerializableDto);
              break;
            case "TenantDTO":
              TenantSerializableDTO tenantSerializableDto = serializableStructure.tenantList.FirstOrDefault<TenantSerializableDTO>((Func<TenantSerializableDTO, bool>) (m => m.Id == structureNodeSerializableDto.EntityId));
              serializableStructure.tenantList.Remove(tenantSerializableDto);
              break;
            case "LocationDTO":
              LocationSerializableDTO locationSerializableDto = serializableStructure.locationList.FirstOrDefault<LocationSerializableDTO>((Func<LocationSerializableDTO, bool>) (m => m.Id == structureNodeSerializableDto.EntityId));
              serializableStructure.locationList.Remove(locationSerializableDto);
              break;
          }
        }
      }
    }

    private static void GetEntityIdAndentityName(
      StructureNodeDTO structureNodeDTO,
      out Guid entityId,
      out string entityName)
    {
      entityId = Guid.Empty;
      entityName = string.Empty;
      if (structureNodeDTO.Entity == null)
        return;
      switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeDTO.NodeType.Name, true))
      {
        case StructureNodeTypeEnum.Location:
          entityId = (structureNodeDTO.Entity as LocationDTO).Id;
          entityName = StructureNodeTypeEnum.Location.ToString();
          break;
        case StructureNodeTypeEnum.Tenant:
          entityId = (structureNodeDTO.Entity as TenantDTO).Id;
          entityName = StructureNodeTypeEnum.Tenant.ToString();
          break;
        case StructureNodeTypeEnum.Meter:
          entityId = (structureNodeDTO.Entity as MeterDTO).Id;
          entityName = StructureNodeTypeEnum.Meter.ToString();
          break;
        case StructureNodeTypeEnum.MinomatMaster:
          entityId = (structureNodeDTO.Entity as MinomatSerializableDTO).Id;
          entityName = StructureNodeTypeEnum.MinomatMaster.ToString();
          break;
        case StructureNodeTypeEnum.MinomatSlave:
          entityId = (structureNodeDTO.Entity as MinomatSerializableDTO).Id;
          entityName = StructureNodeTypeEnum.MinomatSlave.ToString();
          break;
        case StructureNodeTypeEnum.RadioMeter:
          entityId = (structureNodeDTO.Entity as MeterDTO).Id;
          entityName = StructureNodeTypeEnum.RadioMeter.ToString();
          break;
      }
    }

    public static StructureNodeDTO LoadStructureFromRootNodeId(
      IRepositoryFactory repositoryFactory,
      Guid rootNodeId)
    {
      Mapper.CreateMap<StructureNode, StructureNodeDTO>();
      StructureNodeDTO rootNodeDto = (StructureNodeDTO) null;
      StructureNodeLinks structureNodeLink = repositoryFactory.GetRepository<StructureNodeLinks>().FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (item => item.Node.Id == rootNodeId));
      if (structureNodeLink != null)
      {
        Dictionary<Guid, StructureNodeType> dictionary = repositoryFactory.GetRepository<StructureNodeType>().GetAll().ToDictionary<StructureNodeType, Guid, StructureNodeType>((Func<StructureNodeType, Guid>) (item => item.Id), (Func<StructureNodeType, StructureNodeType>) (item => item));
        Dictionary<Guid, object> entitiesDictionary;
        repositoryFactory.GetStructureNodeRepository().GetStructureRootLinks(structureNodeLink.StructureType, out entitiesDictionary);
        rootNodeDto = StructuresHelper.CreateNewNode(structureNodeLink, (IDictionary<Guid, StructureNodeType>) dictionary, entitiesDictionary);
        rootNodeDto.RootNode = rootNodeDto;
        StructuresHelper.LoadSubNodesForRootNode(repositoryFactory, rootNodeDto, new StructuresManager(repositoryFactory));
      }
      return rootNodeDto;
    }

    public static void LoadSubNodesForRootNode(
      IRepositoryFactory repositoryFactory,
      StructureNodeDTO rootNodeDto,
      StructuresManager structuresManager)
    {
      IList<StructureNodeType> all1 = repositoryFactory.GetRepository<StructureNodeType>().GetAll();
      IList<MeterReplacementHistory> all2 = repositoryFactory.GetRepository<MeterReplacementHistory>().GetAll();
      StructureTypeEnum? structureType = rootNodeDto.StructureType;
      ObservableCollection<StructureNodeDTO> collectionWithChildren = structuresManager.GetNodeCollectionWithChildren(repositoryFactory.GetStructureNodeRepository(), structureType, all1, all2, rootNodeDto.Id);
      if (collectionWithChildren.Count <= 0)
        return;
      rootNodeDto.SubNodes = collectionWithChildren.First<StructureNodeDTO>().SubNodes;
    }

    public static OrderSerializableStructure DeserializeStructure(byte[] structureBytes)
    {
      return new XmlSerializer(typeof (OrderSerializableStructure)).Deserialize((Stream) new MemoryStream(structureBytes)) as OrderSerializableStructure;
    }

    public static Guid GetParentEntityIdForNode(Structure structure, Guid entityId)
    {
      StructureNode node = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.EntityId == entityId));
      if (node == null)
        return Guid.Empty;
      StructureNodeLinks link = structure.Links.FirstOrDefault<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (l => l.Node.Id == node.Id));
      if (link == null)
        return Guid.Empty;
      StructureNode structureNode = structure.Nodes.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (n => n.Id == link.ParentNodeId));
      return structureNode != null ? structureNode.EntityId : Guid.Empty;
    }

    public static Guid GetParentEntityIdForNode(OrderSerializableStructure structure, Guid entityId)
    {
      StructureNodeSerializableDTO node = structure.structureNodeList.FirstOrDefault<StructureNodeSerializableDTO>((Func<StructureNodeSerializableDTO, bool>) (n => n.EntityId == entityId));
      if (node == null)
        return Guid.Empty;
      StructureNodeLinksSerializableDTO link = structure.structureNodesLinksList.FirstOrDefault<StructureNodeLinksSerializableDTO>((Func<StructureNodeLinksSerializableDTO, bool>) (l => l.NodeId == node.Id));
      if (link == null)
        return Guid.Empty;
      StructureNodeSerializableDTO nodeSerializableDto = structure.structureNodeList.FirstOrDefault<StructureNodeSerializableDTO>((Func<StructureNodeSerializableDTO, bool>) (n => n.Id == link.ParentNodeId));
      return nodeSerializableDto != null ? nodeSerializableDto.EntityId : Guid.Empty;
    }

    public static int GetGroupNumber(DeviceTypeEnum deviceType, ConnectedDeviceType cdt)
    {
      switch (deviceType)
      {
        case DeviceTypeEnum.C5MBus:
        case DeviceTypeEnum.C5Radio:
        case DeviceTypeEnum.EDCRadio:
        case DeviceTypeEnum.MultidataS1:
        case DeviceTypeEnum.MultidataN1:
        case DeviceTypeEnum.WR3:
        case DeviceTypeEnum.Zelsius:
          return 100;
        case DeviceTypeEnum.M6:
          return 100;
        case DeviceTypeEnum.M7:
        case DeviceTypeEnum.MinomessMicroRadio3:
          return 100;
        case DeviceTypeEnum.MinotelContactRadio3:
          if (cdt != null)
          {
            switch (cdt.Code)
            {
              case "0":
              case "4":
              case "5":
                return 300;
              case "1":
                return 200;
              case "2":
              case "3":
                return 100;
            }
          }
          return 100;
        default:
          return 100;
      }
    }

    private static bool ValidateReadingMetersByTenantUniqueness(
      List<MeterDTO> meterList,
      out List<ReadingMeterUniqueIdentifier> duplicatesMeterIdentifier,
      KeyValuePair<object, ObservableCollection<StructureNodeDTO>> meterForTenant = default (KeyValuePair<object, ObservableCollection<StructureNodeDTO>>))
    {
      Dictionary<int, ReadingMeterUniqueIdentifier> meterUniqueIdentifierList = new Dictionary<int, ReadingMeterUniqueIdentifier>();
      int i = 1;
      meterList.ForEach((Action<MeterDTO>) (meter =>
      {
        MeterDTO meterDto = meter;
        int num;
        if (meterDto != null)
        {
          ReadingMeterUniqueIdentifier uniqueIdentifier1 = new ReadingMeterUniqueIdentifier()
          {
            GroupNumber = StructuresHelper.GetGroupNumber(meterDto.DeviceType, meterDto.ConnectedDeviceType),
            SerialNumber = meterDto.SerialNumber
          };
          if (meterForTenant.Key is TenantDTO)
          {
            ref ReadingMeterUniqueIdentifier local = ref uniqueIdentifier1;
            num = (meterForTenant.Key as TenantDTO).TenantNr;
            string str = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            local.TenantNo = str;
            meterUniqueIdentifierList.Add(i, uniqueIdentifier1);
          }
          else
          {
            ReadingMeterUniqueIdentifier uniqueIdentifier2 = new ReadingMeterUniqueIdentifier()
            {
              GroupNumber = StructuresHelper.GetGroupNumber(meterDto.DeviceType, meterDto.ConnectedDeviceType),
              SerialNumber = meterDto.SerialNumber
            };
            if (meterForTenant.Key is StructureNodeDTO key2)
            {
              uniqueIdentifier2.TenantNo = key2.Name;
              meterUniqueIdentifierList.Add(i, uniqueIdentifier2);
            }
          }
        }
        num = i++;
      }));
      List<ReadingMeterUniqueIdentifier> duplicates = meterUniqueIdentifierList.GroupBy<KeyValuePair<int, ReadingMeterUniqueIdentifier>, ReadingMeterUniqueIdentifier>((Func<KeyValuePair<int, ReadingMeterUniqueIdentifier>, ReadingMeterUniqueIdentifier>) (x => x.Value)).Where<IGrouping<ReadingMeterUniqueIdentifier, KeyValuePair<int, ReadingMeterUniqueIdentifier>>>((Func<IGrouping<ReadingMeterUniqueIdentifier, KeyValuePair<int, ReadingMeterUniqueIdentifier>>, bool>) (g => g.Skip<KeyValuePair<int, ReadingMeterUniqueIdentifier>>(1).Any<KeyValuePair<int, ReadingMeterUniqueIdentifier>>())).SelectMany<IGrouping<ReadingMeterUniqueIdentifier, KeyValuePair<int, ReadingMeterUniqueIdentifier>>, KeyValuePair<int, ReadingMeterUniqueIdentifier>>((Func<IGrouping<ReadingMeterUniqueIdentifier, KeyValuePair<int, ReadingMeterUniqueIdentifier>>, IEnumerable<KeyValuePair<int, ReadingMeterUniqueIdentifier>>>) (g => (IEnumerable<KeyValuePair<int, ReadingMeterUniqueIdentifier>>) g)).Distinct<KeyValuePair<int, ReadingMeterUniqueIdentifier>>().Select<KeyValuePair<int, ReadingMeterUniqueIdentifier>, ReadingMeterUniqueIdentifier>((Func<KeyValuePair<int, ReadingMeterUniqueIdentifier>, ReadingMeterUniqueIdentifier>) (x => x.Value)).ToList<ReadingMeterUniqueIdentifier>();
      duplicatesMeterIdentifier = new List<ReadingMeterUniqueIdentifier>();
      List<ReadingMeterUniqueIdentifier> identifier = duplicatesMeterIdentifier;
      duplicates.ForEach((Action<ReadingMeterUniqueIdentifier>) (x =>
      {
        if (!duplicates.Any<ReadingMeterUniqueIdentifier>((Func<ReadingMeterUniqueIdentifier, bool>) (y => y.SerialNumber == x.SerialNumber && y.GroupNumber == x.GroupNumber)))
          return;
        identifier.Add(x);
      }));
      return !meterUniqueIdentifierList.GroupBy<KeyValuePair<int, ReadingMeterUniqueIdentifier>, ReadingMeterUniqueIdentifier>((Func<KeyValuePair<int, ReadingMeterUniqueIdentifier>, ReadingMeterUniqueIdentifier>) (x => x.Value)).Any<IGrouping<ReadingMeterUniqueIdentifier, KeyValuePair<int, ReadingMeterUniqueIdentifier>>>((Func<IGrouping<ReadingMeterUniqueIdentifier, KeyValuePair<int, ReadingMeterUniqueIdentifier>>, bool>) (x => x.Count<KeyValuePair<int, ReadingMeterUniqueIdentifier>>() > 1));
    }

    private static bool ValidateInstallationMetersByTenantUniqueness(
      List<MeterDTO> meterList,
      out List<InstallationMeterUniqueIdentifier> duplicatesMeterIdentifier,
      KeyValuePair<object, ObservableCollection<StructureNodeDTO>> meterForTenant)
    {
      Dictionary<int, InstallationMeterUniqueIdentifier> meterUniqueIdentifierList = new Dictionary<int, InstallationMeterUniqueIdentifier>();
      int i = 1;
      meterList.ForEach((Action<MeterDTO>) (meter =>
      {
        MeterDTO meterDto = meter;
        int num;
        if (meterDto != null)
        {
          InstallationMeterUniqueIdentifier uniqueIdentifier1 = new InstallationMeterUniqueIdentifier()
          {
            SerialNumber = meterDto.SerialNumber
          };
          if (meterForTenant.Key is TenantDTO)
          {
            ref InstallationMeterUniqueIdentifier local = ref uniqueIdentifier1;
            num = (meterForTenant.Key as TenantDTO).TenantNr;
            string str = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            local.TenantNo = str;
            meterUniqueIdentifierList.Add(i, uniqueIdentifier1);
          }
          else
          {
            InstallationMeterUniqueIdentifier uniqueIdentifier2 = new InstallationMeterUniqueIdentifier()
            {
              SerialNumber = meterDto.SerialNumber
            };
            if (meterForTenant.Key is StructureNodeDTO key2)
            {
              uniqueIdentifier1.TenantNo = key2.Description;
              meterUniqueIdentifierList.Add(i, uniqueIdentifier2);
            }
          }
        }
        num = i++;
      }));
      List<InstallationMeterUniqueIdentifier> list = meterUniqueIdentifierList.GroupBy<KeyValuePair<int, InstallationMeterUniqueIdentifier>, InstallationMeterUniqueIdentifier>((Func<KeyValuePair<int, InstallationMeterUniqueIdentifier>, InstallationMeterUniqueIdentifier>) (x => x.Value)).Where<IGrouping<InstallationMeterUniqueIdentifier, KeyValuePair<int, InstallationMeterUniqueIdentifier>>>((Func<IGrouping<InstallationMeterUniqueIdentifier, KeyValuePair<int, InstallationMeterUniqueIdentifier>>, bool>) (g => g.Skip<KeyValuePair<int, InstallationMeterUniqueIdentifier>>(1).Any<KeyValuePair<int, InstallationMeterUniqueIdentifier>>())).SelectMany<IGrouping<InstallationMeterUniqueIdentifier, KeyValuePair<int, InstallationMeterUniqueIdentifier>>, KeyValuePair<int, InstallationMeterUniqueIdentifier>>((Func<IGrouping<InstallationMeterUniqueIdentifier, KeyValuePair<int, InstallationMeterUniqueIdentifier>>, IEnumerable<KeyValuePair<int, InstallationMeterUniqueIdentifier>>>) (g => (IEnumerable<KeyValuePair<int, InstallationMeterUniqueIdentifier>>) g)).Distinct<KeyValuePair<int, InstallationMeterUniqueIdentifier>>().Select<KeyValuePair<int, InstallationMeterUniqueIdentifier>, InstallationMeterUniqueIdentifier>((Func<KeyValuePair<int, InstallationMeterUniqueIdentifier>, InstallationMeterUniqueIdentifier>) (x => x.Value)).ToList<InstallationMeterUniqueIdentifier>();
      duplicatesMeterIdentifier = list;
      return !meterUniqueIdentifierList.GroupBy<KeyValuePair<int, InstallationMeterUniqueIdentifier>, InstallationMeterUniqueIdentifier>((Func<KeyValuePair<int, InstallationMeterUniqueIdentifier>, InstallationMeterUniqueIdentifier>) (x => x.Value)).Where<IGrouping<InstallationMeterUniqueIdentifier, KeyValuePair<int, InstallationMeterUniqueIdentifier>>>((Func<IGrouping<InstallationMeterUniqueIdentifier, KeyValuePair<int, InstallationMeterUniqueIdentifier>>, bool>) (x => x.Count<KeyValuePair<int, InstallationMeterUniqueIdentifier>>() > 1)).Any<IGrouping<InstallationMeterUniqueIdentifier, KeyValuePair<int, InstallationMeterUniqueIdentifier>>>();
    }

    public static bool ValidationMeterUniqueness(
      StructureNodeDTO rootNodeDTO,
      OrderTypeEnum orderType,
      out List<MeterDTO> notUniqueReadingMeters,
      out List<MeterDTO> notUniqueInstallationMeters)
    {
      bool flag = true;
      Dictionary<object, ObservableCollection<StructureNodeDTO>> tenantDictionary = StructuresHelper.GetMetersByTenantDictionary(rootNodeDTO);
      notUniqueReadingMeters = new List<MeterDTO>();
      notUniqueInstallationMeters = new List<MeterDTO>();
      foreach (KeyValuePair<object, ObservableCollection<StructureNodeDTO>> meterForTenant in tenantDictionary)
      {
        List<MeterDTO> meterList = new List<MeterDTO>();
        TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) meterForTenant.Value, (Action<StructureNodeDTO>) (strNode =>
        {
          if (strNode.Entity == null)
            return;
          meterList.Add(strNode.Entity as MeterDTO);
        }));
        switch (orderType)
        {
          case OrderTypeEnum.ReadingOrder:
            List<ReadingMeterUniqueIdentifier> duplicatesMeterIdentifier1;
            if (!StructuresHelper.ValidateReadingMetersByTenantUniqueness(meterList, out duplicatesMeterIdentifier1, meterForTenant))
              flag = false;
            List<MeterDTO> meters1 = notUniqueReadingMeters;
            duplicatesMeterIdentifier1.ForEach((Action<ReadingMeterUniqueIdentifier>) (x =>
            {
              foreach (MeterDTO meterDto in meterList.Where<MeterDTO>((Func<MeterDTO, bool>) (y => y.SerialNumber == x.SerialNumber)).ToList<MeterDTO>())
              {
                MeterDTO duplicatedMeter = meterDto;
                duplicatedMeter.TenantNo = x.TenantNo;
                if (meters1.All<MeterDTO>((Func<MeterDTO, bool>) (m => m.SerialNumber != duplicatedMeter.SerialNumber)))
                  meters1.Add(duplicatedMeter);
              }
            }));
            notUniqueInstallationMeters = meters1;
            break;
          case OrderTypeEnum.InstallationOrder:
            List<InstallationMeterUniqueIdentifier> duplicatesMeterIdentifier2;
            if (!StructuresHelper.ValidateInstallationMetersByTenantUniqueness(meterList, out duplicatesMeterIdentifier2, meterForTenant))
              flag = false;
            List<MeterDTO> meters2 = notUniqueInstallationMeters;
            duplicatesMeterIdentifier2.ForEach((Action<InstallationMeterUniqueIdentifier>) (x =>
            {
              MeterDTO meterDto = meterList.FirstOrDefault<MeterDTO>((Func<MeterDTO, bool>) (y => y.SerialNumber == x.SerialNumber));
              if (meterDto == null)
                return;
              meterDto.TenantNo = x.TenantNo;
              meters2.Add(meterDto);
            }));
            notUniqueInstallationMeters = meters2;
            break;
        }
      }
      return flag;
    }

    public static bool ValidateTenantUniqueness(
      StructureNodeDTO rootNodeDTO,
      out List<TenantDTO> duplicates)
    {
      List<object> entity = StructuresHelper.GetEntityDictionary(rootNodeDTO)[FixedStructureNodeTypesEnum.Tenant];
      List<TenantDTO> tenantNoList = new List<TenantDTO>();
      entity.ForEach((Action<object>) (tenant =>
      {
        if (!(tenant is TenantDTO tenantDto2))
          return;
        tenantNoList.Add(tenantDto2);
      }));
      List<TenantDTO> list = tenantNoList.GroupBy<TenantDTO, int>((Func<TenantDTO, int>) (x => x.TenantNr)).Where<IGrouping<int, TenantDTO>>((Func<IGrouping<int, TenantDTO>, bool>) (g => g.Skip<TenantDTO>(1).Any<TenantDTO>())).SelectMany<IGrouping<int, TenantDTO>, TenantDTO>((Func<IGrouping<int, TenantDTO>, IEnumerable<TenantDTO>>) (g => (IEnumerable<TenantDTO>) g)).ToList<TenantDTO>();
      duplicates = new List<TenantDTO>();
      List<TenantDTO> dtos = duplicates;
      list.ForEach((Action<TenantDTO>) (x =>
      {
        if (!dtos.All<TenantDTO>((Func<TenantDTO, bool>) (y => y.TenantNr != x.TenantNr)))
          return;
        dtos.Add(x);
      }));
      return !dtos.Any<TenantDTO>();
    }

    public static bool ValidateMinomatUniqueness(
      StructureNodeDTO rootNodeDTO,
      out List<MinomatSerializableDTO> duplicates)
    {
      List<object> entity = StructuresHelper.GetEntityDictionary(rootNodeDTO)[FixedStructureNodeTypesEnum.Minomat];
      List<MinomatSerializableDTO> minomatNoList = new List<MinomatSerializableDTO>();
      entity.ForEach((Action<object>) (minomat =>
      {
        if (!(minomat is MinomatSerializableDTO minomatSerializableDto2))
          return;
        minomatNoList.Add(minomatSerializableDto2);
      }));
      List<MinomatSerializableDTO> list = minomatNoList.GroupBy<MinomatSerializableDTO, string>((Func<MinomatSerializableDTO, string>) (x => x.RadioId)).Where<IGrouping<string, MinomatSerializableDTO>>((Func<IGrouping<string, MinomatSerializableDTO>, bool>) (g => g.Count<MinomatSerializableDTO>() > 1)).SelectMany<IGrouping<string, MinomatSerializableDTO>, MinomatSerializableDTO>((Func<IGrouping<string, MinomatSerializableDTO>, IEnumerable<MinomatSerializableDTO>>) (g => (IEnumerable<MinomatSerializableDTO>) g)).ToList<MinomatSerializableDTO>();
      duplicates = new List<MinomatSerializableDTO>();
      List<MinomatSerializableDTO> dtos = duplicates;
      list.ForEach((Action<MinomatSerializableDTO>) (x =>
      {
        if (!dtos.All<MinomatSerializableDTO>((Func<MinomatSerializableDTO, bool>) (y => y.RadioId != x.RadioId)))
          return;
        dtos.Add(x);
      }));
      return !dtos.Any<MinomatSerializableDTO>();
    }

    public static Dictionary<FixedStructureNodeTypesEnum, List<object>> GetEntityDictionary(
      StructureNodeDTO selectedStructureNodeItem)
    {
      IEnumerable<StructureNodeDTO> structureNodeDtos = StructuresHelper.Descendants(selectedStructureNodeItem);
      List<TenantDTO> source1 = new List<TenantDTO>();
      List<MeterDTO> source2 = new List<MeterDTO>();
      List<LocationDTO> source3 = new List<LocationDTO>();
      List<MinomatSerializableDTO> source4 = new List<MinomatSerializableDTO>();
      Dictionary<FixedStructureNodeTypesEnum, List<object>> entityDictionary = new Dictionary<FixedStructureNodeTypesEnum, List<object>>();
      foreach (StructureNodeDTO structureNodeDto in structureNodeDtos)
      {
        object obj = new object();
        switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeDto.NodeType.Name, true))
        {
          case StructureNodeTypeEnum.Location:
            object entity1 = (object) (structureNodeDto.Entity as LocationDTO);
            source3.Add(entity1 as LocationDTO);
            break;
          case StructureNodeTypeEnum.Tenant:
            object entity2 = (object) (structureNodeDto.Entity as TenantDTO);
            source1.Add(entity2 as TenantDTO);
            break;
          case StructureNodeTypeEnum.Meter:
            object entity3 = (object) (structureNodeDto.Entity as MeterDTO);
            source2.Add(entity3 as MeterDTO);
            break;
          case StructureNodeTypeEnum.MinomatMaster:
          case StructureNodeTypeEnum.MinomatSlave:
            object entity4 = (object) (structureNodeDto.Entity as MinomatSerializableDTO);
            source4.Add(entity4 as MinomatSerializableDTO);
            break;
        }
      }
      entityDictionary.Add(FixedStructureNodeTypesEnum.Location, ((IEnumerable<object>) source3).ToList<object>());
      entityDictionary.Add(FixedStructureNodeTypesEnum.Tenant, ((IEnumerable<object>) source1).ToList<object>());
      entityDictionary.Add(FixedStructureNodeTypesEnum.Meter, ((IEnumerable<object>) source2).ToList<object>());
      entityDictionary.Add(FixedStructureNodeTypesEnum.Minomat, ((IEnumerable<object>) source4).ToList<object>());
      return entityDictionary;
    }

    public static Dictionary<object, ObservableCollection<StructureNodeDTO>> GetMetersByTenantDictionary(
      StructureNodeDTO selectedStructureNodeItem)
    {
      IEnumerable<StructureNodeDTO> structureNodeDtos = StructuresHelper.Descendants(selectedStructureNodeItem);
      Dictionary<object, ObservableCollection<StructureNodeDTO>> tenantDictionary = new Dictionary<object, ObservableCollection<StructureNodeDTO>>();
      foreach (StructureNodeDTO key in structureNodeDtos)
      {
        if ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), key.NodeType.Name, true) == StructureNodeTypeEnum.Tenant)
        {
          object entity = (object) (key.Entity as TenantDTO);
          if (entity != null)
            tenantDictionary.Add((object) (entity as TenantDTO), key.SubNodes);
          else
            tenantDictionary.Add((object) key, key.SubNodes);
        }
      }
      return tenantDictionary;
    }

    public static List<StructureNodeDTO> GetMeters(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      IEnumerable<StructureNodeDTO> structureNodeDtos = StructuresHelper.Descendants(nodeCollection.First<StructureNodeDTO>());
      List<StructureNodeDTO> meters = new List<StructureNodeDTO>();
      foreach (StructureNodeDTO structureNodeDto in structureNodeDtos)
      {
        if ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeDto.NodeType.Name, true) == StructureNodeTypeEnum.Meter && (object) (structureNodeDto.Entity as MeterDTO) != null)
          meters.Add(structureNodeDto);
      }
      return meters;
    }

    public static List<StructureNodeDTO> GetRadioMeters(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      IEnumerable<StructureNodeDTO> structureNodeDtos = StructuresHelper.Descendants(nodeCollection.First<StructureNodeDTO>());
      List<StructureNodeDTO> radioMeters = new List<StructureNodeDTO>();
      foreach (StructureNodeDTO structureNodeDto in structureNodeDtos)
      {
        if ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeDto.NodeType.Name, true) == StructureNodeTypeEnum.RadioMeter && (object) (structureNodeDto.Entity as MeterDTO) != null)
          radioMeters.Add(structureNodeDto);
      }
      return radioMeters;
    }

    public static bool HasAny(List<StructureNodeTypeEnum> enumList, StructureNodeDTO rootNode)
    {
      if (rootNode == null)
        return false;
      foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(rootNode))
      {
        StructureNodeTypeEnum structureNodeTypeEnum = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), descendant.NodeType.Name, true);
        if (enumList.Contains(structureNodeTypeEnum))
          return true;
      }
      return false;
    }

    public static void RemoveSelectedNodeFromStructure(
      StructureNodeDTO selectedNode,
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        if (node == selectedNode)
        {
          nodeCollection.Remove(node);
          break;
        }
        StructuresHelper.RemoveSelectedNodeFromStructure(selectedNode, node.SubNodes);
      }
    }

    public static StructureNodeDTO UnreferencedStructureNode(StructureNodeDTO node)
    {
      if (!node.SubNodes.Any<StructureNodeDTO>())
        return new StructureNodeDTO()
        {
          Image = node.Image,
          IsExpanded = node.IsExpanded,
          IsNewNode = node.IsNewNode,
          Name = node.Name,
          NodeType = node.NodeType,
          ParentNode = node.ParentNode,
          RootNode = node.RootNode,
          StructureType = node.StructureType,
          Entity = node.Entity,
          BackgroundColor = node.BackgroundColor,
          Description = node.Description,
          Id = node.Id
        };
      ObservableCollection<StructureNodeDTO> observableCollection = StructuresHelper.RecreateSubnodes((IEnumerable<StructureNodeDTO>) node.SubNodes);
      return new StructureNodeDTO()
      {
        Image = node.Image,
        IsExpanded = node.IsExpanded,
        IsNewNode = node.IsNewNode,
        Name = node.Name,
        NodeType = node.NodeType,
        ParentNode = node.ParentNode,
        RootNode = node.RootNode,
        StructureType = node.StructureType,
        Entity = node.Entity,
        BackgroundColor = node.BackgroundColor,
        Description = node.Description,
        Id = node.Id,
        SubNodes = observableCollection
      };
    }

    private static ObservableCollection<StructureNodeDTO> RecreateSubnodes(
      IEnumerable<StructureNodeDTO> subnodes)
    {
      ObservableCollection<StructureNodeDTO> newSubnodesList = new ObservableCollection<StructureNodeDTO>();
      TypeHelperExtensionMethods.ForEach<StructureNodeDTO>(subnodes, (Action<StructureNodeDTO>) (subnode => newSubnodesList.Add(StructuresHelper.UnreferencedStructureNode(subnode))));
      return newSubnodesList;
    }

    public static StructureNodeDTO GetStructureByDeviceType(
      StructureNodeDTO selectedNode,
      IEnumerable<DeviceTypeEnum> deviceTypes)
    {
      ObservableCollection<StructureNodeDTO> observableCollection1 = new ObservableCollection<StructureNodeDTO>();
      observableCollection1.Add(selectedNode);
      ObservableCollection<StructureNodeDTO> observableCollection2 = new ObservableCollection<StructureNodeDTO>();
      foreach (StructureNodeDTO subNode1 in (Collection<StructureNodeDTO>) selectedNode.SubNodes)
      {
        foreach (StructureNodeDTO subNode2 in (Collection<StructureNodeDTO>) subNode1.SubNodes)
        {
          if (subNode2.Entity != null)
          {
            MeterDTO entity = subNode2.Entity as MeterDTO;
            if (!deviceTypes.Contains<DeviceTypeEnum>(entity.DeviceType))
              observableCollection2.Add(subNode2);
          }
        }
      }
      foreach (StructureNodeDTO selectedNode1 in (Collection<StructureNodeDTO>) observableCollection2)
        StructuresHelper.RemoveSelectedNodeFromStructure(selectedNode1, observableCollection1);
      ObservableCollection<StructureNodeDTO> observableCollection3 = new ObservableCollection<StructureNodeDTO>();
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) observableCollection1.First<StructureNodeDTO>().SubNodes)
      {
        if (!subNode.SubNodes.Any<StructureNodeDTO>())
          observableCollection3.Add(subNode);
      }
      foreach (StructureNodeDTO selectedNode2 in (Collection<StructureNodeDTO>) observableCollection3)
        StructuresHelper.RemoveSelectedNodeFromStructure(selectedNode2, observableCollection1);
      return observableCollection1.First<StructureNodeDTO>();
    }

    public static DeviceTypeEnum? GetDeviceTypeEnumByDeviceModelName(string deviceModelName)
    {
      foreach (DeviceTypeEnum deviceTypeEnum in Enum.GetValues(typeof (DeviceTypeEnum)).Cast<DeviceTypeEnum>())
      {
        if (deviceTypeEnum.GetGMMDeviceModelName().Equals(deviceModelName))
          return new DeviceTypeEnum?(deviceTypeEnum);
      }
      return new DeviceTypeEnum?();
    }

    public static void SetNodesOrderNumber(
      this ObservableCollection<StructureNodeDTO> nodeCollection,
      StructureNodeDTO selectedStructureNode = null,
      StructureNodeDTO selectedNodeParent = null)
    {
      if (selectedNodeParent == null)
        StructuresHelper.SetOrderNrForCompleteStructure(nodeCollection);
      else
        StructuresHelper.SetOrderNrForPartialStructure(nodeCollection, selectedNodeParent, selectedStructureNode);
    }

    private static void SetOrderNrForCompleteStructure(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        ObservableCollection<StructureNodeDTO> subNodes = node.SubNodes;
        int num = 1;
        foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) subNodes)
        {
          structureNodeDto.OrderNr = num;
          ++num;
        }
      }
    }

    private static void SetOrderNrForPartialStructure(
      ObservableCollection<StructureNodeDTO> nodeCollection,
      StructureNodeDTO selectedNodeParent,
      StructureNodeDTO selectedStructureNode)
    {
      ObservableCollection<StructureNodeDTO> subNodes = selectedNodeParent.SubNodes;
      StructureNodeDTO structureNodeDto1 = subNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n == selectedStructureNode));
      if (structureNodeDto1 == null)
        return;
      int previousOrderNr = structureNodeDto1.OrderNr;
      int num = previousOrderNr;
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        node.OrderNr = num;
        ++num;
      }
      foreach (StructureNodeDTO structureNodeDto2 in subNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.OrderNr > previousOrderNr)))
      {
        structureNodeDto2.OrderNr = num;
        ++num;
      }
    }

    private static StructureNodeDTO GetParents(StructureNodeDTO node)
    {
      if (node.ParentNode == null)
        return node;
      StructureNodeDTO parentNode = node.ParentNode;
      StructureNodeDTO structureNodeDto = parentNode;
      ObservableCollection<StructureNodeDTO> observableCollection1 = new ObservableCollection<StructureNodeDTO>();
      observableCollection1.Add(node);
      ObservableCollection<StructureNodeDTO> observableCollection2 = observableCollection1;
      structureNodeDto.SubNodes = observableCollection2;
      StructuresHelper.GetParents(parentNode);
      return parentNode.RootNode;
    }

    public static StructureNodeDTO GetPartialStructureNodeDTO(
      ObservableCollection<StructureNodeDTO> selectedNodes)
    {
      ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>();
      ObservableCollection<StructureNodeDTO> source = new ObservableCollection<StructureNodeDTO>();
      foreach (StructureNodeDTO selectedNode in (Collection<StructureNodeDTO>) selectedNodes)
      {
        observableCollection.Add(selectedNode);
        foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(selectedNode))
        {
          StructureNodeDTO child = descendant;
          if (!source.Any<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.Equals((object) child))))
            source.Add(child);
          else
            observableCollection.Remove(child);
        }
      }
      StructureNodeDTO existingStructure = (StructureNodeDTO) null;
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) observableCollection)
        existingStructure = StructuresHelper.GetParentForMultipleSelectedNodes(node, existingStructure);
      return existingStructure;
    }

    private static StructureNodeDTO GetParentForMultipleSelectedNodes(
      StructureNodeDTO node,
      StructureNodeDTO existingStructure)
    {
      if (node.ParentNode == null)
        existingStructure = node;
      else if (existingStructure != null)
      {
        StructureNodeDTO parentNode = node.ParentNode;
        IEnumerable<StructureNodeDTO> structureNodeDtos = StructuresHelper.Descendants(existingStructure);
        bool flag = false;
        foreach (StructureNodeDTO structureNodeDto in structureNodeDtos)
        {
          if (structureNodeDto == parentNode)
          {
            structureNodeDto.SubNodes.Add(node);
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          StructureNodeDTO structureNodeDto = parentNode;
          ObservableCollection<StructureNodeDTO> observableCollection1 = new ObservableCollection<StructureNodeDTO>();
          observableCollection1.Add(node);
          ObservableCollection<StructureNodeDTO> observableCollection2 = observableCollection1;
          structureNodeDto.SubNodes = observableCollection2;
          StructuresHelper.GetParentForMultipleSelectedNodes(parentNode, existingStructure);
        }
      }
      else
      {
        StructureNodeDTO parentNode = node.ParentNode;
        StructureNodeDTO structureNodeDto = parentNode;
        ObservableCollection<StructureNodeDTO> observableCollection3 = new ObservableCollection<StructureNodeDTO>();
        observableCollection3.Add(node);
        ObservableCollection<StructureNodeDTO> observableCollection4 = observableCollection3;
        structureNodeDto.SubNodes = observableCollection4;
        StructuresHelper.GetParents(parentNode);
        existingStructure = parentNode.RootNode;
      }
      return existingStructure;
    }

    public static bool IsMeterWithMeterParent(StructureNodeDTO node)
    {
      if (node != null && (node.NodeType.Name == "Meter" || node.NodeType.Name == "RadioMeter"))
      {
        StructureNodeDTO parentNode = node.ParentNode;
        if (parentNode != null && (parentNode.NodeType.Name == "Meter" || parentNode.NodeType.Name == "RadioMeter"))
          return true;
      }
      return false;
    }
  }
}
