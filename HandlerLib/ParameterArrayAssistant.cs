// Decompiled with JetBrains decompiler
// Type: HandlerLib.ParameterArrayAssistant
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class ParameterArrayAssistant : Window, IComponentConnector
  {
    private myDataRow Data;
    private Dictionary<string, MySpecialParam> mySParamList;
    private uint TypeSize = 0;
    private uint CellWidth = 25;
    private uint CellHeight = 23;
    private uint CharWidth = 9;
    private uint Cells = 0;
    private uint MinWindowHeight = 300;
    private uint MinWindowWidth = 600;
    internal bool isCanceled = false;
    private static string showDataInHex = string.Empty;
    internal GmmCorporateControl gmmCorporateControl1;
    internal TextBox txtColumnCount;
    internal Button btnCancel;
    internal Button btnSave;
    internal Button btnCopyToClipboard;
    internal Canvas myTableCanvas;
    internal Label lblParameterName;
    internal CheckBox chkBoxShowDataInHEX;
    private bool _contentLoaded;

    public ObservableCollection<MySpecialParam> mySParams { get; set; }

    public ParameterArrayAssistant(myDataRow Parameter)
    {
      this.InitializeComponent();
      this.Data = Parameter;
      this.DataContext = (object) this;
      this.Title = "Change data for " + Parameter.Name;
      this.lblParameterName.Content = (object) Parameter.Name;
      ParameterArrayAssistant.showDataInHex = Parameter.Name + "_showDataInHex";
      string fromUserSettings = FirmwareParameterManager.getStringFromUserSettings(ParameterArrayAssistant.showDataInHex);
      if (!string.IsNullOrEmpty(fromUserSettings))
        this.chkBoxShowDataInHEX.IsChecked = new bool?(fromUserSettings.Contains("true"));
      else
        this.chkBoxShowDataInHEX.IsChecked = new bool?(false);
      if (Parameter.ValType != (Type) null && Parameter.ValType != typeof (byte[]))
      {
        uint num = uint.Parse(this.Data.Bytes.Substring(2), NumberStyles.HexNumber);
        this.TypeSize = Parameter32bit.GetSizeOfType(Parameter.ValType);
        this.txtColumnCount.Text = this.Data.savedColumns != 0 ? this.Data.savedColumns.ToString() : Math.Ceiling((double) (num / this.TypeSize * this.CellHeight) / 140.0).ToString();
        this.txtColumnCount.TextChanged += new TextChangedEventHandler(this.txtColumnCount_TextChanged);
      }
      else
      {
        this.TypeSize = uint.Parse(this.Data.Bytes.Substring(2), NumberStyles.HexNumber);
        this.txtColumnCount.Text = this.Data.savedColumns != 0 ? this.Data.savedColumns.ToString() : "1";
        this.txtColumnCount.IsEnabled = false;
      }
      this.mySParamList = new Dictionary<string, MySpecialParam>();
      this.mySParams = new ObservableCollection<MySpecialParam>();
      this.Width = this.MinWidth = (double) this.MinWindowWidth;
      this.Height = this.MinHeight = (double) this.MinWindowHeight;
      this.buildGRID();
    }

    private void txtColumnCount_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtColumnCount.Text))
        return;
      this.Data.savedColumns = int.Parse(this.txtColumnCount.Text);
      this.buildGRID();
    }

    private void buildGRID()
    {
      try
      {
        this.myTableCanvas.Children.Clear();
        if (this.myTableCanvas.Children.Count != 0)
          this.myTableCanvas.Children.Clear();
        uint length = uint.Parse(this.Data.Bytes.Substring(2), NumberStyles.HexNumber);
        byte[] destinationArray = new byte[(int) length];
        if (this.Data.OrigValue != null)
          Array.Copy((Array) this.Data.OrigValue, (Array) destinationArray, (long) ((byte[]) this.Data.OrigValue).Length);
        else
          destinationArray = new byte[1];
        int int32 = Convert.ToInt32(this.txtColumnCount.Text);
        int num1 = (int) ((long) (length / this.TypeSize) / (long) int32);
        this.Cells = length / this.TypeSize;
        this.CellWidth = this.CharWidth * (this.TypeSize * 2U);
        if (int32 == 1 && num1 == 1)
          this.CellWidth = this.CharWidth * (this.TypeSize * 2U);
        int num2 = 0;
        for (int index1 = 0; index1 <= num1; ++index1)
        {
          for (int index2 = 0; index2 < int32; ++index2)
          {
            TextBox element = new TextBox();
            byte[] numArray = new byte[(int) this.TypeSize];
            for (int index3 = 0; (long) index3 < (long) this.TypeSize; ++index3)
              numArray[index3] = !(this.Data.ValType != (Type) null) ? (destinationArray.Length > index3 ? destinationArray[index3] : Convert.ToByte(0)) : ((long) destinationArray.Length > (long) num2 + (long) this.TypeSize ? destinationArray[(long) num2 * (long) this.TypeSize + (long) index3] : Convert.ToByte(0));
            element.Name = "Cell_" + num2.ToString();
            if (num2 == 0)
              element.Focus();
            if (!this.mySParamList.ContainsKey(element.Name))
            {
              MySpecialParam mySpecialParam = new MySpecialParam();
              mySpecialParam.InternalData = numArray;
              mySpecialParam.Position = (uint) num2;
              mySpecialParam.Name = element.Name;
              this.mySParamList.Add(mySpecialParam.Name, mySpecialParam);
              element.Tag = (object) mySpecialParam;
            }
            else
            {
              MySpecialParam sparam = this.mySParamList[element.Name];
              element.Tag = (object) sparam;
              element.Text = Util.ByteArrayToHexString(sparam.InternalData);
            }
            bool? isChecked;
            if (this.Data.ValType != (Type) null)
            {
              TextBox textBox = element;
              Type valType = this.Data.ValType;
              byte[] bArray = numArray;
              isChecked = this.chkBoxShowDataInHEX.IsChecked;
              int num3 = isChecked.Value ? 1 : 0;
              string str = ParameterArrayAssistant.ByteArrayToType(valType, bArray, num3 != 0).ToString();
              textBox.Text = str;
            }
            else
            {
              isChecked = this.chkBoxShowDataInHEX.IsChecked;
              element.Text = !isChecked.Value ? Util.ByteArrayToString(numArray) : Util.ByteArrayToHexString(numArray);
            }
            element.TextChanged += new TextChangedEventHandler(this.tbx_TextChanged);
            element.GotFocus += new RoutedEventHandler(this.tbx_GotFocus);
            element.LostFocus += new RoutedEventHandler(this.tbx_LostFocus);
            element.ToolTip = (object) ("Position: " + ((MySpecialParam) element.Tag).Position.ToString());
            if (num1 == 1 && length > 100U)
            {
              element.TextWrapping = TextWrapping.Wrap;
              this.CellWidth = 720U;
              this.CellHeight *= length / 65U + 1U;
              element.Height = (double) this.CellHeight;
              element.Width = (double) this.CellWidth;
              element.VerticalAlignment = VerticalAlignment.Top;
              element.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
              element.Height = (double) this.CellHeight;
              element.Width = (double) this.CellWidth;
            }
            element.TextAlignment = TextAlignment.Left;
            element.Margin = new Thickness(5.0, 10.0, 0.0, 0.0);
            this.myTableCanvas.Children.Add((UIElement) element);
            Canvas.SetLeft((UIElement) element, (double) ((long) this.CellWidth * (long) index2));
            Canvas.SetTop((UIElement) element, (double) ((long) this.CellHeight * (long) index1));
            ++num2;
            if ((long) num2 >= (long) this.Cells)
              break;
          }
          if ((long) num2 >= (long) this.Cells)
            break;
        }
        if ((double) ((long) this.CellWidth * (long) int32 + 55L) >= this.MinWidth)
          this.Width = (double) ((long) this.CellWidth * (long) int32 + 55L);
        else
          this.Width = this.MinWidth;
        if ((double) ((long) this.CellHeight * (long) num1 + 205L) >= this.MinHeight)
          this.Height = (double) ((long) this.CellHeight * (long) num1 + 205L);
        else
          this.Height = this.MinHeight;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("ERROR:\n\n" + ex.Message);
      }
    }

    private void ToLittleIndian(ref byte[] cellContent) => Array.Reverse((Array) cellContent);

    private void tbx_LostFocus(object sender, RoutedEventArgs e)
    {
      (sender as TextBox).Select(0, 0);
    }

    private void tbx_GotFocus(object sender, RoutedEventArgs e) => (sender as TextBox).SelectAll();

    private void tbx_TextChanged(object sender, TextChangedEventArgs e)
    {
      TextBox textBox = sender as TextBox;
      MySpecialParam tag = textBox.Tag as MySpecialParam;
      string str = textBox.Text;
      try
      {
        textBox.Background = (Brush) Brushes.White;
        textBox.Foreground = (Brush) Brushes.Black;
        textBox.BorderBrush = (Brush) Brushes.Black;
        textBox.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
        bool flag1 = true;
        this.Data.ValType = this.Data.ValType == (Type) null ? typeof (string) : this.Data.ValType;
        if (this.Data.ValType == typeof (string))
        {
          bool? isChecked = this.chkBoxShowDataInHEX.IsChecked;
          bool flag2 = true;
          if (!(isChecked.GetValueOrDefault() == flag2 & isChecked.HasValue))
            str = Util.ByteArrayToHexString(Util.StringToByteArray(str));
          string valueHeXfull = this.Data.ValueHEXfull;
          if (valueHeXfull.Length != str.Length)
            textBox.Background = (Brush) Brushes.LightGoldenrodYellow;
          if (valueHeXfull.Length < str.Length)
          {
            textBox.BorderBrush = (Brush) Brushes.Red;
            textBox.BorderThickness = new Thickness(1.0, 3.0, 1.0, 3.0);
          }
          if (!valueHeXfull.Equals(str))
            textBox.Foreground = (Brush) Brushes.Red;
          tag.InternalData = ParameterArrayAssistant.TypeToByteArray(this.Data.ValType, (object) str);
        }
        else
        {
          bool? isChecked = this.chkBoxShowDataInHEX.IsChecked;
          bool flag3 = true;
          if (isChecked.GetValueOrDefault() == flag3 & isChecked.HasValue)
            str = ParameterArrayAssistant.HEXtoObject(this.Data.ValType, str).ToString();
          byte[] byteArray1 = ParameterArrayAssistant.TypeToByteArray(this.Data.ValType, (object) str);
          byte[] byteArray2 = ParameterArrayAssistant.TypeToByteArray(this.Data.ValType, (object) str);
          if (this.Data.OrigValue != null)
            Array.Copy((Array) this.Data.OrigValue, (long) (tag.Position * this.TypeSize), (Array) byteArray2, 0L, (long) this.TypeSize);
          else if (this.Data.ValType == typeof (byte[]))
            this.Data.OrigValue = (object) ParameterArrayAssistant.TypeToByteArray(this.Data.ValType, (object) string.Empty.PadLeft((int) this.TypeSize, '0'));
          if (tag.InternalData.Length != byteArray1.Length)
            throw new Exception("wrong lenght...");
          for (int index = 0; index < byteArray1.Length; index = index + 1 + 1)
            flag1 &= (int) byteArray2[index] == (int) byteArray1[index];
          tag.InternalData = byteArray1;
          if (!flag1)
            textBox.Foreground = (Brush) Brushes.Red;
          else
            textBox.Foreground = (Brush) Brushes.Black;
        }
      }
      catch (Exception ex)
      {
        textBox.Background = (Brush) Brushes.LightSalmon;
      }
    }

    private void setBytesToOriginalByteArray()
    {
      byte[] numArray = new byte[(int) this.Cells * (int) this.TypeSize];
      if (this.Data.OrigValue != null)
        numArray = (byte[]) this.Data.OrigValue;
      foreach (MySpecialParam mySpecialParam in this.mySParamList.Values)
      {
        for (int index = 0; (long) index < (long) this.TypeSize; ++index)
          numArray[(long) (mySpecialParam.Position * this.TypeSize) + (long) index] = mySpecialParam.InternalData[index];
      }
      this.Data.OrigValue = (object) numArray;
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      this.setBytesToOriginalByteArray();
      this.isCanceled = false;
      this.Close();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.isCanceled = true;

    private void btnCopy2Clipboard_Click(object sender, RoutedEventArgs e)
    {
      string data = string.Empty;
      bool toHEX = this.chkBoxShowDataInHEX.IsChecked.Value;
      foreach (MySpecialParam mySpecialParam in this.mySParamList.Values)
      {
        data = !(this.Data.ValType != (Type) null) ? (!this.chkBoxShowDataInHEX.IsChecked.Value ? data + Util.ByteArrayToString(mySpecialParam.InternalData) : data + Util.ByteArrayToHexString(mySpecialParam.InternalData)) : data + ParameterArrayAssistant.ByteArrayToType(this.Data.ValType, mySpecialParam.InternalData, toHEX).ToString();
        data += ";";
      }
      Clipboard.SetDataObject((object) data);
    }

    public static object ByteArrayToType(Type localType, byte[] bArray, bool toHEX = true)
    {
      localType.GetType();
      object type;
      if (localType == typeof (int))
        type = toHEX ? (object) Util.ByteArrayToHexString(((IEnumerable<byte>) bArray).Reverse<byte>().ToArray<byte>()) : (object) BitConverter.ToInt32(bArray, 0).ToString();
      else if (localType == typeof (short))
        type = toHEX ? (object) Util.ByteArrayToHexString(((IEnumerable<byte>) bArray).Reverse<byte>().ToArray<byte>()) : (object) BitConverter.ToInt16(bArray, 0).ToString();
      else if (localType == typeof (bool))
        type = toHEX ? (BitConverter.ToBoolean(bArray, 0) ? (object) "1" : (object) "0") : (object) BitConverter.ToBoolean(bArray, 0).ToString();
      else if (localType == typeof (byte))
        type = toHEX ? (object) bArray[0].ToString("x") : (object) bArray[0].ToString();
      else if (localType == typeof (sbyte))
        type = toHEX ? (object) bArray[0].ToString("x") : (object) bArray[0].ToString();
      else if (localType == typeof (float))
        type = toHEX ? (object) ParameterArrayAssistant.float2HEXString(BitConverter.ToSingle(bArray, 0)) : (object) BitConverter.ToSingle(bArray, 0).ToString();
      else if (localType == typeof (float))
        type = toHEX ? (object) ParameterArrayAssistant.float2HEXString(BitConverter.ToSingle(bArray, 0)) : (object) BitConverter.ToSingle(bArray, 0).ToString();
      else if (localType == typeof (double))
        type = toHEX ? (object) Util.ByteArrayToHexString(((IEnumerable<byte>) bArray).Reverse<byte>().ToArray<byte>()) : (object) BitConverter.ToDouble(bArray, 0).ToString();
      else if (localType == typeof (string))
        type = toHEX ? (object) Util.ByteArrayToHexString(((IEnumerable<byte>) bArray).Reverse<byte>().ToArray<byte>()) : (object) Util.ByteArrayToString(((IEnumerable<byte>) bArray).Reverse<byte>().ToArray<byte>());
      else if (localType == typeof (uint))
      {
        type = toHEX ? (object) Util.ByteArrayToHexString(((IEnumerable<byte>) bArray).Reverse<byte>().ToArray<byte>()) : (object) BitConverter.ToUInt32(bArray, 0).ToString();
      }
      else
      {
        if (!(localType == typeof (ushort)))
          throw new InvalidCastException("Unsupported type" + localType.ToString());
        type = toHEX ? (object) Util.ByteArrayToHexString(((IEnumerable<byte>) bArray).Reverse<byte>().ToArray<byte>()) : (object) BitConverter.ToUInt16(bArray, 0).ToString();
      }
      return type;
    }

    public static string float2HEXString(float val)
    {
      return BitConverter.DoubleToInt64Bits((double) val).ToString("x");
    }

    public static float HEXStringToFloat(string val)
    {
      return (float) BitConverter.Int64BitsToDouble(Convert.ToInt64(val, 16));
    }

    public static string double2HEXString(double val)
    {
      return new Double2ulong() { d = val }.ul.ToString("x");
    }

    public static double HEXStringToDouble(string val)
    {
      ulong num = ulong.Parse(val, NumberStyles.AllowHexSpecifier);
      return new Double2ulong() { ul = num }.d;
    }

    public static byte[] TypeToByteArray(Type localType, object localValue)
    {
      byte[] byteArray;
      if (localType == typeof (int))
        byteArray = BitConverter.GetBytes(Convert.ToInt32(localValue));
      else if (localType == typeof (short))
        byteArray = BitConverter.GetBytes(Convert.ToInt16(localValue));
      else if (localType == typeof (bool))
        byteArray = BitConverter.GetBytes(Convert.ToBoolean(localValue));
      else if (localType == typeof (byte))
        byteArray = new byte[1]
        {
          Convert.ToByte(localValue)
        };
      else if (localType == typeof (sbyte))
        byteArray = new byte[1]
        {
          (byte) Convert.ToSByte(localValue)
        };
      else if (localType == typeof (float))
        byteArray = BitConverter.GetBytes(Convert.ToSingle(localValue));
      else if (localType == typeof (double))
        byteArray = BitConverter.GetBytes(Convert.ToDouble(localValue));
      else if (localType == typeof (uint))
        byteArray = BitConverter.GetBytes(Convert.ToUInt32(localValue));
      else if (localType == typeof (ulong))
        byteArray = BitConverter.GetBytes(Convert.ToUInt64(localValue));
      else if (localType == typeof (long))
        byteArray = BitConverter.GetBytes(Convert.ToInt64(localValue));
      else if (localType == typeof (ushort))
      {
        byteArray = BitConverter.GetBytes(Convert.ToUInt16(localValue));
      }
      else
      {
        if (!(localType == typeof (string)))
          throw new InvalidCastException("Unsupported type" + localType.ToString());
        byteArray = Util.HexStringToByteArray(localValue.ToString());
      }
      return byteArray;
    }

    public static object HEXtoObject(Type localType, string localValue)
    {
      object obj;
      if (localType == typeof (int))
        obj = (object) Convert.ToInt32(localValue, 16);
      else if (localType == typeof (short))
        obj = (object) Convert.ToInt16(localValue, 16);
      else if (localType == typeof (bool))
        obj = (object) Convert.ToBoolean(localValue);
      else if (localType == typeof (byte))
        obj = (object) Convert.ToByte(localValue, 16);
      else if (localType == typeof (sbyte))
        obj = (object) Convert.ToSByte(localValue, 16);
      else if (localType == typeof (float))
        obj = (object) ParameterArrayAssistant.HEXStringToFloat(localValue);
      else if (localType == typeof (double))
        obj = (object) ParameterArrayAssistant.HEXStringToDouble(localValue);
      else if (localType == typeof (uint))
        obj = (object) Convert.ToUInt32(localValue, 16);
      else if (localType == typeof (ulong))
        obj = (object) Convert.ToUInt64(localValue, 16);
      else if (localType == typeof (long))
        obj = (object) Convert.ToInt64(localValue, 16);
      else if (localType == typeof (ushort))
      {
        obj = (object) Convert.ToUInt16(localValue, 16);
      }
      else
      {
        if (!(localType == typeof (string)))
          throw new InvalidCastException("Unsupported type" + localType.ToString());
        if (localValue.Length % 2 != 0)
          localValue = "0" + localValue;
        obj = (object) Util.ByteArrayToHexString(Util.HexStringToByteArray(localValue));
      }
      return obj;
    }

    private void chkBoxShowDataInHEX_Checked(object sender, RoutedEventArgs e)
    {
      FirmwareParameterManager.setStringToUserSettings(ParameterArrayAssistant.showDataInHex, this.chkBoxShowDataInHEX.IsChecked.ToString());
      this.buildGRID();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/parameterarrayassistant.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 2:
          this.txtColumnCount = (TextBox) target;
          break;
        case 3:
          this.btnCancel = (Button) target;
          this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
          break;
        case 4:
          this.btnSave = (Button) target;
          this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
          break;
        case 5:
          this.btnCopyToClipboard = (Button) target;
          this.btnCopyToClipboard.Click += new RoutedEventHandler(this.btnCopy2Clipboard_Click);
          break;
        case 6:
          this.myTableCanvas = (Canvas) target;
          break;
        case 7:
          this.lblParameterName = (Label) target;
          break;
        case 8:
          this.chkBoxShowDataInHEX = (CheckBox) target;
          this.chkBoxShowDataInHEX.Click += new RoutedEventHandler(this.chkBoxShowDataInHEX_Checked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
