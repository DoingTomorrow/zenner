// Decompiled with JetBrains decompiler
// Type: S3_Handler.EditRadioScenarioNameView
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace S3_Handler
{
  public class EditRadioScenarioNameView : Form
  {
    private IContainer components = (IContainer) null;
    private Button btnSetScenarioName;
    private TextBox txtBxSetScenarioName;

    public EditRadioScenarioNameView() => this.InitializeComponent();

    internal static string Show(Form owner, string oldName)
    {
      using (EditRadioScenarioNameView scenarioNameView = new EditRadioScenarioNameView())
      {
        scenarioNameView.Owner = owner;
        scenarioNameView.txtBxSetScenarioName.Text = oldName;
        return scenarioNameView.ShowDialog() == DialogResult.OK ? scenarioNameView.txtBxSetScenarioName.Text : oldName;
      }
    }

    private void btnSetScenarioName_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnSetScenarioName = new Button();
      this.txtBxSetScenarioName = new TextBox();
      this.SuspendLayout();
      this.btnSetScenarioName.Location = new Point(194, 21);
      this.btnSetScenarioName.Name = "btnSetScenarioName";
      this.btnSetScenarioName.Size = new Size(75, 23);
      this.btnSetScenarioName.TabIndex = 0;
      this.btnSetScenarioName.Text = "Set ";
      this.btnSetScenarioName.UseVisualStyleBackColor = true;
      this.btnSetScenarioName.Click += new EventHandler(this.btnSetScenarioName_Click);
      this.txtBxSetScenarioName.Location = new Point(12, 23);
      this.txtBxSetScenarioName.Name = "txtBxSetScenarioName";
      this.txtBxSetScenarioName.Size = new Size(176, 20);
      this.txtBxSetScenarioName.TabIndex = 1;
      this.AcceptButton = (IButtonControl) this.btnSetScenarioName;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(281, 57);
      this.Controls.Add((Control) this.txtBxSetScenarioName);
      this.Controls.Add((Control) this.btnSetScenarioName);
      this.Name = nameof (EditRadioScenarioNameView);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Edit Scenario Name";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
