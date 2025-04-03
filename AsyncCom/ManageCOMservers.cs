// Decompiled with JetBrains decompiler
// Type: AsyncCom.ManageCOMservers
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using CorporateDesign;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace AsyncCom
{
  public class ManageCOMservers : Form
  {
    private AsyncFunctions ComX;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Panel panel1;
    private Button buttonClose;
    private TextBox textBoxSerialNumber;
    private Label label2;
    private Button buttonAdd;
    private Button buttonRefresh;
    private Label label1;
    private TextBox textBoxPassword;
    private Label label3;
    private DataGridView dataGridViewCOMservers;
    private GroupBox groupBox1;
    private Button buttonDelete;
    private GroupBox groupBox2;
    private Label label4;
    private Label label5;
    private Button buttonModify;
    private TextBox textBoxNameModify;
    private TextBox textBoxSerialNumberModify;
    private TextBox textBoxName;
    private Label label6;

    public ManageCOMservers(AsyncFunctions ComX)
    {
      this.InitializeComponent();
      this.ComX = ComX;
      this.UpdateCOMserverList();
    }

    private void buttonClose_Click(object sender, EventArgs e) => this.Close();

    private void UpdateCOMserverList()
    {
      DataTable dataTable = new DataTable("COMservers");
      dataTable.Rows.Clear();
      dataTable.Columns.Add("Nr.", typeof (int));
      dataTable.Columns.Add("Name", typeof (string));
      dataTable.Columns.Add("Serialnumber", typeof (string));
      dataTable.Columns.Add("Online", typeof (string));
      dataTable.Columns.Add("Last Seen", typeof (string));
      dataTable.Columns.Add("Traffic", typeof (long));
      this.dataGridViewCOMservers.DataSource = (object) dataTable;
      this.dataGridViewCOMservers.Columns["Traffic"].HeaderText = "Traffic (In+Out, kBytes)";
      int num = 1;
      this.ComX.MyMeterVPN.Update((AsyncIP) this.ComX.MyComType);
      foreach (DictionaryEntry coMserver1 in this.ComX.MyMeterVPN.COMservers)
      {
        COMserver coMserver2 = (COMserver) coMserver1.Value;
        object[] objArray = new object[6]
        {
          (object) num.ToString(),
          (object) coMserver2.Name,
          (object) coMserver1.Key.ToString(),
          (object) coMserver2.Online,
          (object) coMserver2.LastSeen,
          (object) (Convert.ToInt64(coMserver2.Traffic) / 1024L)
        };
        dataTable.Rows.Add(objArray);
        ++num;
      }
      this.textBoxSerialNumberModify.Text = "";
      this.textBoxPassword.Text = "";
      this.textBoxSerialNumber.Text = "";
      this.textBoxNameModify.Text = "";
      this.textBoxName.Text = "";
    }

    private void buttonRefresh_Click(object sender, EventArgs e) => this.UpdateCOMserverList();

    private void buttonAdd_Click(object sender, EventArgs e)
    {
      if (this.ComX.MyMeterVPN.AddCOMserver((AsyncIP) this.ComX.MyComType, "COMserver" + this.textBoxSerialNumber.Text, this.textBoxName.Text, this.textBoxPassword.Text))
      {
        this.UpdateCOMserverList();
      }
      else
      {
        int num = (int) MessageBox.Show("Bitte überprüfen Sie die Seriennummer und das Passwort");
      }
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
      if (this.dataGridViewCOMservers.SelectedRows.Count != 1)
        return;
      if (this.ComX.MyMeterVPN.DelCOMserver((AsyncIP) this.ComX.MyComType, (string) this.dataGridViewCOMservers.SelectedCells[2].Value))
      {
        this.UpdateCOMserverList();
      }
      else
      {
        int num = (int) MessageBox.Show("COMserver konnte nicht gelöscht werden");
      }
    }

    private void buttonModify_Click(object sender, EventArgs e)
    {
      if (this.dataGridViewCOMservers.SelectedRows.Count != 1)
        return;
      if (this.ComX.MyMeterVPN.ModCOMserver((AsyncIP) this.ComX.MyComType, (string) this.dataGridViewCOMservers.SelectedCells[2].Value, this.textBoxNameModify.Text))
      {
        this.UpdateCOMserverList();
      }
      else
      {
        int num = (int) MessageBox.Show("COMserver konnte nicht gelöscht werden");
      }
    }

    private void dataGridViewCOMservers_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dataGridViewCOMservers.SelectedRows.Count != 1)
        return;
      this.textBoxSerialNumberModify.Text = (string) this.dataGridViewCOMservers.SelectedCells[2].Value;
      this.textBoxNameModify.Text = (string) this.dataGridViewCOMservers.SelectedCells[1].Value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ManageCOMservers));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.panel1 = new Panel();
      this.groupBox2 = new GroupBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.buttonModify = new Button();
      this.textBoxNameModify = new TextBox();
      this.textBoxSerialNumberModify = new TextBox();
      this.buttonDelete = new Button();
      this.groupBox1 = new GroupBox();
      this.label6 = new Label();
      this.textBoxName = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.buttonAdd = new Button();
      this.textBoxPassword = new TextBox();
      this.textBoxSerialNumber = new TextBox();
      this.dataGridViewCOMservers = new DataGridView();
      this.buttonRefresh = new Button();
      this.label1 = new Label();
      this.buttonClose = new Button();
      this.panel1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewCOMservers).BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.AccessibleDescription = (string) null;
      this.zennerCoroprateDesign1.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.BackgroundImage = (Image) null;
      this.zennerCoroprateDesign1.Font = (Font) null;
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.panel1.AccessibleDescription = (string) null;
      this.panel1.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.BackgroundImage = (Image) null;
      this.panel1.Controls.Add((Control) this.groupBox2);
      this.panel1.Controls.Add((Control) this.buttonDelete);
      this.panel1.Controls.Add((Control) this.groupBox1);
      this.panel1.Controls.Add((Control) this.dataGridViewCOMservers);
      this.panel1.Controls.Add((Control) this.buttonRefresh);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.buttonClose);
      this.panel1.Font = (Font) null;
      this.panel1.Name = "panel1";
      this.groupBox2.AccessibleDescription = (string) null;
      this.groupBox2.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.BackgroundImage = (Image) null;
      this.groupBox2.Controls.Add((Control) this.label4);
      this.groupBox2.Controls.Add((Control) this.label5);
      this.groupBox2.Controls.Add((Control) this.buttonModify);
      this.groupBox2.Controls.Add((Control) this.textBoxNameModify);
      this.groupBox2.Controls.Add((Control) this.textBoxSerialNumberModify);
      this.groupBox2.Font = (Font) null;
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      this.label4.AccessibleDescription = (string) null;
      this.label4.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Font = (Font) null;
      this.label4.Name = "label4";
      this.label5.AccessibleDescription = (string) null;
      this.label5.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Font = (Font) null;
      this.label5.Name = "label5";
      this.buttonModify.AccessibleDescription = (string) null;
      this.buttonModify.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.buttonModify, "buttonModify");
      this.buttonModify.BackgroundImage = (Image) null;
      this.buttonModify.Font = (Font) null;
      this.buttonModify.Name = "buttonModify";
      this.buttonModify.UseVisualStyleBackColor = true;
      this.buttonModify.Click += new EventHandler(this.buttonModify_Click);
      this.textBoxNameModify.AccessibleDescription = (string) null;
      this.textBoxNameModify.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.textBoxNameModify, "textBoxNameModify");
      this.textBoxNameModify.BackgroundImage = (Image) null;
      this.textBoxNameModify.Font = (Font) null;
      this.textBoxNameModify.Name = "textBoxNameModify";
      this.textBoxSerialNumberModify.AccessibleDescription = (string) null;
      this.textBoxSerialNumberModify.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.textBoxSerialNumberModify, "textBoxSerialNumberModify");
      this.textBoxSerialNumberModify.BackgroundImage = (Image) null;
      this.textBoxSerialNumberModify.Font = (Font) null;
      this.textBoxSerialNumberModify.Name = "textBoxSerialNumberModify";
      this.buttonDelete.AccessibleDescription = (string) null;
      this.buttonDelete.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.buttonDelete, "buttonDelete");
      this.buttonDelete.BackgroundImage = (Image) null;
      this.buttonDelete.Font = (Font) null;
      this.buttonDelete.Name = "buttonDelete";
      this.buttonDelete.UseVisualStyleBackColor = true;
      this.buttonDelete.Click += new EventHandler(this.buttonDelete_Click);
      this.groupBox1.AccessibleDescription = (string) null;
      this.groupBox1.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.BackgroundImage = (Image) null;
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.textBoxName);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.buttonAdd);
      this.groupBox1.Controls.Add((Control) this.textBoxPassword);
      this.groupBox1.Controls.Add((Control) this.textBoxSerialNumber);
      this.groupBox1.Font = (Font) null;
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      this.label6.AccessibleDescription = (string) null;
      this.label6.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Font = (Font) null;
      this.label6.Name = "label6";
      this.textBoxName.AccessibleDescription = (string) null;
      this.textBoxName.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.textBoxName, "textBoxName");
      this.textBoxName.BackgroundImage = (Image) null;
      this.textBoxName.Font = (Font) null;
      this.textBoxName.Name = "textBoxName";
      this.label3.AccessibleDescription = (string) null;
      this.label3.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Font = (Font) null;
      this.label3.Name = "label3";
      this.label2.AccessibleDescription = (string) null;
      this.label2.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Font = (Font) null;
      this.label2.Name = "label2";
      this.buttonAdd.AccessibleDescription = (string) null;
      this.buttonAdd.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.buttonAdd, "buttonAdd");
      this.buttonAdd.BackgroundImage = (Image) null;
      this.buttonAdd.Font = (Font) null;
      this.buttonAdd.Name = "buttonAdd";
      this.buttonAdd.UseVisualStyleBackColor = true;
      this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
      this.textBoxPassword.AccessibleDescription = (string) null;
      this.textBoxPassword.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.textBoxPassword, "textBoxPassword");
      this.textBoxPassword.BackgroundImage = (Image) null;
      this.textBoxPassword.Font = (Font) null;
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxSerialNumber.AccessibleDescription = (string) null;
      this.textBoxSerialNumber.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.textBoxSerialNumber, "textBoxSerialNumber");
      this.textBoxSerialNumber.BackgroundImage = (Image) null;
      this.textBoxSerialNumber.Font = (Font) null;
      this.textBoxSerialNumber.Name = "textBoxSerialNumber";
      this.dataGridViewCOMservers.AccessibleDescription = (string) null;
      this.dataGridViewCOMservers.AccessibleName = (string) null;
      this.dataGridViewCOMservers.AllowUserToAddRows = false;
      this.dataGridViewCOMservers.AllowUserToDeleteRows = false;
      this.dataGridViewCOMservers.AllowUserToOrderColumns = true;
      this.dataGridViewCOMservers.AllowUserToResizeColumns = false;
      this.dataGridViewCOMservers.AllowUserToResizeRows = false;
      componentResourceManager.ApplyResources((object) this.dataGridViewCOMservers, "dataGridViewCOMservers");
      this.dataGridViewCOMservers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewCOMservers.BackgroundImage = (Image) null;
      this.dataGridViewCOMservers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewCOMservers.Font = (Font) null;
      this.dataGridViewCOMservers.MultiSelect = false;
      this.dataGridViewCOMservers.Name = "dataGridViewCOMservers";
      this.dataGridViewCOMservers.ReadOnly = true;
      this.dataGridViewCOMservers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewCOMservers.CellClick += new DataGridViewCellEventHandler(this.dataGridViewCOMservers_CellClick);
      this.buttonRefresh.AccessibleDescription = (string) null;
      this.buttonRefresh.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.buttonRefresh, "buttonRefresh");
      this.buttonRefresh.BackgroundImage = (Image) null;
      this.buttonRefresh.Font = (Font) null;
      this.buttonRefresh.Name = "buttonRefresh";
      this.buttonRefresh.UseVisualStyleBackColor = true;
      this.buttonRefresh.Click += new EventHandler(this.buttonRefresh_Click);
      this.label1.AccessibleDescription = (string) null;
      this.label1.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Font = (Font) null;
      this.label1.Name = "label1";
      this.buttonClose.AccessibleDescription = (string) null;
      this.buttonClose.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.buttonClose, "buttonClose");
      this.buttonClose.BackgroundImage = (Image) null;
      this.buttonClose.Font = (Font) null;
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.UseVisualStyleBackColor = true;
      this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
      this.AccessibleDescription = (string) null;
      this.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) null;
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Font = (Font) null;
      this.Name = nameof (ManageCOMservers);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((ISupportInitialize) this.dataGridViewCOMservers).EndInit();
      this.ResumeLayout(false);
    }
  }
}
