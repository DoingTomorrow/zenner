// Decompiled with JetBrains decompiler
// Type: InTheHand.Windows.Forms.SelectBluetoothDeviceForm
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace InTheHand.Windows.Forms
{
  internal sealed class SelectBluetoothDeviceForm : Form, ISelectBluetoothDevice
  {
    private MainMenu mainMenu1;
    private MenuItem mnuMenu;
    private MenuItem mnuSelect;
    private MenuItem mnuAgain;
    private ListView lvDevices;
    private MenuItem mnuCancel;
    public BluetoothDeviceInfo selectedDevice;
    private ColumnHeader clmDevice;
    private bool fShowAuthenticated;
    private bool fShowRemembered;
    private bool fShowUnknown;
    private bool fForceAuthentication;
    private bool fDiscoverableOnly;
    private Predicate<BluetoothDeviceInfo> _filterFn;
    private string info = string.Empty;
    private volatile bool _closed;
    private List<uint> classOfDeviceFilter;

    public SelectBluetoothDeviceForm()
    {
      this.InitializeComponent();
      this.classOfDeviceFilter = new List<uint>();
      this.Size = new Size(400, 250);
      this.fShowAuthenticated = true;
      this.fShowRemembered = true;
      this.fShowUnknown = true;
      this.fForceAuthentication = false;
    }

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void InitializeComponent()
    {
      ListViewItem listViewItem = new ListViewItem();
      this.lvDevices = new ListView();
      this.clmDevice = new ColumnHeader();
      this.mainMenu1 = new MainMenu();
      this.mnuSelect = new MenuItem();
      this.mnuMenu = new MenuItem();
      this.mnuAgain = new MenuItem();
      this.mnuCancel = new MenuItem();
      this.SuspendLayout();
      this.lvDevices.Columns.Add(this.clmDevice);
      this.lvDevices.Font = new Font("Tahoma", 12f, FontStyle.Regular);
      this.lvDevices.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem.Text = "Scanning for Bluetooth devices...";
      this.lvDevices.Items.Add(listViewItem);
      this.lvDevices.Location = new Point(8, 32);
      this.lvDevices.Name = "lvDevices";
      this.lvDevices.Size = new Size(100, 200);
      this.lvDevices.TabIndex = 0;
      this.lvDevices.View = View.Details;
      this.lvDevices.ItemActivate += new EventHandler(this.selectDevice);
      this.clmDevice.Text = "ColumnHeader";
      this.clmDevice.Width = 100;
      this.mainMenu1.MenuItems.Add(this.mnuSelect);
      this.mainMenu1.MenuItems.Add(this.mnuMenu);
      this.mnuSelect.Enabled = false;
      this.mnuSelect.Text = "Select";
      this.mnuSelect.Click += new EventHandler(this.selectDevice);
      this.mnuMenu.MenuItems.Add(this.mnuAgain);
      this.mnuMenu.MenuItems.Add(this.mnuCancel);
      this.mnuMenu.Text = "Menu";
      this.mnuAgain.Enabled = false;
      this.mnuAgain.Text = "Search Again";
      this.mnuAgain.Click += new EventHandler(this.mnuAgain_Click);
      this.mnuCancel.Text = "Cancel";
      this.mnuCancel.Click += new EventHandler(this.mnuCancel_Click);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(240, 240);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lvDevices);
      this.Menu = this.mainMenu1;
      this.Name = nameof (SelectBluetoothDeviceForm);
      this.Text = "Bluetooth";
      this.Load += new EventHandler(this.SelectBluetoothDeviceForm_Load);
      this.Resize += new EventHandler(this.SelectBluetoothDeviceForm_Resize);
      this.ResumeLayout(false);
    }

    private void mnuCancel_Click(object sender, EventArgs e)
    {
      this._closed = true;
      Cursor.Current = Cursors.Default;
      this.DialogResult = DialogResult.Cancel;
    }

    private void SelectBluetoothDeviceForm_Load(object sender, EventArgs e) => this.Start();

    private void Start()
    {
      this._closed = false;
      Cursor.Current = Cursors.WaitCursor;
      this.lvDevices.Items.Clear();
      this.mnuAgain.Enabled = false;
      this.StartDiscovery();
    }

    private void SelectBluetoothDeviceForm_Resize(object sender, EventArgs e)
    {
      this.lvDevices.Bounds = new Rectangle(-1, -1, this.Width + 2, this.Height + 2);
      this.lvDevices.Columns[0].Width = this.Width - 2;
    }

    private void StartDiscovery()
    {
      BluetoothClient cli = new BluetoothClient();
      this.UpdateProgressChanged((object) null, new DiscoverDevicesEventArgs(this.fShowAuthenticated || this.fShowRemembered ? cli.DiscoverDevices((int) byte.MaxValue, this.fShowAuthenticated, this.fShowRemembered, false, this.fDiscoverableOnly) : new BluetoothDeviceInfo[0], (object) null));
      if (this.fShowUnknown || this.fDiscoverableOnly)
      {
        BluetoothComponent state = new BluetoothComponent(cli);
        state.DiscoverDevicesAsync((int) byte.MaxValue, false, false, true, this.fDiscoverableOnly, (object) state);
        state.DiscoverDevicesProgress += new EventHandler<DiscoverDevicesEventArgs>(this.bco_DiscoverDevicesProgress);
        state.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(this.bco_DiscoverDevicesComplete);
      }
      else
        this.UpdateCompleted((object) null, new DiscoverDevicesEventArgs(new BluetoothDeviceInfo[0], (object) null));
    }

    private void bco_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
    {
      this.lvDevices.BeginInvoke((Delegate) new EventHandler<DiscoverDevicesEventArgs>(this.UpdateProgressChanged), (object) this, (object) e);
    }

    private void bco_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
    {
      this.lvDevices.BeginInvoke((Delegate) new EventHandler<DiscoverDevicesEventArgs>(this.UpdateCompleted), (object) this, (object) e);
      ((Component) e.UserState).Dispose();
    }

    private void UpdateProgressChanged(object sender, DiscoverDevicesEventArgs e)
    {
      if (this._closed)
        return;
      this.AppendDevices(this.DoFiltering(e.Devices));
    }

    private BluetoothDeviceInfo[] DoFiltering(BluetoothDeviceInfo[] newDevices)
    {
      if (this.classOfDeviceFilter.Count > 0)
      {
        List<BluetoothDeviceInfo> bluetoothDeviceInfoList = new List<BluetoothDeviceInfo>();
        foreach (BluetoothDeviceInfo newDevice in newDevices)
        {
          foreach (uint num in this.classOfDeviceFilter)
          {
            if (((int) num & 7936) == (int) num)
            {
              if (((int) newDevice.ClassOfDevice.Value & 7936) == (int) num)
              {
                bluetoothDeviceInfoList.Add(newDevice);
                break;
              }
            }
            else if (((int) newDevice.ClassOfDevice.Value & 8188 & (int) num) == (int) num)
            {
              bluetoothDeviceInfoList.Add(newDevice);
              break;
            }
          }
        }
        newDevices = bluetoothDeviceInfoList.ToArray();
      }
      if (this._filterFn != null)
      {
        Predicate<BluetoothDeviceInfo> filterFn = this._filterFn;
        List<BluetoothDeviceInfo> bluetoothDeviceInfoList = new List<BluetoothDeviceInfo>();
        foreach (BluetoothDeviceInfo newDevice in newDevices)
        {
          if (filterFn(newDevice))
            bluetoothDeviceInfoList.Add(newDevice);
        }
        newDevices = bluetoothDeviceInfoList.ToArray();
      }
      return newDevices;
    }

    private void UpdateCompleted(object sender, DiscoverDevicesEventArgs e)
    {
      if (this._closed)
        return;
      if (e.Error != null)
      {
        this.lvDevices.Items.Add(new ListViewItem("Failed: " + (object) e.Error)
        {
          ImageIndex = -1
        });
      }
      else
      {
        BluetoothDeviceInfo[] devices = e.Devices;
        if (devices != null)
          this.AppendDevices(this.DoFiltering(devices));
      }
      this.mnuAgain.Enabled = true;
      this.lvDevices.Focus();
      Cursor.Current = Cursors.Default;
    }

    private void AppendDevices(BluetoothDeviceInfo[] newDevices)
    {
      if (newDevices == null)
        return;
      foreach (BluetoothDeviceInfo newDevice in newDevices)
      {
        SelectBluetoothDeviceForm.BdiListViewItem bdiListViewItem = new SelectBluetoothDeviceForm.BdiListViewItem(newDevice);
        bdiListViewItem.ImageIndex = 0;
        this.lvDevices.Items.Add((ListViewItem) bdiListViewItem);
      }
      if (this.lvDevices.Items.Count <= 0)
        return;
      this.mnuSelect.Enabled = true;
    }

    private void selectDevice(object sender, EventArgs e)
    {
      if (this.lvDevices.SelectedIndices.Count <= 0 || this.lvDevices.SelectedIndices[0] <= -1 || this.lvDevices.SelectedIndices[0] >= this.lvDevices.Items.Count)
        return;
      this._closed = true;
      Cursor.Current = Cursors.Default;
      if (!(this.lvDevices.Items[this.lvDevices.SelectedIndices[0]] is SelectBluetoothDeviceForm.BdiListViewItem bdiListViewItem))
        return;
      this.selectedDevice = bdiListViewItem.Device;
      int num = this.fForceAuthentication ? 1 : 0;
      this.DialogResult = DialogResult.OK;
    }

    private void mnuAgain_Click(object sender, EventArgs e) => this.Start();

    public override void Refresh()
    {
      this.lvDevices.Items.Clear();
      this.fShowAuthenticated = true;
      this.fShowUnknown = true;
      this.fForceAuthentication = false;
      base.Refresh();
    }

    void ISelectBluetoothDevice.Reset() => this.Refresh();

    bool ISelectBluetoothDevice.ShowAuthenticated
    {
      get => this.fShowAuthenticated;
      set => this.fShowAuthenticated = value;
    }

    bool ISelectBluetoothDevice.ShowRemembered
    {
      get => this.fShowRemembered;
      set => this.fShowRemembered = value;
    }

    bool ISelectBluetoothDevice.ShowUnknown
    {
      get => this.fShowUnknown;
      set => this.fShowUnknown = value;
    }

    bool ISelectBluetoothDevice.ForceAuthentication
    {
      get => this.fForceAuthentication;
      set => this.fForceAuthentication = value;
    }

    bool ISelectBluetoothDevice.ShowDiscoverableOnly
    {
      get => this.fDiscoverableOnly;
      set => this.fDiscoverableOnly = value;
    }

    string ISelectBluetoothDevice.Info
    {
      get => this.info;
      set => this.info = value;
    }

    bool ISelectBluetoothDevice.AddNewDeviceWizard
    {
      get => false;
      set => throw new NotImplementedException("The method or operation is not implemented.");
    }

    bool ISelectBluetoothDevice.SkipServicesPage
    {
      get => true;
      set => throw new NotImplementedException("The method or operation is not implemented.");
    }

    public void SetClassOfDevices(ClassOfDevice[] classOfDevices)
    {
      this.classOfDeviceFilter.Clear();
      foreach (ClassOfDevice classOfDevice in classOfDevices)
        this.classOfDeviceFilter.Add(classOfDevice.Value);
    }

    public void SetFilter(
      Predicate<BluetoothDeviceInfo> filterFn,
      SelectBluetoothDeviceDialog.PFN_DEVICE_CALLBACK msftFilterFn)
    {
      this._filterFn = filterFn;
    }

    private class BdiListViewItem : ListViewItem
    {
      private readonly BluetoothDeviceInfo m_bdi;

      public BdiListViewItem(BluetoothDeviceInfo device)
        : base(device.DeviceName)
      {
        this.m_bdi = device;
      }

      public BluetoothDeviceInfo Device => this.m_bdi;
    }
  }
}
