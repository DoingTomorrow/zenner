// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.PDCConfigurator
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Business.Modules.GMMWrapper;
using MSS.Localisation;
using PDC_Handler;
using System;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class PDCConfigurator : IFirmwareConfigurator, IDisposable
  {
    private PDC_HandlerFunctions handler;
    private byte[] buffer;
    private readonly EquipmentModel _equipmentModel;
    private readonly IHandlerManager _handlerManager;
    private readonly IDeviceManager _deviceManager;

    public PDCConfigurator(
      EquipmentModel equipmentModel,
      IHandlerManager handlerManager,
      IDeviceManager deviceManager)
    {
      this._equipmentModel = equipmentModel;
      this._handlerManager = handlerManager;
      this._deviceManager = deviceManager;
    }

    public void InitializeFirmwareConfigurator(EDC_Handler.Firmware edcFirmware, PDC_Handler.Firmware pdcFirmware)
    {
      this.buffer = pdcFirmware != null ? PDC_Handler.FirmwareManager.ReadFirmwareFromText(pdcFirmware.FirmwareText) : throw new BaseApplicationException(Resources.MSS_Client_Configuration_FirmwareUpgrade_DbHasNoFirmware);
      ConnectionProfile connectionProfile = this._deviceManager.GetConnectionProfile(77);
      connectionProfile.EquipmentModel = this._equipmentModel;
      this.handler = this._handlerManager.CreateInstance<PDC_HandlerFunctions>(connectionProfile);
    }

    public void UpgradeFirmWare()
    {
      try
      {
        PDC_Meter pdcMeter1 = this.handler.ReadDevice() ? this.handler.Meter.DeepCopy() : throw new BaseApplicationException(Resources.MSS_Client_Configuration_ReadDeviceError);
        if (!this.handler.RunRAMBackup())
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_UpgradeFirmware_CannotSaveConfiguration);
        if (!this.handler.UpgradeFirmware(this.buffer))
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_UpgradeFirmware_CannotUpdateFirmware);
        PDC_Meter pdcMeter2 = this.handler.ReadDevice() ? this.handler.Meter.DeepCopy() : throw new BaseApplicationException(Resources.MSS_Client_Configuration_ReadDeviceError);
        foreach (PDC_Handler.Parameter p in PDC_MemoryMap.GetParameter(pdcMeter2.Version))
        {
          if (p.Name.StartsWith("cfg_") || p.Name.StartsWith("Con_") || p.Name.StartsWith("Bak_"))
          {
            byte[] memoryBytes1 = pdcMeter1.Map.GetMemoryBytes(p);
            byte[] memoryBytes2 = pdcMeter2.Map.GetMemoryBytes(p);
            if (!Util.ArraysEqual(memoryBytes1, memoryBytes2))
              throw new BaseApplicationException(string.Format(Resources.MSS_Client_Configuration_UpgradeFirmware_Success_DifferentParameter, (object) p.Name, (object) Util.ByteArrayToString(memoryBytes1), (object) Util.ByteArrayToString(memoryBytes2)));
          }
        }
      }
      finally
      {
        this.handler?.GMM_Dispose();
      }
    }

    public event EventHandler<int> OnProgress
    {
      add => this.handler.OnProgress += (ValueEventHandler<int>) ((sender, e) => value(sender, e));
      remove
      {
        this.handler.OnProgress -= (ValueEventHandler<int>) ((sender, e) => value(sender, e));
      }
    }

    void IDisposable.Dispose() => this.handler?.Dispose();
  }
}
