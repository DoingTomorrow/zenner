// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.MutableList
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

#nullable disable
namespace ProtoBuf.Meta
{
  internal sealed class MutableList : BasicList
  {
    public new object this[int index]
    {
      get => this.head[index];
      set => this.head[index] = value;
    }

    public void RemoveLast() => this.head.RemoveLastWithMutate();

    public void Clear() => this.head.Clear();
  }
}
