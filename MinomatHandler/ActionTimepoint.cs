// Decompiled with JetBrains decompiler
// Type: MinomatHandler.ActionTimepoint
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;

#nullable disable
namespace MinomatHandler
{
  public sealed class ActionTimepoint
  {
    public ActionMode Action { get; set; }

    public ActionTimepointType TimepointType { get; set; }

    public DateTime Timepoint { get; set; }

    public override string ToString()
    {
      return string.Format("Action: {0}, Type: {1}, Timepoint: {2}", (object) this.Action, (object) this.TimepointType, (object) this.Timepoint);
    }
  }
}
