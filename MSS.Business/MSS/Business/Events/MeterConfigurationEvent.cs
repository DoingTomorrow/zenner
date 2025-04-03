// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.MeterConfigurationEvent
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.Configuration;
using System.Collections.Generic;
using System.Windows.Input;

#nullable disable
namespace MSS.Business.Events
{
  public class MeterConfigurationEvent
  {
    public List<ConfigurationPerChannel> ConfigValuesPerChannelList { get; set; }

    public ICommand ComboboxCommand { get; set; }

    public int SelectedTab { get; set; }
  }
}
