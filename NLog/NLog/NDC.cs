// Decompiled with JetBrains decompiler
// Type: NLog.NDC
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog
{
  [Obsolete("Use NestedDiagnosticsContext class instead. Marked obsolete on NLog 2.0")]
  public static class NDC
  {
    public static string TopMessage => NestedDiagnosticsContext.TopMessage;

    public static object TopObject => NestedDiagnosticsContext.TopObject;

    public static IDisposable Push(string text) => NestedDiagnosticsContext.Push(text);

    public static string Pop() => NestedDiagnosticsContext.Pop();

    public static object PopObject() => NestedDiagnosticsContext.PopObject();

    public static void Clear() => NestedDiagnosticsContext.Clear();

    public static string[] GetAllMessages() => NestedDiagnosticsContext.GetAllMessages();

    public static object[] GetAllObjects() => NestedDiagnosticsContext.GetAllObjects();
  }
}
