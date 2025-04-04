// Decompiled with JetBrains decompiler
// Type: THL_Handler.Reading
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace THL_Handler
{
  public sealed class Reading
  {
    private const int MEASUREMENT_INTERVAL_SECS = 180;
    private const int AVERAGING_INTERVAL_SECS = 900;
    public const int AVERAGING_SAMPLES = 5;
    public const int DAILY_SAMPLES = 96;
    public Reading.SHT2x_Result_t[] measurement_samples = new Reading.SHT2x_Result_t[5];
    public Reading.SHT2x_Result_t[] daily_samples = new Reading.SHT2x_Result_t[96];
    public Reading.SHT2x_Result_t daily_average = new Reading.SHT2x_Result_t();
    public byte daily_sample_count;
    public byte quarter_hour_sample_count;
    public ushort pagging;
    public int measurement_countdown;
    public Reading.daily_bucket_t dailyBucket;
    public Reading.SHT2x_Result_t current;
    public Reading.reading_logger_t logger = new Reading.reading_logger_t();

    public static Reading Parse(byte[] buffer, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      Reading reading = new Reading();
      int index1 = offset;
      for (int index2 = 0; index2 < reading.measurement_samples.Length; ++index2)
      {
        reading.measurement_samples[index2].SHT2x_RH = Reading.GetValue<int>(buffer, ref index1);
        reading.measurement_samples[index2].SHT2x_T = Reading.GetValue<int>(buffer, ref index1);
        reading.measurement_samples[index2].SHT2x_RH_Valid = Reading.GetValue<bool>(buffer, ref index1);
        reading.measurement_samples[index2].SHT2x_T_Valid = Reading.GetValue<bool>(buffer, ref index1);
        reading.measurement_samples[index2].pagging = Reading.GetValue<ushort>(buffer, ref index1);
      }
      for (int index3 = 0; index3 < reading.daily_samples.Length; ++index3)
      {
        reading.daily_samples[index3].SHT2x_RH = Reading.GetValue<int>(buffer, ref index1);
        reading.daily_samples[index3].SHT2x_T = Reading.GetValue<int>(buffer, ref index1);
        reading.daily_samples[index3].SHT2x_RH_Valid = Reading.GetValue<bool>(buffer, ref index1);
        reading.daily_samples[index3].SHT2x_T_Valid = Reading.GetValue<bool>(buffer, ref index1);
        reading.daily_samples[index3].pagging = Reading.GetValue<ushort>(buffer, ref index1);
      }
      reading.daily_average.SHT2x_RH = Reading.GetValue<int>(buffer, ref index1);
      reading.daily_average.SHT2x_T = Reading.GetValue<int>(buffer, ref index1);
      reading.daily_average.SHT2x_RH_Valid = Reading.GetValue<bool>(buffer, ref index1);
      reading.daily_average.SHT2x_T_Valid = Reading.GetValue<bool>(buffer, ref index1);
      reading.daily_average.pagging = Reading.GetValue<ushort>(buffer, ref index1);
      reading.daily_sample_count = Reading.GetValue<byte>(buffer, ref index1);
      reading.quarter_hour_sample_count = Reading.GetValue<byte>(buffer, ref index1);
      reading.pagging = Reading.GetValue<ushort>(buffer, ref index1);
      reading.measurement_countdown = Reading.GetValue<int>(buffer, ref index1);
      reading.dailyBucket.temperatureDistribution.bucket_1 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.temperatureDistribution.bucket_2 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.temperatureDistribution.bucket_3 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.temperatureDistribution.bucket_4 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.temperatureDistribution.bucket_5 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.temperatureDistribution.alarm_bits = Reading.GetValue<Reading.BucketAlarm>(buffer, ref index1);
      reading.dailyBucket.humidityDistribution.bucket_1 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.humidityDistribution.bucket_2 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.humidityDistribution.bucket_3 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.humidityDistribution.bucket_4 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.humidityDistribution.bucket_5 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.humidityDistribution.alarm_bits = Reading.GetValue<Reading.BucketAlarm>(buffer, ref index1);
      reading.dailyBucket.count = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.pagging1 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.pagging2 = Reading.GetValue<byte>(buffer, ref index1);
      reading.dailyBucket.pagging3 = Reading.GetValue<byte>(buffer, ref index1);
      reading.current.SHT2x_RH = Reading.GetValue<int>(buffer, ref index1);
      reading.current.SHT2x_T = Reading.GetValue<int>(buffer, ref index1);
      reading.current.SHT2x_RH_Valid = Reading.GetValue<bool>(buffer, ref index1);
      reading.current.SHT2x_T_Valid = Reading.GetValue<bool>(buffer, ref index1);
      reading.current.pagging = Reading.GetValue<ushort>(buffer, ref index1);
      for (int index4 = 0; index4 < reading.logger.daily.Length; ++index4)
      {
        reading.logger.daily[index4].reading.avg_temperature = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.daily[index4].reading.avg_humidity = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.daily[index4].reading.distribution_temperature = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.daily[index4].reading.distribution_humidity = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.daily[index4].datestamp = Reading.GetValue<ushort>(buffer, ref index1);
        reading.logger.daily[index4].pagging = Reading.GetValue<ushort>(buffer, ref index1);
      }
      for (int index5 = 0; index5 < reading.logger.monthly.Length; ++index5)
      {
        reading.logger.monthly[index5].reading.avg_temperature = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.monthly[index5].reading.avg_humidity = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.monthly[index5].reading.distribution_temperature = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.monthly[index5].reading.distribution_humidity = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.monthly[index5].datestamp = Reading.GetValue<ushort>(buffer, ref index1);
        reading.logger.monthly[index5].pagging = Reading.GetValue<ushort>(buffer, ref index1);
      }
      for (int index6 = 0; index6 < reading.logger.half_monthly.Length; ++index6)
      {
        reading.logger.half_monthly[index6].reading.avg_temperature = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.half_monthly[index6].reading.avg_humidity = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.half_monthly[index6].reading.distribution_temperature = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.half_monthly[index6].reading.distribution_humidity = Reading.GetValue<uint>(buffer, ref index1);
        reading.logger.half_monthly[index6].datestamp = Reading.GetValue<ushort>(buffer, ref index1);
        reading.logger.half_monthly[index6].pagging = Reading.GetValue<ushort>(buffer, ref index1);
      }
      reading.logger.last_daily = Reading.GetValue<byte>(buffer, ref index1);
      reading.logger.last_monthly = Reading.GetValue<byte>(buffer, ref index1);
      reading.logger.last_half_monthly = Reading.GetValue<byte>(buffer, ref index1);
      reading.logger.pagging1 = Reading.GetValue<byte>(buffer, ref index1);
      reading.logger.crc = Reading.GetValue<ushort>(buffer, ref index1);
      reading.logger.pagging2 = Reading.GetValue<ushort>(buffer, ref index1);
      return reading;
    }

    private static T GetValue<T>(byte[] buffer, ref int index)
    {
      if (typeof (T) == typeof (int))
      {
        int int32 = BitConverter.ToInt32(buffer, index);
        index += 4;
        return (T) (System.ValueType) int32;
      }
      if (typeof (T) == typeof (uint))
      {
        uint uint32 = BitConverter.ToUInt32(buffer, index);
        index += 4;
        return (T) (System.ValueType) uint32;
      }
      if (typeof (T) == typeof (ushort))
      {
        ushort uint16 = BitConverter.ToUInt16(buffer, index);
        index += 2;
        return (T) (System.ValueType) uint16;
      }
      if (typeof (T) == typeof (bool))
        return (T) (System.ValueType) BitConverter.ToBoolean(buffer, index++);
      if (typeof (T) == typeof (byte))
        return (T) (System.ValueType) buffer[index++];
      if (typeof (T) == typeof (Reading.BucketAlarm))
        return (T) (System.ValueType) buffer[index++];
      throw new NotImplementedException(typeof (T).ToString());
    }

    public struct SHT2x_Result_t
    {
      public int SHT2x_RH;
      public int SHT2x_T;
      public bool SHT2x_RH_Valid;
      public bool SHT2x_T_Valid;
      public ushort pagging;

      public double H => (double) this.SHT2x_RH / 10.0;

      public double T => (double) this.SHT2x_T / 10.0;

      public override string ToString()
      {
        return string.Format("T:{0}   H:{1}", this.SHT2x_T_Valid ? (object) this.T.ToString() : (object) "-", this.SHT2x_RH_Valid ? (object) this.H.ToString() : (object) "-");
      }
    }

    public struct daily_bucket_t
    {
      public Reading.bucket_t temperatureDistribution;
      public Reading.bucket_t humidityDistribution;
      public byte count;
      public byte pagging1;
      public byte pagging2;
      public byte pagging3;

      public override string ToString()
      {
        return string.Format("Count:{0} TD:{1} HD:{2}", (object) this.count, (object) this.temperatureDistribution, (object) this.humidityDistribution);
      }
    }

    [Flags]
    public enum BucketAlarm : byte
    {
      TEMP_BELOW_OR_EQUAL_ZERO = 1,
      BUCKET_INCOMPLETE = 8,
    }

    public struct bucket_t
    {
      public byte bucket_1;
      public byte bucket_2;
      public byte bucket_3;
      public byte bucket_4;
      public byte bucket_5;
      public Reading.BucketAlarm alarm_bits;

      public override string ToString()
      {
        return string.Format("{0} {1} {2} {3} {4} {5}", (object) this.bucket_1, (object) this.bucket_2, (object) this.bucket_3, (object) this.bucket_4, (object) this.bucket_5, (object) this.alarm_bits);
      }
    }

    public sealed class reading_logger_t
    {
      public Reading.reading_log_t[] daily = new Reading.reading_log_t[32];
      public Reading.reading_log_t[] monthly = new Reading.reading_log_t[18];
      public Reading.reading_log_t[] half_monthly = new Reading.reading_log_t[18];
      public byte last_daily;
      public byte last_monthly;
      public byte last_half_monthly;
      public byte pagging1;
      public ushort crc;
      public ushort pagging2;
    }

    public struct reading_log_t
    {
      public Reading.reading_log_t.readings_t reading;
      public ushort datestamp;
      public ushort pagging;

      public DateTime? Date
      {
        get => Util.ConvertToDate_MBus_CP16_TypeG(BitConverter.GetBytes(this.datestamp), 0);
      }

      public override string ToString()
      {
        return string.Format("{0} {1}", this.Date.HasValue ? (object) this.Date.Value.ToString("d") : (object) string.Empty, (object) this.reading);
      }

      public struct readings_t
      {
        public uint avg_temperature;
        public uint avg_humidity;
        public uint distribution_temperature;
        public uint distribution_humidity;

        public double? AT
        {
          get
          {
            return this.avg_temperature == uint.MaxValue ? new double?() : new double?((double) Util.ConvertBcdUInt32ToUInt32(this.avg_temperature) / 10.0);
          }
        }

        public double? AH
        {
          get
          {
            return this.avg_humidity == uint.MaxValue ? new double?() : new double?((double) Util.ConvertBcdUInt32ToUInt32(this.avg_humidity) / 10.0);
          }
        }

        public uint? DT
        {
          get
          {
            return this.distribution_temperature == uint.MaxValue ? new uint?() : new uint?(this.distribution_temperature);
          }
        }

        public uint? DH
        {
          get
          {
            return this.distribution_humidity == uint.MaxValue ? new uint?() : new uint?(this.distribution_humidity);
          }
        }

        public override string ToString()
        {
          return string.Format("AT:{0,-5} AH:{1,-5} DT:{2,-5} DH:{3,-5}", (object) this.Get(this.AT), (object) this.Get(this.AH), (object) this.Get(this.DT), (object) this.Get(this.DH));
        }

        private string Get(uint? value) => !value.HasValue ? "-" : value.Value.ToString("X8") + "h";

        private string Get(double? value) => !value.HasValue ? "-" : value.ToString();
      }
    }
  }
}
