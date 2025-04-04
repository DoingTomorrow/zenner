// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MinomatHandlerFunctions
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using AsyncCom;
using DeviceCollector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public class MinomatHandlerFunctions : IMinomatHandler
  {
    private IAsyncFunctions MyCom;
    public MinomatV4 MyMinomatV4;
    private MinomatHandlerWindow MyWindow;

    public MinomatHandlerFunctions()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.AsyncCom);
      this.MyCom = (IAsyncFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.AsyncCom];
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      IDeviceCollector loadedComponents = (IDeviceCollector) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector];
      this.MyMinomatV4 = new MinomatV4(new SCGiConnection(this.MyCom));
      SortedList<DeviceCollectorSettings, object> collectorSettings = loadedComponents.GetDeviceCollectorSettings();
      if (collectorSettings == null || !collectorSettings.ContainsKey(DeviceCollectorSettings.MaxRequestRepeat))
        return;
      this.MyMinomatV4.MaxAttempt = Convert.ToInt32(collectorSettings[DeviceCollectorSettings.MaxRequestRepeat]);
    }

    public MinomatHandlerFunctions(IDeviceCollector deviceCollector)
    {
      this.MyMinomatV4 = new MinomatV4(new SCGiConnection(deviceCollector.AsyncCom));
      SortedList<DeviceCollectorSettings, object> collectorSettings = deviceCollector.GetDeviceCollectorSettings();
      if (collectorSettings == null || !collectorSettings.ContainsKey(DeviceCollectorSettings.MaxRequestRepeat))
        return;
      this.MyMinomatV4.MaxAttempt = Convert.ToInt32(collectorSettings[DeviceCollectorSettings.MaxRequestRepeat]);
    }

    public void GMM_Dispose()
    {
    }

    public string ShowMinomatHandlerWindow()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      this.MyWindow = new MinomatHandlerWindow(this);
      int num = (int) this.MyWindow.ShowDialog();
      string nextComponentName = this.MyWindow.NextComponentName;
      this.MyWindow.Dispose();
      return nextComponentName;
    }

    internal ToolStripItem[] GetComponentMenuItems()
    {
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem();
      toolStripMenuItem1.Name = "ComponentMenuItemGMM";
      toolStripMenuItem1.Size = new Size(173, 22);
      toolStripMenuItem1.Text = "GlobalMeterManager";
      toolStripMenuItem1.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem1.Tag = (object) GMM_Components.GMM.ToString();
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem();
      toolStripMenuItem2.Name = "ComponentMenuItemBack";
      toolStripMenuItem2.Size = new Size(173, 22);
      toolStripMenuItem2.Text = "Back";
      toolStripMenuItem2.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem2.Tag = (object) "";
      ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem();
      toolStripMenuItem3.Name = "ComponentMenuItemQuit";
      toolStripMenuItem3.Size = new Size(173, 22);
      toolStripMenuItem3.Text = "Quit";
      toolStripMenuItem3.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem3.Tag = (object) "Exit";
      ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
      ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem();
      toolStripMenuItem4.Name = "ComponentMenuItemConfigurator";
      toolStripMenuItem4.Size = new Size(173, 22);
      toolStripMenuItem4.Text = GMM_Components.Configurator.ToString();
      toolStripMenuItem4.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem4.Tag = (object) GMM_Components.Configurator.ToString();
      ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem();
      toolStripMenuItem5.Name = "ComponentMenuMinolHandler";
      toolStripMenuItem5.Size = new Size(173, 22);
      toolStripMenuItem5.Text = GMM_Components.MinolHandler.ToString();
      toolStripMenuItem5.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem5.Tag = (object) GMM_Components.MinolHandler.ToString();
      ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
      ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem();
      toolStripMenuItem6.Name = "ComponentMenuItemDeviceCollector";
      toolStripMenuItem6.Size = new Size(173, 22);
      toolStripMenuItem6.Text = GMM_Components.DeviceCollector.ToString();
      toolStripMenuItem6.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem6.Tag = (object) GMM_Components.DeviceCollector.ToString();
      ToolStripMenuItem toolStripMenuItem7 = new ToolStripMenuItem();
      toolStripMenuItem7.Name = "ComponentMenuItemAsyncCom";
      toolStripMenuItem7.Size = new Size(173, 22);
      toolStripMenuItem7.Text = GMM_Components.AsyncCom.ToString();
      toolStripMenuItem7.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem7.Tag = (object) GMM_Components.AsyncCom.ToString();
      return new ToolStripItem[9]
      {
        (ToolStripItem) toolStripMenuItem1,
        (ToolStripItem) toolStripMenuItem2,
        (ToolStripItem) toolStripMenuItem3,
        (ToolStripItem) toolStripSeparator1,
        (ToolStripItem) toolStripMenuItem4,
        (ToolStripItem) toolStripMenuItem5,
        (ToolStripItem) toolStripSeparator2,
        (ToolStripItem) toolStripMenuItem6,
        (ToolStripItem) toolStripMenuItem7
      };
    }

    private void MenuItemSelectComponent_Click(object sender, EventArgs e)
    {
      this.MyWindow.NextComponentName = (sender as ToolStripMenuItem).Tag.ToString();
      this.MyWindow.Close();
    }
  }
}
