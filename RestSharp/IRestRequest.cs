// Decompiled with JetBrains decompiler
// Type: RestSharp.IRestRequest
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace RestSharp
{
  public interface IRestRequest
  {
    ISerializer JsonSerializer { get; set; }

    ISerializer XmlSerializer { get; set; }

    List<Parameter> Parameters { get; }

    List<FileParameter> Files { get; }

    Method Method { get; set; }

    string Resource { get; set; }

    DataFormat RequestFormat { get; set; }

    string RootElement { get; set; }

    string DateFormat { get; set; }

    string XmlNamespace { get; set; }

    ICredentials Credentials { get; set; }

    int Timeout { get; set; }

    int Attempts { get; }

    IRestRequest AddFile(string name, string path);

    IRestRequest AddFile(string name, byte[] bytes, string fileName);

    IRestRequest AddFile(string name, byte[] bytes, string fileName, string contentType);

    IRestRequest AddBody(object obj, string xmlNamespace);

    IRestRequest AddBody(object obj);

    IRestRequest AddObject(object obj, params string[] whitelist);

    IRestRequest AddObject(object obj);

    IRestRequest AddParameter(Parameter p);

    IRestRequest AddParameter(string name, object value);

    IRestRequest AddParameter(string name, object value, ParameterType type);

    IRestRequest AddHeader(string name, string value);

    IRestRequest AddCookie(string name, string value);

    IRestRequest AddUrlSegment(string name, string value);

    Action<IRestResponse> OnBeforeDeserialization { get; set; }

    void IncreaseNumAttempts();
  }
}
