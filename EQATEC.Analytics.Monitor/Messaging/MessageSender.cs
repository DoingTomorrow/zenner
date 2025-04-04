// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Messaging.MessageSender
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace EQATEC.Analytics.Monitor.Messaging
{
  internal class MessageSender : IMessageSender
  {
    private readonly ILogAnalyticsMonitor m_log;
    private readonly ProxyConfiguration m_proxyConfiguration;
    private readonly MonitorPolicy m_policy;

    internal MessageSender(
      ILogAnalyticsMonitor log,
      ProxyConfiguration proxyConfig,
      MonitorPolicy policy)
    {
      this.m_log = Guard.IsNotNull<ILogAnalyticsMonitor>(log, nameof (log));
      this.m_proxyConfiguration = proxyConfig;
      this.m_policy = policy;
    }

    public void SendMessage(
      Uri baseUri,
      MessagePayload payload,
      Action<MessageResponse> sendMessageCallback)
    {
      Uri uri = MessageSendingHelper.GetUri(baseUri, payload.Content);
      try
      {
        HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
        if (request == null)
          throw new InvalidOperationException("Could not create a HttpWebRequest");
        request.Method = "POST";
        request.ContentType = "text/xml";
        request.CookieContainer = this.m_policy.RuntimeStatus.CookieContainer;
        MessageSendingHelper.ApplyProxy(this.m_proxyConfiguration, (WebRequest) request);
        request.ContentLength = (long) payload.Data.Length;
        if (!MessageSendingHelper.CanSendWithinDailyBandwidth(this.m_policy, (long) payload.Data.Length))
        {
          string str = string.Format("Bandwidth quota is reached ({0} KB)", (object) ((double) this.m_policy.Info.BandwidthUtilization / 1024.0));
          sendMessageCallback(new MessageResponse()
          {
            ResponsePayload = (string) null,
            ResponseReceived = false,
            ErrorMessage = str
          });
        }
        else
          request.BeginGetRequestStream((AsyncCallback) (r => this.ReadCallbackRequest(r, request, payload, sendMessageCallback)), (object) null);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("Failed to send message asynchronously to endpoint {0}: {1}", (object) uri, (object) ex.Message);
        this.m_log.LogError(errorMessage);
        sendMessageCallback(new MessageResponse()
        {
          ResponsePayload = (string) null,
          ResponseReceived = false,
          ErrorMessage = errorMessage
        });
      }
    }

    private void ReadCallbackRequest(
      IAsyncResult asyncResult,
      HttpWebRequest request,
      MessagePayload dataToSend,
      Action<MessageResponse> sendMessageCallback)
    {
      try
      {
        using (Stream requestStream = request.EndGetRequestStream(asyncResult))
        {
          try
          {
            requestStream.Write(dataToSend.Data, 0, dataToSend.Data.Length);
            requestStream.Flush();
          }
          finally
          {
            requestStream.Close();
          }
        }
        request.BeginGetResponse((AsyncCallback) (r =>
        {
          try
          {
            string str = (string) null;
            Stream stream = (Stream) null;
            using (WebResponse response = request.EndGetResponse(r))
            {
              try
              {
                stream = response.GetResponseStream();
                if (stream != null)
                {
                  using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                  {
                    str = streamReader.ReadToEnd();
                    streamReader.Close();
                  }
                }
              }
              finally
              {
                stream?.Close();
                response?.Close();
              }
              MessageSendingHelper.AddToDailyBandwidth(this.m_policy, (long) dataToSend.Data.Length);
              MessageSendingHelper.AddToDailyBandwidth(this.m_policy, (long) str.Length);
              sendMessageCallback(new MessageResponse()
              {
                ErrorMessage = (string) null,
                ResponsePayload = str,
                ResponseReceived = true
              });
            }
          }
          catch (WebException ex)
          {
            string errorMessage = string.Format("Failed to read server response; HTTP status {0}: {1}", (object) ex.Status, (object) ex.Message);
            this.m_log.LogError(errorMessage);
            sendMessageCallback(new MessageResponse()
            {
              ErrorMessage = errorMessage,
              ResponsePayload = (string) null,
              ResponseReceived = false
            });
          }
          catch (Exception ex)
          {
            string errorMessage = string.Format("Failed to correctly receive server response: {0}", (object) ex.Message);
            this.m_log.LogError(errorMessage);
            sendMessageCallback(new MessageResponse()
            {
              ErrorMessage = errorMessage,
              ResponsePayload = (string) null,
              ResponseReceived = false
            });
          }
        }), (object) null);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("Failed to send data to analytics server: {0}", (object) ex.Message);
        this.m_log.LogError(errorMessage);
        sendMessageCallback(new MessageResponse()
        {
          ErrorMessage = errorMessage,
          ResponsePayload = (string) null,
          ResponseReceived = false
        });
      }
    }
  }
}
