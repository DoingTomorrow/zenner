// Decompiled with JetBrains decompiler
// Type: AsyncCom.MeterVPN
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using AsyncCom.MeterVPNServer;
using NLog;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace AsyncCom
{
  public class MeterVPN
  {
    private static Logger logger = LogManager.GetLogger(nameof (MeterVPN));
    internal SortedList COMservers = new SortedList();
    internal string SelectedCOMserver = "";

    public bool Update(AsyncIP MyAsyncIP)
    {
      MeterVPN.logger.Info("Update COMServer list");
      AsyncCom.MeterVPNServer.COMserver[] COMservers;
      if (!MyAsyncIP.GetCOMServersFromMeterVPN(out COMservers))
      {
        MeterVPN.logger.Error("MyAsyncIP.GetCOMServersFromMeterVPN(out AvailableCOMservers)) == false");
        return false;
      }
      SortedList sortedList = new SortedList();
      for (int index = 0; index < this.COMservers.Count; ++index)
      {
        if (IPAddress.TryParse(((COMserver) this.COMservers.GetByIndex(index)).Name, out IPAddress _))
          sortedList.Add((object) ((COMserver) this.COMservers.GetByIndex(index)).Name, (object) (COMserver) this.COMservers.GetByIndex(index));
      }
      this.COMservers = sortedList;
      foreach (AsyncCom.MeterVPNServer.COMserver coMserver1 in COMservers)
      {
        COMserver coMserver2 = new COMserver();
        coMserver2.IPAddress = coMserver1.IP;
        coMserver2.Online = coMserver1.Online;
        coMserver2.Name = coMserver1.Name;
        coMserver2.LastSeen = coMserver1.LastSeen;
        coMserver2.Traffic = coMserver1.Traffic;
        if (!this.COMservers.ContainsKey((object) coMserver1.Cert))
          this.COMservers.Add((object) coMserver1.Cert, (object) coMserver2);
      }
      return true;
    }

    public bool AddCOMserver(AsyncIP MyAsyncIP, string Cert, string Name, string Password)
    {
      return MyAsyncIP.AddCOMserverToCustomer(Cert, Name, Password);
    }

    public bool DelCOMserver(AsyncIP MyAsyncIP, string Cert)
    {
      return MyAsyncIP.DelCOMserverFromCustomer(Cert);
    }

    public bool ModCOMserver(AsyncIP MyAsyncIP, string Cert, string Name)
    {
      return MyAsyncIP.ModCOMserver(Cert, Name);
    }

    public static AsyncCom.MeterVPNServer.COMserver[] ReadListOfCOMserver()
    {
      using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
      {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("1.0.0.1"), 80);
        if (!socket.BeginConnect((EndPoint) remoteEP, (AsyncCallback) null, (object) null).AsyncWaitHandle.WaitOne(500, false))
          return (AsyncCom.MeterVPNServer.COMserver[]) null;
      }
      MeterVPNService meterVpnService = new MeterVPNService();
      meterVpnService.Proxy = GlobalProxySelection.GetEmptyWebProxy();
      return meterVpnService.GetCOMservers("test");
    }
  }
}
