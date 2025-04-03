// Decompiled with JetBrains decompiler
// Type: S3_Handler.TypeChecker
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  public class TypeChecker : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private DiagnoseWindow diagnoseWindow;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Button buttonSelectType;
    private Button buttonCheckType;
    private TextBox textBoxSelectedType;
    private Label label1;

    public TypeChecker(S3_HandlerFunctions MyFunctions, DiagnoseWindow diagnoseWindow)
    {
      this.MyFunctions = MyFunctions;
      this.diagnoseWindow = diagnoseWindow;
      this.InitializeComponent();
    }

    private void buttonSelectType_Click(object sender, EventArgs e)
    {
      this.textBoxSelectedType.Clear();
      OpenType openType = new OpenType(this.MyFunctions);
      openType.GetOnlySelectedInfos = true;
      if (openType.ShowDialog() != DialogResult.OK)
        return;
      if (!int.TryParse(openType.SAP_Number, out int _))
      {
        int num = (int) GMM_MessageBox.ShowMessage("Type checker", "Not a type. (BaseType?)", true);
      }
      else
        this.textBoxSelectedType.Text = openType.SAP_Number;
    }

    private void buttonCheckType_Click(object sender, EventArgs e)
    {
      try
      {
        this.MyFunctions.Clear();
        string SQLCommand = "SELECT * FROM MeterInfo WHERE PPSArtikelNr = '" + this.textBoxSelectedType.Text + "'";
        Schema.MeterInfoDataTable Table = new Schema.MeterInfoDataTable();
        this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, (DataTable) Table);
        if (Table.Rows.Count != 1)
        {
          int num = (int) GMM_MessageBox.ShowMessage("Type checker", "Type not found", true);
        }
        else
        {
          int meterInfoId = Table[0].MeterInfoID;
          ZR_ClassLibMessages.ClearErrors();
          this.MyFunctions.MyMeters.OpenType(meterInfoId);
          if (ZR_ClassLibMessages.GetLastError() != 0)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load type error");
            ZR_ClassLibMessages.ShowAndClearErrors();
          }
          else
          {
            string blockPrint1 = this.diagnoseWindow.GetBlockPrint(this.MyFunctions.MyMeters.TypeMeter);
            string blockPrint2 = this.diagnoseWindow.GetBlockPrint(this.MyFunctions.MyMeters.WorkMeter);
            string str1 = this.diagnoseWindow.WriteInfoFile("Loaded type", blockPrint1);
            string str2 = this.diagnoseWindow.WriteInfoFile("Type clone", blockPrint2);
            Process process1 = new Process();
            if (this.diagnoseWindow.checkBoxUseWinDiff.Checked)
            {
              process1.StartInfo.FileName = "WinDiff";
              process1.StartInfo.Arguments = "\"" + str1 + "\" \"" + str2 + "\"";
            }
            else
            {
              process1.StartInfo.FileName = "TortoiseMerge";
              process1.StartInfo.Arguments = "/base:\"" + str1 + "\" /theirs:\"" + str2 + "\"";
            }
            process1.Start();
            ZR_ClassLibMessages.ClearErrors();
            SortedList<OverrideID, ConfigurationParameter>[] parameterLists = new SortedList<OverrideID, ConfigurationParameter>[4]
            {
              new SortedList<OverrideID, ConfigurationParameter>(),
              null,
              null,
              null
            };
            this.MyFunctions.MyMeters.WorkMeter.GetConfigurationParameters(0, parameterLists[0], true);
            parameterLists[1] = new SortedList<OverrideID, ConfigurationParameter>();
            this.MyFunctions.MyMeters.WorkMeter.GetConfigurationParameters(1, parameterLists[1], true);
            parameterLists[2] = new SortedList<OverrideID, ConfigurationParameter>();
            this.MyFunctions.MyMeters.WorkMeter.GetConfigurationParameters(2, parameterLists[2], true);
            parameterLists[3] = new SortedList<OverrideID, ConfigurationParameter>();
            this.MyFunctions.MyMeters.WorkMeter.GetConfigurationParameters(3, parameterLists[3], true);
            this.MyFunctions.MyMeters.SetAllConfigurationParameter(parameterLists, true);
            if (ZR_ClassLibMessages.GetLastError() != 0)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load type error");
              ZR_ClassLibMessages.ShowAndClearErrors();
            }
            else
            {
              string blockPrint3 = this.diagnoseWindow.GetBlockPrint(this.MyFunctions.MyMeters.WorkMeter);
              string str3 = this.diagnoseWindow.WriteInfoFile("Type clone", blockPrint2);
              string str4 = this.diagnoseWindow.WriteInfoFile("AfterConfig", blockPrint3);
              Process process2 = new Process();
              if (this.diagnoseWindow.checkBoxUseWinDiff.Checked)
              {
                process2.StartInfo.FileName = "WinDiff";
                process2.StartInfo.Arguments = "\"" + str3 + "\" \"" + str4 + "\"";
              }
              else
              {
                process2.StartInfo.FileName = "TortoiseMerge";
                process2.StartInfo.Arguments = "/base:\"" + str3 + "\" /theirs:\"" + str4 + "\"";
              }
              process2.Start();
            }
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read type data");
        ZR_ClassLibMessages.ShowAndClearErrors();
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
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.buttonSelectType = new Button();
      this.buttonCheckType = new Button();
      this.textBoxSelectedType = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(899, 39);
      this.zennerCoroprateDesign2.TabIndex = 17;
      this.buttonSelectType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSelectType.Location = new Point(651, 99);
      this.buttonSelectType.Name = "buttonSelectType";
      this.buttonSelectType.Size = new Size(236, 23);
      this.buttonSelectType.TabIndex = 18;
      this.buttonSelectType.Text = "Select type";
      this.buttonSelectType.UseVisualStyleBackColor = true;
      this.buttonSelectType.Click += new System.EventHandler(this.buttonSelectType_Click);
      this.buttonCheckType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCheckType.Location = new Point(651, 128);
      this.buttonCheckType.Name = "buttonCheckType";
      this.buttonCheckType.Size = new Size(236, 23);
      this.buttonCheckType.TabIndex = 18;
      this.buttonCheckType.Text = "Check type";
      this.buttonCheckType.UseVisualStyleBackColor = true;
      this.buttonCheckType.Click += new System.EventHandler(this.buttonCheckType_Click);
      this.textBoxSelectedType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxSelectedType.Location = new Point(734, 56);
      this.textBoxSelectedType.Name = "textBoxSelectedType";
      this.textBoxSelectedType.Size = new Size(153, 20);
      this.textBoxSelectedType.TabIndex = 19;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(662, 59);
      this.label1.Name = "label1";
      this.label1.Size = new Size(66, 13);
      this.label1.TabIndex = 20;
      this.label1.Text = "SAP number";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(899, 455);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxSelectedType);
      this.Controls.Add((Control) this.buttonCheckType);
      this.Controls.Add((Control) this.buttonSelectType);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (TypeChecker);
      this.Text = nameof (TypeChecker);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
