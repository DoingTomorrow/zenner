// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.MSSHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Common.Library.NHibernate.Data;
using GmmDbLib;
using MSS.Business.Errors;
using MSS.Business.Modules.Archiving;
using MSS.Business.Modules.LicenseManagement;
using MSS.Localisation;
using MSS.Utils.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.ServiceModel;
using Telerik.Windows.Data;
using ZENNER;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZENNER.Managers;

#nullable disable
namespace MSS.Business.Utils
{
  public static class MSSHelper
  {
    public static bool InitializeGMM()
    {
      string currentLicenseFileName;
      if (!string.IsNullOrEmpty(currentLicenseFileName = LicenseHelper.GetCurrentLicenseFileName(LicenseHelper.GetValidHardwareKey())))
      {
        try
        {
          GmmInterface.Initialize(currentLicenseFileName, MeterDbTypes.SQLite, Path.Combine(AppDataFolderHelper.GetUserAppDataPath(), ConfigurationManager.AppSettings["GMMDatabasePath"]));
        }
        catch (Exception ex)
        {
          MessageHandler.LogException(ex);
          return false;
        }
        return true;
      }
      MessageHandler.LogDebug("License not valid for GMM or does not exist. GMM could not be initialized.");
      return false;
    }

    public static bool InitializeGMMAndSetEquipment()
    {
      if (!MSSHelper.InitializeGMM())
        return false;
      AppContext.Current.LoadDefaultEquipment();
      return true;
    }

    public static bool IsMinoConnectConnected()
    {
      if (!LicenseHelper.LicenseExistsAndIsValid() || !LicenseHelper.IsMinoConnectNeeded(LicenseHelper.GetValidHardwareKey()))
        return false;
      bool flag = false;
      foreach (ValueItem availableComPort in Constants.GetAvailableComPorts())
      {
        try
        {
          flag |= MinoConnectManager.IsMinoConnect(availableComPort.Value);
        }
        catch (Exception ex)
        {
          MessageHandler.LogException(ex);
        }
      }
      return flag;
    }

    public static NetTcpBinding GetNetTcpBinding()
    {
      NetTcpBinding netTcpBinding = new NetTcpBinding()
      {
        TransactionFlow = false
      };
      netTcpBinding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
      netTcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
      netTcpBinding.Security.Mode = SecurityMode.None;
      netTcpBinding.MaxReceivedMessageSize = (long) int.MaxValue;
      netTcpBinding.OpenTimeout = new TimeSpan(0, 10, 0);
      netTcpBinding.CloseTimeout = new TimeSpan(0, 10, 0);
      netTcpBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
      netTcpBinding.SendTimeout = new TimeSpan(0, 10, 0);
      netTcpBinding.MaxReceivedMessageSize = 734003200L;
      netTcpBinding.MaxBufferSize = 734003200;
      netTcpBinding.MaxBufferPoolSize = 734003200L;
      return netTcpBinding;
    }

    public static List<T> GetUnique<T>(this List<T> nonUniqueItems)
    {
      return nonUniqueItems.GroupBy<T, object>((Func<T, object>) (item => item.GetType().GetProperty("Id").GetValue((object) item))).Select<IGrouping<object, T>, T>((Func<IGrouping<object, T>, T>) (item => item.First<T>())).ToList<T>();
    }

    public static bool IsSimpleType(this Type type)
    {
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
        type = Nullable.GetUnderlyingType(type);
      return type.IsPrimitive || type == typeof (DateTime) || type == typeof (Decimal) || type == typeof (string) || type == typeof (Guid) || type == typeof (int) || type == typeof (long) || type == typeof (long) || type == typeof (double) || type == typeof (int);
    }

    private static bool IsIList(Type type) => typeof (IList).IsAssignableFrom(type);

    public static string GetErrorMessage(Exception ex)
    {
      return CultureResources.GetValue(ErrorCodes.GetErrorMessage("MSSError_3")) + Environment.NewLine + "Message:" + ex.Message + Environment.NewLine + "Inner Exception:" + (ex.InnerException != null ? ex.InnerException.Message : string.Empty) + Environment.NewLine + "Stack Trace:" + ex.StackTrace;
    }

    public static string GenerateRandomNumber(int length)
    {
      return BitConverter.ToUInt32(Guid.NewGuid().ToByteArray(), length).ToString();
    }

    public static IEnumerable<T2> GetListOfObjectsFromEnum<T1, T2>()
    {
      ObservableCollection<T2> ofObjectsFromEnum = new ObservableCollection<T2>();
      Dictionary<string, string> enumElements = EnumHelper.GetEnumElements<T1>();
      int num = 1;
      foreach (KeyValuePair<string, string> keyValuePair in enumElements)
      {
        object obj = Enum.Parse(typeof (T1), keyValuePair.Key, true);
        ofObjectsFromEnum.Add((T2) Activator.CreateInstance(typeof (T2), (object) num, (object) ((Enum) obj).GetStringValue(), obj));
        ++num;
      }
      return (IEnumerable<T2>) ofObjectsFromEnum;
    }

    public static string ParseJsonToString(string jsonString)
    {
      string empty = string.Empty;
      return ((IEnumerable<string>) jsonString.Split(',')).Aggregate<string, string>(empty, (Func<string, string, string>) ((current, item) => current + item.TrimStart('{').TrimEnd('}') + Environment.NewLine));
    }

    public static void InitializeNHibernateFactoryForMSSDatabase()
    {
      HibernateMultipleDatabasesManager.Initialize(ConfigurationManager.AppSettings["DatabaseEngine"]);
    }

    public static Telerik.Windows.Data.VirtualQueryableCollectionView<T> LoadCollection<T>(
      Expression<Func<T, bool>> mySearchExpression)
    {
      return MSSHelper.VirtualQueryableCollectionView<T>(new PagingProvider<T>(ArchiveManagerNHibernate.GetSessionFactoryMSSArchive(), mySearchExpression));
    }

    public static Telerik.Windows.Data.VirtualQueryableCollectionView<T> LoadCollection<T>()
    {
      return MSSHelper.VirtualQueryableCollectionView<T>(new PagingProvider<T>(ArchiveManagerNHibernate.GetSessionFactoryMSSArchive()));
    }

    public static Telerik.Windows.Data.VirtualQueryableCollectionView<T> VirtualQueryableCollectionView<T>(
      PagingProvider<T> pagingProvider)
    {
      Telerik.Windows.Data.VirtualQueryableCollectionView<T> queryableCollectionView = new Telerik.Windows.Data.VirtualQueryableCollectionView<T>();
      queryableCollectionView.LoadSize = AppContext.Current.GetParameterValue<int>("LoadSizeForVirtualScrolling");
      queryableCollectionView.VirtualItemCount = pagingProvider.FetchCount();
      Telerik.Windows.Data.VirtualQueryableCollectionView<T> collection = queryableCollectionView;
      collection.ItemsLoading += (EventHandler<VirtualQueryableCollectionViewItemsLoadingEventArgs>) ((s, args) => collection.Load(args.StartIndex, (IEnumerable) pagingProvider.FetchRange(args.StartIndex, args.ItemCount)));
      return collection;
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
      IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      HashSet<TKey> seenKeys = new HashSet<TKey>();
      foreach (TSource source1 in source)
      {
        TSource element = source1;
        if (seenKeys.Add(keySelector(element)))
          yield return element;
        element = default (TSource);
      }
    }

    public static ObservableCollection<T> ListToObsCollection<T>(IEnumerable<T> enumCollection)
    {
      List<T> list = enumCollection.ToList<T>();
      ObservableCollection<T> obsCollection = new ObservableCollection<T>();
      list.ForEach(new Action<T>(((Collection<T>) obsCollection).Add));
      return obsCollection;
    }
  }
}
