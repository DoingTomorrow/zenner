// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.GMMMinomatConfigurator
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MinomatHandler;
using MSS.Business.Errors;
using MSS.Core.Model.DataCollectors;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class GMMMinomatConfigurator
  {
    private const int waitTime = 5000;
    private MinomatHandlerFunctions _minomatHandlerMaster;
    private MinomatHandlerFunctions _minomatHandlerSlave;
    private static GMMMinomatConfigurator _instance;
    public EventHandler<GMMMinomatConfiguratorResult> OnError;
    private bool _isMaster;
    private bool _isConnectionMandatory;

    private void InitializeMinomatHandler(bool isMaster, bool isConnectionMandatory)
    {
      this._isConnectionMandatory = isConnectionMandatory;
      if (!isConnectionMandatory)
        return;
      string port = GMMMinomatConfigurator.GetPort();
      DeviceModel deviceModel = GmmInterface.DeviceManager.GetDeviceModel(isMaster ? "Minomat V4 Master" : "Minomat V4 Slave");
      List<ProfileType> profileTypes = GmmInterface.DeviceManager.GetProfileTypes(deviceModel, MSS.Business.Utils.AppContext.Current.DefaultEquipment);
      ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(deviceModel, MSS.Business.Utils.AppContext.Current.DefaultEquipment, profileTypes[0]);
      connectionProfile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value = port;
      if (isMaster)
      {
        this._minomatHandlerMaster = GmmInterface.HandlerManager.CreateInstance<MinomatHandlerFunctions>(connectionProfile);
        this._minomatHandlerMaster.MyMinomatV4.OnError += new EventHandlerEx<Exception>(this.MyMinomatV4_OnError);
        this._minomatHandlerMaster.MyMinomatV4.OnMeasurementDataReceived += new EventHandler<MeasurementData>(this.MyMinomatV4_OnMeasurementDataReceived);
        this._minomatHandlerMaster.MyMinomatV4.OnMessage += new EventHandler<MinomatV4.StateEventArgs>(this.MyMinomatV4_OnMessage);
        this._minomatHandlerMaster.MyMinomatV4.OnMinomatV4ParameterReceived += new EventHandler<MinomatV4Parameter>(this.MyMinomatV4_OnMinomatV4ParameterReceived);
      }
      else
      {
        this._minomatHandlerSlave = GmmInterface.HandlerManager.CreateInstance<MinomatHandlerFunctions>(connectionProfile);
        this._minomatHandlerSlave.MyMinomatV4.OnError += new EventHandlerEx<Exception>(this.MyMinomatV4_OnError);
        this._minomatHandlerSlave.MyMinomatV4.OnMeasurementDataReceived += new EventHandler<MeasurementData>(this.MyMinomatV4_OnMeasurementDataReceived);
        this._minomatHandlerSlave.MyMinomatV4.OnMessage += new EventHandler<MinomatV4.StateEventArgs>(this.MyMinomatV4_OnMessage);
        this._minomatHandlerSlave.MyMinomatV4.OnMinomatV4ParameterReceived += new EventHandler<MinomatV4Parameter>(this.MyMinomatV4_OnMinomatV4ParameterReceived);
      }
    }

    private MinomatHandlerFunctions GetMinomatHandler()
    {
      return this._isMaster ? this._minomatHandlerMaster : this._minomatHandlerSlave;
    }

    public static GMMMinomatConfigurator GetInstance(bool isMaster, bool isConnectionMandatory)
    {
      lock (typeof (GMMJobsManager))
      {
        if (GMMMinomatConfigurator._instance == null)
          GMMMinomatConfigurator._instance = new GMMMinomatConfigurator();
        GMMMinomatConfigurator._instance._isMaster = isMaster;
        if (isMaster)
        {
          if (GMMMinomatConfigurator._instance._minomatHandlerMaster == null)
            GMMMinomatConfigurator._instance.InitializeMinomatHandler(true, isConnectionMandatory);
        }
        else if (GMMMinomatConfigurator._instance._minomatHandlerSlave == null)
          GMMMinomatConfigurator._instance.InitializeMinomatHandler(false, isConnectionMandatory);
        return GMMMinomatConfigurator._instance;
      }
    }

    private void MyMinomatV4_OnMinomatV4ParameterReceived(object sender, MinomatV4Parameter e)
    {
    }

    private void MyMinomatV4_OnMessage(object sender, MinomatV4.StateEventArgs e)
    {
    }

    private void MyMinomatV4_OnMeasurementDataReceived(object sender, MeasurementData e)
    {
    }

    private void MyMinomatV4_OnError(object sender, Exception e)
    {
      EventHandler<GMMMinomatConfiguratorResult> onError = this.OnError;
      if (onError == null)
        return;
      onError((object) this, new GMMMinomatConfiguratorResult()
      {
        IsSuccess = false,
        Message = e.Message
      });
    }

    public string GetMinolID()
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          if (!minomatHandler.MyMinomatV4.Connection.Open())
            throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
          return (minomatHandler.MyMinomatV4.GetMinolId() ?? throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription)).ToString();
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return "47444444";
      }
    }

    public bool ResetMinomat(Minomat minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          return GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId).IsSuccess && minomatHandler.MyMinomatV4.ResetConfiguration();
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return true;
      }
    }

    public GMMMinomatConfiguratorResult SetupMinomat(Minomat minomat, string installationNumber)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          MinomatV4 minomatV4_1 = minomatHandler.MyMinomatV4;
          GMMMinomatConfiguratorResult configuratorResult = GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId);
          if (!configuratorResult.IsSuccess)
            return configuratorResult;
          if (minomat.IsMaster)
          {
            ulong num = ulong.Parse(minomat.SessionKey);
            if (!minomatV4_1.SetAppInitialSettings(minomat.Challenge, minomat.GsmId, installationNumber, num.ToString("X16")))
              return new GMMMinomatConfiguratorResult()
              {
                IsSuccess = false
              };
            minomatV4_1.SetAPN(minomat.AccessPoint);
            minomatV4_1.SetSimPin(minomat.SimPin);
            minomatV4_1.SetGPRSUserName(minomat.UserId);
            minomatV4_1.SetGPRSPassword(minomat.UserPassword);
            string[] strArray = minomat.HostAndPort.Split(':');
            minomatV4_1.SetHttpServer(strArray[1], strArray[0]);
            minomatV4_1.SetHttpResourceName(minomat.Url);
            DateTime dateTime;
            if (minomat.RadioDetails.DueDate.HasValue)
            {
              MinomatV4 minomatV4_2 = minomatV4_1;
              DateTime? dueDate = minomat.RadioDetails.DueDate;
              dateTime = dueDate.Value;
              int month = dateTime.Month;
              dueDate = minomat.RadioDetails.DueDate;
              dateTime = dueDate.Value;
              int day = dateTime.Day;
              DateTime newDueDate = new DateTime(2000, month, day);
              minomatV4_2.SetModemDueDate(newDueDate);
            }
            MinomatV4 minomatV4_3 = minomatV4_1;
            dateTime = new DateTime(2001, 1, 1);
            DateTime timepoint = dateTime.AddSeconds((double) minomat.Polling);
            minomatV4_3.SetActionTimepoint(ActionMode.CreateHttp, ActionTimepointType.Interval, timepoint);
          }
          minomatV4_1.SetNodeId(minomat.RadioDetails.NodeId);
          minomatV4_1.SetRadioChannel(minomat.RadioDetails.Channel);
          minomatV4_1.SetNetworkId(minomat.RadioDetails.NetId);
          minomatV4_1.SetScenario((Scenario) minomat.Scenario.Code);
          return new GMMMinomatConfiguratorResult()
          {
            IsSuccess = true
          };
        }
        catch (Exception ex)
        {
          return this.HandleException(ex);
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = true
        };
      }
    }

    public GMMMinomatConfiguratorResult GetRoutingTable(
      MinomatSerializableDTO minomat,
      out RoutingTable routingTable)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          routingTable = (RoutingTable) null;
          GMMMinomatConfiguratorResult routingTable1 = GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId);
          if (!routingTable1.IsSuccess)
            return routingTable1;
          routingTable = minomatHandler.MyMinomatV4.GetRoutingTable();
          return new GMMMinomatConfiguratorResult()
          {
            IsSuccess = true
          };
        }
        catch (Exception ex)
        {
          routingTable = (RoutingTable) null;
          return this.HandleException(ex);
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        routingTable = new RoutingTable();
        RoutingRow routingRow1 = new RoutingRow();
        routingRow1.HopCount = (byte) new Random().Next(1, (int) byte.MaxValue);
        routingRow1.NodeId = (ushort) new Random().Next(1, 1000);
        routingRow1.ParentNodeId = (ushort) new Random().Next(1, 1000);
        routingRow1.RSSI = (byte) new Random().Next(1, (int) byte.MaxValue);
        RoutingRow routingRow2 = new RoutingRow();
        routingRow2.HopCount = (byte) new Random().Next(1, (int) byte.MaxValue);
        routingRow2.NodeId = (ushort) new Random().Next(1, 1000);
        routingRow2.ParentNodeId = (ushort) new Random().Next(1, 1000);
        routingRow2.RSSI = (byte) new Random().Next(1, (int) byte.MaxValue);
        RoutingRow routingRow3 = new RoutingRow();
        routingRow3.HopCount = (byte) new Random().Next(1, (int) byte.MaxValue);
        routingRow3.NodeId = (ushort) new Random().Next(1, 1000);
        routingRow3.ParentNodeId = (ushort) new Random().Next(1, 1000);
        routingRow3.RSSI = (byte) new Random().Next(1, (int) byte.MaxValue);
        routingTable.Add(routingRow1);
        routingTable.Add(routingRow2);
        routingTable.Add(routingRow3);
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = true
        };
      }
    }

    public GMMMinomatConfiguratorResult NetworkOptimization(MinomatSerializableDTO minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          GMMMinomatConfiguratorResult configuratorResult = GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId);
          if (!configuratorResult.IsSuccess)
            return configuratorResult;
          bool flag = minomatHandler.MyMinomatV4.StartNetworkOptimization();
          return new GMMMinomatConfiguratorResult()
          {
            IsSuccess = flag
          };
        }
        catch (Exception ex)
        {
          return this.HandleException(ex);
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = true
        };
      }
    }

    public bool StartNetworkTestReception(Minomat minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          return GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId).IsSuccess && minomatHandler.MyMinomatV4.StartTestReception(StartTestReceptionAction.Start, MinomatHandler.RadioProtocol.HCM_Radio3);
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return true;
      }
    }

    public TestReceptionResult ReadNetworkTestReception(Minomat minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          if (!GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId).IsSuccess)
            return (TestReceptionResult) null;
          TestReceptionResult testReceptionResult = minomatHandler.MyMinomatV4.GetTestReceptionResult();
          if (testReceptionResult != null)
            minomatHandler.MyMinomatV4.StartTestReception(StartTestReceptionAction.Commit, MinomatHandler.RadioProtocol.HCM_Radio3);
          return testReceptionResult;
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        List<RadioDevice> foundDevices = new List<RadioDevice>();
        RadioDevice radioDevice1 = new RadioDevice();
        radioDevice1.RSSI = (byte) new Random().Next(1, (int) byte.MaxValue);
        radioDevice1.LQI = (byte) new Random().Next(1, (int) byte.MaxValue);
        radioDevice1.MUID = (uint) new Random().Next(1, 1000);
        RadioDevice radioDevice2 = new RadioDevice();
        radioDevice1.RSSI = (byte) new Random().Next(1, (int) byte.MaxValue);
        radioDevice1.LQI = (byte) new Random().Next(1, (int) byte.MaxValue);
        radioDevice1.MUID = (uint) new Random().Next(1, 1000);
        foundDevices.Add(radioDevice1);
        foundDevices.Add(radioDevice2);
        return new TestReceptionResult(foundDevices);
      }
    }

    public Dictionary<string, GMMMinomatConfiguratorResult> RegisterSlavesOnMinomat(
      Dictionary<string, MinomatSerializableDTO> slaves,
      MinomatSerializableDTO master,
      out GMMMinomatConfiguratorResult canSlavesBeRegisteredOnMaster)
    {
      canSlavesBeRegisteredOnMaster = new GMMMinomatConfiguratorResult()
      {
        IsSuccess = false,
        Message = Resources.MSS_Client_SlavesCannotBeRegisteredOnMaster
      };
      Dictionary<string, GMMMinomatConfiguratorResult> dictionary = new Dictionary<string, GMMMinomatConfiguratorResult>();
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          GMMMinomatConfiguratorResult configuratorResult1 = GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, master.RadioId);
          if (configuratorResult1.IsSuccess)
          {
            foreach (KeyValuePair<string, MinomatSerializableDTO> slave in slaves)
            {
              GMMMinomatConfiguratorResult configuratorResult2 = this.RegisterOneSlaveOnMinomat(minomatHandler, slave.Key, slave.Value, master);
              dictionary.Add(slave.Key, configuratorResult2);
            }
          }
          canSlavesBeRegisteredOnMaster = configuratorResult1;
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        foreach (KeyValuePair<string, MinomatSerializableDTO> slave in slaves)
        {
          Thread.Sleep(5000);
          dictionary.Add(slave.Key, new GMMMinomatConfiguratorResult()
          {
            IsSuccess = true
          });
          canSlavesBeRegisteredOnMaster = new GMMMinomatConfiguratorResult()
          {
            IsSuccess = true
          };
        }
      }
      return dictionary;
    }

    public GMMMinomatConfiguratorResult RegisterOneSlaveOnMinomat(
      MinomatHandlerFunctions minomatHandler,
      string slaveNodeId,
      MinomatSerializableDTO slave,
      MinomatSerializableDTO master)
    {
      try
      {
        bool flag = minomatHandler.MyMinomatV4.RegisterSlave((object) slaveNodeId, (object) Convert.ToUInt32(slave.RadioId));
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = flag
        };
      }
      catch (Exception ex)
      {
        return this.HandleException(ex);
      }
    }

    public GMMMinomatConfiguratorResult RegisterDevicesOnMinomat(
      List<MeterDTO> foundDevices,
      Minomat minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          GMMMinomatConfiguratorResult configuratorResult = GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId);
          if (!configuratorResult.IsSuccess)
            return configuratorResult;
          foreach (MeterDTO foundDevice in foundDevices)
          {
            bool flag1 = Convert.ToInt32(foundDevice.SerialNumber) > 80000000 && Convert.ToInt32(foundDevice.SerialNumber) < 89999999;
            bool flag2 = Convert.ToInt32(foundDevice.SerialNumber) > 30000000 && Convert.ToInt32(foundDevice.SerialNumber) < 36999999;
            minomatHandler.MyMinomatV4.RegisterMessUnit(Convert.ToUInt32(foundDevice.SerialNumber), flag2 ? MeasurementCategory.B : MeasurementCategory.A, flag2 | flag1 ? MeasurementValueType.Slow_2Byte : MeasurementValueType.Fast_4Byte, MinomatHandler.RadioProtocol.HCM_Radio3);
          }
          return new GMMMinomatConfiguratorResult()
          {
            IsSuccess = true
          };
        }
        catch (Exception ex)
        {
          return this.HandleException(ex);
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = true
        };
      }
    }

    public bool DeregisterDevicesOnMinomat(List<RadioDevice> foundDevices, Minomat minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          if (!GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId).IsSuccess)
            return false;
          foreach (RadioDevice foundDevice in foundDevices)
            minomatHandler.MyMinomatV4.DeleteMessUnit(foundDevice.MUID);
          return true;
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return true;
      }
    }

    public GMMMinomatConfiguratorResult StartMinomatMaster(MinomatSerializableDTO minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          GMMMinomatConfiguratorResult configuratorResult = GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId);
          if (!configuratorResult.IsSuccess)
            return configuratorResult;
          minomatHandler.MyMinomatV4.StartNetworkSetup(NetworkSetupMode.StartNormal);
          return new GMMMinomatConfiguratorResult()
          {
            IsSuccess = true
          };
        }
        catch (Exception ex)
        {
          return this.HandleException(ex);
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = true
        };
      }
    }

    public GMMMinomatConfiguratorResult StartMinomatSlave(MinomatSerializableDTO minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          GMMMinomatConfiguratorResult configuratorResult = GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId);
          if (!configuratorResult.IsSuccess)
            return configuratorResult;
          minomatHandler.MyMinomatV4.StartNetworkSetup(NetworkSetupMode.StartNormal);
          return new GMMMinomatConfiguratorResult()
          {
            IsSuccess = true
          };
        }
        catch (Exception ex)
        {
          return this.HandleException(ex);
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = true
        };
      }
    }

    public GMMMinomatConfiguratorResult StartMasterGSMTestReception(MinomatSerializableDTO minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          GMMMinomatConfiguratorResult configuratorResult = GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId);
          if (!configuratorResult.IsSuccess)
            return configuratorResult;
          bool flag = minomatHandler.MyMinomatV4.StartGSMTestReception();
          return new GMMMinomatConfiguratorResult()
          {
            IsSuccess = flag
          };
        }
        catch (Exception ex)
        {
          return this.HandleException(ex);
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = true
        };
      }
    }

    public GSMTestReceptionState ReadMasterGSMTestReception(MinomatSerializableDTO minomat)
    {
      if (this._isConnectionMandatory)
      {
        MinomatHandlerFunctions minomatHandler = this.GetMinomatHandler();
        try
        {
          return GMMMinomatConfigurator.CheckConnectionAndRadioId(minomatHandler, minomat.RadioId).IsSuccess ? minomatHandler.MyMinomatV4.GetGSMTestReceptionState() : GSMTestReceptionState.Failed;
        }
        finally
        {
          minomatHandler.MyMinomatV4.Connection.Close();
        }
      }
      else
      {
        Thread.Sleep(5000);
        return GSMTestReceptionState.Successful;
      }
    }

    private GMMMinomatConfiguratorResult HandleException(Exception ex)
    {
      MessageHandler.LogException(ex);
      return new GMMMinomatConfiguratorResult()
      {
        IsSuccess = false,
        Message = ex.Message
      };
    }

    private static string GetPort()
    {
      try
      {
        if (MSS.Business.Utils.AppContext.Current.DefaultEquipment == null)
          return string.Empty;
        ChangeableParameter changeableParameter = MSS.Business.Utils.AppContext.Current.DefaultEquipment.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "Port"));
        return changeableParameter != null ? changeableParameter.Value : string.Empty;
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        return string.Empty;
      }
    }

    private static GMMMinomatConfiguratorResult CheckConnectionAndRadioId(
      MinomatHandlerFunctions minomatHandler,
      string radioId)
    {
      if (!minomatHandler.MyMinomatV4.Connection.Open())
      {
        string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = false,
          Message = errorDescription
        };
      }
      uint? minolId = minomatHandler.MyMinomatV4.GetMinolId();
      if (!minolId.HasValue)
      {
        string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = false,
          Message = errorDescription
        };
      }
      if (minolId.ToString() == radioId)
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = true
        };
      return new GMMMinomatConfiguratorResult()
      {
        IsSuccess = false,
        Message = "Different Radio Id!"
      };
    }
  }
}
