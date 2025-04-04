// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.PropertyToField
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public class PropertyToField
  {
    private static readonly Dictionary<string, IFieldNamingStrategy> FieldNamingStrategies = new Dictionary<string, IFieldNamingStrategy>()
    {
      {
        "camelcase",
        (IFieldNamingStrategy) new CamelCaseStrategy()
      },
      {
        "camelcase-underscore",
        (IFieldNamingStrategy) new CamelCaseUnderscoreStrategy()
      },
      {
        "lowercase",
        (IFieldNamingStrategy) new LowerCaseStrategy()
      },
      {
        "lowercase-underscore",
        (IFieldNamingStrategy) new LowerCaseUnderscoreStrategy()
      },
      {
        "pascalcase-underscore",
        (IFieldNamingStrategy) new PascalCaseUnderscoreStrategy()
      },
      {
        "pascalcase-m-underscore",
        (IFieldNamingStrategy) new PascalCaseMUnderscoreStrategy()
      }
    };

    public static IDictionary<string, IFieldNamingStrategy> DefaultStrategies
    {
      get => (IDictionary<string, IFieldNamingStrategy>) PropertyToField.FieldNamingStrategies;
    }

    public static FieldInfo GetBackFieldInfo(PropertyInfo subject)
    {
      return PropertyToField.FieldNamingStrategies.Values.Select(s => new
      {
        s = s,
        field = subject.DeclaringType.GetField(s.GetFieldName(subject.Name), BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
      }).Where(_param0 => _param0.field != null).Select(_param0 => _param0.field).FirstOrDefault<FieldInfo>();
    }
  }
}
