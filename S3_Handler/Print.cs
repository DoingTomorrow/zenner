// Decompiled with JetBrains decompiler
// Type: S3_Handler.Print
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace S3_Handler
{
  public class Print : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private StringBuilder TypePrint;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Button buttonPrint;
    private FolderBrowserDialog folderBrowserDialog1;
    private Button buttonSelectFolder;
    private TextBox textBoxFolder;
    private Label label1;
    private CheckBox checkBoxAddSaveTime;
    private TextBox textBoxNumberOfReleaseBlocks;
    private Label labelNumberOfReleaseBlocks;

    public Print(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      if (!Directory.Exists(this.MyFunctions.PrintConfigPath))
        Directory.CreateDirectory(this.MyFunctions.PrintConfigPath);
      if (!this.MyFunctions.IsHandlerCompleteEnabled())
      {
        this.textBoxNumberOfReleaseBlocks.Visible = false;
        this.labelNumberOfReleaseBlocks.Visible = false;
        this.checkBoxAddSaveTime.Checked = true;
        this.textBoxNumberOfReleaseBlocks.Text = "0";
      }
      this.textBoxFolder.Text = this.MyFunctions.PrintConfigPath;
      this.checkBoxAddSaveTime.Checked = this.MyFunctions.PrintConfigUseDateTimeFileToken;
    }

    public bool PrintConfig(string options)
    {
      S3_AllMeters meters = this.MyFunctions.MyMeters;
      this.TypePrint = new StringBuilder();
      S3_Meter theMeter;
      if (meters.UndoCount == 0 && meters.ConnectedMeter != null)
      {
        this.TypePrint.AppendLine("*** Unchanged connected device data ***");
        this.TypePrint.AppendLine();
        theMeter = meters.ConnectedMeter;
      }
      else
      {
        this.TypePrint.AppendLine("*** Prepared data ***");
        this.TypePrint.AppendLine();
        if (this.MyFunctions.MyMeters.WorkMeter == null)
          return false;
        theMeter = this.MyFunctions.MyMeters.WorkMeter;
      }
      this.PrintBlock("Device identification");
      new S3_CommonDeviceIdentification(theMeter).PrintId(this.TypePrint);
      theMeter.MyIdentification.PrintNotStandardId(this.TypePrint);
      this.PrintBlock("Setup");
      this.MyFunctions.MyMeters.WorkMeter.PrintConfiguration(this.TypePrint);
      this.MyFunctions.MyMeters.WorkMeter.MyTransmitParameterManager.Transmitter.PrintRadioListsShortInfo(this.TypePrint);
      this.PrintBlock("LCD Menu");
      theMeter.MyFunctionManager.PrintMenu(this.TypePrint);
      this.PrintBlock("Bus and optical interface parameter lists");
      theMeter.MyTransmitParameterManager.Transmitter.PrintMBusLists(this.TypePrint);
      if (theMeter.MyIdentification.IsLoRa)
      {
        this.PrintBlock("LoRa parameters");
        this.TypePrint.AppendLine("Scenario: " + ((LoRa_TransmissionScenario) theMeter.MyParameters.ParameterByName[S3_ParameterNames.cfg_transmission_scenario.ToString()].GetByteValue()).ToString());
      }
      else
      {
        this.PrintBlock("Radio parameter lists");
        theMeter.MyTransmitParameterManager.Transmitter.PrintRadioLists(this.TypePrint);
      }
      this.PrintBlock("Logger and time events");
      theMeter.MyLoggerManager.PrintAllLoggers(this.TypePrint);
      int num = int.Parse(this.textBoxNumberOfReleaseBlocks.Text);
      for (int index = 0; index < num; ++index)
        this.PrintRelease(this.TypePrint);
      string str = this.WritePrintFile("TypeDoc_" + theMeter.MyIdentification.SAP_MaterialNumber.ToString(), this.TypePrint.ToString());
      new Process() { StartInfo = { FileName = str } }.Start();
      return true;
    }

    private void PrintRelease(StringBuilder TypePrint)
    {
      TypePrint.AppendLine();
      TypePrint.AppendLine();
      TypePrint.AppendLine("================================================================================");
      TypePrint.AppendLine("===== Released =================================================================");
      TypePrint.AppendLine("  * department: ............. ");
      TypePrint.AppendLine("  * by: .........  .......... ");
      TypePrint.AppendLine("  * at (yyyy MM dd): ........ ");
      TypePrint.AppendLine("Checked on LCD                ");
      TypePrint.AppendLine("  * identification: ......... ");
      TypePrint.AppendLine("  * menues: ................. ");
      TypePrint.AppendLine("  * units: .................. ");
      TypePrint.AppendLine("Checked by MBus/Optical reading. (DeviceCollector)");
      TypePrint.AppendLine("  * identification: ......... ");
      TypePrint.AppendLine("  * all parameters available: ");
      TypePrint.AppendLine("Checked by configuration reading. (Configurator/S3_Handler");
      TypePrint.AppendLine("  * identification: ......... ");
      TypePrint.AppendLine("  * logger chanals: ......... ");
      TypePrint.AppendLine("Checked device lable          ");
      TypePrint.AppendLine("  * identification: ......... ");
      TypePrint.AppendLine("  * settings: ............... ");
      TypePrint.AppendLine("Checked carton lable          ");
      TypePrint.AppendLine("  * identification: ......... ");
      TypePrint.AppendLine("  * settings: ............... ");
    }

    internal void PrintBlock(string blockName)
    {
      this.TypePrint.AppendLine();
      this.TypePrint.AppendLine("----------------------------------------------------------------------");
      this.TypePrint.AppendLine(blockName);
      this.TypePrint.AppendLine();
    }

    internal string WritePrintFile(string BaseName, string TheData)
    {
      DateTime now = DateTime.Now;
      string path = !this.checkBoxAddSaveTime.Checked ? Path.Combine(this.MyFunctions.PrintConfigPath, BaseName + ".txt") : Path.Combine(this.MyFunctions.PrintConfigPath, now.ToString("yyMMddHHmmss") + "_" + BaseName + ".txt");
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
      {
        streamWriter.WriteLine(BaseName + "         " + now.ToLongDateString() + " " + now.ToLongTimeString());
        streamWriter.Write(TheData);
        streamWriter.Flush();
        streamWriter.Close();
      }
      return path;
    }

    private void buttonSelectFolder_Click(object sender, EventArgs e)
    {
      this.folderBrowserDialog1.SelectedPath = this.MyFunctions.PrintConfigPath;
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.textBoxFolder.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void buttonPrint_Click(object sender, EventArgs e) => this.PrintConfig(string.Empty);

    private void textBoxFolder_TextChanged(object sender, EventArgs e)
    {
      if (Directory.Exists(this.textBoxFolder.Text))
      {
        this.textBoxFolder.BackColor = Control.DefaultBackColor;
        this.MyFunctions.PrintConfigPath = this.textBoxFolder.Text;
      }
      else
        this.textBoxFolder.BackColor = Color.LightPink;
    }

    private void checkBoxAddSaveTime_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions.PrintConfigUseDateTimeFileToken = this.checkBoxAddSaveTime.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Print));
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.buttonPrint = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.buttonSelectFolder = new Button();
      this.textBoxFolder = new TextBox();
      this.label1 = new Label();
      this.checkBoxAddSaveTime = new CheckBox();
      this.textBoxNumberOfReleaseBlocks = new TextBox();
      this.labelNumberOfReleaseBlocks = new Label();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(605, 45);
      this.zennerCoroprateDesign2.TabIndex = 16;
      this.buttonPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonPrint.Location = new Point(495, 214);
      this.buttonPrint.Name = "buttonPrint";
      this.buttonPrint.Size = new Size(98, 23);
      this.buttonPrint.TabIndex = 17;
      this.buttonPrint.Text = nameof (Print);
      this.buttonPrint.UseVisualStyleBackColor = true;
      this.buttonPrint.Click += new EventHandler(this.buttonPrint_Click);
      this.buttonSelectFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSelectFolder.Location = new Point(498, 51);
      this.buttonSelectFolder.Name = "buttonSelectFolder";
      this.buttonSelectFolder.Size = new Size(98, 23);
      this.buttonSelectFolder.TabIndex = 18;
      this.buttonSelectFolder.Text = "Select folder";
      this.buttonSelectFolder.UseVisualStyleBackColor = true;
      this.buttonSelectFolder.Click += new EventHandler(this.buttonSelectFolder_Click);
      this.textBoxFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxFolder.Location = new Point(12, 81);
      this.textBoxFolder.Name = "textBoxFolder";
      this.textBoxFolder.Size = new Size(583, 20);
      this.textBoxFolder.TabIndex = 19;
      this.textBoxFolder.TextChanged += new EventHandler(this.textBoxFolder_TextChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 62);
      this.label1.Name = "label1";
      this.label1.Size = new Size(64, 13);
      this.label1.TabIndex = 20;
      this.label1.Text = "Save folder:";
      this.checkBoxAddSaveTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.checkBoxAddSaveTime.AutoSize = true;
      this.checkBoxAddSaveTime.Checked = true;
      this.checkBoxAddSaveTime.CheckState = CheckState.Checked;
      this.checkBoxAddSaveTime.Location = new Point(354, 107);
      this.checkBoxAddSaveTime.Name = "checkBoxAddSaveTime";
      this.checkBoxAddSaveTime.RightToLeft = RightToLeft.Yes;
      this.checkBoxAddSaveTime.Size = new Size(238, 17);
      this.checkBoxAddSaveTime.TabIndex = 21;
      this.checkBoxAddSaveTime.Text = "Add save time to file name: yyMMddHHmmss";
      this.checkBoxAddSaveTime.UseVisualStyleBackColor = true;
      this.checkBoxAddSaveTime.CheckedChanged += new EventHandler(this.checkBoxAddSaveTime_CheckedChanged);
      this.textBoxNumberOfReleaseBlocks.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxNumberOfReleaseBlocks.Location = new Point(567, 131);
      this.textBoxNumberOfReleaseBlocks.Name = "textBoxNumberOfReleaseBlocks";
      this.textBoxNumberOfReleaseBlocks.Size = new Size(27, 20);
      this.textBoxNumberOfReleaseBlocks.TabIndex = 22;
      this.textBoxNumberOfReleaseBlocks.Text = "3";
      this.labelNumberOfReleaseBlocks.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labelNumberOfReleaseBlocks.AutoSize = true;
      this.labelNumberOfReleaseBlocks.Location = new Point(434, 134);
      this.labelNumberOfReleaseBlocks.Name = "labelNumberOfReleaseBlocks";
      this.labelNumberOfReleaseBlocks.RightToLeft = RightToLeft.Yes;
      this.labelNumberOfReleaseBlocks.Size = new Size((int) sbyte.MaxValue, 13);
      this.labelNumberOfReleaseBlocks.TabIndex = 23;
      this.labelNumberOfReleaseBlocks.Text = "Number of release blocks";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(605, 246);
      this.Controls.Add((Control) this.labelNumberOfReleaseBlocks);
      this.Controls.Add((Control) this.textBoxNumberOfReleaseBlocks);
      this.Controls.Add((Control) this.checkBoxAddSaveTime);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxFolder);
      this.Controls.Add((Control) this.buttonSelectFolder);
      this.Controls.Add((Control) this.buttonPrint);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Print);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Meter setup print";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
