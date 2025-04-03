// Decompiled with JetBrains decompiler
// Type: CommunicationPort.Functions.MiConPollingThread
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using GmmDbLib;
using NLog;
using System;
using System.Threading;
using ZENNER.CommonLibrary;

#nullable disable
namespace CommunicationPort.Functions
{
  internal sealed class MiConPollingThread
  {
    internal static Logger Base_MiConPollingLogger = LogManager.GetLogger(nameof (MiConPollingThread));
    internal ChannelLogger MiConPollingLogger;
    private CommunicationByMinoConnect MyCommunication;
    private const string StateRequestString = "#s\r\n";
    internal bool StopThread = false;
    internal string CommandStringForPollingThread = (string) null;

    internal event EventHandler ConnectionLost;

    internal MiConPollingThread(CommunicationByMinoConnect myCommunication)
    {
      this.MyCommunication = myCommunication;
      this.MiConPollingLogger = new ChannelLogger(MiConPollingThread.Base_MiConPollingLogger, myCommunication.configList);
      this.MiConPollingLogger.Debug("MiConPollingThread object created");
    }

    internal void PollingThreadMain()
    {
      this.MiConPollingLogger.Info("MinoConnect polling thread start");
      bool flag = false;
      while (!this.StopThread)
      {
        try
        {
          if (this.MyCommunication.ConfigurationChanged)
          {
            this.MyCommunication.StateRequired = new MinoConnectState(this.MyCommunication, this.MiConPollingLogger);
            this.CommandStringForPollingThread = this.MyCommunication.StateLastReceived.GetChangeCommand(this.MyCommunication.StateRequired);
            this.MyCommunication.StateLastReceived.SetFromMinoConnectState(this.MyCommunication.StateRequired);
            this.MyCommunication.ConfigurationChanged = false;
          }
          DataReceiveInfo queueData = this.MyCommunication.ReceiveQueueData();
          if (queueData != null && queueData.StatusInfoReceived)
          {
            this.MyCommunication.StatusTimeoutCounter = this.MyCommunication.StatusErrorTime_ms / this.MyCommunication.StatusPolling_ms;
            flag = false;
          }
          else if (this.MyCommunication.StatusTimeoutCounter <= 0 && !flag)
          {
            flag = true;
            this.MyCommunication.MyFunctions.RaiseMessageEvent(Ot.Gtm(Tg.CommunicationLogic, "MiConPollingTimeout", "No status polling answer form MinoConnect. Connection lost!"));
          }
          --this.MyCommunication.StatusTimeoutCounter;
          this.MyCommunication.WorkStatusLine();
          string text;
          if (this.CommandStringForPollingThread != null)
          {
            text = this.CommandStringForPollingThread + "#s\r\n";
            this.CommandStringForPollingThread = (string) null;
          }
          else
            text = "#s\r\n";
          if (this.MyCommunication.channel != null && this.MyCommunication.channel.IsOpen)
          {
            lock (this.MyCommunication.channel)
            {
              this.MiConPollingLogger.Trace("Write MiConCommand: '" + text + "'");
              this.MyCommunication.channel.Write(text);
            }
          }
          else
            break;
        }
        catch (Exception ex)
        {
          string str = Ot.Gtm(Tg.CommunicationLogic, "MiConPollingError", "MinoConnect polling thread error") + " " + ex.ToString();
          this.MiConPollingLogger.Error(str);
          this.MyCommunication.MyFunctions.RaiseMessageEvent(str);
          break;
        }
        Thread.Sleep(this.MyCommunication.StatusPolling_ms);
      }
      string str1 = Ot.Gtm(Tg.CommunicationLogic, "MiConPollingThreadStopped", "MinoConnect polling thread stopped.");
      this.MyCommunication.MyFunctions.RaiseMessageEvent(str1);
      this.MiConPollingLogger.Info(str1);
      if (this.StopThread || this.ConnectionLost == null)
        return;
      this.ConnectionLost((object) this, (EventArgs) null);
      this.MyCommunication.Close();
    }

    internal delegate void Start();
  }
}
