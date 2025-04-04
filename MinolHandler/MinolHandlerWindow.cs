// Decompiled with JetBrains decompiler
// Type: MinolHandler.MinolHandlerWindow
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using CorporateDesign;
using GmmDbLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  public class MinolHandlerWindow : Form
  {
    private static Logger logger = LogManager.GetLogger(nameof (MinolHandlerWindow));
    private const string translaterBaseKey = "MinolHandlerWindow";
    private bool isCancel;
    private MinolHandlerFunctions MyFunctions;
    internal string StartComponentName = "";
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Panel panel1;
    private Button buttonWrite;
    private Button buttonRead;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem componentToolStripMenuItem;
    private ToolStripMenuItem globalMeterManagerToolStripMenuItem;
    private ToolStripMenuItem backToolStripMenuItem;
    private ToolStripMenuItem quitToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem ToolStripMenuItemSerialBus;
    private ToolStripMenuItem ToolStripMenuItemAsyncCom;
    private TextBox txtStatus;
    private GroupBox groupBoxReadLoop;
    private Button buttonLoop;
    private Button buttonBreak;
    private Label label4;
    private Label label3;
    private Label label2;
    private TextBox textBoxLoopWaitTime;
    private TextBox textBoxOkCounter;
    private TextBox textBoxErrorCounter;
    private Button buttonSaveToDatabase;
    private Button buttonLoadFromDatabase;
    private TextBox textBoxSerialNumber;
    private TextBox textBoxDeviceType;
    private Label label5;
    private Label label1;
    private Button buttonSaveTypeToDatabase;
    private Button buttonLoadTypeFromDatabase;
    private CheckBox checkBoxDailyAuotsave;
    private Button btnShowParameters;
    private CheckBox cboxEnableMinoConnectButton;
    private Button btnCreateBinaryMap;

    public MinolHandlerWindow(MinolHandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      this.MyFunctions.MyBus.OnMessage += new EventHandler<GMM_EventArgs>(this.OnDeviceCollectorMessage);
      this.MyFunctions.MyCom.OnAsyncComMessage += new EventHandler<GMM_EventArgs>(this.OnAsyncComMessage);
      this.isCancel = false;
      this.checkBoxDailyAuotsave.Checked = this.MyFunctions.DailySave;
      bool flag1 = UserManager.CheckPermission(UserRights.Rights.Developer);
      bool flag2 = UserManager.CheckPermission(UserRights.Rights.ProfessionalConfig);
      this.buttonWrite.Visible = flag1 | flag2;
      this.buttonLoadFromDatabase.Visible = flag1 | flag2;
      this.buttonSaveToDatabase.Visible = flag1 | flag2;
      this.buttonLoadTypeFromDatabase.Visible = flag1 | flag2;
      this.buttonSaveTypeToDatabase.Visible = flag1 | flag2;
      this.buttonBreak.Visible = flag1 | flag2;
      this.checkBoxDailyAuotsave.Visible = flag1 | flag2;
      this.groupBoxReadLoop.Visible = flag1 | flag2;
      FormTranslatorSupport.TranslateWindow(Tg.MinolHandlerWindow, (Form) this);
    }

    private void OnDeviceCollectorMessage(object sender, GMM_EventArgs e)
    {
      if (string.IsNullOrEmpty(e.EventMessage))
        return;
      if (e.TheMessageType == GMM_EventArgs.MessageType.SimpleMessage)
        this.txtStatus.Text = e.EventMessage;
      e.Cancel = this.isCancel;
    }

    private void OnAsyncComMessage(object sender, GMM_EventArgs e)
    {
      if (this.InvokeRequired)
      {
        try
        {
          this.BeginInvoke((Delegate) new EventHandler<GMM_EventArgs>(this.OnAsyncComMessage), sender, (object) e);
        }
        catch
        {
        }
      }
      else
      {
        e.Cancel = this.isCancel;
        if (e.TheMessageType != GMM_EventArgs.MessageType.KeyReceived || !this.MyFunctions.IsMinoConnectKeyEventEnabled)
          return;
        if (this.ReadDevice(false))
          SystemSounds.Beep.Play();
        else
          SystemSounds.Hand.Play();
      }
    }

    private void MinolHandlerWindow_Activated(object sender, EventArgs e)
    {
      this.StartComponentName = "";
    }

    private void CheckBoxDailyAuotsave_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions.DailySave = this.checkBoxDailyAuotsave.Checked;
    }

    private void DisableControls()
    {
      Cursor.Current = Cursors.WaitCursor;
      this.groupBoxReadLoop.Enabled = false;
      this.buttonBreak.Enabled = true;
      this.buttonRead.Enabled = false;
      this.buttonWrite.Enabled = false;
      this.Update();
      Application.DoEvents();
    }

    private void EnableControls()
    {
      Cursor.Current = Cursors.Default;
      this.groupBoxReadLoop.Enabled = true;
      this.buttonBreak.Enabled = false;
      this.buttonRead.Enabled = true;
      this.buttonWrite.Enabled = true;
    }

    private void buttonRead_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyBus.BreakRequest = false;
      this.ReadDevice(true);
      ZR_ClassLibMessages.ShowAndClearErrors("MinolHandler", (string) null);
    }

    private bool ReadDevice(bool showDialog)
    {
      if (this.MyFunctions.IsBusy)
      {
        MinolHandlerWindow.logger.Error("Abort an asynchronous method call! The MinolHandler is busy.");
        return false;
      }
      this.isCancel = false;
      this.txtStatus.BackColor = Color.WhiteSmoke;
      this.txtStatus.Text = string.Empty;
      try
      {
        ZR_ClassLibMessages.ClearErrors();
        this.DisableControls();
        GlobalDeviceId UpdatedDeviceIdentification;
        if (this.MyFunctions.ReadConfigurationParameters(out UpdatedDeviceIdentification, ReadMode.Complete))
        {
          if (this.MyFunctions.GetConfigurationParameters(ConfigurationParameter.ValueType.Complete) != null)
          {
            this.textBoxDeviceType.Text = UpdatedDeviceIdentification.DeviceTypeName;
            this.textBoxSerialNumber.Text = UpdatedDeviceIdentification.Serialnumber;
            this.txtStatus.BackColor = Color.LightGreen;
            this.txtStatus.Text = string.Format("{0} {1} {2}  {3}", (object) UpdatedDeviceIdentification.Manufacturer, (object) UpdatedDeviceIdentification.DeviceTypeName, (object) UpdatedDeviceIdentification.Generation, (object) UpdatedDeviceIdentification.Serialnumber);
            if (showDialog)
              ShowParameter.Show(this.MyFunctions.MyDevices.WorkDevice);
          }
          else
            this.txtStatus.BackColor = Color.LightPink;
        }
        else
          this.txtStatus.BackColor = Color.LightPink;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.ShowAndClearErrors("MinolHandler", ex.Message);
      }
      finally
      {
        if (showDialog)
          this.EnableControls();
      }
      return true;
    }

    private void buttonReadLoop_Click(object sender, EventArgs e)
    {
      this.DisableControls();
      this.isCancel = false;
      this.MyFunctions.MyBus.BreakRequest = false;
      try
      {
        while (!this.isCancel)
        {
          if (this.MyFunctions.ReadConfigurationParameters(out GlobalDeviceId _, ReadMode.Complete))
          {
            if (this.MyFunctions.GetConfigurationParameters(ConfigurationParameter.ValueType.Complete) != null)
              this.textBoxOkCounter.Text = (int.Parse(this.textBoxOkCounter.Text) + 1).ToString();
            else
              this.textBoxErrorCounter.Text = (int.Parse(this.textBoxErrorCounter.Text) + 1).ToString();
          }
          else
            this.textBoxErrorCounter.Text = (int.Parse(this.textBoxErrorCounter.Text) + 1).ToString();
          int result;
          if (int.TryParse(this.textBoxLoopWaitTime.Text, out result))
          {
            result *= 4;
            while (result-- > 0)
            {
              Application.DoEvents();
              if (this.buttonBreak.Enabled)
                Thread.Sleep(250);
              else
                break;
            }
          }
        }
      }
      catch
      {
      }
      finally
      {
        this.EnableControls();
      }
      this.Update();
    }

    private void buttonWrite_Click(object sender, EventArgs e)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Developer) && !UserManager.CheckPermission(UserRights.Rights.ProfessionalConfig))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to write configuration paramter!");
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
      {
        this.isCancel = false;
        Cursor.Current = Cursors.WaitCursor;
        this.panel1.Enabled = false;
        this.Update();
        Application.DoEvents();
        try
        {
          if (this.MyFunctions.WriteChangesToDevice())
          {
            int num = (int) MessageBox.Show("Device parameters successfully changed!");
            this.buttonWrite.Enabled = false;
          }
          ZR_ClassLibMessages.ShowAndClearErrors("MinolHandler", (string) null);
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.ShowAndClearErrors("MinolHandler", ex.Message);
        }
        finally
        {
          this.panel1.Enabled = true;
          Cursor.Current = Cursors.Default;
        }
      }
    }

    private void globalMeterManagerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "GMM";
      this.Hide();
    }

    private void backToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.Hide();
    }

    private void buttonBack_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "";
      this.Hide();
    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "Exit";
      this.Hide();
    }

    private void ToolStripMenuItemSerialBus_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "DeviceCollector";
      this.Hide();
    }

    private void ToolStripMenuItemAsyncCom_Click(object sender, EventArgs e)
    {
      this.StartComponentName = "AsyncCom";
      this.Hide();
    }

    private void buttonBreak_Click(object sender, EventArgs e)
    {
      this.buttonBreak.Enabled = false;
      this.isCancel = true;
    }

    private void buttonLoadFromDatabase_Click(object sender, EventArgs e)
    {
      COpenMeter copenMeter = new COpenMeter();
      copenMeter.DeviceFilter = "MinolDevice";
      string str = "%";
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.MyFunctions.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident);
      if (configurationParameters != null)
      {
        object parameterValue = configurationParameters[OverrideID.SerialNumber].ParameterValue;
        if (parameterValue != null)
          str = parameterValue.ToString();
      }
      copenMeter.m_sSerialNr = str;
      if (copenMeter.ShowDialog() != DialogResult.OK)
        return;
      if (this.MyFunctions.MyDevices.LoadFromDatabase(copenMeter.m_iMeterID, copenMeter.DataTimePoint))
      {
        this.txtStatus.BackColor = Color.White;
        this.txtStatus.Text = "";
        object parameterValue = this.MyFunctions.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident)[OverrideID.SerialNumber].ParameterValue;
        if (parameterValue != null)
          this.textBoxSerialNumber.Text = parameterValue.ToString();
        this.textBoxDeviceType.Text = this.MyFunctions.MyDevices.WorkDevice.DeviceType.ToString();
        ShowParameter.Show(this.MyFunctions.MyDevices.WorkDevice);
        this.buttonWrite.Enabled = true;
      }
      else
      {
        int num = (int) MessageBox.Show("Not found!");
      }
    }

    private void buttonSaveToDatabase_Click(object sender, EventArgs e)
    {
      DateTime backupTimePoint;
      if (this.MyFunctions.SaveToDatabase(out backupTimePoint))
      {
        int num1 = (int) MessageBox.Show("Successfully saved at " + backupTimePoint.ToShortTimeString());
      }
      else
      {
        int num2 = (int) MessageBox.Show("Failed!");
      }
    }

    private void buttonLoadTypeFromDatabase_Click(object sender, EventArgs e)
    {
      COpenTypeDlg copenTypeDlg = new COpenTypeDlg();
      copenTypeDlg.FilterByPPSArtikelNr = "MinolDevice";
      if (copenTypeDlg.ShowDialog() != DialogResult.OK)
        return;
      if (this.MyFunctions.LoadTypeFromDatabase(copenTypeDlg.m_nMeterInfoID))
      {
        this.txtStatus.BackColor = Color.White;
        this.txtStatus.Text = "";
        this.textBoxDeviceType.Text = copenTypeDlg.MeterTypeDescription;
        object parameterValue = this.MyFunctions.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident)[OverrideID.SerialNumber].ParameterValue;
        if (parameterValue != null)
          this.textBoxSerialNumber.Text = parameterValue.ToString();
        ShowParameter.Show(this.MyFunctions.MyDevices.WorkDevice);
      }
      else
      {
        int num = (int) MessageBox.Show("Not found!");
      }
    }

    private void buttonSaveTypeToDatabase_Click(object sender, EventArgs e)
    {
      COpenTypeDlg copenTypeDlg = new COpenTypeDlg();
      copenTypeDlg.FilterByPPSArtikelNr = "MinolDevice";
      if (copenTypeDlg.ShowDialog() != DialogResult.OK)
        return;
      if (this.MyFunctions.MyDatabaseAccess.SaveType(this.MyFunctions.MyDevices.WorkDevice, copenTypeDlg.m_nMeterTypeID))
      {
        this.txtStatus.BackColor = Color.White;
        this.txtStatus.Text = "";
        this.textBoxDeviceType.Text = copenTypeDlg.MeterTypeDescription;
        object parameterValue = this.MyFunctions.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident)[OverrideID.SerialNumber].ParameterValue;
        if (parameterValue != null)
          this.textBoxSerialNumber.Text = parameterValue.ToString();
        int num = (int) MessageBox.Show("OK!");
      }
      else
      {
        int num1 = (int) MessageBox.Show("Not OK!");
      }
    }

    private void btnShowParameters_Click(object sender, EventArgs e)
    {
      ShowParameter.Show(this.MyFunctions.MyDevices.WorkDevice);
    }

    private void cboxEnableMinoConnectButton_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions.IsMinoConnectKeyEventEnabled = this.cboxEnableMinoConnectButton.Checked;
    }

    private void btnCreateStaticClassOfMinolDeviceDataTable_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.FileName = "MinolDeviceData.cs";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        this.MyFunctions.MyDatabaseAccess.CreateStaticClassOfMinolDeviceDataTable(saveFileDialog.FileName);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MinolHandlerWindow));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.panel1 = new Panel();
      this.cboxEnableMinoConnectButton = new CheckBox();
      this.btnShowParameters = new Button();
      this.checkBoxDailyAuotsave = new CheckBox();
      this.textBoxSerialNumber = new TextBox();
      this.textBoxDeviceType = new TextBox();
      this.label5 = new Label();
      this.label1 = new Label();
      this.buttonSaveTypeToDatabase = new Button();
      this.buttonSaveToDatabase = new Button();
      this.buttonLoadTypeFromDatabase = new Button();
      this.buttonLoadFromDatabase = new Button();
      this.buttonBreak = new Button();
      this.groupBoxReadLoop = new GroupBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.textBoxLoopWaitTime = new TextBox();
      this.textBoxOkCounter = new TextBox();
      this.textBoxErrorCounter = new TextBox();
      this.buttonLoop = new Button();
      this.txtStatus = new TextBox();
      this.buttonWrite = new Button();
      this.buttonRead = new Button();
      this.menuStrip1 = new MenuStrip();
      this.componentToolStripMenuItem = new ToolStripMenuItem();
      this.globalMeterManagerToolStripMenuItem = new ToolStripMenuItem();
      this.backToolStripMenuItem = new ToolStripMenuItem();
      this.quitToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.ToolStripMenuItemSerialBus = new ToolStripMenuItem();
      this.ToolStripMenuItemAsyncCom = new ToolStripMenuItem();
      this.btnCreateBinaryMap = new Button();
      this.panel1.SuspendLayout();
      this.groupBoxReadLoop.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign1.Location = new Point(0, 24);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(614, 375);
      this.zennerCoroprateDesign1.TabIndex = 0;
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.btnCreateBinaryMap);
      this.panel1.Controls.Add((Control) this.cboxEnableMinoConnectButton);
      this.panel1.Controls.Add((Control) this.btnShowParameters);
      this.panel1.Controls.Add((Control) this.checkBoxDailyAuotsave);
      this.panel1.Controls.Add((Control) this.textBoxSerialNumber);
      this.panel1.Controls.Add((Control) this.textBoxDeviceType);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.buttonSaveTypeToDatabase);
      this.panel1.Controls.Add((Control) this.buttonSaveToDatabase);
      this.panel1.Controls.Add((Control) this.buttonLoadTypeFromDatabase);
      this.panel1.Controls.Add((Control) this.buttonLoadFromDatabase);
      this.panel1.Controls.Add((Control) this.buttonBreak);
      this.panel1.Controls.Add((Control) this.groupBoxReadLoop);
      this.panel1.Controls.Add((Control) this.txtStatus);
      this.panel1.Controls.Add((Control) this.buttonWrite);
      this.panel1.Controls.Add((Control) this.buttonRead);
      this.panel1.Location = new Point(0, 60);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(614, 339);
      this.panel1.TabIndex = 1;
      this.cboxEnableMinoConnectButton.AutoSize = true;
      this.cboxEnableMinoConnectButton.Location = new Point(12, 123);
      this.cboxEnableMinoConnectButton.Name = "cboxEnableMinoConnectButton";
      this.cboxEnableMinoConnectButton.Size = new Size(122, 17);
      this.cboxEnableMinoConnectButton.TabIndex = 13;
      this.cboxEnableMinoConnectButton.Text = "MinoConnect button";
      this.cboxEnableMinoConnectButton.UseVisualStyleBackColor = true;
      this.cboxEnableMinoConnectButton.CheckedChanged += new System.EventHandler(this.cboxEnableMinoConnectButton_CheckedChanged);
      this.btnShowParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnShowParameters.Location = new Point(464, 50);
      this.btnShowParameters.Name = "btnShowParameters";
      this.btnShowParameters.Size = new Size(138, 23);
      this.btnShowParameters.TabIndex = 12;
      this.btnShowParameters.Text = "Show parameters";
      this.btnShowParameters.UseVisualStyleBackColor = true;
      this.btnShowParameters.Click += new System.EventHandler(this.btnShowParameters_Click);
      this.checkBoxDailyAuotsave.AutoSize = true;
      this.checkBoxDailyAuotsave.Location = new Point(12, 146);
      this.checkBoxDailyAuotsave.Name = "checkBoxDailyAuotsave";
      this.checkBoxDailyAuotsave.Size = new Size(135, 17);
      this.checkBoxDailyAuotsave.TabIndex = 11;
      this.checkBoxDailyAuotsave.Text = "Daily autosave on read";
      this.checkBoxDailyAuotsave.UseVisualStyleBackColor = true;
      this.checkBoxDailyAuotsave.CheckedChanged += new System.EventHandler(this.CheckBoxDailyAuotsave_CheckedChanged);
      this.textBoxSerialNumber.Location = new Point(106, 47);
      this.textBoxSerialNumber.Name = "textBoxSerialNumber";
      this.textBoxSerialNumber.ReadOnly = true;
      this.textBoxSerialNumber.Size = new Size(185, 20);
      this.textBoxSerialNumber.TabIndex = 10;
      this.textBoxDeviceType.Location = new Point(106, 21);
      this.textBoxDeviceType.Name = "textBoxDeviceType";
      this.textBoxDeviceType.ReadOnly = true;
      this.textBoxDeviceType.Size = new Size(185, 20);
      this.textBoxDeviceType.TabIndex = 10;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(14, 50);
      this.label5.Name = "label5";
      this.label5.Size = new Size(71, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "Serial number";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(14, 21);
      this.label1.Name = "label1";
      this.label1.Size = new Size(64, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "Device type";
      this.buttonSaveTypeToDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSaveTypeToDatabase.Location = new Point(464, 252);
      this.buttonSaveTypeToDatabase.Name = "buttonSaveTypeToDatabase";
      this.buttonSaveTypeToDatabase.Size = new Size(137, 21);
      this.buttonSaveTypeToDatabase.TabIndex = 8;
      this.buttonSaveTypeToDatabase.Text = "Save type to database";
      this.buttonSaveTypeToDatabase.UseVisualStyleBackColor = true;
      this.buttonSaveTypeToDatabase.Click += new System.EventHandler(this.buttonSaveTypeToDatabase_Click);
      this.buttonSaveToDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSaveToDatabase.Location = new Point(464, 168);
      this.buttonSaveToDatabase.Name = "buttonSaveToDatabase";
      this.buttonSaveToDatabase.Size = new Size(137, 21);
      this.buttonSaveToDatabase.TabIndex = 8;
      this.buttonSaveToDatabase.Text = "Save to database";
      this.buttonSaveToDatabase.UseVisualStyleBackColor = true;
      this.buttonSaveToDatabase.Click += new System.EventHandler(this.buttonSaveToDatabase_Click);
      this.buttonLoadTypeFromDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonLoadTypeFromDatabase.Location = new Point(464, 225);
      this.buttonLoadTypeFromDatabase.Name = "buttonLoadTypeFromDatabase";
      this.buttonLoadTypeFromDatabase.Size = new Size(137, 21);
      this.buttonLoadTypeFromDatabase.TabIndex = 8;
      this.buttonLoadTypeFromDatabase.Text = "Load type from database";
      this.buttonLoadTypeFromDatabase.UseVisualStyleBackColor = true;
      this.buttonLoadTypeFromDatabase.Click += new System.EventHandler(this.buttonLoadTypeFromDatabase_Click);
      this.buttonLoadFromDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonLoadFromDatabase.Location = new Point(464, 141);
      this.buttonLoadFromDatabase.Name = "buttonLoadFromDatabase";
      this.buttonLoadFromDatabase.Size = new Size(137, 21);
      this.buttonLoadFromDatabase.TabIndex = 8;
      this.buttonLoadFromDatabase.Text = "Load from database";
      this.buttonLoadFromDatabase.UseVisualStyleBackColor = true;
      this.buttonLoadFromDatabase.Click += new System.EventHandler(this.buttonLoadFromDatabase_Click);
      this.buttonBreak.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonBreak.Enabled = false;
      this.buttonBreak.Location = new Point(464, 305);
      this.buttonBreak.Name = "buttonBreak";
      this.buttonBreak.Size = new Size(138, 23);
      this.buttonBreak.TabIndex = 7;
      this.buttonBreak.Text = "Break";
      this.buttonBreak.UseVisualStyleBackColor = true;
      this.buttonBreak.Click += new System.EventHandler(this.buttonBreak_Click);
      this.groupBoxReadLoop.Controls.Add((Control) this.label4);
      this.groupBoxReadLoop.Controls.Add((Control) this.label3);
      this.groupBoxReadLoop.Controls.Add((Control) this.label2);
      this.groupBoxReadLoop.Controls.Add((Control) this.textBoxLoopWaitTime);
      this.groupBoxReadLoop.Controls.Add((Control) this.textBoxOkCounter);
      this.groupBoxReadLoop.Controls.Add((Control) this.textBoxErrorCounter);
      this.groupBoxReadLoop.Controls.Add((Control) this.buttonLoop);
      this.groupBoxReadLoop.Location = new Point(12, 169);
      this.groupBoxReadLoop.Name = "groupBoxReadLoop";
      this.groupBoxReadLoop.Size = new Size(218, 132);
      this.groupBoxReadLoop.TabIndex = 5;
      this.groupBoxReadLoop.TabStop = false;
      this.groupBoxReadLoop.Text = "Read loop";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 22);
      this.label4.Name = "label4";
      this.label4.Size = new Size(65, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Wait time [s]";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 48);
      this.label3.Name = "label3";
      this.label3.Size = new Size(60, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Ok counter";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 74);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Error counter";
      this.textBoxLoopWaitTime.Location = new Point(94, 19);
      this.textBoxLoopWaitTime.Name = "textBoxLoopWaitTime";
      this.textBoxLoopWaitTime.Size = new Size(100, 20);
      this.textBoxLoopWaitTime.TabIndex = 1;
      this.textBoxLoopWaitTime.Text = "0";
      this.textBoxOkCounter.Location = new Point(94, 45);
      this.textBoxOkCounter.Name = "textBoxOkCounter";
      this.textBoxOkCounter.Size = new Size(100, 20);
      this.textBoxOkCounter.TabIndex = 1;
      this.textBoxOkCounter.Text = "0";
      this.textBoxErrorCounter.Location = new Point(94, 71);
      this.textBoxErrorCounter.Name = "textBoxErrorCounter";
      this.textBoxErrorCounter.Size = new Size(100, 20);
      this.textBoxErrorCounter.TabIndex = 1;
      this.textBoxErrorCounter.Text = "0";
      this.buttonLoop.Location = new Point(6, 97);
      this.buttonLoop.Name = "buttonLoop";
      this.buttonLoop.Size = new Size(188, 23);
      this.buttonLoop.TabIndex = 0;
      this.buttonLoop.Text = "Start";
      this.buttonLoop.UseVisualStyleBackColor = true;
      this.buttonLoop.Click += new System.EventHandler(this.buttonReadLoop_Click);
      this.txtStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStatus.BackColor = Color.WhiteSmoke;
      this.txtStatus.Location = new Point(12, 307);
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.ReadOnly = true;
      this.txtStatus.Size = new Size(446, 20);
      this.txtStatus.TabIndex = 4;
      this.buttonWrite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonWrite.Enabled = false;
      this.buttonWrite.Location = new Point(464, 81);
      this.buttonWrite.Name = "buttonWrite";
      this.buttonWrite.Size = new Size(138, 23);
      this.buttonWrite.TabIndex = 2;
      this.buttonWrite.Text = "Write to device";
      this.buttonWrite.UseVisualStyleBackColor = true;
      this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
      this.buttonRead.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonRead.Location = new Point(464, 21);
      this.buttonRead.Name = "buttonRead";
      this.buttonRead.Size = new Size(138, 23);
      this.buttonRead.TabIndex = 2;
      this.buttonRead.Text = "Read from device";
      this.buttonRead.UseVisualStyleBackColor = true;
      this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
      this.menuStrip1.BackColor = SystemColors.Control;
      this.menuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.componentToolStripMenuItem
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(614, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      this.componentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.globalMeterManagerToolStripMenuItem,
        (ToolStripItem) this.backToolStripMenuItem,
        (ToolStripItem) this.quitToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.ToolStripMenuItemSerialBus,
        (ToolStripItem) this.ToolStripMenuItemAsyncCom
      });
      this.componentToolStripMenuItem.Name = "componentToolStripMenuItem";
      this.componentToolStripMenuItem.Size = new Size(83, 20);
      this.componentToolStripMenuItem.Text = "Component";
      this.globalMeterManagerToolStripMenuItem.Name = "globalMeterManagerToolStripMenuItem";
      this.globalMeterManagerToolStripMenuItem.Size = new Size(186, 22);
      this.globalMeterManagerToolStripMenuItem.Text = "GlobalMeterManager";
      this.globalMeterManagerToolStripMenuItem.Click += new System.EventHandler(this.globalMeterManagerToolStripMenuItem_Click);
      this.backToolStripMenuItem.Name = "backToolStripMenuItem";
      this.backToolStripMenuItem.Size = new Size(186, 22);
      this.backToolStripMenuItem.Text = "Back";
      this.backToolStripMenuItem.Click += new System.EventHandler(this.backToolStripMenuItem_Click);
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new Size(186, 22);
      this.quitToolStripMenuItem.Text = "Quit";
      this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(183, 6);
      this.ToolStripMenuItemSerialBus.Name = "ToolStripMenuItemSerialBus";
      this.ToolStripMenuItemSerialBus.Size = new Size(186, 22);
      this.ToolStripMenuItemSerialBus.Text = "DeviceCollector";
      this.ToolStripMenuItemSerialBus.Click += new System.EventHandler(this.ToolStripMenuItemSerialBus_Click);
      this.ToolStripMenuItemAsyncCom.Name = "ToolStripMenuItemAsyncCom";
      this.ToolStripMenuItemAsyncCom.Size = new Size(186, 22);
      this.ToolStripMenuItemAsyncCom.Text = "AsyncCom";
      this.ToolStripMenuItemAsyncCom.Click += new System.EventHandler(this.ToolStripMenuItemAsyncCom_Click);
      this.btnCreateBinaryMap.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCreateBinaryMap.Location = new Point(305, 225);
      this.btnCreateBinaryMap.Name = "btnCreateBinaryMap";
      this.btnCreateBinaryMap.Size = new Size(137, 21);
      this.btnCreateBinaryMap.TabIndex = 14;
      this.btnCreateBinaryMap.Text = "Create Binary Map";
      this.btnCreateBinaryMap.UseVisualStyleBackColor = true;
      this.btnCreateBinaryMap.Click += new System.EventHandler(this.btnCreateStaticClassOfMinolDeviceDataTable_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(614, 399);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Controls.Add((Control) this.menuStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MinolHandlerWindow);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Minol Handler";
      this.Activated += new System.EventHandler(this.MinolHandlerWindow_Activated);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBoxReadLoop.ResumeLayout(false);
      this.groupBoxReadLoop.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
