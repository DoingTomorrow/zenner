// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothService
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Reflection;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public static class BluetoothService
  {
    public static readonly Guid Empty = Guid.Empty;
    public static readonly Guid BluetoothBase = new Guid(0, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid SdpProtocol = new Guid(1, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UdpProtocol = new Guid(2, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid RFCommProtocol = new Guid(3, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid TcpProtocol = new Guid(4, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid TcsBinProtocol = new Guid(5, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid TcsAtProtocol = new Guid(6, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AttProtocol = new Guid(7, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ObexProtocol = new Guid(8, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid IPProtocol = new Guid(9, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid FtpProtocol = new Guid(10, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HttpProtocol = new Guid(12, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid WspProtocol = new Guid(14, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid BnepProtocol = new Guid(15, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UpnpProtocol = new Guid(16, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HidpProtocol = new Guid(17, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HardcopyControlChannelProtocol = new Guid(18, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HardcopyDataChannelProtocol = new Guid(20, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HardcopyNotificationProtocol = new Guid(22, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AvctpProtocol = new Guid(23, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AvdtpProtocol = new Guid(25, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid CmtpProtocol = new Guid(27, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UdiCPlaneProtocol = new Guid(29, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid McapControlChannelProtocol = new Guid(30, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid McapDataChannelProtocol = new Guid(31, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid L2CapProtocol = new Guid(256, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ServiceDiscoveryServer = new Guid(4096, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid BrowseGroupDescriptor = new Guid(4097, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid PublicBrowseGroup = new Guid(4098, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid SerialPort = new Guid(4353, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid LanAccessUsingPpp = new Guid(4354, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid DialupNetworking = new Guid(4355, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid IrMCSync = new Guid(4356, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ObexObjectPush = new Guid(4357, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ObexFileTransfer = new Guid(4358, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid IrMCSyncCommand = new Guid(4359, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid Headset = new Guid(4360, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid CordlessTelephony = new Guid(4361, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AudioSource = new Guid(4362, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AudioSink = new Guid(4363, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AVRemoteControlTarget = new Guid(4364, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AdvancedAudioDistribution = new Guid(4365, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AVRemoteControl = new Guid(4366, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AVRemoteControlController = new Guid(4367, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid Intercom = new Guid(4368, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid Fax = new Guid(4369, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HeadsetAudioGateway = new Guid(4370, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid Wap = new Guid(4371, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid WapClient = new Guid(4372, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid Panu = new Guid(4373, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid Nap = new Guid(4374, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid GN = new Guid(4375, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid DirectPrinting = new Guid(4376, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ReferencePrinting = new Guid(4377, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid Imaging = new Guid(4378, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ImagingResponder = new Guid(4379, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ImagingAutomaticArchive = new Guid(4380, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ImagingReferenceObjects = new Guid(4381, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid Handsfree = new Guid(4382, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HandsfreeAudioGateway = new Guid(4383, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid DirectPrintingReferenceObjects = new Guid(4384, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid ReflectedUI = new Guid(4385, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid BasicPrinting = new Guid(4386, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid PrintingStatus = new Guid(4387, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HumanInterfaceDevice = new Guid(4388, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HardcopyCableReplacement = new Guid(4389, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HardcopyCableReplacementPrint = new Guid(4390, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HardcopyCableReplacementScan = new Guid(4391, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid CommonIsdnAccess = new Guid(4392, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid VideoConferencingGW = new Guid(4393, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UdiMT = new Guid(4394, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UdiTA = new Guid(4395, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid AudioVideo = new Guid(4396, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid SimAccess = new Guid(4397, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid PhonebookAccessPce = new Guid(4398, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid PhonebookAccessPse = new Guid(4399, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid PhonebookAccess = new Guid(4400, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HeadsetHeadset = new Guid(4401, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid MessageAccessServer = new Guid(4402, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid MessageNotificationServer = new Guid(4403, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid MessageAccessProfile = new Guid(4404, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid PnPInformation = new Guid(4608, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid GenericNetworking = new Guid(4609, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid GenericFileTransfer = new Guid(4610, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid GenericAudio = new Guid(4611, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid GenericTelephony = new Guid(4612, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UPnp = new Guid(4613, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UPnpIP = new Guid(4614, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UPnpIPPan = new Guid(4864, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UPnpIPLap = new Guid(4865, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid UPnpIPL2Cap = new Guid(4866, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid VideoSource = new Guid(4867, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid VideoSink = new Guid(4868, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid VideoDistribution = new Guid(4869, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HealthDevice = new Guid(5120, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HealthDeviceSource = new Guid(5121, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    public static readonly Guid HealthDeviceSink = new Guid(5122, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);

    public static string GetName(Guid uuid)
    {
      foreach (FieldInfo field in typeof (BluetoothService).GetFields(BindingFlags.Static | BindingFlags.Public))
      {
        if (((Guid) field.GetValue((object) null)).Equals(uuid))
          return field.Name;
      }
      return (string) null;
    }

    public static string GetName(short uuid16)
    {
      return BluetoothService.GetName(BluetoothService.CreateBluetoothUuid(uuid16));
    }

    [CLSCompliant(false)]
    public static string GetName(ushort uuid16)
    {
      return BluetoothService.GetName(BluetoothService.CreateBluetoothUuid(uuid16));
    }

    public static string GetName(int uuid32)
    {
      return BluetoothService.GetName(BluetoothService.CreateBluetoothUuid(uuid32));
    }

    [CLSCompliant(false)]
    public static string GetName(uint uuid32)
    {
      return BluetoothService.GetName(BluetoothService.CreateBluetoothUuid(uuid32));
    }

    public static Guid CreateBluetoothUuid(short uuid16)
    {
      return BluetoothService.CreateBluetoothUuid((int) uuid16);
    }

    [CLSCompliant(false)]
    public static Guid CreateBluetoothUuid(ushort uuid16)
    {
      return BluetoothService.CreateBluetoothUuid((uint) uuid16);
    }

    public static Guid CreateBluetoothUuid(int uuid32)
    {
      return new Guid(uuid32, (short) 0, (short) 4096, (byte) 128, (byte) 0, (byte) 0, (byte) 128, (byte) 95, (byte) 155, (byte) 52, (byte) 251);
    }

    [CLSCompliant(false)]
    public static Guid CreateBluetoothUuid(uint uuid32)
    {
      return BluetoothService.CreateBluetoothUuid((int) uuid32);
    }

    internal static ushort? GetAsClassId16(Guid service)
    {
      ushort uint16 = BitConverter.ToUInt16(service.ToByteArray(), 0);
      Guid bluetoothUuid = BluetoothService.CreateBluetoothUuid(uint16);
      return service == bluetoothUuid ? new ushort?(uint16) : new ushort?();
    }
  }
}
