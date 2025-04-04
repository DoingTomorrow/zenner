// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.ObjectUtils
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Proxy;

#nullable disable
namespace NHibernate.Util
{
  public sealed class ObjectUtils
  {
    private static object theNull = (object) new ObjectUtils.NullClass();

    private ObjectUtils()
    {
    }

    public static object DefaultIfNull(object obj, object defaultVal)
    {
      return obj == null ? defaultVal : obj;
    }

    public new static bool Equals(object obj1, object obj2) => object.Equals(obj1, obj2);

    public static string IdentityToString(object obj)
    {
      if (obj == null)
        return "null";
      if (obj is INHibernateProxy nhibernateProxy)
      {
        ILazyInitializer hibernateLazyInitializer = nhibernateProxy.HibernateLazyInitializer;
        return StringHelper.Unqualify(hibernateLazyInitializer.EntityName) + "#" + hibernateLazyInitializer.Identifier;
      }
      return StringHelper.Unqualify(obj.GetType().FullName) + "@" + (object) obj.GetHashCode() + "(hash)";
    }

    public static object Null => ObjectUtils.theNull;

    private class NullClass
    {
    }
  }
}
