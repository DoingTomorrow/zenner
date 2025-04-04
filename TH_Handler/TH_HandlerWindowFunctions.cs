// Decompiled with JetBrains decompiler
// Type: TH_Handler.TH_HandlerWindowFunctions
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using TH_Handler.Properties;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public class TH_HandlerWindowFunctions
  {
    public TH_HandlerFunctions Handler { get; private set; }

    public TH_HandlerWindowFunctions(TH_HandlerFunctions handler) => this.Handler = handler;

    public string ShowMainWindow()
    {
      TH_HandlerWindow thHandlerWindow = new TH_HandlerWindow(this.Handler);
      thHandlerWindow.ShowDialog();
      Settings.Default.Save();
      ZR_ClassLibMessages.ClearErrors();
      return thHandlerWindow.NextPlugin;
    }
  }
}
