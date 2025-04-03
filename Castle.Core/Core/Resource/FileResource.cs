// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.FileResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Globalization;
using System.IO;

#nullable disable
namespace Castle.Core.Resource
{
  public class FileResource : AbstractStreamResource
  {
    private string filePath;
    private string basePath;

    public FileResource(CustomUri resource)
    {
      FileResource fileResource = this;
      this.CreateStream = (StreamFactory) (() => fileResource.CreateStreamFromUri(resource, AbstractResource.DefaultBasePath));
    }

    public FileResource(CustomUri resource, string basePath)
    {
      FileResource fileResource = this;
      this.CreateStream = (StreamFactory) (() => fileResource.CreateStreamFromUri(resource, basePath));
    }

    public FileResource(string resourceName)
    {
      FileResource fileResource = this;
      this.CreateStream = (StreamFactory) (() => fileResource.CreateStreamFromPath(resourceName, AbstractResource.DefaultBasePath));
    }

    public FileResource(string resourceName, string basePath)
    {
      FileResource fileResource = this;
      this.CreateStream = (StreamFactory) (() => fileResource.CreateStreamFromPath(resourceName, basePath));
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "FileResource: [{0}] [{1}]", (object) this.filePath, (object) this.basePath);
    }

    public override string FileBasePath => this.basePath;

    public override IResource CreateRelative(string relativePath)
    {
      return (IResource) new FileResource(relativePath, this.basePath);
    }

    private Stream CreateStreamFromUri(CustomUri resource, string rootPath)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      if (rootPath == null)
        throw new ArgumentNullException(nameof (rootPath));
      if (!resource.IsFile)
        throw new ArgumentException("The specified resource is not a file", nameof (resource));
      return this.CreateStreamFromPath(resource.Path, rootPath);
    }

    private Stream CreateStreamFromPath(string resourcePath, string rootPath)
    {
      if (resourcePath == null)
        throw new ArgumentNullException(nameof (resourcePath));
      if (rootPath == null)
        throw new ArgumentNullException(nameof (rootPath));
      if (!Path.IsPathRooted(resourcePath) || !File.Exists(resourcePath))
        resourcePath = Path.Combine(rootPath, resourcePath);
      FileResource.CheckFileExists(resourcePath);
      this.filePath = Path.GetFileName(resourcePath);
      this.basePath = Path.GetDirectoryName(resourcePath);
      return (Stream) File.OpenRead(resourcePath);
    }

    private static void CheckFileExists(string path)
    {
      if (!File.Exists(path))
        throw new ResourceException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "File {0} could not be found", (object) new FileInfo(path).FullName));
    }
  }
}
