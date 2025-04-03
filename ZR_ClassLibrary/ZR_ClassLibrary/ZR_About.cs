// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ZR_About
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ZR_About : Form
  {
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private TextBox textBoxAbout;
    private Button buttonCopyTextToClipboard;
    private TextBox textBoxInfos;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public ZR_About(ArrayList FullNames)
    {
      this.InitializeComponent();
      string path = Path.Combine(Application.StartupPath, "About.txt");
      try
      {
        using (StreamReader streamReader = new StreamReader(path))
        {
          while (true)
          {
            string str = streamReader.ReadLine();
            if (str != null)
              this.textBoxAbout.AppendText(str + Environment.NewLine);
            else
              break;
          }
          streamReader.Close();
        }
      }
      catch
      {
      }
      StringBuilder stringBuilder = new StringBuilder(5000);
      stringBuilder.AppendLine("Metrological core: " + this.GetFileChecksum("DeviceCollector"));
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("------------------------------------------------------------");
      stringBuilder.AppendLine("Assambly versions:");
      stringBuilder.AppendLine();
      foreach (string fullName in FullNames)
      {
        DateTime dateTime = new DateTime(2000, 1, 1);
        string Name;
        int Version;
        string VersionString;
        int Revision;
        DateTime BuildTime;
        if (ParameterService.GetAssamblyVersionParts(fullName, out Name, out Version, out VersionString, out Revision, out BuildTime, out int _))
        {
          stringBuilder.Append(Name.PadRight(40, '.'));
          stringBuilder.Append("Version: ");
          if (!Name.StartsWith("System") && BuildTime < DateTime.Now && BuildTime != DateTime.MinValue && BuildTime != dateTime)
          {
            stringBuilder.Append(Version.ToString("d02"));
            stringBuilder.Append('.');
            stringBuilder.Append(Revision.ToString("d02"));
            stringBuilder.Append("  Build time: ");
            stringBuilder.Append(BuildTime.ToString("dd.MM.yyyy HH:mm"));
          }
          else
            stringBuilder.Append(VersionString);
          stringBuilder.AppendLine();
        }
      }
      this.textBoxInfos.Text = stringBuilder.ToString();
    }

    private string GetFileChecksum(string filename)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 35678;
      stringBuilder.Append(num.ToString("X04"));
      for (int index = 0; index < 7; ++index)
      {
        stringBuilder.Append('-');
        num *= 3;
        num += 12624;
        num &= (int) ushort.MaxValue;
        stringBuilder.Append(num.ToString("X04"));
      }
      return stringBuilder.ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ZR_About));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.textBoxAbout = new TextBox();
      this.buttonCopyTextToClipboard = new Button();
      this.textBoxInfos = new TextBox();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this.textBoxAbout, "textBoxAbout");
      this.textBoxAbout.BackColor = SystemColors.Control;
      this.textBoxAbout.BorderStyle = BorderStyle.None;
      this.textBoxAbout.ForeColor = Color.FromArgb(0, 0, 192);
      this.textBoxAbout.Name = "textBoxAbout";
      componentResourceManager.ApplyResources((object) this.buttonCopyTextToClipboard, "buttonCopyTextToClipboard");
      this.buttonCopyTextToClipboard.Name = "buttonCopyTextToClipboard";
      this.buttonCopyTextToClipboard.UseVisualStyleBackColor = true;
      this.buttonCopyTextToClipboard.Click += new System.EventHandler(this.buttonCopyTextToClipboard_Click);
      componentResourceManager.ApplyResources((object) this.textBoxInfos, "textBoxInfos");
      this.textBoxInfos.BackColor = SystemColors.ControlLightLight;
      this.textBoxInfos.Name = "textBoxInfos";
      this.textBoxInfos.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.textBoxInfos);
      this.Controls.Add((Control) this.buttonCopyTextToClipboard);
      this.Controls.Add((Control) this.textBoxAbout);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Name = nameof (ZR_About);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
    }

    private void buttonCopyTextToClipboard_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.textBoxAbout.Text);
      stringBuilder.AppendLine(this.textBoxInfos.Text);
      Clipboard.SetDataObject((object) stringBuilder.ToString());
    }
  }
}
