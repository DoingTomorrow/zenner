// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.EnumExtensions
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public static class EnumExtensions
  {
    public static IEnumerable<Enum> GetFlags(this Enum value)
    {
      return EnumExtensions.GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray<Enum>());
    }

    public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
    {
      return EnumExtensions.GetFlags(value, EnumExtensions.GetFlagValues(value.GetType()).ToArray<Enum>());
    }

    private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
    {
      ulong uint64_1 = Convert.ToUInt64((object) value);
      List<Enum> source = new List<Enum>();
      for (int index = values.Length - 1; index >= 0; --index)
      {
        ulong uint64_2 = Convert.ToUInt64((object) values[index]);
        if (index != 0 || uint64_2 != 0UL)
        {
          if (((long) uint64_1 & (long) uint64_2) == (long) uint64_2)
          {
            source.Add(values[index]);
            uint64_1 -= uint64_2;
          }
        }
        else
          break;
      }
      if (uint64_1 > 0UL)
        return Enumerable.Empty<Enum>();
      if (Convert.ToUInt64((object) value) > 0UL)
        return source.Reverse<Enum>();
      return (long) uint64_1 == (long) Convert.ToUInt64((object) value) && values.Length != 0 && Convert.ToUInt64((object) values[0]) == 0UL ? ((IEnumerable<Enum>) values).Take<Enum>(1) : Enumerable.Empty<Enum>();
    }

    private static IEnumerable<Enum> GetFlagValues(Type enumType)
    {
      ulong flag = 1;
      foreach (Enum @enum in Enum.GetValues(enumType).Cast<Enum>())
      {
        Enum value = @enum;
        ulong bits = Convert.ToUInt64((object) value);
        if (bits != 0UL)
        {
          while (flag < bits)
            flag <<= 1;
          if ((long) flag == (long) bits)
            yield return value;
          value = (Enum) null;
        }
      }
    }
  }
}
