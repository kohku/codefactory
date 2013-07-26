using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using System.Web.Security;

/// <summary>
/// Summary description for AuthorizerMembersResults
/// </summary>
[DataObject]
public class MemberAuthorizersResults
{
    public MemberAuthorizersResults()
    {
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<Authorizer> GetAuthorizers()
    {
        List<Authorizer> autorizers = new List<Authorizer>();

        foreach (string item in Roles.GetUsersInRole("Authorizer"))
            autorizers.Add(new Authorizer(item));

        return autorizers;
    }
}

public class Authorizer
{
    private string _authorizer;

    public Authorizer(string authorizer)
    {
        _authorizer = authorizer;
    }

    public string Name
    {
        get
        {
            return _authorizer;
        }
    }
}
