// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GMM_Help
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class GMM_Help
  {
    public static GMM_Help TheHelp;
    private Process MyHelpProcess = new Process();
    private string BasicHelpPath;
    private string BasicHelpPathEn;
    private string BasicHelpPathDe;

    public GMM_Help(string Language)
    {
      this.BasicHelpPath = Path.Combine(SystemValues.AppPath, nameof (GMM_Help));
      if (!Directory.Exists(this.BasicHelpPath))
      {
        this.BasicHelpPath = Path.Combine(SystemValues.AppPath, "..\\..\\..\\GMM_Help");
        this.BasicHelpPath = Path.GetFullPath(this.BasicHelpPath);
      }
      this.BasicHelpPathEn = Path.Combine(this.BasicHelpPath, "en");
      this.BasicHelpPathDe = Path.Combine(this.BasicHelpPath, "de");
      this.BasicHelpPath = Path.Combine(this.BasicHelpPath, Language);
    }

    public void ShowHelp(string HelpFilePath)
    {
      string path = Path.Combine(this.BasicHelpPath, HelpFilePath);
      if (!File.Exists(path))
      {
        path = Path.Combine(this.BasicHelpPathEn, HelpFilePath);
        if (!File.Exists(path))
        {
          path = Path.Combine(this.BasicHelpPathDe, HelpFilePath);
          if (!File.Exists(path))
          {
            int num = (int) MessageBox.Show("This help file is not available:\r\n" + path);
            return;
          }
        }
      }
      try
      {
        if (this.MyHelpProcess.StartInfo.FileName.Length != 0)
        {
          try
          {
            Process.GetProcessById(this.MyHelpProcess.Id);
            if (this.MyHelpProcess.StartInfo.FileName == path)
              return;
            this.MyHelpProcess.Kill();
            this.MyHelpProcess.WaitForExit();
          }
          catch
          {
          }
        }
        this.MyHelpProcess.StartInfo.FileName = path;
        this.MyHelpProcess.Start();
        Thread.Sleep(500);
      }
      catch
      {
        int num = (int) MessageBox.Show("Can`t start the HTML viewer");
      }
    }
  }
}
