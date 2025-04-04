// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DatabaseCommandInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

#nullable disable
namespace NLog.Targets
{
  [NLogConfigurationItem]
  public class DatabaseCommandInfo
  {
    public DatabaseCommandInfo()
    {
      this.Parameters = (IList<DatabaseParameterInfo>) new List<DatabaseParameterInfo>();
      this.CommandType = CommandType.Text;
    }

    [RequiredParameter]
    [DefaultValue(CommandType.Text)]
    public CommandType CommandType { get; set; }

    public Layout ConnectionString { get; set; }

    [RequiredParameter]
    public Layout Text { get; set; }

    public bool IgnoreFailures { get; set; }

    [ArrayParameter(typeof (DatabaseParameterInfo), "parameter")]
    public IList<DatabaseParameterInfo> Parameters { get; private set; }
  }
}
