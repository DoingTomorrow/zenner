// Decompiled with JetBrains decompiler
// Type: ProtoBuf.BufferPool
// Assembly: protobuf-net, Version=2.1.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 9CE408C1-C78A-444E-8AE3-93D4A699B63F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\protobuf-net.dll

using System.Threading;

#nullable disable
namespace ProtoBuf
{
  internal sealed class BufferPool
  {
    private const int PoolSize = 20;
    internal const int BufferLength = 1024;
    private static readonly object[] pool = new object[20];

    internal static void Flush()
    {
      for (int index = 0; index < BufferPool.pool.Length; ++index)
        Interlocked.Exchange(ref BufferPool.pool[index], (object) null);
    }

    private BufferPool()
    {
    }

    internal static byte[] GetBuffer()
    {
      for (int index = 0; index < BufferPool.pool.Length; ++index)
      {
        object buffer;
        if ((buffer = Interlocked.Exchange(ref BufferPool.pool[index], (object) null)) != null)
          return (byte[]) buffer;
      }
      return new byte[1024];
    }

    internal static void ResizeAndFlushLeft(
      ref byte[] buffer,
      int toFitAtLeastBytes,
      int copyFromIndex,
      int copyBytes)
    {
      int length = buffer.Length * 2;
      if (length < toFitAtLeastBytes)
        length = toFitAtLeastBytes;
      byte[] to = new byte[length];
      if (copyBytes > 0)
        Helpers.BlockCopy(buffer, copyFromIndex, to, 0, copyBytes);
      if (buffer.Length == 1024)
        BufferPool.ReleaseBufferToPool(ref buffer);
      buffer = to;
    }

    internal static void ReleaseBufferToPool(ref byte[] buffer)
    {
      if (buffer == null)
        return;
      if (buffer.Length == 1024)
      {
        int index = 0;
        while (index < BufferPool.pool.Length && Interlocked.CompareExchange(ref BufferPool.pool[index], (object) buffer, (object) null) != null)
          ++index;
      }
      buffer = (byte[]) null;
    }
  }
}
