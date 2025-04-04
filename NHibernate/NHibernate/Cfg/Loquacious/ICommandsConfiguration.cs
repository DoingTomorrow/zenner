// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.ICommandsConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Exceptions;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  public interface ICommandsConfiguration
  {
    ICommandsConfiguration Preparing();

    ICommandsConfiguration WithTimeout(byte seconds);

    ICommandsConfiguration ConvertingExceptionsThrough<TExceptionConverter>() where TExceptionConverter : ISQLExceptionConverter;

    ICommandsConfiguration AutoCommentingSql();

    IDbIntegrationConfiguration WithHqlToSqlSubstitutions(string csvQuerySubstitutions);

    IDbIntegrationConfiguration WithDefaultHqlToSqlSubstitutions();

    ICommandsConfiguration WithMaximumDepthOfOuterJoinFetching(byte maxFetchDepth);
  }
}
