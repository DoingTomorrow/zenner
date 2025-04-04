// Decompiled with JetBrains decompiler
// Type: NHibernate.IDatabinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;
using System.Xml;

#nullable disable
namespace NHibernate
{
  public interface IDatabinder
  {
    IDatabinder Bind(object obj);

    IDatabinder BindAll(ICollection objs);

    string ToGenericXml();

    XmlDocument ToGenericXmlDocument();

    string ToXML();

    XmlDocument ToXmlDocument();

    bool InitializeLazy { get; set; }
  }
}
