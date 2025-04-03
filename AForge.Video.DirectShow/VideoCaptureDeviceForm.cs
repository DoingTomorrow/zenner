// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.VideoCaptureDeviceForm
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using AForge.Video.DirectShow.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace AForge.Video.DirectShow
{
  public class VideoCaptureDeviceForm : Form
  {
    private IContainer components;
    private Button cancelButton;
    private Button okButton;
    private ComboBox devicesCombo;
    private GroupBox groupBox1;
    private PictureBox pictureBox;
    private Label label1;
    private Label snapshotsLabel;
    private ComboBox snapshotResolutionsCombo;
    private ComboBox videoResolutionsCombo;
    private Label label2;
    private ComboBox videoInputsCombo;
    private Label label3;
    private FilterInfoCollection videoDevices;
    private VideoCaptureDevice videoDevice;
    private Dictionary<string, VideoCapabilities> videoCapabilitiesDictionary = new Dictionary<string, VideoCapabilities>();
    private Dictionary<string, VideoCapabilities> snapshotCapabilitiesDictionary = new Dictionary<string, VideoCapabilities>();
    private VideoInput[] availableVideoInputs;
    private bool configureSnapshots;
    private string videoDeviceMoniker = string.Empty;
    private Size captureSize = new Size(0, 0);
    private Size snapshotSize = new Size(0, 0);
    private VideoInput videoInput = VideoInput.Default;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cancelButton = new Button();
      this.okButton = new Button();
      this.devicesCombo = new ComboBox();
      this.groupBox1 = new GroupBox();
      this.videoInputsCombo = new ComboBox();
      this.label3 = new Label();
      this.snapshotsLabel = new Label();
      this.snapshotResolutionsCombo = new ComboBox();
      this.videoResolutionsCombo = new ComboBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.pictureBox = new PictureBox();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox).BeginInit();
      this.SuspendLayout();
      this.cancelButton.DialogResult = DialogResult.Cancel;
      this.cancelButton.FlatStyle = FlatStyle.System;
      this.cancelButton.Location = new Point(239, 190);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(75, 23);
      this.cancelButton.TabIndex = 11;
      this.cancelButton.Text = "Cancel";
      this.okButton.DialogResult = DialogResult.OK;
      this.okButton.FlatStyle = FlatStyle.System;
      this.okButton.Location = new Point(149, 190);
      this.okButton.Name = "okButton";
      this.okButton.Size = new Size(75, 23);
      this.okButton.TabIndex = 10;
      this.okButton.Text = "OK";
      this.okButton.Click += new EventHandler(this.okButton_Click);
      this.devicesCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.devicesCombo.FormattingEnabled = true;
      this.devicesCombo.Location = new Point(100, 40);
      this.devicesCombo.Name = "devicesCombo";
      this.devicesCombo.Size = new Size(325, 21);
      this.devicesCombo.TabIndex = 9;
      this.devicesCombo.SelectedIndexChanged += new EventHandler(this.devicesCombo_SelectedIndexChanged);
      this.groupBox1.Controls.Add((Control) this.videoInputsCombo);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.snapshotsLabel);
      this.groupBox1.Controls.Add((Control) this.snapshotResolutionsCombo);
      this.groupBox1.Controls.Add((Control) this.videoResolutionsCombo);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.pictureBox);
      this.groupBox1.Controls.Add((Control) this.devicesCombo);
      this.groupBox1.Location = new Point(10, 10);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(440, 165);
      this.groupBox1.TabIndex = 12;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Video capture device settings";
      this.videoInputsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.videoInputsCombo.FormattingEnabled = true;
      this.videoInputsCombo.Location = new Point(100, 130);
      this.videoInputsCombo.Name = "videoInputsCombo";
      this.videoInputsCombo.Size = new Size(150, 21);
      this.videoInputsCombo.TabIndex = 17;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(100, 115);
      this.label3.Name = "label3";
      this.label3.Size = new Size(63, 13);
      this.label3.TabIndex = 16;
      this.label3.Text = "Video input:";
      this.snapshotsLabel.AutoSize = true;
      this.snapshotsLabel.Location = new Point(275, 70);
      this.snapshotsLabel.Name = "snapshotsLabel";
      this.snapshotsLabel.Size = new Size(101, 13);
      this.snapshotsLabel.TabIndex = 15;
      this.snapshotsLabel.Text = "Snapshot resoluton:";
      this.snapshotResolutionsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.snapshotResolutionsCombo.FormattingEnabled = true;
      this.snapshotResolutionsCombo.Location = new Point(275, 85);
      this.snapshotResolutionsCombo.Name = "snapshotResolutionsCombo";
      this.snapshotResolutionsCombo.Size = new Size(150, 21);
      this.snapshotResolutionsCombo.TabIndex = 14;
      this.videoResolutionsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.videoResolutionsCombo.FormattingEnabled = true;
      this.videoResolutionsCombo.Location = new Point(100, 85);
      this.videoResolutionsCombo.Name = "videoResolutionsCombo";
      this.videoResolutionsCombo.Size = new Size(150, 21);
      this.videoResolutionsCombo.TabIndex = 13;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(100, 70);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Video resoluton:";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(100, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(72, 13);
      this.label1.TabIndex = 11;
      this.label1.Text = "Video device:";
      this.pictureBox.Image = (Image) Resources.camera;
      this.pictureBox.Location = new Point(20, 28);
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.Size = new Size(64, 64);
      this.pictureBox.TabIndex = 10;
      this.pictureBox.TabStop = false;
      this.AcceptButton = (IButtonControl) this.okButton;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cancelButton;
      this.ClientSize = new Size(462, 221);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.cancelButton);
      this.Controls.Add((Control) this.okButton);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (VideoCaptureDeviceForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Open local  video capture device";
      this.Load += new EventHandler(this.VideoCaptureDeviceForm_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((ISupportInitialize) this.pictureBox).EndInit();
      this.ResumeLayout(false);
    }

    public bool ConfigureSnapshots
    {
      get => this.configureSnapshots;
      set
      {
        this.configureSnapshots = value;
        this.snapshotsLabel.Visible = value;
        this.snapshotResolutionsCombo.Visible = value;
      }
    }

    public VideoCaptureDevice VideoDevice => this.videoDevice;

    public string VideoDeviceMoniker
    {
      get => this.videoDeviceMoniker;
      set => this.videoDeviceMoniker = value;
    }

    public Size CaptureSize
    {
      get => this.captureSize;
      set => this.captureSize = value;
    }

    public Size SnapshotSize
    {
      get => this.snapshotSize;
      set => this.snapshotSize = value;
    }

    public VideoInput VideoInput
    {
      get => this.videoInput;
      set => this.videoInput = value;
    }

    public VideoCaptureDeviceForm()
    {
      this.InitializeComponent();
      this.ConfigureSnapshots = false;
      try
      {
        this.videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        if (this.videoDevices.Count == 0)
          throw new ApplicationException();
        foreach (FilterInfo videoDevice in (CollectionBase) this.videoDevices)
          this.devicesCombo.Items.Add((object) videoDevice.Name);
      }
      catch (ApplicationException ex)
      {
        this.devicesCombo.Items.Add((object) "No local capture devices");
        this.devicesCombo.Enabled = false;
        this.okButton.Enabled = false;
      }
    }

    private void VideoCaptureDeviceForm_Load(object sender, EventArgs e)
    {
      int num = 0;
      for (int index = 0; index < this.videoDevices.Count; ++index)
      {
        if (this.videoDeviceMoniker == this.videoDevices[index].MonikerString)
        {
          num = index;
          break;
        }
      }
      this.devicesCombo.SelectedIndex = num;
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      this.videoDeviceMoniker = this.videoDevice.Source;
      if (this.videoCapabilitiesDictionary.Count != 0)
      {
        VideoCapabilities videoCapabilities = this.videoCapabilitiesDictionary[(string) this.videoResolutionsCombo.SelectedItem];
        this.videoDevice.VideoResolution = videoCapabilities;
        this.captureSize = videoCapabilities.FrameSize;
      }
      if (this.configureSnapshots && this.snapshotCapabilitiesDictionary.Count != 0)
      {
        VideoCapabilities snapshotCapabilities = this.snapshotCapabilitiesDictionary[(string) this.snapshotResolutionsCombo.SelectedItem];
        this.videoDevice.ProvideSnapshots = true;
        this.videoDevice.SnapshotResolution = snapshotCapabilities;
        this.snapshotSize = snapshotCapabilities.FrameSize;
      }
      if (this.availableVideoInputs.Length == 0)
        return;
      this.videoInput = this.availableVideoInputs[this.videoInputsCombo.SelectedIndex];
      this.videoDevice.CrossbarVideoInput = this.videoInput;
    }

    private void devicesCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.videoDevices.Count == 0)
        return;
      this.videoDevice = new VideoCaptureDevice(this.videoDevices[this.devicesCombo.SelectedIndex].MonikerString);
      this.EnumeratedSupportedFrameSizes(this.videoDevice);
    }

    private void EnumeratedSupportedFrameSizes(VideoCaptureDevice videoDevice)
    {
      this.Cursor = Cursors.WaitCursor;
      this.videoResolutionsCombo.Items.Clear();
      this.snapshotResolutionsCombo.Items.Clear();
      this.videoInputsCombo.Items.Clear();
      this.videoCapabilitiesDictionary.Clear();
      this.snapshotCapabilitiesDictionary.Clear();
      try
      {
        VideoCapabilities[] videoCapabilities1 = videoDevice.VideoCapabilities;
        int num1 = 0;
        foreach (VideoCapabilities videoCapabilities2 in videoCapabilities1)
        {
          string key = string.Format("{0} x {1}", (object) videoCapabilities2.FrameSize.Width, (object) videoCapabilities2.FrameSize.Height);
          if (!this.videoResolutionsCombo.Items.Contains((object) key))
          {
            if (this.captureSize == videoCapabilities2.FrameSize)
              num1 = this.videoResolutionsCombo.Items.Count;
            this.videoResolutionsCombo.Items.Add((object) key);
          }
          if (!this.videoCapabilitiesDictionary.ContainsKey(key))
            this.videoCapabilitiesDictionary.Add(key, videoCapabilities2);
        }
        if (videoCapabilities1.Length == 0)
          this.videoResolutionsCombo.Items.Add((object) "Not supported");
        this.videoResolutionsCombo.SelectedIndex = num1;
        if (this.configureSnapshots)
        {
          VideoCapabilities[] snapshotCapabilities = videoDevice.SnapshotCapabilities;
          int num2 = 0;
          foreach (VideoCapabilities videoCapabilities3 in snapshotCapabilities)
          {
            string key = string.Format("{0} x {1}", (object) videoCapabilities3.FrameSize.Width, (object) videoCapabilities3.FrameSize.Height);
            if (!this.snapshotResolutionsCombo.Items.Contains((object) key))
            {
              if (this.snapshotSize == videoCapabilities3.FrameSize)
                num2 = this.snapshotResolutionsCombo.Items.Count;
              this.snapshotResolutionsCombo.Items.Add((object) key);
              this.snapshotCapabilitiesDictionary.Add(key, videoCapabilities3);
            }
          }
          if (snapshotCapabilities.Length == 0)
            this.snapshotResolutionsCombo.Items.Add((object) "Not supported");
          this.snapshotResolutionsCombo.SelectedIndex = num2;
        }
        this.availableVideoInputs = videoDevice.AvailableCrossbarVideoInputs;
        int num3 = 0;
        foreach (VideoInput availableVideoInput in this.availableVideoInputs)
        {
          string str = string.Format("{0}: {1}", (object) availableVideoInput.Index, (object) availableVideoInput.Type);
          if (availableVideoInput.Index == this.videoInput.Index && availableVideoInput.Type == this.videoInput.Type)
            num3 = this.videoInputsCombo.Items.Count;
          this.videoInputsCombo.Items.Add((object) str);
        }
        if (this.availableVideoInputs.Length == 0)
          this.videoInputsCombo.Items.Add((object) "Not supported");
        this.videoInputsCombo.SelectedIndex = num3;
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }
  }
}
