// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.FileProvider.LocalFileProvider
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Client.UI.Common.FileProvider.Interfaces;
using System;
using System.Diagnostics;

#nullable disable
namespace MSS.Client.UI.Common.FileProvider
{
  public class LocalFileProvider : IFileProvider
  {
    private string _licenseType;

    public LocalFileProvider(string LicenseType) => this._licenseType = LicenseType;

    public void OpenFile(Action errorCase = null)
    {
      Process.Start(string.Format("C:\\ProgramData\\MSS\\{0}_HelpFile.pdf", (object) this._licenseType));
    }
  }
}
