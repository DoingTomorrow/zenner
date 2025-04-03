// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.UncResourceFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

#nullable disable
namespace Castle.Core.Resource
{
  public class UncResourceFactory : IResourceFactory
  {
    public bool Accept(CustomUri uri) => uri.IsUnc;

    public IResource Create(CustomUri uri) => (IResource) new UncResource(uri);

    public IResource Create(CustomUri uri, string basePath)
    {
      return (IResource) new UncResource(uri, basePath);
    }
  }
}
