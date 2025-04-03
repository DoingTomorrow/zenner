// Decompiled with JetBrains decompiler
// Type: AsyncCom.MBusFrameTestWindow
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace AsyncCom
{
  public class MBusFrameTestWindow : Form
  {
    private Queue<byte> ByteList;
    private IContainer components = (IContainer) null;
    private TextBox textBoxReceiveFrame;
    private Button buttonOk;

    public MBusFrameTestWindow()
    {
      this.InitializeComponent();
      this.ByteList = new Queue<byte>();
    }

    internal int Read(byte[] buffer, int offset, int count)
    {
      if (this.ByteList.Count < count)
      {
        this.ByteList.Clear();
        this.DialogResult = DialogResult.Cancel;
        if (this.ShowDialog() != DialogResult.OK)
          throw new Exception("Not enough bytes");
        string[] strArray1 = this.textBoxReceiveFrame.Text.Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int index1 = 0; index1 < strArray1.Length; ++index1)
        {
          string empty = string.Empty;
          int startIndex = strArray1[index1].IndexOf('|') + 1;
          string[] strArray2 = (startIndex < 1 || strArray1[index1].Length <= startIndex ? strArray1[index1] : strArray1[index1].Substring(startIndex)).Replace('.', ' ').Split(' ');
          for (int index2 = 0; index2 < strArray2.Length; ++index2)
          {
            if (strArray2[index2].Length > 0)
              this.ByteList.Enqueue(byte.Parse(strArray2[index2], NumberStyles.HexNumber));
          }
        }
        this.textBoxReceiveFrame.Clear();
      }
      for (int index = 0; index < count; ++index)
      {
        if (this.ByteList.Count == 0)
          return index;
        buffer[offset++] = this.ByteList.Dequeue();
      }
      return count;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBoxReceiveFrame = new TextBox();
      this.buttonOk = new Button();
      this.SuspendLayout();
      this.textBoxReceiveFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxReceiveFrame.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxReceiveFrame.Location = new Point(12, 12);
      this.textBoxReceiveFrame.Multiline = true;
      this.textBoxReceiveFrame.Name = "textBoxReceiveFrame";
      this.textBoxReceiveFrame.Size = new Size(569, 286);
      this.textBoxReceiveFrame.TabIndex = 0;
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.DialogResult = DialogResult.OK;
      this.buttonOk.Location = new Point(506, 329);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(75, 23);
      this.buttonOk.TabIndex = 1;
      this.buttonOk.Text = "Ok";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(593, 374);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.textBoxReceiveFrame);
      this.Name = nameof (MBusFrameTestWindow);
      this.Text = nameof (MBusFrameTestWindow);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
