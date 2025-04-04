// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Messaging.MessagingSubSystem
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor.Messaging
{
  internal class MessagingSubSystem : IMessagingSubSystem
  {
    private readonly IMessageReceiver m_receiver;
    private readonly IMessageSender m_sender;
    private readonly ILogAnalyticsMonitor m_log;
    private readonly Uri m_defaultUri;
    private readonly MonitorPolicy m_policy;

    public MessagingSubSystem(
      IMessageReceiver receiver,
      IMessageSender sender,
      MonitorPolicy policy,
      Uri defaultUri,
      ILogAnalyticsMonitor log)
    {
      this.m_receiver = Guard.IsNotNull<IMessageReceiver>(receiver, nameof (receiver));
      this.m_sender = Guard.IsNotNull<IMessageSender>(sender, nameof (sender));
      this.m_defaultUri = Guard.IsNotNull<Uri>(defaultUri, "factory");
      this.m_log = Guard.IsNotNull<ILogAnalyticsMonitor>(log, nameof (log));
      this.m_policy = Guard.IsNotNull<MonitorPolicy>(policy, nameof (policy));
    }

    private static void Void<T>(T errorMessage)
    {
    }

    public void SendMessage(MessagePayload payload, Action<SendMessageResult> sendingResultCallback)
    {
      if (payload == null || payload.Data == null)
      {
        if (sendingResultCallback == null)
          return;
        sendingResultCallback(new SendMessageResult()
        {
          Result = SendResult.Failure,
          ShouldResend = false,
          Message = "Message payload was empty"
        });
      }
      else
        this.SendMessage(new MessagingSubSystem.Message()
        {
          Payload = payload,
          UriAttempted = new List<Uri>()
        }, sendingResultCallback ?? new Action<SendMessageResult>(MessagingSubSystem.Void<SendMessageResult>));
    }

    private void SendMessage(MessagingSubSystem.Message message, Action<SendMessageResult> callback)
    {
      if (message == null)
        return;
      if (this.m_policy.TransmissionBlocking.IsBlocking(Timing.Uptime))
      {
        DateTime dateTime = Timing.Now.Add(this.m_policy.TransmissionBlocking.BlockingUntil);
        callback(new SendMessageResult()
        {
          Message = string.Format("Data is blocked until {0}. {1}", (object) dateTime.ToShortTimeString(), (object) this.m_policy.TransmissionBlocking.GetBlockingDescription()),
          Result = SendResult.Success,
          ShouldResend = false
        });
      }
      else
      {
        Uri baseUri = this.m_policy.Info.AlternativeUri;
        if (baseUri != (Uri) null && !message.UriAttempted.Contains(baseUri))
        {
          message.UriAttempted.Add(baseUri);
        }
        else
        {
          baseUri = this.m_defaultUri;
          if (baseUri != (Uri) null && !message.UriAttempted.Contains(baseUri))
          {
            message.UriAttempted.Add(baseUri);
          }
          else
          {
            callback(new SendMessageResult()
            {
              Message = message.LastSendResult == null ? "" : message.LastSendResult.Message,
              Result = SendResult.Failure,
              ShouldResend = false
            });
            return;
          }
        }
        this.m_sender.SendMessage(baseUri, message.Payload, (Action<MessageResponse>) (response => this.RecieveMessage(message, response, callback)));
      }
    }

    private void RecieveMessage(
      MessagingSubSystem.Message message,
      MessageResponse response,
      Action<SendMessageResult> callback)
    {
      try
      {
        if (!response.ResponseReceived)
        {
          SendMessageResult sendMessageResult = new SendMessageResult()
          {
            Message = response.ErrorMessage,
            Result = SendResult.Failure,
            ShouldResend = false
          };
          message.LastSendResult = sendMessageResult;
          this.SendMessage(message, callback);
        }
        else
        {
          SendMessageResult response1 = this.m_receiver.ParseResponse(response);
          message.LastSendResult = response1;
          if (response1.ShouldResend)
            this.SendMessage(message, callback);
          else
            callback(message.LastSendResult);
        }
      }
      catch (Exception ex)
      {
        this.m_log.LogError("Failed to receive message: " + ex.Message);
      }
    }

    internal class Message
    {
      public MessagePayload Payload { get; set; }

      public List<Uri> UriAttempted { get; set; }

      public SendMessageResult LastSendResult { get; set; }
    }
  }
}
