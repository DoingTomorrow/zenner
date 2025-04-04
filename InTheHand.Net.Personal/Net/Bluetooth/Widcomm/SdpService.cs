// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.SdpService
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class SdpService : ISdpService, IDisposable
  {
    private IntPtr m_pSdpService;

    internal SdpService()
    {
      try
      {
        SdpService.NativeMethods.SdpService_Create(out this.m_pSdpService);
        if (this.m_pSdpService == IntPtr.Zero)
          throw new InvalidOperationException("Native object creation failed.");
      }
      finally
      {
        if (this.m_pSdpService == IntPtr.Zero)
          GC.SuppressFinalize((object) this);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~SdpService() => this.Dispose(false);

    public void Dispose(bool disposing)
    {
      if (!(this.m_pSdpService != IntPtr.Zero))
        return;
      SdpService.NativeMethods.SdpService_Destroy(this.m_pSdpService);
      this.m_pSdpService = IntPtr.Zero;
    }

    void ISdpService.AddServiceClassIdList(IList<Guid> serviceClasses)
    {
      Guid[] array = new Guid[serviceClasses.Count];
      serviceClasses.CopyTo(array, 0);
      GCHandle gcHandle = GCHandle.Alloc((object) array, GCHandleType.Pinned);
      try
      {
        IntPtr guidArray = gcHandle.AddrOfPinnedObject();
        SdpService.SDP_RETURN_CODE ret = SdpService.NativeMethods.SdpService_AddServiceClassIdList(this.m_pSdpService, array.Length, guidArray);
        if (ret != SdpService.SDP_RETURN_CODE.OK)
          throw WidcommSocketExceptions.Create_SDP_RETURN_CODE(ret, "AddServiceClassIdList");
      }
      finally
      {
        gcHandle.Free();
      }
    }

    void ISdpService.AddServiceClassIdList(Guid serviceClass)
    {
      GCHandle gcHandle = GCHandle.Alloc((object) serviceClass, GCHandleType.Pinned);
      try
      {
        SdpService.SDP_RETURN_CODE ret = SdpService.NativeMethods.SdpService_AddServiceClassIdList(this.m_pSdpService, 1, gcHandle.AddrOfPinnedObject());
        if (ret != SdpService.SDP_RETURN_CODE.OK)
          throw WidcommSocketExceptions.Create_SDP_RETURN_CODE(ret, "AddServiceClassIdList");
      }
      finally
      {
        gcHandle.Free();
      }
    }

    void ISdpService.AddRFCommProtocolDescriptor(byte scn)
    {
      SdpService.SDP_RETURN_CODE ret = SdpService.NativeMethods.SdpService_AddRFCommProtocolDescriptor(this.m_pSdpService, scn);
      if (ret != SdpService.SDP_RETURN_CODE.OK)
        throw WidcommSocketExceptions.Create_SDP_RETURN_CODE(ret, "AddRFCommProtocolDescriptor");
    }

    void ISdpService.AddServiceName(string serviceName)
    {
      IntPtr zero = IntPtr.Zero;
      IntPtr hglobalAnsi = Marshal.StringToHGlobalAnsi(serviceName);
      try
      {
        SdpService.SDP_RETURN_CODE ret = SdpService.NativeMethods.SdpService_AddServiceName(this.m_pSdpService, serviceName, hglobalAnsi);
        if (ret != SdpService.SDP_RETURN_CODE.OK)
          throw WidcommSocketExceptions.Create_SDP_RETURN_CODE(ret, "AddServiceName");
      }
      finally
      {
        if (hglobalAnsi != IntPtr.Zero)
          Marshal.FreeHGlobal(hglobalAnsi);
      }
    }

    void ISdpService.AddAttribute(ushort id, SdpService.DESC_TYPE dt, int valLen, byte[] val)
    {
      SdpService.SDP_RETURN_CODE ret = SdpService.NativeMethods.SdpService_AddAttribute(this.m_pSdpService, id, dt, checked ((uint) valLen), val);
      if (ret != SdpService.SDP_RETURN_CODE.OK)
        throw WidcommSocketExceptions.Create_SDP_RETURN_CODE(ret, "AddAttribute");
    }

    void ISdpService.CommitRecord()
    {
    }

    public static ISdpService CreateRfcomm(
      Guid serviceClass,
      string serviceName,
      byte scn,
      WidcommBluetoothFactoryBase factory)
    {
      if (scn < (byte) 1 || scn > (byte) 30)
        throw new ArgumentOutOfRangeException(nameof (scn), (object) scn, (string) null);
      ISdpService widcommSdpService = factory.GetWidcommSdpService();
      widcommSdpService.AddServiceClassIdList(serviceClass);
      widcommSdpService.AddRFCommProtocolDescriptor(scn);
      if (serviceName != null)
        widcommSdpService.AddServiceName(serviceName);
      widcommSdpService.CommitRecord();
      return widcommSdpService;
    }

    public static ISdpService CreateCustom(
      ServiceRecord record,
      WidcommBluetoothFactoryBase factory)
    {
      ISdpService widcommSdpService = factory.GetWidcommSdpService();
      new WidcommSdpServiceCreator().CreateServiceRecord(record, widcommSdpService);
      return widcommSdpService;
    }

    private static class NativeMethods
    {
      [DllImport("32feetWidcomm")]
      internal static extern void SdpService_Create(out IntPtr ppObj);

      [DllImport("32feetWidcomm")]
      internal static extern void SdpService_Destroy(IntPtr pObj);

      [DllImport("32feetWidcomm")]
      internal static extern SdpService.SDP_RETURN_CODE SdpService_AddServiceClassIdList(
        IntPtr pObj,
        int recordCount,
        IntPtr guidArray);

      [DllImport("32feetWidcomm")]
      internal static extern SdpService.SDP_RETURN_CODE SdpService_AddRFCommProtocolDescriptor(
        IntPtr pObj,
        byte scn);

      [DllImport("32feetWidcomm", CharSet = CharSet.Unicode)]
      internal static extern SdpService.SDP_RETURN_CODE SdpService_AddServiceName(
        IntPtr pObj,
        string p_service_nameWchar,
        IntPtr p_service_nameChar);

      [DllImport("32feetWidcomm")]
      internal static extern SdpService.SDP_RETURN_CODE SdpService_AddAttribute(
        IntPtr pObj,
        ushort attrId,
        SdpService.DESC_TYPE attrType,
        uint attrLen,
        byte[] val);

      [DllImport("32feetWidcomm")]
      internal static extern SdpService.SDP_RETURN_CODE SdpService_CommitRecord(IntPtr pObj);
    }

    internal enum SDP_RETURN_CODE
    {
      OK,
      COULD_NOT_ADD_RECORD,
      INVALID_RECORD,
      INVALID_PARAMETERS,
    }

    internal enum DESC_TYPE : byte
    {
      NULL,
      UINT,
      TWO_COMP_INT,
      UUID,
      TEXT_STR,
      BOOLEAN,
      DATA_ELE_SEQ,
      DATA_ELE_ALT,
      URL,
    }
  }
}
