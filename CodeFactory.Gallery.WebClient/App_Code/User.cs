using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for User
/// </summary>
public class CodeFactoryUser : MembershipUser
{
    private string _username = string.Empty;
    private string _password = string.Empty;
    private bool _isLockedOut = false;

    public CodeFactoryUser()
    {
    }

    public string Password
    {
        get { return _password; }
        set { _password = value; }
    }

    public new string UserName
    {
        get { return _username; }
        set { _username = value; }
    }

    public new bool IsLockedOut
    {
        get { return _isLockedOut; }
        set { _isLockedOut = false; }
    }

    public new DateTime CreationDate 
    {
        get { return base.CreationDate; }
        set { }             
    }

    public new DateTime LastActivityDate 
    {
        get { return base.LastActivityDate; }
        set { }
    }

    public new DateTime LastLoginDate
    {
        get { return base.LastLoginDate; }
        set { } 
    }
}
