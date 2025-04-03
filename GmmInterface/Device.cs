// Decompiled with JetBrains decompiler
// Type: ZENNER.Device
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZENNER
{
  [Serializable]
  public sealed class Device
  {
    public string ID { get; set; }

    public IdType IdType { get; set; }

    public int Channel { get; set; }

    public SortedList<long, SortedList<DateTime, double>> ValueSets { get; set; }
  }
}
