// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.IReadingValuesRepository
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Interfaces
{
  public interface IReadingValuesRepository
  {
    List<OrderReadingValues> LoadOrderReadingValues(List<Guid> readingValuesIds);

    List<JobReadingValues> LoadJobReadingValues(List<Guid> readingValuesIds);
  }
}
