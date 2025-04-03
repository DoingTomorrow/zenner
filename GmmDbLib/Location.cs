// Decompiled with JetBrains decompiler
// Type: GmmDbLib.Location
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using GmmDbLib.DataSets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  public static class Location
  {
    public static List<ZENNER.CommonLibrary.Entities.Location> LoadLocation(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT * FROM Location;";
        List<ZENNER.CommonLibrary.Entities.Location> locationList = new List<ZENNER.CommonLibrary.Entities.Location>();
        using (DbDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
            locationList.Add(new ZENNER.CommonLibrary.Entities.Location()
            {
              ID = reader.GetInt32("LocationID"),
              Country = reader.SafeGetString("Country"),
              Region = reader.SafeGetString("Region"),
              City = reader.SafeGetString("City"),
              Zip = reader.SafeGetString("Zip"),
              Street = reader.SafeGetString("Street"),
              Floor = reader.SafeGetString("Floor"),
              HouseNumber = reader.SafeGetString("HouseNumber"),
              RoomNumber = reader.SafeGetString("RoomNumber"),
              Latitude = reader.SafeGetDouble("Latitude"),
              Longitude = reader.SafeGetDouble("Longitude"),
              Description = reader.SafeGetString("Description")
            });
        }
        return locationList;
      }
    }

    public static ZENNER.CommonLibrary.Entities.Location SaveLocation(
      BaseDbConnection db,
      ZENNER.CommonLibrary.Entities.Location location)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (location == null)
        throw new ArgumentNullException(nameof (location));
      return location.ID == 0 ? Location.InsertLocation(db, location) : Location.UpdateLocation(db, location);
    }

    public static ZENNER.CommonLibrary.Entities.Location InsertLocation(
      BaseDbConnection db,
      ZENNER.CommonLibrary.Entities.Location location)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (location == null)
        throw new ArgumentNullException(nameof (location));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        using (DbTransaction tran = newConnection.BeginTransaction())
        {
          ZENNER.CommonLibrary.Entities.Location location1 = Location.InsertLocation(db, newConnection, tran, location);
          if (location1 != null)
            tran.Commit();
          return location1;
        }
      }
    }

    public static ZENNER.CommonLibrary.Entities.Location UpdateLocation(
      BaseDbConnection db,
      ZENNER.CommonLibrary.Entities.Location location)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (location == null)
        throw new ArgumentNullException(nameof (location));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        using (DbTransaction tran = newConnection.BeginTransaction())
        {
          ZENNER.CommonLibrary.Entities.Location location1 = Location.UpdateLocation(db, newConnection, tran, location);
          if (location1 != null)
            tran.Commit();
          return location1;
        }
      }
    }

    public static ZENNER.CommonLibrary.Entities.Location InsertLocation(
      BaseDbConnection db,
      DbConnection conn,
      DbTransaction tran,
      ZENNER.CommonLibrary.Entities.Location location)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (conn == null)
        throw new ArgumentNullException(nameof (conn));
      if (location == null)
        throw new ArgumentNullException(nameof (location));
      location.ID = db.GetNewId(nameof (Location));
      DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM Location;", conn, tran, out DbCommandBuilder _);
      BaseTables.LocationDataTable locationDataTable = new BaseTables.LocationDataTable();
      BaseTables.LocationRow row = locationDataTable.NewLocationRow();
      row.LocationID = location.ID;
      row.Country = location.Country;
      row.Region = location.Region;
      row.City = location.City;
      row.Zip = location.Zip;
      row.Street = location.Street;
      row.Floor = location.Floor;
      row.HouseNumber = location.HouseNumber;
      row.RoomNumber = location.RoomNumber;
      row.Latitude = location.Latitude;
      row.Longitude = location.Longitude;
      row.Description = location.Description;
      locationDataTable.AddLocationRow(row);
      if (dataAdapter.Update((DataTable) locationDataTable) != 1)
        throw new Exception("Can not add new location!");
      return location;
    }

    public static ZENNER.CommonLibrary.Entities.Location UpdateLocation(
      BaseDbConnection db,
      DbConnection conn,
      DbTransaction tran,
      ZENNER.CommonLibrary.Entities.Location location)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (conn == null)
        throw new ArgumentNullException(nameof (conn));
      if (location == null)
        throw new ArgumentNullException(nameof (location));
      if (location.ID <= 0)
        throw new ArgumentException("location.ID");
      DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM Location WHERE LocationID=" + location.ID.ToString() + ";", conn, tran, out DbCommandBuilder _);
      BaseTables.LocationDataTable locationDataTable = new BaseTables.LocationDataTable();
      if (dataAdapter.Fill((DataTable) locationDataTable) != 1)
        throw new Exception("Can not update location! Such location does not exists in current database.");
      BaseTables.LocationRow byLocationId = locationDataTable.FindByLocationID(location.ID);
      if (byLocationId == null)
        throw new Exception("Can not find location ID!");
      byLocationId.Country = location.Country;
      byLocationId.Region = location.Region;
      byLocationId.City = location.City;
      byLocationId.Zip = location.Zip;
      byLocationId.Street = location.Street;
      byLocationId.Floor = location.Floor;
      byLocationId.HouseNumber = location.HouseNumber;
      byLocationId.RoomNumber = location.RoomNumber;
      byLocationId.Latitude = location.Latitude;
      byLocationId.Longitude = location.Longitude;
      byLocationId.Description = location.Description;
      if (dataAdapter.Update((DataTable) locationDataTable) != 1)
        throw new Exception("Can not update location!");
      return location;
    }

    public static bool DeleteLocation(BaseDbConnection db, int locationID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM Location WHERE LocationID=" + locationID.ToString() + ";";
        return command.ExecuteNonQuery() > 0;
      }
    }
  }
}
