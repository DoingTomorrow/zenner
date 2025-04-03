// Decompiled with JetBrains decompiler
// Type: DeviceCollector.AddressReader
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace DeviceCollector
{
  public class AddressReader : Form
  {
    internal int Address = 0;
    internal string AddressString = "0";
    internal bool Break;
    private AddressReader.AddressNode ReadType;
    private TextBox textBoxAddress;
    private Button buttonOk;
    private Button buttonCancel;
    private Label labelAddressType;
    private static ResourceManager addressReaderMessages = new ResourceManager("DeviceCollector.AddressReader", typeof (DeviceCollectorFunctions).Assembly);
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public AddressReader(AddressReader.AddressNode R_Type) => this.BaseConstructer(R_Type, "0");

    public AddressReader(AddressReader.AddressNode R_Type, string StartAddress)
    {
      this.BaseConstructer(R_Type, StartAddress);
    }

    private void BaseConstructer(AddressReader.AddressNode R_Type, string StartAddress)
    {
      this.InitializeComponent();
      this.ReadType = R_Type;
      this.Break = true;
      if (this.ReadType == AddressReader.AddressNode.ShortAdr)
      {
        this.labelAddressType.Text = AddressReader.addressReaderMessages.GetString("primAddr");
        this.textBoxAddress.Text = StartAddress;
      }
      else if (this.ReadType == AddressReader.AddressNode.SerialNrAndWildcard)
      {
        this.labelAddressType.Text = AddressReader.addressReaderMessages.GetString("secAddr");
        this.textBoxAddress.Text = StartAddress;
      }
      else
      {
        this.labelAddressType.Text = AddressReader.addressReaderMessages.GetString("ParameterListNumber");
        this.textBoxAddress.Text = StartAddress;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddressReader));
      this.textBoxAddress = new TextBox();
      this.buttonOk = new Button();
      this.buttonCancel = new Button();
      this.labelAddressType = new Label();
      this.SuspendLayout();
      this.textBoxAddress.AccessibleDescription = (string) null;
      this.textBoxAddress.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.textBoxAddress, "textBoxAddress");
      this.textBoxAddress.BackgroundImage = (Image) null;
      this.textBoxAddress.Font = (Font) null;
      this.textBoxAddress.Name = "textBoxAddress";
      this.textBoxAddress.KeyDown += new KeyEventHandler(this.textBoxAddress_KeyDown);
      this.buttonOk.AccessibleDescription = (string) null;
      this.buttonOk.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.buttonOk, "buttonOk");
      this.buttonOk.BackgroundImage = (Image) null;
      this.buttonOk.Font = (Font) null;
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
      this.buttonCancel.AccessibleDescription = (string) null;
      this.buttonCancel.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.buttonCancel, "buttonCancel");
      this.buttonCancel.BackgroundImage = (Image) null;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Font = (Font) null;
      this.buttonCancel.Name = "buttonCancel";
      this.labelAddressType.AccessibleDescription = (string) null;
      this.labelAddressType.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this.labelAddressType, "labelAddressType");
      this.labelAddressType.Font = (Font) null;
      this.labelAddressType.Name = "labelAddressType";
      this.AccessibleDescription = (string) null;
      this.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.BackgroundImage = (Image) null;
      this.ControlBox = false;
      this.Controls.Add((Control) this.labelAddressType);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.textBoxAddress);
      this.Controls.Add((Control) this.buttonCancel);
      this.Font = (Font) null;
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) null;
      this.Name = nameof (AddressReader);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void textBoxAddress_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        if (!this.SetAddress())
          return;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
      else
      {
        if (e.KeyCode != Keys.Escape)
          return;
        this.Close();
      }
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
      if (!this.SetAddress())
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private bool SetAddress()
    {
      if (this.ReadType == AddressReader.AddressNode.ShortAdr || this.ReadType == AddressReader.AddressNode.ParameterListIndex)
      {
        try
        {
          this.Address = int.Parse(this.textBoxAddress.Text);
          if (this.Address < 0 || this.Address > 251)
          {
            this.textBoxAddress.Text = "0";
            return false;
          }
        }
        catch
        {
          this.textBoxAddress.Text = "0";
          return false;
        }
      }
      else
        this.AddressString = this.textBoxAddress.Text;
      this.Break = false;
      return true;
    }

    public enum AddressNode
    {
      ShortAdr,
      SerialNrAndWildcard,
      ParameterListIndex,
    }
  }
}
