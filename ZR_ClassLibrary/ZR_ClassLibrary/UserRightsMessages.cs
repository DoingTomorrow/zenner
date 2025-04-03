// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.UserRightsMessages
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace ZR_ClassLibrary
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class UserRightsMessages
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal UserRightsMessages()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (UserRightsMessages.resourceMan == null)
          UserRightsMessages.resourceMan = new ResourceManager("ZR_ClassLibrary.UserRightsMessages", typeof (UserRightsMessages).Assembly);
        return UserRightsMessages.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => UserRightsMessages.resourceCulture;
      set => UserRightsMessages.resourceCulture = value;
    }

    internal static string Alle_Rechte_freigeben_
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Alle Rechte freigeben?", UserRightsMessages.resourceCulture);
      }
    }

    internal static string Are_you_sure_to_delete_the_old_licence_
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Are you sure to delete the old licence?", UserRightsMessages.resourceCulture);
      }
    }

    internal static string Change_licence
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Change licence", UserRightsMessages.resourceCulture);
      }
    }

    internal static string DemoVersionExhausted
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString(nameof (DemoVersionExhausted), UserRightsMessages.resourceCulture);
      }
    }

    internal static string GMM_user_rights
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("GMM user rights", UserRightsMessages.resourceCulture);
      }
    }

    internal static string Illegal_personal_number_
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Illegal personal number.", UserRightsMessages.resourceCulture);
      }
    }

    internal static string New_user_created
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("New user created", UserRightsMessages.resourceCulture);
      }
    }

    internal static string Not_yet_available
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Not yet available", UserRightsMessages.resourceCulture);
      }
    }

    internal static string Password_error
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Password error", UserRightsMessages.resourceCulture);
      }
    }

    internal static string Personal_number_out_of_range
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Personal number out of range", UserRightsMessages.resourceCulture);
      }
    }

    internal static string Rights_string_error
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Rights string error", UserRightsMessages.resourceCulture);
      }
    }

    internal static string The_minimun_name_size_is_7_characters
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("The minimun name size is 7 characters", UserRightsMessages.resourceCulture);
      }
    }

    internal static string The_minimun_password_size_is_5_characters
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("The minimun password size is 5 characters", UserRightsMessages.resourceCulture);
      }
    }

    internal static string The_passwords_are_not_equal_
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("The passwords are not equal.", UserRightsMessages.resourceCulture);
      }
    }

    internal static string The_program_will_be_terminated
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("The program will be terminated", UserRightsMessages.resourceCulture);
      }
    }

    internal static string User_already_exists
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("User already exists", UserRightsMessages.resourceCulture);
      }
    }

    internal static string User_data_changed
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("User data changed", UserRightsMessages.resourceCulture);
      }
    }

    internal static string User_data_not_changed
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("User data not changed", UserRightsMessages.resourceCulture);
      }
    }

    internal static string User_not_available
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("User not available", UserRightsMessages.resourceCulture);
      }
    }

    internal static string User_not_defined
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("User not defined", UserRightsMessages.resourceCulture);
      }
    }

    internal static string User_not_found
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("User not found", UserRightsMessages.resourceCulture);
      }
    }

    internal static string User_permissions_changed
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("User permissions changed", UserRightsMessages.resourceCulture);
      }
    }

    internal static string User_Rights
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("User Rights", UserRightsMessages.resourceCulture);
      }
    }

    internal static string With_the_next_start_of_the_program_you_have_to_type_in_the_new_licence_code_
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("With the next start of the program you have to type in the new licence code.", UserRightsMessages.resourceCulture);
      }
    }

    internal static string Wrong_package_number
    {
      get
      {
        return UserRightsMessages.ResourceManager.GetString("Wrong package number", UserRightsMessages.resourceCulture);
      }
    }
  }
}
