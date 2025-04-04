// Decompiled with JetBrains decompiler
// Type: Microsoft.Internal.Web.Utils.PhysicalFileSystem
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Microsoft.Internal.Web.Utils
{
  internal sealed class PhysicalFileSystem : IFileSystem
  {
    public bool FileExists(string path) => File.Exists(path);

    public Stream ReadFile(string path) => (Stream) File.OpenRead(path);

    public Stream OpenFile(string path)
    {
      PhysicalFileSystem.EnsureDirectory(Path.GetDirectoryName(path));
      return (Stream) File.OpenWrite(path);
    }

    public IEnumerable<string> EnumerateFiles(string path) => Directory.EnumerateFiles(path);

    private static void EnsureDirectory(string path)
    {
      if (Directory.Exists(path))
        return;
      Directory.CreateDirectory(path);
    }
  }
}
