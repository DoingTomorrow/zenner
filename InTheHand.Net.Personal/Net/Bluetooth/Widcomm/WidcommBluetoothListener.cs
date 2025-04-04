// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothListener
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommBluetoothListener : CommonBluetoothListener
  {
    private readonly WidcommBluetoothFactoryBase m_factory;
    private volatile WidcommRfcommInterface m_RfCommIf;
    private ISdpService m_sdpService;

    internal WidcommBluetoothListener(WidcommBluetoothFactoryBase factory)
      : base((BluetoothFactory) factory)
    {
      this.m_factory = factory;
      GC.SuppressFinalize((object) this);
    }

    private void SetupRfcommIf()
    {
      IRfCommIf widcommRfCommIf = this.m_factory.GetWidcommRfCommIf();
      this.m_RfCommIf = new WidcommRfcommInterface(widcommRfCommIf);
      widcommRfCommIf.Create();
      GC.ReRegisterForFinalize((object) this);
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
      byte channelNumber = checked ((byte) livePort);
      ServiceRecordHelper.SetRfcommChannelNumber(fullServiceRecord, channelNumber);
      this.m_sdpService = SdpService.CreateCustom(fullServiceRecord, this.m_factory);
    }

    protected override void AddSimpleServiceRecord(
      out ServiceRecord fullServiceRecord,
      int livePort,
      Guid serviceClass,
      string serviceName)
    {
      byte num = checked ((byte) livePort);
      this.m_sdpService = SdpService.CreateRfcomm(serviceClass, serviceName, num, this.m_factory);
      ServiceRecordBuilder serviceRecordBuilder = new ServiceRecordBuilder();
      serviceRecordBuilder.AddServiceClass(serviceClass);
      serviceRecordBuilder.ServiceName = serviceName;
      fullServiceRecord = serviceRecordBuilder.ServiceRecord;
      ServiceRecordHelper.SetRfcommChannelNumber(fullServiceRecord, num);
    }

    protected override bool IsDisposed => this.m_RfCommIf == null;

    protected override void OtherDispose(bool disposing)
    {
      WidcommRfcommInterface rfCommIf = this.m_RfCommIf;
      this.m_RfCommIf = (WidcommRfcommInterface) null;
      rfCommIf?.Dispose(disposing);
    }

    protected override void OtherDisposeMore()
    {
      if (this.m_sdpService == null)
        return;
      this.m_sdpService.Dispose();
    }

    protected override CommonRfcommStream GetNewPort()
    {
      return (CommonRfcommStream) this.m_factory.GetWidcommRfcommStreamWithoutRfcommIf();
    }
  }
}
