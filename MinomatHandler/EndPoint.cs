// Decompiled with JetBrains decompiler
// Type: MinomatHandler.EndPoint
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using System.Text;

#nullable disable
namespace MinomatHandler
{
  public sealed class EndPoint
  {
    private static Logger logger = LogManager.GetLogger(nameof (EndPoint));

    public ushort Port { get; set; }

    public string Servername { get; set; }

    public EndPoint()
    {
      this.Port = (ushort) 0;
      this.Servername = string.Empty;
    }

    public static EndPoint Parse(byte[] payload, int offset)
    {
      if (payload == null)
        throw new ArgumentNullException("Payload can not be empty!");
      if (offset < 0)
        throw new ArgumentException("Offset can not be negativ!");
      EndPoint endPoint = new EndPoint();
      if (payload.Length >= 2 + offset)
        endPoint.Port = BitConverter.ToUInt16(payload, offset);
      if (payload.Length > 2 + offset)
        endPoint.Servername = Encoding.ASCII.GetString(payload, 2 + offset, payload.Length - (2 + offset)).TrimEnd(new char[1]).Trim();
      return endPoint;
    }

    public override string ToString()
    {
      return string.Format("Port: {0}, Servername: {1}", (object) this.Port, (object) this.Servername);
    }
  }
}
