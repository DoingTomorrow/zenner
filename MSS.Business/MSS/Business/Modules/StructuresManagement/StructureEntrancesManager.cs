// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.StructureEntrancesManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Structures;
using MSS.DTO.Structures;
using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public class StructureEntrancesManager
  {
    private IRepositoryFactory _repositoryFactory;

    public List<string> GetStructureEntrances(
      StructureNodeDTO node,
      IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      List<string> first = new List<string>();
      List<Guid> minomatGuidList = new List<Guid>();
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.RootNode.SubNodes)
      {
        if (subNode.Entity != null)
        {
          string name = subNode.NodeType?.Name;
          if (name != null)
          {
            switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), name, true))
            {
              case StructureNodeTypeEnum.Tenant:
                string entrance = subNode.Entity is TenantDTO entity1 ? entity1.Entrance : (string) null;
                if (entrance != null && !first.Contains(entrance))
                {
                  first.Add(entrance);
                  break;
                }
                break;
              case StructureNodeTypeEnum.MinomatMaster:
                if (subNode.Entity is MinomatSerializableDTO entity3)
                {
                  Guid id1 = entity3.Id;
                  if (id1 != Guid.Empty)
                    minomatGuidList.Add(id1);
                  using (IEnumerator<StructureNodeDTO> enumerator = subNode.SubNodes.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      if (enumerator.Current.Entity is MinomatSerializableDTO entity2)
                      {
                        Guid id2 = entity2.Id;
                        if (id2 != Guid.Empty)
                          minomatGuidList.Add(id2);
                      }
                    }
                    break;
                  }
                }
                else
                  break;
            }
          }
        }
      }
      List<string> stringList;
      if (minomatGuidList.Count <= 0)
        stringList = new List<string>();
      else
        stringList = this._repositoryFactory.GetRepository<MinomatRadioDetails>().Where((Expression<Func<MinomatRadioDetails, bool>>) (x => minomatGuidList.Contains(x.Minomat.Id))).Select<MinomatRadioDetails, string>((Expression<Func<MinomatRadioDetails, string>>) (x => x.Entrance)).Distinct<string>().ToList<string>();
      List<string> second = stringList;
      return first.Union<string>((IEnumerable<string>) second).OrderBy<string, string>((Func<string, string>) (x => x)).ToList<string>();
    }
  }
}
