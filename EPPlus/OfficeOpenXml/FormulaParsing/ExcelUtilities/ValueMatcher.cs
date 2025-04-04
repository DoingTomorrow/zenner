// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.ValueMatcher
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class ValueMatcher
  {
    public const int IncompatibleOperands = -2;

    public virtual int IsMatch(object o1, object o2)
    {
      if (o1 != null && o2 == null)
        return 1;
      if (o1 == null && o2 != null)
        return -1;
      if (o1 == null && o2 == null)
        return 0;
      if (o1 is string && o2 is string)
        return this.CompareStringToString(o1.ToString().ToLower(), o2.ToString().ToLower());
      if (o1.GetType() == typeof (string))
        return this.CompareStringToObject(o1.ToString(), o2);
      return o2.GetType() == typeof (string) ? this.CompareObjectToString(o1, o2.ToString()) : Convert.ToDouble(o1).CompareTo(Convert.ToDouble(o2));
    }

    protected virtual int CompareStringToString(string s1, string s2) => s1.CompareTo(s2);

    protected virtual int CompareStringToObject(string o1, object o2)
    {
      double result;
      return double.TryParse(o1, out result) ? result.CompareTo(Convert.ToDouble(o2)) : -2;
    }

    protected virtual int CompareObjectToString(object o1, string o2)
    {
      double result;
      return double.TryParse(o2, out result) ? Convert.ToDouble(o1).CompareTo(result) : -2;
    }
  }
}
