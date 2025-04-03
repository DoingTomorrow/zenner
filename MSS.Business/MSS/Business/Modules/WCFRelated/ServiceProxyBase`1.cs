// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.WCFRelated.ServiceProxyBase`1
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Utils.Utils;
using System;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

#nullable disable
namespace MSS.Business.Modules.WCFRelated
{
  public abstract class ServiceProxyBase<T> : IDisposable where T : class
  {
    private readonly string _serviceEndpointUri;
    private readonly object _sync = new object();
    private IChannelFactory<T> _channelFactory;
    private T _channel;
    private bool _disposed = false;

    protected ServiceProxyBase(string ip) => this._serviceEndpointUri = this.GetEndpointAdress(ip);

    protected abstract string GetEndpointAdress(string ip);

    ~ServiceProxyBase() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposeManaged)
    {
      if (this._disposed || !disposeManaged)
        return;
      lock (this._sync)
      {
        this.CloseChannel();
        if (this._channelFactory != null)
          ((IDisposable) this._channelFactory).Dispose();
        this._channel = default (T);
        this._channelFactory = (IChannelFactory<T>) null;
      }
    }

    protected T Channel
    {
      get
      {
        this.Initialize();
        return this._channel;
      }
    }

    protected void CloseChannel()
    {
      if ((object) this._channel == null)
        return;
      ((ICommunicationObject) (object) this._channel).Close();
    }

    private void Initialize()
    {
      lock (this._sync)
      {
        if ((object) this._channel != null)
          return;
        this._channelFactory = (IChannelFactory<T>) new ChannelFactory<T>((Binding) this.GetNetTcpBinding());
        this._channel = this._channelFactory.CreateChannel(new EndpointAddress(this._serviceEndpointUri));
        ((ICommunicationObject) (object) this._channel).Faulted += new EventHandler(this.ServiceProxyBase_Faulted);
        ((ICommunicationObject) (object) this._channel).Opened += new EventHandler(this.ServiceProxyBase_Opened);
      }
    }

    private void ServiceProxyBase_Opened(object sender, EventArgs e)
    {
      MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = true;
    }

    private void ServiceProxyBase_Faulted(object sender, EventArgs e)
    {
      MSS.Business.Utils.AppContext.Current.IsServerAvailableAndStatusAccepted = false;
    }

    private NetTcpBinding GetNetTcpBinding()
    {
      NetTcpBinding netTcpBinding = new NetTcpBinding()
      {
        TransactionFlow = false
      };
      netTcpBinding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
      netTcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
      netTcpBinding.Security.Mode = SecurityMode.None;
      netTcpBinding.MaxReceivedMessageSize = (long) int.MaxValue;
      netTcpBinding.OpenTimeout = new TimeSpan(0, 10, 0);
      netTcpBinding.CloseTimeout = new TimeSpan(0, 10, 0);
      netTcpBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
      netTcpBinding.SendTimeout = new TimeSpan(0, 10, 0);
      netTcpBinding.MaxReceivedMessageSize = 734003200L;
      netTcpBinding.MaxBufferSize = 734003200;
      netTcpBinding.MaxBufferPoolSize = 734003200L;
      return netTcpBinding;
    }

    public void Use(ServiceProxyBase<T>.UseServiceDelegate codeBlock)
    {
      this.Initialize();
      bool flag = false;
      ICommunicationObject communicationObject = (ICommunicationObject) null;
      if ((object) this._channel != null)
        communicationObject = (ICommunicationObject) (object) this._channel;
      try
      {
        codeBlock(this._channel);
        communicationObject.Close();
        flag = true;
      }
      catch (CommunicationObjectAbortedException ex)
      {
        throw;
      }
      catch (CommunicationObjectFaultedException ex)
      {
        throw;
      }
      catch (MessageSecurityException ex)
      {
        throw;
      }
      catch (ChannelTerminatedException ex)
      {
        communicationObject?.Abort();
        throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
      }
      catch (ServerTooBusyException ex)
      {
        communicationObject?.Abort();
        throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
      }
      catch (EndpointNotFoundException ex)
      {
        communicationObject?.Abort();
        throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
      }
      catch (FaultException ex)
      {
        MessageHandler.LogException((Exception) ex);
        communicationObject?.Abort();
        throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
      }
      catch (CommunicationException ex)
      {
        communicationObject?.Abort();
        throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
      }
      catch (TimeoutException ex)
      {
        communicationObject?.Abort();
        throw new BaseApplicationException(MessageCodes.Server_Not_Available.GetStringValue());
      }
      catch (ObjectDisposedException ex)
      {
      }
      finally
      {
        if (!flag && communicationObject != null)
          communicationObject.Abort();
      }
    }

    public delegate void UseServiceDelegate(T proxy) where T : class;
  }
}
