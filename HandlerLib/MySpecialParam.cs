// Decompiled with JetBrains decompiler
// Type: HandlerLib.MySpecialParam
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.ComponentModel;

#nullable disable
namespace HandlerLib
{
  public class MySpecialParam : INotifyPropertyChanged
  {
    private byte[] _internalData;

    public event PropertyChangedEventHandler PropertyChanged;

    public byte[] InternalData
    {
      get => this._internalData;
      set
      {
        if (this._internalData == value)
          return;
        this._internalData = value;
        if (this.PropertyChanged == null)
          return;
        this.PropertyChanged((object) this, new PropertyChangedEventArgs(nameof (InternalData)));
      }
    }

    internal uint TypeSize { get; set; }

    internal string Name { get; set; }

    internal uint Position { get; set; }
  }
}
