// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ValueIdentTimeListItem
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ValueIdentTimeListItem : IComparable<ValueIdentTimeListItem>
  {
    internal DateTime TimePoint;
    internal long ValueID;
    internal double Value;

    public ValueIdentTimeListItem(DateTime timePoint, long valueID, double value)
    {
      this.TimePoint = timePoint;
      this.ValueID = valueID;
      this.Value = value;
    }

    public int CompareTo(ValueIdentTimeListItem toObject)
    {
      int num = this.TimePoint.CompareTo(toObject.TimePoint);
      return num != 0 ? num : this.ValueID.CompareTo(toObject.ValueID);
    }

    public string ToPathLikeString(int itemNumber)
    {
      return itemNumber.ToString() + " " + ValueIdent.ToPathLine(this.TimePoint, this.Value, this.ValueID);
    }
  }
}
