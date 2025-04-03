// Decompiled with JetBrains decompiler
// Type: ZENNER.DatabaseConnectionSettings
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using GmmDbLib;
using System;

#nullable disable
namespace ZENNER
{
  [Serializable]
  public sealed class DatabaseConnectionSettings
  {
    public MeterDbTypes Type { get; set; }

    public string DataSource { get; set; }

    public string Password { get; set; }

    public string User { get; set; }

    public string DatabaseName { get; set; }
  }
}
