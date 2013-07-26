using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for RolesResult
/// </summary>
[Serializable]
public class RolesResult
{
    public static string Message = string.Empty;

    public RolesResult()
    {
    }

    [DataObjectMethod(DataObjectMethodType.Insert)]
    public void InsertRole(string role)
    {
        try
        {
            Roles.CreateRole(role);
        }
        catch (Exception ex)
        {
            UsersResult.Message = ex.Message;
        }
    }

    [DataObjectMethod(DataObjectMethodType.Delete)]
    public void DeleteRole(string role)
    {
        Roles.DeleteRole(role);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<string> GetRoles()
    {
        List<string> roles = new List<string>();

        string[] items = Roles.GetAllRoles();

        foreach (string item in items)
            roles.Add(item);

        return roles;
    }
}
