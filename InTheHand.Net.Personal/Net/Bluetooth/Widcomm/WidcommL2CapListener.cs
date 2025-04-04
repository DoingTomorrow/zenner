// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommL2CapListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommL2CapListener : CommonBluetoothListener
  {
    private readonly WidcommBluetoothFactoryBase m_factory;
    private volatile WidcommRfcommInterface m_RfCommIf;
    private IRfCommIf _rfCommIf__tmp;
    private ISdpService m_sdpService;

    public static CommonBluetoothListener Create()
    {
      return (CommonBluetoothListener) new WidcommL2CapListener(WidcommBluetoothFactory.GetWidcommIfExists());
    }

    internal WidcommL2CapListener(WidcommBluetoothFactoryBase factory)
      : base((BluetoothFactory) factory)
    {
      this.m_factory = factory;
      GC.SuppressFinalize((object) this);
    }

    private void SetupRfcommIf()
    {
      IRfCommIf widcommL2CapIf = WidcommL2CapClient.GetWidcommL2CapIf(this.m_factory);
      this._rfCommIf__tmp = widcommL2CapIf;
      this.m_RfCommIf = new WidcommRfcommInterface(widcommL2CapIf);
      widcommL2CapIf.Create();
      GC.ReRegisterForFinalize((object) this);
    }

    protected override void VerifyPortIsInRange(BluetoothEndPoint bep)
    {
    }

    protected override void SetupListener(
      BluetoothEndPoint bep,
      int requestedScn,
      out BluetoothEndPoint liveLocalEP)
    {
      this.SetupRfcommIf();
      int port = this.m_RfCommIf.SetScnForLocalServer(bep.Service, requestedScn);
      this.m_RfCommIf.SetSecurityLevelServer(WidcommUtils.ToBTM_SEC(true, this.m_authenticate, this.m_encrypt), new byte[7]
      {
        (byte) 104,
        (byte) 97,
        (byte) 99,
        (byte) 107,
        (byte) 83,
        (byte) 118,
        (byte) 114
      });
      liveLocalEP = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.Empty, port);
    }

    protected override void AddCustomServiceRecord(
      ref ServiceRecord fullServiceRecord,
      int livePort)
    {
      ushort psm = checked ((ushort) livePort);
      ServiceRecordHelper.SetL2CapPsmNumber(fullServiceRecord, (int) psm);
      this.m_sdpService = SdpService.CreateCustom(fullServiceRecord, this.m_factory);
    }

    protected override void AddSimpleServiceRecord(
      out ServiceRecord fullServiceRecord,
      int livePort,
      Guid serviceClass,
      string serviceName)
    {
      ServiceRecordBuilder serviceRecordBuilder = new ServiceRecordBuilder();
      serviceRecordBuilder.ProtocolType = BluetoothProtocolDescriptorType.L2Cap;
      serviceRecordBuilder.AddServiceClass(serviceClass);
      serviceRecordBuilder.ServiceName = serviceName;
      fullServiceRecord = serviceRecordBuilder.ServiceRecord;
      this.AddCustomServiceRecord(ref fullServiceRecord, livePort);
    }

    protected override bool IsDisposed => this.m_RfCommIf == null;

    protected override void OtherDispose(bool disposing)
    {
      WidcommRfcommInterface rfCommIf = this.m_RfCommIf;
      this.m_RfCommIf = (WidcommRfcommInterface) null;
      if (rfCommIf == null)
        return;
      MiscUtils.Trace_WriteLine("!! skipping l2capif.Dispose/Destroy/CL2CapIf.Destroy !!");
    }

    protected override void OtherDisposeMore()
    {
      if (this.m_sdpService == null)
        return;
      this.m_sdpService.Dispose();
    }

    protected override CommonRfcommStream GetNewPort()
    {
      return (CommonRfcommStream) WidcommL2CapClient.GetWidcommL2CapStreamWithThisIf(this.m_factory, this._rfCommIf__tmp);
    }

    protected override IBluetoothClient GetBluetoothClientForListener(CommonRfcommStream strm)
    {
      return WidcommL2CapClient.factory_DoGetBluetoothClientForListener(this.m_factory, strm);
    }
  }
}
