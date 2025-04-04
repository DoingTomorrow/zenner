// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.SimulatorParameter
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System.ComponentModel;

#nullable disable
namespace SmartFunctionCompiler
{
  public class SimulatorParameter : INotifyPropertyChanged
  {
    private double startValue;
    private double valueIncrement;
    private double _value;

    public string Name { get; private set; }

    public double StartValue
    {
      get => this.startValue;
      set
      {
        this.startValue = value;
        this.OnPropertyChanged(nameof (StartValue));
      }
    }

    public double ValueIncrement
    {
      get => this.valueIncrement;
      set
      {
        this.valueIncrement = value;
        this.OnPropertyChanged(nameof (ValueIncrement));
      }
    }

    public double Value
    {
      get => this._value;
      set
      {
        this._value = value;
        this.OnPropertyChanged(nameof (Value));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public SimulatorParameter(string name, double? value = null)
    {
      this.Name = name;
      this.StartValue = !value.HasValue ? 0.0 : value.Value;
      this.ValueIncrement = 0.0;
      this.Value = this.StartValue;
    }

    public void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
