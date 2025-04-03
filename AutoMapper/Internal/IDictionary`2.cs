// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.IDictionary`2
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper.Internal
{
  public interface IDictionary<TKey, TValue>
  {
    TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory);

    bool TryGetValue(TKey key, out TValue value);

    TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);

    TValue this[TKey key] { get; set; }

    bool TryRemove(TKey key, out TValue value);
  }
}
