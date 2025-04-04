// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.DbTimestampType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class DbTimestampType : TimestampType, IVersionType, IType, ICacheAssembler
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DbTimestampType));
    private static readonly SqlType[] EmptyParams = new SqlType[0];

    public override string Name => "DbTimestamp";

    public override object Seed(ISessionImplementor session)
    {
      if (session == null)
      {
        DbTimestampType.log.Debug((object) "incoming session was null; using current vm time");
        return base.Seed(session);
      }
      if (session.Factory.Dialect.SupportsCurrentTimestampSelection)
        return this.GetCurrentTimestamp(session);
      DbTimestampType.log.Debug((object) "falling back to vm-based timestamp, as dialect does not support current timestamp selection");
      return base.Seed(session);
    }

    private object GetCurrentTimestamp(ISessionImplementor session)
    {
      return this.UsePreparedStatement(session.Factory.Dialect.CurrentTimestampSelectString, session);
    }

    protected virtual object UsePreparedStatement(
      string timestampSelectString,
      ISessionImplementor session)
    {
      SqlString sql = new SqlString(timestampSelectString);
      IDbCommand cmd = (IDbCommand) null;
      IDataReader reader = (IDataReader) null;
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          cmd = session.Batcher.PrepareCommand(CommandType.Text, sql, DbTimestampType.EmptyParams);
          reader = session.Batcher.ExecuteReader(cmd);
          reader.Read();
          DateTime dateTime = reader.GetDateTime(0);
          if (DbTimestampType.log.IsDebugEnabled)
            DbTimestampType.log.Debug((object) ("current timestamp retreived from db : " + (object) dateTime + " (tiks=" + (object) dateTime.Ticks + ")"));
          return (object) dateTime;
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, (Exception) ex, "could not select current db timestamp", sql);
        }
        finally
        {
          if (cmd != null)
          {
            try
            {
              session.Batcher.CloseCommand(cmd, reader);
            }
            catch (DbException ex)
            {
              DbTimestampType.log.Warn((object) "unable to clean up prepared statement", (Exception) ex);
            }
          }
        }
      }
    }
  }
}
