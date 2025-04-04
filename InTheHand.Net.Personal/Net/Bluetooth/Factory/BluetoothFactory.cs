// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.BluetoothFactory
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Widcomm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  [CLSCompliant(false)]
  public abstract class BluetoothFactory : IDisposable
  {
    private static IList<BluetoothFactory> s_factories;
    private static object lockKey = new object();

    protected virtual IL2CapClient GetL2CapClient() => throw new NotImplementedException();

    protected abstract IBluetoothClient GetBluetoothClient();

    protected abstract IBluetoothClient GetBluetoothClient(Socket acceptedSocket);

    protected abstract IBluetoothClient GetBluetoothClientForListener(
      CommonRfcommStream acceptedStrm);

    protected abstract IBluetoothClient GetBluetoothClient(BluetoothEndPoint localEP);

    protected abstract IBluetoothDeviceInfo GetBluetoothDeviceInfo(BluetoothAddress address);

    protected abstract IBluetoothListener GetBluetoothListener();

    protected abstract IBluetoothRadio GetPrimaryRadio();

    protected abstract IBluetoothRadio[] GetAllRadios();

    protected abstract IBluetoothSecurity GetBluetoothSecurity();

    void IDisposable.Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected abstract void Dispose(bool disposing);

    public IL2CapClient DoGetL2CapClient() => this.GetL2CapClient();

    public IBluetoothClient DoGetBluetoothClient() => this.GetBluetoothClient();

    public IBluetoothClient DoGetBluetoothClientForListener(CommonRfcommStream acceptedStream)
    {
      return this.GetBluetoothClientForListener(acceptedStream);
    }

    public IBluetoothClient DoGetBluetoothClient(Socket acceptedSocket)
    {
      return this.GetBluetoothClient(acceptedSocket);
    }

    public IBluetoothClient DoGetBluetoothClient(BluetoothEndPoint localEP)
    {
      return this.GetBluetoothClient(localEP);
    }

    public IBluetoothDeviceInfo DoGetBluetoothDeviceInfo(BluetoothAddress address)
    {
      return this.GetBluetoothDeviceInfo(address);
    }

    public IBluetoothListener DoGetBluetoothListener() => this.GetBluetoothListener();

    public IBluetoothRadio DoGetPrimaryRadio() => this.GetPrimaryRadio();

    public IBluetoothRadio[] DoGetAllRadios() => this.GetAllRadios();

    public IBluetoothSecurity DoGetBluetoothSecurity() => this.GetBluetoothSecurity();

    private static void GetStacks_inLock()
    {
      List<BluetoothFactory> list = new List<BluetoothFactory>();
      List<Exception> errors = new List<Exception>();
      List<string> stringList = new List<string>((IEnumerable<string>) BluetoothFactoryConfig.KnownStacks);
      BluetoothFactory.TraceWriteLibraryVersion();
      foreach (string typeName in stringList)
      {
        try
        {
          object instance = Activator.CreateInstance(Type.GetType(typeName, true));
          if (!(instance is IBluetoothFactoryFactory bluetoothFactoryFactory))
          {
            list.Add((BluetoothFactory) instance);
          }
          else
          {
            IList<BluetoothFactory> factories = bluetoothFactoryFactory.GetFactories((IList<Exception>) errors);
            if (factories != null)
              list.AddRange((IEnumerable<BluetoothFactory>) factories);
          }
          if (BluetoothFactoryConfig.OneStackOnly)
            break;
        }
        catch (Exception ex)
        {
          Exception exception = ex;
          if (exception is TargetInvocationException)
            exception = exception.InnerException;
          errors.Add(exception);
          string message = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Exception creating factory '{0}, ex: {1}", (object) typeName, (object) exception);
          if (BluetoothFactoryConfig.ReportAllErrors)
            Trace.Assert(false, message);
        }
      }
      if (list.Count == 0)
      {
        if (!BluetoothFactoryConfig.ReportAllErrors)
        {
          foreach (Exception @this in errors)
            MiscUtils.Trace_WriteLine(ExceptionExtension.ToStringNoStackTrace(@this));
        }
        Exception exception = (Exception) null;
        try
        {
          exception = WidcommBtIf.IsWidcommStackPresentButNotInterfaceDll();
        }
        catch (Exception ex)
        {
          if (Environment.OSVersion.Platform.ToString().StartsWith("Win", StringComparison.Ordinal))
            MiscUtils.Trace_WriteLine("Exception in IsWidcommStackPresentButNotInterfaceDll: " + (object) ex);
        }
        if (exception != null)
          throw exception;
        throw new PlatformNotSupportedException("No supported Bluetooth protocol stack found.");
      }
      BluetoothFactory.SetFactories_inLock(list);
    }

    internal static IList<BluetoothFactory> Factories
    {
      get
      {
        lock (BluetoothFactory.lockKey)
        {
          if (BluetoothFactory.s_factories == null)
            BluetoothFactory.GetStacks_inLock();
          return BluetoothFactory.s_factories;
        }
      }
    }

    internal static BluetoothFactory Factory => BluetoothFactory.Factories[0];

    internal static void SetFactory(BluetoothFactory factory)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      lock (BluetoothFactory.lockKey)
      {
        TestUtilities.IsUnderTestHarness();
        BluetoothFactory.SetFactories_inLock(new List<BluetoothFactory>((IEnumerable<BluetoothFactory>) new BluetoothFactory[1]
        {
          factory
        }));
      }
    }

    private static void SetFactories_inLock(List<BluetoothFactory> list)
    {
      BluetoothFactory.s_factories = (IList<BluetoothFactory>) list.AsReadOnly();
    }

    public static void HackShutdownAll()
    {
      lock (BluetoothFactory.lockKey)
      {
        if (BluetoothFactory.s_factories != null)
        {
          foreach (IDisposable factory in (IEnumerable<BluetoothFactory>) BluetoothFactory.s_factories)
            factory.Dispose();
        }
      }
      BluetoothFactory.s_factories = (IList<BluetoothFactory>) null;
    }

    public static TFactory GetTheFactoryOfTypeOrDefault<TFactory>() where TFactory : BluetoothFactory
    {
      foreach (BluetoothFactory factory in (IEnumerable<BluetoothFactory>) BluetoothFactory.Factories)
      {
        if ((object) (factory as TFactory) != null)
          return (TFactory) factory;
      }
      return default (TFactory);
    }

    public static BluetoothFactory GetTheFactoryOfTypeOrDefault(Type factoryType)
    {
      foreach (BluetoothFactory factory in (IEnumerable<BluetoothFactory>) BluetoothFactory.Factories)
      {
        if (factoryType.IsInstanceOfType((object) factory))
          return factory;
      }
      return (BluetoothFactory) null;
    }

    private static void TraceWriteLibraryVersion()
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      string fullName = executingAssembly.FullName;
      AssemblyName name = executingAssembly.GetName();
      Version version = name.Version;
      string informationalVersion = BluetoothFactory.GetCustomAttributes<AssemblyInformationalVersionAttribute>(executingAssembly)?.InformationalVersion;
      MiscUtils.Trace_WriteLine("32feet.NET: '{0}'\r\n   versions: '{1}' and '{2}'.", (object) name, (object) version, (object) informationalVersion);
    }

    private static TAttr GetCustomAttributes<TAttr>(Assembly a) where TAttr : Attribute
    {
      object[] customAttributes = a.GetCustomAttributes(typeof (TAttr), true);
      if (customAttributes.Length == 0)
        return default (TAttr);
      return customAttributes.Length <= 1 ? (TAttr) customAttributes[0] : throw new InvalidOperationException("Don't support multiple attribute instances.");
    }
  }
}
