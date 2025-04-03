// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.DeliveryNote.DeliveryNoteContent
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;

#nullable disable
namespace ZENNER.CommonLibrary.DeliveryNote
{
  public class DeliveryNoteContent
  {
    private XML_ZR_Converter xmlConverter = new XML_ZR_Converter();

    public ZENNER.CommonLibrary.DeliveryNote.DeliveryNote LoadDeliveryNoteFile(
      string filePath,
      out string error)
    {
      error = string.Empty;
      try
      {
        return this.xmlConverter.readDeliveryNoteFile(filePath);
      }
      catch (Exception ex)
      {
        error = ex.Message;
        return (ZENNER.CommonLibrary.DeliveryNote.DeliveryNote) null;
      }
    }
  }
}
