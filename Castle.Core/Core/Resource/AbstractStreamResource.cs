// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.AbstractStreamResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.IO;
using System.Text;

#nullable disable
namespace Castle.Core.Resource
{
  public abstract class AbstractStreamResource : AbstractResource
  {
    private StreamFactory createStream;

    ~AbstractStreamResource() => this.Dispose(false);

    public StreamFactory CreateStream
    {
      get => this.createStream;
      set => this.createStream = value;
    }

    public override TextReader GetStreamReader()
    {
      return (TextReader) new StreamReader(this.CreateStream());
    }

    public override TextReader GetStreamReader(Encoding encoding)
    {
      return (TextReader) new StreamReader(this.CreateStream(), encoding);
    }
  }
}
