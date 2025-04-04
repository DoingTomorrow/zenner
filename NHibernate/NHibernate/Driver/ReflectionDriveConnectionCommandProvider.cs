// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.ReflectionDriveConnectionCommandProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Driver
{
  public class ReflectionDriveConnectionCommandProvider : IDriveConnectionCommandProvider
  {
    private readonly Type commandType;
    private readonly Type connectionType;

    public ReflectionDriveConnectionCommandProvider(Type connectionType, Type commandType)
    {
      if (connectionType == null)
        throw new ArgumentNullException(nameof (connectionType));
      if (commandType == null)
        throw new ArgumentNullException(nameof (commandType));
      this.connectionType = connectionType;
      this.commandType = commandType;
    }

    public IDbConnection CreateConnection()
    {
      return (IDbConnection) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(this.connectionType);
    }

    public IDbCommand CreateCommand()
    {
      return (IDbCommand) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(this.commandType);
    }
  }
}
