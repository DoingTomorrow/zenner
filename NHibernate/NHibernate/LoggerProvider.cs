// Decompiled with JetBrains decompiler
// Type: NHibernate.LoggerProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Configuration;
using System.IO;
using System.Linq;

#nullable disable
namespace NHibernate
{
  public class LoggerProvider
  {
    private const string NhibernateLoggerConfKey = "nhibernate-logger";
    private readonly ILoggerFactory loggerFactory;
    private static LoggerProvider instance;

    static LoggerProvider()
    {
      string nhibernateLoggerClass = LoggerProvider.GetNhibernateLoggerClass();
      LoggerProvider.SetLoggersFactory(string.IsNullOrEmpty(nhibernateLoggerClass) ? (ILoggerFactory) new NoLoggingLoggerFactory() : LoggerProvider.GetLoggerFactory(nhibernateLoggerClass));
    }

    private static ILoggerFactory GetLoggerFactory(string nhibernateLoggerClass)
    {
      Type type = Type.GetType(nhibernateLoggerClass);
      try
      {
        return (ILoggerFactory) Activator.CreateInstance(type);
      }
      catch (MissingMethodException ex)
      {
        throw new ApplicationException("Public constructor was not found for " + (object) type, (Exception) ex);
      }
      catch (InvalidCastException ex)
      {
        throw new ApplicationException(type.ToString() + "Type does not implement " + (object) typeof (ILoggerFactory), (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Unable to instantiate: " + (object) type, ex);
      }
    }

    private static string GetNhibernateLoggerClass()
    {
      string name = ConfigurationManager.AppSettings.Keys.Cast<string>().FirstOrDefault<string>((Func<string, bool>) (k => "nhibernate-logger".Equals(k.ToLowerInvariant())));
      string nhibernateLoggerClass = (string) null;
      if (string.IsNullOrEmpty(name))
      {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
        if (File.Exists(Path.Combine(relativeSearchPath == null ? baseDirectory : Path.Combine(baseDirectory, relativeSearchPath), "log4net.dll")))
          nhibernateLoggerClass = typeof (Log4NetLoggerFactory).AssemblyQualifiedName;
      }
      else
        nhibernateLoggerClass = ConfigurationManager.AppSettings[name];
      return nhibernateLoggerClass;
    }

    public static void SetLoggersFactory(ILoggerFactory loggerFactory)
    {
      LoggerProvider.instance = new LoggerProvider(loggerFactory);
    }

    private LoggerProvider(ILoggerFactory loggerFactory) => this.loggerFactory = loggerFactory;

    public static IInternalLogger LoggerFor(string keyName)
    {
      return LoggerProvider.instance.loggerFactory.LoggerFor(keyName);
    }

    public static IInternalLogger LoggerFor(Type type)
    {
      return LoggerProvider.instance.loggerFactory.LoggerFor(type);
    }
  }
}
