// Decompiled with JetBrains decompiler
// Type: AForge.CommunicationBufferEventArgs
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;

#nullable disable
namespace AForge
{
  public class CommunicationBufferEventArgs : EventArgs
  {
    private readonly byte[] message;
    private readonly int index;
    private readonly int length;

    public int MessageLength => this.length;

    public CommunicationBufferEventArgs(byte[] message)
    {
      this.message = message;
      this.index = 0;
      this.length = message.Length;
    }

    public CommunicationBufferEventArgs(byte[] buffer, int index, int length)
    {
      this.message = buffer;
      this.index = index;
      this.length = length;
    }

    public byte[] GetMessage()
    {
      byte[] destinationArray = new byte[this.length];
      Array.Copy((Array) this.message, this.index, (Array) destinationArray, 0, this.length);
      return destinationArray;
    }

    public string GetMessageString()
    {
      return BitConverter.ToString(this.message, this.index, this.length);
    }
  }
}
