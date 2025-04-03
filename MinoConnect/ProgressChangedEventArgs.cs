// Decompiled with JetBrains decompiler
// Type: MinoConnect.ProgressChangedEventArgs
// Assembly: MinoConnect, Version=1.5.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E4D0ECC-943B-4E96-B8E2-CE02CEE9906B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinoConnect.exe

using System;

#nullable disable
namespace MinoConnect
{
  public class ProgressChangedEventArgs : EventArgs
  {
    public int ProgressPercentage { get; private set; }

    public ProgressChangedEventArgs() => this.ProgressPercentage = 0;

    public ProgressChangedEventArgs(int progressPercentage)
    {
      this.ProgressPercentage = progressPercentage;
    }
  }
}
