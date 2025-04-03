// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MetaTypeElement
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public abstract class MetaTypeElement
  {
    protected readonly Type sourceType;

    protected MetaTypeElement(Type sourceType) => this.sourceType = sourceType;

    internal bool CanBeImplementedExplicitly
    {
      get => this.sourceType != null && this.sourceType.IsInterface;
    }

    internal abstract void SwitchToExplicitImplementation();
  }
}
