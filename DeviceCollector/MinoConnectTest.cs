// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MinoConnectTest
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MinoConnectTest : Form
  {
    private DeviceCollectorFunctions myFunctions;
    private AsyncFunctions TestAsyncCom;
    private bool breakLoop = false;
    private IContainer components = (IContainer) null;
    private Button buttonStart;
    private Button buttonTestAsyncCom;
    private TextBox textBoxState;
    private TextBox textBoxNumberOfBytes;
    private Label label1;
    private TextBox textBoxStartBytes;
    private Label label2;
    private TextBox textBoxTestCom;
    private Label label3;
    private RadioButton radioButtonTransmit;
    private RadioButton radioButtonReceive;
    private RadioButton radioButtonRandom;

    public MinoConnectTest(DeviceCollectorFunctions myFunctions)
    {
      this.myFunctions = myFunctions;
      this.InitializeComponent();
      this.buttonStart.Enabled = false;
    }

    private void buttonTestAsyncCom_Click(object sender, EventArgs e)
    {
      if (this.TestAsyncCom == null)
        this.TestAsyncCom = new AsyncFunctions();
      int num = int.Parse(this.textBoxTestCom.Text);
      this.TestAsyncCom.SingleParameter(CommParameter.TestEcho, "false");
      this.TestAsyncCom.SingleParameter(CommParameter.Port, "COM" + num.ToString());
      this.TestAsyncCom.SingleParameter(CommParameter.Baudrate, this.myFunctions.MyCom.SingleParameter(CommParameter.Baudrate, (string) null));
      this.TestAsyncCom.SingleParameter(CommParameter.Parity, this.myFunctions.MyCom.SingleParameter(CommParameter.Parity, (string) null));
      this.TestAsyncCom.ShowComWindow();
      this.buttonStart.Enabled = true;
    }

    private void buttonStart_Click(object sender, EventArgs e)
    {
      AsyncFunctions transmitAsyncCom = this.myFunctions.MyCom as AsyncFunctions;
      AsyncFunctions asyncFunctions = this.TestAsyncCom;
      if (this.radioButtonReceive.Checked)
      {
        transmitAsyncCom = this.TestAsyncCom;
        asyncFunctions = this.myFunctions.MyCom as AsyncFunctions;
      }
      if (this.buttonStart.Text == "break")
      {
        this.breakLoop = true;
      }
      else
      {
        try
        {
          this.buttonStart.Text = "break";
          this.breakLoop = false;
          int num1 = 0;
          ZR_ClassLibMessages.ClearErrors();
          int.Parse(this.textBoxTestCom.Text);
          int length = int.Parse(this.textBoxStartBytes.Text);
          int num2 = int.Parse(this.textBoxNumberOfBytes.Text);
          this.textBoxState.Text = "Starting test. Open coms.";
          if (!asyncFunctions.Open())
            ZR_ClassLibMessages.ShowAndClearErrors();
          else if (!transmitAsyncCom.Open())
          {
            asyncFunctions.Close();
            ZR_ClassLibMessages.ShowAndClearErrors();
          }
          else
          {
            asyncFunctions.ClearCom();
            transmitAsyncCom.ClearCom();
            this.textBoxState.AppendText("\r\nComs open!");
            Random random = new Random(DateTime.Now.Millisecond);
            Queue<byte> byteQueue = new Queue<byte>();
            byte[] buffer1 = new byte[1];
            DateTime now1 = DateTime.Now;
            DateTime now2 = DateTime.Now;
            List<byte> byteList = new List<byte>();
            this.textBoxState.AppendText("\r\nStart transmitting");
            int bytePerSecound = int.Parse(this.myFunctions.MyCom.SingleParameter(CommParameter.Baudrate, (string) null)) / 11;
            byte[] numArray = new byte[length];
            random.NextBytes(numArray);
            for (int index = 0; index < length; ++index)
              byteQueue.Enqueue(numArray[index]);
            this.LoopTransmitBlock(numArray, transmitAsyncCom, bytePerSecound);
            int num3 = length;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            double num7 = 0.0;
            int num8 = 0;
            while (!this.breakLoop && num5 < num2 && num1 < 10)
            {
              if (num7 > 1000.0 || num8 > 10 || this.breakLoop)
              {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("!!!!!!! Timeout !!!!!!!   " + num5.ToString() + " received");
                stringBuilder.AppendLine("Additional send bytes:");
                while (byteQueue.Count > 0)
                {
                  stringBuilder.AppendLine();
                  for (int index = 0; index < 50 && byteQueue.Count > 0; ++index)
                  {
                    byte num9 = byteQueue.Dequeue();
                    stringBuilder.Append(num9.ToString("x02") + " ");
                    if ((char) num9 == '#')
                      stringBuilder.Append('#');
                  }
                }
                this.textBoxState.AppendText(stringBuilder.ToString());
                break;
              }
              DateTime now3 = DateTime.Now;
              byte[] buffer2;
              bool block = asyncFunctions.TryReceiveBlock(out buffer2);
              double num10 = num7;
              DateTime now4 = DateTime.Now;
              TimeSpan timeSpan1 = now4.Subtract(now3);
              double totalMilliseconds1 = timeSpan1.TotalMilliseconds;
              num7 = num10 + totalMilliseconds1;
              if (block)
              {
                byteList.Clear();
                int num11 = num3;
                for (int index = 0; index < buffer2.Length; ++index)
                {
                  ++num5;
                  byte num12 = byteQueue.Dequeue();
                  if ((int) num12 != (int) buffer2[index])
                  {
                    TextBox textBoxState = this.textBoxState;
                    string[] strArray = new string[11];
                    strArray[0] = "\r\nByte error on byte:";
                    strArray[1] = num5.ToString("d06");
                    strArray[2] = " send=0x";
                    strArray[3] = num12.ToString("x02");
                    strArray[4] = "'";
                    char ch = (char) num12;
                    strArray[5] = ch.ToString();
                    strArray[6] = "' rec=0x";
                    strArray[7] = buffer2[index].ToString("x02");
                    strArray[8] = "'";
                    ch = (char) buffer2[index];
                    strArray[9] = ch.ToString();
                    strArray[10] = "'";
                    string text = string.Concat(strArray);
                    textBoxState.AppendText(text);
                    ++num8;
                  }
                  num7 = 0.0;
                  if (num11 < num2)
                  {
                    random.NextBytes(buffer1);
                    byteQueue.Enqueue(buffer1[0]);
                    byteList.Add(buffer1[0]);
                    ++num11;
                  }
                  if (this.breakLoop || num8 > 10)
                    break;
                }
                Application.DoEvents();
                now4 = DateTime.Now;
                timeSpan1 = now4.Subtract(now2);
                double totalMilliseconds2 = timeSpan1.TotalMilliseconds;
                int num13 = 0;
                if (totalMilliseconds2 > 0.0 && totalMilliseconds2 < (double) int.MaxValue)
                  num13 = Convert.ToInt32(totalMilliseconds2);
                int num14 = num5 - num6;
                int num15 = num3 - num4;
                int num16 = num3 - num5;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine();
                stringBuilder.Append("msTimeDiff:" + num13.ToString());
                stringBuilder.Append("  Transmitted: " + num15.ToString() + " (" + num3.ToString() + ")");
                stringBuilder.Append("  Received:" + num14.ToString() + " (" + num5.ToString() + ")");
                stringBuilder.Append("  On the way:" + num16.ToString());
                this.textBoxState.AppendText(stringBuilder.ToString());
                num4 = num3;
                num6 = num5;
                now2 = DateTime.Now;
                if (num5 == num2)
                {
                  this.textBoxState.AppendText("\r\nok! All bytes received");
                  now4 = DateTime.Now;
                  TimeSpan timeSpan2 = now4.Subtract(now1);
                  double num17 = double.Parse(this.myFunctions.MyCom.SingleParameter(CommParameter.Baudrate, (string) null));
                  double num18 = (double) num2 * 11.0 / num17;
                  this.textBoxState.AppendText("\r\nUsed time[s]: " + timeSpan2.TotalSeconds.ToString() + "   extimated time[s]: " + num18.ToString());
                }
                else
                {
                  this.LoopTransmitBlock(byteList.ToArray(), transmitAsyncCom, bytePerSecound);
                  num3 += byteList.Count;
                }
              }
            }
            transmitAsyncCom.Close();
            asyncFunctions.Close();
            this.textBoxState.AppendText("\r\nComs closed!");
          }
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddException(ex);
          ZR_ClassLibMessages.ShowAndClearErrors();
        }
        finally
        {
          this.buttonStart.Text = "Start";
          ZR_ClassLibMessages.ShowAndClearErrors();
          if (this.breakLoop)
          {
            this.textBoxState.AppendText("\r\nmanual break");
            this.breakLoop = false;
          }
        }
      }
    }

    private void LoopTransmitBlock(
      byte[] completeBlock,
      AsyncFunctions transmitAsyncCom,
      int bytePerSecound)
    {
      int num = 0;
      while (num < completeBlock.Length)
      {
        int length = completeBlock.Length - num;
        if (length > bytePerSecound)
          length = bytePerSecound;
        byte[] byteList = new byte[length];
        for (int index = 0; index < length; ++index)
          byteList[index] = completeBlock[index + num];
        transmitAsyncCom.PureTransmit(byteList);
        num += length;
        this.textBoxState.AppendText("\r\nBytes transmitted: " + num.ToString());
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
      this.buttonStart = new Button();
      this.buttonTestAsyncCom = new Button();
      this.textBoxState = new TextBox();
      this.textBoxNumberOfBytes = new TextBox();
      this.label1 = new Label();
      this.textBoxStartBytes = new TextBox();
      this.label2 = new Label();
      this.textBoxTestCom = new TextBox();
      this.label3 = new Label();
      this.radioButtonTransmit = new RadioButton();
      this.radioButtonReceive = new RadioButton();
      this.radioButtonRandom = new RadioButton();
      this.SuspendLayout();
      this.buttonStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonStart.Location = new Point(998, 601);
      this.buttonStart.Margin = new Padding(4);
      this.buttonStart.Name = "buttonStart";
      this.buttonStart.Size = new Size(164, 28);
      this.buttonStart.TabIndex = 0;
      this.buttonStart.Text = "Start";
      this.buttonStart.UseVisualStyleBackColor = true;
      this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
      this.buttonTestAsyncCom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonTestAsyncCom.Location = new Point(16, 605);
      this.buttonTestAsyncCom.Margin = new Padding(4);
      this.buttonTestAsyncCom.Name = "buttonTestAsyncCom";
      this.buttonTestAsyncCom.Size = new Size(164, 28);
      this.buttonTestAsyncCom.TabIndex = 0;
      this.buttonTestAsyncCom.Text = "InitTestAsyncCom";
      this.buttonTestAsyncCom.UseVisualStyleBackColor = true;
      this.buttonTestAsyncCom.Click += new System.EventHandler(this.buttonTestAsyncCom_Click);
      this.textBoxState.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxState.Font = new Font("Courier New", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxState.Location = new Point(16, 206);
      this.textBoxState.Margin = new Padding(4);
      this.textBoxState.Multiline = true;
      this.textBoxState.Name = "textBoxState";
      this.textBoxState.ScrollBars = ScrollBars.Both;
      this.textBoxState.Size = new Size(1145, 387);
      this.textBoxState.TabIndex = 1;
      this.textBoxState.WordWrap = false;
      this.textBoxNumberOfBytes.Location = new Point(169, 44);
      this.textBoxNumberOfBytes.Margin = new Padding(4);
      this.textBoxNumberOfBytes.Name = "textBoxNumberOfBytes";
      this.textBoxNumberOfBytes.Size = new Size(132, 22);
      this.textBoxNumberOfBytes.TabIndex = 2;
      this.textBoxNumberOfBytes.Text = "10000";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(19, 46);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(106, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Number of bytes";
      this.textBoxStartBytes.Location = new Point(169, 76);
      this.textBoxStartBytes.Margin = new Padding(4);
      this.textBoxStartBytes.Name = "textBoxStartBytes";
      this.textBoxStartBytes.Size = new Size(132, 22);
      this.textBoxStartBytes.TabIndex = 2;
      this.textBoxStartBytes.Text = "500";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(19, 78);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(71, 16);
      this.label2.TabIndex = 3;
      this.label2.Text = "Start bytes";
      this.textBoxTestCom.Location = new Point(169, 12);
      this.textBoxTestCom.Margin = new Padding(4);
      this.textBoxTestCom.Name = "textBoxTestCom";
      this.textBoxTestCom.Size = new Size(132, 22);
      this.textBoxTestCom.TabIndex = 2;
      this.textBoxTestCom.Text = "1";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(19, 14);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(68, 16);
      this.label3.TabIndex = 3;
      this.label3.Text = "Test COM";
      this.radioButtonTransmit.AutoSize = true;
      this.radioButtonTransmit.Checked = true;
      this.radioButtonTransmit.Location = new Point(374, 13);
      this.radioButtonTransmit.Name = "radioButtonTransmit";
      this.radioButtonTransmit.Size = new Size(177, 20);
      this.radioButtonTransmit.TabIndex = 4;
      this.radioButtonTransmit.TabStop = true;
      this.radioButtonTransmit.Text = "Transmit by MinoConnect";
      this.radioButtonTransmit.UseVisualStyleBackColor = true;
      this.radioButtonReceive.AutoSize = true;
      this.radioButtonReceive.Location = new Point(374, 39);
      this.radioButtonReceive.Name = "radioButtonReceive";
      this.radioButtonReceive.Size = new Size(176, 20);
      this.radioButtonReceive.TabIndex = 4;
      this.radioButtonReceive.Text = "Receive by MinoConnect";
      this.radioButtonReceive.UseVisualStyleBackColor = true;
      this.radioButtonRandom.AutoSize = true;
      this.radioButtonRandom.Location = new Point(374, 65);
      this.radioButtonRandom.Name = "radioButtonRandom";
      this.radioButtonRandom.Size = new Size(126, 20);
      this.radioButtonRandom.TabIndex = 4;
      this.radioButtonRandom.Text = "random direction";
      this.radioButtonRandom.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1182, 648);
      this.Controls.Add((Control) this.radioButtonRandom);
      this.Controls.Add((Control) this.radioButtonReceive);
      this.Controls.Add((Control) this.radioButtonTransmit);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxStartBytes);
      this.Controls.Add((Control) this.textBoxTestCom);
      this.Controls.Add((Control) this.textBoxNumberOfBytes);
      this.Controls.Add((Control) this.textBoxState);
      this.Controls.Add((Control) this.buttonTestAsyncCom);
      this.Controls.Add((Control) this.buttonStart);
      this.Margin = new Padding(4);
      this.Name = nameof (MinoConnectTest);
      this.Text = nameof (MinoConnectTest);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
