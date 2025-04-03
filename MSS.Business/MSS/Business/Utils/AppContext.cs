// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.AppContext
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using InTheHand.Net.Sockets;
using MSS.Business.Errors;
using MSS.Business.Modules.Synchronization.HandleConflicts;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.TechnicalParameters;
using MSS.Core.Model.UsersManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Utils
{
  public class AppContext
  {
    private static AppContext _instance;
    private IEnumerable<ApplicationParameter> _parameters;
    private readonly Dictionary<Guid, ConflictDetails> _syncConflicts = new Dictionary<Guid, ConflictDetails>();
    private Dictionary<Guid, string> _syncExtraData = new Dictionary<Guid, string>();
    private User _loggedUser;

    public static AppContext Current
    {
      get
      {
        lock (typeof (AppContext))
          return AppContext._instance ?? (AppContext._instance = new AppContext());
      }
    }

    public User LoggedUser
    {
      get => this._loggedUser;
      set => this._loggedUser = value;
    }

    public string MSSClientId { get; set; }

    public IEnumerable<ApplicationParameter> Parameters => this._parameters;

    public IEnumerable<string> Operations { get; set; }

    public bool HasServer { get; set; }

    public bool HasConflicts => this.SyncConflicts.Count > 0;

    public bool HandleConflicts { get; set; }

    public bool HasLocks { get; set; }

    public Dictionary<Guid, ConflictDetails> SyncConflicts => this._syncConflicts;

    public Dictionary<Guid, string> SyncExtraData
    {
      get => this._syncExtraData;
      set => this._syncExtraData = value;
    }

    public bool IsServerAvailableAndStatusAccepted { get; set; }

    public bool IsClientUpToDateSend { get; set; }

    public EquipmentModel DefaultEquipment { get; set; }

    public List<BluetoothDeviceInfo> MinoConnectDeviceNames { get; set; }

    public void LoadApplicationParameters(IList<ApplicationParameter> applicationParameters)
    {
      this._parameters = (IEnumerable<ApplicationParameter>) applicationParameters;
      this.HasServer = !string.IsNullOrEmpty(this.GetParameterValue<string>("ServerIp"));
    }

    public void Initialize(IList<ApplicationParameter> applicationParameters)
    {
      this.LoadApplicationParameters(applicationParameters);
    }

    public void LoadDefaultEquipment()
    {
      string equipmentName = AppContext.Current.GetParameterValue<string>("DefaultEquipment");
      Application.Current.Dispatcher.Invoke((Action) (() => AppContext.Current.DefaultEquipment = GmmInterface.DeviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (d => d.Name == equipmentName))));
      if (AppContext.Current.DefaultEquipment == null)
        return;
      AppContext.Current.DefaultEquipment = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateEquipmentWithSavedParams(AppContext.Current.DefaultEquipment, AppContext.Current.GetParameterValue<string>("DefaultEquipmentParams"));
    }

    public T GetParameterValue<T>(string name)
    {
      return (T) Convert.ChangeType((object) (this.Parameters.FirstOrDefault<ApplicationParameter>((Func<ApplicationParameter, bool>) (p => p.Parameter.Equals(name))) ?? throw new BaseApplicationException(string.Format(ErrorCodes.GetErrorMessage("MSSError_5"), (object) name))).Value, typeof (T));
    }

    public bool IsDeviceConnected { get; set; }

    public bool IsMinoConnectConnected { get; set; }

    public TechnicalParameter TechnicalParameters { get; set; }
  }
}
