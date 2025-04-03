// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.GenericArchiver
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Archiving;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSSArchive.Core.Model.DataCollectors;
using MSSArchive.Core.Model.Jobs;
using MSSArchive.Core.Model.Logs;
using MSSArchive.Core.Model.Meters;
using MSSArchive.Core.Model.Orders;
using MSSArchive.Core.Model.Structures;
using MSSArchive.Core.Utils;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  internal abstract class GenericArchiver
  {
    protected string ConnStringMSS { get; set; }

    protected string ConnStringArchive { get; set; }

    protected Guid ArchiveId { get; set; }

    protected Dictionary<Type, DbType> TypeMapper { get; set; }

    protected ISessionFactory SessionFactory { get; set; }

    internal void ArchiveReadingData(ArchiveJob archiveJob)
    {
      this.ArchiveEntity<MeterReadingValue, ArchiveMeterReadingValue>(new DatasourcesInfo()
      {
        SourceTableName = "t_ReadingValues",
        DestinationTableName = "t_ReadingValues",
        SourceViewName = "view_ArchiveReadingValues"
      }, archiveJob);
    }

    internal void ArchiveStructures(ArchiveJob archiveJob)
    {
      this.ArchiveEntity<Meter, ArchiveMeter>(new DatasourcesInfo()
      {
        SourceTableName = "t_Meter",
        DestinationTableName = "t_Meter",
        SourceViewName = "view_ArchiveMeter"
      }, archiveJob);
      this.ArchiveEntity<Location, ArchiveLocation>(new DatasourcesInfo()
      {
        SourceTableName = "t_Location",
        DestinationTableName = "t_Location",
        SourceViewName = "view_ArchiveLocation"
      }, archiveJob);
      this.ArchiveEntity<Tenant, ArchiveTenant>(new DatasourcesInfo()
      {
        SourceTableName = "t_Tenant",
        DestinationTableName = "t_Tenant",
        SourceViewName = "view_ArchiveTenant"
      }, archiveJob);
      this.ArchiveEntity<Minomat, ArchiveMinomat>(new DatasourcesInfo()
      {
        SourceTableName = "t_Minomat",
        DestinationTableName = "t_Minomat",
        SourceViewName = "view_ArchiveMinomat"
      }, archiveJob);
      this.ArchiveEntity<MeterRadioDetails, ArchiveMeterRadioDetails>(new DatasourcesInfo()
      {
        SourceTableName = "t_MeterRadioDetails",
        DestinationTableName = "t_MeterRadioDetails",
        SourceViewName = "view_ArchiveMeterRadioDetails"
      }, archiveJob);
      this.ArchiveEntity<MbusRadioMeter, ArchiveMbusRadioMeter>(new DatasourcesInfo()
      {
        SourceTableName = "t_MeterMbusRadio",
        DestinationTableName = "t_MeterMbusRadio",
        SourceViewName = "view_ArchiveMeterMbusRadio"
      }, archiveJob);
      this.ArchiveEntity<MinomatRadioDetails, ArchiveMinomatRadioDetails>(new DatasourcesInfo()
      {
        SourceTableName = "t_MinomatRadioDetails",
        DestinationTableName = "t_MinomatRadioDetails",
        SourceViewName = "view_ArchiveMinomatRadioDetails"
      }, archiveJob);
      this.ArchiveEntity<StructureNodeLinks, ArchiveStructureNodeLinks>(new DatasourcesInfo()
      {
        SourceTableName = "t_StructureNodeLinks",
        DestinationTableName = "t_StructureNodeLinks",
        SourceViewName = "view_ArchiveStructureNodeLinks"
      }, archiveJob);
      this.ArchiveEntity<StructureNode, ArchiveStructureNode>(new DatasourcesInfo()
      {
        SourceTableName = "t_StructureNode",
        DestinationTableName = "t_StructureNode",
        SourceViewName = "view_ArchiveStructureNode"
      }, archiveJob);
    }

    internal void ArchiveOrders(ArchiveJob archiveJob)
    {
      this.ArchiveEntity<Order, ArchiveOrder>(new DatasourcesInfo()
      {
        SourceTableName = "t_Order",
        DestinationTableName = "t_Order",
        SourceViewName = "view_ArchiveOrders"
      }, archiveJob);
    }

    internal void ArchiveJobs(ArchiveJob archiveJob)
    {
      this.ArchiveEntity<MssReadingJob, ArchiveMssReadingJob>(new DatasourcesInfo()
      {
        SourceTableName = "t_Job",
        DestinationTableName = "t_Job",
        SourceViewName = "view_ArchiveMssReadingJobs"
      }, archiveJob);
    }

    internal void ArchiveLogs(ArchiveJob archiveJob)
    {
      this.ArchiveEntity<MinomatConnectionLog, ArchiveMinomatConnectionLogs>(new DatasourcesInfo()
      {
        SourceTableName = "t_MinomatConnectionLog",
        DestinationTableName = "t_MinomatConnectionLog",
        SourceViewName = "view_ArchiveLogs"
      }, archiveJob);
    }

    internal abstract void SaveArchiveInformation(ArchiveDetailsADO archiveDetails);

    internal abstract void Initialize();

    internal abstract void Commit();

    internal abstract void Error();

    protected abstract string GetSelectStatement<SourceEntity>(string tableName);

    protected abstract string GetInsertStatement<T>(string tableName);

    protected abstract void UpdateArchiveInformationId(string tableName, Guid archiveJobId);

    protected abstract void ArchiveEntity<SourceEntity, DestinationEntity>(
      DatasourcesInfo dsInfo,
      ArchiveJob archiveJob);

    protected PropertyInfo[] GetProperties<T>()
    {
      return ((IEnumerable<PropertyInfo>) typeof (T).GetProperties()).Where<PropertyInfo>((System.Func<PropertyInfo, bool>) (p => p.GetCustomAttributes(typeof (ExcludePropertyAttribute), true).Length == 0)).ToArray<PropertyInfo>();
    }

    protected string GetSourceName(DatasourcesInfo dsInfo)
    {
      return string.IsNullOrEmpty(dsInfo.SourceViewName) ? dsInfo.SourceTableName : dsInfo.SourceViewName;
    }

    protected void InitializeTypesConverter()
    {
      this.TypeMapper = new Dictionary<Type, DbType>();
      this.TypeMapper[typeof (byte)] = DbType.Byte;
      this.TypeMapper[typeof (sbyte)] = DbType.SByte;
      this.TypeMapper[typeof (short)] = DbType.Int16;
      this.TypeMapper[typeof (ushort)] = DbType.UInt16;
      this.TypeMapper[typeof (int)] = DbType.Int32;
      this.TypeMapper[typeof (uint)] = DbType.UInt32;
      this.TypeMapper[typeof (long)] = DbType.Int64;
      this.TypeMapper[typeof (ulong)] = DbType.UInt64;
      this.TypeMapper[typeof (float)] = DbType.Single;
      this.TypeMapper[typeof (double)] = DbType.Double;
      this.TypeMapper[typeof (Decimal)] = DbType.Decimal;
      this.TypeMapper[typeof (bool)] = DbType.Boolean;
      this.TypeMapper[typeof (string)] = DbType.String;
      this.TypeMapper[typeof (char)] = DbType.StringFixedLength;
      this.TypeMapper[typeof (Guid)] = DbType.Guid;
      this.TypeMapper[typeof (DateTime)] = DbType.DateTime;
      this.TypeMapper[typeof (DateTimeOffset)] = DbType.DateTimeOffset;
      this.TypeMapper[typeof (byte[])] = DbType.Binary;
      this.TypeMapper[typeof (byte?)] = DbType.Byte;
      this.TypeMapper[typeof (sbyte?)] = DbType.SByte;
      this.TypeMapper[typeof (short?)] = DbType.Int16;
      this.TypeMapper[typeof (ushort?)] = DbType.UInt16;
      this.TypeMapper[typeof (int?)] = DbType.Int32;
      this.TypeMapper[typeof (uint?)] = DbType.UInt32;
      this.TypeMapper[typeof (long?)] = DbType.Int64;
      this.TypeMapper[typeof (ulong?)] = DbType.UInt64;
      this.TypeMapper[typeof (float?)] = DbType.Single;
      this.TypeMapper[typeof (double?)] = DbType.Double;
      this.TypeMapper[typeof (Decimal?)] = DbType.Decimal;
      this.TypeMapper[typeof (bool?)] = DbType.Boolean;
      this.TypeMapper[typeof (char?)] = DbType.StringFixedLength;
      this.TypeMapper[typeof (Guid?)] = DbType.Guid;
      this.TypeMapper[typeof (DateTime?)] = DbType.DateTime;
      this.TypeMapper[typeof (DateTimeOffset?)] = DbType.DateTimeOffset;
      this.TypeMapper[typeof (Enum)] = DbType.String;
    }

    protected DbType GetDbType(PropertyInfo property)
    {
      if (this.TypeMapper.ContainsKey(property.PropertyType))
        return this.TypeMapper[property.PropertyType];
      if (property.PropertyType.BaseType != (Type) null && this.TypeMapper.ContainsKey(property.PropertyType.BaseType))
        return this.TypeMapper[property.PropertyType.BaseType];
      return property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>) && Nullable.GetUnderlyingType(property.PropertyType).BaseType != (Type) null && this.TypeMapper.ContainsKey(Nullable.GetUnderlyingType(property.PropertyType).BaseType) ? this.TypeMapper[Nullable.GetUnderlyingType(property.PropertyType).BaseType] : DbType.Object;
    }
  }
}
