// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.DataGridViewColumnSelector
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class DataGridViewColumnSelector : Form
  {
    private DataGridView dataGrid;
    private MenuItem menu;
    private DataGridViewColumnSelector.DataGridViewSettings settings;
    private Dictionary<string, bool> oldSettings;
    public const string ROOT_DIRECTORY = "DataGridViews";
    private IContainer components = (IContainer) null;
    private CheckedListBox listBoxColumns;
    private Button btnOk;
    private TableLayoutPanel tableLayoutPanel1;

    public DataGridViewColumnSelector() => this.InitializeComponent();

    public DataGridViewColumnSelector(DataGridView dataGrid, MenuItem menu)
      : this()
    {
      this.settings = new DataGridViewColumnSelector.DataGridViewSettings();
      this.dataGrid = dataGrid;
      this.dataGrid.DataSourceChanged += new System.EventHandler(this.DataGrid_DataSourceChanged);
      this.menu = menu;
      this.menu.Click += new System.EventHandler(this.Menu_Click);
      this.oldSettings = new Dictionary<string, bool>();
    }

    public string[] ColumnsShowAlways { get; set; }

    private void Menu_Click(object sender, EventArgs e)
    {
      if (this.dataGrid == null || this.dataGrid.Columns.Count == 0)
        return;
      this.Location = this.PointToClient(Cursor.Position);
      this.listBoxColumns.Items.Clear();
      this.oldSettings.Clear();
      foreach (DataGridViewColumn column in (BaseCollection) this.dataGrid.Columns)
      {
        if (!this.IsAllowedHideThisColumn(column.Name))
          this.listBoxColumns.Items.Add((object) column.Name, CheckState.Indeterminate);
        else
          this.listBoxColumns.Items.Add((object) column.Name, column.Visible);
        this.oldSettings.Add(column.Name, column.Visible);
      }
      this.DialogResult = DialogResult.None;
      int num = (int) this.ShowDialog();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void listBoxColumns_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!this.IsAllowedHideThisColumn(this.listBoxColumns.Items[e.Index].ToString()))
        e.NewValue = CheckState.Indeterminate;
      else
        this.dataGrid.Columns[e.Index].Visible = e.NewValue == CheckState.Checked;
    }

    private void DataGrid_DataSourceChanged(object sender, EventArgs e)
    {
      if (this.dataGrid.Columns.Count == 0)
        return;
      this.LoadSettings();
      if (this.settings == null || this.settings.HidedColumns == null)
        return;
      foreach (string hidedColumn in this.settings.HidedColumns)
      {
        if (this.dataGrid.Columns.Contains(hidedColumn))
          this.dataGrid.Columns[hidedColumn].Visible = false;
      }
    }

    private void DataGridViewColumnSelector_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.OK)
      {
        foreach (KeyValuePair<string, bool> oldSetting in this.oldSettings)
        {
          if (this.dataGrid.Columns.Contains(oldSetting.Key))
            this.dataGrid.Columns[oldSetting.Key].Visible = oldSetting.Value;
        }
      }
      else
      {
        List<string> stringList = new List<string>();
        foreach (DataGridViewColumn column in (BaseCollection) this.dataGrid.Columns)
        {
          if (!column.Visible)
            stringList.Add(column.Name);
        }
        this.settings.HidedColumns = stringList.ToArray();
        this.SaveSettings();
      }
    }

    private void SaveSettings()
    {
      if (this.settings == null || this.settings.HidedColumns == null || this.settings.HidedColumns.Length == 0)
        return;
      try
      {
        string str = Path.Combine(SystemValues.SettingsPath, "DataGridViews");
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        using (Stream serializationStream = (Stream) File.Open(Path.Combine(str, this.GetSettingsFileName()), FileMode.Create))
          new BinaryFormatter().Serialize(serializationStream, (object) this.settings);
      }
      catch
      {
      }
    }

    private void LoadSettings()
    {
      try
      {
        string str = Path.Combine(SystemValues.SettingsPath, "DataGridViews");
        if (!Directory.Exists(str))
          return;
        string path = Path.Combine(str, this.GetSettingsFileName());
        if (!File.Exists(path))
          return;
        using (Stream serializationStream = (Stream) File.Open(path, FileMode.Open))
          this.settings = (DataGridViewColumnSelector.DataGridViewSettings) new BinaryFormatter().Deserialize(serializationStream);
      }
      catch
      {
      }
    }

    private string GetSettingsFileName()
    {
      StringBuilder stringBuilder = new StringBuilder(this.dataGrid.Name);
      for (Control parent = this.dataGrid.Parent; parent != null; parent = parent.Parent)
        stringBuilder.Append(parent.Name);
      foreach (DataGridViewColumn column in (BaseCollection) this.dataGrid.Columns)
        stringBuilder.Append(column.Name);
      return DataGridViewColumnSelector.GetSHA1Hash(stringBuilder.ToString());
    }

    public static string GetSHA1Hash(string text)
    {
      SHA1CryptoServiceProvider cryptoServiceProvider = new SHA1CryptoServiceProvider();
      string shA1Hash = (string) null;
      byte[] bytes = Encoding.ASCII.GetBytes(text);
      foreach (byte num in cryptoServiceProvider.ComputeHash(bytes))
      {
        string str = Convert.ToString(num, 16);
        if (str.Length == 1)
          str = "0" + str;
        shA1Hash += str;
      }
      return shA1Hash;
    }

    private bool IsAllowedHideThisColumn(string columnName)
    {
      if (this.ColumnsShowAlways != null && this.ColumnsShowAlways.Length != 0)
      {
        foreach (string columnsShowAlway in this.ColumnsShowAlways)
        {
          if (columnName == columnsShowAlway)
            return false;
        }
      }
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DataGridViewColumnSelector));
      this.listBoxColumns = new CheckedListBox();
      this.btnOk = new Button();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.listBoxColumns, "listBoxColumns");
      this.listBoxColumns.BorderStyle = BorderStyle.FixedSingle;
      this.listBoxColumns.CheckOnClick = true;
      this.listBoxColumns.FormattingEnabled = true;
      this.listBoxColumns.Name = "listBoxColumns";
      this.listBoxColumns.ItemCheck += new ItemCheckEventHandler(this.listBoxColumns_ItemCheck);
      componentResourceManager.ApplyResources((object) this.btnOk, "btnOk");
      this.btnOk.Name = "btnOk";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.btnOk, 1, 0);
      this.tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.AcceptButton = (IButtonControl) this.btnOk;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Controls.Add((Control) this.listBoxColumns);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DataGridViewColumnSelector);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.FormClosing += new FormClosingEventHandler(this.DataGridViewColumnSelector_FormClosing);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    [Serializable]
    private sealed class DataGridViewSettings
    {
      public string[] HidedColumns { get; set; }
    }
  }
}
