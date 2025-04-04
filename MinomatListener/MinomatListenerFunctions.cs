// Decompiled with JetBrains decompiler
// Type: MinomatListener.MinomatListenerFunctions
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

using MinomatListener.View;

#nullable disable
namespace MinomatListener
{
  public sealed class MinomatListenerFunctions
  {
    public string ShowMainWindow()
    {
      new MainWindow().ShowDialog();
      return string.Empty;
    }
  }
}
