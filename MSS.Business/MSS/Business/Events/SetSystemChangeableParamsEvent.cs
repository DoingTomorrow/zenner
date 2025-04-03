// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.SetSystemChangeableParamsEvent
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;

#nullable disable
namespace MSS.Business.Events
{
  public class SetSystemChangeableParamsEvent
  {
    public TimeSpan? DueDate { get; set; }

    public TimeSpan? Month { get; set; }

    public TimeSpan? Day { get; set; }

    public TimeSpan? QuarterHour { get; set; }
  }
}
