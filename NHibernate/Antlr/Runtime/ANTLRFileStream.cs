// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ANTLRFileStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.IO;
using System.Text;

#nullable disable
namespace Antlr.Runtime
{
  internal class ANTLRFileStream : ANTLRStringStream
  {
    protected string fileName;

    protected ANTLRFileStream()
    {
    }

    public ANTLRFileStream(string fileName)
      : this(fileName, Encoding.Default)
    {
    }

    public ANTLRFileStream(string fileName, Encoding encoding)
    {
      this.fileName = fileName;
      this.Load(fileName, encoding);
    }

    public override string SourceName => this.fileName;

    public virtual void Load(string fileName, Encoding encoding)
    {
      if (fileName == null)
        return;
      StreamReader streamReader = (StreamReader) null;
      try
      {
        this.data = new char[(int) this.GetFileLength(new FileInfo(fileName))];
        streamReader = encoding == null ? new StreamReader(fileName, Encoding.Default) : new StreamReader(fileName, encoding);
        this.n = streamReader.Read(this.data, 0, this.data.Length);
      }
      finally
      {
        streamReader?.Close();
      }
    }

    private long GetFileLength(FileInfo file) => file.Exists ? file.Length : 0L;
  }
}
