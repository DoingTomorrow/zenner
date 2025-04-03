// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.SingletonRepository
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public sealed class SingletonRepository
  {
    private readonly SortedList<string, object> list = new SortedList<string, object>();

    private SingletonRepository()
    {
    }

    public static SingletonRepository Instance => SingletonRepository.Nested.instance;

    public T Get<T>(string key) => this.list.ContainsKey(key) ? (T) this.list[key] : default (T);

    public void Add<T>(string key, object obj)
    {
      if (this.list.ContainsKey(key))
        throw new Exception("Object already exists");
      this.list.Add(key, obj);
    }

    private class Nested
    {
      internal static readonly SingletonRepository instance = new SingletonRepository();
    }
  }
}
