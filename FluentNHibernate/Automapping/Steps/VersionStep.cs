// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.VersionStep
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class VersionStep : IAutomappingStep
  {
    private readonly IAutomappingConfiguration cfg;

    public VersionStep(IAutomappingConfiguration cfg) => this.cfg = cfg;

    public bool ShouldMap(Member member) => this.cfg.IsVersion(member);

    public void Map(ClassMappingBase classMap, Member member)
    {
      if (!(classMap is ClassMapping))
        return;
      VersionMapping mapping1 = new VersionMapping()
      {
        ContainingEntityType = classMap.Type
      };
      mapping1.Set<string>((Expression<Func<VersionMapping, string>>) (x => x.Name), 0, member.Name);
      mapping1.Set<TypeReference>((Expression<Func<VersionMapping, TypeReference>>) (x => x.Type), 0, VersionStep.GetDefaultType(member));
      ColumnMapping mapping2 = new ColumnMapping();
      mapping2.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, member.Name);
      mapping1.AddColumn(0, mapping2);
      this.SetDefaultAccess(member, mapping1);
      if (VersionStep.IsSqlTimestamp(member))
      {
        mapping1.Columns.Each<ColumnMapping>((Action<ColumnMapping>) (column =>
        {
          column.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.SqlType), 0, "timestamp");
          column.Set<bool>((Expression<Func<ColumnMapping, bool>>) (x => x.NotNull), 0, true);
        }));
        mapping1.Set<string>((Expression<Func<VersionMapping, string>>) (x => x.UnsavedValue), 0, (string) null);
      }
      ((ClassMapping) classMap).Set<VersionMapping>((Expression<Func<ClassMapping, VersionMapping>>) (x => x.Version), 0, mapping1);
    }

    private void SetDefaultAccess(Member member, VersionMapping mapping)
    {
      Access access = MemberAccessResolver.Resolve(member);
      if (access != Access.Property && access != Access.Unset)
        mapping.Set<string>((Expression<Func<VersionMapping, string>>) (x => x.Access), 0, access.ToString());
      if (!member.IsProperty || member.CanWrite)
        return;
      mapping.Set<string>((Expression<Func<VersionMapping, string>>) (x => x.Access), 0, this.cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
    }

    private static bool IsSqlTimestamp(Member property) => property.PropertyType == typeof (byte[]);

    private static TypeReference GetDefaultType(Member property)
    {
      return VersionStep.IsSqlTimestamp(property) ? new TypeReference("BinaryBlob") : new TypeReference(property.PropertyType);
    }
  }
}
