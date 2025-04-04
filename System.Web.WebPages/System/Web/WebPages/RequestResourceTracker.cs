// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.RequestResourceTracker
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.WebPages
{
  internal static class RequestResourceTracker
  {
    private static readonly object _resourcesKey = new object();

    private static List<RequestResourceTracker.SecureWeakReference> GetResources(
      HttpContextBase context)
    {
      List<RequestResourceTracker.SecureWeakReference> resources = (List<RequestResourceTracker.SecureWeakReference>) context.Items[RequestResourceTracker._resourcesKey];
      if (resources == null)
      {
        resources = new List<RequestResourceTracker.SecureWeakReference>();
        context.Items[RequestResourceTracker._resourcesKey] = (object) resources;
      }
      return resources;
    }

    internal static void DisposeResources(HttpContextBase context)
    {
      List<RequestResourceTracker.SecureWeakReference> resources = RequestResourceTracker.GetResources(context);
      if (resources == null)
        return;
      resources.ForEach((Action<RequestResourceTracker.SecureWeakReference>) (resource => resource.Dispose()));
      resources.Clear();
    }

    internal static void RegisterForDispose(HttpContextBase context, IDisposable resource)
    {
      RequestResourceTracker.GetResources(context)?.Add(new RequestResourceTracker.SecureWeakReference(resource));
    }

    internal static void RegisterForDispose(IDisposable resource)
    {
      HttpContext current = HttpContext.Current;
      if (current == null)
        return;
      RequestResourceTracker.RegisterForDispose((HttpContextBase) new HttpContextWrapper(current), resource);
    }

    private sealed class SecureWeakReference
    {
      private readonly WeakReference _reference;

      public SecureWeakReference(IDisposable reference)
      {
        this._reference = new WeakReference((object) reference);
      }

      internal void Dispose() => ((IDisposable) this._reference.Target)?.Dispose();
    }
  }
}
