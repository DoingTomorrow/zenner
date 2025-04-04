// Decompiled with JetBrains decompiler
// Type: NLog.Internal.EnvironmentHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Security;

#nullable disable
namespace NLog.Internal
{
  internal static class EnvironmentHelper
  {
    internal static string NewLine => Environment.NewLine;

    internal static string GetMachineName()
    {
      try
      {
        return Environment.MachineName;
      }
      catch (SecurityException ex)
      {
        return string.Empty;
      }
    }

    internal static string GetSafeEnvironmentVariable(string name)
    {
      try
      {
        string environmentVariable = Environment.GetEnvironmentVariable(name);
        return environmentVariable == null || environmentVariable.Length == 0 ? (string) null : environmentVariable;
      }
      catch (SecurityException ex)
      {
        return (string) null;
      }
    }
  }
}
