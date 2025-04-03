// Decompiled with JetBrains decompiler
// Type: GmmDbLib.BaseType
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  [Serializable]
  public sealed class BaseType
  {
    public MeterInfo MeterInfo { get; private set; }

    public MeterType MeterType { get; private set; }

    public MTypeElectronic Data { get; private set; }

    public BaseType DeepCopy() => this.MemberwiseClone() as BaseType;

    public static BaseType GetBaseType(int meterInfoID)
    {
      MeterInfo meterInfo = MeterInfo.GetMeterInfo(meterInfoID);
      MeterType meterType = meterInfo != null ? MeterType.GetMeterType(meterInfo.MeterTypeID) : throw new Exception("Can not find the MeterInfo. MeterInfoID " + meterInfoID.ToString());
      MTypeElectronic mtypeElectronic = MTypeElectronic.GetMTypeElectronic(meterInfo.MeterTypeID);
      if (mtypeElectronic == null && meterType != null && meterInfo.MeterTypeID > 0)
        throw new Exception("Invalid type. MeterInfoID " + meterInfoID.ToString());
      return new BaseType()
      {
        MeterInfo = meterInfo,
        MeterType = meterType,
        Data = mtypeElectronic
      };
    }

    public static int? CreateType(
      string hardwareName,
      string sapNumber,
      int hardwareTypeID,
      int meterHardwareID,
      string description,
      byte[] compressedData,
      string typeCreationString,
      bool isBaseType = true)
    {
      return BaseType.CreateType(DbBasis.PrimaryDB.BaseDbConnection, hardwareName, sapNumber, hardwareTypeID, meterHardwareID, description, compressedData, typeCreationString, isBaseType);
    }

    public static int? CreateType(
      BaseDbConnection db,
      string hardwareName,
      string sapNumber,
      int hardwareTypeID,
      int meterHardwareID,
      string description,
      byte[] compressedData,
      string typeCreationString,
      bool isBaseType = true)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      int newId1;
      int newId2;
      if (isBaseType)
      {
        newId1 = db.GetNewId("MeterInfo_BASE");
        newId2 = db.GetNewId("MeterType_BASE");
      }
      else
      {
        newId1 = db.GetNewId("MeterInfo");
        newId2 = db.GetNewId("MeterType");
      }
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        using (DbTransaction dbTransaction = newConnection.BeginTransaction())
        {
          DbCommand command = newConnection.CreateCommand();
          command.Transaction = dbTransaction;
          MeterInfo.AddMeterInfo(command, newId1, meterHardwareID, newId2, sapNumber, "0", description, hardwareTypeID);
          if (compressedData != null)
          {
            MeterType.AddMeterType(command, newId2, "MTypeElectronic", hardwareName, DateTime.UtcNow, description);
            MTypeElectronic.AddMTypeElectronic(command, newId2, compressedData, typeCreationString);
          }
          dbTransaction.Commit();
          return new int?(newId1);
        }
      }
    }

    public static int? CreateType(
      OpenTransaction openTransaction,
      string hardwareName,
      string sapNumber,
      int hardwareTypeID,
      int meterHardwareID,
      string description,
      byte[] compressedData,
      string typeCreationString,
      bool isBaseType = true)
    {
      if (openTransaction.Database == null)
        throw new ArgumentNullException("Database");
      int newId1;
      int newId2;
      if (isBaseType)
      {
        newId1 = openTransaction.Database.GetNewId("MeterInfo_BASE");
        newId2 = openTransaction.Database.GetNewId("MeterType_BASE");
      }
      else
      {
        newId1 = openTransaction.Database.GetNewId("MeterInfo");
        newId2 = openTransaction.Database.GetNewId("MeterType");
      }
      DbCommand command = openTransaction.Connection.CreateCommand();
      command.Transaction = openTransaction.Transaction;
      MeterInfo.AddMeterInfo(command, newId1, meterHardwareID, newId2, sapNumber, "0", description, hardwareTypeID);
      if (compressedData != null)
      {
        MeterType.AddMeterType(command, newId2, "MTypeElectronic", hardwareName, DateTime.UtcNow, description);
        MTypeElectronic.AddMTypeElectronic(command, newId2, compressedData, typeCreationString);
      }
      return new int?(newId1);
    }

    public static void UpdateType(
      string hardwareName,
      int meterInfoID,
      int? meterTypeID,
      string sapNumber,
      int hardwareTypeID,
      string description,
      byte[] compressedData,
      string typeCreationString)
    {
      BaseType.UpdateType(DbBasis.PrimaryDB.BaseDbConnection, hardwareName, meterInfoID, meterTypeID, sapNumber, hardwareTypeID, description, compressedData, typeCreationString);
    }

    public static void UpdateType(
      BaseDbConnection db,
      string hardwareName,
      int meterInfoID,
      int? meterTypeID,
      string sapNumber,
      int hardwareTypeID,
      string description,
      byte[] compressedData,
      string typeCreationString)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      int? nullable = new int?();
      if (compressedData != null && !meterTypeID.HasValue)
        nullable = new int?(db.GetNewId("MeterType_BASE"));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        using (DbTransaction dbTransaction = newConnection.BeginTransaction())
        {
          DbCommand command = newConnection.CreateCommand();
          command.Transaction = dbTransaction;
          MeterInfo.UpdateMeterInfo(command, meterInfoID, sapNumber, description, hardwareTypeID);
          if (compressedData != null)
          {
            if (meterTypeID.HasValue)
            {
              MeterType.UpdateMeterType(command, meterTypeID.Value, DateTime.UtcNow);
              MTypeElectronic.UpdateMTypeElectronic(command, meterTypeID.Value, compressedData, typeCreationString);
            }
            else
            {
              MeterType.AddMeterType(command, nullable.Value, "MTypeElectronic", hardwareName, DateTime.UtcNow, description);
              MTypeElectronic.AddMTypeElectronic(command, nullable.Value, compressedData, typeCreationString);
            }
          }
          dbTransaction.Commit();
        }
      }
    }

    public static void DeleteType(MeterInfo meterInfo)
    {
      BaseType.DeleteType(DbBasis.PrimaryDB.BaseDbConnection, meterInfo);
    }

    public static void DeleteType(BaseDbConnection db, MeterInfo meterInfo)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (meterInfo == null)
        throw new ArgumentNullException(nameof (meterInfo));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        using (DbTransaction dbTransaction = newConnection.BeginTransaction())
        {
          DbCommand command = newConnection.CreateCommand();
          command.Transaction = dbTransaction;
          MeterInfo.DeleteMeterInfo(command, meterInfo.MeterInfoID);
          if (meterInfo.MeterTypeID > 0)
          {
            MeterType.DeleteMeterType(command, meterInfo.MeterTypeID);
            MTypeElectronic.DeleteMTypeElectronic(command, meterInfo.MeterTypeID);
          }
          dbTransaction.Commit();
        }
      }
    }
  }
}
