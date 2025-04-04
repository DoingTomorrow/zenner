// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ANTLRReaderStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.IO;

#nullable disable
namespace Antlr.Runtime
{
  internal class ANTLRReaderStream : ANTLRStringStream
  {
    public static readonly int READ_BUFFER_SIZE = 1024;
    public static readonly int INITIAL_BUFFER_SIZE = 1024;

    protected ANTLRReaderStream()
    {
    }

    public ANTLRReaderStream(TextReader reader)
      : this(reader, ANTLRReaderStream.INITIAL_BUFFER_SIZE, ANTLRReaderStream.READ_BUFFER_SIZE)
    {
    }

    public ANTLRReaderStream(TextReader reader, int size)
      : this(reader, size, ANTLRReaderStream.READ_BUFFER_SIZE)
    {
    }

    public ANTLRReaderStream(TextReader reader, int size, int readChunkSize)
    {
      this.Load(reader, size, readChunkSize);
    }

    public virtual void Load(TextReader reader, int size, int readChunkSize)
    {
      if (reader == null)
        return;
      if (size <= 0)
        size = ANTLRReaderStream.INITIAL_BUFFER_SIZE;
      if (readChunkSize <= 0)
        readChunkSize = ANTLRReaderStream.READ_BUFFER_SIZE;
      try
      {
        this.data = new char[size];
        int index = 0;
        int num;
        do
        {
          if (index + readChunkSize > this.data.Length)
          {
            char[] destinationArray = new char[this.data.Length * 2];
            Array.Copy((Array) this.data, 0, (Array) destinationArray, 0, this.data.Length);
            this.data = destinationArray;
          }
          num = reader.Read(this.data, index, readChunkSize);
          index += num;
        }
        while (num != 0);
        this.n = index;
      }
      finally
      {
        reader.Close();
      }
    }
  }
}
