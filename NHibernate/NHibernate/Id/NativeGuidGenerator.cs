// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.NativeGuidGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Id
{
  public class NativeGuidGenerator : IIdentifierGenerator
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (NativeGuidGenerator));
    private readonly IType identifierType = (IType) new GuidType();

    public object Generate(ISessionImplementor session, object obj)
    {
      SqlString sql = new SqlString(session.Factory.Dialect.SelectGUIDString);
      try
      {
        IDbCommand cmd = session.Batcher.PrepareCommand(CommandType.Text, sql, SqlTypeFactory.NoTypes);
        IDataReader dataReader = (IDataReader) null;
        try
        {
          dataReader = session.Batcher.ExecuteReader(cmd);
          object obj1;
          try
          {
            dataReader.Read();
            obj1 = IdentifierGeneratorFactory.Get(dataReader, this.identifierType, session);
          }
          finally
          {
            dataReader.Close();
          }
          NativeGuidGenerator.log.Debug((object) ("GUID identifier generated: " + obj1));
          return obj1;
        }
        finally
        {
          session.Batcher.CloseCommand(cmd, dataReader);
        }
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, ex, "could not retrieve GUID", sql);
      }
    }
  }
}
