// Decompiled with JetBrains decompiler
// Type: PDC_Handler.TypeDataEditor
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public class TypeDataEditor : Form
  {
    private PDC_HandlerFunctions MyFunctions;
    private MeterInfo editMeterInfo;
    private PDC_Meter editMeter;
    private PDC_DeviceIdentity hardware;
    private IContainer components = (IContainer) null;
    private ComboBox cboxHardwareType;
    private Label lblHardwareType;
    private TextBox txtDescription;
    private ComboBox txtSapNumber;
    private Label label4;
    private Label label2;
    private Button btnSave;
    private Button btnCancel;
    private ErrorProvider error;
    private ComboBox cboxHandlerObject;
    private RadioButton rbtnUse;
    private RadioButton rbtnUseCurrentMeterObject;
    private ComboBox cboxBaseType;
    private Label lblBaseType;

    public TypeDataEditor() => this.InitializeComponent();

    internal static void ShowDialog(
      Form owner,
      PDC_DeviceIdentity hardware,
      PDC_HandlerFunctions MyFunctions)
    {
      TypeDataEditor.ShowDialog(owner, hardware, MyFunctions, (PDC_Meter) null, (MeterInfo) null);
    }

    internal static void ShowDialog(
      Form owner,
      PDC_DeviceIdentity hardware,
      PDC_HandlerFunctions MyFunctions,
      PDC_Meter editMeter,
      MeterInfo editMeterInfo)
    {
      using (TypeDataEditor typeDataEditor = new TypeDataEditor())
      {
        typeDataEditor.editMeter = editMeter;
        typeDataEditor.MyFunctions = MyFunctions;
        typeDataEditor.editMeterInfo = editMeterInfo;
        typeDataEditor.hardware = hardware;
        int num = (int) typeDataEditor.ShowDialog((IWin32Window) owner);
      }
    }

    private void TypeDataEditor_Load(object sender, EventArgs e)
    {
      this.cboxHandlerObject.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (HandlerMeterType));
      this.cboxHandlerObject.SelectedItem = (object) HandlerMeterType.WorkMeter;
      this.cboxBaseType.DataSource = (object) PDC_Database.LoadMeterInfo("PDC_BASETYPE", this.hardware);
      this.RefreshHardwareTypes();
      if (this.editMeterInfo != null)
      {
        this.SelectHardwareType(MeterDatabase.GetHardwareType(this.editMeterInfo.HardwareTypeID));
        this.txtSapNumber.Text = this.editMeterInfo.PPSArtikelNr;
        this.txtDescription.Text = this.editMeterInfo.Description;
        this.btnSave.Text = "Override";
        this.Text = "Edit type";
        this.rbtnUse.Checked = false;
        this.rbtnUseCurrentMeterObject.Visible = true;
        this.SelectBaseType();
      }
      else
      {
        this.txtSapNumber.Text = "PDC_BASETYPE";
        this.txtDescription.Text = "Base type";
        this.btnSave.Text = "Save";
        this.Text = "Create new type";
        this.rbtnUse.Checked = true;
        this.rbtnUseCurrentMeterObject.Visible = false;
      }
    }

    private void SelectBaseType()
    {
      if (this.editMeter == null)
        return;
      DeviceIdentification deviceIdentification = this.editMeter.GetDeviceIdentification();
      if (deviceIdentification == null || !deviceIdentification.IsChecksumOK)
        return;
      for (int index = 0; index < this.cboxBaseType.Items.Count; ++index)
      {
        if ((long) (this.cboxBaseType.Items[index] as MeterInfo).MeterInfoID == (long) deviceIdentification.BaseTypeID)
        {
          this.cboxBaseType.SelectedIndex = index;
          break;
        }
      }
    }

    private void SelectHardwareType(HardwareType sel)
    {
      for (int index = 0; index < this.cboxHardwareType.Items.Count; ++index)
      {
        if ((this.cboxHardwareType.Items[index] as HardwareType).HardwareTypeID == sel.HardwareTypeID)
        {
          this.cboxHardwareType.SelectedIndex = index;
          break;
        }
      }
    }

    private void rbtnUse_CheckedChanged(object sender, EventArgs e)
    {
      this.cboxHandlerObject.Enabled = this.rbtnUse.Checked;
      this.RefreshHardwareTypes();
    }

    private void cboxHandlerObject_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.RefreshHardwareTypes();
    }

    private void txtSapNumber_TextChanged(object sender, EventArgs e)
    {
      bool flag = this.txtSapNumber.Text == "PDC_BASETYPE";
      this.cboxHardwareType.Visible = !flag;
      this.lblHardwareType.Visible = !flag;
      this.cboxBaseType.Visible = !flag;
      this.lblBaseType.Visible = !flag;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.error.Clear();
      bool flag = true;
      PDC_Meter selectedHandlerMeter = this.GetSelectedHandlerMeter();
      if (selectedHandlerMeter == null)
      {
        string str = "Meter object can not be null!";
        this.error.SetError(this.rbtnUse.Checked ? (Control) this.cboxHandlerObject : (Control) this.rbtnUseCurrentMeterObject, str);
        flag = false;
      }
      if (selectedHandlerMeter != null && this.hardware != selectedHandlerMeter.Version.Type)
      {
        string str = "Wrong type of Meter object! Actual: " + selectedHandlerMeter.Version.Type.ToString() + " Expected: " + this.hardware.ToString();
        this.error.SetError(this.rbtnUse.Checked ? (Control) this.cboxHandlerObject : (Control) this.rbtnUseCurrentMeterObject, str);
        flag = false;
      }
      if (string.IsNullOrEmpty(this.txtSapNumber.Text))
      {
        this.error.SetError((Control) this.txtSapNumber, "SAP material number can not be empty!");
        flag = false;
      }
      if (this.txtSapNumber.Text == "PDC_BASETYPE" && MeterDatabase.GetDatabaseLocationName() != "ZENNER Development")
      {
        this.error.SetError((Control) this.txtSapNumber, "Only 'ZENNER Development' can create a base type!");
        flag = false;
      }
      HardwareType selectedHardwareType = this.GetSelectedHardwareType();
      if (selectedHardwareType == null)
      {
        this.error.SetError((Control) this.cboxHardwareType, "Hardware type can not be null!");
        flag = false;
      }
      if (string.IsNullOrEmpty(this.txtDescription.Text))
      {
        this.error.SetError((Control) this.txtDescription, "The type description can not be empty!");
        flag = false;
      }
      if (!flag)
        return;
      if (this.cboxBaseType.Visible && this.cboxBaseType.SelectedItem is MeterInfo)
      {
        MeterInfo selectedItem = this.cboxBaseType.SelectedItem as MeterInfo;
        DeviceIdentification ident = selectedHandlerMeter.GetDeviceIdentification();
        if (ident == null || !ident.IsChecksumOK)
          ident = new DeviceIdentification();
        ident.BaseTypeID = (uint) selectedItem.MeterInfoID;
        selectedHandlerMeter.SetDeviceIdentification(ident);
      }
      if (this.editMeterInfo == null)
      {
        if (PDC_Database.CreateType(this.txtSapNumber.Text.Trim(), selectedHardwareType.HardwareTypeID, this.txtDescription.Text.Trim(), selectedHandlerMeter))
          this.Close();
        else
          ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
      {
        if (MessageBox.Show("Override existing type?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        this.editMeterInfo.Description = this.txtDescription.Text.Trim();
        this.editMeterInfo.PPSArtikelNr = this.txtSapNumber.Text.Trim();
        this.editMeterInfo.HardwareTypeID = selectedHardwareType.HardwareTypeID;
        if (PDC_Database.UpdateType(this.editMeterInfo, selectedHandlerMeter))
          this.Close();
        else
          ZR_ClassLibMessages.ShowAndClearErrors();
      }
      this.DialogResult = DialogResult.OK;
    }

    private HardwareType GetSelectedHardwareType()
    {
      return this.cboxHardwareType.SelectedItem as HardwareType;
    }

    private PDC_Meter GetSelectedHandlerMeter()
    {
      if (this.MyFunctions == null)
        return (PDC_Meter) null;
      if (!this.rbtnUse.Checked)
        return this.editMeter;
      switch ((HandlerMeterType) Enum.Parse(typeof (HandlerMeterType), this.cboxHandlerObject.SelectedItem.ToString()))
      {
        case HandlerMeterType.WorkMeter:
          return this.MyFunctions.WorkMeter != null ? this.MyFunctions.WorkMeter.DeepCopy() : (PDC_Meter) null;
        case HandlerMeterType.TypeMeter:
          return this.MyFunctions.TypeMeter != null ? this.MyFunctions.TypeMeter.DeepCopy() : (PDC_Meter) null;
        case HandlerMeterType.BackupMeter:
          return this.MyFunctions.BackupMeter != null ? this.MyFunctions.BackupMeter.DeepCopy() : (PDC_Meter) null;
        case HandlerMeterType.ConnectedMeter:
          return this.MyFunctions.ConnectedMeter != null ? this.MyFunctions.ConnectedMeter.DeepCopy() : (PDC_Meter) null;
        default:
          throw new NotImplementedException();
      }
    }

    private void RefreshHardwareTypes()
    {
      PDC_Meter selectedHandlerMeter = this.GetSelectedHandlerMeter();
      if (selectedHandlerMeter != null)
        this.cboxHardwareType.DataSource = (object) PDC_Database.LoadHardwareType(selectedHandlerMeter.Version);
      else
        this.cboxHardwareType.DataSource = (object) null;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TypeDataEditor));
      this.cboxHardwareType = new ComboBox();
      this.lblHardwareType = new Label();
      this.txtDescription = new TextBox();
      this.txtSapNumber = new ComboBox();
      this.label4 = new Label();
      this.label2 = new Label();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.error = new ErrorProvider(this.components);
      this.cboxHandlerObject = new ComboBox();
      this.rbtnUseCurrentMeterObject = new RadioButton();
      this.rbtnUse = new RadioButton();
      this.cboxBaseType = new ComboBox();
      this.lblBaseType = new Label();
      ((ISupportInitialize) this.error).BeginInit();
      this.SuspendLayout();
      this.cboxHardwareType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxHardwareType.FormattingEnabled = true;
      this.cboxHardwareType.Location = new Point(132, 121);
      this.cboxHardwareType.Name = "cboxHardwareType";
      this.cboxHardwareType.Size = new Size(428, 21);
      this.cboxHardwareType.TabIndex = 4;
      this.lblHardwareType.Location = new Point(13, 123);
      this.lblHardwareType.Name = "lblHardwareType";
      this.lblHardwareType.Size = new Size(113, 13);
      this.lblHardwareType.TabIndex = 60;
      this.lblHardwareType.Text = "Hardware Type:";
      this.lblHardwareType.TextAlign = ContentAlignment.MiddleRight;
      this.txtDescription.Location = new Point(132, 41);
      this.txtDescription.MaxLength = (int) byte.MaxValue;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(428, 20);
      this.txtDescription.TabIndex = 1;
      this.txtSapNumber.FormattingEnabled = true;
      this.txtSapNumber.Items.AddRange(new object[2]
      {
        (object) "NotDefined",
        (object) "PDC_BASETYPE"
      });
      this.txtSapNumber.Location = new Point(132, 67);
      this.txtSapNumber.Name = "txtSapNumber";
      this.txtSapNumber.Size = new Size(132, 21);
      this.txtSapNumber.TabIndex = 3;
      this.txtSapNumber.TextChanged += new System.EventHandler(this.txtSapNumber_TextChanged);
      this.label4.Location = new Point(13, 70);
      this.label4.Name = "label4";
      this.label4.Size = new Size(113, 13);
      this.label4.TabIndex = 40;
      this.label4.Text = "SAP material number:";
      this.label4.TextAlign = ContentAlignment.MiddleRight;
      this.label2.Location = new Point(13, 44);
      this.label2.Name = "label2";
      this.label2.Size = new Size(113, 13);
      this.label2.TabIndex = 54;
      this.label2.Text = "Description:";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.ImeMode = ImeMode.NoControl;
      this.btnSave.Location = new Point(179, 154);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(99, 29);
      this.btnSave.TabIndex = 5;
      this.btnSave.Text = "Save";
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(294, 154);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(99, 29);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.error.ContainerControl = (ContainerControl) this;
      this.cboxHandlerObject.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxHandlerObject.Enabled = false;
      this.cboxHandlerObject.FormattingEnabled = true;
      this.cboxHandlerObject.Location = new Point(132, 12);
      this.cboxHandlerObject.Name = "cboxHandlerObject";
      this.cboxHandlerObject.Size = new Size(132, 21);
      this.cboxHandlerObject.TabIndex = 62;
      this.cboxHandlerObject.SelectedIndexChanged += new System.EventHandler(this.cboxHandlerObject_SelectedIndexChanged);
      this.rbtnUseCurrentMeterObject.AutoSize = true;
      this.rbtnUseCurrentMeterObject.Checked = true;
      this.rbtnUseCurrentMeterObject.Location = new Point(274, 13);
      this.rbtnUseCurrentMeterObject.Name = "rbtnUseCurrentMeterObject";
      this.rbtnUseCurrentMeterObject.Size = new Size(141, 17);
      this.rbtnUseCurrentMeterObject.TabIndex = 63;
      this.rbtnUseCurrentMeterObject.TabStop = true;
      this.rbtnUseCurrentMeterObject.Text = "Use current meter object";
      this.rbtnUseCurrentMeterObject.UseVisualStyleBackColor = true;
      this.rbtnUseCurrentMeterObject.CheckedChanged += new System.EventHandler(this.rbtnUse_CheckedChanged);
      this.rbtnUse.AutoSize = true;
      this.rbtnUse.Location = new Point(82, 13);
      this.rbtnUse.Name = "rbtnUse";
      this.rbtnUse.Size = new Size(44, 17);
      this.rbtnUse.TabIndex = 64;
      this.rbtnUse.Text = "Use";
      this.rbtnUse.UseVisualStyleBackColor = true;
      this.rbtnUse.CheckedChanged += new System.EventHandler(this.rbtnUse_CheckedChanged);
      this.cboxBaseType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxBaseType.FormattingEnabled = true;
      this.cboxBaseType.Items.AddRange(new object[2]
      {
        (object) "NotDefined",
        (object) "PDC_BASETYPE"
      });
      this.cboxBaseType.Location = new Point(132, 94);
      this.cboxBaseType.Name = "cboxBaseType";
      this.cboxBaseType.Size = new Size(428, 21);
      this.cboxBaseType.TabIndex = 65;
      this.lblBaseType.Location = new Point(17, 97);
      this.lblBaseType.Name = "lblBaseType";
      this.lblBaseType.Size = new Size(109, 13);
      this.lblBaseType.TabIndex = 66;
      this.lblBaseType.Text = "Base type:";
      this.lblBaseType.TextAlign = ContentAlignment.MiddleRight;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(572, 195);
      this.Controls.Add((Control) this.cboxBaseType);
      this.Controls.Add((Control) this.lblBaseType);
      this.Controls.Add((Control) this.rbtnUse);
      this.Controls.Add((Control) this.rbtnUseCurrentMeterObject);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.cboxHardwareType);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.lblHardwareType);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.txtSapNumber);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label4);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TypeDataEditor);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Type";
      this.Load += new System.EventHandler(this.TypeDataEditor_Load);
      ((ISupportInitialize) this.error).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
