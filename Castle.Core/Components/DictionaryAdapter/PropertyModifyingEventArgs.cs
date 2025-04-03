// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.PropertyModifyingEventArgs
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class PropertyModifyingEventArgs : PropertyChangingEventArgs
  {
    public PropertyModifyingEventArgs(
      string propertyName,
      object oldPropertyValue,
      object newPropertyValue)
      : base(propertyName)
    {
      this.OldPropertyValue = oldPropertyValue;
      this.NewPropertyValue = newPropertyValue;
    }

    public object OldPropertyValue { get; private set; }

    public object NewPropertyValue { get; private set; }

    public bool Cancel { get; set; }
  }
}
