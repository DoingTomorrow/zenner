// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AreaRegistration
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Reflection;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class AreaRegistration
  {
    private const string TypeCacheName = "MVC-AreaRegistrationTypeCache.xml";

    public abstract string AreaName { get; }

    internal void CreateContextAndRegister(RouteCollection routes, object state)
    {
      AreaRegistrationContext context = new AreaRegistrationContext(this.AreaName, routes, state);
      string str = this.GetType().Namespace;
      if (str != null)
        context.Namespaces.Add(str + ".*");
      this.RegisterArea(context);
    }

    private static bool IsAreaRegistrationType(Type type)
    {
      return typeof (AreaRegistration).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != (ConstructorInfo) null;
    }

    public static void RegisterAllAreas() => AreaRegistration.RegisterAllAreas((object) null);

    public static void RegisterAllAreas(object state)
    {
      AreaRegistration.RegisterAllAreas(RouteTable.Routes, (IBuildManager) new BuildManagerWrapper(), state);
    }

    internal static void RegisterAllAreas(
      RouteCollection routes,
      IBuildManager buildManager,
      object state)
    {
      foreach (Type typesFromAssembly in TypeCacheUtil.GetFilteredTypesFromAssemblies("MVC-AreaRegistrationTypeCache.xml", new Predicate<Type>(AreaRegistration.IsAreaRegistrationType), buildManager))
        ((AreaRegistration) Activator.CreateInstance(typesFromAssembly)).CreateContextAndRegister(routes, state);
    }

    public abstract void RegisterArea(AreaRegistrationContext context);
  }
}
