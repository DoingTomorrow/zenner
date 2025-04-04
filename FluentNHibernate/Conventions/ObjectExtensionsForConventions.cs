// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.ObjectExtensionsForConventions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public static class ObjectExtensionsForConventions
  {
    public static bool IsAny<T>(this T target, params T[] possibleValues)
    {
      return ((IEnumerable<T>) possibleValues).Contains<T>(target);
    }

    public static bool IsNotAny<T>(this T target, params T[] possibleValues)
    {
      return !target.IsAny<T>(possibleValues);
    }
  }
}
