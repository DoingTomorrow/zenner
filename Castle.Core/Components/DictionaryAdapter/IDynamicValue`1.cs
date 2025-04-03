// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.IDynamicValue`1
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public interface IDynamicValue<T> : IDynamicValue
  {
    T Value { get; }
  }
}
