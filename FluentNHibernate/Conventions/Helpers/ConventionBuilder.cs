// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.ConventionBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers
{
  public static class ConventionBuilder
  {
    public static IConventionBuilder<IClassConvention, IClassInspector, IClassInstance> Class
    {
      get
      {
        return (IConventionBuilder<IClassConvention, IClassInspector, IClassInstance>) new ClassConventionBuilder();
      }
    }

    public static IConventionBuilder<IIdConvention, IIdentityInspector, IIdentityInstance> Id
    {
      get
      {
        return (IConventionBuilder<IIdConvention, IIdentityInspector, IIdentityInstance>) new IdConventionBuilder();
      }
    }

    public static IConventionBuilder<IPropertyConvention, IPropertyInspector, IPropertyInstance> Property
    {
      get
      {
        return (IConventionBuilder<IPropertyConvention, IPropertyInspector, IPropertyInstance>) new PropertyConventionBuilder();
      }
    }

    public static IConventionBuilder<IHasManyConvention, IOneToManyCollectionInspector, IOneToManyCollectionInstance> HasMany
    {
      get
      {
        return (IConventionBuilder<IHasManyConvention, IOneToManyCollectionInspector, IOneToManyCollectionInstance>) new HasManyConventionBuilder();
      }
    }

    public static IConventionBuilder<IHasManyToManyConvention, IManyToManyCollectionInspector, IManyToManyCollectionInstance> HasManyToMany
    {
      get
      {
        return (IConventionBuilder<IHasManyToManyConvention, IManyToManyCollectionInspector, IManyToManyCollectionInstance>) new HasManyToManyConventionBuilder();
      }
    }

    public static IConventionBuilder<IReferenceConvention, IManyToOneInspector, IManyToOneInstance> Reference
    {
      get
      {
        return (IConventionBuilder<IReferenceConvention, IManyToOneInspector, IManyToOneInstance>) new ReferenceConventionBuilder();
      }
    }
  }
}
