// Decompiled with JetBrains decompiler
// Type: WpfKb.LogicalKeys.StringKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using WindowsInput;

#nullable disable
namespace WpfKb.LogicalKeys
{
  public class StringKey : LogicalKeyBase
  {
    private string _stringToSimulate;

    public virtual string StringToSimulate
    {
      get => this._stringToSimulate;
      set
      {
        if (!(value != this._stringToSimulate))
          return;
        this._stringToSimulate = value;
        this.OnPropertyChanged(nameof (StringToSimulate));
      }
    }

    public StringKey(string displayName, string stringToSimulate)
    {
      this.DisplayName = displayName;
      this._stringToSimulate = stringToSimulate;
    }

    public StringKey()
    {
    }

    public override void Press()
    {
      InputSimulator.SimulateTextEntry(this._stringToSimulate);
      base.Press();
    }
  }
}
