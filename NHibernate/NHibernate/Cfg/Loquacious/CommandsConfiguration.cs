// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.CommandsConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Exceptions;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class CommandsConfiguration : ICommandsConfiguration
  {
    private readonly DbIntegrationConfiguration dbc;

    public CommandsConfiguration(DbIntegrationConfiguration dbc) => this.dbc = dbc;

    public ICommandsConfiguration Preparing()
    {
      this.dbc.Configuration.SetProperty("prepare_sql", "true");
      return (ICommandsConfiguration) this;
    }

    public ICommandsConfiguration WithTimeout(byte seconds)
    {
      this.dbc.Configuration.SetProperty("command_timeout", seconds.ToString());
      return (ICommandsConfiguration) this;
    }

    public ICommandsConfiguration ConvertingExceptionsThrough<TExceptionConverter>() where TExceptionConverter : ISQLExceptionConverter
    {
      this.dbc.Configuration.SetProperty("sql_exception_converter", typeof (TExceptionConverter).AssemblyQualifiedName);
      return (ICommandsConfiguration) this;
    }

    public ICommandsConfiguration AutoCommentingSql()
    {
      this.dbc.Configuration.SetProperty("use_sql_comments", "true");
      return (ICommandsConfiguration) this;
    }

    public IDbIntegrationConfiguration WithHqlToSqlSubstitutions(string csvQuerySubstitutions)
    {
      this.dbc.Configuration.SetProperty("query.substitutions", csvQuerySubstitutions);
      return (IDbIntegrationConfiguration) this.dbc;
    }

    public IDbIntegrationConfiguration WithDefaultHqlToSqlSubstitutions()
    {
      return (IDbIntegrationConfiguration) this.dbc;
    }

    public ICommandsConfiguration WithMaximumDepthOfOuterJoinFetching(byte maxFetchDepth)
    {
      this.dbc.Configuration.SetProperty("max_fetch_depth", maxFetchDepth.ToString());
      return (ICommandsConfiguration) this;
    }
  }
}
