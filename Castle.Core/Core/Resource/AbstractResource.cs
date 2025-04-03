// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.AbstractResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Castle.Core.Resource
{
  public abstract class AbstractResource : IResource, IDisposable
  {
    protected static readonly string DefaultBasePath = AppDomain.CurrentDomain.BaseDirectory;

    public virtual string FileBasePath => AbstractResource.DefaultBasePath;

    public abstract TextReader GetStreamReader();

    public abstract TextReader GetStreamReader(Encoding encoding);

    public abstract IResource CreateRelative(string relativePath);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
  }
}
