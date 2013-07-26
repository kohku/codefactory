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
/// Summary description for UsersResult
/// </summary>
[Serializable]
public class UsersResult
{
    private int _totalCount;
    public static string Message = string.Empty;

    public UsersResult()
    {
    }

    [DataObjectMethod(DataObjectMethodType.Insert)]
    public void InsertUser(CodeFactoryUser user)
    {
        try
        {
            MembershipUser item = Membership.CreateUser(user.UserName, user.Password);
            item.Email = user.Email;
            item.Comment = user.Comment;
            Membership.UpdateUser(item);
        }
        catch (MembershipCreateUserException ex)
        {
            UsersResult.Message = ex.Message;
        }
    }

    [DataObjectMethod(DataObjectMethodType.Update)]
    public void UpdateUser(CodeFactoryUser user)
    {
        MembershipUser item = Membership.GetUser(user.UserName);
        item.Comment = user.Comment;
        item.Email = user.Email;

        Membership.UpdateUser(item);
        if (!user.IsLockedOut)
            item.UnlockUser();
    }

    [DataObjectMethod(DataObjectMethodType.Delete)]
    public void DeleteUser(CodeFactoryUser user)
    {
        Membership.DeleteUser(user.UserName);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public MembershipUserCollection GetUsers()
    {
        return GetUsers(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public MembershipUserCollection GetUsers(int maximumRows, int startRowIndex)
    {
        return Membership.GetAllUsers((int)Math.Floor((double)startRowIndex/maximumRows), maximumRows, out _totalCount);
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return _totalCount;
    }
}
