// Decompiled with JetBrains decompiler
// Type: Castle.Core.Resource.AssemblyBundleResource
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

#nullable disable
namespace Castle.Core.Resource
{
  public class AssemblyBundleResource : AbstractResource
  {
    private readonly CustomUri resource;

    public AssemblyBundleResource(CustomUri resource) => this.resource = resource;

    public override TextReader GetStreamReader()
    {
      Assembly assembly = AssemblyBundleResource.ObtainAssembly(this.resource.Host);
      string[] strArray = this.resource.Path.Split(new char[1]
      {
        '/'
      }, StringSplitOptions.RemoveEmptyEntries);
      return strArray.Length == 2 ? (TextReader) new StringReader(new ResourceManager(strArray[0], assembly).GetString(strArray[1])) : throw new ResourceException("AssemblyBundleResource does not support paths with more than 2 levels in depth. See " + this.resource.Path);
    }

    public override TextReader GetStreamReader(Encoding encoding) => this.GetStreamReader();

    public override IResource CreateRelative(string relativePath)
    {
      throw new NotImplementedException();
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
