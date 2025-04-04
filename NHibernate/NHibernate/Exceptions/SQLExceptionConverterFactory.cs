// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.SQLExceptionConverterFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Exceptions
{
  public static class SQLExceptionConverterFactory
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SQLExceptionConverterFactory));

    public static ISQLExceptionConverter BuildSQLExceptionConverter(
      NHibernate.Dialect.Dialect dialect,
      IDictionary<string, string> properties)
    {
      ISQLExceptionConverter exceptionConverter = (ISQLExceptionConverter) null;
      string converterClassName;
      properties.TryGetValue("sql_exception_converter", out converterClassName);
      if (!string.IsNullOrEmpty(converterClassName))
        exceptionConverter = SQLExceptionConverterFactory.ConstructConverter(converterClassName, dialect.ViolatedConstraintNameExtracter);
      if (exceptionConverter == null)
      {
        SQLExceptionConverterFactory.log.Info((object) "Using dialect defined converter");
        exceptionConverter = dialect.BuildSQLExceptionConverter();
      }
      if (exceptionConverter is IConfigurable configurable)
      {
        try
        {
          configurable.Configure(properties);
        }
        catch (HibernateException ex)
        {
          SQLExceptionConverterFactory.log.Warn((object) "Unable to configure SQLExceptionConverter", (Exception) ex);
          throw;
        }
      }
      return exceptionConverter;
    }

    public static ISQLExceptionConverter BuildMinimalSQLExceptionConverter()
    {
      return (ISQLExceptionConverter) new SQLExceptionConverterFactory.MinimalSQLExceptionConverter();
    }

    private static ISQLExceptionConverter ConstructConverter(
      string converterClassName,
      IViolatedConstraintNameExtracter violatedConstraintNameExtracter)
    {
      try
      {
        SQLExceptionConverterFactory.log.Debug((object) ("Attempting to construct instance of specified SQLExceptionConverter [" + converterClassName + "]"));
        Type type = ReflectHelper.ClassForName(converterClassName);
        foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
          ParameterInfo[] parameters = constructor.GetParameters();
          if (parameters != null && parameters.Length == 1)
          {
            if (typeof (IViolatedConstraintNameExtracter).IsAssignableFrom(parameters[0].ParameterType))
            {
              try
              {
                return (ISQLExceptionConverter) constructor.Invoke(new object[1]
                {
                  (object) violatedConstraintNameExtracter
                });
              }
              catch (Exception ex)
              {
              }
            }
          }
        }
        return (ISQLExceptionConverter) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(type);
      }
      catch (Exception ex)
      {
        SQLExceptionConverterFactory.log.Warn((object) "Unable to construct instance of specified SQLExceptionConverter", ex);
      }
      return (ISQLExceptionConverter) null;
    }

    private class MinimalSQLExceptionConverter : ISQLExceptionConverter
    {
      public Exception Convert(AdoExceptionContextInfo exceptionContextInfo)
      {
        throw new GenericADOException(exceptionContextInfo.Message, exceptionContextInfo.SqlException, exceptionContextInfo.Sql);
      }
    }
  }
}
