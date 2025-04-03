// Decompiled with JetBrains decompiler
// Type: Devices.ThHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using TH_Handler;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class ThHandler : BaseDevice
  {
    private TH_HandlerWindowFunctions handler;

    public ThHandler(DeviceManager MyDeviceManager)
      : base(MyDeviceManager)
    {
      if (ZR_Component.CommonGmmInterface != null)
      {
        ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.TH_Handler);
        this.handler = (TH_HandlerWindowFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.TH_Handler];
      }
      else
        this.handler = new TH_HandlerWindowFunctions(new TH_HandlerFunctions((IDeviceCollector) MyDeviceManager.MyBus));
    }

    public override object GetHandler() => (object) this.handler.Handler;

    internal override void ShowHandlerWindow()
    {
      if (this.handler == null)
        return;
      this.handler.ShowMainWindow();
    }

    public override void Dispose()
    {
      if (this.handler == null)
        return;
      this.handler.Handler.Dispose();
      this.handler = (TH_HandlerWindowFunctions) null;
    }
  }
}
