// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.GuidCombGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Id
{
  public class GuidCombGenerator : IIdentifierGenerator
  {
    public object Generate(ISessionImplementor session, object obj) => (object) this.GenerateComb();

    private Guid GenerateComb()
    {
      byte[] byteArray = Guid.NewGuid().ToByteArray();
      DateTime dateTime = new DateTime(1900, 1, 1);
      DateTime now = DateTime.Now;
      TimeSpan timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
      TimeSpan timeOfDay = now.TimeOfDay;
      byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
      byte[] bytes2 = BitConverter.GetBytes((long) (timeOfDay.TotalMilliseconds / 3.333333));
      Array.Reverse((Array) bytes1);
      Array.Reverse((Array) bytes2);
      Array.Copy((Array) bytes1, bytes1.Length - 2, (Array) byteArray, byteArray.Length - 6, 2);
      Array.Copy((Array) bytes2, bytes2.Length - 4, (Array) byteArray, byteArray.Length - 4, 4);
      return new Guid(byteArray);
    }
  }
}
