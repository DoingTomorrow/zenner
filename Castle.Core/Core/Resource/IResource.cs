// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.IResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Castle.Core.Resource
{
  public interface IResource : IDisposable
  {
    string FileBasePath { get; }

    TextReader GetStreamReader();

    TextReader GetStreamReader(Encoding encoding);

    IResource CreateRelative(string relativePath);
  }
}
