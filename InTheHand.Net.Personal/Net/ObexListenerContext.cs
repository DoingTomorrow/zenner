// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexListenerContext
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Sockets;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net
{
  public class ObexListenerContext
  {
    private byte[] buffer;
    private ObexListenerRequest request;
    private WebHeaderCollection headers = new WebHeaderCollection();
    private MemoryStream bodyStream = new MemoryStream();
    private EndPoint localEndPoint;
    private EndPoint remoteEndPoint;
    private ushort remoteMaxPacket;

    internal ObexListenerContext(SocketAdapter s)
    {
      this.buffer = new byte[8192];
      this.localEndPoint = s.LocalEndPoint;
      this.remoteEndPoint = s.RemoteEndPoint;
      bool moretoreceive = true;
      bool putCompleted = false;
      while (moretoreceive)
      {
        int num1 = 0;
        try
        {
          int num2;
          for (; num1 < 3; num1 += num2)
          {
            num2 = s.Receive(this.buffer, num1, 3 - num1, SocketFlags.None);
            if (num2 == 0)
            {
              moretoreceive = false;
              if (num1 != 0)
                throw new EndOfStreamException("Connection lost.");
              break;
            }
          }
        }
        catch (SocketException ex)
        {
          this.HandleConnectionError((Exception) ex);
        }
        if (num1 == 3)
        {
          ObexMethod method = (ObexMethod) this.buffer[0];
          short num3 = (short) ((int) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(this.buffer, 1)) - 3);
          int num4;
          if (num3 > (short) 0)
          {
            for (int index = 0; index < (int) num3; index += num4)
            {
              int size = (int) num3 - index;
              num4 = s.Receive(this.buffer, index + 3, size, SocketFlags.None);
              if (num4 == 0)
                throw new EndOfStreamException("Connection lost.");
            }
          }
          byte[] buffer = this.HandleAndMakeResponse(ref moretoreceive, ref putCompleted, num1, method);
          try
          {
            if (buffer != null)
              s.Send(buffer);
          }
          catch (Exception ex)
          {
            this.HandleConnectionError(ex);
          }
        }
        else
          moretoreceive = false;
      }
      s.Close();
      s = (SocketAdapter) null;
      if (!putCompleted)
        throw new ProtocolViolationException("No PutFinal received.");
      this.request = new ObexListenerRequest(this.bodyStream.ToArray(), this.headers, this.localEndPoint, this.remoteEndPoint);
    }

    private byte[] HandleAndMakeResponse(
      ref bool moretoreceive,
      ref bool putCompleted,
      int received,
      ObexMethod method)
    {
      byte[] numArray;
      switch (method)
      {
        case ObexMethod.Put:
          if (!putCompleted)
          {
            ObexParser.ParseHeaders(this.buffer, false, ref this.remoteMaxPacket, (Stream) this.bodyStream, this.headers);
            numArray = new byte[3]
            {
              (byte) 144,
              (byte) 0,
              (byte) 3
            };
            break;
          }
          goto case ObexMethod.PutFinal;
        case ObexMethod.Connect:
          ObexParser.ParseHeaders(this.buffer, true, ref this.remoteMaxPacket, (Stream) this.bodyStream, this.headers);
          numArray = new byte[7]
          {
            (byte) 160,
            (byte) 0,
            (byte) 7,
            (byte) 16,
            (byte) 0,
            (byte) 32,
            (byte) 0
          };
          break;
        case ObexMethod.Disconnect:
          ObexParser.ParseHeaders(this.buffer, false, ref this.remoteMaxPacket, (Stream) this.bodyStream, this.headers);
          numArray = new byte[3]
          {
            (byte) 160,
            (byte) 0,
            (byte) 3
          };
          moretoreceive = false;
          break;
        case ObexMethod.PutFinal:
          if (putCompleted)
          {
            numArray = new byte[3]
            {
              (byte) (ObexStatusCode.Forbidden | ObexStatusCode.Final),
              (byte) 0,
              (byte) 3
            };
            moretoreceive = false;
            break;
          }
          ObexParser.ParseHeaders(this.buffer, false, ref this.remoteMaxPacket, (Stream) this.bodyStream, this.headers);
          numArray = new byte[3]
          {
            (byte) 160,
            (byte) 0,
            (byte) 3
          };
          putCompleted = true;
          break;
        default:
          numArray = new byte[3]
          {
            (byte) 81,
            (byte) 0,
            (byte) 3
          };
          break;
      }
      return numArray;
    }

    private void HandleConnectionError(Exception ex) => throw ex;

    public ObexListenerRequest Request => this.request;
  }
}
