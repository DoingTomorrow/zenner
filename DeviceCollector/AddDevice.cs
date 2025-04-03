// Decompiled with JetBrains decompiler
// Type: DeviceCollector.AddDevice
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class AddDevice : Form
  {
    private List<ListElement> DisplayList = new List<ListElement>();
    private DeviceCollectorFunctions MyBus;
    internal object NewDevice = (object) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button buttonAdd;
    private Button buttonCancel;
    private Label label1;
    private ListBox listBoxDeviceTypes;
    private Label lbAddress;
    private ComboBox comboBoxAddress;
    private Label lbSerialNumber;
    private ComboBox comboBoxSerialNumber;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public AddDevice(DeviceCollectorFunctions MyBusIn)
    {
      this.InitializeComponent();
      this.MyBus = MyBusIn;
      if (this.MyBus.MyBusMode == BusMode.WaveFlowRadio)
        this.DisplayList.Add(new ListElement("WAFEFLOW (common)", DeviceTypes.WaveFlowDevice));
      else if (this.MyBus.MyBusMode == BusMode.Minol_Device)
      {
        this.DisplayList.Add(new ListElement("Minol Device", DeviceTypes.Minol_Device));
      }
      else
      {
        this.DisplayList.Add(new ListElement("COMMON MBUS DEVICE", DeviceTypes.MBus));
        this.DisplayList.Add(new ListElement("ZENNER Serie1 (multidata S1/N1)", DeviceTypes.ZR_Serie1));
        this.DisplayList.Add(new ListElement("ZENNER Serie2 (zelsius C2,multidata WR3)", DeviceTypes.ZR_Serie2));
        this.DisplayList.Add(new ListElement("ZENNER Serie3 (C5)", DeviceTypes.ZR_Serie3));
        this.DisplayList.Add(new ListElement("ZENNER EDC", DeviceTypes.EDC));
        this.DisplayList.Add(new ListElement("ZENNER PDC", DeviceTypes.PDC));
      }
      foreach (ListElement display in this.DisplayList)
        this.listBoxDeviceTypes.Items.Add((object) display.Name);
      this.listBoxDeviceTypes.SelectedIndex = 0;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddDevice));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.buttonAdd = new Button();
      this.buttonCancel = new Button();
      this.listBoxDeviceTypes = new ListBox();
      this.label1 = new Label();
      this.lbAddress = new Label();
      this.comboBoxAddress = new ComboBox();
      this.lbSerialNumber = new Label();
      this.comboBoxSerialNumber = new ComboBox();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this.buttonAdd, "buttonAdd");
      this.buttonAdd.Name = "buttonAdd";
      this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
      componentResourceManager.ApplyResources((object) this.buttonCancel, "buttonCancel");
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      componentResourceManager.ApplyResources((object) this.listBoxDeviceTypes, "listBoxDeviceTypes");
      this.listBoxDeviceTypes.Name = "listBoxDeviceTypes";
      this.listBoxDeviceTypes.MouseDoubleClick += new MouseEventHandler(this.listBoxDeviceTypes_MouseDoubleClick);
      this.listBoxDeviceTypes.SelectedIndexChanged += new System.EventHandler(this.listBoxDeviceTypes_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.lbAddress, "lbAddress");
      this.lbAddress.Name = "lbAddress";
      componentResourceManager.ApplyResources((object) this.comboBoxAddress, "comboBoxAddress");
      this.comboBoxAddress.Items.AddRange(new object[21]
      {
        (object) componentResourceManager.GetString("comboBoxAddress.Items"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items1"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items2"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items3"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items4"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items5"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items6"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items7"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items8"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items9"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items10"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items11"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items12"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items13"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items14"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items15"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items16"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items17"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items18"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items19"),
        (object) componentResourceManager.GetString("comboBoxAddress.Items20")
      });
      this.comboBoxAddress.Name = "comboBoxAddress";
      componentResourceManager.ApplyResources((object) this.lbSerialNumber, "lbSerialNumber");
      this.lbSerialNumber.Name = "lbSerialNumber";
      componentResourceManager.ApplyResources((object) this.comboBoxSerialNumber, "comboBoxSerialNumber");
      this.comboBoxSerialNumber.Items.AddRange(new object[1]
      {
        (object) componentResourceManager.GetString("comboBoxSerialNumber.Items")
      });
      this.comboBoxSerialNumber.Name = "comboBoxSerialNumber";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.comboBoxAddress);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.listBoxDeviceTypes);
      this.Controls.Add((Control) this.buttonAdd);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.lbAddress);
      this.Controls.Add((Control) this.lbSerialNumber);
      this.Controls.Add((Control) this.comboBoxSerialNumber);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Name = nameof (AddDevice);
      this.ResumeLayout(false);
    }

    private void buttonAdd_Click(object sender, EventArgs e)
    {
      this.SelectType(this.listBoxDeviceTypes.SelectedIndex);
    }

    private void listBoxDeviceTypes_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.SelectType(this.listBoxDeviceTypes.IndexFromPoint(e.Location));
    }

    private void SelectType(int index)
    {
      if (index < 0)
        return;
      DeviceTypes type = this.DisplayList[index].Type;
      int PrimaryAddress = -1;
      long SerialNumber = -1;
      if (type == DeviceTypes.WaveFlowDevice)
      {
        try
        {
          SerialNumber = long.Parse(this.comboBoxSerialNumber.Text);
        }
        catch
        {
          int num = (int) MessageBox.Show("Wrong serial number!", "Add device", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      else
      {
        if (char.IsDigit(this.comboBoxAddress.Text[0]))
        {
          try
          {
            PrimaryAddress = int.Parse(this.comboBoxAddress.Text);
            if (PrimaryAddress < 0 || PrimaryAddress > (int) byte.MaxValue)
              PrimaryAddress = -1;
          }
          catch
          {
          }
        }
        if (char.IsDigit(this.comboBoxSerialNumber.Text[0]))
        {
          try
          {
            SerialNumber = long.Parse(this.comboBoxSerialNumber.Text);
            if (SerialNumber < 0L || SerialNumber > 99999999L)
              SerialNumber = -1L;
          }
          catch
          {
          }
        }
      }
      this.MyBus.AddDevice(type, PrimaryAddress, SerialNumber);
      this.Close();
    }

    private void listBoxDeviceTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListElement display = this.DisplayList[this.listBoxDeviceTypes.SelectedIndex];
      if (display.Type == DeviceTypes.MBus)
      {
        this.lbAddress.Visible = true;
        this.comboBoxAddress.Visible = true;
        this.lbSerialNumber.Visible = true;
        this.comboBoxSerialNumber.Visible = true;
      }
      else if (display.Type == DeviceTypes.ZR_Serie1)
      {
        this.lbAddress.Visible = true;
        this.comboBoxAddress.Visible = true;
        this.lbSerialNumber.Visible = true;
        this.comboBoxSerialNumber.Visible = true;
      }
      else if (display.Type == DeviceTypes.ZR_Serie2)
      {
        this.lbAddress.Visible = true;
        this.comboBoxAddress.Visible = true;
        this.lbSerialNumber.Visible = true;
        this.comboBoxSerialNumber.Visible = true;
      }
      else if (display.Type == DeviceTypes.ZR_Serie3)
      {
        this.lbAddress.Visible = true;
        this.comboBoxAddress.Visible = true;
        this.lbSerialNumber.Visible = true;
        this.comboBoxSerialNumber.Visible = true;
      }
      else if (display.Type == DeviceTypes.ZR_EHCA)
      {
        this.lbAddress.Visible = false;
        this.comboBoxAddress.Visible = false;
        this.lbSerialNumber.Visible = true;
        this.comboBoxSerialNumber.Visible = true;
      }
      else if (display.Type == DeviceTypes.WaveFlowDevice)
      {
        this.lbAddress.Visible = false;
        this.comboBoxAddress.Visible = false;
        this.lbSerialNumber.Visible = true;
        this.comboBoxSerialNumber.Visible = true;
      }
      else
      {
        if (display.Type != DeviceTypes.Minol_Device)
          return;
        this.lbAddress.Visible = false;
        this.comboBoxAddress.Visible = false;
        this.lbSerialNumber.Visible = false;
        this.comboBoxSerialNumber.Visible = false;
      }
    }

    private void buttonCancel_Click(object sender, EventArgs e) => this.Close();
  }
}
