// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.ConfigResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Castle.Core.Resource
{
  public class ConfigResource : AbstractResource
  {
    private readonly XmlNode configSectionNode;
    private readonly string sectionName;

    public ConfigResource()
      : this("castle")
    {
    }

    public ConfigResource(CustomUri uri)
      : this(uri.Host)
    {
    }

    public ConfigResource(string sectionName)
    {
      this.sectionName = sectionName;
      this.configSectionNode = (XmlNode) ConfigurationManager.GetSection(sectionName) ?? throw new ConfigurationErrorsException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Could not find section '{0}' in the configuration file associated with this domain.", (object) sectionName));
    }

    public override TextReader GetStreamReader()
    {
      return (TextReader) new StringReader(this.configSectionNode.OuterXml);
    }

    public override TextReader GetStreamReader(Encoding encoding)
    {
      throw new NotSupportedException("Encoding is not supported");
    }

    public override IResource CreateRelative(string relativePath)
    {
      return (IResource) new ConfigResource(relativePath);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "ConfigResource: [{0}]", (object) this.sectionName);
    }
  }
}
