// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterMathRes
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace ZR_ClassLibrary
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class MeterMathRes
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal MeterMathRes()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (MeterMathRes.resourceMan == null)
          MeterMathRes.resourceMan = new ResourceManager("ZR_ClassLibrary.MeterMathRes", typeof (MeterMathRes).Assembly);
        return MeterMathRes.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => MeterMathRes.resourceCulture;
      set => MeterMathRes.resourceCulture = value;
    }

    internal static string EnergyUnitNotAvailable
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (EnergyUnitNotAvailable), MeterMathRes.resourceCulture);
      }
    }

    internal static string FlowOutOfRange
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (FlowOutOfRange), MeterMathRes.resourceCulture);
      }
    }

    internal static string Input1ToManyDecimalPlaces
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (Input1ToManyDecimalPlaces), MeterMathRes.resourceCulture);
      }
    }

    internal static string Input1ToOutOfRange
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (Input1ToOutOfRange), MeterMathRes.resourceCulture);
      }
    }

    internal static string Input1UnitNotAvailable
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (Input1UnitNotAvailable), MeterMathRes.resourceCulture);
      }
    }

    internal static string Input2ToManyDecimalPlaces
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (Input2ToManyDecimalPlaces), MeterMathRes.resourceCulture);
      }
    }

    internal static string Input2ToOutOfRange
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (Input2ToOutOfRange), MeterMathRes.resourceCulture);
      }
    }

    internal static string Input2UnitNotAvailable
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (Input2UnitNotAvailable), MeterMathRes.resourceCulture);
      }
    }

    internal static string MathematicError
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (MathematicError), MeterMathRes.resourceCulture);
      }
    }

    internal static string NoError
    {
      get => MeterMathRes.ResourceManager.GetString(nameof (NoError), MeterMathRes.resourceCulture);
    }

    internal static string PowerOutOfRange
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (PowerOutOfRange), MeterMathRes.resourceCulture);
      }
    }

    internal static string PowerUnitNotAvailable
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (PowerUnitNotAvailable), MeterMathRes.resourceCulture);
      }
    }

    internal static string PulsValueOutOfRange
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (PulsValueOutOfRange), MeterMathRes.resourceCulture);
      }
    }

    internal static string VolumeUnitNotAvailable
    {
      get
      {
        return MeterMathRes.ResourceManager.GetString(nameof (VolumeUnitNotAvailable), MeterMathRes.resourceCulture);
      }
    }
  }
}
