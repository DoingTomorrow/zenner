// Decompiled with JetBrains decompiler
// Type: Fluent.DpiHelper
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  internal static class DpiHelper
  {
    private static Matrix transformToDevice;
    private static Matrix transformToDip;

    [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
    static DpiHelper()
    {
      IntPtr dc = NativeMethods.GetDC(IntPtr.Zero);
      int deviceCaps1 = NativeMethods.GetDeviceCaps(dc, 88);
      int deviceCaps2 = NativeMethods.GetDeviceCaps(dc, 90);
      DpiHelper.transformToDip = Matrix.Identity;
      DpiHelper.transformToDip.Scale(96.0 / (double) deviceCaps1, 96.0 / (double) deviceCaps2);
      DpiHelper.transformToDevice = Matrix.Identity;
      DpiHelper.transformToDevice.Scale((double) deviceCaps1 / 96.0, (double) deviceCaps2 / 96.0);
      NativeMethods.ReleaseDC(IntPtr.Zero, dc);
    }

    public static Point LogicalPixelsToDevice(Point logicalPoint)
    {
      return DpiHelper.transformToDevice.Transform(logicalPoint);
    }

    public static Point DevicePixelsToLogical(Point devicePoint)
    {
      return DpiHelper.transformToDip.Transform(devicePoint);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static System.Windows.Rect LogicalRectToDevice(System.Windows.Rect logicalRectangle)
    {
      return new System.Windows.Rect(DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top)), DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom)));
    }

    public static System.Windows.Rect DeviceRectToLogical(System.Windows.Rect deviceRectangle)
    {
      return new System.Windows.Rect(DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top)), DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom)));
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static Size LogicalSizeToDevice(Size logicalSize)
    {
      Point device = DpiHelper.LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height));
      return new Size()
      {
        Width = device.X,
        Height = device.Y
      };
    }

    public static Size DeviceSizeToLogical(Size deviceSize)
    {
      Point logical = DpiHelper.DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height));
      return new Size(logical.X, logical.Y);
    }
  }
}
