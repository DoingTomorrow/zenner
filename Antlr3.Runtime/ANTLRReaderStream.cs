// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ANTLRReaderStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.IO;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class ANTLRReaderStream : ANTLRStringStream
  {
    public const int ReadBufferSize = 1024;
    public const int InitialBufferSize = 1024;

    public ANTLRReaderStream(TextReader r)
      : this(r, 1024, 1024)
    {
    }

    public ANTLRReaderStream(TextReader r, int size)
      : this(r, size, 1024)
    {
    }

    public ANTLRReaderStream(TextReader r, int size, int readChunkSize)
    {
      this.Load(r, size, readChunkSize);
    }

    public virtual void Load(TextReader r, int size, int readChunkSize)
    {
      if (r == null)
        return;
      if (size <= 0)
        size = 1024;
      if (readChunkSize <= 0)
        readChunkSize = 1024;
      try
      {
        this.data = r.ReadToEnd().ToCharArray();
        this.n = this.data.Length;
      }
      finally
      {
        r.Close();
      }
    }
  }
}
