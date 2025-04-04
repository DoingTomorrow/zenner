// Decompiled with JetBrains decompiler
// Type: NLog.Internal.AssemblyHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System.IO;
using System.Reflection;

#nullable disable
namespace NLog.Internal
{
  internal static class AssemblyHelpers
  {
    public static Assembly LoadFromPath(string assemblyFileName, string baseDirectory = null)
    {
      string assemblyFile = baseDirectory == null ? assemblyFileName : Path.Combine(baseDirectory, assemblyFileName);
      InternalLogger.Info<string>("Loading assembly file: {0}", assemblyFile);
      return Assembly.LoadFrom(assemblyFile);
    }

    public static Assembly LoadFromName(string assemblyName)
    {
      InternalLogger.Info<string>("Loading assembly: {0}", assemblyName);
      return Assembly.Load(assemblyName);
    }
  }
}
