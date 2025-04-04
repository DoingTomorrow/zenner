// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PathHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System.IO;

#nullable disable
namespace NLog.Internal
{
  internal static class PathHelpers
  {
    internal static string CombinePaths(string path, string dir, string file)
    {
      if (dir != null)
        path = Path.Combine(path, dir);
      if (file != null)
        path = Path.Combine(path, file);
      return path;
    }
  }
}
