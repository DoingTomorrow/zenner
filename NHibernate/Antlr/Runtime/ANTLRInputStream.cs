// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ANTLRInputStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.IO;
using System.Text;

#nullable disable
namespace Antlr.Runtime
{
  internal class ANTLRInputStream : ANTLRReaderStream
  {
    protected ANTLRInputStream()
    {
    }

    public ANTLRInputStream(Stream istream)
      : this(istream, (Encoding) null)
    {
    }

    public ANTLRInputStream(Stream istream, Encoding encoding)
      : this(istream, ANTLRReaderStream.INITIAL_BUFFER_SIZE, encoding)
    {
    }

    public ANTLRInputStream(Stream istream, int size)
      : this(istream, size, (Encoding) null)
    {
    }

    public ANTLRInputStream(Stream istream, int size, Encoding encoding)
      : this(istream, size, ANTLRReaderStream.READ_BUFFER_SIZE, encoding)
    {
    }

    public ANTLRInputStream(Stream istream, int size, int readBufferSize, Encoding encoding)
    {
      this.Load(encoding == null ? (TextReader) new StreamReader(istream) : (TextReader) new StreamReader(istream, encoding), size, readBufferSize);
    }
  }
}
