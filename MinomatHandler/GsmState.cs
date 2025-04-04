// Decompiled with JetBrains decompiler
// Type: MinomatHandler.GsmState
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class GsmState
  {
    private static Logger logger = LogManager.GetLogger(nameof (GsmState));

    public GsmStateA StateA { get; set; }

    public GsmStateB StateB { get; set; }

    public static GsmState Parse(byte[] payload)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be empty!");
      if (payload.Length < 3)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the GsmState! Wrong length of the payload. Payload: {0}", (object) Util.ByteArrayToHexString(payload)));
        GsmState.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return new GsmState()
      {
        StateA = (GsmStateA) Enum.ToObject(typeof (GsmStateA), payload[1]),
        StateB = (GsmStateB) Enum.ToObject(typeof (GsmStateB), payload[2])
      };
    }

    public override string ToString()
    {
      return string.Format("StateA: {0}, StateB: {1}", (object) this.StateA, (object) this.StateB);
    }
  }
}
