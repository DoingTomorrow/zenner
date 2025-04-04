// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.FileProvider.Interfaces.IFileProvider
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using System;

#nullable disable
namespace MSS.Client.UI.Common.FileProvider.Interfaces
{
  public interface IFileProvider
  {
    void OpenFile(Action errorCase = null);
  }
}
