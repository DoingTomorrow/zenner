// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DynamicValue`1
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public abstract class DynamicValue<T> : IDynamicValue<T>, IDynamicValue
  {
    object IDynamicValue.GetValue() => (object) this.Value;

    public abstract T Value { get; }

    public override string ToString()
    {
      T obj = this.Value;
      return (object) obj != null ? obj.ToString() : base.ToString();
    }
  }
}
