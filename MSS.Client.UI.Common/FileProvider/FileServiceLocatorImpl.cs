// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.FileProvider.FileServiceLocatorImpl
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Client.UI.Common.FileProvider.Interfaces;
using System;

#nullable disable
namespace MSS.Client.UI.Common.FileProvider
{
  public class FileServiceLocatorImpl : IFileServiceLocator
  {
    private readonly string _licenseType;

    public FileServiceLocatorImpl(string LicenseType) => this._licenseType = LicenseType;

    public IFileProvider GetProvider(Func<string, bool> condition)
    {
      return condition(this._licenseType) ? (IFileProvider) new LocalFileProvider(this._licenseType) : (IFileProvider) new RemoteFileProvider(this._licenseType);
    }
  }
}
