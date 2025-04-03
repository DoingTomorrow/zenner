// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.StructureNodesValidator
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Business.Modules.GMM;
using MSS.Core.Model.Structures;
using MSS.Utils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public class StructureNodesValidator
  {
    protected internal Dictionary<StructureNodeTypeEnum, List<StructureNodeTypeEnum?>> _invalidCombinations;

    public StructureNodesValidator()
    {
      this._invalidCombinations = new Dictionary<StructureNodeTypeEnum, List<StructureNodeTypeEnum?>>();
      this._invalidCombinations.Add(StructureNodeTypeEnum.Meter, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Location),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatMaster),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatSlave),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Radio)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.COMServer, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.City, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.CityArea, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Converter, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Country, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.RadioMeter, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.COMServer),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.City),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.CityArea),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Converter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Country),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Minomat),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatMaster),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatSlave),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.DeviceGroup),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Flat),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Floor),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.House),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Location),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Manifold),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Repeater),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Street),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Tenant),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.User),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.UserDevice),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.UserGroup)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Minomat, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Tenant),
        new StructureNodeTypeEnum?()
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.MinomatMaster, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Tenant),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatMaster),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatSlave),
        new StructureNodeTypeEnum?(),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.MinomatSlave, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Tenant),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatSlave),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Location),
        new StructureNodeTypeEnum?(),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.DeviceGroup, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Flat, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Floor, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.House, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Location, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Tenant),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Location),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatMaster),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatSlave),
        new StructureNodeTypeEnum?(),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Manifold, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Repeater, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Street, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.Tenant, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Tenant),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatMaster),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.MinomatSlave),
        new StructureNodeTypeEnum?(),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.User, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.UserDevice, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
      this._invalidCombinations.Add(StructureNodeTypeEnum.UserGroup, new List<StructureNodeTypeEnum?>()
      {
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.Meter),
        new StructureNodeTypeEnum?(StructureNodeTypeEnum.RadioMeter)
      });
    }

    public List<StructureNodeLinks> PhysicalLinks { get; set; }

    public bool IsValidNodesRelationship(
      StructureNodeDTO node,
      StructureNodeDTO parentNode,
      bool isDragFromTreelist)
    {
      if (node == null)
        return false;
      StructureNodeTypeEnum key = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), node.NodeType.Name, true);
      if (key == StructureNodeTypeEnum.Folder)
        return false;
      StructureNodeTypeEnum? typeParent = new StructureNodeTypeEnum?();
      if (parentNode != null)
      {
        StructureNodeDTO root = parentNode.RootNode == null || parentNode.RootNode == parentNode ? parentNode : parentNode.RootNode;
        StructureTypeEnum? structureType = root.StructureType;
        StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Logical;
        if (structureType.GetValueOrDefault() == structureTypeEnum1 && structureType.HasValue)
        {
          IEnumerable<StructureNodeDTO> source1 = StructuresHelper.Descendants(root);
          if (!isDragFromTreelist)
          {
            structureType = node.StructureType;
            StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Physical;
            if (structureType.GetValueOrDefault() == structureTypeEnum2 && structureType.HasValue && (source1.Any<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (s => s.Id == node.Id)) || node.Entity == null && (node.NodeType.Name == StructureNodeTypeEnum.Meter.GetStringName() || node.NodeType.Name == StructureNodeTypeEnum.Location.GetStringName() || node.NodeType.Name == StructureNodeTypeEnum.Tenant.GetStringName())))
              return false;
          }
          else
          {
            StructureNodeDTO structureNodeDto1 = node.RootNode == null || node.RootNode == node || !(node.RootNode.NodeType.Name != StructureNodeTypeEnum.Folder.GetStringName()) ? node : node.RootNode;
            if (root != structureNodeDto1)
            {
              IEnumerable<StructureNodeDTO> source2 = StructuresHelper.Descendants(node);
              foreach (StructureNodeDTO structureNodeDto2 in source1)
              {
                StructureNodeDTO structureNodeDto = structureNodeDto2;
                if (source2.Any<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (nodeDto => structureNodeDto.Id == nodeDto.Id && nodeDto.Id != Guid.Empty)))
                  return false;
              }
            }
          }
        }
        typeParent = new StructureNodeTypeEnum?((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), parentNode.NodeType.Name, true));
      }
      else if (this.PhysicalLinks != null && this.PhysicalLinks.Any<StructureNodeLinks>())
      {
        StructureTypeEnum? structureType;
        int num1;
        if (!node.IsNewNode)
        {
          structureType = node.StructureType;
          if (structureType.HasValue)
          {
            structureType = node.StructureType;
            num1 = structureType.Value != StructureTypeEnum.Logical ? 0 : (this.PhysicalLinks.Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (l => l.Node.Id)).Contains<Guid>(node.Id) ? 1 : 0);
            goto label_26;
          }
        }
        num1 = 0;
label_26:
        int num2 = isDragFromTreelist ? 1 : 0;
        if ((num1 & num2) != 0)
          return false;
        int num3;
        if (!node.IsNewNode)
        {
          structureType = node.StructureType;
          if (structureType.HasValue)
          {
            structureType = node.StructureType;
            num3 = structureType.Value == StructureTypeEnum.Physical ? 1 : 0;
            goto label_32;
          }
        }
        num3 = 0;
label_32:
        int num4 = isDragFromTreelist ? 1 : 0;
        if ((num3 & num4) != 0)
          return false;
        int num5;
        if (!node.IsNewNode)
        {
          structureType = node.StructureType;
          if (structureType.HasValue)
          {
            structureType = node.StructureType;
            if (structureType.Value == StructureTypeEnum.Physical)
            {
              num5 = !isDragFromTreelist ? 1 : 0;
              goto label_39;
            }
          }
        }
        num5 = 0;
label_39:
        if (num5 != 0)
          return false;
      }
      else
      {
        StructureTypeEnum? structureType;
        int num6;
        if (!node.IsNewNode)
        {
          structureType = node.StructureType;
          if (structureType.HasValue)
          {
            structureType = node.StructureType;
            if (structureType.Value == StructureTypeEnum.Physical)
            {
              num6 = !isDragFromTreelist ? 1 : 0;
              goto label_46;
            }
          }
        }
        num6 = 0;
label_46:
        if (num6 != 0)
          return false;
        int num7;
        if (isDragFromTreelist && node.RootNode != null)
        {
          structureType = node.RootNode.StructureType;
          StructureTypeEnum structureTypeEnum3 = StructureTypeEnum.Logical;
          if ((structureType.GetValueOrDefault() == structureTypeEnum3 ? (structureType.HasValue ? 1 : 0) : 0) != 0)
          {
            structureType = node.StructureType;
            StructureTypeEnum structureTypeEnum4 = StructureTypeEnum.Physical;
            if ((structureType.GetValueOrDefault() == structureTypeEnum4 ? (structureType.HasValue ? 1 : 0) : 0) != 0)
            {
              num7 = node.RootNode != node ? 1 : 0;
              goto label_53;
            }
          }
        }
        num7 = 0;
label_53:
        if (num7 != 0)
          return false;
      }
      if (ScanMinoConnectManager.IsScanningStarted || WalkByTestManager.IsWalkByTestStarted)
        return false;
      List<StructureNodeTypeEnum?> source;
      this._invalidCombinations.TryGetValue(key, out source);
      return source == null || source.All<StructureNodeTypeEnum?>((Func<StructureNodeTypeEnum?, bool>) (n =>
      {
        StructureNodeTypeEnum? nullable1 = n;
        StructureNodeTypeEnum? nullable2 = typeParent;
        return nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() || nullable1.HasValue != nullable2.HasValue;
      }));
    }
  }
}
