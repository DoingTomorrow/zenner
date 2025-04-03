// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.IDictionaryAdapterFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.XPath;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public interface IDictionaryAdapterFactory
  {
    T GetAdapter<T>(IDictionary dictionary);

    object GetAdapter(Type type, IDictionary dictionary);

    object GetAdapter(Type type, IDictionary dictionary, PropertyDescriptor descriptor);

    T GetAdapter<T>(NameValueCollection nameValues);

    object GetAdapter(Type type, NameValueCollection nameValues);

    T GetAdapter<T>(IXPathNavigable xpathNavigable);

    object GetAdapter(Type type, IXPathNavigable xpathNavigable);

    DictionaryAdapterMeta GetAdapterMeta(Type type);

    DictionaryAdapterMeta GetAdapterMeta(Type type, PropertyDescriptor descriptor);
  }
}
