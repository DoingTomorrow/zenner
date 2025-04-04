// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.Util.SqlStatementLogger
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;
using System.Text;

#nullable disable
namespace NHibernate.AdoNet.Util
{
  public class SqlStatementLogger
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor("NHibernate.SQL");

    public SqlStatementLogger()
      : this(false, false)
    {
    }

    public SqlStatementLogger(bool logToStdout, bool formatSql)
    {
      this.LogToStdout = logToStdout;
      this.FormatSql = formatSql;
    }

    public bool LogToStdout { get; set; }

    public bool FormatSql { get; set; }

    public bool IsDebugEnabled => SqlStatementLogger.log.IsDebugEnabled;

    public virtual void LogCommand(string message, IDbCommand command, FormatStyle style)
    {
      if (!SqlStatementLogger.log.IsDebugEnabled && !this.LogToStdout || string.IsNullOrEmpty(command.CommandText))
        return;
      style = this.DetermineActualStyle(style);
      string str = style.Formatter.Format(this.GetCommandLineWithParameters(command));
      string message1 = !string.IsNullOrEmpty(message) ? message + str : str;
      SqlStatementLogger.log.Debug((object) message1);
      if (!this.LogToStdout)
        return;
      Console.Out.WriteLine("NHibernate: " + str);
    }

    public virtual void LogCommand(IDbCommand command, FormatStyle style)
    {
      this.LogCommand((string) null, command, style);
    }

    public string GetCommandLineWithParameters(IDbCommand command)
    {
      string commandText;
      if (command.Parameters.Count == 0)
      {
        commandText = command.CommandText;
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder(command.CommandText.Length + command.Parameters.Count * 20);
        stringBuilder.Append(command.CommandText.TrimEnd(' ', ';', '\n'));
        stringBuilder.Append(";");
        int count = command.Parameters.Count;
        bool flag = false;
        for (int index = 0; index < count; ++index)
        {
          if (flag)
            stringBuilder.Append(", ");
          flag = true;
          IDataParameter parameter = (IDataParameter) command.Parameters[index];
          stringBuilder.Append(string.Format("{0} = {1} [Type: {2}]", (object) parameter.ParameterName, (object) this.GetParameterLogableValue(parameter), (object) SqlStatementLogger.GetParameterLogableType(parameter)));
        }
        commandText = stringBuilder.ToString();
      }
      return commandText;
    }

    private static string GetParameterLogableType(IDataParameter dataParameter)
    {
      if (!(dataParameter is IDbDataParameter dbDataParameter))
        return dbDataParameter.DbType.ToString();
      return dbDataParameter.DbType.ToString() + " (" + (object) dbDataParameter.Size + ")";
    }

    public string GetParameterLogableValue(IDataParameter parameter)
    {
      if (parameter.Value == null || DBNull.Value.Equals(parameter.Value))
        return "NULL";
      if (SqlStatementLogger.IsStringType(parameter.DbType))
        return "'" + this.TruncateWithEllipsis(parameter.Value.ToString(), 1000) + "'";
      return parameter.Value is byte[] buffer ? SqlStatementLogger.GetBufferAsHexString(buffer) : parameter.Value.ToString();
    }

    private static string GetBufferAsHexString(byte[] buffer)
    {
      int length = buffer.Length;
      StringBuilder stringBuilder = new StringBuilder(264);
      stringBuilder.Append("0x");
      for (int index = 0; index < length && index < 128; ++index)
        stringBuilder.Append(buffer[index].ToString("X2"));
      if (length > 128)
        stringBuilder.Append("...");
      return stringBuilder.ToString();
    }

    private static bool IsStringType(DbType dbType)
    {
      return DbType.String.Equals((object) dbType) || DbType.AnsiString.Equals((object) dbType) || DbType.AnsiStringFixedLength.Equals((object) dbType) || DbType.StringFixedLength.Equals((object) dbType);
    }

    public FormatStyle DetermineActualStyle(FormatStyle style)
    {
      return !this.FormatSql ? FormatStyle.None : style;
    }

    public void LogBatchCommand(string batchCommand)
    {
      SqlStatementLogger.log.Debug((object) batchCommand);
      if (!this.LogToStdout)
        return;
      Console.Out.WriteLine("NHibernate: " + batchCommand);
    }

    private string TruncateWithEllipsis(string source, int length)
    {
      return source.Length > length ? source.Substring(0, length) + "..." : source;
    }
  }
}
