// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XmlNamespaceAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
  public class XmlNamespaceAttribute : Attribute
  {
    public XmlNamespaceAttribute(string namespaceUri, string prefix)
    {
      this.NamespaceUri = namespaceUri;
      this.Prefix = prefix;
    }

    public bool Default { get; set; }

    public string NamespaceUri { get; private set; }

    public string Prefix { get; private set; }
  }
}
