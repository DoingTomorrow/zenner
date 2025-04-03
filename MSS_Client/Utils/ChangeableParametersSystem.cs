// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.ChangeableParametersSystem
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using System;

#nullable disable
namespace MSS_Client.Utils
{
  public class ChangeableParametersSystem
  {
    public TimeSpan? DueDate { get; set; }

    public TimeSpan? Month { get; set; }

    public TimeSpan? Day { get; set; }

    public TimeSpan? QuarterHour { get; set; }
  }
}
