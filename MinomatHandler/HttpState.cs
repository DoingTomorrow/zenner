// Decompiled with JetBrains decompiler
// Type: MinomatHandler.HttpState
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class HttpState
  {
    private static Logger logger = LogManager.GetLogger(nameof (HttpState));

    public HttpCondition State { get; private set; }

    public EndPoint Endpoint { get; set; }

    public static HttpState Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be empty!");
      if (payload.Length < 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the HttpState! Wrong length of the payload. Payload: {0}", (object) Util.ByteArrayToHexString(payload)));
        HttpState.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return new HttpState()
      {
        State = (HttpCondition) Enum.ToObject(typeof (HttpCondition), payload[1]),
        Endpoint = EndPoint.Parse(payload, 2)
      };
    }

    public override string ToString()
    {
      return this.Endpoint != null ? string.Format("State: {0}, {1}", (object) this.State, (object) this.Endpoint) : string.Format("State: {0}", (object) this.State);
    }
  }
}
