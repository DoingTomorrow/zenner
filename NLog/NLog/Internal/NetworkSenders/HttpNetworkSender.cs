// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.HttpNetworkSender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Net;

#nullable disable
namespace NLog.Internal.NetworkSenders
{
  internal class HttpNetworkSender : NetworkSender
  {
    private readonly Uri _addressUri;

    public HttpNetworkSender(string url)
      : base(url)
    {
      this._addressUri = new Uri(this.Address);
    }

    protected override void DoSend(
      byte[] bytes,
      int offset,
      int length,
      AsyncContinuation asyncContinuation)
    {
      WebRequest webRequest = WebRequest.Create(this._addressUri);
      webRequest.Method = "POST";
      AsyncCallback onResponse = (AsyncCallback) (r =>
      {
        try
        {
          using (webRequest.EndGetResponse(r))
            ;
          asyncContinuation((Exception) null);
        }
        catch (Exception ex)
        {
          if (ex.MustBeRethrownImmediately())
            throw;
          else
            asyncContinuation(ex);
        }
      });
      webRequest.BeginGetRequestStream((AsyncCallback) (r =>
      {
        try
        {
          using (Stream requestStream = webRequest.EndGetRequestStream(r))
            requestStream.Write(bytes, offset, length);
          webRequest.BeginGetResponse(onResponse, (object) null);
        }
        catch (Exception ex)
        {
          if (ex.MustBeRethrown())
            throw;
          else
            asyncContinuation(ex);
        }
      }), (object) null);
    }
  }
}
