// Decompiled with JetBrains decompiler
// Type: HandlerLib.myDataRow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class myDataRow : INotifyPropertyChanged
  {
    private object _origValue;
    private string _oValHex;
    private object _oValue;
    private bool IsCrossChange = false;
    public int savedColumns = 0;
    public ParameterWindow ParamWindow = (ParameterWindow) null;

    public void ResetOldValue() => this.OldOrigValue = (object) null;

    private object OldOrigValue { get; set; }

    public object OrigValue
    {
      get => this._origValue;
      set
      {
        if (value != null && this.OldOrigValue != null)
        {
          if (value.GetType() == typeof (byte[]))
          {
            bool flag = true;
            for (int index = 0; index < ((byte[]) value).Length; ++index)
            {
              byte num1 = ((byte[]) this.OldOrigValue)[index];
              byte num2 = ((byte[]) value)[index];
              flag &= (int) num2 == (int) num1;
            }
            this.IsChanged = flag && this.IsChanged || !flag && !this.IsChanged || !flag;
          }
          else if (value != this.OldOrigValue)
            this.IsChanged = true;
        }
        else if (value != null && this.OldOrigValue == null)
        {
          if (!this.IsReading)
            this.IsChanged = true;
        }
        else if (value == null && this.OldOrigValue != null)
          this.IsChanged = true;
        if (!this.IsChanged && !this.IsReading)
          return;
        this.OldOrigValue = this._origValue;
        this._origValue = value;
      }
    }

    public string Name { get; set; }

    public string Section { get; set; }

    public string MemoryArea { get; set; }

    private string _oType { get; set; }

    public string Typ
    {
      get => this._oType;
      set
      {
        this._oType = value;
        this.NotifyPropertyChanged(nameof (Typ));
      }
    }

    public string ValueHEXfull => this._oValHex;

    public string ValueHEX
    {
      get
      {
        uint num = uint.Parse(this.Bytes.Substring(2), NumberStyles.HexNumber);
        int length = (int) num * 2 > 40 ? 40 : (int) num * 2;
        string valueHex = this._oValHex;
        if (this._oValHex != null && this._oValHex.Length >= length)
          valueHex = this._oValHex.Substring(0, length) + (num > 40U ? "..." : "");
        return valueHex;
      }
      set
      {
        try
        {
          if (value == null)
            value = string.Empty;
          if (this._oValHex == null)
            this._oValHex = value;
          if (this._oValHex.Trim().ToLower().Equals(value.Trim().ToLower()))
            return;
          this._oValHex = value;
          if (!this.IsCrossChange)
          {
            this.IsChanged = true;
            this.NotifyPropertyChanged(nameof (ValueHEX));
            if (this.Value != null && !this.Value.ToString().Contains("[...]"))
            {
              this.IsCrossChange = true;
              this.Value = ParameterArrayAssistant.HEXtoObject(this.ValType, this.ValueHEX);
              this.IsCrossChange = false;
            }
            if (!this.IsReading)
              this.setValueToMemory();
          }
          else
            this.NotifyPropertyChanged(nameof (ValueHEX));
        }
        catch (Exception ex)
        {
          this.IsCrossChange = false;
        }
      }
    }

    public object Value
    {
      get => this._oValue;
      set
      {
        try
        {
          if (value == null || string.IsNullOrEmpty(value.ToString()) || value == this._oValue)
            return;
          if (this.checkValueOK(value))
          {
            this._oValue = value;
            if (!this.IsCrossChange)
            {
              if (!this._oValue.ToString().Contains("[...]"))
              {
                this.IsChanged = true;
                this.NotifyPropertyChanged(nameof (Value));
                this.IsCrossChange = true;
                this.ValueHEX = this.getHexStringForObject(this.Value.ToString(), this.ValType);
                this.OrigValue = this._oValue;
                this.IsCrossChange = false;
                if (!this.IsReading)
                  this.setValueToMemory();
              }
            }
            else
            {
              this.NotifyPropertyChanged(nameof (Value));
              if (!this._oValue.ToString().Contains("[...]"))
                this.OrigValue = this._oValue;
            }
          }
          else if (this.IsValueForced)
          {
            this._oValue = value;
            this.NotifyPropertyChanged(nameof (Value));
          }
        }
        catch (Exception ex)
        {
          this.IsCrossChange = false;
        }
      }
    }

    public string Bytes { get; set; }

    public string Address { get; set; }

    public Type ValType { get; set; }

    private bool _isChanged { get; set; }

    public bool IsChanged
    {
      get => this._isChanged;
      set
      {
        this._isChanged = value;
        this.NotifyPropertyChanged(nameof (IsChanged));
      }
    }

    public bool IsChangedInternal { get; set; }

    public bool IsTypeChanged { get; set; }

    public bool IsSavedTemp { get; set; }

    private bool _isInit { get; set; }

    public bool IsInit
    {
      get => this._isInit;
      set
      {
        this._isInit = value;
        this.NotifyPropertyChanged(nameof (IsInit));
      }
    }

    private bool _isMemoryAvail { get; set; }

    public bool IsMemoryAvail
    {
      get => this._isMemoryAvail;
      set
      {
        this._isMemoryAvail = value;
        this.NotifyPropertyChanged(nameof (IsMemoryAvail));
      }
    }

    public bool IsReading { get; set; }

    public bool IsValueForced { get; set; }

    public myDataRow()
    {
    }

    public myDataRow(
      bool isInit,
      ParameterWindow oPW,
      string name,
      string section,
      string memoryArea,
      string typ,
      string bytes,
      string address,
      string valuehex,
      object value,
      Type valTyp)
    {
      this.IsInit = isInit;
      this.IsReading = this.IsInit;
      this.ParamWindow = oPW;
      this.Name = name;
      this.Section = section;
      this.MemoryArea = memoryArea;
      this.Typ = typ;
      this.ValType = valTyp;
      this.Bytes = bytes;
      this.Address = address;
      this.ValueHEX = valuehex;
      if (value != null && value.ToString().Equals("[...]"))
      {
        this.IsValueForced = true;
        this.Value = (object) "[...]";
        this.IsValueForced = false;
      }
      else
        this.Value = value;
      this.IsChanged = false;
      this.IsTypeChanged = false;
      this.IsSavedTemp = false;
      this.IsMemoryAvail = false;
      this.IsValueForced = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    private bool checkValueOK(object value)
    {
      if (value.ToString().Equals("[...]"))
        return false;
      try
      {
        return this.getHexStringForObject(value.ToString(), this.ValType) != null;
      }
      catch
      {
        return false;
      }
    }

    private void setValueToMemory()
    {
      if (this.Value == null || !this.IsChanged)
        return;
      uint address = uint.Parse(this.Address.Substring(2), NumberStyles.HexNumber);
      uint byteSize = uint.Parse(this.Bytes.Substring(2), NumberStyles.HexNumber);
      if (!this.Value.ToString().Contains("[...]"))
      {
        if (this.Value.ToString().Length > 1 && this.Value.ToString().Contains("-") || !this.Value.ToString().Contains("-"))
          Parameter32bit.SetValue(this.ValType, this.Value, address, this.ParamWindow.saveDeviceMemory);
      }
      else if (!string.IsNullOrEmpty(this.ValueHEX))
        Parameter32bit.SetValue(Util.HexStringToByteArray(this.ValueHEX), address, this.ParamWindow.saveDeviceMemory);
      string message = string.Empty;
      DeviceMemoryStorage memoryTypeForData = this.ParamWindow.saveDeviceMemory.GetDeviceMemoryTypeForData(address, byteSize, out message);
      DeviceMemoryType deviceMemoryType1 = memoryTypeForData == null ? DeviceMemoryType.NotAvail : memoryTypeForData.MemoryType;
      DeviceMemoryType deviceMemoryType2 = memoryTypeForData == null ? DeviceMemoryType.NotAvail : memoryTypeForData.MemoryType;
      this.MemoryArea = deviceMemoryType2 == DeviceMemoryType.Unknown ? "" : deviceMemoryType2.ToString();
      this.IsMemoryAvail = string.IsNullOrEmpty(message);
    }

    private string getValueStringFromHex(Type myType, string value)
    {
      try
      {
        string empty = string.Empty;
        if (myType == typeof (double) || myType == typeof (float))
          return double.Parse(value, NumberStyles.HexNumber).ToString();
        if (myType == typeof (byte))
          return byte.Parse(value, NumberStyles.HexNumber).ToString();
        if (myType == typeof (sbyte))
          return sbyte.Parse(value, NumberStyles.HexNumber).ToString();
        if (myType == typeof (short))
          return short.Parse(value, NumberStyles.HexNumber).ToString();
        if (myType == typeof (ushort))
          return ushort.Parse(value, NumberStyles.HexNumber).ToString();
        if (myType == typeof (int))
          return int.Parse(value, NumberStyles.HexNumber).ToString();
        if (myType == typeof (uint))
          return uint.Parse(value, NumberStyles.HexNumber).ToString();
        return myType == typeof (Decimal) ? Decimal.Parse(value, NumberStyles.HexNumber).ToString() : empty;
      }
      catch (Exception ex)
      {
        throw new Exception("Error while converting from HEX-String!", ex);
      }
    }

    public string getHexStringForObject(string value, Type myType)
    {
      try
      {
        if (myType == typeof (byte))
          return byte.Parse(value).ToString("X2");
        if (myType == typeof (sbyte))
          return sbyte.Parse(value).ToString("X2");
        if (myType == typeof (short))
          return short.Parse(value).ToString("X4");
        if (myType == typeof (ushort))
          return ushort.Parse(value).ToString("X4");
        if (myType == typeof (int))
          return int.Parse(value).ToString("X8");
        if (myType == typeof (uint))
          return uint.Parse(value).ToString("X8");
        if (myType == typeof (float))
          return ParameterArrayAssistant.float2HEXString(float.Parse(value));
        if (myType == typeof (double))
          return ParameterArrayAssistant.double2HEXString(double.Parse(value));
        return myType == typeof (Decimal) ? Decimal.Parse(value).ToString("x8") : string.Empty;
      }
      catch (Exception ex)
      {
        throw new Exception("Conversion Type is not implemented!!!", ex);
      }
    }
  }
}
