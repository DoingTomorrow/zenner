// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.VirtualPathFactoryExtensions
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  internal static class VirtualPathFactoryExtensions
  {
    public static T CreateInstance<T>(this IVirtualPathFactory factory, string virtualPath) where T : class
    {
      switch (factory)
      {
        case VirtualPathFactoryManager pathFactoryManager:
          return pathFactoryManager.CreateInstanceOfType<T>(virtualPath);
        case BuildManagerWrapper buildManagerWrapper:
          return buildManagerWrapper.CreateInstanceOfType<T>(virtualPath);
        default:
          return factory.CreateInstance(virtualPath) as T;
      }
    }
  }
}
