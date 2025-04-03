// Decompiled with JetBrains decompiler
// Type: AutoMapper.AutoMapperMappingException
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace AutoMapper
{
  public class AutoMapperMappingException : Exception
  {
    private new string _message;

    public AutoMapperMappingException()
    {
    }

    public AutoMapperMappingException(string message)
      : base(message)
    {
      this._message = message;
    }

    public AutoMapperMappingException(string message, Exception inner)
      : base((string) null, inner)
    {
      this._message = message;
    }

    public AutoMapperMappingException(ResolutionContext context) => this.Context = context;

    public AutoMapperMappingException(ResolutionContext context, Exception inner)
      : base((string) null, inner)
    {
      this.Context = context;
    }

    public AutoMapperMappingException(ResolutionContext context, string message)
      : this(context)
    {
      this._message = message;
    }

    public ResolutionContext Context { get; private set; }

    public override string Message
    {
      get
      {
        string str1 = (string) null;
        string newLine = Environment.NewLine;
        if (this.Context != null)
        {
          string str2 = this._message + newLine + newLine + "Mapping types:" + newLine + string.Format("{0} -> {1}", new object[2]
          {
            (object) this.Context.SourceType.Name,
            (object) this.Context.DestinationType.Name
          }) + newLine + string.Format("{0} -> {1}", new object[2]
          {
            (object) this.Context.SourceType.FullName,
            (object) this.Context.DestinationType.FullName
          });
          string destPath = this.GetDestPath(this.Context);
          return str2 + newLine + newLine + "Destination path:" + newLine + destPath + newLine + newLine + "Source value:" + newLine + (this.Context.SourceValue ?? (object) "(null)");
        }
        if (this._message != null)
          str1 = this._message;
        return (str1 == null ? (string) null : str1 + newLine) + base.Message;
      }
    }

    private string GetDestPath(ResolutionContext context)
    {
      IEnumerable<ResolutionContext> source = AutoMapperMappingException.GetContexts(context).Reverse<ResolutionContext>();
      StringBuilder stringBuilder = new StringBuilder(source.First<ResolutionContext>().DestinationType.Name);
      foreach (ResolutionContext resolutionContext in source)
      {
        if (!string.IsNullOrEmpty(resolutionContext.MemberName))
        {
          stringBuilder.Append(".");
          stringBuilder.Append(resolutionContext.MemberName);
        }
        if (resolutionContext.ArrayIndex.HasValue)
          stringBuilder.AppendFormat("[{0}]", new object[1]
          {
            (object) resolutionContext.ArrayIndex
          });
      }
      return stringBuilder.ToString();
    }

    private static IEnumerable<ResolutionContext> GetContexts(ResolutionContext context)
    {
      for (; context.Parent != null; context = context.Parent)
        yield return context;
      yield return context;
    }

    public override string StackTrace
    {
      get
      {
        return string.Join(Environment.NewLine, ((IEnumerable<string>) base.StackTrace.Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.None)).Where<string>((Func<string, bool>) (str => !str.TrimStart().StartsWith("at AutoMapper."))));
      }
    }
  }
}
