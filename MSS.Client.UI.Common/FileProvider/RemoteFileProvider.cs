// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.FileProvider.RemoteFileProvider
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Business.Utils;
using MSS.Client.UI.Common.FileProvider.Interfaces;
using MSS.Localisation;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

#nullable disable
namespace MSS.Client.UI.Common.FileProvider
{
  public class RemoteFileProvider : IFileProvider
  {
    public readonly string _licenseType;

    private HttpResponseMessage Response { get; set; }

    private HttpClient Client { get; set; }

    public RemoteFileProvider(string LicenseType) => this._licenseType = LicenseType;

    public async void OpenFile(Action errorCase = null)
    {
      if (this.Response != null)
      {
        await this.ConfigureConnection();
        if (!this.Response.IsSuccessStatusCode || this.Response.StatusCode == HttpStatusCode.NoContent)
          return;
        byte[] res = await this.Response.Content.ReadAsByteArrayAsync();
        System.IO.File.WriteAllBytes(string.Format("C:\\ProgramData\\MSS\\{0}_HelpFile.pdf", (object) this._licenseType), res);
        Process.Start(string.Format("C:\\ProgramData\\MSS\\{0}_HelpFile.pdf", (object) this._licenseType));
        res = (byte[]) null;
      }
      else
      {
        if (this.Response != null)
          throw new Exception(string.Format("{0} {1}", (object) Resources.UploadHelpFileConnectionError, (object) CustomerConfiguration.GetPropertyValue("LicenseWebApi")));
        errorCase();
      }
    }

    private async Task ConfigureConnection()
    {
      using (this.Client = new HttpClient())
      {
        this.Client.BaseAddress = new Uri(CustomerConfiguration.GetPropertyValue("LicenseWebApi"));
        this.Client.DefaultRequestHeaders.Accept.Clear();
        this.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
        HttpResponseMessage httpResponseMessage = await this.Client.GetAsync(string.Format("License/DownloadHelpFile?licenseType={0}", (object) this._licenseType));
        this.Response = httpResponseMessage;
        httpResponseMessage = (HttpResponseMessage) null;
      }
    }
  }
}
