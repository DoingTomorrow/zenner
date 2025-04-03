// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.WCFRelated.ServiceClient
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.Structures;
using MSS.DTO.Sync;
using MSS.Interfaces;
using MSS.Utils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace MSS.Business.Modules.WCFRelated
{
  public class ServiceClient : ServiceProxyBase<IService>
  {
    private readonly string ip;

    public ServiceClient(string ip)
      : base(ip)
    {
      this.ip = ip;
    }

    protected override string GetEndpointAdress(string ip)
    {
      return string.Format("net.tcp://{0}:13758/MSSService", (object) ip);
    }

    public string InsertNewClient(string clientId, string userName, Guid userId)
    {
      string result = (string) null;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => result = d.InsertNewClient(clientId, userName, userId)));
      return result;
    }

    public byte[] GetOrders()
    {
      byte[] res = (byte[]) null;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => res = d.GetOrders()));
      return res;
    }

    public byte[] GetData(string t, object predicate)
    {
      byte[] res = (byte[]) null;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => res = d.GetData(t, (object) predicate.ToString())));
      return res;
    }

    public void UpdateEntities(string t, object items, Guid idUser)
    {
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => d.UpdateEntities(t, items, idUser)));
    }

    public string GetClientStatus(string clientId)
    {
      string result = (string) null;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => result = d.GetClientStatus(clientId)));
      return result;
    }

    public async Task<string> GetClientStatusAsync(string clientId)
    {
      string test = await this.Channel.GetClientStatusAsync(clientId);
      return test;
    }

    public Dictionary<Guid, string> GetSynctonizationExtraInformation(
      Dictionary<Guid, Type> conflictsDictionary)
    {
      Dictionary<string, string> extraCollection = (Dictionary<string, string>) null;
      List<string> guids = new List<string>();
      List<string> types = new List<string>();
      foreach (KeyValuePair<Guid, Type> conflicts in conflictsDictionary)
      {
        guids.Add(conflicts.Key.ToString());
        types.Add(conflicts.Value.Name);
      }
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => extraCollection = d.GetSynctonizationExtraInformation(guids, types)));
      return extraCollection.ToDictionary<KeyValuePair<string, string>, Guid, string>((Func<KeyValuePair<string, string>, Guid>) (node => Guid.Parse(node.Key)), (Func<KeyValuePair<string, string>, string>) (node => node.Value));
    }

    public StructureNodeDTOListsSerializable GetStructures(
      string searchText,
      StructureTypeEnum structureType)
    {
      StructureNodeDTOListsSerializable ch = (StructureNodeDTOListsSerializable) null;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => ch = d.GetStructures(searchText, structureType)));
      return ch;
    }

    public void SendReadingValues(List<MeterReadingValue> readingValues)
    {
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => d.SaveReadingValues(readingValues)));
    }

    public void ReceiveData(string t, object items)
    {
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => d.ReceiveData(t, items as byte[])));
    }

    public void TestConnection()
    {
      try
      {
        PingReply pingReply = new Ping().Send(this.ip, 10);
        bool flag = true;
        if (pingReply != null)
        {
          MessageHandler.LogDebug("ping ok!");
          flag = pingReply.Status == IPStatus.Success;
        }
        MessageHandler.LogDebug("ping tested");
        if (!flag)
          throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
        using (TcpClient tcpClient = new TcpClient())
        {
          IAsyncResult asyncResult = tcpClient.BeginConnect(this.ip, 13758, (AsyncCallback) null, (object) null);
          WaitHandle asyncWaitHandle = asyncResult.AsyncWaitHandle;
          try
          {
            if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10.0), false))
            {
              tcpClient.Close();
              throw new TimeoutException();
            }
            tcpClient.EndConnect(asyncResult);
            MessageHandler.LogDebug("tcp ok!");
            this.Channel.TestConnection();
            MessageHandler.LogDebug("channel ok!!");
          }
          catch (SocketException ex)
          {
            MessageHandler.LogException((Exception) ex);
            throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
          }
          catch (TimeoutException ex)
          {
            MessageHandler.LogException((Exception) ex);
            throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
          }
          finally
          {
            asyncWaitHandle.Close();
          }
        }
      }
      catch (Exception ex)
      {
        throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
      }
    }

    public void SetClientState(Guid[] listOfObjects)
    {
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => d.SetClientState(listOfObjects)));
    }

    public Dictionary<Guid, SimpleMetadata> GetTemporaryMetadataDictionary()
    {
      Dictionary<Guid, SimpleMetadata> result = new Dictionary<Guid, SimpleMetadata>();
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => result = d.GetTemporaryMetadataDictionary()));
      return result;
    }

    public SerializedSyncResponse DownloadFromServer(
      Guid userId,
      DateTime lastSuccessfulDownload,
      List<Guid> existingRootNodes,
      List<Guid> existingOrders,
      bool userMasterPool)
    {
      SerializedSyncResponse value = (SerializedSyncResponse) null;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => value = d.DownloadFromServer(userId, lastSuccessfulDownload, existingRootNodes, existingOrders, userMasterPool)));
      return value;
    }

    public bool UploadToServer(SerializedSyncResponse changeset)
    {
      bool value = true;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => value = d.UploadToServer(changeset)));
      return value;
    }

    public bool LockDownloadedEntitiesFromServer(List<Guid> orderToUpdateIdList, Guid lockByUser)
    {
      bool value = true;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => value = d.LockDownloadedEntitiesFromServer(orderToUpdateIdList, lockByUser)));
      return value;
    }

    public string GetTimeFromServer()
    {
      string value = (string) null;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => value = d.GetTimeFromServer()));
      return value;
    }

    public bool SaveMinomatOnServer(MinomatSerializableDTO minomat)
    {
      bool value = true;
      this.Use((ServiceProxyBase<IService>.UseServiceDelegate) (d => value = d.SaveMinomatOnServer(minomat)));
      return value;
    }
  }
}
