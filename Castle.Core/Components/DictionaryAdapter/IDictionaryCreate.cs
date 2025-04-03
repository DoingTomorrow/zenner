// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.IDictionaryCreate
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public interface IDictionaryCreate
  {
    T Create<T>();

    object Create(Type type);

    T Create<T>(IDictionary dictionary);

    object Create(Type type, IDictionary dictionary);

    T Create<T>(Action<T> init);

    T Create<T>(IDictionary dictionary, Action<T> init);
  }
}
