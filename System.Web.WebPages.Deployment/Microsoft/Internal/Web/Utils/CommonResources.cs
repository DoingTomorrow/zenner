// Decompiled with JetBrains decompiler
// Type: Microsoft.Internal.Web.Utils.CommonResources
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Internal.Web.Utils
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  internal class CommonResources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal CommonResources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) CommonResources.resourceMan, (object) null))
        {
          string str = ((IEnumerable<string>) Assembly.GetExecutingAssembly().GetManifestResourceNames()).Where<string>((Func<string, bool>) (s => s.EndsWith("CommonResources.resources", StringComparison.OrdinalIgnoreCase))).Single<string>();
          CommonResources.resourceMan = new ResourceManager(str.Substring(0, str.Length - 10), typeof (CommonResources).Assembly);
        }
        return CommonResources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => CommonResources.resourceCulture;
      set => CommonResources.resourceCulture = value;
    }

    internal static string Argument_Cannot_Be_Null_Or_Empty
    {
      get
      {
        return CommonResources.ResourceManager.GetString(nameof (Argument_Cannot_Be_Null_Or_Empty), CommonResources.resourceCulture);
      }
    }

    internal static string Argument_Must_Be_Between
    {
      get
      {
        return CommonResources.ResourceManager.GetString(nameof (Argument_Must_Be_Between), CommonResources.resourceCulture);
      }
    }

    internal static string Argument_Must_Be_Enum_Member
    {
      get
      {
        return CommonResources.ResourceManager.GetString(nameof (Argument_Must_Be_Enum_Member), CommonResources.resourceCulture);
      }
    }

    internal static string Argument_Must_Be_GreaterThan
    {
      get
      {
        return CommonResources.ResourceManager.GetString(nameof (Argument_Must_Be_GreaterThan), CommonResources.resourceCulture);
      }
    }

    internal static string Argument_Must_Be_GreaterThanOrEqualTo
    {
      get
      {
        return CommonResources.ResourceManager.GetString(nameof (Argument_Must_Be_GreaterThanOrEqualTo), CommonResources.resourceCulture);
      }
    }

    internal static string Argument_Must_Be_LessThan
    {
      get
      {
        return CommonResources.ResourceManager.GetString(nameof (Argument_Must_Be_LessThan), CommonResources.resourceCulture);
      }
    }

    internal static string Argument_Must_Be_LessThanOrEqualTo
    {
      get
      {
        return CommonResources.ResourceManager.GetString(nameof (Argument_Must_Be_LessThanOrEqualTo), CommonResources.resourceCulture);
      }
    }

    internal static string Argument_Must_Be_Null_Or_Non_Empty
    {
      get
      {
        return CommonResources.ResourceManager.GetString(nameof (Argument_Must_Be_Null_Or_Non_Empty), CommonResources.resourceCulture);
      }
    }
  }
}
