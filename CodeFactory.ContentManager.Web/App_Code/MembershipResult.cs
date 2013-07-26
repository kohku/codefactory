using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using System.Web.Security;

/// <summary>
/// Summary description for MembershipResult
/// </summary>
[DataObject]
public class MembershipResult
{
    private string _usernameToMatch;
    private string _emailToMatch;
    private string _rolenameToMatch;

    public MembershipResult(string username, string email)
    {
        _usernameToMatch = username;
        _emailToMatch = email;
    }

    public MembershipResult(string username, string email, string rolename) 
        : this(username, email)
    {
        _rolenameToMatch = rolename;
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public MembershipUserCollection GetMembers()
    {
        return GetMembers(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public MembershipUserCollection GetMembers(int maximumRows, int startRowIndex)
    {
        int totalRecords = 0;

        MembershipUserCollection users = null;

        if (!string.IsNullOrEmpty(_rolenameToMatch))
        {
            users = new MembershipUserCollection();

            foreach (string usr in Roles.GetUsersInRole(_rolenameToMatch))
                users.Add(Membership.GetUser(usr));

            totalRecords = users.Count;
        }
        else if (!string.IsNullOrEmpty(_usernameToMatch))
            users = Membership.FindUsersByName(_usernameToMatch, maximumRows > 0 ? startRowIndex / maximumRows : 0, maximumRows > 0 ? maximumRows : int.MaxValue, out totalRecords);
        else if (!string.IsNullOrEmpty(_emailToMatch))
            users = Membership.FindUsersByEmail(_emailToMatch, maximumRows > 0 ? startRowIndex / maximumRows : 0, maximumRows > 0 ? maximumRows : int.MaxValue, out totalRecords);
        else
            users = Membership.GetAllUsers(maximumRows > 0 ? startRowIndex / maximumRows : 0, maximumRows, out totalRecords);

        HttpContext.Current.Items["MembershipUsers_TotalCount"] = totalRecords;

        return users;
    }

    public int TotalCount()
    {
        return TotalCount(int.MaxValue, 0);
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["MembershipUsers_TotalCount"];
    }

    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public void DeleteUser(string username, string original_username)
    {
        Membership.DeleteUser(original_username);
    }
}
