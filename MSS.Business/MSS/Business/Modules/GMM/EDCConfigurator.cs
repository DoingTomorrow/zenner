// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.EDCConfigurator
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using EDC_Handler;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.DTO.MessageHandler;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class EDCConfigurator : IFirmwareConfigurator, IDisposable
  {
    private EDC_HandlerFunctions handler;
    private byte[] buffer;
    private readonly EquipmentModel _equipmentModel;

    public EDCConfigurator(EquipmentModel equipmentModel) => this._equipmentModel = equipmentModel;

    public void InitializeFirmwareConfigurator(EDC_Handler.Firmware edcFirmware, PDC_Handler.Firmware pdcFirmware)
    {
      if (edcFirmware == null)
        return;
      this.buffer = EDC_Handler.FirmwareManager.ReadFirmwareFromText(edcFirmware.FirmwareText);
      ConnectionProfile connectionProfile = GmmInterface.DeviceManager.GetConnectionProfile(1);
      connectionProfile.EquipmentModel = this._equipmentModel;
      this.handler = GmmInterface.HandlerManager.CreateInstance<EDC_HandlerFunctions>(connectionProfile);
    }

    public void UpgradeFirmWare()
    {
      try
      {
        if (!this.handler.ReadDevice())
        {
          MSS.Business.Errors.MessageHandler.LogGMMExceptionMessage(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_ReadDeviceError);
        }
        if (!this.handler.SaveDevice())
        {
          MSS.Business.Errors.MessageHandler.LogGMMExceptionMessage(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_UpgradeFirmware_CannotSaveDevice);
        }
        EDC_Meter edcMeter = this.handler.Meter.DeepCopy();
        if (!this.handler.UpgradeFirmware(this.buffer))
        {
          MSS.Business.Errors.MessageHandler.LogGMMExceptionMessage(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_UpgradeFirmware_CannotUpdateFirmware);
        }
        if (!this.handler.ReadDevice())
        {
          MSS.Business.Errors.MessageHandler.LogGMMExceptionMessage(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_ReadDeviceError);
        }
        EDC_Meter meter1 = this.handler.Meter;
        List<EDC_Handler.Parameter> parameter1 = EDC_MemoryMap.GetParameter(edcMeter.Version);
        foreach (EDC_Handler.Parameter parameter2 in EDC_MemoryMap.GetParameter(meter1.Version))
        {
          EDC_Handler.Parameter p = parameter2;
          if (!(p.Name == "cfg_crc") && !(p.Name == "cfg_version_major") && !(p.Name == "cfg_version_minor") && (p.Name.StartsWith("cfg_") || p.Name.StartsWith("Con_") || p.Name.StartsWith("Bak_")))
          {
            EDC_Handler.Parameter p1 = parameter1.Find((Predicate<EDC_Handler.Parameter>) (x => x.Name == p.Name));
            if (p1 != null)
            {
              byte[] memoryBytes1 = edcMeter.Map.GetMemoryBytes(p1);
              byte[] memoryBytes2 = meter1.Map.GetMemoryBytes(p);
              if (!Util.ArraysEqual(memoryBytes1, memoryBytes2))
              {
                if (!meter1.Map.SetMemoryBytes(p.Address, memoryBytes1))
                  throw new BaseApplicationException(string.Format(Resources.MSS_Client_Configuration_UpgradeFirmware_Success_DifferentParameter, (object) p.Name));
                EventPublisher.Publish<ShowMessage>(new ShowMessage()
                {
                  Message = new MSS.DTO.Message.Message()
                  {
                    MessageType = MessageTypeEnum.Warning,
                    MessageText = string.Format(Resources.MSS_Client_Configuration_UpgradeFirmware_Success_DifferentParameter, (object) p.Name, (object) Util.ByteArrayToString(memoryBytes1), (object) Util.ByteArrayToString(memoryBytes2))
                  }
                }, (object) this);
              }
            }
          }
        }
        byte major = meter1.Version.Major;
        meter1.SetParameterValue<byte>("cfg_version_major", major);
        byte minor = meter1.Version.Minor;
        meter1.SetParameterValue<byte>("cfg_version_minor", minor);
        meter1.SetHardwareErrors((EDC_Handler.HardwareError) 0);
        meter1.SetWarnings(~(EDC_Handler.Warning.APP_BUSY | EDC_Handler.Warning.ABNORMAL | EDC_Handler.Warning.BATT_LOW | EDC_Handler.Warning.PERMANENT_ERROR | EDC_Handler.Warning.TEMPORARY_ERROR | EDC_Handler.Warning.TAMPER_A | EDC_Handler.Warning.REMOVAL_A | EDC_Handler.Warning.LEAK | EDC_Handler.Warning.LEAK_A | EDC_Handler.Warning.UNDERSIZE | EDC_Handler.Warning.BLOCK_A | EDC_Handler.Warning.BACKFLOW | EDC_Handler.Warning.BACKFLOW_A | EDC_Handler.Warning.INTERFERE | EDC_Handler.Warning.OVERSIZE | EDC_Handler.Warning.BURST));
        if (!this.handler.WriteDevice())
        {
          MSS.Business.Errors.MessageHandler.LogGMMExceptionMessage(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_Write_Error);
        }
        if (!this.handler.SaveDevice())
        {
          MSS.Business.Errors.MessageHandler.LogGMMExceptionMessage(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_UpgradeFirmware_CannotSaveDevice);
        }
        if (!this.handler.ReadDevice())
        {
          MSS.Business.Errors.MessageHandler.LogGMMExceptionMessage(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
          throw new BaseApplicationException(Resources.MSS_Client_Configuration_ReadDeviceError);
        }
        EDC_Meter meter2 = this.handler.Meter;
        foreach (EDC_Handler.Parameter parameter3 in EDC_MemoryMap.GetParameter(meter2.Version))
        {
          EDC_Handler.Parameter p = parameter3;
          if (!(p.Name == "cfg_crc") && !(p.Name == "cfg_version_major") && !(p.Name == "cfg_version_minor") && (p.Name.StartsWith("cfg_") || p.Name.StartsWith("Con_") || p.Name.StartsWith("Bak_")))
          {
            EDC_Handler.Parameter p2 = parameter1.Find((Predicate<EDC_Handler.Parameter>) (x => x.Name == p.Name));
            if (p2 != null)
            {
              byte[] memoryBytes3 = edcMeter.Map.GetMemoryBytes(p2);
              byte[] memoryBytes4 = meter2.Map.GetMemoryBytes(p);
              if (!Util.ArraysEqual(memoryBytes3, memoryBytes4))
                EventPublisher.Publish<ShowMessage>(new ShowMessage()
                {
                  Message = new MSS.DTO.Message.Message()
                  {
                    MessageType = MessageTypeEnum.Warning,
                    MessageText = string.Format(Resources.MSS_Client_Configuration_UpgradeFirmware_Success_DifferentParameter, (object) p.Name, (object) Util.ByteArrayToString(memoryBytes3), (object) Util.ByteArrayToString(memoryBytes4))
                  }
                }, (object) this);
            }
          }
        }
      }
      finally
      {
        this.handler?.GMM_Dispose();
      }
    }

    void IDisposable.Dispose() => this.handler?.Dispose();

    public event EventHandler<int> OnProgress
    {
      add => this.handler.OnProgress += (ValueEventHandler<int>) ((sender, e) => value(sender, e));
      remove
      {
        this.handler.OnProgress -= (ValueEventHandler<int>) ((sender, e) => value(sender, e));
      }
    }
  }
}
