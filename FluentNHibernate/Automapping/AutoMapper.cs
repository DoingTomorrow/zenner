// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoMapper
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class AutoMapper
  {
    private List<AutoMapType> mappingTypes;
    private readonly IAutomappingConfiguration cfg;
    private readonly IConventionFinder conventionFinder;
    private readonly IEnumerable<InlineOverride> inlineOverrides;

    public AutoMapper(
      IAutomappingConfiguration cfg,
      IConventionFinder conventionFinder,
      IEnumerable<InlineOverride> inlineOverrides)
    {
      this.cfg = cfg;
      this.conventionFinder = conventionFinder;
      this.inlineOverrides = inlineOverrides;
    }

    private void ApplyOverrides(
      Type classType,
      IList<Member> mappedMembers,
      ClassMappingBase mapping)
    {
      object autoMap = Activator.CreateInstance(typeof (AutoMapping<>).MakeGenericType(classType), (object) mappedMembers);
      this.inlineOverrides.Where<InlineOverride>((Func<InlineOverride, bool>) (x => x.CanOverride(classType))).Each<InlineOverride>((Action<InlineOverride>) (x => x.Apply(autoMap)));
      ((IAutoClasslike) autoMap).AlterModel(mapping);
    }

    public ClassMappingBase MergeMap(
      Type classType,
      ClassMappingBase mapping,
      IList<Member> mappedMembers)
    {
      this.ApplyOverrides(classType, mappedMembers, mapping);
      this.ProcessClass(mapping, classType, mappedMembers);
      if (this.mappingTypes != null)
        this.MapInheritanceTree(classType, mapping, mappedMembers);
      return mapping;
    }

    private void MapInheritanceTree(
      Type classType,
      ClassMappingBase mapping,
      IList<Member> mappedMembers)
    {
      bool flag1 = AutoMapper.HasDiscriminator(mapping);
      bool flag2 = this.cfg.IsDiscriminated(classType) || flag1;
      foreach (AutoMapType inheritedClass in this.GetMappingTypesWithLogicalParents().Where<KeyValuePair<AutoMapType, AutoMapType>>((Func<KeyValuePair<AutoMapType, AutoMapType>, bool>) (x => x.Value != null && x.Value.Type == classType)).Select<KeyValuePair<AutoMapType, AutoMapType>, AutoMapType>((Func<KeyValuePair<AutoMapType, AutoMapType>, AutoMapType>) (x => x.Key)))
      {
        ClassMapping classMapping = mapping as ClassMapping;
        bool flag3 = classMapping == null;
        if (flag2 && !flag1 && !flag3)
        {
          string discriminatorColumn = this.cfg.GetDiscriminatorColumn(classType);
          DiscriminatorMapping discriminatorMapping = new DiscriminatorMapping()
          {
            ContainingEntityType = classType
          };
          discriminatorMapping.Set((Expression<Func<DiscriminatorMapping, object>>) (x => x.Type), 0, (object) new TypeReference(typeof (string)));
          ColumnMapping mapping1 = new ColumnMapping();
          mapping1.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, discriminatorColumn);
          discriminatorMapping.AddColumn(0, mapping1);
          classMapping.Set<DiscriminatorMapping>((Expression<Func<ClassMapping, DiscriminatorMapping>>) (x => x.Discriminator), 0, discriminatorMapping);
          flag1 = true;
        }
        SubclassMapping subclassMapping;
        if (!flag3 && classMapping.IsUnionSubclass)
        {
          subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
          subclassMapping.Set<Type>((Expression<Func<SubclassMapping, Type>>) (x => x.Type), 0, inheritedClass.Type);
        }
        else if (!flag2)
        {
          subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
          subclassMapping.Set<Type>((Expression<Func<SubclassMapping, Type>>) (x => x.Type), 0, inheritedClass.Type);
          subclassMapping.Set<KeyMapping>((Expression<Func<SubclassMapping, KeyMapping>>) (x => x.Key), 0, new KeyMapping());
          ColumnMapping mapping2 = new ColumnMapping();
          mapping2.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, mapping.Type.Name + "_id");
          subclassMapping.Key.AddColumn(0, mapping2);
        }
        else
        {
          subclassMapping = new SubclassMapping(SubclassType.Subclass);
          subclassMapping.Set<Type>((Expression<Func<SubclassMapping, Type>>) (x => x.Type), 0, inheritedClass.Type);
        }
        List<Member> mappedMembers1 = new List<Member>((IEnumerable<Member>) mappedMembers);
        this.MapSubclass((IList<Member>) mappedMembers1, subclassMapping, inheritedClass);
        mapping.AddSubclass(subclassMapping);
        this.MergeMap(inheritedClass.Type, (ClassMappingBase) subclassMapping, (IList<Member>) mappedMembers1);
      }
    }

    private static bool HasDiscriminator(ClassMappingBase mapping)
    {
      return mapping is ClassMapping && ((ClassMapping) mapping).Discriminator != null;
    }

    private Dictionary<AutoMapType, AutoMapType> GetMappingTypesWithLogicalParents()
    {
      Dictionary<Type, AutoMapType> dictionary = this.mappingTypes.Except<AutoMapType>(this.mappingTypes.Where<AutoMapType>((Func<AutoMapType, bool>) (x => this.cfg.IsConcreteBaseType(x.Type.BaseType))).ToArray<AutoMapType>()).ToDictionary<AutoMapType, Type>((Func<AutoMapType, Type>) (x => x.Type));
      Dictionary<AutoMapType, AutoMapType> withLogicalParents = new Dictionary<AutoMapType, AutoMapType>();
      foreach (KeyValuePair<Type, AutoMapType> keyValuePair in dictionary)
        withLogicalParents.Add(keyValuePair.Value, AutoMapper.GetLogicalParent(keyValuePair.Key, (IDictionary<Type, AutoMapType>) dictionary));
      return withLogicalParents;
    }

    private static AutoMapType GetLogicalParent(
      Type type,
      IDictionary<Type, AutoMapType> availableTypes)
    {
      if (type.BaseType == typeof (object) || type.BaseType == null)
        return (AutoMapType) null;
      AutoMapType autoMapType;
      return availableTypes.TryGetValue(type.BaseType, out autoMapType) ? autoMapType : AutoMapper.GetLogicalParent(type.BaseType, availableTypes);
    }

    private void MapSubclass(
      IList<Member> mappedMembers,
      SubclassMapping subclass,
      AutoMapType inheritedClass)
    {
      subclass.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.Name), 0, inheritedClass.Type.AssemblyQualifiedName);
      subclass.Set<Type>((Expression<Func<SubclassMapping, Type>>) (x => x.Type), 0, inheritedClass.Type);
      this.ApplyOverrides(inheritedClass.Type, mappedMembers, (ClassMappingBase) subclass);
      this.ProcessClass((ClassMappingBase) subclass, inheritedClass.Type, mappedMembers);
      inheritedClass.IsMapped = true;
    }

    public virtual void ProcessClass(
      ClassMappingBase mapping,
      Type entityType,
      IList<Member> mappedMembers)
    {
      entityType.GetInstanceMembers().Where<Member>(new Func<Member, bool>(this.cfg.ShouldMap)).Each<Member>((Action<Member>) (x => this.TryMapProperty(mapping, x, mappedMembers)));
    }

    private void TryMapProperty(
      ClassMappingBase mapping,
      Member member,
      IList<Member> mappedMembers)
    {
      if (member.HasIndexParameters)
        return;
      foreach (IAutomappingStep mappingStep in this.cfg.GetMappingSteps(this, this.conventionFinder))
      {
        if (mappingStep.ShouldMap(member) && !mappedMembers.Contains(member))
        {
          mappingStep.Map(mapping, member);
          mappedMembers.Add(member);
          break;
        }
      }
    }

    public ClassMapping Map(Type classType, List<AutoMapType> types)
    {
      ClassMapping mapping = new ClassMapping();
      mapping.Set<Type>((Expression<Func<ClassMapping, Type>>) (x => x.Type), 0, classType);
      mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.Name), 0, classType.AssemblyQualifiedName);
      mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.TableName), 0, AutoMapper.GetDefaultTableName(classType));
      this.mappingTypes = types;
      return (ClassMapping) this.MergeMap(classType, (ClassMappingBase) mapping, (IList<Member>) new List<Member>());
    }

    private static string GetDefaultTableName(Type type)
    {
      string str = type.Name;
      if (type.IsGenericType)
      {
        str = type.Name.Substring(0, type.Name.IndexOf('`'));
        foreach (Type genericArgument in type.GetGenericArguments())
          str = str + "_" + genericArgument.Name;
      }
      return "`" + str + "`";
    }

    public void FlagAsMapped(Type type)
    {
      this.mappingTypes.Where<AutoMapType>((Func<AutoMapType, bool>) (x => x.Type == type)).Each<AutoMapType>((Action<AutoMapType>) (x => x.IsMapped = true));
    }
  }
}
