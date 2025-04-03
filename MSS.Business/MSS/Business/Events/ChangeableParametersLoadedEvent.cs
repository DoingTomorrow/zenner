// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.ChangeableParametersLoadedEvent
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.Configuration;
using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Events
{
  public class ChangeableParametersLoadedEvent
  {
    public List<Config> ChangeableParameters { get; set; }

    public string StackPanelName { get; set; }

    public double GridFirstColumnPercentage { get; set; }

    public double GridWidth { get; set; }

    public ChangeableParameterUsings Type { get; set; }
  }
}
