// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.TypeReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public abstract class TypeReference : Reference
  {
    private readonly Type type;

    protected TypeReference(Type argumentType)
      : this((Reference) null, argumentType)
    {
    }

    protected TypeReference(Reference owner, Type type)
      : base(owner)
    {
      this.type = type;
    }

    public Type Type => this.type;
  }
}
