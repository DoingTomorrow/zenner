// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.AssemblyResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Globalization;
using System.IO;
using System.Reflection;

#nullable disable
namespace Castle.Core.Resource
{
  public class AssemblyResource : AbstractStreamResource
  {
    private string assemblyName;
    private string resourcePath;
    private string basePath;

    public AssemblyResource(CustomUri resource)
    {
      AssemblyResource assemblyResource = this;
      this.CreateStream = (StreamFactory) (() => assemblyResource.CreateResourceFromUri(resource, (string) null));
    }

    public AssemblyResource(CustomUri resource, string basePath)
    {
      AssemblyResource assemblyResource = this;
      this.CreateStream = (StreamFactory) (() => assemblyResource.CreateResourceFromUri(resource, basePath));
    }

    public AssemblyResource(string resource)
    {
      AssemblyResource assemblyResource = this;
      this.CreateStream = (StreamFactory) (() => assemblyResource.CreateResourceFromPath(resource, assemblyResource.basePath));
    }

    public override IResource CreateRelative(string relativePath)
    {
      throw new NotImplementedException();
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "AssemblyResource: [{0}] [{1}]", (object) this.assemblyName, (object) this.resourcePath);
    }

    private Stream CreateResourceFromPath(string resource, string path)
    {
      if (!resource.StartsWith("assembly" + CustomUri.SchemeDelimiter, StringComparison.CurrentCulture))
        resource = "assembly" + CustomUri.SchemeDelimiter + resource;
      return this.CreateResourceFromUri(new CustomUri(resource), path);
    }

    private Stream CreateResourceFromUri(CustomUri resourcex, string path)
    {
      this.assemblyName = resourcex != null ? resourcex.Host : throw new ArgumentNullException(nameof (resourcex));
      this.resourcePath = this.ConvertToResourceName(this.assemblyName, resourcex.Path);
      Assembly assembly = AssemblyResource.ObtainAssembly(this.assemblyName);
      string[] manifestResourceNames = assembly.GetManifestResourceNames();
      string nameFound = this.GetNameFound(manifestResourceNames);
      if (nameFound == null)
      {
        this.resourcePath = resourcex.Path.Replace('/', '.').Substring(1);
        nameFound = this.GetNameFound(manifestResourceNames);
      }
      if (nameFound == null)
        throw new ResourceException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The assembly resource {0} could not be located", (object) this.resourcePath));
      this.basePath = this.ConvertToPath(this.resourcePath);
      return assembly.GetManifestResourceStream(nameFound);
    }

    private string GetNameFound(string[] names)
    {
      string nameFound = (string) null;
      foreach (string name in names)
      {
        if (string.Compare(this.resourcePath, name, StringComparison.OrdinalIgnoreCase) == 0)
        {
          nameFound = name;
          break;
        }
      }
      return nameFound;
    }

    private string ConvertToResourceName(string assembly, string resource)
    {
      assembly = this.GetSimpleName(assembly);
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}{1}", (object) assembly, (object) resource.Replace('/', '.'));
    }

    private string GetSimpleName(string assembly)
    {
      int length = assembly.IndexOf(',');
      return length < 0 ? assembly : assembly.Substring(0, length);
    }

    private string ConvertToPath(string resource)
    {
      string path = resource.Replace('.', '/');
      if (path[0] != '/')
        path = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "/{0}", (object) path);
      return path;
    }

    private static Assembly ObtainAssembly(string assemblyName)
    {
      try
      {
        return Assembly.Load(assemblyName);
      }
      catch (Exception ex)
      {
        throw new ResourceException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The assembly {0} could not be loaded", (object) assemblyName), ex);
      }
    }
  }
}
