// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ConnectionItemSelectObject
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ReadoutConfiguration
{
  public class ConnectionItemSelectObject
  {
    internal IConnectionItem selectedItem;
    internal List<IConnectionItem> itemList = new List<IConnectionItem>();
  }
}
