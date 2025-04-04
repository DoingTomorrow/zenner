// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSchema.ListenerConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Event;
using System;
using System.Xml.XPath;

#nullable disable
namespace NHibernate.Cfg.ConfigurationSchema
{
  public class ListenerConfiguration
  {
    private string clazz;
    private ListenerType type;

    internal ListenerConfiguration(XPathNavigator listenerElement) => this.Parse(listenerElement);

    internal ListenerConfiguration(XPathNavigator listenerElement, ListenerType defaultType)
    {
      this.type = defaultType;
      this.Parse(listenerElement);
    }

    public ListenerConfiguration(string clazz)
    {
      this.clazz = !string.IsNullOrEmpty(clazz) ? clazz : throw new ArgumentException("clazz is null or empty.", nameof (clazz));
    }

    public ListenerConfiguration(string clazz, ListenerType type)
      : this(clazz)
    {
      this.type = type;
    }

    private void Parse(XPathNavigator listenerElement)
    {
      if (!listenerElement.MoveToFirstAttribute())
        return;
      do
      {
        switch (listenerElement.Name)
        {
          case "class":
            if (listenerElement.Value.Trim().Length == 0)
              throw new HibernateConfigException("Invalid listener element; the attribute <class> must be assigned with no empty value");
            this.clazz = listenerElement.Value;
            break;
          case "type":
            this.type = CfgXmlHelper.ListenerTypeConvertFrom(listenerElement.Value);
            break;
        }
      }
      while (listenerElement.MoveToNextAttribute());
    }

    public string Class => this.clazz;

    public ListenerType Type => this.type;
  }
}
