// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.CommonExtensions
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

#nullable disable
namespace MSS.Utils.Utils
{
  public static class CommonExtensions
  {
    public static bool CheckForInternetConnection()
    {
      try
      {
        using (WebClient webClient = new WebClient())
        {
          using (webClient.OpenRead("http://www.google.com"))
            return true;
        }
      }
      catch
      {
        return false;
      }
    }

    public static string SerializeAnObject(object item)
    {
      if (item == null)
        return (string) null;
      StringBuilder sb = new StringBuilder();
      new XmlSerializer(item.GetType()).Serialize((TextWriter) new StringWriter(sb), item);
      return sb.ToString();
    }

    public static string ReplaceIfDifferent(this string source, string newValue)
    {
      return string.Equals(source, newValue) ? source : source.Replace(source, newValue);
    }

    public static void IfNotNull<T>(this T @this, Action<T> action) where T : class
    {
      if ((object) @this == null)
        return;
      action(@this);
    }

    public static void IfNull<T>(this T @this, Action<T> action, Action<T> elseAction = null) where T : class
    {
      if ((object) @this == null)
        action(@this);
      else if (elseAction != null)
        elseAction(@this);
    }

    public static void IsTrue(this bool source, Action action1, Action action2 = null)
    {
      if (source)
        action1();
      else if (action2 != null)
        action2();
    }

    public static TResult IfNotNull<T, TResult>(
      this T @this,
      Func<T, TResult> getter,
      TResult defaultValue = null)
      where T : class
    {
      return (object) @this != null ? getter(@this) : defaultValue;
    }

    public static TResult IfNotNullOrEmpty<T, TResult>(
      this IEnumerable<T> @this,
      Func<IEnumerable<T>, TResult> getter,
      TResult defaultValue = null)
      where T : class
    {
      if (!(@this is T[] objArray))
        objArray = @this.ToArray<T>();
      T[] source = objArray;
      if (@this != null && ((IEnumerable<T>) source).Any<T>())
      {
        TResult result = getter((IEnumerable<T>) source);
      }
      return defaultValue;
    }

    public static void IfIs<T>(this object @this, Action<T> action)
    {
      if (!(@this is T obj))
        return;
      action(obj);
    }

    public static TResult IfIs<T, TResult>(
      this object @this,
      Func<T, TResult> getter,
      TResult defaultValue = null)
    {
      return @this is T obj ? getter(obj) : defaultValue;
    }

    public static int FindIndex<T>(this IEnumerable<T> list, Func<T, bool> finder)
    {
      int index = 0;
      foreach (T obj in list)
      {
        if (finder(obj))
          return index;
        ++index;
      }
      return -1;
    }

    public static TType SafeCast<TType>([ValidatedNotNull] this object @this, string message = "The expected type was {0}") where TType : class
    {
      return @this is TType type ? type : throw new UnexpectedTypeException(message.FormatStr((object) typeof (TType).Name), @this, typeof (TType));
    }

    public static string FormatStr(this string @this, params object[] args)
    {
      return string.Format(@this, args);
    }

    public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
    {
      collection.IfNull<Dictionary<T, S>>((Action<Dictionary<T, S>>) (_ =>
      {
        throw new ArgumentNullException(nameof (_), "Collection is null");
      }));
      collection.ForEach<KeyValuePair<T, S>>((Action<KeyValuePair<T, S>>) (_ =>
      {
        if (source.ContainsKey(_.Key))
          return;
        source.Add(_.Key, _.Value);
      }));
    }

    private static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (T obj in source)
        action(obj);
    }

    public static T If<T>(
      this T @this,
      Func<T, bool> isTrue,
      Action<T> thenAction,
      Action<T> elseAction = null)
    {
      if (isTrue(@this))
        thenAction(@this);
      else if (elseAction != null)
        elseAction(@this);
      return @this;
    }

    public static TResult If<T, TResult>(
      this T @this,
      Func<T, bool> isTrue,
      Func<T, TResult> thenAction,
      Func<T, TResult> elseAction = null,
      TResult defaultValue = null)
    {
      if (isTrue(@this))
        return thenAction(@this);
      return elseAction != null ? elseAction(@this) : defaultValue;
    }

    public static bool IsTrue(this bool value) => value;

    public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
    {
      if (depObj != null)
      {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); ++i)
        {
          DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
          if (child != null && child is T)
            yield return (T) child;
          foreach (T visualChild in child.FindVisualChildren<T>())
          {
            T childOfChild = visualChild;
            yield return childOfChild;
            childOfChild = default (T);
          }
          child = (DependencyObject) null;
        }
      }
    }

    public static bool IsAny<T>(this IEnumerable<T> data) => data != null && data.Any<T>();

    public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (T obj in source)
      {
        T e = obj;
        action(e);
        yield return e;
        e = default (T);
      }
    }

    public static IEnumerable<Tuple<T, T, T>> AsPairsSafe<T>(this List<T> list)
    {
      int index = 0;
      while (index < ((IEnumerable<T>) list).Count<T>() && index + 1 < ((IEnumerable<T>) list).Count<T>())
        yield return new Tuple<T, T, T>(list[index++], list[index++], list[index++]);
    }

    public static IDictionary<string, string> SplitAndGroup(this string sourceStr, char separator)
    {
      return (IDictionary<string, string>) ((IEnumerable<string>) sourceStr.Split(separator)).Zip(((IEnumerable<string>) sourceStr.Split(separator)).Skip<string>(1), (Key, Value) => new
      {
        Key = Key,
        Value = Value
      }).Where((pair, index) => index % 2 == 0).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public static byte[] ToByteArray(this ImageSource imageSource, BitmapEncoder encoder)
    {
      byte[] byteArray = (byte[]) null;
      if (imageSource is BitmapSource source)
      {
        encoder.Frames.Add(BitmapFrame.Create(source));
        using (MemoryStream memoryStream = new MemoryStream())
        {
          encoder.Save((Stream) memoryStream);
          byteArray = memoryStream.ToArray();
        }
      }
      return byteArray;
    }

    public static BitmapImage ToImageSource(this byte[] imageData)
    {
      if (imageData == null || imageData.Length == 0)
        return (BitmapImage) null;
      BitmapImage imageSource = new BitmapImage();
      using (MemoryStream memoryStream = new MemoryStream(imageData))
      {
        memoryStream.Position = 0L;
        imageSource.BeginInit();
        imageSource.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
        imageSource.CacheOption = BitmapCacheOption.OnLoad;
        imageSource.UriSource = (Uri) null;
        imageSource.StreamSource = (Stream) memoryStream;
        imageSource.EndInit();
      }
      imageSource.Freeze();
      return imageSource;
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
      this IEnumerable<TSource> source,
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

    public static T Cast<T>(this object source, bool convertEnumToString = false)
    {
      if (source == null)
        return default (T);
      Type type1 = source.GetType();
      Type type2 = typeof (T);
      object instance = Activator.CreateInstance(type2, false);
      IEnumerable<MemberInfo> source1 = ((IEnumerable<MemberInfo>) type1.GetMembers()).ToList<MemberInfo>().Where<MemberInfo>((Func<MemberInfo, bool>) (src => src.MemberType == MemberTypes.Property));
      IEnumerable<MemberInfo> targetProperties = ((IEnumerable<MemberInfo>) type2.GetMembers()).ToList<MemberInfo>().Where<MemberInfo>((Func<MemberInfo, bool>) (src => src.MemberType == MemberTypes.Property));
      foreach (MemberInfo memberInfo in source1.Where<MemberInfo>((Func<MemberInfo, bool>) (memberInfo => targetProperties.Select<MemberInfo, string>((Func<MemberInfo, string>) (c => c.Name)).ToList<string>().Contains(memberInfo.Name))).ToList<MemberInfo>())
      {
        PropertyInfo property1 = typeof (T).GetProperty(memberInfo.Name);
        PropertyInfo property2 = source.GetType().GetProperty(memberInfo.Name);
        if (property2 != (PropertyInfo) null && property1 != (PropertyInfo) null)
        {
          object obj = property2.GetValue(source, (object[]) null);
          Type underlyingType = Nullable.GetUnderlyingType(property2.PropertyType);
          if (convertEnumToString && (property2.PropertyType.IsEnum || underlyingType != (Type) null && underlyingType.IsEnum))
            property1.SetValue(instance, (object) (obj?.ToString() ?? string.Empty), (object[]) null);
          else
            property1.SetValue(instance, obj, (object[]) null);
        }
      }
      return (T) instance;
    }
  }
}
