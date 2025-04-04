// Decompiled with JetBrains decompiler
// Type: ProtoBuf.BufferExtension
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System.IO;

#nullable disable
namespace ProtoBuf
{
  public sealed class BufferExtension : IExtension
  {
    private byte[] buffer;

    int IExtension.GetLength() => this.buffer != null ? this.buffer.Length : 0;

    Stream IExtension.BeginAppend() => (Stream) new MemoryStream();

    void IExtension.EndAppend(Stream stream, bool commit)
    {
      using (stream)
      {
        int length1;
        if (!commit || (length1 = (int) stream.Length) <= 0)
          return;
        MemoryStream ms = (MemoryStream) stream;
        if (this.buffer == null)
        {
          this.buffer = ms.ToArray();
        }
        else
        {
          int length2 = this.buffer.Length;
          byte[] to = new byte[length2 + length1];
          Helpers.BlockCopy(this.buffer, 0, to, 0, length2);
          Helpers.BlockCopy(Helpers.GetBuffer(ms), 0, to, length2, length1);
          this.buffer = to;
        }
      }
    }

    Stream IExtension.BeginQuery()
    {
      return this.buffer != null ? (Stream) new MemoryStream(this.buffer) : Stream.Null;
    }

    void IExtension.EndQuery(Stream stream)
    {
      using (stream)
        ;
    }
  }
}
