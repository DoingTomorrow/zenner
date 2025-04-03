// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.DeliveryNote.DeliveryNote
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary.DeliveryNote
{
  public class DeliveryNote
  {
    private List<Device_DeliveryNote> devices = new List<Device_DeliveryNote>();

    public DeliveryNote()
    {
    }

    public DeliveryNote(int nr, DateTime date)
    {
      this.DeliveryNoteNr = nr;
      this.DeliveryNotDate = date;
    }

    public DeliveryNote(int nr)
    {
      this.DeliveryNoteNr = nr;
      this.DeliveryNotDate = DateTime.UtcNow.Truncate(TimeSpan.FromSeconds(1.0));
    }

    public DateTime DeliveryNotDate { get; set; }

    public int DeliveryNoteNr { get; set; }

    public List<Device_DeliveryNote> Devices
    {
      get => this.devices;
      set => this.devices = value;
    }
  }
}
