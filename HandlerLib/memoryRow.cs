// Decompiled with JetBrains decompiler
// Type: HandlerLib.memoryRow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class memoryRow : INotifyPropertyChanged
  {
    public bool overWriteValuesOnly = false;
    private string Byte_0;
    private string Byte_1;
    private string Byte_2;
    private string Byte_3;
    private string Byte_4;
    private string Byte_5;
    private string Byte_6;
    private string Byte_7;
    private string Byte_8;
    private string Byte_9;
    private string Byte_10;
    private string Byte_11;
    private string Byte_12;
    private string Byte_13;
    private string Byte_14;
    private string Byte_15;
    public bool[] dataChanged = new bool[16];
    private byte[] byteArray;
    private byte[] oldArray;

    public string Address { get; set; }

    public uint address { get; set; }

    public bool resetArray()
    {
      if (this.oldArray.Length == 0)
        return false;
      Array.Copy((Array) this.oldArray, (Array) this.byteArray, this.oldArray.Length);
      return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public bool dataChanged_Byte_0
    {
      get => this.dataChanged[0];
      set
      {
        if (this.overWriteValuesOnly)
          return;
        this.dataChanged[0] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_0));
      }
    }

    public bool dataChanged_Byte_1
    {
      get => this.dataChanged[1];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[1] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_1));
      }
    }

    public bool dataChanged_Byte_2
    {
      get => this.dataChanged[2];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[2] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_2));
      }
    }

    public bool dataChanged_Byte_3
    {
      get => this.dataChanged[3];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[3] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_3));
      }
    }

    public bool dataChanged_Byte_4
    {
      get => this.dataChanged[4];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[4] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_4));
      }
    }

    public bool dataChanged_Byte_5
    {
      get => this.dataChanged[5];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[5] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_5));
      }
    }

    public bool dataChanged_Byte_6
    {
      get => this.dataChanged[6];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[6] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_6));
      }
    }

    public bool dataChanged_Byte_7
    {
      get => this.dataChanged[7];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[7] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_7));
      }
    }

    public bool dataChanged_Byte_8
    {
      get => this.dataChanged[8];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[8] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_8));
      }
    }

    public bool dataChanged_Byte_9
    {
      get => this.dataChanged[9];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[9] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_9));
      }
    }

    public bool dataChanged_Byte_10
    {
      get => this.dataChanged[10];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[10] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_10));
      }
    }

    public bool dataChanged_Byte_11
    {
      get => this.dataChanged[11];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[11] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_11));
      }
    }

    public bool dataChanged_Byte_12
    {
      get => this.dataChanged[12];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[12] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_12));
      }
    }

    public bool dataChanged_Byte_13
    {
      get => this.dataChanged[13];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[13] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_13));
      }
    }

    public bool dataChanged_Byte_14
    {
      get => this.dataChanged[14];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[14] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_14));
      }
    }

    public bool dataChanged_Byte_15
    {
      get => this.dataChanged[15];
      set
      {
        if (!this.overWriteValuesOnly)
          this.dataChanged[15] = value;
        this.NotifyPropertyChanged(nameof (dataChanged_Byte_15));
      }
    }

    public string byte_0
    {
      get => this.Byte_0;
      set
      {
        this.dataChanged_Byte_0 = !(this.Byte_0 == value);
        this.Byte_0 = value;
      }
    }

    public string byte_1
    {
      get => this.Byte_1;
      set
      {
        this.dataChanged_Byte_1 = !(this.Byte_1 == value);
        this.Byte_1 = value;
      }
    }

    public string byte_2
    {
      get => this.Byte_2;
      set
      {
        this.dataChanged_Byte_2 = !(this.Byte_2 == value);
        this.Byte_2 = value;
      }
    }

    public string byte_3
    {
      get => this.Byte_3;
      set
      {
        this.dataChanged_Byte_3 = !(this.Byte_3 == value);
        this.Byte_3 = value;
      }
    }

    public string byte_4
    {
      get => this.Byte_4;
      set
      {
        this.dataChanged_Byte_4 = !(this.Byte_4 == value);
        this.Byte_4 = value;
      }
    }

    public string byte_5
    {
      get => this.Byte_5;
      set
      {
        this.dataChanged_Byte_5 = !(this.Byte_5 == value);
        this.Byte_5 = value;
      }
    }

    public string byte_6
    {
      get => this.Byte_6;
      set
      {
        this.dataChanged_Byte_6 = !(this.Byte_6 == value);
        this.Byte_6 = value;
      }
    }

    public string byte_7
    {
      get => this.Byte_7;
      set
      {
        this.dataChanged_Byte_7 = !(this.Byte_7 == value);
        this.Byte_7 = value;
      }
    }

    public string byte_8
    {
      get => this.Byte_8;
      set
      {
        this.dataChanged_Byte_8 = !(this.Byte_8 == value);
        this.Byte_8 = value;
      }
    }

    public string byte_9
    {
      get => this.Byte_9;
      set
      {
        this.dataChanged_Byte_9 = !(this.Byte_9 == value);
        this.Byte_9 = value;
      }
    }

    public string byte_10
    {
      get => this.Byte_10;
      set
      {
        this.dataChanged_Byte_10 = !(this.Byte_10 == value);
        this.Byte_10 = value;
      }
    }

    public string byte_11
    {
      get => this.Byte_11;
      set
      {
        this.dataChanged_Byte_11 = !(this.Byte_11 == value);
        this.Byte_11 = value;
      }
    }

    public string byte_12
    {
      get => this.Byte_12;
      set
      {
        this.dataChanged_Byte_12 = !(this.Byte_12 == value);
        this.Byte_12 = value;
      }
    }

    public string byte_13
    {
      get => this.Byte_13;
      set
      {
        this.dataChanged_Byte_13 = !(this.Byte_13 == value);
        this.Byte_13 = value;
      }
    }

    public string byte_14
    {
      get => this.Byte_14;
      set
      {
        this.dataChanged_Byte_14 = !(this.Byte_14 == value);
        this.Byte_14 = value;
      }
    }

    public string byte_15
    {
      get => this.Byte_15;
      set
      {
        this.dataChanged_Byte_15 = !(this.Byte_15 == value);
        this.Byte_15 = value;
      }
    }

    public string data { get; set; }

    public bool IsChanged { get; set; }

    public uint row { get; set; }

    public void ClearDataChanged()
    {
      this.dataChanged_Byte_0 = false;
      this.dataChanged_Byte_1 = false;
      this.dataChanged_Byte_2 = false;
      this.dataChanged_Byte_3 = false;
      this.dataChanged_Byte_4 = false;
      this.dataChanged_Byte_5 = false;
      this.dataChanged_Byte_6 = false;
      this.dataChanged_Byte_7 = false;
      this.dataChanged_Byte_8 = false;
      this.dataChanged_Byte_9 = false;
      this.dataChanged_Byte_10 = false;
      this.dataChanged_Byte_11 = false;
      this.dataChanged_Byte_12 = false;
      this.dataChanged_Byte_13 = false;
      this.dataChanged_Byte_14 = false;
      this.dataChanged_Byte_15 = false;
    }

    public byte[] ByteArray
    {
      get
      {
        return new byte[16]
        {
          Convert.ToByte(this.byte_0, 16),
          Convert.ToByte(this.byte_1, 16),
          Convert.ToByte(this.byte_2, 16),
          Convert.ToByte(this.byte_3, 16),
          Convert.ToByte(this.byte_4, 16),
          Convert.ToByte(this.byte_5, 16),
          Convert.ToByte(this.byte_6, 16),
          Convert.ToByte(this.byte_7, 16),
          Convert.ToByte(this.byte_8, 16),
          Convert.ToByte(this.byte_9, 16),
          Convert.ToByte(this.byte_10, 16),
          Convert.ToByte(this.byte_11, 16),
          Convert.ToByte(this.byte_12, 16),
          Convert.ToByte(this.byte_13, 16),
          Convert.ToByte(this.byte_14, 16),
          Convert.ToByte(this.byte_15, 16)
        };
      }
      set
      {
        this.oldArray = this.byteArray;
        Array.Copy((Array) this.byteArray, (Array) this.oldArray, this.byteArray.Length);
        this.byteArray = value;
        if (value.Length != 16)
          return;
        string hexString = Util.ByteArrayToHexString(this.byteArray, 0);
        this.byte_0 = hexString.Substring(0, 2);
        this.byte_1 = hexString.Substring(2, 2);
        this.byte_2 = hexString.Substring(4, 2);
        this.byte_3 = hexString.Substring(6, 2);
        this.byte_4 = hexString.Substring(8, 2);
        this.byte_5 = hexString.Substring(10, 2);
        this.byte_6 = hexString.Substring(12, 2);
        this.byte_7 = hexString.Substring(14, 2);
        this.byte_8 = hexString.Substring(16, 2);
        this.byte_9 = hexString.Substring(18, 2);
        this.byte_10 = hexString.Substring(20, 2);
        this.byte_11 = hexString.Substring(22, 2);
        this.byte_12 = hexString.Substring(24, 2);
        this.byte_13 = hexString.Substring(26, 2);
        this.byte_14 = hexString.Substring(28, 2);
        this.byte_15 = hexString.Substring(30, 2);
      }
    }
  }
}
