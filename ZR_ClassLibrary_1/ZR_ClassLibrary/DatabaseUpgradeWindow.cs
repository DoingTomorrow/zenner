// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.DatabaseUpgradeWindow
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class DatabaseUpgradeWindow : Form
  {
    private DatabaseUpgradeManager manager;
    private IContainer components = (IContainer) null;
    private Button btnStart;
    private Button btnCancel;
    private Panel panel1;
    private Label label2;
    private Label label1;
    private ListViewDoubledBuffered listView;
    private ColumnHeader colAction;
    private ColumnHeader colStatus;
    private Label label3;
    private Label label4;
    private Label lblOldDatabaseVersion;
    private Label lblNewDatabaseVersion;

    public DatabaseUpgradeWindow(Form owner, DatabaseUpgradeManager manager)
    {
      this.Owner = owner;
      this.InitializeComponent();
      this.manager = manager;
      this.manager.OnActionStateChanged += new EventHandler<UpgradeActionEventArgs>(this.Manager_OnActionStateChanged);
    }

    private void DatabaseUpgradeWindow_Load(object sender, EventArgs e)
    {
      this.lblOldDatabaseVersion.Text = string.Format("{0} ({1})", (object) this.manager.OldDatabaseVersion, (object) this.manager.OldDatabaseVersionDate);
      this.lblNewDatabaseVersion.Text = string.Format("{0} ({1})", (object) this.manager.NewDatabaseVersion, (object) this.manager.NewDatabaseVersionDate);
      List<UpgradeAction> ofUpgradeActions = this.manager.GetListOfUpgradeActions();
      if (ofUpgradeActions != null)
      {
        this.listView.Items.Clear();
        foreach (UpgradeAction upgradeAction in ofUpgradeActions)
          this.listView.Items.Add(new ListViewItem(upgradeAction.ToString())
          {
            SubItems = {
              ""
            }
          });
      }
      this.TopMost = true;
      this.TopMost = false;
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      try
      {
        this.btnStart.Enabled = false;
        this.btnCancel.Enabled = false;
        this.manager.StartUpgrade();
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        this.DialogResult = DialogResult.Abort;
      }
      finally
      {
        this.btnStart.Enabled = true;
        this.btnCancel.Enabled = true;
      }
    }

    private void Manager_OnActionStateChanged(object sender, UpgradeActionEventArgs e)
    {
      ListViewItem itemWithText = this.listView.FindItemWithText(e.Action.ToString(), false, 0, false);
      if (itemWithText == null)
        return;
      itemWithText.SubItems[1].Text = e.State;
      Application.DoEvents();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DatabaseUpgradeWindow));
      this.btnStart = new Button();
      this.btnCancel = new Button();
      this.panel1 = new Panel();
      this.label2 = new Label();
      this.label1 = new Label();
      this.listView = new ListViewDoubledBuffered();
      this.colAction = new ColumnHeader();
      this.colStatus = new ColumnHeader();
      this.label3 = new Label();
      this.label4 = new Label();
      this.lblOldDatabaseVersion = new Label();
      this.lblNewDatabaseVersion = new Label();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.btnStart, "btnStart");
      this.btnStart.Name = "btnStart";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.BackColor = Color.White;
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.listView, "listView");
      this.listView.Columns.AddRange(new ColumnHeader[2]
      {
        this.colAction,
        this.colStatus
      });
      this.listView.GridLines = true;
      this.listView.Name = "listView";
      this.listView.UseCompatibleStateImageBehavior = false;
      this.listView.View = View.Details;
      componentResourceManager.ApplyResources((object) this.colAction, "colAction");
      componentResourceManager.ApplyResources((object) this.colStatus, "colStatus");
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.lblOldDatabaseVersion, "lblOldDatabaseVersion");
      this.lblOldDatabaseVersion.ForeColor = Color.Blue;
      this.lblOldDatabaseVersion.Name = "lblOldDatabaseVersion";
      componentResourceManager.ApplyResources((object) this.lblNewDatabaseVersion, "lblNewDatabaseVersion");
      this.lblNewDatabaseVersion.ForeColor = Color.Blue;
      this.lblNewDatabaseVersion.Name = "lblNewDatabaseVersion";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblNewDatabaseVersion);
      this.Controls.Add((Control) this.lblOldDatabaseVersion);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.listView);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnStart);
      this.Name = nameof (DatabaseUpgradeWindow);
      this.Load += new System.EventHandler(this.DatabaseUpgradeWindow_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
