// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSchema.MappingConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Xml.XPath;

#nullable disable
namespace NHibernate.Cfg.ConfigurationSchema
{
  public class MappingConfiguration : IEquatable<MappingConfiguration>
  {
    private string file;
    private string assembly;
    private string resource;

    internal MappingConfiguration(XPathNavigator mappingElement) => this.Parse(mappingElement);

    public MappingConfiguration(string file)
    {
      this.file = !string.IsNullOrEmpty(file) ? file : throw new ArgumentException("file is null or empty.", nameof (file));
    }

    public MappingConfiguration(string assembly, string resource)
    {
      this.assembly = !string.IsNullOrEmpty(assembly) ? assembly : throw new ArgumentException("assembly is null or empty.", nameof (assembly));
      this.resource = resource;
    }

    private bool IsValid()
    {
      return !string.IsNullOrEmpty(this.assembly) && string.IsNullOrEmpty(this.file) || !string.IsNullOrEmpty(this.file) && string.IsNullOrEmpty(this.assembly) || !string.IsNullOrEmpty(this.resource) && !string.IsNullOrEmpty(this.assembly) || this.IsEmpty();
    }

    private void Parse(XPathNavigator mappingElement)
    {
      if (mappingElement.MoveToFirstAttribute())
      {
        do
        {
          switch (mappingElement.Name)
          {
            case "assembly":
              this.assembly = mappingElement.Value;
              break;
            case "resource":
              this.resource = mappingElement.Value;
              break;
            case "file":
              this.file = mappingElement.Value;
              break;
          }
        }
        while (mappingElement.MoveToNextAttribute());
      }
      if (!this.IsValid())
        throw new HibernateConfigException(string.Format("Ambiguous mapping tag in configuration assembly={0} resource={1} file={2};\r\nThere are 3 possible combinations of mapping attributes\r\n\t1 - resource & assembly:  NHibernate will read the mapping resource from the specified assembly\r\n\t2 - file only: NHibernate will read the mapping from the file.\r\n\t3 - assembly only: NHibernate will find all the resources ending in hbm.xml from the assembly.", (object) this.assembly, (object) this.resource, (object) this.file));
    }

    public bool IsEmpty()
    {
      return string.IsNullOrEmpty(this.resource) && string.IsNullOrEmpty(this.assembly) && string.IsNullOrEmpty(this.file);
    }

    public string File => this.file;

    public string Assembly => this.assembly;

    public string Resource => this.resource;

    public bool Equals(MappingConfiguration other)
    {
      if (other == null)
        return false;
      if (!string.IsNullOrEmpty(this.file) && !string.IsNullOrEmpty(other.file))
        return this.file.Equals(other.file);
      if (!string.IsNullOrEmpty(this.assembly) && !string.IsNullOrEmpty(other.assembly) && (string.IsNullOrEmpty(this.resource) || string.IsNullOrEmpty(other.resource)))
        return this.assembly.Equals(other.assembly);
      return !string.IsNullOrEmpty(this.assembly) && !string.IsNullOrEmpty(this.resource) && !string.IsNullOrEmpty(other.assembly) && !string.IsNullOrEmpty(other.resource) && this.assembly.Equals(other.assembly) && this.resource.Equals(other.resource);
    }

    public override string ToString()
    {
      return string.Format("file='{0}';assembly='{1}';resource='{2}'", (object) this.file, (object) this.assembly, (object) this.resource);
    }
  }
}
