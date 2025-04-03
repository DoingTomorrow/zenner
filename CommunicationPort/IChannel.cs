// Decompiled with JetBrains decompiler
// Type: CommunicationPort.IChannel
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using System;

#nullable disable
namespace CommunicationPort
{
  internal interface IChannel : IDisposable
  {
    string PortName { get; set; }

    bool IsOpen { get; }

    int BytesToRead { get; }

    void Open();

    void Close();

    void DiscardInBuffer();

    void DiscardOutBuffer();

    int ReadByte();

    int Read(byte[] buffer, int offset, int count);

    void Write(byte[] buffer, int offset, int count);

    void Write(string text);

    void WriteBaudrateCarrier(int numberOfBytes);
  }
}
