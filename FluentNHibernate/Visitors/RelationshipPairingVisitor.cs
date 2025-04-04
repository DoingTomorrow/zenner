// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.RelationshipPairingVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public class RelationshipPairingVisitor : DefaultMappingModelVisitor
  {
    private readonly PairBiDirectionalManyToManySidesDelegate userControlledPair;
    private readonly List<CollectionMapping> manyToManys = new List<CollectionMapping>();
    private readonly List<CollectionMapping> oneToManys = new List<CollectionMapping>();
    private readonly List<ManyToOneMapping> references = new List<ManyToOneMapping>();

    public RelationshipPairingVisitor(
      PairBiDirectionalManyToManySidesDelegate userControlledPair)
    {
      this.userControlledPair = userControlledPair;
    }

    public override void Visit(IEnumerable<HibernateMapping> mappings)
    {
      base.Visit(mappings);
      this.PairManyToManys((IEnumerable<CollectionMapping>) this.manyToManys);
      RelationshipPairingVisitor.PairOneToManys((IEnumerable<CollectionMapping>) this.oneToManys, (IEnumerable<ManyToOneMapping>) this.references);
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
      if (mapping.Relationship is ManyToManyMapping)
        this.manyToManys.Add(mapping);
      if (!(mapping.Relationship is OneToManyMapping))
        return;
      this.oneToManys.Add(mapping);
    }

    public override void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
    {
      this.references.Add(manyToOneMapping);
    }

    private static void PairOneToManys(
      IEnumerable<CollectionMapping> collections,
      IEnumerable<ManyToOneMapping> refs)
    {
      CollectionMapping[] array1 = collections.OrderBy<CollectionMapping, string>((Func<CollectionMapping, string>) (x => x.Name)).ToArray<CollectionMapping>();
      ManyToOneMapping[] array2 = refs.OrderBy<ManyToOneMapping, string>((Func<ManyToOneMapping, string>) (x => x.Name)).ToArray<ManyToOneMapping>();
      foreach (CollectionMapping collectionMapping in array1)
      {
        Type type = collectionMapping.ContainingEntityType;
        ManyToOneMapping manyToOneMapping = ((IEnumerable<ManyToOneMapping>) array2).FirstOrDefault<ManyToOneMapping>((Func<ManyToOneMapping, bool>) (x => x.OtherSide == null && x.Class.GetUnderlyingSystemType() == type));
        if (manyToOneMapping != null)
        {
          collectionMapping.OtherSide = (IRelationship) manyToOneMapping;
          manyToOneMapping.OtherSide = (IRelationship) collectionMapping;
        }
      }
    }

    private void PairManyToManys(IEnumerable<CollectionMapping> rs)
    {
      if (!rs.Any<CollectionMapping>())
        return;
      CollectionMapping current = rs.First<CollectionMapping>();
      bool wasResolved = true;
      IEnumerable<CollectionMapping> collectionMappings = rs.Where<CollectionMapping>((Func<CollectionMapping, bool>) (x => RelationshipPairingVisitor.both_collections_point_to_each_others_types(x, current)));
      CollectionMapping current1 = current;
      int num = collectionMappings.Count<CollectionMapping>();
      if (num == 1)
        current1 = RelationshipPairingVisitor.PairExactMatches(rs, current, collectionMappings);
      else if (num > 1)
        current1 = RelationshipPairingVisitor.PairFuzzyMatches(rs, current, collectionMappings);
      if (current1 == null)
      {
        current1 = current;
        wasResolved = false;
      }
      this.userControlledPair(current1, rs, wasResolved);
      this.PairManyToManys(rs.Except<CollectionMapping>(current1, (CollectionMapping) current1.OtherSide));
    }

    private static string GetMemberName(Member member)
    {
      return member is MethodMember && member.Name.StartsWith("Get") ? member.Name.Substring(3) : member.Name;
    }

    private static RelationshipPairingVisitor.LikenessContainer GetLikeness(
      CollectionMapping currentMapping,
      CollectionMapping mapping)
    {
      string memberName1 = RelationshipPairingVisitor.GetMemberName(currentMapping.Member);
      string memberName2 = RelationshipPairingVisitor.GetMemberName(mapping.Member);
      return new RelationshipPairingVisitor.LikenessContainer()
      {
        Collection = currentMapping,
        CurrentMemberName = memberName1,
        MappingMemberName = memberName2,
        Differences = StringLikeness.EditDistance(memberName1, memberName2)
      };
    }

    private static bool both_collections_point_to_each_others_types(
      CollectionMapping left,
      CollectionMapping right)
    {
      return left.ContainingEntityType == right.ChildType && left.ChildType == right.ContainingEntityType;
    }

    private static bool self_referenced_relation_does_not_point_to_itself(
      CollectionMapping left,
      CollectionMapping right)
    {
      return left.ChildType == left.ContainingEntityType && right.ChildType == right.ContainingEntityType && left != right;
    }

    private static bool likeness_within_threshold(
      RelationshipPairingVisitor.LikenessContainer current)
    {
      return current.Differences != current.CurrentMemberName.Length;
    }

    private static CollectionMapping PairFuzzyMatches(
      IEnumerable<CollectionMapping> rs,
      CollectionMapping current,
      IEnumerable<CollectionMapping> potentialOtherSides)
    {
      CollectionMapping current1 = current;
      IOrderedEnumerable<RelationshipPairingVisitor.LikenessContainer> orderedEnumerable = potentialOtherSides.Select<CollectionMapping, RelationshipPairingVisitor.LikenessContainer>((Func<CollectionMapping, RelationshipPairingVisitor.LikenessContainer>) (x => RelationshipPairingVisitor.GetLikeness(x, current))).Where<RelationshipPairingVisitor.LikenessContainer>(new Func<RelationshipPairingVisitor.LikenessContainer, bool>(RelationshipPairingVisitor.likeness_within_threshold)).OrderBy<RelationshipPairingVisitor.LikenessContainer, int>((Func<RelationshipPairingVisitor.LikenessContainer, int>) (x => x.Differences));
      RelationshipPairingVisitor.LikenessContainer current2 = orderedEnumerable.FirstOrDefault<RelationshipPairingVisitor.LikenessContainer>();
      if (current2 == null || RelationshipPairingVisitor.AnyHaveSameLikeness((IEnumerable<RelationshipPairingVisitor.LikenessContainer>) orderedEnumerable, current2))
        return (CollectionMapping) null;
      CollectionMapping collection = orderedEnumerable.First<RelationshipPairingVisitor.LikenessContainer>().Collection;
      CollectionMapping collectionMapping = RelationshipPairingVisitor.FindAlternative(rs, current1, collection) ?? current1;
      collectionMapping.OtherSide = (IRelationship) collection;
      collection.OtherSide = (IRelationship) collectionMapping;
      return collectionMapping;
    }

    private static CollectionMapping PairExactMatches(
      IEnumerable<CollectionMapping> rs,
      CollectionMapping current,
      IEnumerable<CollectionMapping> potentialOtherSides)
    {
      CollectionMapping otherSide = potentialOtherSides.Single<CollectionMapping>();
      CollectionMapping collectionMapping = RelationshipPairingVisitor.FindAlternative(rs, current, otherSide) ?? current;
      collectionMapping.OtherSide = (IRelationship) otherSide;
      otherSide.OtherSide = (IRelationship) collectionMapping;
      return collectionMapping;
    }

    private static CollectionMapping FindAlternative(
      IEnumerable<CollectionMapping> rs,
      CollectionMapping current,
      CollectionMapping otherSide)
    {
      return rs.Where<CollectionMapping>((Func<CollectionMapping, bool>) (x => RelationshipPairingVisitor.self_referenced_relation_does_not_point_to_itself(x, current))).Where<CollectionMapping>((Func<CollectionMapping, bool>) (x => x.ContainingEntityType == current.ContainingEntityType)).Select<CollectionMapping, RelationshipPairingVisitor.LikenessContainer>((Func<CollectionMapping, RelationshipPairingVisitor.LikenessContainer>) (x => RelationshipPairingVisitor.GetLikeness(x, otherSide))).OrderBy<RelationshipPairingVisitor.LikenessContainer, int>((Func<RelationshipPairingVisitor.LikenessContainer, int>) (x => x.Differences)).FirstOrDefault<RelationshipPairingVisitor.LikenessContainer>()?.Collection;
    }

    private static bool AnyHaveSameLikeness(
      IEnumerable<RelationshipPairingVisitor.LikenessContainer> likenesses,
      RelationshipPairingVisitor.LikenessContainer current)
    {
      return likenesses.Except<RelationshipPairingVisitor.LikenessContainer>(current).Any<RelationshipPairingVisitor.LikenessContainer>((Func<RelationshipPairingVisitor.LikenessContainer, bool>) (x => x.Differences == current.Differences));
    }

    private class LikenessContainer
    {
      public CollectionMapping Collection { get; set; }

      public string CurrentMemberName { get; set; }

      public string MappingMemberName { get; set; }

      public int Differences { get; set; }

      public override bool Equals(object obj)
      {
        return obj is RelationshipPairingVisitor.LikenessContainer && ((RelationshipPairingVisitor.LikenessContainer) obj).CurrentMemberName == this.CurrentMemberName && ((RelationshipPairingVisitor.LikenessContainer) obj).MappingMemberName == this.MappingMemberName;
      }

      public override int GetHashCode()
      {
        return (this.CurrentMemberName != null ? this.CurrentMemberName.GetHashCode() : 0) * 397 ^ (this.MappingMemberName != null ? this.MappingMemberName.GetHashCode() : 0);
      }
    }
  }
}
