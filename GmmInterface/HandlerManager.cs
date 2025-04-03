// Decompiled with JetBrains decompiler
// Type: ZENNER.HandlerManager
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using AsyncCom;
using DeviceCollector;
using Devices;
using GmmDbLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;

#nullable disable
namespace ZENNER
{
  public sealed class HandlerManager : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (HandlerManager));

    public T CreateInstance<T>(ConnectionProfile profile)
    {
      if (profile == null)
        throw new ArgumentNullException(nameof (profile));
      Type type = typeof (T);
      try
      {
        DeviceCollectorFunctions collectorFunctions = new DeviceCollectorFunctions((IAsyncFunctions) new AsyncFunctions(true));
        SortedList<string, string> settingsList = profile.GetSettingsList();
        collectorFunctions.DisableBusWriteOnDispose = true;
        collectorFunctions.SetAsyncComSettings(settingsList);
        if (!collectorFunctions.SetDeviceCollectorSettings(settingsList))
        {
          string message = Ot.Gtm(Tg.CommunicationLogic, "FailedSetDeviceCollectorSettings", "Can not set the DeviceCollector settings!");
          throw new InvalidSettingsException(settingsList, message);
        }
        return (T) Activator.CreateInstance(type, (object) collectorFunctions);
      }
      catch (Exception ex)
      {
        HandlerManager.logger.Error<Type, string>("Can not create the handler from type: {0}. Error: {1}", type, ex.Message);
        throw ex;
      }
    }

    public void Dispose()
    {
    }

    public void ShowHandlerWindow(ConnectionProfile profile)
    {
      if (!UserManager.CheckPermission("Developer"))
        throw new PermissionException("Developer");
      DeviceManager instance = this.CreateInstance<DeviceManager>(profile);
      instance.ShowHandlerWindow();
      instance.Dispose();
    }
  }
}
