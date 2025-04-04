// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiFrame
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace MinomatHandler
{
  public sealed class SCGiFrame : List<SCGiPacket>
  {
    public SCGiFrame()
    {
    }

    public SCGiFrame(SCGiPacket packet) => this.Add(packet);

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (SCGiPacket scGiPacket in (List<SCGiPacket>) this)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append("\n");
        stringBuilder.Append((object) scGiPacket);
      }
      return stringBuilder.ToString();
    }

    internal static byte[] GetData(byte[] stuffedBuffer)
    {
      IEnumerable<byte[]> numArrays = stuffedBuffer != null && stuffedBuffer.Length >= 1 ? SCGiFrame.Split(stuffedBuffer, (byte) 170) : throw new ArgumentException(nameof (stuffedBuffer));
      List<byte> byteList = new List<byte>();
      foreach (byte[] stuffedBuffer1 in numArrays)
      {
        SCGiPacket scGiPacket = SCGiPacket.Parse(stuffedBuffer1);
        byteList.AddRange((IEnumerable<byte>) scGiPacket.Payload);
      }
      return byteList.ToArray();
    }

    private static IEnumerable<byte[]> Split(byte[] buffer, byte splitByte)
    {
      List<byte> bytes = new List<byte>();
      byte[] numArray = buffer;
      for (int index = 0; index < numArray.Length; ++index)
      {
        byte b = numArray[index];
        if ((int) b == (int) splitByte && bytes.Count > 0)
        {
          yield return bytes.ToArray();
          bytes.Clear();
        }
        bytes.Add(b);
      }
      numArray = (byte[]) null;
      yield return bytes.ToArray();
    }
  }
}
