// Decompiled with JetBrains decompiler
// Type: GMMToMSSMigrator.MigrationManager
// Assembly: GMMToMSSMigrator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3ACF3C29-B99D-4830-8DFE-AD2278C0306B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMMToMSSMigrator.dll

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.Meters;
using MSS.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

#nullable disable
namespace GMMToMSSMigrator
{
  public class MigrationManager
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly GMMDataProvider _gmmDataProvider;
    private readonly ulong BATCH_SIZE = 100000;
    private readonly int NUMBER_OF_ROWS_PER_INSERT = 1000;

    public MigrationManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._repositoryFactory.GetRepository<Meter>();
      this._gmmDataProvider = new GMMDataProvider(CustomerConfiguration.GetPropertyValue("GMMMigrationDatabasePath"), repositoryFactory);
    }

    public void ValidateStructureMigration(
      out List<StructureNodeDTO> validatedStructures,
      out string validationMessages)
    {
      List<StructureNodeDTO> structureNodeDtos;
      List<GMMRowValidationResult> structuresAndMeters = this._gmmDataProvider.GetStructuresAndMeters(out structureNodeDtos);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (GMMRowValidationResult validationResult in structuresAndMeters)
      {
        if (!validationResult.IsSuccess)
        {
          if (!string.IsNullOrEmpty(stringBuilder.ToString()))
            stringBuilder.Append(Environment.NewLine + Environment.NewLine);
          stringBuilder.Append(validationResult.Message);
        }
      }
      validatedStructures = structureNodeDtos;
      validationMessages = stringBuilder.ToString();
    }

    public void MigrateStructuresAndMeters(List<StructureNodeDTO> structureNodes)
    {
      Mapper.CreateMap<MeterDTO, Meter>();
      ISession session = this._repositoryFactory.GetSession();
      try
      {
        StructuresManager structuresManager = new StructuresManager(this._repositoryFactory);
        structureNodes = this.RearrangeStructureNodesForSaving(structureNodes);
        structuresManager.TransactionalSaveNewPhysicalStructure((IList<StructureNodeDTO>) structureNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item =>
        {
          StructureTypeEnum? structureType = item.StructureType;
          StructureTypeEnum structureTypeEnum = StructureTypeEnum.Physical;
          return structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue;
        })).ToList<StructureNodeDTO>(), (StructureNodeEquipmentSettings) null);
        structuresManager.TransactionalSaveNewLogicalStructure((IList<StructureNodeDTO>) structureNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item =>
        {
          StructureTypeEnum? structureType = item.StructureType;
          StructureTypeEnum structureTypeEnum = StructureTypeEnum.Logical;
          return structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue;
        })).ToList<StructureNodeDTO>());
      }
      catch (Exception ex)
      {
        session.Transaction.Rollback();
        MessageHandler.LogException(ex);
        throw;
      }
    }

    public void MigrateReadingValues()
    {
      this._gmmDataProvider.SetMssGuidsForImportedMeters();
      ulong? numberOfReadingValues = this._gmmDataProvider.GetNumberOfReadingValues();
      int num1;
      if (numberOfReadingValues.HasValue)
      {
        ulong? nullable = numberOfReadingValues;
        ulong num2 = 0;
        num1 = nullable.GetValueOrDefault() > num2 ? (nullable.HasValue ? 1 : 0) : 0;
      }
      else
        num1 = 0;
      if (num1 == 0)
        return;
      ulong num3 = numberOfReadingValues.Value / this.BATCH_SIZE;
      if (numberOfReadingValues.Value % this.BATCH_SIZE > 0UL)
        ++num3;
      string connectionString = this._repositoryFactory.GetSession().Connection.ConnectionString;
      SQLiteTransaction sqLiteTransaction = (SQLiteTransaction) null;
      try
      {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
          connection.Open();
          using (SQLiteCommand sqLiteCommand = new SQLiteCommand(connection))
          {
            sqLiteTransaction = connection.BeginTransaction();
            for (ulong index1 = 0; index1 < num3; ++index1)
            {
              List<MeterReadingValue> readingValuesBatch = this._gmmDataProvider.GetReadingValuesBatch(index1 * this.BATCH_SIZE, this.BATCH_SIZE);
              int num4 = readingValuesBatch.Count / this.NUMBER_OF_ROWS_PER_INSERT;
              if (readingValuesBatch.Count % this.NUMBER_OF_ROWS_PER_INSERT > 0)
                ++num4;
              for (int index2 = 0; index2 < num4; ++index2)
              {
                int num5 = index2 < num4 - 1 ? this.NUMBER_OF_ROWS_PER_INSERT : readingValuesBatch.Count - (num4 - 1) * this.NUMBER_OF_ROWS_PER_INSERT;
                StringBuilder stringBuilder1 = new StringBuilder("INSERT INTO t_ReadingValues(Id, MeterId, MeterSerialNumber, Date, Value, ValueId, CreatedOn, ExportedOn, MDMExportedOn, UnitId, PhysicalQuantity, MeterType, CalculationStart, Creation, Calculation, StorageInterval) VALUES ");
                for (int index3 = 0; index3 < num5; ++index3)
                {
                  MeterReadingValue meterReadingValue = readingValuesBatch.ElementAt<MeterReadingValue>(index2 * this.NUMBER_OF_ROWS_PER_INSERT + index3);
                  stringBuilder1.Append("('" + Guid.NewGuid().ToString().ToLower() + "', ");
                  stringBuilder1.Append("'" + (object) meterReadingValue.MeterId + "', ");
                  stringBuilder1.Append("'" + meterReadingValue.MeterSerialNumber + "', ");
                  stringBuilder1.Append("'" + meterReadingValue.Date.ToString("yyyy-MM-dd HH:mm:ss") + "', ");
                  stringBuilder1.Append(meterReadingValue.Value.ToString() + ", ");
                  stringBuilder1.Append(meterReadingValue.ValueId.ToString() + ", ");
                  stringBuilder1.Append("'" + meterReadingValue.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss") + "', ");
                  DateTime? nullable = meterReadingValue.ExportedOn;
                  if (nullable.HasValue)
                  {
                    StringBuilder stringBuilder2 = stringBuilder1;
                    nullable = meterReadingValue.ExportedOn;
                    string str = "'" + (object) nullable.Value + "', ";
                    stringBuilder2.Append(str);
                  }
                  else
                    stringBuilder1.Append("null, ");
                  nullable = meterReadingValue.MDMExportedOn;
                  if (nullable.HasValue)
                  {
                    StringBuilder stringBuilder3 = stringBuilder1;
                    nullable = meterReadingValue.MDMExportedOn;
                    string str = "'" + (object) nullable.Value + "', ";
                    stringBuilder3.Append(str);
                  }
                  else
                    stringBuilder1.Append("null, ");
                  if (meterReadingValue.Unit != null)
                    stringBuilder1.Append("'" + (object) meterReadingValue.Unit.Id + "', ");
                  else
                    stringBuilder1.Append("null, ");
                  stringBuilder1.Append(meterReadingValue.PhysicalQuantity.ToString() + ", ");
                  stringBuilder1.Append(meterReadingValue.MeterType.ToString() + ", ");
                  stringBuilder1.Append(meterReadingValue.CalculationStart.ToString() + ", ");
                  stringBuilder1.Append(meterReadingValue.Creation.ToString() + ", ");
                  stringBuilder1.Append(meterReadingValue.Calculation.ToString() + ", ");
                  stringBuilder1.Append(meterReadingValue.StorageInterval);
                  stringBuilder1.Append(")");
                  if (index3 < num5 - 1)
                    stringBuilder1.Append(", ");
                  else
                    stringBuilder1.Append(";");
                }
                sqLiteCommand.CommandText = stringBuilder1.ToString();
                sqLiteCommand.ExecuteNonQuery();
              }
              sqLiteTransaction.Commit();
              sqLiteTransaction = connection.BeginTransaction();
            }
            sqLiteTransaction.Commit();
          }
        }
      }
      catch (Exception ex)
      {
        sqLiteTransaction?.Rollback();
        MessageHandler.LogException(ex);
        throw;
      }
    }

    private List<StructureNodeDTO> RearrangeStructureNodesForSaving(
      List<StructureNodeDTO> structureNodes)
    {
      List<StructureNodeDTO> orderedStructureNodes = structureNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.ParentNode == null)).ToList<StructureNodeDTO>();
      int num1 = 0;
      List<StructureNodeDTO> list;
      do
      {
        list = structureNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => !orderedStructureNodes.Contains(item))).ToList<StructureNodeDTO>();
        int num2 = orderedStructureNodes.Count<StructureNodeDTO>();
        if (list.Count > 0)
        {
          for (int index = num1; index < num2; ++index)
          {
            foreach (StructureNodeDTO structureNodeDto in list)
            {
              if (structureNodeDto.ParentNode == orderedStructureNodes[index])
                orderedStructureNodes.Add(structureNodeDto);
            }
          }
        }
        num1 = num2;
      }
      while (list.Count > 0);
      return orderedStructureNodes;
    }
  }
}
