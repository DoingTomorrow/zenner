// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothRadio
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [DebuggerDisplay("impl={m_impl}")]
  public sealed class BluetoothRadio
  {
    private readonly IBluetoothRadio m_impl;
    private readonly BluetoothPublicFactory m_publicFactory;

    public static BluetoothRadio[] AllRadios
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        IEnumerable factories;
        try
        {
          factories = (IEnumerable) BluetoothFactory.Factories;
        }
        catch (PlatformNotSupportedException ex)
        {
          return new BluetoothRadio[0];
        }
        foreach (BluetoothFactory factory in factories)
        {
          foreach (IBluetoothRadio allRadio in factory.DoGetAllRadios())
            arrayList.Add((object) new BluetoothRadio(factory, allRadio));
        }
        return (BluetoothRadio[]) arrayList.ToArray(typeof (BluetoothRadio));
      }
    }

    public static BluetoothRadio PrimaryRadio
    {
      get
      {
        try
        {
          return new BluetoothRadio(BluetoothFactory.Factory, BluetoothFactory.Factory.DoGetPrimaryRadio());
        }
        catch (PlatformNotSupportedException ex)
        {
          return (BluetoothRadio) null;
        }
      }
    }

    public static bool IsSupported => BluetoothRadio.AllRadios.Length > 0;

    private BluetoothRadio(BluetoothFactory factory, IBluetoothRadio impl)
    {
      this.m_impl = impl;
      this.m_publicFactory = new BluetoothPublicFactory(factory);
    }

    public BluetoothPublicFactory StackFactory
    {
      [DebuggerStepThrough] get => this.m_publicFactory;
    }

    public string Remote
    {
      [DebuggerStepThrough] get => this.m_impl.Remote;
    }

    public IntPtr Handle
    {
      [DebuggerStepThrough] get => this.m_impl.Handle;
    }

    public HardwareStatus HardwareStatus
    {
      [DebuggerStepThrough] get => this.m_impl.HardwareStatus;
    }

    public RadioMode Mode
    {
      [DebuggerStepThrough] get => this.m_impl.Mode;
      [DebuggerStepThrough] set => this.m_impl.Mode = value;
    }

    public BluetoothAddress LocalAddress
    {
      [DebuggerStepThrough] get => this.m_impl.LocalAddress;
    }

    public string Name
    {
      [DebuggerStepThrough] get => this.m_impl.Name;
      [DebuggerStepThrough] set => this.m_impl.Name = value;
    }

    public ClassOfDevice ClassOfDevice
    {
      [DebuggerStepThrough] get => this.m_impl.ClassOfDevice;
    }

    public Manufacturer Manufacturer
    {
      [DebuggerStepThrough] get => this.m_impl.Manufacturer;
    }

    public HciVersion HciVersion
    {
      [DebuggerStepThrough] get => this.m_impl.HciVersion;
    }

    public int HciRevision
    {
      [DebuggerStepThrough] get => this.m_impl.HciRevision;
    }

    public LmpVersion LmpVersion
    {
      [DebuggerStepThrough] get => this.m_impl.LmpVersion;
    }

    public int LmpSubversion
    {
      [DebuggerStepThrough] get => this.m_impl.LmpSubversion;
    }

    public Manufacturer SoftwareManufacturer
    {
      [DebuggerStepThrough] get => this.m_impl.SoftwareManufacturer;
    }

    private string DebugImplType => this.m_impl == null ? "(null)" : this.m_impl.GetType().Name;
  }
}
