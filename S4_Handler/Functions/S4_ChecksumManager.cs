// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_ChecksumManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using HandlerLib.NFC;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  public class S4_ChecksumManager
  {
    private NfcDeviceCommands NfcCmd;

    internal S4_ChecksumManager(NfcDeviceCommands nfcCmd) => this.NfcCmd = nfcCmd;

    public async Task<S4_ChecksumCheckResults> CheckAllChecksums(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] result = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.ChecksumManagement, new byte[1]);
      S4_ChecksumCheckResults uint16 = (S4_ChecksumCheckResults) BitConverter.ToUInt16(result, 2);
      result = (byte[]) null;
      return uint16;
    }

    public async Task GenerateAllChecksums(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte[] numArray = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.ChecksumManagement, new byte[1]
      {
        (byte) 1
      });
    }

    public async Task GenerateFirmwareChecksums(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] numArray = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.ChecksumManagement, new byte[1]
      {
        (byte) 2
      });
    }

    public async Task GenerateParameterChecksum(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] numArray = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.ChecksumManagement, new byte[1]
      {
        (byte) 3
      });
    }
  }
}
