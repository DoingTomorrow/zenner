// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationSchema.EventConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Xml.XPath;

#nullable disable
namespace NHibernate.Cfg.ConfigurationSchema
{
  public class EventConfiguration
  {
    private ListenerType type;
    private IList<ListenerConfiguration> listeners = (IList<ListenerConfiguration>) new List<ListenerConfiguration>();

    internal EventConfiguration(XPathNavigator eventElement) => this.Parse(eventElement);

    public EventConfiguration(ListenerConfiguration listener, ListenerType type)
    {
      if (listener == null)
        throw new ArgumentNullException(nameof (listener));
      this.type = type;
      this.listeners.Add(listener);
    }

    private void Parse(XPathNavigator eventElement)
    {
      XPathNavigator xpathNavigator = eventElement.Clone();
      eventElement.MoveToFirstAttribute();
      this.type = CfgXmlHelper.ListenerTypeConvertFrom(eventElement.Value);
      XPathNodeIterator xpathNodeIterator = xpathNavigator.SelectDescendants(XPathNodeType.Element, false);
      while (xpathNodeIterator.MoveNext())
        this.listeners.Add(new ListenerConfiguration(xpathNodeIterator.Current, this.type));
    }

    public ListenerType Type => this.type;

    public IList<ListenerConfiguration> Listeners => this.listeners;
  }
}
