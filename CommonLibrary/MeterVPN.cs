// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.MeterVPN
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using ZENNER.CommonLibrary.MeterVPNServer;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public static class MeterVPN
  {
    public static COMserver[] ReadListOfCOMserver()
    {
      if (!MeterVPN.Ping())
        return (COMserver[]) null;
      MeterVPNPortClient meterVpnPortClient = new MeterVPNPortClient((Binding) new BasicHttpBinding(), new EndpointAddress("http://1.0.0.1/server.php"));
      WebRequest.DefaultWebProxy = (IWebProxy) null;
      return meterVpnPortClient.GetCOMservers("test");
    }

    public static bool Ping()
    {
      using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
      {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("1.0.0.1"), 80);
        if (!socket.BeginConnect((EndPoint) remoteEP, (AsyncCallback) null, (object) null).AsyncWaitHandle.WaitOne(500, false))
          return false;
      }
      return true;
    }
  }
}
