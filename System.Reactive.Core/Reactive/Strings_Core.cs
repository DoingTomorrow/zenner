// Decompiled with JetBrains decompiler
// Type: System.Reactive.Strings_Core
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Reactive
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Strings_Core
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Strings_Core()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Strings_Core.resourceMan == null)
          Strings_Core.resourceMan = new ResourceManager("System.Reactive.Core.Strings_Core", typeof (Strings_Core).Assembly);
        return Strings_Core.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Strings_Core.resourceCulture;
      set => Strings_Core.resourceCulture = value;
    }

    internal static string CANT_OBTAIN_SCHEDULER
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (CANT_OBTAIN_SCHEDULER), Strings_Core.resourceCulture);
      }
    }

    internal static string COMPLETED_NO_VALUE
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (COMPLETED_NO_VALUE), Strings_Core.resourceCulture);
      }
    }

    internal static string DISPOSABLE_ALREADY_ASSIGNED
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (DISPOSABLE_ALREADY_ASSIGNED), Strings_Core.resourceCulture);
      }
    }

    internal static string FAILED_CLOCK_MONITORING
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (FAILED_CLOCK_MONITORING), Strings_Core.resourceCulture);
      }
    }

    internal static string HEAP_EMPTY
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (HEAP_EMPTY), Strings_Core.resourceCulture);
      }
    }

    internal static string REENTRANCY_DETECTED
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (REENTRANCY_DETECTED), Strings_Core.resourceCulture);
      }
    }

    internal static string OBSERVER_TERMINATED
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (OBSERVER_TERMINATED), Strings_Core.resourceCulture);
      }
    }

    internal static string SCHEDULER_OPERATION_ALREADY_AWAITED
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (SCHEDULER_OPERATION_ALREADY_AWAITED), Strings_Core.resourceCulture);
      }
    }

    internal static string DISPOSABLES_CANT_CONTAIN_NULL
    {
      get
      {
        return Strings_Core.ResourceManager.GetString(nameof (DISPOSABLES_CANT_CONTAIN_NULL), Strings_Core.resourceCulture);
      }
    }
  }
}
