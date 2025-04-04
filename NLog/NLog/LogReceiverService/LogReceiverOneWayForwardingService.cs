// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.LogReceiverOneWayForwardingService
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.LogReceiverService
{
  public class LogReceiverOneWayForwardingService(LogFactory logFactory) : 
    BaseLogReceiverForwardingService(logFactory),
    ILogReceiverOneWayServer
  {
    public LogReceiverOneWayForwardingService()
      : this((LogFactory) null)
    {
    }
  }
}
