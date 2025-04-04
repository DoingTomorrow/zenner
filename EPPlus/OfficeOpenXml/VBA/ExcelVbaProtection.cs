// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.VBA.ExcelVbaProtection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace OfficeOpenXml.VBA
{
  public class ExcelVbaProtection
  {
    private ExcelVbaProject _project;

    internal ExcelVbaProtection(ExcelVbaProject project)
    {
      this._project = project;
      this.VisibilityState = true;
    }

    public bool UserProtected { get; internal set; }

    public bool HostProtected { get; internal set; }

    public bool VbeProtected { get; internal set; }

    public bool VisibilityState { get; internal set; }

    internal byte[] PasswordHash { get; set; }

    internal byte[] PasswordKey { get; set; }

    public void SetPassword(string Password)
    {
      if (string.IsNullOrEmpty(Password))
      {
        this.PasswordHash = (byte[]) null;
        this.PasswordKey = (byte[]) null;
        this.VbeProtected = false;
        this.HostProtected = false;
        this.UserProtected = false;
        this.VisibilityState = true;
        this._project.ProjectID = "{5DD90D76-4904-47A2-AF0D-D69B4673604E}";
      }
      else
      {
        this.PasswordKey = new byte[4];
        RandomNumberGenerator.Create().GetBytes(this.PasswordKey);
        byte[] numArray = new byte[Password.Length + 4];
        Array.Copy((Array) Encoding.GetEncoding(this._project.CodePage).GetBytes(Password), (Array) numArray, Password.Length);
        this.VbeProtected = true;
        this.VisibilityState = false;
        Array.Copy((Array) this.PasswordKey, 0, (Array) numArray, numArray.Length - 4, 4);
        this.PasswordHash = SHA1.Create().ComputeHash(numArray);
        this._project.ProjectID = "{00000000-0000-0000-0000-000000000000}";
      }
    }
  }
}
