// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.ILogicalKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System.ComponentModel;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public interface ILogicalKey : INotifyPropertyChanged
  {
    string DisplayName { get; }

    void Press();

    event LogicalKeyPressedEventHandler LogicalKeyPressed;
  }
}
