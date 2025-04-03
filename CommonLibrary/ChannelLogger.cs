// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.ChannelLogger
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using NLog;
using System;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public class ChannelLogger
  {
    public Logger TheLogger;
    private ConfigList TheConfigList;
    private string ReadingChannelIdentification = (string) null;

    public ChannelLogger(Logger logger, ConfigList configList)
    {
      this.TheLogger = logger;
      this.TheConfigList = configList;
    }

    public ChannelLogger(Logger logger, string readingChannelIdentification)
    {
      this.TheLogger = logger;
      this.ReadingChannelIdentification = readingChannelIdentification;
    }

    public string ChannelInfo
    {
      get
      {
        if (this.TheConfigList != null)
          return this.TheConfigList.ReadingChannelIdentification + ": ";
        return this.ReadingChannelIdentification != null ? this.ReadingChannelIdentification + ": " : "NoChannel: ";
      }
    }

    public bool IsTraceEnabled => this.TheLogger.IsTraceEnabled;

    public void Trace(string info) => this.TheLogger.Trace(this.ChannelInfo + info);

    public void Debug(string info) => this.TheLogger.Debug(this.ChannelInfo + info);

    public void Info(string info) => this.TheLogger.Debug(this.ChannelInfo + info);

    public void Warn(string info) => this.TheLogger.Warn(this.ChannelInfo + info);

    public void Error(string info) => this.TheLogger.Error(this.ChannelInfo + info);

    public void Fatal(string info) => this.TheLogger.Fatal(this.ChannelInfo + info);

    public void Trace(string info, Exception ex)
    {
      this.TheLogger.Trace(this.ChannelInfo + info, ex);
    }

    public void Debug(string info, Exception ex)
    {
      this.TheLogger.Debug(this.ChannelInfo + info, ex);
    }

    public void Info(string info, Exception ex)
    {
      this.TheLogger.Debug(this.ChannelInfo + info, ex);
    }

    public void Warn(string info, Exception ex) => this.TheLogger.Warn(this.ChannelInfo + info, ex);

    public void Error(string info, Exception ex)
    {
      this.TheLogger.Error(this.ChannelInfo + info, ex);
    }

    public void Fatal(string info, Exception ex)
    {
      this.TheLogger.Fatal(this.ChannelInfo + info, ex);
    }
  }
}
