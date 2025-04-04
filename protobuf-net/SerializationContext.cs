// Decompiled with JetBrains decompiler
// Type: ProtoBuf.SerializationContext
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace ProtoBuf
{
  public sealed class SerializationContext
  {
    private bool frozen;
    private object context;
    private static readonly SerializationContext @default = new SerializationContext();
    private StreamingContextStates state = StreamingContextStates.Persistence;

    internal void Freeze() => this.frozen = true;

    private void ThrowIfFrozen()
    {
      if (this.frozen)
        throw new InvalidOperationException("The serialization-context cannot be changed once it is in use");
    }

    public object Context
    {
      get => this.context;
      set
      {
        if (this.context == value)
          return;
        this.ThrowIfFrozen();
        this.context = value;
      }
    }

    static SerializationContext() => SerializationContext.@default.Freeze();

    internal static SerializationContext Default => SerializationContext.@default;

    public StreamingContextStates State
    {
      get => this.state;
      set
      {
        if (this.state == value)
          return;
        this.ThrowIfFrozen();
        this.state = value;
      }
    }

    public static implicit operator StreamingContext(SerializationContext ctx)
    {
      return ctx == null ? new StreamingContext(StreamingContextStates.Persistence) : new StreamingContext(ctx.state, ctx.context);
    }

    public static implicit operator SerializationContext(StreamingContext ctx)
    {
      return new SerializationContext()
      {
        Context = ctx.Context,
        State = ctx.State
      };
    }
  }
}
