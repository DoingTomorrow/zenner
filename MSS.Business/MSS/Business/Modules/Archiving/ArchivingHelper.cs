// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.ArchivingHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Core.Model.Archiving;
using MSS.Core.Model.Structures;
using MSS.DTO.Archive;
using MSSArchive.Core.Model.DataCollectors;
using MSSArchive.Core.Model.Meters;
using MSSArchive.Core.Model.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  public static class ArchivingHelper
  {
    public static byte[] SerializeArchivedEntities(List<ArchiveEntity> archivedEntities)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(archivedEntities.GetType());
      MemoryStream memoryStream = new MemoryStream();
      xmlSerializer.Serialize((Stream) memoryStream, (object) archivedEntities);
      return memoryStream.ToArray();
    }

    public static List<ArchiveEntity> DeserializeArchivedEntities(byte[] structureBytes)
    {
      return new XmlSerializer(typeof (List<ArchiveEntity>)).Deserialize((Stream) new MemoryStream(structureBytes)) as List<ArchiveEntity>;
    }

    public static string ArchivedEntitiesString(byte[] structureBytes)
    {
      return ArchivingHelper.DeserializeArchivedEntities(structureBytes).Where<ArchiveEntity>((Func<ArchiveEntity, bool>) (entity => entity.IsChecked)).Aggregate<ArchiveEntity, string>(string.Empty, (Func<string, ArchiveEntity, string>) ((current, entity) => current + entity.Name + "; ")).Trim().TrimEnd(';');
    }

    public static List<ArchiveStructureNodeDTO> CreateArchiveStructureCollection(
      IOrderedEnumerable<ArchiveStructureNodeLinks> archiveStructureNodeLinks,
      List<ArchiveStructureNode> archiveStructureNodes,
      Dictionary<Guid, object> entitiesDictionary)
    {
      Mapper.CreateMap<ArchiveMeter, ArchiveMeterDTO>();
      Mapper.CreateMap<ArchiveLocation, ArchiveLocationDTO>();
      Mapper.CreateMap<ArchiveTenant, ArchiveTenantDTO>();
      Mapper.CreateMap<ArchiveMinomat, ArchiveMinomatDTO>();
      List<ArchiveStructureNodeDTO> structureCollection = new List<ArchiveStructureNodeDTO>();
      IList<ArchiveStructureNodeLinks> list = (IList<ArchiveStructureNodeLinks>) archiveStructureNodeLinks.Where<ArchiveStructureNodeLinks>((Func<ArchiveStructureNodeLinks, bool>) (x => x.ParentNodeId == Guid.Empty && x.RootNodeId == x.NodeId)).ToList<ArchiveStructureNodeLinks>();
      Dictionary<Guid, \u003C\u003Ef__AnonymousType0<Guid, ILookup<Guid, ArchiveStructureNodeLinks>>> dictionary = archiveStructureNodeLinks.GroupBy((Func<ArchiveStructureNodeLinks, Guid>) (p => p.RootNodeId), (key, g) => new
      {
        TreeRootNode = key,
        TreeElements = g.ToLookup<ArchiveStructureNodeLinks, Guid>((Func<ArchiveStructureNodeLinks, Guid>) (x => x.ParentNodeId))
      }).ToDictionary(x => x.TreeRootNode);
      foreach (ArchiveStructureNodeLinks structureNodeLink in (IEnumerable<ArchiveStructureNodeLinks>) list)
      {
        ArchiveStructureNodeDTO newNode = ArchivingHelper.CreateNewNode(structureNodeLink, (IEnumerable<ArchiveStructureNode>) archiveStructureNodes, entitiesDictionary);
        newNode.RootNode = newNode;
        structureCollection.Add(newNode);
        Guid id = newNode.Id;
        if (dictionary.ContainsKey(id))
        {
          ILookup<Guid, ArchiveStructureNodeLinks> treeElements = dictionary[id].TreeElements;
          ArchivingHelper.FillChild(newNode, newNode.Id, treeElements, archiveStructureNodes, entitiesDictionary);
        }
      }
      return structureCollection;
    }

    private static ArchiveStructureNodeDTO CreateNewNode(
      ArchiveStructureNodeLinks structureNodeLink,
      IEnumerable<ArchiveStructureNode> archiveStructureNodes,
      Dictionary<Guid, object> entitiesDictionary)
    {
      ArchiveStructureNodeDTO node = new ArchiveStructureNodeDTO();
      if (structureNodeLink.NodeId != Guid.Empty)
      {
        node.Id = structureNodeLink.NodeId;
        ArchiveStructureNode archiveStructureNode = archiveStructureNodes.FirstOrDefault<ArchiveStructureNode>((Func<ArchiveStructureNode, bool>) (n => n.Id == structureNodeLink.NodeId));
        if (archiveStructureNode != null)
        {
          node.Name = archiveStructureNode.Name;
          node.Description = archiveStructureNode.Description;
          node.NodeType = archiveStructureNode.NodeType;
          if (archiveStructureNode.EntityId != Guid.Empty)
          {
            object entities = entitiesDictionary[archiveStructureNode.EntityId];
            if (entities != null)
            {
              object archiveEntityDto = ArchivingHelper.GetArchiveEntityDTO(archiveStructureNode.NodeType, entities);
              node.Entity = archiveEntityDto;
            }
          }
        }
        node.StructureType = new StructureTypeEnum?(structureNodeLink.StructureType);
        node.Image = ArchivingHelper.GetImageForNode(node);
      }
      return node;
    }

    private static object GetArchiveEntityDTO(
      StructureNodeTypeEnum structureNodeTypeName,
      object entityObj)
    {
      object obj = new object();
      object archiveEntityDto = new object();
      switch (structureNodeTypeName)
      {
        case StructureNodeTypeEnum.Location:
          archiveEntityDto = (object) Mapper.Map<ArchiveLocation, ArchiveLocationDTO>(entityObj as ArchiveLocation);
          break;
        case StructureNodeTypeEnum.Tenant:
          archiveEntityDto = (object) Mapper.Map<ArchiveTenant, ArchiveTenantDTO>(entityObj as ArchiveTenant);
          break;
        case StructureNodeTypeEnum.Meter:
          archiveEntityDto = (object) Mapper.Map<ArchiveMeter, ArchiveMeterDTO>(entityObj as ArchiveMeter);
          break;
        case StructureNodeTypeEnum.MinomatMaster:
        case StructureNodeTypeEnum.MinomatSlave:
          archiveEntityDto = (object) Mapper.Map<ArchiveMinomat, ArchiveMinomatDTO>(entityObj as ArchiveMinomat);
          break;
      }
      return archiveEntityDto;
    }

    private static BitmapImage GetImageForNode(ArchiveStructureNodeDTO node)
    {
      switch (node.NodeType)
      {
        case StructureNodeTypeEnum.City:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/city.png"));
        case StructureNodeTypeEnum.CityArea:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/cityarea.png"));
        case StructureNodeTypeEnum.COMServer:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/comserver.png"));
        case StructureNodeTypeEnum.Converter:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/converter.png"));
        case StructureNodeTypeEnum.Country:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/country.png"));
        case StructureNodeTypeEnum.DeviceGroup:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/devicegroup.png"));
        case StructureNodeTypeEnum.Flat:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/flat.png"));
        case StructureNodeTypeEnum.Floor:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/floor.png"));
        case StructureNodeTypeEnum.House:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/house.png"));
        case StructureNodeTypeEnum.Manifold:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/manifold.png"));
        case StructureNodeTypeEnum.Repeater:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/repeater.png"));
        case StructureNodeTypeEnum.Street:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/street.png"));
        case StructureNodeTypeEnum.User:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/user.png"));
        case StructureNodeTypeEnum.UserGroup:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/usergroup.png"));
        case StructureNodeTypeEnum.Location:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/location.png"));
        case StructureNodeTypeEnum.Tenant:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/tenant.png"));
        case StructureNodeTypeEnum.Meter:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/meter.png"));
        case StructureNodeTypeEnum.Radio:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/Radio.png"));
        case StructureNodeTypeEnum.MinomatMaster:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/minomatm.png"));
        case StructureNodeTypeEnum.MinomatSlave:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/minomats.png"));
        case StructureNodeTypeEnum.RadioMeter:
          return new BitmapImage(new Uri("pack://application:,,,/Styles;component/Images/StructureIcons/radiometer.png"));
        default:
          return new BitmapImage(new Uri(""));
      }
    }

    private static void FillChild(
      ArchiveStructureNodeDTO parentNode,
      Guid parentNodeId,
      ILookup<Guid, ArchiveStructureNodeLinks> currentTreeLookup,
      List<ArchiveStructureNode> archiveStructureNodes,
      Dictionary<Guid, object> entitiesDictionary)
    {
      if (!currentTreeLookup.Contains(parentNodeId))
        return;
      foreach (ArchiveStructureNodeLinks structureNodeLink in currentTreeLookup[parentNodeId])
      {
        ArchiveStructureNodeDTO newNode = ArchivingHelper.CreateNewNode(structureNodeLink, (IEnumerable<ArchiveStructureNode>) archiveStructureNodes, entitiesDictionary);
        parentNode.Children.Add(newNode);
        ArchivingHelper.FillChild(newNode, newNode.Id, currentTreeLookup, archiveStructureNodes, entitiesDictionary);
      }
    }

    public static IEnumerable<ArchiveStructureNodeDTO> Descendants(ArchiveStructureNodeDTO root)
    {
      Stack<ArchiveStructureNodeDTO> nodes = new Stack<ArchiveStructureNodeDTO>((IEnumerable<ArchiveStructureNodeDTO>) new ArchiveStructureNodeDTO[1]
      {
        root
      });
      while (nodes.Any<ArchiveStructureNodeDTO>())
      {
        ArchiveStructureNodeDTO node = nodes.Pop();
        yield return node;
        foreach (ArchiveStructureNodeDTO n in (Collection<ArchiveStructureNodeDTO>) node.Children)
          nodes.Push(n);
        node = (ArchiveStructureNodeDTO) null;
      }
    }

    public static List<ArchiveStructureNodeDTO> FilterArchiveStructureCollectionByName(
      string searchText,
      List<ArchiveStructureNodeDTO> allArchiveStrNodeCollectionDto)
    {
      if (searchText == "")
        return allArchiveStrNodeCollectionDto;
      List<ArchiveStructureNodeDTO> structureNodeDtoList = new List<ArchiveStructureNodeDTO>();
      foreach (ArchiveStructureNodeDTO root in allArchiveStrNodeCollectionDto)
      {
        List<ArchiveStructureNodeDTO> list = ArchivingHelper.Descendants(root).ToList<ArchiveStructureNodeDTO>();
        list.Add(root);
        if (list.Any<ArchiveStructureNodeDTO>((Func<ArchiveStructureNodeDTO, bool>) (d => d.Name.Contains(searchText))))
        {
          foreach (ArchiveStructureNodeDTO structureNodeDto in list.Where<ArchiveStructureNodeDTO>((Func<ArchiveStructureNodeDTO, bool>) (d => d.Name.Contains(searchText))))
          {
            structureNodeDto.BackgroundColor = (Brush) Brushes.LightGreen;
            structureNodeDto.IsExpanded = true;
          }
          structureNodeDtoList.Add(root);
        }
      }
      return structureNodeDtoList;
    }
  }
}
