// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.LogicalKeyEventArgs
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public class LogicalKeyEventArgs : EventArgs
  {
    public ILogicalKey Key { get; private set; }

    public LogicalKeyEventArgs(ILogicalKey key) => this.Key = key;
  }
}
