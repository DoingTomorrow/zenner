// Decompiled with JetBrains decompiler
// Type: MSS.Business.Errors.ErrorCodes
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections;

#nullable disable
namespace MSS.Business.Errors
{
  public class ErrorCodes
  {
    private static readonly Hashtable ErrorMessage = new Hashtable();
    public const string ERR__ERROR_TEST1 = "MSSError_1";
    public const string ERR__INSERT_ONLY_CHILDREN = "MSSError_2";
    public const string ERR__SYNC_FAILED = "MSSError_3";
    public const string ERR__FAILED_TO_CREATE_CHANNEL = "MSSError_4";
    public const string ERR__FAILED_TO_RETRIEVE_APPLICATION_PARAMETER = "MSSError_5";
    public const string ERR__RETRIEVING_RESPONSE = "MSSError_6";
    public const string ERR__INVALID_RESPONSE_STATUS = "MSSError_7";
    public const string ERR__EXECUTING_METHOD = "MSSError_8";
    public const string ERR__IMPORT_INCORRECT_DATA_FORMAT = "MSSError_9";

    static ErrorCodes()
    {
      ErrorCodes.ErrorMessage[(object) "MSSError_1"] = (object) "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
      ErrorCodes.ErrorMessage[(object) "MSSError_2"] = (object) "MSS_Error_Message_Insert_Only_Children_Problem";
      ErrorCodes.ErrorMessage[(object) "MSSError_3"] = (object) "An Error has Occurred. Please try synchronization later.";
      ErrorCodes.ErrorMessage[(object) "MSSError_4"] = (object) "Failed to create channel";
      ErrorCodes.ErrorMessage[(object) "MSSError_5"] = (object) "The parameter '{0}' could not be found in the database.";
      ErrorCodes.ErrorMessage[(object) "MSSError_6"] = (object) "Error retrieving response. Check inner details for more info.";
      ErrorCodes.ErrorMessage[(object) "MSSError_7"] = (object) "Error retrieving response. Check inner details for more info.";
      ErrorCodes.ErrorMessage[(object) "MSSError_8"] = (object) "Error retrieving response. Check inned details for more info.";
      ErrorCodes.ErrorMessage[(object) "MSSError_9"] = (object) "The file you tried to import has an incorrect data format. Please use another file.";
    }

    public static string GetErrorMessage(string errorCode)
    {
      return ErrorCodes.ErrorMessage[(object) errorCode] as string;
    }

    public static BaseApplicationException GetException(string errorCode)
    {
      return ErrorCodes.GetException(errorCode, (Exception) null);
    }

    public static BaseApplicationException GetException(string errorCode, Exception inner)
    {
      return new BaseApplicationException(errorCode, new object[2]
      {
        (object) ErrorCodes.GetErrorMessage(errorCode),
        (object) inner
      });
    }
  }
}
