using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using CodeFactory.Workflow;
using System.Web.Hosting;
using System.IO;
using CodeFactory.Web.PostOffice;
using System.Web.Security;
using System.Collections.Generic;

namespace CodeFactory.Wiki.Workflow
{
    public sealed partial class AuthorizeEntries : SequentialWorkflowActivity
    {
        private Guid _wikiId;

        public Guid WikiID
        {
            get { return _wikiId; }
            set { _wikiId = value; }
        }

        private WorkflowRoleCollection _authorizers = new WorkflowRoleCollection();

        public WorkflowRoleCollection Authorizers
        {
            get { return _authorizers; }
        }

        public WikiEntryEventArgs args = default(WikiEntryEventArgs);

        public AuthorizeEntries()
        {
            InitializeComponent();

            _authorizers.Add(new WebWorkflowRole("Administrator"));
            _authorizers.Add(new WebWorkflowRole("Authorizer"));
        }

        private void InitializeProcessing(object sender, EventArgs e)
        {
            // Set authorizer
            WikiService.Provider.AuthorizationReceived(WorkflowEnvironment.WorkflowInstanceId);
        }

        private void NotifyRequest_SendingEmail(object sender, EventArgs e)
        {
            SendEmailActivity activity = (SendEmailActivity)sender;

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            WorkWikiItem workitem = WorkWikiItem.Load(WorkflowEnvironment.WorkflowInstanceId);

            WikiSerializable item = new WikiSerializable(workitem);

            string requestPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath,
                WikiService.Provider.RequestSchemaPath);

            Stream template = File.OpenRead(requestPath);

            try
            {
                MailMessage message = PostOfficeService.Create(item, template, parameters);

                if (string.IsNullOrEmpty(workitem.Authorizer))
                    return;

                MembershipUser auhtorizer = Membership.GetUser(workitem.Authorizer);

                if (auhtorizer == null || string.IsNullOrEmpty(auhtorizer.Email))
                    return;

                message.AddAddresses(auhtorizer.Email);

                activity.UserData.Add("MailMessage", message);
            }
            finally
            {
                template.Close();
            }
        }

        private void EvaluateAuthorization(object sender, ConditionalEventArgs e)
        {
            WorkWikiItem workitem = WorkWikiItem.Load(WorkflowEnvironment.WorkflowInstanceId);

            e.Result = (workitem != null && workitem.Status == WikiStatus.AuthorizationRequested);
        }

        private void AuthorizationTimer_Initialize(object sender, EventArgs e)
        {
            DelayActivity activity = (DelayActivity)sender;

            DateTime Now = DateTime.Now;

            WorkWikiItem item = WorkWikiItem.Load(WorkflowEnvironment.WorkflowInstanceId);

            DateTime expirationDate = Now;

            if (!item.ExpirationDate.HasValue || item.ExpirationDate.Equals(DateTime.MinValue))
            {
                TimeSpan startTime = TimeSpan.Parse(WikiService.Provider.DefaultStartTime);
                TimeSpan endTime = TimeSpan.Parse(WikiService.Provider.DefaultEndTime);

                DateTime startNextDayDateTime = Now.Date.Add(startTime).AddDays(1);

                // Si es sábado y no se incluyen fines de semana nos vamos hasta el lunes.
                if (!WikiService.Provider.IncludeWeekends && startNextDayDateTime.DayOfWeek == DayOfWeek.Saturday)
                    startNextDayDateTime.AddDays(2);

                DateTime todayEndDateTime = Now.Date.Add(endTime);
                expirationDate = Now.Add(WikiService.Provider.TimeToExpire);

                // Si la fecha de expiración sobre pasa a la fecha de término del día de hoy entonces 
                // la diferencia será tomada para el siguiente día laboral.
                if (expirationDate > todayEndDateTime)
                    expirationDate = startNextDayDateTime.Add(expirationDate.Subtract(todayEndDateTime));

                item.ExpirationDate = expirationDate;
                item.Save();
            }
            else
            {
                if (item.ExpirationDate.HasValue)
                    expirationDate = item.ExpirationDate.Value;
            }

            activity.TimeoutDuration = expirationDate > Now ? expirationDate.Subtract(Now) :
                new TimeSpan(0, 0, 1);
        }

        private void Authorized_Invoked(object sender, ExternalDataEventArgs e)
        {
            WikiService.Provider.CompleteAuthorization(WorkflowEnvironment.WorkflowInstanceId, e.Identity);
        }

        private void Rejected_Invoked(object sender, ExternalDataEventArgs e)
        {
            WikiService.Provider.CompleteRejection(WorkflowEnvironment.WorkflowInstanceId, e.Identity);
        }

        private void ExpireAuthorization_ExecuteCode(object sender, EventArgs e)
        {
            WikiService.Provider.CompleteExpiration(WorkflowEnvironment.WorkflowInstanceId);
        }

        private void NotifyResponse_SendingEmail(object sender, EventArgs e)
        {
            SendEmailActivity activity = (SendEmailActivity)sender;

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            WikiHistory record = WikiHistory.Load(WorkflowEnvironment.WorkflowInstanceId);

            WikiSerializable item = new WikiSerializable(record);

            string responsePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                WikiService.Provider.ResponseSchemaPath);

            Stream template = File.OpenRead(responsePath);

            try
            {
                MailMessage message = PostOfficeService.Create(item, template, parameters);

                if (string.IsNullOrEmpty(record.Author))
                    return;

                MembershipUser user = Membership.GetUser(record.Author);

                if (user == null || string.IsNullOrEmpty(user.Email))
                    return;

                message.AddAddresses(user.Email);

                activity.UserData.Add("MailMessage", message);
            }
            finally
            {
                template.Close();
            }
        }

        private void HandleAuthorizationException(object sender, EventArgs e)
        {
            WorkWikiItem workitem = WorkWikiItem.Load(WorkflowEnvironment.WorkflowInstanceId);

            if (workitem != null)
            {
                workitem.Messages = string.Empty;
                workitem.Status = WikiStatus.AuthorizationRequested;
                workitem.Save();
            }
        }
    }
}
