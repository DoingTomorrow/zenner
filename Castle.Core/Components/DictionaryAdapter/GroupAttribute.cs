// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.GroupAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class GroupAttribute : Attribute
  {
    public GroupAttribute(object group)
    {
      this.Group = new object[1]{ group };
    }

    public GroupAttribute(params object[] group) => this.Group = group;

    public object[] Group { get; private set; }
  }
}
