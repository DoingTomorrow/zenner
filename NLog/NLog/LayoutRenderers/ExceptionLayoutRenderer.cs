// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ExceptionLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.MessageTemplates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("exception")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class ExceptionLayoutRenderer : LayoutRenderer
  {
    private string _format;
    private string _innerFormat = string.Empty;
    private readonly Dictionary<ExceptionRenderingFormat, Action<StringBuilder, Exception>> _renderingfunctions;
    private static readonly Dictionary<string, ExceptionRenderingFormat> _formatsMapping = new Dictionary<string, ExceptionRenderingFormat>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "MESSAGE",
        ExceptionRenderingFormat.Message
      },
      {
        "TYPE",
        ExceptionRenderingFormat.Type
      },
      {
        "SHORTTYPE",
        ExceptionRenderingFormat.ShortType
      },
      {
        "TOSTRING",
        ExceptionRenderingFormat.ToString
      },
      {
        "METHOD",
        ExceptionRenderingFormat.Method
      },
      {
        "STACKTRACE",
        ExceptionRenderingFormat.StackTrace
      },
      {
        "DATA",
        ExceptionRenderingFormat.Data
      },
      {
        "@",
        ExceptionRenderingFormat.Serialize
      }
    };

    public ExceptionLayoutRenderer()
    {
      this.Format = "message";
      this.Separator = " ";
      this.ExceptionDataSeparator = ";";
      this.InnerExceptionSeparator = EnvironmentHelper.NewLine;
      this.MaxInnerExceptionLevel = 0;
      this._renderingfunctions = new Dictionary<ExceptionRenderingFormat, Action<StringBuilder, Exception>>()
      {
        {
          ExceptionRenderingFormat.Message,
          new Action<StringBuilder, Exception>(this.AppendMessage)
        },
        {
          ExceptionRenderingFormat.Type,
          new Action<StringBuilder, Exception>(this.AppendType)
        },
        {
          ExceptionRenderingFormat.ShortType,
          new Action<StringBuilder, Exception>(this.AppendShortType)
        },
        {
          ExceptionRenderingFormat.ToString,
          new Action<StringBuilder, Exception>(this.AppendToString)
        },
        {
          ExceptionRenderingFormat.Method,
          new Action<StringBuilder, Exception>(this.AppendMethod)
        },
        {
          ExceptionRenderingFormat.StackTrace,
          new Action<StringBuilder, Exception>(this.AppendStackTrace)
        },
        {
          ExceptionRenderingFormat.Data,
          new Action<StringBuilder, Exception>(this.AppendData)
        },
        {
          ExceptionRenderingFormat.Serialize,
          new Action<StringBuilder, Exception>(this.AppendSerializeObject)
        }
      };
    }

    [DefaultParameter]
    public string Format
    {
      get => this._format;
      set
      {
        this._format = value;
        this.Formats = ExceptionLayoutRenderer.CompileFormat(value);
      }
    }

    public string InnerFormat
    {
      get => this._innerFormat;
      set
      {
        this._innerFormat = value;
        this.InnerFormats = ExceptionLayoutRenderer.CompileFormat(value);
      }
    }

    [DefaultValue(" ")]
    public string Separator { get; set; }

    [DefaultValue(";")]
    public string ExceptionDataSeparator { get; set; }

    [DefaultValue(0)]
    public int MaxInnerExceptionLevel { get; set; }

    public string InnerExceptionSeparator { get; set; }

    public List<ExceptionRenderingFormat> Formats { get; private set; }

    public List<ExceptionRenderingFormat> InnerFormats { get; private set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      Exception exception = logEvent.Exception;
      if (exception == null)
        return;
      this.AppendException(exception, this.Formats, builder);
      int currentLevel1 = 0;
      if (currentLevel1 >= this.MaxInnerExceptionLevel)
        return;
      int currentLevel2 = this.AppendInnerExceptionTree(exception, currentLevel1, builder);
      if (!(exception is AggregateException primaryException))
        return;
      this.AppendAggregateException(primaryException, currentLevel2, builder);
    }

    private void AppendAggregateException(
      AggregateException primaryException,
      int currentLevel,
      StringBuilder builder)
    {
      AggregateException aggregateException = primaryException.Flatten();
      if (aggregateException.InnerExceptions == null)
        return;
      for (int index = 0; index < aggregateException.InnerExceptions.Count && currentLevel < this.MaxInnerExceptionLevel; ++currentLevel)
      {
        Exception innerException = aggregateException.InnerExceptions[index];
        if (innerException != primaryException.InnerException)
        {
          if (innerException == null)
          {
            InternalLogger.Debug("Skipping rendering exception as exception is null");
          }
          else
          {
            this.AppendInnerException(innerException, builder);
            ++currentLevel;
            currentLevel = this.AppendInnerExceptionTree(innerException, currentLevel, builder);
          }
        }
        ++index;
      }
    }

    private int AppendInnerExceptionTree(
      Exception currentException,
      int currentLevel,
      StringBuilder sb)
    {
      for (currentException = currentException.InnerException; currentException != null && currentLevel < this.MaxInnerExceptionLevel; currentException = currentException.InnerException)
      {
        this.AppendInnerException(currentException, sb);
        ++currentLevel;
      }
      return currentLevel;
    }

    private void AppendInnerException(Exception currentException, StringBuilder builder)
    {
      builder.Append(this.InnerExceptionSeparator);
      this.AppendException(currentException, this.InnerFormats ?? this.Formats, builder);
    }

    private void AppendException(
      Exception currentException,
      List<ExceptionRenderingFormat> renderFormats,
      StringBuilder builder)
    {
      int length1 = builder.Length;
      foreach (ExceptionRenderingFormat renderFormat in renderFormats)
      {
        if (length1 != builder.Length)
        {
          length1 = builder.Length;
          builder.Append(this.Separator);
        }
        int length2 = builder.Length;
        this._renderingfunctions[renderFormat](builder, currentException);
        if (builder.Length == length2 && builder.Length != length1)
          builder.Length = length1;
      }
    }

    protected virtual void AppendMessage(StringBuilder sb, Exception ex)
    {
      try
      {
        sb.Append(ex.Message);
      }
      catch (Exception ex1)
      {
        string message = string.Format("Exception in {0}.AppendMessage(): {1}.", (object) typeof (ExceptionLayoutRenderer).FullName, (object) ex1.GetType().FullName);
        sb.Append("NLog message: ");
        sb.Append(message);
        InternalLogger.Warn(ex1, message);
      }
    }

    protected virtual void AppendMethod(StringBuilder sb, Exception ex)
    {
      if (!(ex.TargetSite != (MethodBase) null))
        return;
      sb.Append(ex.TargetSite.ToString());
    }

    protected virtual void AppendStackTrace(StringBuilder sb, Exception ex)
    {
      if (string.IsNullOrEmpty(ex.StackTrace))
        return;
      sb.Append(ex.StackTrace);
    }

    protected virtual void AppendToString(StringBuilder sb, Exception ex)
    {
      sb.Append(ex.ToString());
    }

    protected virtual void AppendType(StringBuilder sb, Exception ex)
    {
      sb.Append(ex.GetType().FullName);
    }

    protected virtual void AppendShortType(StringBuilder sb, Exception ex)
    {
      sb.Append(ex.GetType().Name);
    }

    protected virtual void AppendData(StringBuilder sb, Exception ex)
    {
      if (ex.Data == null || ex.Data.Count <= 0)
        return;
      string str = string.Empty;
      foreach (object key in (IEnumerable) ex.Data.Keys)
      {
        sb.Append(str);
        sb.AppendFormat("{0}: {1}", key, ex.Data[key]);
        str = this.ExceptionDataSeparator;
      }
    }

    protected virtual void AppendSerializeObject(StringBuilder sb, Exception ex)
    {
      ConfigurationItemFactory.Default.ValueFormatter.FormatValue((object) ex, (string) null, CaptureType.Serialize, (IFormatProvider) null, sb);
    }

    private static List<ExceptionRenderingFormat> CompileFormat(string formatSpecifier)
    {
      List<ExceptionRenderingFormat> exceptionRenderingFormatList = new List<ExceptionRenderingFormat>();
      string str = formatSpecifier.Replace(" ", string.Empty);
      string[] separator = new string[1]{ "," };
      foreach (string key in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        ExceptionRenderingFormat exceptionRenderingFormat;
        if (ExceptionLayoutRenderer._formatsMapping.TryGetValue(key, out exceptionRenderingFormat))
          exceptionRenderingFormatList.Add(exceptionRenderingFormat);
        else
          InternalLogger.Warn<string>("Unknown exception data target: {0}", key);
      }
      return exceptionRenderingFormatList;
    }
  }
}
