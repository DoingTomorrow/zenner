// Decompiled with JetBrains decompiler
// Type: MinomatListener.HttpPacket
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

using NLog;
using System;
using System.Text;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatListener
{
  public sealed class HttpPacket
  {
    private static Logger logger = LogManager.GetLogger(nameof (HttpPacket));

    public string RemoteFile { get; private set; }

    public string Host { get; private set; }

    public byte[] Content { get; private set; }

    public HttpPacketType Type
    {
      get => this.Content == null ? HttpPacketType.None : (HttpPacketType) this.Content[0];
    }

    public static HttpPacket TryParse(byte[] buffer)
    {
      string str1 = buffer != null ? Encoding.ASCII.GetString(buffer) : throw new ArgumentNullException(nameof (buffer));
      if (string.IsNullOrEmpty(str1))
        return (HttpPacket) null;
      string[] strArray = str1.Split(new string[1]{ "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray == null || strArray.Length < 4 || !strArray[0].StartsWith("POST"))
        return (HttpPacket) null;
      int num1 = strArray[0].IndexOf("POST ");
      int num2 = strArray[0].IndexOf(" HTTP/1.1");
      if (num1 < 0 || num2 < 0)
        return (HttpPacket) null;
      string str2 = strArray[0].Substring(num1 + 5, num2 - (num1 + 5));
      int num3 = strArray[1].IndexOf("Host: ");
      if (num3 < 0)
        return (HttpPacket) null;
      string str3 = strArray[1].Substring(num3 + 6);
      int num4 = strArray[2].IndexOf("Content-Length: ");
      if (num4 < 0)
        return (HttpPacket) null;
      string s = strArray[2].Substring(num4 + 16);
      int result;
      if (!int.TryParse(s, out result))
        return (HttpPacket) null;
      if (result <= 0)
        throw new Exception("Content length in HTTP frame can not be 0!");
      string str4 = "Content-Length: " + s + "\r\n\r\n";
      int num5 = str1.IndexOf(str4);
      if (num5 < 0)
        return (HttpPacket) null;
      int sourceIndex = num5 + str4.Length;
      int num6 = buffer.Length - sourceIndex;
      byte[] destinationArray = result <= num6 ? new byte[result] : throw new HttpPacketIsNotCompleteException(result - num6);
      Array.Copy((Array) buffer, sourceIndex, (Array) destinationArray, 0, destinationArray.Length);
      return new HttpPacket()
      {
        RemoteFile = str2,
        Host = str3,
        Content = destinationArray
      };
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.RemoteFile))
        return base.ToString();
      return string.Format("{0} {1} {2} {3}", (object) this.Type, (object) this.Host, (object) this.RemoteFile, (object) Util.ByteArrayToHexString(this.Content));
    }
  }
}
