// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Configuration.HibernateMappingsAssembly
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using System.Configuration;

#nullable disable
namespace Common.Library.NHibernate.Data.Configuration
{
  public class HibernateMappingsAssembly : ConfigurationElement
  {
    [StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|\\")]
    [ConfigurationProperty("fullyQualifiedName", DefaultValue = "", IsRequired = true, IsKey = true)]
    public string FullyQualifiedName
    {
      get => (string) this["fullyQualifiedName"];
      set => this["fullyQualifiedName"] = (object) value;
    }
  }
}
