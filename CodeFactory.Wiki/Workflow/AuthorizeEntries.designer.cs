using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace CodeFactory.Wiki.Workflow
{
    partial class AuthorizeEntries
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding1 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            this.NotAuthorizedC2 = new System.Workflow.Activities.CodeActivity();
            this.NotAuthorizedC1 = new System.Workflow.Activities.CodeActivity();
            this.NotAuthorized2 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.NotAuthorized1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.ExpireAuthorization = new System.Workflow.Activities.CodeActivity();
            this.AutorizationTimer = new System.Workflow.Activities.DelayActivity();
            this.cancellationHandlerActivity3 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.AuthorizationFailed = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.Rejected = new System.Workflow.Activities.HandleExternalEventActivity();
            this.AutorizationFailed = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.cancellationHandlerActivity2 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.Authorized = new System.Workflow.Activities.HandleExternalEventActivity();
            this.AuthorizationExpired = new System.Workflow.Activities.EventDrivenActivity();
            this.AuthorizationRejected = new System.Workflow.Activities.EventDrivenActivity();
            this.AuthorizationAccepted = new System.Workflow.Activities.EventDrivenActivity();
            this.WaitForResponse = new System.Workflow.Activities.ListenActivity();
            this.Warrant = new System.Workflow.Activities.WhileActivity();
            this.NotifyRequest = new CodeFactory.Workflow.SendEmailActivity();
            this.AuthorizationRequested = new System.Workflow.Activities.SequenceActivity();
            // 
            // NotAuthorizedC2
            // 
            this.NotAuthorizedC2.Name = "NotAuthorizedC2";
            this.NotAuthorizedC2.ExecuteCode += new System.EventHandler(this.HandleAuthorizationException);
            // 
            // NotAuthorizedC1
            // 
            this.NotAuthorizedC1.Name = "NotAuthorizedC1";
            this.NotAuthorizedC1.ExecuteCode += new System.EventHandler(this.HandleAuthorizationException);
            // 
            // NotAuthorized2
            // 
            this.NotAuthorized2.Activities.Add(this.NotAuthorizedC2);
            this.NotAuthorized2.FaultType = typeof(System.Workflow.Activities.WorkflowAuthorizationException);
            this.NotAuthorized2.Name = "NotAuthorized2";
            // 
            // NotAuthorized1
            // 
            this.NotAuthorized1.Activities.Add(this.NotAuthorizedC1);
            this.NotAuthorized1.FaultType = typeof(System.Workflow.Activities.WorkflowAuthorizationException);
            this.NotAuthorized1.Name = "NotAuthorized1";
            // 
            // ExpireAuthorization
            // 
            this.ExpireAuthorization.Name = "ExpireAuthorization";
            this.ExpireAuthorization.ExecuteCode += new System.EventHandler(this.ExpireAuthorization_ExecuteCode);
            // 
            // AutorizationTimer
            // 
            this.AutorizationTimer.Name = "AutorizationTimer";
            this.AutorizationTimer.TimeoutDuration = System.TimeSpan.Parse("00:00:00");
            this.AutorizationTimer.InitializeTimeoutDuration += new System.EventHandler(this.AuthorizationTimer_Initialize);
            // 
            // cancellationHandlerActivity3
            // 
            this.cancellationHandlerActivity3.Name = "cancellationHandlerActivity3";
            // 
            // AuthorizationFailed
            // 
            this.AuthorizationFailed.Activities.Add(this.NotAuthorized2);
            this.AuthorizationFailed.Name = "AuthorizationFailed";
            activitybind1.Name = "AuthorizeEntries";
            activitybind1.Path = "Authorizers";
            // 
            // Rejected
            // 
            this.Rejected.EventName = "AuthorizationRejected";
            this.Rejected.InterfaceType = typeof(CodeFactory.Wiki.Workflow.IWikiServiceProvider);
            this.Rejected.Name = "Rejected";
            this.Rejected.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.Rejected_Invoked);
            this.Rejected.SetBinding(System.Workflow.Activities.HandleExternalEventActivity.RolesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // AutorizationFailed
            // 
            this.AutorizationFailed.Activities.Add(this.NotAuthorized1);
            this.AutorizationFailed.Name = "AutorizationFailed";
            // 
            // cancellationHandlerActivity2
            // 
            this.cancellationHandlerActivity2.Name = "cancellationHandlerActivity2";
            activitybind2.Name = "AuthorizeEntries";
            activitybind2.Path = "Authorizers";
            // 
            // Authorized
            // 
            this.Authorized.EventName = "AuthorizationAccepted";
            this.Authorized.InterfaceType = typeof(CodeFactory.Wiki.Workflow.IWikiServiceProvider);
            this.Authorized.Name = "Authorized";
            workflowparameterbinding1.ParameterName = "e";
            this.Authorized.ParameterBindings.Add(workflowparameterbinding1);
            this.Authorized.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.Authorized_Invoked);
            this.Authorized.SetBinding(System.Workflow.Activities.HandleExternalEventActivity.RolesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // AuthorizationExpired
            // 
            this.AuthorizationExpired.Activities.Add(this.AutorizationTimer);
            this.AuthorizationExpired.Activities.Add(this.ExpireAuthorization);
            this.AuthorizationExpired.Name = "AuthorizationExpired";
            // 
            // AuthorizationRejected
            // 
            this.AuthorizationRejected.Activities.Add(this.Rejected);
            this.AuthorizationRejected.Activities.Add(this.AuthorizationFailed);
            this.AuthorizationRejected.Activities.Add(this.cancellationHandlerActivity3);
            this.AuthorizationRejected.Name = "AuthorizationRejected";
            // 
            // AuthorizationAccepted
            // 
            this.AuthorizationAccepted.Activities.Add(this.Authorized);
            this.AuthorizationAccepted.Activities.Add(this.cancellationHandlerActivity2);
            this.AuthorizationAccepted.Activities.Add(this.AutorizationFailed);
            this.AuthorizationAccepted.Name = "AuthorizationAccepted";
            // 
            // WaitForResponse
            // 
            this.WaitForResponse.Activities.Add(this.AuthorizationAccepted);
            this.WaitForResponse.Activities.Add(this.AuthorizationRejected);
            this.WaitForResponse.Activities.Add(this.AuthorizationExpired);
            this.WaitForResponse.Name = "WaitForResponse";
            // 
            // Warrant
            // 
            this.Warrant.Activities.Add(this.WaitForResponse);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.EvaluateAuthorization);
            this.Warrant.Condition = codecondition1;
            this.Warrant.Name = "Warrant";
            // 
            // NotifyRequest
            // 
            this.NotifyRequest.Bcc = null;
            this.NotifyRequest.Body = null;
            this.NotifyRequest.CC = null;
            this.NotifyRequest.Description = "Notifica al autorizador de la solicitud recibida.";
            this.NotifyRequest.From = "someone@example.com";
            this.NotifyRequest.Name = "NotifyRequest";
            this.NotifyRequest.Port = 25;
            this.NotifyRequest.ReplyTo = null;
            this.NotifyRequest.SmtpHost = "localhost";
            this.NotifyRequest.Subject = null;
            this.NotifyRequest.To = "someone@example.com";
            this.NotifyRequest.SendingEmail += new System.EventHandler(this.NotifyRequest_SendingEmail);
            // 
            // AuthorizationRequested
            // 
            this.AuthorizationRequested.Activities.Add(this.NotifyRequest);
            this.AuthorizationRequested.Activities.Add(this.Warrant);
            this.AuthorizationRequested.Name = "AuthorizationRequested";
            // 
            // AuthorizeEntries
            // 
            this.Activities.Add(this.AuthorizationRequested);
            this.Name = "AuthorizeEntries";
            this.Initialized += new System.EventHandler(this.InitializeProcessing);
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity NotAuthorizedC2;
        private CodeActivity NotAuthorizedC1;
        private FaultHandlerActivity NotAuthorized2;
        private FaultHandlerActivity NotAuthorized1;
        private CodeActivity ExpireAuthorization;
        private DelayActivity AutorizationTimer;
        private CancellationHandlerActivity cancellationHandlerActivity3;
        private FaultHandlersActivity AuthorizationFailed;
        private HandleExternalEventActivity Rejected;
        private FaultHandlersActivity AutorizationFailed;
        private CancellationHandlerActivity cancellationHandlerActivity2;
        private HandleExternalEventActivity Authorized;
        private EventDrivenActivity AuthorizationExpired;
        private EventDrivenActivity AuthorizationRejected;
        private EventDrivenActivity AuthorizationAccepted;
        private ListenActivity WaitForResponse;
        private WhileActivity Warrant;
        private CodeFactory.Workflow.SendEmailActivity NotifyRequest;
        private SequenceActivity AuthorizationRequested;


    }
}
