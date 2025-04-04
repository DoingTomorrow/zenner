// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.ReflectionBasedDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Driver
{
  public abstract class ReflectionBasedDriver : DriverBase
  {
    protected const string ReflectionTypedProviderExceptionMessageTemplate = "The IDbCommand and IDbConnection implementation in the assembly {0} could not be found. Ensure that the assembly {0} is located in the application directory or in the Global Assembly Cache. If the assembly is in the GAC, use <qualifyAssembly/> element in the application configuration file to specify the full name of the assembly.";
    private readonly IDriveConnectionCommandProvider connectionCommandProvider;

    protected ReflectionBasedDriver(
      string driverAssemblyName,
      string connectionTypeName,
      string commandTypeName)
      : this((string) null, driverAssemblyName, connectionTypeName, commandTypeName)
    {
    }

    protected ReflectionBasedDriver(
      string providerInvariantName,
      string driverAssemblyName,
      string connectionTypeName,
      string commandTypeName)
    {
      Type connectionType = ReflectHelper.TypeFromAssembly(connectionTypeName, driverAssemblyName, false);
      Type commandType = ReflectHelper.TypeFromAssembly(commandTypeName, driverAssemblyName, false);
      if (connectionType == null || commandType == null)
        this.connectionCommandProvider = !string.IsNullOrEmpty(providerInvariantName) ? (IDriveConnectionCommandProvider) new DbProviderFactoryDriveConnectionCommandProvider(DbProviderFactories.GetFactory(providerInvariantName)) : throw new HibernateException(string.Format("The IDbCommand and IDbConnection implementation in the assembly {0} could not be found. Ensure that the assembly {0} is located in the application directory or in the Global Assembly Cache. If the assembly is in the GAC, use <qualifyAssembly/> element in the application configuration file to specify the full name of the assembly.", (object) driverAssemblyName));
      else
        this.connectionCommandProvider = (IDriveConnectionCommandProvider) new ReflectionDriveConnectionCommandProvider(connectionType, commandType);
    }

    public override IDbConnection CreateConnection()
    {
      return this.connectionCommandProvider.CreateConnection();
    }

    public override IDbCommand CreateCommand() => this.connectionCommandProvider.CreateCommand();
  }
}
