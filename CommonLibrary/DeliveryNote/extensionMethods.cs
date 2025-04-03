// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.DeliveryNote.extensionMethods
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;

#nullable disable
namespace ZENNER.CommonLibrary.DeliveryNote
{
  public static class extensionMethods
  {
    public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
    {
      return timeSpan == TimeSpan.Zero ? dateTime : dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
    }
  }
}
