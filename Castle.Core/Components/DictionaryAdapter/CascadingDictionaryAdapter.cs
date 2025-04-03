// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.CascadingDictionaryAdapter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class CascadingDictionaryAdapter : AbstractDictionaryAdapter
  {
    private readonly IDictionary primary;
    private readonly IDictionary secondary;

    public CascadingDictionaryAdapter(IDictionary primary, IDictionary secondary)
    {
      this.primary = primary;
      this.secondary = secondary;
    }

    public IDictionary Primary => this.primary;

    public IDictionary Secondary => this.secondary;

    public override bool IsReadOnly => this.primary.IsReadOnly;

    public override bool Contains(object key)
    {
      return this.primary.Contains(key) || this.secondary.Contains(key);
    }

    public override object this[object key]
    {
      get => this.primary[key] ?? this.secondary[key];
      set => this.primary[key] = value;
    }
  }
}
