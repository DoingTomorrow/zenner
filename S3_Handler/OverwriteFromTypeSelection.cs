// Decompiled with JetBrains decompiler
// Type: S3_Handler.OverwriteFromTypeSelection
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class OverwriteFromTypeSelection : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private StringBuilder SelectionInfo;
    private string SelectionLineStartString;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private CheckedListBox checkedListBoxParts;
    private Button buttonOverwriteWorkData;
    private Button buttonSelectAll;
    private Button buttonSelectFactoryTypeProgramming;
    private Button buttonDeselectAll;
    private Button buttonExportSelectionInfo;

    public OverwriteFromTypeSelection(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      this.checkedListBoxParts.Items.AddRange((object[]) Enum.GetNames(typeof (OverwriteSelections)));
      this.checkedListBoxParts.Items.RemoveAt(21);
    }

    private void buttonOverwriteWorkData_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      bool[] OverwriteSelection = new bool[21];
      for (int index = 0; index < OverwriteSelection.Length; ++index)
        OverwriteSelection[index] = this.checkedListBoxParts.GetItemChecked(index);
      if (this.MyFunctions.OverwriteWorkFromType(OverwriteSelection))
        return;
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonSelectAll_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.checkedListBoxParts.Items.Count; ++index)
        this.checkedListBoxParts.SetItemChecked(index, true);
    }

    private void buttonSelectFactoryTypeProgramming_Click(object sender, EventArgs e)
    {
      this.deselectAll();
      for (int index = 0; index < this.checkedListBoxParts.Items.Count; ++index)
        this.checkedListBoxParts.SetItemChecked(index, S3_HandlerFunctions.OverwriteFromType_On_ProductionTypeProgramming[index]);
    }

    private void buttonDeselectAll_Click(object sender, EventArgs e) => this.deselectAll();

    private void deselectAll()
    {
      for (int index = 0; index < this.checkedListBoxParts.Items.Count; ++index)
        this.checkedListBoxParts.SetItemChecked(index, false);
    }

    private void buttonExportSelectionInfo_Click(object sender, EventArgs e)
    {
      this.SelectionInfo = new StringBuilder();
      this.SelectionInfo.AppendLine("Overwrite selection info");
      this.SelectionInfo.AppendLine();
      this.SelectionLineStartString = "   ";
      S3_DeviceId identification = (S3_DeviceId) this.MyFunctions.MyMeters.TypeMeter.MyIdentification;
      this.SelectionInfo.AppendLine("TypeInformation:");
      this.WriteParameterLine("MeterTypeId", (ulong) identification.MeterTypeId);
      this.WriteParameterLine("Version", identification.FirmwareVersionString);
      this.WriteParameterLine("MapId", (ulong) identification.MapId);
      this.SelectionInfo.AppendLine();
      this.SelectionInfo.AppendLine("Selected groups:");
      string[] names = Enum.GetNames(typeof (OverwriteSelections));
      bool[] flagArray = new bool[21];
      for (int index = 0; index < flagArray.Length; ++index)
      {
        flagArray[index] = this.checkedListBoxParts.GetItemChecked(index);
        if (flagArray[index])
          this.SelectionInfo.AppendLine(this.SelectionLineStartString + names[index]);
      }
      this.SelectionInfo.AppendLine();
      this.SelectionInfo.AppendLine("-------------------------------------------------------------------");
      this.SelectionInfo.AppendLine("Groups sorted by name");
      for (int theSelection = 0; theSelection < flagArray.Length; ++theSelection)
      {
        if (flagArray[theSelection])
        {
          this.SelectionInfo.AppendLine();
          this.SelectionInfo.AppendLine("Group name: " + names[theSelection]);
          this.WriteOverwriteSelectionInfo((OverwriteSelections) theSelection);
        }
      }
      this.WriteInfoFile();
    }

    private void WriteOverwriteSelectionInfo(OverwriteSelections theSelection)
    {
      switch (theSelection)
      {
        case OverwriteSelections.TimeSettings:
          this.WriteParameterList(OverwriteWorkMeter.TimeSettingsParameterList);
          break;
        case OverwriteSelections.CycleSettings:
          this.WriteParameterList(OverwriteWorkMeter.CycleSettingsList);
          break;
        case OverwriteSelections.TypeIdentificationParameters:
          this.SelectionInfo.AppendLine(this.SelectionLineStartString + "--- protected");
          this.WriteParameterList(OverwriteWorkMeter.TypeIdentificationParametersList_Protected);
          this.SelectionInfo.AppendLine(this.SelectionLineStartString + "--- not protected");
          this.WriteParameterList(OverwriteWorkMeter.TypeVariantParametersList_NotProtected);
          break;
        case OverwriteSelections.DeviceIdentificationParameters:
          this.SelectionInfo.AppendLine(this.SelectionLineStartString + "--- protected");
          this.WriteParameterList(OverwriteWorkMeter.DeviceIdentificationParametersList_Protected);
          this.SelectionInfo.AppendLine(this.SelectionLineStartString + "--- not protected");
          this.WriteParameterList(OverwriteWorkMeter.DeviceIdentificationParametersList_NotProtected);
          break;
        case OverwriteSelections.VolumeParameter:
          this.WriteParameterList(OverwriteWorkMeter.VolumeParameterList);
          break;
        case OverwriteSelections.EnergyParameter:
          this.WriteParameterList(OverwriteWorkMeter.EnergyParameterList);
          break;
        case OverwriteSelections.IO_Settings:
          this.WriteParameterList(OverwriteWorkMeter.IO_SettingsList);
          break;
        case OverwriteSelections.AccumulatedValues:
          this.WriteParameterList(OverwriteWorkMeter.AccumulatedValuesList);
          break;
        case OverwriteSelections.UltrasonicSettings:
          this.WriteParameterList(OverwriteWorkMeter.UltrasonicSettingsList);
          break;
        case OverwriteSelections.TemperatureSettings:
          this.WriteParameterList(OverwriteWorkMeter.TemperatureParameterList);
          break;
        case OverwriteSelections.TemperatureLimits:
          this.WriteParameterList(OverwriteWorkMeter.TemperatureLimitsParameterList);
          break;
        case OverwriteSelections.ResetAccumulatedValues:
          this.WriteParameterList(OverwriteWorkMeter.AccumulatedValuesList);
          break;
      }
    }

    private void WriteParameterList(string[] parameterList)
    {
      SortedList<string, string> sortedList = new SortedList<string, string>();
      for (int index = 0; index < parameterList.Length; ++index)
        sortedList.Add(parameterList[index], (string) null);
      foreach (string key in (IEnumerable<string>) sortedList.Keys)
      {
        int index = this.MyFunctions.MyMeters.TypeMeter.MyParameters.ParameterByName.IndexOfKey(key);
        if (index >= 0)
          this.SelectionInfo.AppendLine(this.SelectionLineStartString + this.MyFunctions.MyMeters.TypeMeter.MyParameters.ParameterByName.Values[index].ToString());
        else
          this.WriteParameterLine(key, "");
      }
    }

    private void WriteParameterLine(string name, ulong value)
    {
      this.SelectionInfo.AppendLine(this.SelectionLineStartString + name + ": " + value.ToString() + " = 0x" + value.ToString("x"));
    }

    private void WriteParameterLine(string name, string value)
    {
      this.SelectionInfo.AppendLine(this.SelectionLineStartString + name + ": " + value);
    }

    internal void WriteInfoFile()
    {
      string loggDataPath = SystemValues.LoggDataPath;
      DateTime now = DateTime.Now;
      string path = Path.Combine(loggDataPath, now.ToString("yyMMddHHmmss") + "_OverwriteGroupLog_.txt");
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.ASCII))
      {
        streamWriter.WriteLine("S3_Handler diagnostic                     " + now.ToLongDateString() + " " + now.ToLongTimeString());
        streamWriter.WriteLine();
        streamWriter.Write(this.SelectionInfo.ToString());
        streamWriter.Flush();
        streamWriter.Close();
      }
      new Process() { StartInfo = { FileName = path } }.Start();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OverwriteFromTypeSelection));
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.checkedListBoxParts = new CheckedListBox();
      this.buttonOverwriteWorkData = new Button();
      this.buttonSelectAll = new Button();
      this.buttonSelectFactoryTypeProgramming = new Button();
      this.buttonDeselectAll = new Button();
      this.buttonExportSelectionInfo = new Button();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(620, 45);
      this.zennerCoroprateDesign2.TabIndex = 16;
      this.checkedListBoxParts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.checkedListBoxParts.CheckOnClick = true;
      this.checkedListBoxParts.Location = new Point(12, 50);
      this.checkedListBoxParts.Name = "checkedListBoxParts";
      this.checkedListBoxParts.Size = new Size(317, 319);
      this.checkedListBoxParts.TabIndex = 17;
      this.buttonOverwriteWorkData.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOverwriteWorkData.DialogResult = DialogResult.OK;
      this.buttonOverwriteWorkData.Location = new Point(335, 344);
      this.buttonOverwriteWorkData.Name = "buttonOverwriteWorkData";
      this.buttonOverwriteWorkData.Size = new Size(270, 23);
      this.buttonOverwriteWorkData.TabIndex = 18;
      this.buttonOverwriteWorkData.Text = "Overwrite work data";
      this.buttonOverwriteWorkData.UseVisualStyleBackColor = true;
      this.buttonOverwriteWorkData.Click += new System.EventHandler(this.buttonOverwriteWorkData_Click);
      this.buttonSelectAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSelectAll.Location = new Point(337, 192);
      this.buttonSelectAll.Name = "buttonSelectAll";
      this.buttonSelectAll.Size = new Size(270, 23);
      this.buttonSelectAll.TabIndex = 18;
      this.buttonSelectAll.Text = "Select all";
      this.buttonSelectAll.UseVisualStyleBackColor = true;
      this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
      this.buttonSelectFactoryTypeProgramming.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSelectFactoryTypeProgramming.Location = new Point(335, 276);
      this.buttonSelectFactoryTypeProgramming.Name = "buttonSelectFactoryTypeProgramming";
      this.buttonSelectFactoryTypeProgramming.Size = new Size(270, 23);
      this.buttonSelectFactoryTypeProgramming.TabIndex = 18;
      this.buttonSelectFactoryTypeProgramming.Text = "Select factory type programming";
      this.buttonSelectFactoryTypeProgramming.UseVisualStyleBackColor = true;
      this.buttonSelectFactoryTypeProgramming.Click += new System.EventHandler(this.buttonSelectFactoryTypeProgramming_Click);
      this.buttonDeselectAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonDeselectAll.Location = new Point(337, 221);
      this.buttonDeselectAll.Name = "buttonDeselectAll";
      this.buttonDeselectAll.Size = new Size(270, 23);
      this.buttonDeselectAll.TabIndex = 18;
      this.buttonDeselectAll.Text = "Deselect all";
      this.buttonDeselectAll.UseVisualStyleBackColor = true;
      this.buttonDeselectAll.Click += new System.EventHandler(this.buttonDeselectAll_Click);
      this.buttonExportSelectionInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonExportSelectionInfo.Location = new Point(335, 50);
      this.buttonExportSelectionInfo.Name = "buttonExportSelectionInfo";
      this.buttonExportSelectionInfo.Size = new Size(270, 23);
      this.buttonExportSelectionInfo.TabIndex = 18;
      this.buttonExportSelectionInfo.Text = "Export selection info";
      this.buttonExportSelectionInfo.UseVisualStyleBackColor = true;
      this.buttonExportSelectionInfo.Click += new System.EventHandler(this.buttonExportSelectionInfo_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(619, 382);
      this.Controls.Add((Control) this.buttonSelectFactoryTypeProgramming);
      this.Controls.Add((Control) this.buttonDeselectAll);
      this.Controls.Add((Control) this.buttonExportSelectionInfo);
      this.Controls.Add((Control) this.buttonSelectAll);
      this.Controls.Add((Control) this.buttonOverwriteWorkData);
      this.Controls.Add((Control) this.checkedListBoxParts);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (OverwriteFromTypeSelection);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Overwrite from type";
      this.ResumeLayout(false);
    }
  }
}
