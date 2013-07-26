<%@ Application Language="C#" %>
<%@ Import Namespace="Microsoft.Practices.EnterpriseLibrary.Logging" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="CodeFactory.Web.Storage" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        this.AuthenticateRequest += new EventHandler(global_asax_AuthenticateRequest);

        CodeFactory.Web.HttpHandlers.UploadStorageFileHandler.Served += new EventHandler<EventArgs>(FileHandler_Served);
    }

    void FileHandler_Served(object sender, EventArgs e)
    {
        UploadedFile file = sender as UploadedFile;

        LogEntry entry = new LogEntry();

        entry.Categories.Clear();
        entry.Categories.Add("Bitacora de consultas");
        entry.Priority = 5;
        entry.Severity = TraceEventType.Information;
        entry.Message = "Documento consultado";
        entry.ExtendedProperties.Add("id", file.ID);
        entry.ExtendedProperties.Add("title", file.FileName);
        entry.ExtendedProperties.Add("urlRequested", HttpContext.Current != null ?
            HttpContext.Current.Request.Url.PathAndQuery : "No disponible");
        entry.ExtendedProperties.Add("username", HttpContext.Current != null ?
            HttpContext.Current.User.Identity.Name : string.Empty);
        entry.ExtendedProperties.Add("type", file.GetType().ToString());

        Logger.Write(entry);
    }

    void global_asax_AuthenticateRequest(object sender, EventArgs e)
    {
        if (!this.Request.IsAuthenticated)
        {
            int start = this.Request.Path.LastIndexOf("/");
            string path = this.Request.Path.Substring(start + 1);

            if (path.ToUpper() != "LOGIN.ASPX")
                this.Response.Cookies["ReturnUrl"].Value = this.Request.Path;
        }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
        if (CodeFactory.Wiki.WikiService.Provider is CodeFactory.Wiki.Providers.LinqWikiProvider)
            ((CodeFactory.Wiki.Providers.LinqWikiProvider)CodeFactory.Wiki.WikiService.Provider).Dispose();
        
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Debug.WriteLine("Session starts");
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        foreach (string key in Session)
        {
            if (CodeFactory.Web.Utils.IsGuid(key))
            {
                CodeFactory.Wiki.Wiki wiki = (CodeFactory.Wiki.Wiki)Session[key];

                if (wiki.IsNew)
                {
                    foreach (CodeFactory.Web.Storage.UploadedFile file in wiki.Files)
                    {
                        file.Delete();
                        file.Save();
                    }
                }
            }
        }
        Debug.WriteLine("Session ends");
    }

    public override string GetVaryByCustomString(HttpContext context, string custom)
    {
        if (custom == "userId")
            return User.Identity.Name;

        return base.GetVaryByCustomString(context, custom);
    }
       
</script>
