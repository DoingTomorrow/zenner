// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ChainsawTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Targets
{
  [Target("Chainsaw")]
  public class ChainsawTarget : NLogViewerTarget
  {
    public ChainsawTarget()
    {
      this.IncludeNLogData = false;
      this.OptimizeBufferReuse = this.GetType() == typeof (ChainsawTarget);
    }

    public ChainsawTarget(string name)
      : this()
    {
      this.Name = name;
    }
  }
}
