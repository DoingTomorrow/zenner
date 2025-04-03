// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Configuration.HibernateMappingsAssemblyCollection
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using System.Configuration;

#nullable disable
namespace Common.Library.NHibernate.Data.Configuration
{
  [ConfigurationCollection(typeof (HibernateMappingsAssembly))]
  public class HibernateMappingsAssemblyCollection : ConfigurationElementCollection
  {
    public override ConfigurationElementCollectionType CollectionType
    {
      get => ConfigurationElementCollectionType.AddRemoveClearMap;
    }

    public HibernateMappingsAssembly this[int index]
    {
      get => (HibernateMappingsAssembly) this.BaseGet(index);
      set
      {
        if (this.BaseGet(index) != null)
          this.BaseRemoveAt(index);
        this.BaseAdd(index, (ConfigurationElement) value);
      }
    }

    public void Add(HibernateMappingsAssembly element)
    {
      this.BaseAdd((ConfigurationElement) element);
    }

    public void Clear() => this.BaseClear();

    protected override ConfigurationElement CreateNewElement()
    {
      return (ConfigurationElement) new HibernateMappingsAssembly();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return (object) ((HibernateMappingsAssembly) element).FullyQualifiedName;
    }
  }
}
