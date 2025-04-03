// Decompiled with JetBrains decompiler
// Type: CommonWPF.IuwSysInfoDecoder
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace CommonWPF
{
  public partial class IuwSysInfoDecoder : Window, IComponentConnector
  {
    private SortedList<uint, CheckBox> TheBoxList;
    private SortedList<uint, CheckBox> TheStateBoxList;
    private bool LockEvent;
    private bool ShowTheValue;
    internal Grid GridMain;
    internal TabControl TabControl1;
    internal TabItem TabItemSysInfo;
    internal Grid GridSysInfo;
    internal GroupBox GroupBoxInput;
    internal Grid GridInput;
    internal DockPanel DockPanelHex;
    internal Label LabelHex;
    internal TextBox TextBoxHex;
    internal DockPanel DockPanelDec;
    internal Label LabelDec;
    internal TextBox TextBoxDec;
    internal DockPanel DockPanelBin;
    internal Label LabelBin;
    internal TextBox TextBoxBin;
    internal GroupBox GroupBoxFlags;
    internal Grid GridCheckBoxes;
    internal CheckBox CheckBox0;
    internal CheckBox CheckBox1;
    internal CheckBox CheckBox2;
    internal CheckBox CheckBox3;
    internal CheckBox CheckBox4;
    internal CheckBox CheckBox5;
    internal CheckBox CheckBox6;
    internal CheckBox CheckBox7;
    internal CheckBox CheckBox8;
    internal CheckBox CheckBox9;
    internal CheckBox CheckBox10;
    internal CheckBox CheckBox11;
    internal CheckBox CheckBox12;
    internal CheckBox CheckBox13;
    internal CheckBox CheckBox14;
    internal CheckBox CheckBox15;
    internal CheckBox CheckBox16;
    internal CheckBox CheckBox17;
    internal CheckBox CheckBox18;
    internal CheckBox CheckBox19;
    internal CheckBox CheckBox20;
    internal CheckBox CheckBox21;
    internal CheckBox CheckBox22;
    internal CheckBox CheckBox23;
    internal CheckBox CheckBox24;
    internal CheckBox CheckBox25;
    internal CheckBox CheckBox26;
    internal CheckBox CheckBox27;
    internal CheckBox CheckBox28;
    internal CheckBox CheckBox29;
    internal CheckBox CheckBox30;
    internal CheckBox CheckBox31;
    internal TabItem TabItemState;
    internal Grid GridStateInfo;
    internal GroupBox GroupBoxStateInput;
    internal Grid GridStateInput;
    internal DockPanel DockPanelStateHex;
    internal Label LabelStateHex;
    internal TextBox TextBoxStateHex;
    internal DockPanel DockPanelStateDec;
    internal Label LabelStateDec;
    internal TextBox TextBoxStateDec;
    internal DockPanel DockPanelStateBin;
    internal Label LabelStateBin;
    internal TextBox TextBoxStateBin;
    internal GroupBox GroupBoxStateFlags;
    internal Grid GridCheckBoxesState;
    internal CheckBox CheckBoxState0;
    internal CheckBox CheckBoxState1;
    internal CheckBox CheckBoxState2;
    internal CheckBox CheckBoxState3;
    internal CheckBox CheckBoxState4;
    internal CheckBox CheckBoxState5;
    internal CheckBox CheckBoxState6;
    internal CheckBox CheckBoxState7;
    internal CheckBox CheckBoxState8;
    internal CheckBox CheckBoxState9;
    internal CheckBox CheckBoxState10;
    internal CheckBox CheckBoxState11;
    internal CheckBox CheckBoxState12;
    internal CheckBox CheckBoxState13;
    internal CheckBox CheckBoxState14;
    internal CheckBox CheckBoxState15;
    private bool _contentLoaded;

    public static void ShowModal() => new IuwSysInfoDecoder().ShowDialog();

    public IuwSysInfoDecoder()
    {
      this.InitializeComponent();
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(Colors.White, Colors.LightBlue, new System.Windows.Point(0.0, 0.0), new System.Windows.Point(0.0, 1.0));
      this.Background = (Brush) linearGradientBrush;
      this.GroupBoxInput.Background = (Brush) linearGradientBrush;
      this.GroupBoxFlags.Background = (Brush) linearGradientBrush;
      this.GroupBoxStateInput.Background = (Brush) linearGradientBrush;
      this.GroupBoxStateFlags.Background = (Brush) linearGradientBrush;
      this.LockEvent = false;
      this.ShowTheValue = false;
      this.FillBoxesList();
      this.FillStateBoxesList();
    }

    private void FillBoxesList()
    {
      uint key1 = 1;
      this.TheBoxList = new SortedList<uint, CheckBox>();
      this.CheckBox0.Content = (object) (key1.ToString("X8") + " - Smart function event");
      this.TheBoxList.Add(key1, this.CheckBox0);
      uint key2 = key1 * 2U;
      this.CheckBox1.Content = (object) (key2.ToString("X8") + " - NDC module is not able to use the selected communication scenario");
      this.TheBoxList.Add(key2, this.CheckBox1);
      uint key3 = key2 * 2U;
      this.CheckBox2.Content = (object) (key3.ToString("X8") + " - ");
      this.TheBoxList.Add(key3, this.CheckBox2);
      uint key4 = key3 * 2U;
      this.CheckBox3.Content = (object) (key4.ToString("X8") + " - Device in sleep (delivery mode)");
      this.TheBoxList.Add(key4, this.CheckBox3);
      uint key5 = key4 * 2U;
      this.CheckBox4.Content = (object) (key5.ToString("X8") + " - NTAG I2C fault");
      this.TheBoxList.Add(key5, this.CheckBox4);
      uint key6 = key5 * 2U;
      this.CheckBox5.Content = (object) (key6.ToString("X8") + " - Display interpreter error");
      this.TheBoxList.Add(key6, this.CheckBox5);
      uint key7 = key6 * 2U;
      this.CheckBox6.Content = (object) (key7.ToString("X8") + " - Calculated battery live time over");
      this.TheBoxList.Add(key7, this.CheckBox6);
      uint key8 = key7 * 2U;
      this.CheckBox7.Content = (object) (key8.ToString("X8") + " - Battery live time over detected by under voltage (Device works)");
      this.TheBoxList.Add(key8, this.CheckBox7);
      uint key9 = key8 * 2U;
      this.CheckBox8.Content = (object) (key9.ToString("X8") + " - Write protection not active");
      this.TheBoxList.Add(key9, this.CheckBox8);
      uint key10 = key9 * 2U;
      this.CheckBox9.Content = (object) (key10.ToString("X8") + " - Ultrasonic channel 1 corrupt");
      this.TheBoxList.Add(key10, this.CheckBox9);
      uint key11 = key10 * 2U;
      this.CheckBox10.Content = (object) (key11.ToString("X8") + " - Ultrasonic channel 2 corrupt");
      this.TheBoxList.Add(key11, this.CheckBox10);
      uint key12 = key11 * 2U;
      this.CheckBox11.Content = (object) (key12.ToString("X8") + " - Temperature sensor corrupt");
      this.TheBoxList.Add(key12, this.CheckBox11);
      uint key13 = key12 * 2U;
      this.CheckBox12.Content = (object) (key13.ToString("X8") + " - Test-View is active");
      this.TheBoxList.Add(key13, this.CheckBox12);
      uint key14 = key13 * 2U;
      this.CheckBox13.Content = (object) (key14.ToString("X8") + " - Reverse flow");
      this.TheBoxList.Add(key14, this.CheckBox13);
      uint key15 = key14 * 2U;
      this.CheckBox14.Content = (object) (key15.ToString("X8") + " - Temperature out of range");
      this.TheBoxList.Add(key15, this.CheckBox14);
      uint key16 = key15 * 2U;
      this.CheckBox15.Content = (object) (key16.ToString("X8") + " - Flow out of range");
      this.TheBoxList.Add(key16, this.CheckBox15);
      uint key17 = key16 * 2U;
      this.CheckBox16.Content = (object) (key17.ToString("X8") + " - CRC error firmware code");
      this.TheBoxList.Add(key17, this.CheckBox16);
      uint key18 = key17 * 2U;
      this.CheckBox17.Content = (object) (key18.ToString("X8") + " - CRC error configuration");
      this.TheBoxList.Add(key18, this.CheckBox17);
      uint key19 = key18 * 2U;
      this.CheckBox18.Content = (object) (key19.ToString("X8") + " - ");
      this.TheBoxList.Add(key19, this.CheckBox18);
      uint key20 = key19 * 2U;
      this.CheckBox19.Content = (object) (key20.ToString("X8") + " - ");
      this.TheBoxList.Add(key20, this.CheckBox19);
      uint key21 = key20 * 2U;
      this.CheckBox20.Content = (object) (key21.ToString("X8") + " - ");
      this.TheBoxList.Add(key21, this.CheckBox20);
      uint key22 = key21 * 2U;
      this.CheckBox21.Content = (object) (key22.ToString("X8") + " - ");
      this.TheBoxList.Add(key22, this.CheckBox21);
      uint key23 = key22 * 2U;
      this.CheckBox22.Content = (object) (key23.ToString("X8") + " - Bubbles in the water");
      this.TheBoxList.Add(key23, this.CheckBox22);
      uint key24 = key23 * 2U;
      this.CheckBox23.Content = (object) (key24.ToString("X8") + " - No water in the tube (or all ultrasonic channels corrupted)");
      this.TheBoxList.Add(key24, this.CheckBox23);
      uint key25 = key24 * 2U;
      this.CheckBox24.Content = (object) (key25.ToString("X8") + " - ");
      this.TheBoxList.Add(key25, this.CheckBox24);
      uint key26 = key25 * 2U;
      this.CheckBox25.Content = (object) (key26.ToString("X8") + " - ");
      this.TheBoxList.Add(key26, this.CheckBox25);
      uint key27 = key26 * 2U;
      this.CheckBox26.Content = (object) (key27.ToString("X8") + " - ");
      this.TheBoxList.Add(key27, this.CheckBox26);
      uint key28 = key27 * 2U;
      this.CheckBox27.Content = (object) (key28.ToString("X8") + " - ");
      this.TheBoxList.Add(key28, this.CheckBox27);
      uint key29 = key28 * 2U;
      this.CheckBox28.Content = (object) (key29.ToString("X8") + " - ");
      this.TheBoxList.Add(key29, this.CheckBox28);
      uint key30 = key29 * 2U;
      this.CheckBox29.Content = (object) (key30.ToString("X8") + " - The meter lost its accumulated data");
      this.TheBoxList.Add(key30, this.CheckBox29);
      uint key31 = key30 * 2U;
      this.CheckBox30.Content = (object) (key31.ToString("X8") + " - TDC-Error");
      this.TheBoxList.Add(key31, this.CheckBox30);
      uint key32 = key31 * 2U;
      this.CheckBox31.Content = (object) (key32.ToString("X8") + " - Battery down (Device out of order)");
      this.TheBoxList.Add(key32, this.CheckBox31);
    }

    private void FillStateBoxesList()
    {
      uint key1 = 1;
      this.TheStateBoxList = new SortedList<uint, CheckBox>();
      this.CheckBoxState0.Content = (object) (key1.ToString("X4") + " - BatteryOver");
      this.TheStateBoxList.Add(key1, this.CheckBoxState0);
      uint key2 = key1 * 2U;
      this.CheckBoxState1.Content = (object) (key2.ToString("X4") + " - BatteryWarning");
      this.TheStateBoxList.Add(key2, this.CheckBoxState1);
      uint key3 = key2 * 2U;
      this.CheckBoxState2.Content = (object) (key3.ToString("X4") + " - BatteryError");
      this.TheStateBoxList.Add(key3, this.CheckBoxState2);
      uint key4 = key3 * 2U;
      this.CheckBoxState3.Content = (object) (key4.ToString("X4") + " - AccuracyUnsafe");
      this.TheStateBoxList.Add(key4, this.CheckBoxState3);
      uint key5 = key4 * 2U;
      this.CheckBoxState4.Content = (object) (key5.ToString("X4") + " - HardwareError");
      this.TheStateBoxList.Add(key5, this.CheckBoxState4);
      uint key6 = key5 * 2U;
      this.CheckBoxState5.Content = (object) (key6.ToString("X4") + " - EmptyTube");
      this.TheStateBoxList.Add(key6, this.CheckBoxState5);
      uint key7 = key6 * 2U;
      this.CheckBoxState6.Content = (object) (key7.ToString("X4") + " - FlowOutOfRange");
      this.TheStateBoxList.Add(key7, this.CheckBoxState6);
      uint key8 = key7 * 2U;
      this.CheckBoxState7.Content = (object) (key8.ToString("X4") + " - Sleep");
      this.TheStateBoxList.Add(key8, this.CheckBoxState7);
      uint key9 = key8 * 2U;
      this.CheckBoxState8.Content = (object) (key9.ToString("X4") + " - ");
      this.TheStateBoxList.Add(key9, this.CheckBoxState8);
      uint key10 = key9 * 2U;
      this.CheckBoxState9.Content = (object) (key10.ToString("X4") + " - ");
      this.TheStateBoxList.Add(key10, this.CheckBoxState9);
      uint key11 = key10 * 2U;
      this.CheckBoxState10.Content = (object) (key11.ToString("X4") + " - ");
      this.TheStateBoxList.Add(key11, this.CheckBoxState10);
      uint key12 = key11 * 2U;
      this.CheckBoxState11.Content = (object) (key12.ToString("X4") + " - ");
      this.TheStateBoxList.Add(key12, this.CheckBoxState11);
      uint key13 = key12 * 2U;
      this.CheckBoxState12.Content = (object) (key13.ToString("X4") + " - ");
      this.TheStateBoxList.Add(key13, this.CheckBoxState12);
      uint key14 = key13 * 2U;
      this.CheckBoxState13.Content = (object) (key14.ToString("X4") + " - Reverse flow");
      this.TheStateBoxList.Add(key14, this.CheckBoxState13);
      uint key15 = key14 * 2U;
      this.CheckBoxState14.Content = (object) (key15.ToString("X4") + " - NdcModuleBatteryWarning");
      this.TheStateBoxList.Add(key15, this.CheckBoxState14);
      uint key16 = key15 * 2U;
      this.CheckBoxState15.Content = (object) (key16.ToString("X4") + " - NdcModuleConnectionLost");
      this.TheStateBoxList.Add(key16, this.CheckBoxState15);
      uint num = key16 * 2U;
    }

    private void ClearAllBoxes()
    {
      foreach (ToggleButton toggleButton in (IEnumerable<CheckBox>) this.TheBoxList.Values)
        toggleButton.IsChecked = new bool?(false);
    }

    private void ShowBoxStates(uint TheValue)
    {
      foreach (KeyValuePair<uint, CheckBox> theBox in this.TheBoxList)
        theBox.Value.IsChecked = new bool?(((int) TheValue & (int) theBox.Key) == (int) theBox.Key);
    }

    private void ShowBinaryValue()
    {
      string str = string.Empty;
      int num = 0;
      foreach (CheckBox checkBox in (IEnumerable<CheckBox>) this.TheBoxList.Values)
      {
        ++num;
        str = !checkBox.IsChecked.Value ? "0" + str : "1" + str;
        if (num >= 4)
        {
          num = 0;
          str = " " + str;
        }
      }
      this.TextBoxBin.Text = str;
    }

    private void ShowValue()
    {
      uint num = 0;
      foreach (KeyValuePair<uint, CheckBox> theBox in this.TheBoxList)
      {
        if (theBox.Value.IsChecked.Value)
          num += theBox.Key;
      }
      this.ShowTheValue = true;
      if (this.TextBoxDec.Text.Trim() != string.Empty)
      {
        this.TextBoxHex.Text = string.Empty;
        this.TextBoxDec.Text = num.ToString();
      }
      else
      {
        this.TextBoxHex.Text = num.ToString("X");
        this.TextBoxDec.Text = string.Empty;
      }
      this.ShowBinaryValue();
      this.ShowTheValue = false;
    }

    private void ClearAllStateBoxes()
    {
      foreach (ToggleButton toggleButton in (IEnumerable<CheckBox>) this.TheStateBoxList.Values)
        toggleButton.IsChecked = new bool?(false);
    }

    private void ShowStateBoxStates(uint TheValue)
    {
      foreach (KeyValuePair<uint, CheckBox> theStateBox in this.TheStateBoxList)
        theStateBox.Value.IsChecked = new bool?(((int) TheValue & (int) theStateBox.Key) == (int) theStateBox.Key);
    }

    private void ShowStateBinaryValue()
    {
      string str = string.Empty;
      int num = 0;
      foreach (CheckBox checkBox in (IEnumerable<CheckBox>) this.TheStateBoxList.Values)
      {
        ++num;
        str = !checkBox.IsChecked.Value ? "0" + str : "1" + str;
        if (num >= 4)
        {
          num = 0;
          str = " " + str;
        }
      }
      this.TextBoxStateBin.Text = str;
    }

    private void ShowStateValue()
    {
      uint num = 0;
      foreach (KeyValuePair<uint, CheckBox> theStateBox in this.TheStateBoxList)
      {
        if (theStateBox.Value.IsChecked.Value)
          num += theStateBox.Key;
      }
      this.ShowTheValue = true;
      if (this.TextBoxStateDec.Text.Trim() != string.Empty)
      {
        this.TextBoxStateHex.Text = string.Empty;
        this.TextBoxStateDec.Text = num.ToString();
      }
      else
      {
        this.TextBoxStateHex.Text = num.ToString("X");
        this.TextBoxStateDec.Text = string.Empty;
      }
      this.ShowStateBinaryValue();
      this.ShowTheValue = false;
    }

    private void TextBoxHex_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.ShowTheValue)
        return;
      if (!this.LockEvent)
      {
        this.LockEvent = true;
        this.TextBoxBin.Clear();
        this.TextBoxDec.Clear();
        this.DecodeTheValue();
      }
      this.LockEvent = false;
    }

    private void TextBoxDec_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.ShowTheValue)
        return;
      if (!this.LockEvent)
      {
        this.LockEvent = true;
        this.TextBoxBin.Clear();
        this.TextBoxHex.Clear();
        this.DecodeTheValue();
      }
      this.LockEvent = false;
    }

    private void TextBoxStateHex_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.ShowTheValue)
        return;
      if (!this.LockEvent)
      {
        this.LockEvent = true;
        this.TextBoxStateBin.Clear();
        this.TextBoxStateDec.Clear();
        this.DecodeTheStateValue();
      }
      this.LockEvent = false;
    }

    private void TextBoxStateDec_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.ShowTheValue)
        return;
      if (!this.LockEvent)
      {
        this.LockEvent = true;
        this.TextBoxStateBin.Clear();
        this.TextBoxStateHex.Clear();
        this.DecodeTheStateValue();
      }
      this.LockEvent = false;
    }

    private void CheckBox_Click(object sender, RoutedEventArgs e) => this.ShowValue();

    private void CheckBoxState_Click(object sender, RoutedEventArgs e) => this.ShowStateValue();

    private void DecodeTheValue()
    {
      try
      {
        bool flag = true;
        uint TheValue = 0;
        if (this.TextBoxDec.Text.Trim() != string.Empty)
          TheValue = uint.Parse(this.TextBoxDec.Text.Trim());
        else if (this.TextBoxHex.Text.Trim() != string.Empty)
        {
          TheValue = uint.Parse(this.TextBoxHex.Text.Trim(), NumberStyles.HexNumber);
        }
        else
        {
          this.ClearAllBoxes();
          this.TextBoxBin.Clear();
          flag = false;
        }
        if (!flag)
          return;
        this.ShowBoxStates(TheValue);
        this.ShowBinaryValue();
      }
      catch
      {
        this.TextBoxBin.Clear();
        this.ClearAllBoxes();
      }
    }

    private void DecodeTheStateValue()
    {
      try
      {
        bool flag = true;
        uint TheValue = 0;
        if (this.TextBoxStateDec.Text.Trim() != string.Empty)
          TheValue = uint.Parse(this.TextBoxStateDec.Text.Trim());
        else if (this.TextBoxStateHex.Text.Trim() != string.Empty)
        {
          TheValue = uint.Parse(this.TextBoxStateHex.Text.Trim(), NumberStyles.HexNumber);
        }
        else
        {
          this.ClearAllStateBoxes();
          this.TextBoxStateBin.Clear();
          flag = false;
        }
        if (!flag)
          return;
        this.ShowStateBoxStates(TheValue);
        this.ShowStateBinaryValue();
      }
      catch
      {
        this.TextBoxStateBin.Clear();
        this.ClearAllStateBoxes();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/iuwsysinfodecoder.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.GridMain = (Grid) target;
          break;
        case 2:
          this.TabControl1 = (TabControl) target;
          break;
        case 3:
          this.TabItemSysInfo = (TabItem) target;
          break;
        case 4:
          this.GridSysInfo = (Grid) target;
          break;
        case 5:
          this.GroupBoxInput = (GroupBox) target;
          break;
        case 6:
          this.GridInput = (Grid) target;
          break;
        case 7:
          this.DockPanelHex = (DockPanel) target;
          break;
        case 8:
          this.LabelHex = (Label) target;
          break;
        case 9:
          this.TextBoxHex = (TextBox) target;
          this.TextBoxHex.TextChanged += new TextChangedEventHandler(this.TextBoxHex_TextChanged);
          break;
        case 10:
          this.DockPanelDec = (DockPanel) target;
          break;
        case 11:
          this.LabelDec = (Label) target;
          break;
        case 12:
          this.TextBoxDec = (TextBox) target;
          this.TextBoxDec.TextChanged += new TextChangedEventHandler(this.TextBoxDec_TextChanged);
          break;
        case 13:
          this.DockPanelBin = (DockPanel) target;
          break;
        case 14:
          this.LabelBin = (Label) target;
          break;
        case 15:
          this.TextBoxBin = (TextBox) target;
          break;
        case 16:
          this.GroupBoxFlags = (GroupBox) target;
          break;
        case 17:
          this.GridCheckBoxes = (Grid) target;
          break;
        case 18:
          this.CheckBox0 = (CheckBox) target;
          this.CheckBox0.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 19:
          this.CheckBox1 = (CheckBox) target;
          this.CheckBox1.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 20:
          this.CheckBox2 = (CheckBox) target;
          this.CheckBox2.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 21:
          this.CheckBox3 = (CheckBox) target;
          this.CheckBox3.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 22:
          this.CheckBox4 = (CheckBox) target;
          this.CheckBox4.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 23:
          this.CheckBox5 = (CheckBox) target;
          this.CheckBox5.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 24:
          this.CheckBox6 = (CheckBox) target;
          this.CheckBox6.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 25:
          this.CheckBox7 = (CheckBox) target;
          this.CheckBox7.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 26:
          this.CheckBox8 = (CheckBox) target;
          this.CheckBox8.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 27:
          this.CheckBox9 = (CheckBox) target;
          this.CheckBox9.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 28:
          this.CheckBox10 = (CheckBox) target;
          this.CheckBox10.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 29:
          this.CheckBox11 = (CheckBox) target;
          this.CheckBox11.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 30:
          this.CheckBox12 = (CheckBox) target;
          this.CheckBox12.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 31:
          this.CheckBox13 = (CheckBox) target;
          this.CheckBox13.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 32:
          this.CheckBox14 = (CheckBox) target;
          this.CheckBox14.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 33:
          this.CheckBox15 = (CheckBox) target;
          this.CheckBox15.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 34:
          this.CheckBox16 = (CheckBox) target;
          this.CheckBox16.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 35:
          this.CheckBox17 = (CheckBox) target;
          this.CheckBox17.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 36:
          this.CheckBox18 = (CheckBox) target;
          this.CheckBox18.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 37:
          this.CheckBox19 = (CheckBox) target;
          this.CheckBox19.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 38:
          this.CheckBox20 = (CheckBox) target;
          this.CheckBox20.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 39:
          this.CheckBox21 = (CheckBox) target;
          this.CheckBox21.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 40:
          this.CheckBox22 = (CheckBox) target;
          this.CheckBox22.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 41:
          this.CheckBox23 = (CheckBox) target;
          this.CheckBox23.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 42:
          this.CheckBox24 = (CheckBox) target;
          this.CheckBox24.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 43:
          this.CheckBox25 = (CheckBox) target;
          this.CheckBox25.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 44:
          this.CheckBox26 = (CheckBox) target;
          this.CheckBox26.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 45:
          this.CheckBox27 = (CheckBox) target;
          this.CheckBox27.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 46:
          this.CheckBox28 = (CheckBox) target;
          this.CheckBox28.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 47:
          this.CheckBox29 = (CheckBox) target;
          this.CheckBox29.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 48:
          this.CheckBox30 = (CheckBox) target;
          this.CheckBox30.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 49:
          this.CheckBox31 = (CheckBox) target;
          this.CheckBox31.Click += new RoutedEventHandler(this.CheckBox_Click);
          break;
        case 50:
          this.TabItemState = (TabItem) target;
          break;
        case 51:
          this.GridStateInfo = (Grid) target;
          break;
        case 52:
          this.GroupBoxStateInput = (GroupBox) target;
          break;
        case 53:
          this.GridStateInput = (Grid) target;
          break;
        case 54:
          this.DockPanelStateHex = (DockPanel) target;
          break;
        case 55:
          this.LabelStateHex = (Label) target;
          break;
        case 56:
          this.TextBoxStateHex = (TextBox) target;
          this.TextBoxStateHex.TextChanged += new TextChangedEventHandler(this.TextBoxStateHex_TextChanged);
          break;
        case 57:
          this.DockPanelStateDec = (DockPanel) target;
          break;
        case 58:
          this.LabelStateDec = (Label) target;
          break;
        case 59:
          this.TextBoxStateDec = (TextBox) target;
          this.TextBoxStateDec.TextChanged += new TextChangedEventHandler(this.TextBoxStateDec_TextChanged);
          break;
        case 60:
          this.DockPanelStateBin = (DockPanel) target;
          break;
        case 61:
          this.LabelStateBin = (Label) target;
          break;
        case 62:
          this.TextBoxStateBin = (TextBox) target;
          break;
        case 63:
          this.GroupBoxStateFlags = (GroupBox) target;
          break;
        case 64:
          this.GridCheckBoxesState = (Grid) target;
          break;
        case 65:
          this.CheckBoxState0 = (CheckBox) target;
          this.CheckBoxState0.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 66:
          this.CheckBoxState1 = (CheckBox) target;
          this.CheckBoxState1.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 67:
          this.CheckBoxState2 = (CheckBox) target;
          this.CheckBoxState2.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 68:
          this.CheckBoxState3 = (CheckBox) target;
          this.CheckBoxState3.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 69:
          this.CheckBoxState4 = (CheckBox) target;
          this.CheckBoxState4.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 70:
          this.CheckBoxState5 = (CheckBox) target;
          this.CheckBoxState5.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 71:
          this.CheckBoxState6 = (CheckBox) target;
          this.CheckBoxState6.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 72:
          this.CheckBoxState7 = (CheckBox) target;
          this.CheckBoxState7.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 73:
          this.CheckBoxState8 = (CheckBox) target;
          this.CheckBoxState8.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 74:
          this.CheckBoxState9 = (CheckBox) target;
          this.CheckBoxState9.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 75:
          this.CheckBoxState10 = (CheckBox) target;
          this.CheckBoxState10.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 76:
          this.CheckBoxState11 = (CheckBox) target;
          this.CheckBoxState11.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 77:
          this.CheckBoxState12 = (CheckBox) target;
          this.CheckBoxState12.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 78:
          this.CheckBoxState13 = (CheckBox) target;
          this.CheckBoxState13.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 79:
          this.CheckBoxState14 = (CheckBox) target;
          this.CheckBoxState14.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        case 80:
          this.CheckBoxState15 = (CheckBox) target;
          this.CheckBoxState15.Click += new RoutedEventHandler(this.CheckBoxState_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
