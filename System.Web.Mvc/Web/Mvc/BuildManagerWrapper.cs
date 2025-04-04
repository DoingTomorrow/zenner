// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.BuildManagerWrapper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.IO;
using System.Web.Compilation;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class BuildManagerWrapper : IBuildManager
  {
    bool IBuildManager.FileExists(string virtualPath)
    {
      return BuildManager.GetObjectFactory(virtualPath, false) != null;
    }

    Type IBuildManager.GetCompiledType(string virtualPath)
    {
      return BuildManager.GetCompiledType(virtualPath);
    }

    ICollection IBuildManager.GetReferencedAssemblies() => BuildManager.GetReferencedAssemblies();

    Stream IBuildManager.ReadCachedFile(string fileName) => BuildManager.ReadCachedFile(fileName);

    Stream IBuildManager.CreateCachedFile(string fileName)
    {
      return BuildManager.CreateCachedFile(fileName);
    }
  }
}
