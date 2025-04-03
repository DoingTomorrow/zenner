// Decompiled with JetBrains decompiler
// Type: S3_Handler.TDC_Internals
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace S3_Handler
{
  public class TDC_Internals : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private SortedList<string, ParameterData> parameterByName;
    private SortedList<int, ParameterData> parameterByAddress;
    private int minReadAdr;
    private int readByteSize;
    private bool breakLoop;
    private StringBuilder LineBuilder = new StringBuilder(1000);
    private TDC_Internals.TdcInternalListSelection activeList = ~TDC_Internals.TdcInternalListSelection.standard;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private TextBox textBoxOutData;
    private Button buttonRun;
    private Button buttonRunLoop;
    private Button buttonBreakLoop;
    private Label label1;
    private TextBox textBoxLoopTimeMs;
    private Button buttonClear;
    private ComboBox comboBoxListSelect;

    public TDC_Internals(S3_HandlerFunctions MyFunctions)
    {
      this.InitializeComponent();
      this.MyFunctions = MyFunctions;
      this.comboBoxListSelect.Items.AddRange((object[]) Enum.GetNames(typeof (TDC_Internals.TdcInternalListSelection)));
      this.comboBoxListSelect.SelectedIndex = 0;
      this.parameterByName = new SortedList<string, ParameterData>();
      this.parameterByAddress = new SortedList<int, ParameterData>();
      this.SetParameterList(TDC_Internals.TdcInternalListSelection.standard);
      this.minReadAdr = this.parameterByAddress.Values[0].address;
      this.readByteSize = this.parameterByAddress.Values[this.parameterByAddress.Count - 1].address + this.parameterByAddress.Values[this.parameterByAddress.Count - 1].byteSize - this.minReadAdr;
      this.ClearText();
    }

    private void SetParameterList(
      TDC_Internals.TdcInternalListSelection listSelection)
    {
      if (listSelection == this.activeList)
        return;
      switch (listSelection)
      {
        case TDC_Internals.TdcInternalListSelection.standard:
          this.activeList = TDC_Internals.TdcInternalListSelection.standard;
          this.parameterByName.Clear();
          this.parameterByAddress.Clear();
          S3_ParameterNames s3ParameterNames1 = S3_ParameterNames.tdc_n_up0;
          this.AddParameter("n_up0", 14, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_n_up1;
          this.AddParameter("n_up1", 25, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_n_up2;
          this.AddParameter("n_up2", 36, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_n_up3;
          this.AddParameter("n_up3", 47, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_n_down0;
          this.AddParameter("n_dn0", 58, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_n_down1;
          this.AddParameter("n_dn1", 69, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_n_down2;
          this.AddParameter("n_dn2", 80, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_n_down3;
          this.AddParameter("n_dn3", 91, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_pw_up;
          this.AddParameter("pu", 102, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_pw_down;
          this.AddParameter("pd", 107, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_active_offset_level;
          this.AddParameter("lv", 112, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_present_flow;
          this.AddParameter("Flow", 116, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_flow_without_corr;
          this.AddParameter("|Flow_hp|", 126, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_calib_resonator_correction_value;
          this.AddParameter("korr", 136, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_sonic_velocity;
          this.AddParameter("C_tdc", 146, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdc_temp_to_sonic_velocity;
          this.AddParameter("C_temp", 156, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.vorlauftemperatur;
          this.AddParameter("t_vl", 166, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.ruecklauftemperatur;
          this.AddParameter("t_rl", 172, s3ParameterNames1.ToString());
          s3ParameterNames1 = S3_ParameterNames.tdcStatusErrorFlags;
          this.AddParameter("help1", 178, s3ParameterNames1.ToString());
          this.InsertHeaderLine();
          break;
        case TDC_Internals.TdcInternalListSelection.extendet1:
          this.activeList = TDC_Internals.TdcInternalListSelection.extendet1;
          this.parameterByName.Clear();
          this.parameterByAddress.Clear();
          S3_ParameterNames s3ParameterNames2 = S3_ParameterNames.tdc_n_up0;
          this.AddParameter("n_up0", 14, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_up1;
          this.AddParameter("n_up1", 25, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_up2;
          this.AddParameter("n_up2", 36, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_up3;
          this.AddParameter("n_up3", 47, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_down0;
          this.AddParameter("n_dn0", 58, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_down1;
          this.AddParameter("n_dn1", 69, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_down2;
          this.AddParameter("n_dn2", 80, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_down3;
          this.AddParameter("n_dn3", 91, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_pw_up;
          this.AddParameter("pu", 102, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_pw_down;
          this.AddParameter("pd", 108, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_up0_c2;
          this.AddParameter("n_up0_c2", 114, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_up1_c2;
          this.AddParameter("n_up1_c2", 125, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_up2_c2;
          this.AddParameter("n_up2_c2", 136, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_up3_c2;
          this.AddParameter("n_up3_c2", 147, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_down0_c2;
          this.AddParameter("n_dn0_c2", 158, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_down1_c2;
          this.AddParameter("n_dn1_c2", 169, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_down2_c2;
          this.AddParameter("n_dn2_c2", 180, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_n_down3_c2;
          this.AddParameter("n_dn3_c2", 191, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_pw_up_c2;
          this.AddParameter("pu_c2", 202, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_pw_down_c2;
          this.AddParameter("pd_c2", 210, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_active_offset_level;
          this.AddParameter("lv", 215, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_present_flow;
          this.AddParameter("Flow", 219, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_flow_without_corr;
          this.AddParameter("|Flow_hp|", 229, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_calib_resonator_correction_value;
          this.AddParameter("korr", 239, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_sonic_velocity;
          this.AddParameter("C_tdc", 249, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdc_temp_to_sonic_velocity;
          this.AddParameter("C_temp", 259, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.vorlauftemperatur;
          this.AddParameter("t_vl", 269, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.ruecklauftemperatur;
          this.AddParameter("t_rl", 275, s3ParameterNames2.ToString());
          s3ParameterNames2 = S3_ParameterNames.tdcStatusErrorFlags;
          this.AddParameter("help1", 281, s3ParameterNames2.ToString());
          this.InsertHeaderLine();
          break;
      }
    }

    private void AddParameter(string name, int dispOffset, string firmwareName)
    {
      S3_Parameter s3Parameter = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName[firmwareName];
      ParameterData parameterData = new ParameterData(name, s3Parameter.BlockStartAddress, s3Parameter.ByteSize, dispOffset);
      this.parameterByName.Add(name, parameterData);
      this.parameterByAddress.Add(s3Parameter.BlockStartAddress, parameterData);
    }

    private void AddParameter(string name, int address, int byteSize, int dispOffset)
    {
      ParameterData parameterData = new ParameterData(name, address, byteSize, dispOffset);
      this.parameterByName.Add(name, parameterData);
      this.parameterByAddress.Add(address, parameterData);
    }

    private void buttonRun_Click(object sender, EventArgs e) => this.ReadAndShowData();

    private void buttonRunLoop_Click(object sender, EventArgs e)
    {
      this.buttonBreakLoop.Enabled = true;
      this.buttonRun.Enabled = false;
      this.buttonRunLoop.Enabled = false;
      this.buttonClear.Enabled = false;
      this.breakLoop = false;
      DateTime dateTime = DateTime.Now;
      TimeSpan timeSpan1 = new TimeSpan(0, 0, 0, 0, 300);
      TimeSpan timeSpan2 = new TimeSpan(0L);
      int num = 1000;
      while (!this.breakLoop)
      {
        if (dateTime <= DateTime.Now)
        {
          this.ReadAndShowData();
          dateTime = dateTime.AddMilliseconds((double) num);
        }
        Application.DoEvents();
        int result;
        if (int.TryParse(this.textBoxLoopTimeMs.Text, out result))
          num = result;
        TimeSpan timeSpan3 = dateTime.Subtract(DateTime.Now);
        if (timeSpan3 > timeSpan1)
          Thread.Sleep(200);
        else if (timeSpan3 > timeSpan2)
          Thread.Sleep(timeSpan3.Milliseconds);
      }
      this.buttonBreakLoop.Enabled = false;
      this.buttonRun.Enabled = true;
      this.buttonRunLoop.Enabled = true;
      this.buttonClear.Enabled = true;
    }

    private void buttonBreakLoop_Click(object sender, EventArgs e) => this.breakLoop = true;

    private void buttonClear_Click(object sender, EventArgs e) => this.ClearText();

    private void ClearText()
    {
      this.LineBuilder.Length = 0;
      this.SetLineText(0, "Time");
      foreach (ParameterData parameterData in (IEnumerable<ParameterData>) this.parameterByName.Values)
        this.SetLineText(parameterData.dispOffset, ";" + parameterData.name);
      this.LineBuilder.AppendLine();
      this.textBoxOutData.Text = this.LineBuilder.ToString();
    }

    private void InsertHeaderLine()
    {
      this.LineBuilder.Length = 0;
      this.SetLineText(0, "Time");
      foreach (ParameterData parameterData in (IEnumerable<ParameterData>) this.parameterByName.Values)
        this.SetLineText(parameterData.dispOffset, ";" + parameterData.name);
      this.LineBuilder.AppendLine();
      this.textBoxOutData.AppendText(this.LineBuilder.ToString());
    }

    private void SetLineText(int index, string text)
    {
      int num = index + text.Length;
      while (this.LineBuilder.Length < num)
        this.LineBuilder.Append(' ');
      for (int index1 = 0; index1 < text.Length; ++index1)
        this.LineBuilder[index + index1] = text[index1];
    }

    private void ReadAndShowData()
    {
      this.SetParameterList((TDC_Internals.TdcInternalListSelection) this.comboBoxListSelect.SelectedIndex);
      this.LineBuilder.Length = 0;
      this.LineBuilder.Append(DateTime.Now.ToString("HH:mm:ss.fff"));
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      if (!workMeter.MyDeviceMemory.ReadDataFromConnectedDevice(this.minReadAdr, this.readByteSize))
      {
        this.LineBuilder.Append(" read error");
      }
      else
      {
        string key1 = "n_up0";
        uint uintValue;
        if (this.parameterByName.ContainsKey(key1))
        {
          string valueName = key1;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key1].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key2 = "n_up1";
        if (this.parameterByName.ContainsKey(key2))
        {
          string valueName = key2;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key2].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key3 = "n_up2";
        if (this.parameterByName.ContainsKey(key3))
        {
          string valueName = key3;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key3].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key4 = "n_up3";
        if (this.parameterByName.ContainsKey(key4))
        {
          string valueName = key4;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key4].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key5 = "n_dn0";
        if (this.parameterByName.ContainsKey(key5))
        {
          string valueName = key5;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key5].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key6 = "n_dn1";
        if (this.parameterByName.ContainsKey(key6))
        {
          string valueName = key6;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key6].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key7 = "n_dn2";
        if (this.parameterByName.ContainsKey(key7))
        {
          string valueName = key7;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key7].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key8 = "n_dn3";
        if (this.parameterByName.ContainsKey(key8))
        {
          string valueName = key8;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key8].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key9 = "pu";
        byte byteValue;
        if (this.parameterByName.ContainsKey(key9))
        {
          string valueName = key9;
          byteValue = workMeter.MyDeviceMemory.GetByteValue(this.parameterByName[key9].address);
          string valueString = byteValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key10 = "pd";
        if (this.parameterByName.ContainsKey(key10))
        {
          string valueName = key10;
          byteValue = workMeter.MyDeviceMemory.GetByteValue(this.parameterByName[key10].address);
          string valueString = byteValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string str = "lv";
        if (this.parameterByName.ContainsKey(str))
          this.insertValue(str, workMeter.MyDeviceMemory.GetSByteValue(this.parameterByName[str].address).ToString());
        string key11 = "Flow";
        float floatValue;
        if (this.parameterByName.ContainsKey(key11))
        {
          string valueName = key11;
          floatValue = workMeter.MyDeviceMemory.GetFloatValue(this.parameterByName[key11].address);
          string valueString = floatValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key12 = "|Flow_hp|";
        if (this.parameterByName.ContainsKey(key12))
        {
          string valueName = key12;
          floatValue = workMeter.MyDeviceMemory.GetFloatValue(this.parameterByName[key12].address);
          string valueString = floatValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key13 = "korr";
        if (this.parameterByName.ContainsKey(key13))
        {
          string valueName = key13;
          floatValue = workMeter.MyDeviceMemory.GetFloatValue(this.parameterByName[key13].address);
          string valueString = floatValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key14 = "C_tdc";
        if (this.parameterByName.ContainsKey(key14))
        {
          string valueName = key14;
          floatValue = workMeter.MyDeviceMemory.GetFloatValue(this.parameterByName[key14].address);
          string valueString = floatValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key15 = "C_temp";
        if (this.parameterByName.ContainsKey(key15))
        {
          string valueName = key15;
          floatValue = workMeter.MyDeviceMemory.GetFloatValue(this.parameterByName[key15].address);
          string valueString = floatValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key16 = "t_vl";
        short shortValue;
        if (this.parameterByName.ContainsKey(key16))
        {
          string valueName = key16;
          shortValue = workMeter.MyDeviceMemory.GetShortValue(this.parameterByName[key16].address);
          string valueString = shortValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key17 = "t_rl";
        if (this.parameterByName.ContainsKey(key17))
        {
          string valueName = key17;
          shortValue = workMeter.MyDeviceMemory.GetShortValue(this.parameterByName[key17].address);
          string valueString = shortValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key18 = "help1";
        if (this.parameterByName.ContainsKey(key18))
        {
          string valueName = key18;
          byteValue = workMeter.MyDeviceMemory.GetByteValue(this.parameterByName[key18].address);
          string valueString = byteValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key19 = "n_up0_c2";
        if (this.parameterByName.ContainsKey(key19))
        {
          string valueName = key19;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key19].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key20 = "n_up1_c2";
        if (this.parameterByName.ContainsKey(key20))
        {
          string valueName = key20;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key20].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key21 = "n_up2_c2";
        if (this.parameterByName.ContainsKey(key21))
        {
          string valueName = key21;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key21].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key22 = "n_up3_c2";
        if (this.parameterByName.ContainsKey(key22))
        {
          string valueName = key22;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key22].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key23 = "n_dn0_c2";
        if (this.parameterByName.ContainsKey(key23))
        {
          string valueName = key23;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key23].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key24 = "n_dn1_c2";
        if (this.parameterByName.ContainsKey(key24))
        {
          string valueName = key24;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key24].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key25 = "n_dn2_c2";
        if (this.parameterByName.ContainsKey(key25))
        {
          string valueName = key25;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key25].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key26 = "n_dn3_c2";
        if (this.parameterByName.ContainsKey(key26))
        {
          string valueName = key26;
          uintValue = workMeter.MyDeviceMemory.GetUintValue(this.parameterByName[key26].address);
          string valueString = uintValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key27 = "pu_c2";
        if (this.parameterByName.ContainsKey(key27))
        {
          string valueName = key27;
          byteValue = workMeter.MyDeviceMemory.GetByteValue(this.parameterByName[key27].address);
          string valueString = byteValue.ToString();
          this.insertValue(valueName, valueString);
        }
        string key28 = "pd_c2";
        if (this.parameterByName.ContainsKey(key28))
        {
          string valueName = key28;
          byteValue = workMeter.MyDeviceMemory.GetByteValue(this.parameterByName[key28].address);
          string valueString = byteValue.ToString();
          this.insertValue(valueName, valueString);
        }
      }
      this.LineBuilder.AppendLine();
      this.textBoxOutData.AppendText(this.LineBuilder.ToString());
    }

    private void insertValue(string valueName, string valueString)
    {
      this.SetLineText(this.parameterByName[valueName].dispOffset, ";" + valueString);
    }

    private void TDC_Internals_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.buttonRun.Enabled)
        return;
      e.Cancel = true;
    }

    private void textBoxOutData_TextChanged(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.textBoxOutData = new TextBox();
      this.buttonRun = new Button();
      this.buttonRunLoop = new Button();
      this.buttonBreakLoop = new Button();
      this.label1 = new Label();
      this.textBoxLoopTimeMs = new TextBox();
      this.buttonClear = new Button();
      this.comboBoxListSelect = new ComboBox();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1051, 41);
      this.zennerCoroprateDesign2.TabIndex = 19;
      this.textBoxOutData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxOutData.Font = new Font("Consolas", 6.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxOutData.Location = new Point(12, 46);
      this.textBoxOutData.Multiline = true;
      this.textBoxOutData.Name = "textBoxOutData";
      this.textBoxOutData.ScrollBars = ScrollBars.Both;
      this.textBoxOutData.Size = new Size(1027, 452);
      this.textBoxOutData.TabIndex = 20;
      this.textBoxOutData.WordWrap = false;
      this.textBoxOutData.TextChanged += new EventHandler(this.textBoxOutData_TextChanged);
      this.buttonRun.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRun.Location = new Point(932, 515);
      this.buttonRun.Name = "buttonRun";
      this.buttonRun.Size = new Size(107, 23);
      this.buttonRun.TabIndex = 21;
      this.buttonRun.Text = "Run";
      this.buttonRun.UseVisualStyleBackColor = true;
      this.buttonRun.Click += new EventHandler(this.buttonRun_Click);
      this.buttonRunLoop.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRunLoop.Location = new Point(819, 515);
      this.buttonRunLoop.Name = "buttonRunLoop";
      this.buttonRunLoop.Size = new Size(107, 23);
      this.buttonRunLoop.TabIndex = 21;
      this.buttonRunLoop.Text = "Run loop";
      this.buttonRunLoop.UseVisualStyleBackColor = true;
      this.buttonRunLoop.Click += new EventHandler(this.buttonRunLoop_Click);
      this.buttonBreakLoop.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonBreakLoop.Enabled = false;
      this.buttonBreakLoop.Location = new Point(706, 515);
      this.buttonBreakLoop.Name = "buttonBreakLoop";
      this.buttonBreakLoop.Size = new Size(107, 23);
      this.buttonBreakLoop.TabIndex = 21;
      this.buttonBreakLoop.Text = "BreakLoop";
      this.buttonBreakLoop.UseVisualStyleBackColor = true;
      this.buttonBreakLoop.Click += new EventHandler(this.buttonBreakLoop_Click);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 515);
      this.label1.Name = "label1";
      this.label1.Size = new Size(75, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Loop time [ms]";
      this.textBoxLoopTimeMs.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxLoopTimeMs.Location = new Point(95, 515);
      this.textBoxLoopTimeMs.Name = "textBoxLoopTimeMs";
      this.textBoxLoopTimeMs.Size = new Size(67, 20);
      this.textBoxLoopTimeMs.TabIndex = 23;
      this.textBoxLoopTimeMs.Text = "2000";
      this.textBoxLoopTimeMs.TextAlign = HorizontalAlignment.Right;
      this.buttonClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonClear.Location = new Point(593, 515);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new Size(107, 23);
      this.buttonClear.TabIndex = 21;
      this.buttonClear.Text = "Clear";
      this.buttonClear.UseVisualStyleBackColor = true;
      this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
      this.comboBoxListSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.comboBoxListSelect.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxListSelect.FormattingEnabled = true;
      this.comboBoxListSelect.Location = new Point(183, 515);
      this.comboBoxListSelect.Name = "comboBoxListSelect";
      this.comboBoxListSelect.Size = new Size(180, 21);
      this.comboBoxListSelect.TabIndex = 24;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1051, 552);
      this.Controls.Add((Control) this.comboBoxListSelect);
      this.Controls.Add((Control) this.textBoxLoopTimeMs);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.buttonClear);
      this.Controls.Add((Control) this.buttonBreakLoop);
      this.Controls.Add((Control) this.buttonRunLoop);
      this.Controls.Add((Control) this.buttonRun);
      this.Controls.Add((Control) this.textBoxOutData);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (TDC_Internals);
      this.Text = nameof (TDC_Internals);
      this.FormClosing += new FormClosingEventHandler(this.TDC_Internals_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum TdcInternalListSelection
    {
      standard,
      extendet1,
    }
  }
}
