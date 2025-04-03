// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.UncResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Globalization;
using System.IO;

#nullable disable
namespace Castle.Core.Resource
{
  public class UncResource : AbstractStreamResource
  {
    private string basePath;
    private string filePath;

    public UncResource(CustomUri resource)
    {
      UncResource uncResource = this;
      this.CreateStream = (StreamFactory) (() => uncResource.CreateStreamFromUri(resource, AbstractResource.DefaultBasePath));
    }

    public UncResource(CustomUri resource, string basePath)
    {
      UncResource uncResource = this;
      this.CreateStream = (StreamFactory) (() => uncResource.CreateStreamFromUri(resource, basePath));
    }

    public UncResource(string resourceName)
      : this(new CustomUri(resourceName))
    {
    }

    public UncResource(string resourceName, string basePath)
      : this(new CustomUri(resourceName), basePath)
    {
    }

    public override string FileBasePath => this.basePath;

    public override IResource CreateRelative(string relativePath)
    {
      return (IResource) new UncResource(Path.Combine(this.basePath, relativePath));
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "UncResource: [{0}] [{1}]", (object) this.filePath, (object) this.basePath);
    }

    private Stream CreateStreamFromUri(CustomUri resource, string rootPath)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      if (!resource.IsUnc)
        throw new ArgumentException("Resource must be an Unc", nameof (resource));
      string str = resource.IsFile ? resource.Path : throw new ArgumentException("The specified resource is not a file", nameof (resource));
      if (!File.Exists(str) && rootPath != null)
        str = Path.Combine(rootPath, str);
      this.filePath = Path.GetFileName(str);
      this.basePath = Path.GetDirectoryName(str);
      UncResource.CheckFileExists(str);
      return (Stream) File.OpenRead(str);
    }

    private static void CheckFileExists(string path)
    {
      if (!File.Exists(path))
        throw new ResourceException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "File {0} could not be found", (object) path));
    }
  }
}
