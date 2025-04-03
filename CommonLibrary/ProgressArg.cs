// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.ProgressArg
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

#nullable disable
namespace ZENNER.CommonLibrary
{
  public sealed class ProgressArg
  {
    public double ProgressPercentage { get; private set; }

    public string Message { get; private set; }

    public object Tag { get; private set; }

    public ProgressArg(double progressPercentage, string message)
    {
      this.ProgressPercentage = progressPercentage;
      this.Message = message;
    }

    public ProgressArg(double progressPercentage, string message, object tag)
    {
      this.ProgressPercentage = progressPercentage;
      this.Message = message;
      this.Tag = tag;
    }
  }
}
