// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ANTLRInputStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class ANTLRInputStream : ANTLRReaderStream
  {
    public ANTLRInputStream(Stream input)
      : this(input, (Encoding) null)
    {
    }

    public ANTLRInputStream(Stream input, int size)
      : this(input, size, (Encoding) null)
    {
    }

    public ANTLRInputStream(Stream input, Encoding encoding)
      : this(input, 1024, encoding)
    {
    }

    public ANTLRInputStream(Stream input, int size, Encoding encoding)
      : this(input, size, 1024, encoding)
    {
    }

    public ANTLRInputStream(Stream input, int size, int readBufferSize, Encoding encoding)
      : base((TextReader) ANTLRInputStream.GetStreamReader(input, encoding), size, readBufferSize)
    {
    }

    private static StreamReader GetStreamReader(Stream input, Encoding encoding)
    {
      return encoding != null ? new StreamReader(input, encoding) : new StreamReader(input);
    }
  }
}
