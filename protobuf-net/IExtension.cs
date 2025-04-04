// Decompiled with JetBrains decompiler
// Type: ProtoBuf.IExtension
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System.IO;

#nullable disable
namespace ProtoBuf
{
  public interface IExtension
  {
    Stream BeginAppend();

    void EndAppend(Stream stream, bool commit);

    Stream BeginQuery();

    void EndQuery(Stream stream);

    int GetLength();
  }
}
