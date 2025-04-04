// Decompiled with JetBrains decompiler
// Type: Microsoft.Internal.Web.Utils.ExceptionHelper
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using System;

#nullable disable
namespace Microsoft.Internal.Web.Utils
{
  internal static class ExceptionHelper
  {
    internal static ArgumentException CreateArgumentNullOrEmptyException(string paramName)
    {
      return new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, paramName);
    }
  }
}
