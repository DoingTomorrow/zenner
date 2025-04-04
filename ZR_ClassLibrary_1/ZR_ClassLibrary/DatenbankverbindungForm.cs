// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.DatenbankverbindungForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using GmmDbLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class DatenbankverbindungForm : Form
  {
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;
    private Button OkBtn;
    private Button cancelBtn;
    private GroupBox groupBox1;
    private TextBox ProviderStrTextBox;
    private Label label2;
    private Label label3;
    private Button DateiSuchBtn;
    private Label label1;
    private RadioButton AccessDB_RadioButton;
    private TextBox UserTextBox;
    private TextBox FileNameTextBox;
    private Label FileNameLabel;
    private TextBox PasswordTextBox;
    private Label DataSourceLabel;
    private TextBox DataSourceTextBox;
    private Button buttonTestConnection;
    private OpenFileDialog openFileDialog1;
    private TextBox ServerTextBox;
    private TextBox PortTextBox;
    private Label ServerLabel;
    private Label PortLabel;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Label label5;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private RadioButton radioButtonDBISAM;
    private RadioButton radioButtonSQLite;
    private RadioButton radioButtonMSSQL;
    private RadioButton NPGSqlRadioButton;
    private bool initialising = false;
    private string ErrorMsg;
    private Datenbankverbindung myDatenbankverbindung;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DatenbankverbindungForm));
      this.OkBtn = new Button();
      this.cancelBtn = new Button();
      this.AccessDB_RadioButton = new RadioButton();
      this.groupBox1 = new GroupBox();
      this.radioButtonMSSQL = new RadioButton();
      this.radioButtonDBISAM = new RadioButton();
      this.radioButtonSQLite = new RadioButton();
      this.NPGSqlRadioButton = new RadioButton();
      this.ProviderStrTextBox = new TextBox();
      this.UserTextBox = new TextBox();
      this.FileNameTextBox = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.FileNameLabel = new Label();
      this.DateiSuchBtn = new Button();
      this.label1 = new Label();
      this.PasswordTextBox = new TextBox();
      this.DataSourceLabel = new Label();
      this.DataSourceTextBox = new TextBox();
      this.buttonTestConnection = new Button();
      this.openFileDialog1 = new OpenFileDialog();
      this.ServerLabel = new Label();
      this.ServerTextBox = new TextBox();
      this.PortLabel = new Label();
      this.PortTextBox = new TextBox();
      this.label5 = new Label();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.OkBtn, "OkBtn");
      this.OkBtn.BackColor = SystemColors.Control;
      this.OkBtn.DialogResult = DialogResult.OK;
      this.OkBtn.Name = "OkBtn";
      this.OkBtn.UseVisualStyleBackColor = false;
      this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
      componentResourceManager.ApplyResources((object) this.cancelBtn, "cancelBtn");
      this.cancelBtn.BackColor = SystemColors.Control;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.UseVisualStyleBackColor = false;
      this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
      this.AccessDB_RadioButton.Checked = true;
      componentResourceManager.ApplyResources((object) this.AccessDB_RadioButton, "AccessDB_RadioButton");
      this.AccessDB_RadioButton.Name = "AccessDB_RadioButton";
      this.AccessDB_RadioButton.TabStop = true;
      this.AccessDB_RadioButton.CheckedChanged += new System.EventHandler(this.AccessDB_RadioButton_CheckedChanged);
      this.groupBox1.BackColor = Color.Transparent;
      this.groupBox1.Controls.Add((Control) this.radioButtonMSSQL);
      this.groupBox1.Controls.Add((Control) this.radioButtonDBISAM);
      this.groupBox1.Controls.Add((Control) this.radioButtonSQLite);
      this.groupBox1.Controls.Add((Control) this.NPGSqlRadioButton);
      this.groupBox1.Controls.Add((Control) this.AccessDB_RadioButton);
      this.groupBox1.ForeColor = SystemColors.ControlText;
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      componentResourceManager.ApplyResources((object) this.radioButtonMSSQL, "radioButtonMSSQL");
      this.radioButtonMSSQL.Name = "radioButtonMSSQL";
      this.radioButtonMSSQL.CheckedChanged += new System.EventHandler(this.radioButtonMSSQL_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.radioButtonDBISAM, "radioButtonDBISAM");
      this.radioButtonDBISAM.Name = "radioButtonDBISAM";
      this.radioButtonDBISAM.CheckedChanged += new System.EventHandler(this.radioButtonDBISAM_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.radioButtonSQLite, "radioButtonSQLite");
      this.radioButtonSQLite.Name = "radioButtonSQLite";
      this.radioButtonSQLite.CheckedChanged += new System.EventHandler(this.radioButtonSQLite_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.NPGSqlRadioButton, "NPGSqlRadioButton");
      this.NPGSqlRadioButton.Name = "NPGSqlRadioButton";
      this.NPGSqlRadioButton.CheckedChanged += new System.EventHandler(this.NPGSqlRadioButton_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.ProviderStrTextBox, "ProviderStrTextBox");
      this.ProviderStrTextBox.BackColor = SystemColors.Window;
      this.ProviderStrTextBox.Name = "ProviderStrTextBox";
      componentResourceManager.ApplyResources((object) this.UserTextBox, "UserTextBox");
      this.UserTextBox.BackColor = SystemColors.Window;
      this.UserTextBox.Name = "UserTextBox";
      componentResourceManager.ApplyResources((object) this.FileNameTextBox, "FileNameTextBox");
      this.FileNameTextBox.BackColor = SystemColors.Window;
      this.FileNameTextBox.Name = "FileNameTextBox";
      this.label2.BackColor = Color.Transparent;
      this.label2.ForeColor = SystemColors.ControlText;
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.BackColor = Color.Transparent;
      this.label3.ForeColor = SystemColors.ControlText;
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.FileNameLabel, "FileNameLabel");
      this.FileNameLabel.BackColor = Color.Transparent;
      this.FileNameLabel.ForeColor = SystemColors.ControlText;
      this.FileNameLabel.Name = "FileNameLabel";
      componentResourceManager.ApplyResources((object) this.DateiSuchBtn, "DateiSuchBtn");
      this.DateiSuchBtn.BackColor = SystemColors.Control;
      this.DateiSuchBtn.Name = "DateiSuchBtn";
      this.DateiSuchBtn.UseVisualStyleBackColor = false;
      this.DateiSuchBtn.Click += new System.EventHandler(this.DateiSuchBtn_Click);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.BackColor = Color.Transparent;
      this.label1.ForeColor = SystemColors.ControlText;
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.PasswordTextBox, "PasswordTextBox");
      this.PasswordTextBox.BackColor = SystemColors.Window;
      this.PasswordTextBox.Name = "PasswordTextBox";
      componentResourceManager.ApplyResources((object) this.DataSourceLabel, "DataSourceLabel");
      this.DataSourceLabel.BackColor = Color.Transparent;
      this.DataSourceLabel.ForeColor = SystemColors.ControlText;
      this.DataSourceLabel.Name = "DataSourceLabel";
      componentResourceManager.ApplyResources((object) this.DataSourceTextBox, "DataSourceTextBox");
      this.DataSourceTextBox.BackColor = SystemColors.Window;
      this.DataSourceTextBox.Name = "DataSourceTextBox";
      componentResourceManager.ApplyResources((object) this.buttonTestConnection, "buttonTestConnection");
      this.buttonTestConnection.BackColor = SystemColors.Control;
      this.buttonTestConnection.Name = "buttonTestConnection";
      this.buttonTestConnection.UseVisualStyleBackColor = false;
      this.buttonTestConnection.Click += new System.EventHandler(this.buttonTestConnection_Click);
      componentResourceManager.ApplyResources((object) this.ServerLabel, "ServerLabel");
      this.ServerLabel.Name = "ServerLabel";
      componentResourceManager.ApplyResources((object) this.ServerTextBox, "ServerTextBox");
      this.ServerTextBox.BackColor = SystemColors.Window;
      this.ServerTextBox.Name = "ServerTextBox";
      componentResourceManager.ApplyResources((object) this.PortLabel, "PortLabel");
      this.PortLabel.Name = "PortLabel";
      componentResourceManager.ApplyResources((object) this.PortTextBox, "PortTextBox");
      this.PortTextBox.Name = "PortTextBox";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.BackColor = Color.Transparent;
      this.label5.ForeColor = SystemColors.ControlText;
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign2, "zennerCoroprateDesign2");
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.PortTextBox);
      this.Controls.Add((Control) this.DataSourceTextBox);
      this.Controls.Add((Control) this.PasswordTextBox);
      this.Controls.Add((Control) this.FileNameTextBox);
      this.Controls.Add((Control) this.UserTextBox);
      this.Controls.Add((Control) this.ProviderStrTextBox);
      this.Controls.Add((Control) this.ServerTextBox);
      this.Controls.Add((Control) this.PortLabel);
      this.Controls.Add((Control) this.ServerLabel);
      this.Controls.Add((Control) this.buttonTestConnection);
      this.Controls.Add((Control) this.DataSourceLabel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.DateiSuchBtn);
      this.Controls.Add((Control) this.FileNameLabel);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.OkBtn);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DatenbankverbindungForm);
      this.ShowInTaskbar = false;
      this.Load += new System.EventHandler(this.Datenbankverbindung_Load);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public DatenbankverbindungForm(Datenbankverbindung myDatenbankverbindung)
    {
      this.InitializeComponent();
      this.myDatenbankverbindung = myDatenbankverbindung;
      this.Text = "Database settings : " + this.myDatenbankverbindung.sDataBaseKey;
    }

    private void Datenbankverbindung_Load(object sender, EventArgs e) => this.initall();

    private void initall()
    {
      try
      {
        this.windowDataFromObject();
      }
      catch
      {
        int num = (int) MessageBox.Show("No info for the database connection found\r\nPlease go to Settings/Primary database and Settings/Secondary database!");
      }
    }

    private void addErrorText(string inErrorMsg)
    {
      this.ErrorMsg = this.ErrorMsg + inErrorMsg + "\r\n";
    }

    public string getErrorText()
    {
      string errorMsg = this.ErrorMsg;
      this.ErrorMsg = "";
      return errorMsg;
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
      this.windowDataToObject();
      this.myDatenbankverbindung.saveDBInfo();
      this.DialogResult = DialogResult.Yes;
      if (!this.IsHandleCreated)
        return;
      this.Close();
    }

    private void cancelBtn_Click(object sender, EventArgs e) => this.Close();

    private void showDBControls(MeterDbTypes inDataBaseType)
    {
      this.ServerLabel.Visible = false;
      this.ServerLabel.Text = "ServerPrivate";
      this.PortLabel.Visible = false;
      this.PortTextBox.Visible = false;
      this.ServerTextBox.Visible = false;
      this.FileNameLabel.Visible = false;
      this.FileNameTextBox.Visible = false;
      this.DateiSuchBtn.Visible = false;
      this.DataSourceLabel.Visible = false;
      this.DataSourceTextBox.Visible = false;
      this.UserTextBox.Visible = false;
      this.label3.Visible = false;
      this.label1.Visible = false;
      this.PasswordTextBox.Visible = false;
      switch (inDataBaseType)
      {
        case MeterDbTypes.Access:
          this.FileNameLabel.Visible = true;
          this.FileNameTextBox.Visible = true;
          this.DateiSuchBtn.Visible = true;
          this.FileNameLabel.Text = "AccessDB";
          this.PasswordTextBox.Visible = true;
          this.label1.Visible = true;
          break;
        case MeterDbTypes.NPGSQL:
          this.ServerLabel.Visible = true;
          this.PortLabel.Visible = true;
          this.ServerTextBox.Visible = true;
          this.PortTextBox.Visible = true;
          this.DataSourceLabel.Visible = true;
          this.DataSourceTextBox.Visible = true;
          this.UserTextBox.Visible = true;
          this.label3.Visible = true;
          this.label1.Visible = true;
          this.PasswordTextBox.Visible = true;
          break;
        case MeterDbTypes.SQLite:
          this.FileNameLabel.Visible = true;
          this.FileNameTextBox.Visible = true;
          this.DateiSuchBtn.Visible = true;
          this.FileNameLabel.Text = "SQLiteDB";
          this.PasswordTextBox.Visible = true;
          this.label1.Visible = true;
          break;
        case MeterDbTypes.DBISAM:
          this.ServerLabel.Visible = true;
          this.PortLabel.Visible = true;
          this.ServerTextBox.Visible = true;
          this.PortTextBox.Visible = true;
          this.DataSourceLabel.Visible = true;
          this.DataSourceTextBox.Visible = true;
          this.UserTextBox.Visible = true;
          this.label3.Visible = true;
          this.label1.Visible = true;
          this.PasswordTextBox.Visible = true;
          break;
        case MeterDbTypes.MSSQL:
          this.ServerLabel.Visible = true;
          this.PortLabel.Visible = false;
          this.ServerTextBox.Visible = true;
          this.PortTextBox.Visible = false;
          this.DataSourceLabel.Visible = true;
          this.DataSourceTextBox.Visible = true;
          this.UserTextBox.Visible = true;
          this.label3.Visible = true;
          this.label1.Visible = true;
          this.PasswordTextBox.Visible = true;
          break;
      }
    }

    private void buttonTestConnection_Click(object sender, EventArgs e)
    {
      try
      {
        this.windowDataToObject();
        DbBasis db;
        Datenbankverbindung.MainDBAccess = new MeterDBAccess(this.myDatenbankverbindung.ConnectionInfo, out db);
        db.BaseDbConnection.ConnectDatabase();
        int num = (int) GMM_MessageBox.ShowMessage("Db connection", "Connection is ok");
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Db connection", "Connection error" + ex.ToString(), true);
      }
    }

    private void DateiSuchBtn_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.FileNameTextBox.Text = this.openFileDialog1.FileName;
    }

    private void AccessDB_RadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialising || !this.AccessDB_RadioButton.Checked)
        return;
      this.dbTypeFromButtons();
      this.RadioButtonChanged();
    }

    private void NPGSqlRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialising || !this.NPGSqlRadioButton.Checked)
        return;
      this.dbTypeFromButtons();
      this.ServerLabel.Text = "NPGSQL-Source";
      this.PortTextBox.Text = "5432";
      this.ServerTextBox.Text = "172.16.3.250";
      this.DataSourceTextBox.Text = "meterdb";
      this.RadioButtonChanged();
    }

    private void radioButtonSQLite_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialising || !this.radioButtonSQLite.Checked)
        return;
      this.dbTypeFromButtons();
      this.RadioButtonChanged();
    }

    private void radioButtonDBISAM_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialising || !this.radioButtonDBISAM.Checked)
        return;
      this.dbTypeFromButtons();
      this.ServerLabel.Text = "DBISAM-Source";
      this.PortTextBox.Text = "10005";
      this.ServerTextBox.Text = "10.40.22.6";
      this.DataSourceTextBox.Text = "Hydraulik";
      this.RadioButtonChanged();
    }

    private void radioButtonMSSQL_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialising || !this.radioButtonMSSQL.Checked)
        return;
      this.dbTypeFromButtons();
      this.ServerLabel.Text = "MSSQL-Source";
      this.PortTextBox.Text = "";
      this.ServerTextBox.Text = "srv-sql-03.minol.org";
      this.DataSourceTextBox.Text = "MeterDB";
      this.RadioButtonChanged();
    }

    private void windowDataToObject()
    {
      this.dbTypeFromButtons();
      this.myDatenbankverbindung.ConnectionInfo.ConnectionString = string.Empty;
      if (this.radioButtonSQLite.Checked || this.AccessDB_RadioButton.Checked)
      {
        this.myDatenbankverbindung.ConnectionInfo.UrlOrPath = this.FileNameTextBox.Text;
      }
      else
      {
        this.myDatenbankverbindung.ConnectionInfo.UserName = this.UserTextBox.Text;
        this.myDatenbankverbindung.ConnectionInfo.DatabaseName = this.DataSourceTextBox.Text;
        this.myDatenbankverbindung.ConnectionInfo.UrlOrPath = this.ServerTextBox.Text;
        this.myDatenbankverbindung.ConnectionInfo.Password = this.PasswordTextBox.Text;
      }
    }

    private void windowDataFromObject()
    {
      this.initialising = true;
      if (this.myDatenbankverbindung.ConnectionInfo.DbType == MeterDbTypes.Access)
        this.AccessDB_RadioButton.Checked = true;
      else if (this.myDatenbankverbindung.ConnectionInfo.DbType == MeterDbTypes.NPGSQL)
        this.NPGSqlRadioButton.Checked = true;
      else if (this.myDatenbankverbindung.ConnectionInfo.DbType == MeterDbTypes.SQLite)
        this.radioButtonSQLite.Checked = true;
      else if (this.myDatenbankverbindung.ConnectionInfo.DbType == MeterDbTypes.DBISAM)
        this.radioButtonDBISAM.Checked = true;
      else if (this.myDatenbankverbindung.ConnectionInfo.DbType == MeterDbTypes.MSSQL)
        this.radioButtonMSSQL.Checked = true;
      if (this.radioButtonSQLite.Checked || this.AccessDB_RadioButton.Checked)
      {
        this.FileNameTextBox.Text = this.myDatenbankverbindung.ConnectionInfo.UrlOrPath;
      }
      else
      {
        this.UserTextBox.Text = this.myDatenbankverbindung.ConnectionInfo.UserName;
        this.DataSourceTextBox.Text = this.myDatenbankverbindung.ConnectionInfo.DatabaseName;
        this.ServerTextBox.Text = this.myDatenbankverbindung.ConnectionInfo.UrlOrPath;
        this.PasswordTextBox.Text = this.myDatenbankverbindung.ConnectionInfo.Password;
      }
      this.showDBControls(this.myDatenbankverbindung.ConnectionInfo.DbType);
      this.initialising = false;
    }

    private void dbTypeFromButtons()
    {
      if (this.AccessDB_RadioButton.Checked)
        this.myDatenbankverbindung.ConnectionInfo.DbType = MeterDbTypes.Access;
      else if (this.NPGSqlRadioButton.Checked)
        this.myDatenbankverbindung.ConnectionInfo.DbType = MeterDbTypes.NPGSQL;
      else if (this.radioButtonSQLite.Checked)
        this.myDatenbankverbindung.ConnectionInfo.DbType = MeterDbTypes.SQLite;
      else if (this.radioButtonDBISAM.Checked)
      {
        this.myDatenbankverbindung.ConnectionInfo.DbType = MeterDbTypes.DBISAM;
      }
      else
      {
        if (!this.radioButtonMSSQL.Checked)
          return;
        this.myDatenbankverbindung.ConnectionInfo.DbType = MeterDbTypes.MSSQL;
      }
    }

    private void RadioButtonChanged()
    {
      this.UserTextBox.Text = "";
      this.windowDataToObject();
      this.windowDataFromObject();
      this.Refresh();
    }
  }
}
