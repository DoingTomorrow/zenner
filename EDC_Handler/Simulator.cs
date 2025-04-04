// Decompiled with JetBrains decompiler
// Type: EDC_Handler.Simulator
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using NLog;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EDC_Handler
{
  public class Simulator : Form
  {
    private static Logger logger = LogManager.GetLogger("LoggerSimulator");
    private EDC_HandlerFunctions handler;
    private bool isStopped;
    private const string LOGGER_TEST = "Test logger (RAM + FLASH)";
    private const string READ_WRITE_TEST = "Test Read & Write (FLASH & RAM) + Backup + Reset";
    private const string READ_WRITE_WITHOUT_BACKUP_RESET_TEST = "Test Read & Write (FLASH & RAM) without backup and reset";
    private const string READ_WRITE_WITHOUT_RESET_TEST = "Test Read & Write (FLASH & RAM) without reset";
    private const string READ_WRITE_FLASH_TEST = "Test Read & Write (FLASH)";
    private const string READ_WRITE_RAM_TEST = "Test Read & Write (RAM)";
    private const string RESET_LOOP_TEST = "Test Reset & Read device";
    private const string BACKUP_RESET_LOOP_TEST = "Test Backup & Reset & Read device";
    private const string ERASE_FLASH_TEST = "Test Erase FLASH";
    private const string CRC_TEST = "Test CRC FLASH";
    private IContainer components = (IContainer) null;
    private Button btnExecute;
    private TextBox txtStatus;
    private Button btnStop;
    private ComboBox cboxTestList;

    public Simulator() => this.InitializeComponent();

    private void Simulator_Load(object sender, EventArgs e)
    {
      this.cboxTestList.Items.Clear();
      this.cboxTestList.Items.Add((object) "Test CRC FLASH");
      this.cboxTestList.Items.Add((object) "Test logger (RAM + FLASH)");
      this.cboxTestList.Items.Add((object) "Test Read & Write (FLASH & RAM) + Backup + Reset");
      this.cboxTestList.Items.Add((object) "Test Read & Write (FLASH & RAM) without backup and reset");
      this.cboxTestList.Items.Add((object) "Test Read & Write (FLASH & RAM) without reset");
      this.cboxTestList.Items.Add((object) "Test Read & Write (FLASH)");
      this.cboxTestList.Items.Add((object) "Test Read & Write (RAM)");
      this.cboxTestList.Items.Add((object) "Test Reset & Read device");
      this.cboxTestList.Items.Add((object) "Test Backup & Reset & Read device");
      this.cboxTestList.Items.Add((object) "Test Erase FLASH");
      this.cboxTestList.SelectedIndex = 0;
    }

    internal static void ShowDialog(Form owner, EDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (Simulator simulator = new Simulator())
      {
        simulator.handler = MyFunctions;
        int num = (int) simulator.ShowDialog((IWin32Window) owner);
      }
    }

    private void btnExecute_Click(object sender, EventArgs e)
    {
      if (this.cboxTestList.SelectedItem == null || this.cboxTestList.SelectedItem.ToString() == string.Empty)
        return;
      this.cboxTestList.Enabled = false;
      this.btnExecute.Enabled = false;
      this.btnStop.Enabled = true;
      this.isStopped = false;
      try
      {
        string str = this.cboxTestList.SelectedItem.ToString();
        if (str == null)
          return;
        switch (str.Length)
        {
          case 14:
            if (!(str == "Test CRC FLASH"))
              break;
            this.TestCRC();
            break;
          case 16:
            if (!(str == "Test Erase FLASH"))
              break;
            this.TestEraseFLASH();
            break;
          case 23:
            if (!(str == "Test Read & Write (RAM)"))
              break;
            this.TestReadWriteRAM();
            break;
          case 24:
            if (!(str == "Test Reset & Read device"))
              break;
            this.TestResetDevice();
            break;
          case 25:
            switch (str[5])
            {
              case 'R':
                if (!(str == "Test Read & Write (FLASH)"))
                  return;
                this.TestReadWriteFLASH();
                return;
              case 'l':
                if (!(str == "Test logger (RAM + FLASH)"))
                  return;
                this.TestLogger();
                return;
              default:
                return;
            }
          case 33:
            if (!(str == "Test Backup & Reset & Read device"))
              break;
            this.TestBackupResetDevice();
            break;
          case 45:
            if (!(str == "Test Read & Write (FLASH & RAM) without reset"))
              break;
            this.TestReadWriteWithoutReset();
            break;
          case 48:
            if (!(str == "Test Read & Write (FLASH & RAM) + Backup + Reset"))
              break;
            this.TestReadWrite();
            break;
          case 56:
            if (!(str == "Test Read & Write (FLASH & RAM) without backup and reset"))
              break;
            this.TestReadWriteWithoutBackupAndReset();
            break;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Simulation failed! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.btnExecute.Enabled = true;
        this.btnStop.Enabled = false;
        this.cboxTestList.Enabled = true;
      }
    }

    private void btnStop_Click(object sender, EventArgs e) => this.isStopped = true;

    private void TestLogger()
    {
      uint num = 0;
      DateTime dateTime1 = new DateTime(2014, 1, 1, 23, 59, 58);
      DateTime dateTime2 = dateTime1.AddYears(3).AddMonths(1);
      string newLine = Environment.NewLine;
      if (!this.handler.LogClearAndDisableLog())
        return;
      this.txtStatus.AppendText("LogClearAndDisableLog" + newLine);
      if (!this.handler.LogEnable())
        return;
      this.txtStatus.AppendText("LogEnable" + newLine);
      while (dateTime1 < dateTime2 && !this.isStopped)
      {
        this.handler.WriteMeterValue(num);
        this.handler.WriteSystemTime(dateTime1);
        Thread.Sleep(2000);
        this.txtStatus.AppendText("Read system time: " + this.handler.ReadSystemTime().Value.ToString("G") + newLine);
        this.txtStatus.AppendText("Read meter value: " + this.handler.ReadMeterValue().ToString() + newLine);
        dateTime1 = dateTime1.AddMonths(1);
        ++num;
      }
      this.txtStatus.AppendText(" Done! Please check the logger data manually." + newLine);
    }

    private void TestReadWrite()
    {
      string newLine = Environment.NewLine;
      DateTime dateTime1 = new DateTime(2014, 1, 1);
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num1 = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num1 & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      uint num2 = 1;
      while (!this.isStopped && num2 < 1000U)
      {
        DeviceIdentification deviceIdentification = this.handler.Meter.GetDeviceIdentification();
        deviceIdentification.MeterID = num2;
        if (!this.handler.Meter.SetDeviceIdentification(deviceIdentification))
          break;
        this.txtStatus.AppendText("Set MeterID: " + num2.ToString() + newLine);
        if (!this.handler.Meter.SetBatteryEndDate(dateTime1))
          break;
        this.txtStatus.AppendText("Set BatteryEndDate: " + dateTime1.ToShortDateString() + newLine);
        if (!this.handler.WriteDevice(true, true))
          break;
        this.txtStatus.AppendText("Write device " + newLine);
        if (!this.handler.ReadDevice())
          break;
        this.txtStatus.AppendText("Read device " + newLine);
        uint meterId = this.handler.Meter.GetDeviceIdentification().MeterID;
        DateTime dateTime2 = this.handler.Meter.GetBatteryEndDate().Value;
        if ((int) meterId != (int) num2)
          throw new Exception("Can not change the MeterID!");
        if (dateTime2 != dateTime1)
          throw new Exception("Can not change the BatteryEndDate!");
        if (this.handler.Meter.GetSerialnumberPrimary().Value == 76U)
          throw new Exception("FATAL ERROR: The device reset all values! The cfg_serial_primary == 76");
        ++num2;
        dateTime1 = dateTime1.AddDays(1.0);
      }
    }

    private void TestReadWriteFLASH()
    {
      string newLine = Environment.NewLine;
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      for (uint index = 1; !this.isStopped && index < 1000U; ++index)
      {
        DeviceIdentification deviceIdentification = this.handler.Meter.GetDeviceIdentification();
        deviceIdentification.MeterID = index;
        if (!this.handler.Meter.SetDeviceIdentification(deviceIdentification))
          break;
        this.txtStatus.AppendText("Set MeterID: " + index.ToString() + newLine);
        if (!this.handler.WriteDevice())
          break;
        this.txtStatus.AppendText("Write device " + newLine);
        if (!this.handler.ReadDevice())
          break;
        this.txtStatus.AppendText("Read device " + newLine);
        if ((int) this.handler.Meter.GetDeviceIdentification().MeterID != (int) index)
          throw new Exception("Can not change the MeterID!");
        serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
        if (serialnumberPrimary.Value == 76U)
          throw new Exception("FATAL ERROR: The device reset all values! The cfg_serial_primary == 76");
      }
    }

    private void TestReadWriteRAM()
    {
      string newLine = Environment.NewLine;
      DateTime dateTime = new DateTime(2014, 1, 1);
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      for (uint index = 1; !this.isStopped && index < 1000U && this.handler.Meter.SetBatteryEndDate(dateTime); dateTime = dateTime.AddDays(1.0))
      {
        this.txtStatus.AppendText("Set BatteryEndDate: " + dateTime.ToShortDateString() + newLine);
        if (!this.handler.WriteDevice())
          break;
        this.txtStatus.AppendText("Write device " + newLine);
        if (!this.handler.ReadDevice())
          break;
        this.txtStatus.AppendText("Read device " + newLine);
        if (this.handler.Meter.GetBatteryEndDate().Value != dateTime)
          throw new Exception("Can not change the BatteryEndDate!");
        if (this.handler.Meter.GetSerialnumberPrimary().Value == 76U)
          throw new Exception("FATAL ERROR: The device reset all values! The cfg_serial_primary == 76");
        ++index;
      }
    }

    private void TestReadWriteWithoutBackupAndReset()
    {
      string newLine = Environment.NewLine;
      DateTime dateTime1 = new DateTime(2014, 1, 1);
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num1 = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num1 & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      uint num2 = 1;
      while (!this.isStopped && num2 < 1000U)
      {
        DeviceIdentification deviceIdentification = this.handler.Meter.GetDeviceIdentification();
        deviceIdentification.MeterID = num2;
        if (!this.handler.Meter.SetDeviceIdentification(deviceIdentification))
          break;
        this.txtStatus.AppendText("Set MeterID: " + num2.ToString() + newLine);
        if (!this.handler.Meter.SetBatteryEndDate(dateTime1))
          break;
        this.txtStatus.AppendText("Set BatteryEndDate: " + dateTime1.ToShortDateString() + newLine);
        if (!this.handler.WriteDevice(false, false))
          break;
        this.txtStatus.AppendText("Write device " + newLine);
        if (!this.handler.ReadDevice())
          break;
        this.txtStatus.AppendText("Read device " + newLine);
        uint meterId = this.handler.Meter.GetDeviceIdentification().MeterID;
        DateTime dateTime2 = this.handler.Meter.GetBatteryEndDate().Value;
        if ((int) meterId != (int) num2)
          throw new Exception("Can not change the MeterID!");
        if (dateTime2 != dateTime1)
          throw new Exception("Can not change the BatteryEndDate!");
        if (this.handler.Meter.GetSerialnumberPrimary().Value == 76U)
          throw new Exception("FATAL ERROR: The device reset all values! The cfg_serial_primary == 76");
        ++num2;
        dateTime1 = dateTime1.AddDays(1.0);
      }
    }

    private void TestReadWriteWithoutReset()
    {
      string newLine = Environment.NewLine;
      DateTime dateTime1 = new DateTime(2014, 1, 1);
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num1 = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num1 & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      uint num2 = 1;
      while (!this.isStopped && num2 < 1000U)
      {
        DeviceIdentification deviceIdentification = this.handler.Meter.GetDeviceIdentification();
        deviceIdentification.MeterID = num2;
        if (!this.handler.Meter.SetDeviceIdentification(deviceIdentification))
          break;
        this.txtStatus.AppendText("Set MeterID: " + num2.ToString() + newLine);
        if (!this.handler.Meter.SetBatteryEndDate(dateTime1))
          break;
        this.txtStatus.AppendText("Set BatteryEndDate: " + dateTime1.ToShortDateString() + newLine);
        if (!this.handler.WriteDevice(true, false))
          break;
        this.txtStatus.AppendText("Write device " + newLine);
        if (!this.handler.ReadDevice())
          break;
        this.txtStatus.AppendText("Read device " + newLine);
        uint meterId = this.handler.Meter.GetDeviceIdentification().MeterID;
        DateTime dateTime2 = this.handler.Meter.GetBatteryEndDate().Value;
        if ((int) meterId != (int) num2)
          throw new Exception("Can not change the MeterID!");
        if (dateTime2 != dateTime1)
          throw new Exception("Can not change the BatteryEndDate!");
        if (this.handler.Meter.GetSerialnumberPrimary().Value == 76U)
          throw new Exception("FATAL ERROR: The device reset all values! The cfg_serial_primary == 76");
        ++num2;
        dateTime1 = dateTime1.AddDays(1.0);
      }
    }

    private void TestResetDevice()
    {
      string newLine = Environment.NewLine;
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      for (int index = 1; index <= 1000 && !this.isStopped; ++index)
      {
        if (!this.handler.ResetDevice())
        {
          this.txtStatus.AppendText("Failed ResetDevice " + newLine);
          break;
        }
        this.txtStatus.AppendText(index.ToString() + " ResetDevice ");
        if (!this.handler.ReadDevice())
          break;
        this.txtStatus.AppendText("ReadDevice ");
        serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
        if (serialnumberPrimary.Value == 76U)
          throw new Exception("FATAL ERROR: The device reset all values! The cfg_serial_primary == 76");
        this.txtStatus.AppendText("OK" + newLine);
      }
    }

    private void TestBackupResetDevice()
    {
      string newLine = Environment.NewLine;
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      for (int index = 1; index <= 1000 && !this.isStopped; ++index)
      {
        if (!this.handler.RunRAMBackup())
        {
          this.txtStatus.AppendText("Failed RunRAMBackup " + newLine);
          break;
        }
        this.txtStatus.AppendText(index.ToString() + " RunRAMBackup ");
        if (!this.handler.ResetDevice())
        {
          this.txtStatus.AppendText("Failed ResetDevice " + newLine);
          break;
        }
        this.txtStatus.AppendText("ResetDevice ");
        if (!this.handler.ReadDevice())
          break;
        this.txtStatus.AppendText("ReadDevice ");
        serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
        if (serialnumberPrimary.Value == 76U)
          throw new Exception("FATAL ERROR: The device reset all values! The cfg_serial_primary == 76");
        this.txtStatus.AppendText("OK" + newLine);
      }
    }

    private void TestEraseFLASH()
    {
      string newLine = Environment.NewLine;
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      for (int index = 1; index <= 1000 && !this.isStopped; ++index)
      {
        if (!this.handler.MyDeviceCollector.EDCHandler.EraseFLASHSegment((ushort) 6528))
        {
          this.txtStatus.AppendText("Failed EraseFLASHSegment(0x1980) " + newLine);
          break;
        }
        this.txtStatus.AppendText(index.ToString() + " EraseFLASHSegment(0x1980) ");
        if (!this.handler.ReadDevice())
          break;
        this.txtStatus.AppendText("ReadDevice ");
        serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
        if (serialnumberPrimary.Value == 76U)
          throw new Exception("FATAL ERROR: The device reset all values! The cfg_serial_primary == 76");
        this.txtStatus.AppendText("OK" + newLine);
      }
    }

    private void TestCRC()
    {
      string newLine = Environment.NewLine;
      if (!this.handler.ReadDevice(false))
        return;
      uint? serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
      uint num1 = 76;
      if ((int) serialnumberPrimary.GetValueOrDefault() == (int) num1 & serialnumberPrimary.HasValue)
        throw new ArgumentException("This test can not be run with cfg_serial_primary = 76!");
      for (int index = 1; index <= 1000 && !this.isStopped; ++index)
      {
        if (!this.handler.ReadDevice())
        {
          this.txtStatus.AppendText("Failed ReadDevice " + newLine);
          break;
        }
        if (!this.handler.WorkMeter.SetSerialnumberPrimary((uint) index))
        {
          this.txtStatus.AppendText("Failed SetSerialnumberPrimary " + newLine);
          break;
        }
        if (!this.handler.WriteDevice())
        {
          this.txtStatus.AppendText("Failed WriteDevice " + newLine);
          break;
        }
        this.txtStatus.AppendText("WriteSerialnumber(" + index.ToString() + ") ");
        if (!this.handler.ReadDevice())
        {
          this.txtStatus.AppendText("Failed ReadDevice " + newLine);
          break;
        }
        serialnumberPrimary = this.handler.Meter.GetSerialnumberPrimary();
        uint num2 = serialnumberPrimary.Value;
        if ((long) num2 != (long) index)
        {
          this.txtStatus.AppendText("Invalid Serialnumber: " + num2.ToString() + newLine);
          break;
        }
        ushort parameterValue = this.handler.Meter.GetParameterValue<ushort>("cfg_crc");
        this.txtStatus.AppendText("CRC(" + parameterValue.ToString() + ") " + newLine);
        if (parameterValue == ushort.MaxValue)
        {
          this.txtStatus.AppendText("Broken " + newLine);
          break;
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Simulator));
      this.btnExecute = new Button();
      this.txtStatus = new TextBox();
      this.btnStop = new Button();
      this.cboxTestList = new ComboBox();
      this.SuspendLayout();
      this.btnExecute.Location = new Point(12, 12);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new Size(75, 23);
      this.btnExecute.TabIndex = 0;
      this.btnExecute.Text = "Execute";
      this.btnExecute.UseVisualStyleBackColor = true;
      this.btnExecute.Click += new EventHandler(this.btnExecute_Click);
      this.txtStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStatus.Location = new Point(12, 41);
      this.txtStatus.Multiline = true;
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.ScrollBars = ScrollBars.Vertical;
      this.txtStatus.Size = new Size(535, 430);
      this.txtStatus.TabIndex = 1;
      this.btnStop.Enabled = false;
      this.btnStop.Location = new Point(93, 12);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(75, 23);
      this.btnStop.TabIndex = 2;
      this.btnStop.Text = "Stop";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.cboxTestList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboxTestList.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxTestList.FormattingEnabled = true;
      this.cboxTestList.Location = new Point(195, 13);
      this.cboxTestList.Name = "cboxTestList";
      this.cboxTestList.Size = new Size(352, 21);
      this.cboxTestList.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(559, 483);
      this.Controls.Add((Control) this.cboxTestList);
      this.Controls.Add((Control) this.btnStop);
      this.Controls.Add((Control) this.txtStatus);
      this.Controls.Add((Control) this.btnExecute);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Simulator);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (Simulator);
      this.Load += new EventHandler(this.Simulator_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
