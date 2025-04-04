// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.MapServiceClassToAttributeIdList
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.AttributeIds;
using System;
using System.Collections.Generic;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public class MapServiceClassToAttributeIdList
  {
    private MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow[] m_serviceClassToIdsMapTable = new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow[20]
    {
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.ServiceDiscoveryServer, typeof (ServiceDiscoveryServerAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.IrMCSync, typeof (ObexAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.ObexObjectPush, typeof (ObexAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.ObexFileTransfer, typeof (ObexAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.IrMCSyncCommand, typeof (ObexAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.Headset, typeof (HeadsetProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.HeadsetHeadset, typeof (HeadsetProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.Panu, typeof (PersonalAreaNetworkingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.Nap, typeof (PersonalAreaNetworkingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.GN, typeof (PersonalAreaNetworkingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.DirectPrinting, typeof (BasicPrintingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.ReferencePrinting, typeof (BasicPrintingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.DirectPrintingReferenceObjects, typeof (BasicPrintingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.ReflectedUI, typeof (BasicPrintingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.BasicPrinting, typeof (BasicPrintingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.PrintingStatus, typeof (BasicPrintingProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.Handsfree, typeof (HandsFreeProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.HandsfreeAudioGateway, typeof (HandsFreeProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.HumanInterfaceDevice, typeof (HidProfileAttributeId)),
      new MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow(BluetoothService.PnPInformation, typeof (DeviceIdProfileAttributeId))
    };

    public Type[] GetAttributeIdEnumTypes(ServiceRecord record)
    {
      if (record == null)
        throw new ArgumentNullException(nameof (record));
      ServiceAttribute attributeById;
      try
      {
        attributeById = record.GetAttributeById((ServiceAttributeId) 1);
      }
      catch (KeyNotFoundException ex)
      {
        goto label_8;
      }
      ServiceElement serviceElement = attributeById.Value;
      if (serviceElement.ElementType == ElementType.ElementSequence)
      {
        ServiceElement[] valueAsElementArray = serviceElement.GetValueAsElementArray();
        if (valueAsElementArray.Length != 0)
        {
          Type attributeIdEnumType = this.GetAttributeIdEnumType(valueAsElementArray[0]);
          if (attributeIdEnumType != null)
            return new Type[1]{ attributeIdEnumType };
        }
      }
label_8:
      return new Type[0];
    }

    protected virtual Type GetAttributeIdEnumType(ServiceElement idElement)
    {
      if (idElement == null)
        throw new ArgumentNullException(nameof (idElement));
      return idElement.ElementTypeDescriptor != ElementTypeDescriptor.Uuid ? (Type) null : this.GetAttributeIdEnumType(idElement.GetValueAsUuid());
    }

    protected virtual Type GetAttributeIdEnumType(Guid uuid)
    {
      foreach (MapServiceClassToAttributeIdList.ServiceClassToIdsMapRow classToIdsMapRow in this.m_serviceClassToIdsMapTable)
      {
        if (uuid == classToIdsMapRow.ServiceClassId)
          return classToIdsMapRow.AttributeIdEnumType;
      }
      return (Type) null;
    }

    private struct ServiceClassToIdsMapRow(Guid ServiceClassId, Type AttributeIdEnumType)
    {
      public readonly Guid ServiceClassId = ServiceClassId;
      public readonly Type AttributeIdEnumType = AttributeIdEnumType;
    }
  }
}
