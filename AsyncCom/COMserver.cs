// Decompiled with JetBrains decompiler
// Type: AsyncCom.COMserver
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using NLog;
using System;
using System.Collections;

#nullable disable
namespace AsyncCom
{
  public sealed class COMserver
  {
    private static Logger logger = LogManager.GetLogger(nameof (COMserver));
    public SortedList RemoteComs = new SortedList();
    public string IPAddress = "";
    public bool Online = false;
    public string Name = "";
    public string LastSeen = "";
    public string Traffic = "";

    public bool Update(AsyncIP MyAsyncIP)
    {
      if (MyAsyncIP == null)
        return false;
      COMserver.logger.Trace("Update COMServer called...");
      this.RemoteComs.Clear();
      string ReceivedData;
      if (!MyAsyncIP.TransmitControlCommand("showport") || !MyAsyncIP.ReceiveControlBlock(out ReceivedData, "showport", "->") || string.IsNullOrEmpty(ReceivedData))
        return false;
      string[] strArray1 = ReceivedData.Split('\r');
      if (strArray1.Length > 1)
      {
        int key = 0;
        RemoteCom remoteCom = (RemoteCom) null;
        foreach (string str1 in strArray1)
        {
          if (str1.StartsWith("TCP Port"))
          {
            remoteCom = new RemoteCom();
            ++key;
            remoteCom.Port = Convert.ToInt32(str1.Remove(0, 8));
          }
          if (str1 == "-")
            break;
          if (remoteCom != null)
          {
            string[] strArray2 = str1.Split(':');
            string str2 = strArray2[0];
            if (str2 != null)
            {
              switch (str2.Length)
              {
                case 1:
                  if (str2 == "-")
                    break;
                  break;
                case 8:
                  if (str2 == "  device")
                  {
                    remoteCom.LinuxDevice = strArray2[1].TrimEnd('\n');
                    if (!this.RemoteComs.ContainsKey((object) key))
                      this.RemoteComs.Add((object) key, (object) remoteCom);
                    remoteCom = (RemoteCom) null;
                    break;
                  }
                  break;
                case 9:
                  if (str2 == "  timeout")
                  {
                    remoteCom.Timeout = Convert.ToInt32(strArray2[1]);
                    break;
                  }
                  break;
                case 14:
                  if (str2 == "  enable state")
                    break;
                  break;
                case 15:
                  if (str2 == "  device config")
                  {
                    remoteCom.PortConfig = strArray2[1].TrimEnd('\n');
                    break;
                  }
                  break;
                case 17:
                  if (str2 == "  device controls")
                    break;
                  break;
                case 21:
                  switch (str2[2])
                  {
                    case 'b':
                      if (str2 == "  bytes read from TCP")
                      {
                        remoteCom.BytesTCP_IN = (long) Convert.ToInt32(strArray2[1]);
                        break;
                      }
                      break;
                    case 'd':
                      if (str2 == "  device to tcp state")
                        break;
                      break;
                    case 't':
                      if (str2 == "  tcp to device state")
                        break;
                      break;
                  }
                  break;
                case 22:
                  if (str2 == "  bytes written to TCP")
                  {
                    remoteCom.BytesTCP_IN = (long) Convert.ToInt32(strArray2[1]);
                    break;
                  }
                  break;
                case 24:
                  if (str2 == "  bytes read from device")
                  {
                    remoteCom.BytesTCP_IN = (long) Convert.ToInt32(strArray2[1]);
                    break;
                  }
                  break;
                case 25:
                  if (str2 == "  bytes written to device")
                  {
                    remoteCom.BytesTCP_IN = (long) Convert.ToInt32(strArray2[1]);
                    break;
                  }
                  break;
                case 35:
                  if (str2 == "  connected to (or last connection)" && strArray2.Length == 3)
                  {
                    remoteCom.ConnectedTo = strArray2[1].Trim();
                    break;
                  }
                  break;
              }
            }
          }
        }
      }
      return this.RemoteComs.Count != 0;
    }
  }
}
