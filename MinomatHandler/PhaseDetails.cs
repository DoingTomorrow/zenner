// Decompiled with JetBrains decompiler
// Type: MinomatHandler.PhaseDetails
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class PhaseDetails
  {
    private static Logger logger = LogManager.GetLogger(nameof (PhaseDetails));

    public PhaseMode Phase { get; set; }

    public PhaseState State { get; set; }

    public byte SubPhase { get; set; }

    public static PhaseDetails Parse(byte[] payload)
    {
      if (payload.Length != 6)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 6, Payload buffer: {1}", (object) payload.Length, (object) Util.ByteArrayToHexString(payload)));
        PhaseDetails.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (!Enum.IsDefined(typeof (PhaseMode), (object) payload[2]))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Unknown PhaseMode detected! Value: 0x{0}, Payload buffer: {1}", (object) payload[2].ToString("X2"), (object) Util.ByteArrayToHexString(payload)));
        PhaseDetails.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (!Enum.IsDefined(typeof (PhaseState), (object) payload[3]))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Unknown PhaseState detected! Value: 0x{0}, Payload buffer: {1}", (object) payload[3].ToString("X2"), (object) Util.ByteArrayToHexString(payload)));
        PhaseDetails.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return new PhaseDetails()
      {
        Phase = (PhaseMode) Enum.ToObject(typeof (PhaseMode), payload[2]),
        State = (PhaseState) Enum.ToObject(typeof (PhaseState), payload[3]),
        SubPhase = payload[4]
      };
    }

    public override string ToString()
    {
      return string.Format("Phase: {0}, State: {1}, SubPhase: {2}", (object) this.Phase, (object) this.State, (object) this.SubPhase);
    }
  }
}
