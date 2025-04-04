// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.WebHttpClient`1
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading;

#nullable disable
namespace ExpressionSerialization
{
  public class WebHttpClient<TChannel> where TChannel : IQueryService
  {
    private HashSet<Type> _knownTypes;

    public IEnumerable<Type> knownTypes => this._knownTypes.AsEnumerable<Type>();

    public Uri baseAddress { get; private set; }

    public WebHttpClient(Uri baseAddress, IEnumerable<Type> knownTypes = null)
    {
      this.baseAddress = baseAddress;
      if (knownTypes == null)
        this._knownTypes = new HashSet<Type>(WebHttpClient<TChannel>.GetServiceKnownTypes(typeof (TChannel)));
      else
        this._knownTypes = new HashSet<Type>(WebHttpClient<TChannel>.GetServiceKnownTypes(typeof (TChannel)).Union<Type>(knownTypes));
    }

    public TResult SynchronousCall<TResult>(Expression<Func<TChannel, object>> methodcall)
    {
      return (TResult) this.SynchronousCall(methodcall, typeof (TResult));
    }

    public object SynchronousCall(Expression<Func<TChannel, object>> methodcall, Type returnType)
    {
      object result = (object) null;
      Stream stream = (Stream) null;
      MethodCallExpression body = (MethodCallExpression) methodcall.Body;
      Type type = body.Object.Type;
      bool flag = returnType == body.Method.ReturnType;
      foreach (Type baseType in TypeResolver.GetBaseTypes(returnType))
        this._knownTypes.Add(baseType);
      object parameters = WebHttpClient<TChannel>.GetParameters(body);
      string method;
      WebMessageFormat requestformat;
      WebMessageFormat responseformat;
      Uri operationInfo = WebHttpClient<TChannel>.GetOperationInfo(body.Method, this.baseAddress, out method, out IOperationBehavior _, out OperationContractAttribute _, out requestformat, out responseformat);
      ManualResetEvent reset = new ManualResetEvent(false);
      Action<Stream> action = (Action<Stream>) (s =>
      {
        stream = s;
        result = this.Deserialize(returnType, stream, responseformat);
        reset.Set();
      });
      // ISSUE: reference to a compiler-generated field
      if (WebHttpClient<TChannel>.\u003C\u003Eo__9.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebHttpClient<TChannel>.\u003C\u003Eo__9.\u003C\u003Ep__0 = CallSite<Action<CallSite, WebHttpClient<TChannel>, Uri, object, Action<Stream>, string, WebMessageFormat, WebMessageFormat>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CreateHttpWebRequest", (IEnumerable<Type>) null, typeof (WebHttpClient<TChannel>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[7]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.NamedArgument, "instance"),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.NamedArgument, "callback"),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.NamedArgument, "method"),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.NamedArgument, "requestFormat"),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.NamedArgument, "responseFormat")
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      WebHttpClient<TChannel>.\u003C\u003Eo__9.\u003C\u003Ep__0.Target((CallSite) WebHttpClient<TChannel>.\u003C\u003Eo__9.\u003C\u003Ep__0, this, operationInfo, parameters, action, method, requestformat, responseformat);
      reset.WaitOne();
      return result;
    }

    private void CreateHttpWebRequest(
      Uri absoluteUri,
      object instance,
      Action<Stream> callback,
      string method = "POST",
      WebMessageFormat requestFormat = WebMessageFormat.Xml,
      WebMessageFormat responseFormat = WebMessageFormat.Xml)
    {
      HttpWebRequest request = WebRequest.Create(absoluteUri) as HttpWebRequest;
      request.Method = method;
      HttpWebResponse response;
      AsyncCallback responseCallback = (AsyncCallback) (ar2 =>
      {
        response = (HttpWebResponse) ((WebRequest) ar2.AsyncState).EndGetResponse(ar2);
        callback(response.GetResponseStream());
      });
      if (method == "POST" && instance != null)
      {
        request.ContentType = requestFormat == WebMessageFormat.Json ? "application/json" : "application/xml";
        Stream postStream;
        AsyncCallback callback1 = (AsyncCallback) (ar1 =>
        {
          postStream = request.EndGetRequestStream(ar1);
          this.Serialize(postStream, instance, requestFormat);
          postStream.Close();
          request.BeginGetResponse(responseCallback, (object) request);
        });
        request.BeginGetRequestStream(callback1, (object) request);
      }
      else
      {
        if (!(method == "GET"))
          return;
        request.ContentLength = 0L;
        request.BeginGetResponse(responseCallback, (object) request);
      }
    }

    private long Serialize(Stream stream, object instance, WebMessageFormat requestFormat)
    {
      Type type = instance.GetType();
      this._knownTypes.Add(type);
      object obj;
      switch (requestFormat)
      {
        case WebMessageFormat.Xml:
          obj = (object) new DataContractSerializer(type, (IEnumerable<Type>) this._knownTypes);
          break;
        case WebMessageFormat.Json:
          obj = (object) new DataContractJsonSerializer(type, (IEnumerable<Type>) this._knownTypes);
          break;
        default:
          obj = (object) new DataContractSerializer(type, (IEnumerable<Type>) this._knownTypes);
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (WebHttpClient<TChannel>.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebHttpClient<TChannel>.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, Stream, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteObject", (IEnumerable<Type>) null, typeof (WebHttpClient<TChannel>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      WebHttpClient<TChannel>.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) WebHttpClient<TChannel>.\u003C\u003Eo__11.\u003C\u003Ep__0, obj, stream, instance);
      return 0;
    }

    private object Deserialize(Type type, Stream stream, WebMessageFormat responseformat = WebMessageFormat.Json)
    {
      this._knownTypes.Add(type);
      object obj;
      switch (responseformat)
      {
        case WebMessageFormat.Xml:
          obj = (object) new DataContractSerializer(type, (IEnumerable<Type>) this._knownTypes);
          break;
        case WebMessageFormat.Json:
          obj = (object) new DataContractJsonSerializer(type, (IEnumerable<Type>) this._knownTypes);
          break;
        default:
          obj = (object) new DataContractJsonSerializer(type, (IEnumerable<Type>) this._knownTypes);
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (WebHttpClient<TChannel>.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebHttpClient<TChannel>.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Stream, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ReadObject", (IEnumerable<Type>) null, typeof (WebHttpClient<TChannel>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return WebHttpClient<TChannel>.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) WebHttpClient<TChannel>.\u003C\u003Eo__12.\u003C\u003Ep__0, obj, stream);
    }

    private T Deserialize<T>(Stream stream, WebMessageFormat responseformat = WebMessageFormat.Json)
    {
      return (T) this.Deserialize(typeof (T), stream, responseformat);
    }

    private static object GetParameters(MethodCallExpression m)
    {
      return m.Arguments.Count == 0 ? (object) null : Expression.Lambda(Evaluator.PartialEval(m.Arguments[0])).Compile().DynamicInvoke();
    }

    private static Uri GetOperationInfo(
      MethodInfo operation,
      Uri baseAddress,
      out string method,
      out IOperationBehavior webbehavior,
      out OperationContractAttribute operationcontract,
      out WebMessageFormat requestformat,
      out WebMessageFormat responseformat)
    {
      object[] customAttributes = operation.GetCustomAttributes(false);
      webbehavior = ((IEnumerable<object>) customAttributes).Single<object>((Func<object, bool>) (a => a is WebInvokeAttribute || a is WebGetAttribute)) as IOperationBehavior;
      operationcontract = ((IEnumerable<object>) customAttributes).Single<object>((Func<object, bool>) (a => a is OperationContractAttribute)) as OperationContractAttribute;
      if (webbehavior is WebInvokeAttribute)
      {
        requestformat = ((WebInvokeAttribute) webbehavior).RequestFormat;
        responseformat = ((WebInvokeAttribute) webbehavior).ResponseFormat;
        Uri relativeUri = new Uri(((WebInvokeAttribute) webbehavior).UriTemplate, UriKind.Relative);
        Uri operationInfo = new Uri(baseAddress, relativeUri);
        method = ((WebInvokeAttribute) webbehavior).Method;
        return operationInfo;
      }
      if (!(webbehavior is WebGetAttribute))
        throw new NotSupportedException(webbehavior.GetType().FullName + " is not supported.");
      requestformat = ((WebGetAttribute) webbehavior).RequestFormat;
      responseformat = ((WebGetAttribute) webbehavior).ResponseFormat;
      Uri relativeUri1 = new Uri(((WebGetAttribute) webbehavior).UriTemplate, UriKind.Relative);
      Uri operationInfo1 = new Uri(baseAddress, relativeUri1);
      method = "GET";
      return operationInfo1;
    }

    private static IEnumerable<Type> GetServiceKnownTypes(Type service)
    {
      HashSet<Type> serviceKnownTypes = new HashSet<Type>();
      foreach (ServiceKnownTypeAttribute knownTypeAttribute in service.GetCustomAttributes(true).OfType<ServiceKnownTypeAttribute>())
        serviceKnownTypes.Add(knownTypeAttribute.Type);
      foreach (MemberInfo method in service.GetMethods())
      {
        foreach (ServiceKnownTypeAttribute knownTypeAttribute in method.GetCustomAttributes(true).OfType<ServiceKnownTypeAttribute>())
          serviceKnownTypes.Add(knownTypeAttribute.Type);
      }
      return (IEnumerable<Type>) serviceKnownTypes;
    }
  }
}
