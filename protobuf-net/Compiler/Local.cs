// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Compiler.Local
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace ProtoBuf.Compiler
{
  internal sealed class Local : IDisposable
  {
    private LocalBuilder value;
    private CompilerContext ctx;
    private readonly Type type;

    public Type Type => this.type;

    public Local AsCopy() => this.ctx == null ? this : new Local(this.value, this.type);

    internal LocalBuilder Value
    {
      get
      {
        return this.value != null ? this.value : throw new ObjectDisposedException(this.GetType().Name);
      }
    }

    public void Dispose()
    {
      if (this.ctx == null)
        return;
      this.ctx.ReleaseToPool(this.value);
      this.value = (LocalBuilder) null;
      this.ctx = (CompilerContext) null;
    }

    private Local(LocalBuilder value, Type type)
    {
      this.value = value;
      this.type = type;
    }

    internal Local(CompilerContext ctx, Type type)
    {
      this.ctx = ctx;
      if (ctx != null)
        this.value = ctx.GetFromPool(type);
      this.type = type;
    }

    internal bool IsSame(Local other)
    {
      if (this == other)
        return true;
      object obj = (object) this.value;
      return other != null && obj == other.value;
    }
  }
}
