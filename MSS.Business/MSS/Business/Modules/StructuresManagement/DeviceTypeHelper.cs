// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.DeviceTypeHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Meters;
using MSS.Utils.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public class DeviceTypeHelper
  {
    public static ObservableCollection<DeviceType> GetDeviceTypeCollection()
    {
      ObservableCollection<DeviceType> deviceTypeCollection = new ObservableCollection<DeviceType>();
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.MinotelContactRadio3, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.M7, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.C5MBus, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.EDCRadio, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.C5Radio, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.MultidataN1, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.MultidataS1, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.WR3, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      deviceTypeCollection.Add(DeviceTypeHelper.GetDeviceType(DeviceTypeEnum.Zelsius, "pack://application:,,,/Styles;component/Images/SmallIcons/home-selected.png"));
      return deviceTypeCollection;
    }

    private static DeviceType GetDeviceType(DeviceTypeEnum deviceTypeEnum, string imagePath)
    {
      return new DeviceType()
      {
        DeviceTypeEnum = deviceTypeEnum,
        DeviceName = deviceTypeEnum.GetStringValue(),
        DeviceTypeImage = new BitmapImage(new Uri(imagePath))
      };
    }
  }
}
