// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.NLogFileRecord
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public class NLogFileRecord
  {
    public int Line { get; internal set; }

    public DateTime LogTime { get; internal set; }

    public int ThreadID { get; internal set; }

    public string LoggerName { get; internal set; }

    public string Level { get; internal set; }

    public string Message { get; internal set; }
  }
}
