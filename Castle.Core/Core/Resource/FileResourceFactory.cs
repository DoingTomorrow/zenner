// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.FileResourceFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

#nullable disable
namespace Castle.Core.Resource
{
  public class FileResourceFactory : IResourceFactory
  {
    public bool Accept(CustomUri uri) => "file".Equals(uri.Scheme);

    public IResource Create(CustomUri uri) => this.Create(uri, (string) null);

    public IResource Create(CustomUri uri, string basePath)
    {
      return basePath != null ? (IResource) new FileResource(uri, basePath) : (IResource) new FileResource(uri);
    }
  }
}
