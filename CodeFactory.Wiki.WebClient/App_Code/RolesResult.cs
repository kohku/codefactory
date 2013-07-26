using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using System.Web.Security;

/// <summary>
/// Summary description for RolesResult
/// </summary>
[DataObject]
public class RolesResult
{
    public RolesResult()
    {
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public string[] GetRoles()
    {
        return Roles.GetAllRoles();
    }

    [DataObjectMethod(DataObjectMethodType.Delete)]
    public bool DeleteRole(string rolename)
    {
        if (string.IsNullOrEmpty(rolename) || !Roles.RoleExists(rolename))
            return false;

        return Roles.DeleteRole(rolename);
    }
}
