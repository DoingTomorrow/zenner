// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Jobs.IntervalDto
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Jobs;
using MSS.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace MSS.DTO.Jobs
{
  public class IntervalDto
  {
    [Required]
    public DateTime? StartDate { get; set; }

    [Required]
    public DateTime? EndDate { get; set; }

    [Required]
    public DateTime? Time { get; set; }

    public DateTime? FixedInterval { get; set; }

    public DateTime? OneTimeDate { get; set; }

    public int RepeatIn { get; set; }

    public List<int> WeeklyDays { get; set; }

    public List<int> MonthlyMonth { get; set; }

    public List<int> MonthlyCardinalDay { get; set; }

    public List<int> MonthlyOrdinalDay { get; set; }

    public List<int> MonthlyWeekDay { get; set; }

    public SerializableDictionary<IntervalTypeEnum, bool> IntervalType { get; set; }

    public SerializableDictionary<MonthlyTypeEnum, bool> MonthlyType { get; set; }
  }
}
