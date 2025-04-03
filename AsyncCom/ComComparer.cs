// Decompiled with JetBrains decompiler
// Type: AsyncCom.ComComparer
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using System;
using System.Collections;

#nullable disable
namespace AsyncCom
{
  internal class ComComparer : IComparer
  {
    public int Compare(object x, object y)
    {
      string str1 = x as string;
      string str2 = y as string;
      if (str1 == null || str2 == null)
        throw new ArgumentException("Object is not of type ManagementObject");
      int num1;
      int num2;
      try
      {
        num1 = int.Parse(str1.Substring(3, str1.Length - 3));
        num2 = int.Parse(str2.Substring(3, str2.Length - 3));
      }
      catch
      {
        return 0;
      }
      return num1.CompareTo(num2);
    }
  }
}
