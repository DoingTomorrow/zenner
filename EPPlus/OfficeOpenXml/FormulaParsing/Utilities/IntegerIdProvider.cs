// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Utilities.IntegerIdProvider
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Utilities
{
  public class IntegerIdProvider : IdProvider
  {
    private int _lastId = int.MinValue;

    public override object NewId()
    {
      if (this._lastId >= int.MaxValue)
        throw new InvalidOperationException("IdProvider run out of id:s");
      return (object) this._lastId++;
    }
  }
}
