// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.StaticContentResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Castle.Core.Resource
{
  public class StaticContentResource : AbstractResource
  {
    private readonly string contents;

    public StaticContentResource(string contents) => this.contents = contents;

    public override TextReader GetStreamReader() => (TextReader) new StringReader(this.contents);

    public override TextReader GetStreamReader(Encoding encoding)
    {
      throw new NotImplementedException();
    }

    public override IResource CreateRelative(string relativePath)
    {
      throw new NotImplementedException();
    }
  }
}
