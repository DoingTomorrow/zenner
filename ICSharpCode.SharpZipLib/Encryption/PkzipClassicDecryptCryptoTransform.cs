// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Encryption.PkzipClassicDecryptCryptoTransform
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;
using System.Security.Cryptography;

#nullable disable
namespace ICSharpCode.SharpZipLib.Encryption
{
  internal class PkzipClassicDecryptCryptoTransform : 
    PkzipClassicCryptoBase,
    ICryptoTransform,
    IDisposable
  {
    internal PkzipClassicDecryptCryptoTransform(byte[] keyBlock) => this.SetKeys(keyBlock);

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      byte[] outputBuffer = new byte[inputCount];
      this.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
      return outputBuffer;
    }

    public int TransformBlock(
      byte[] inputBuffer,
      int inputOffset,
      int inputCount,
      byte[] outputBuffer,
      int outputOffset)
    {
      for (int index = inputOffset; index < inputOffset + inputCount; ++index)
      {
        byte ch = (byte) ((uint) inputBuffer[index] ^ (uint) this.TransformByte());
        outputBuffer[outputOffset++] = ch;
        this.UpdateKeys(ch);
      }
      return inputCount;
    }

    public bool CanReuseTransform => true;

    public int InputBlockSize => 1;

    public int OutputBlockSize => 1;

    public bool CanTransformMultipleBlocks => true;

    public void Dispose() => this.Reset();
  }
}
