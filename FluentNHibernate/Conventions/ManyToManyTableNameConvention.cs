// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.ManyToManyTableNameConvention
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public abstract class ManyToManyTableNameConvention : 
    IHasManyToManyConvention,
    IConvention<IManyToManyCollectionInspector, IManyToManyCollectionInstance>,
    IConvention
  {
    public void Apply(IManyToManyCollectionInstance instance)
    {
      if (instance.OtherSide == null)
      {
        string directionalTableName = this.GetUniDirectionalTableName((IManyToManyCollectionInspector) instance);
        instance.Table(directionalTableName);
      }
      else
      {
        string directionalTableName = this.GetBiDirectionalTableName((IManyToManyCollectionInspector) instance.OtherSide, (IManyToManyCollectionInspector) instance);
        instance.Table(directionalTableName);
        instance.OtherSide.Table(directionalTableName);
      }
    }

    protected abstract string GetBiDirectionalTableName(
      IManyToManyCollectionInspector collection,
      IManyToManyCollectionInspector otherSide);

    protected abstract string GetUniDirectionalTableName(IManyToManyCollectionInspector collection);
  }
}
