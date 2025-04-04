// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.StringLikeness
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Utils
{
  public static class StringLikeness
  {
    public static int EditDistance(string x, string y)
    {
      if (x == null)
        throw new ArgumentNullException(nameof (x));
      if (y == null)
        throw new ArgumentNullException(nameof (y));
      List<char> list1 = x.ToList<char>();
      List<char> list2 = y.ToList<char>();
      int count1 = list1.Count;
      int count2 = list2.Count;
      if (count1 == 0)
        return count2;
      if (count2 == 0)
        return count1;
      int index1 = 0;
      int index2 = 1;
      int[][] numArray = new int[2][]
      {
        new int[count2 + 1],
        new int[count2 + 1]
      };
      for (int index3 = 0; index3 <= count2; ++index3)
        numArray[index1][index3] = index3;
      for (int index4 = 1; index4 <= count1; ++index4)
      {
        numArray[index2][0] = index4;
        for (int index5 = 1; index5 <= count2; ++index5)
        {
          int val1_1 = numArray[index1][index5] + 1;
          int val1_2 = numArray[index2][index5 - 1] + 1;
          int val2 = numArray[index1][index5 - 1] + (list1[index4 - 1].Equals(list2[index5 - 1]) ? 0 : 1);
          numArray[index2][index5] = Math.Min(val1_1, Math.Min(val1_2, val2));
        }
        if (index1 == 0)
        {
          index1 = 1;
          index2 = 0;
        }
        else
        {
          index1 = 0;
          index2 = 1;
        }
      }
      return numArray[index1][count2];
    }
  }
}
