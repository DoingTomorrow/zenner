// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DictionaryBehaviorAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public abstract class DictionaryBehaviorAttribute : Attribute, IDictionaryBehavior
  {
    public const int FiratExecutionOrder = 0;
    public const int DefaultExecutionOrder = 1073741823;
    public const int LastExecutionOrder = 2147483647;

    public DictionaryBehaviorAttribute() => this.ExecutionOrder = 1073741823;

    public int ExecutionOrder { get; set; }
  }
}
