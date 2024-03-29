﻿using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace CodeFactory.Workflow
{
    [ActivityValidator(typeof(SendEmailValidator))]
    [ToolboxBitmap(typeof(SendEmailActivity), "Resources.EmailMessage.png")]
    [DefaultEvent("SendingEmail")]
    [DefaultProperty("To")]
    public partial class SendEmailActivity : System.Workflow.ComponentModel.Activity
    {
        // Define the DependencyProperty objects for all of the Properties 
        // ...and Events exposed by this activity
        public static DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(string), typeof(SendEmailActivity), new PropertyMetadata("someone@example.com"));
        public static DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(string), typeof(SendEmailActivity), new PropertyMetadata("someone@example.com"));
        public static DependencyProperty BodyProperty = DependencyProperty.Register("Body", typeof(string), typeof(SendEmailActivity));
        public static DependencyProperty SubjectProperty = DependencyProperty.Register("Subject", typeof(string), typeof(SendEmailActivity));
        public static DependencyProperty HtmlBodyProperty = DependencyProperty.Register("HtmlBody", typeof(bool), typeof(SendEmailActivity), new PropertyMetadata(false));
        public static DependencyProperty CCProperty = DependencyProperty.Register("CC", typeof(string), typeof(SendEmailActivity));
        public static DependencyProperty BccProperty = DependencyProperty.Register("Bcc", typeof(string), typeof(SendEmailActivity));
        public static DependencyProperty PortProperty = DependencyProperty.Register("Port", typeof(int), typeof(SendEmailActivity), new PropertyMetadata(25));
        public static DependencyProperty SmtpHostProperty = DependencyProperty.Register("SmtpHost", typeof(string), typeof(SendEmailActivity), new PropertyMetadata("localhost"));
        public static DependencyProperty ReplyToProperty = DependencyProperty.Register("ReplyTo", typeof(string), typeof(SendEmailActivity));

        public static DependencyProperty SendingEmailEvent = DependencyProperty.Register("SendingEmail", typeof(EventHandler), typeof(SendEmailActivity), new PropertyMetadata(null));
        public static DependencyProperty SentEmailEvent = DependencyProperty.Register("SentEmail", typeof(EventHandler), typeof(SendEmailActivity), new PropertyMetadata(null));


        // Define constant values for the Property categories.  
        private const string MessagePropertiesCategory = "Email Message";
        private const string SMTPPropertiesCategory = "Email Server";
        private const string EventsCategory = "Handlers";

        // Define private constants for the Validation Errors 
        private const int InvalidToAddress = 1;
        private const int InvalidFromAddress = 2;
        private const int InvalidSMTPPort = 3;

        public SendEmailActivity()
        {
            InitializeComponent();
        }

        #region Email Message Properties

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [DescriptionAttribute("The To property is used to specify the receipient's email address.")]
        [CategoryAttribute(MessagePropertiesCategory)]
        public string To
        {
            get
            {
                return ((string)(base.GetValue(SendEmailActivity.ToProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.ToProperty, value);
            }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [DescriptionAttribute("The Subject property is used to specify the subject of the Email message.")]
        [CategoryAttribute(MessagePropertiesCategory)]
        public string Subject
        {
            get
            {
                return ((string)(base.GetValue(SendEmailActivity.SubjectProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.SubjectProperty, value);
            }
        }


        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [DescriptionAttribute("The From property is used to specify the From (Sender's) address for the email mesage.")]
        [CategoryAttribute(MessagePropertiesCategory)]
        public string From
        {
            get
            {
                return ((string)(base.GetValue(SendEmailActivity.FromProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.FromProperty, value);
            }
        }


        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [DescriptionAttribute("The Body property is used to specify the Body of the email message.")]
        [CategoryAttribute(MessagePropertiesCategory)]
        public string Body
        {

            get
            {
                return (string)base.GetValue(SendEmailActivity.BodyProperty);
            }
            set
            {

                base.SetValue(SendEmailActivity.BodyProperty, value);
            }

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("The HTMLBody property is used to specify whether the Body is formatted as HTML (True) or not (False)")]
        [Category(MessagePropertiesCategory)]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool HtmlBody
        {
            get
            {
                return ((bool)(base.GetValue(SendEmailActivity.HtmlBodyProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.HtmlBodyProperty, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("The CC property is used to set the CC recipients for the email message.")]
        [Category(MessagePropertiesCategory)]
        [Browsable(true)]
        public string CC
        {
            get
            {
                return ((string)(base.GetValue(SendEmailActivity.CCProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.CCProperty, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("The Bcc property is used to set the Bcc recipients for the email message.")]
        [Category(MessagePropertiesCategory)]
        [Browsable(true)]
        public string Bcc
        {
            get
            {
                return ((string)(base.GetValue(SendEmailActivity.BccProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.BccProperty, value);
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("The email address that should be used for reply messages.")]
        [Category(MessagePropertiesCategory)]
        [Browsable(true)]
        public string ReplyTo
        {
            get
            {
                return ((string)(base.GetValue(SendEmailActivity.ReplyToProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.ReplyToProperty, value);
            }
        }

        #endregion

        #region SMTP Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("The SMTP host is the machine running SMTP that will send the email.  The default is 'localhost'")]
        [Category(SMTPPropertiesCategory)]
        [Browsable(true)]
        public string SmtpHost
        {
            get
            {
                return ((string)(base.GetValue(SendEmailActivity.SmtpHostProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.SmtpHostProperty, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Specify the Port used for SMTP.  The default is 25.")]
        [Category(SMTPPropertiesCategory)]
        [Browsable(true)]
        public int Port
        {
            get
            {
                return ((int)(base.GetValue(SendEmailActivity.PortProperty)));
            }
            set
            {
                base.SetValue(SendEmailActivity.PortProperty, value);
            }
        }

        #endregion

        #region Public Events

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("The SendingEmail event is raised before an email is sent through SMTP.")]
        [Category(EventsCategory)]
        [Browsable(true)]
        public event EventHandler SendingEmail
        {
            add
            {
                base.AddHandler(SendEmailActivity.SendingEmailEvent, value);
            }
            remove
            {
                base.RemoveHandler(SendEmailActivity.SendingEmailEvent, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("The SentEmail event is raised after an email is sent through SMTP.")]
        [Category(EventsCategory)]
        [Browsable(true)]
        public event EventHandler SentEmail
        {
            add
            {
                base.AddHandler(SendEmailActivity.SentEmailEvent, value);
            }
            remove
            {
                base.RemoveHandler(SendEmailActivity.SentEmailEvent, value);
            }
        }
        #endregion

        #region Activity Execution Logic

        ///    During execution the SendEmail activity should create and send the email using SMTP.  
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext context)
        {
            try
            {
                // Raise the SendingEmail event to the parent workflow or activity
                base.RaiseEvent(SendEmailActivity.SendingEmailEvent, this, EventArgs.Empty);

                this.SendEmailUsingSmtp(context);

                // Raise the SentEmail event to the parent workflow or activity
                base.RaiseEvent(SendEmailActivity.SentEmailEvent, this, EventArgs.Empty);
            }
            catch
            {
                // An unhandled exception occured. Won't be throw it back to the WorkflowRuntime.
            }

            // Retun the closed status indicating that this activity is complete.
            return ActivityExecutionStatus.Closed;
        }


        private void SendEmailUsingSmtp(ActivityExecutionContext context)
        {
            try
            {
                if (!this.UserData.Contains("MailMessage"))
                    return;

                CodeFactory.Web.PostOffice.MailMessage message = (CodeFactory.Web.PostOffice.MailMessage)
                    this.UserData["MailMessage"];

                message.Send();
            }
            catch (Exception)
            {
                //// It won't throw the exception again 'cause this is only a notification activity.
                //ModuloDiagnostico.RegistraWarning(ResourceStringLoader.GetResourceString(
                //    "SEND_EMAIL_USING_SMTP_FAILED", ex.Message, ex.StackTrace));
            }
        }

        public class SendEmailValidator : System.Workflow.ComponentModel.Compiler.ActivityValidator
        {
            //customizing the default activity validation
            public override ValidationErrorCollection ValidateProperties(ValidationManager manager, object obj)
            {

                // Create a new collection for storing the validation errors
                ValidationErrorCollection validationErrors = base.ValidateProperties(manager, obj);


                SendEmailActivity activity = obj as SendEmailActivity;
                if (activity != null)
                {
                    // Validate the Email and SMTP Properties
                    this.ValidateEmailProperties(validationErrors, activity);
                    this.ValidateSMTPProperties(validationErrors, activity);
                }
                return validationErrors;
            }

            private void ValidateEmailProperties(ValidationErrorCollection validationErrors, SendEmailActivity activity)
            {
                // Validate the To property
                if (String.IsNullOrEmpty(activity.To))
                    validationErrors.Add(ValidationError.GetNotSetValidationError(SendEmailActivity.ToProperty.Name));
                else if (!activity.To.Contains("@"))
                    validationErrors.Add(new ValidationError("Invalid To e-mail address", InvalidToAddress, false, SendEmailActivity.ToProperty.Name));

                // Validate the From property
                if (String.IsNullOrEmpty(activity.From))
                    validationErrors.Add(ValidationError.GetNotSetValidationError(SendEmailActivity.FromProperty.Name));
                else if (!activity.From.Contains("@"))
                    validationErrors.Add(new ValidationError("Invalid From e-mail address", InvalidFromAddress, false, SendEmailActivity.FromProperty.Name));
            }


            private void ValidateSMTPProperties(ValidationErrorCollection validationErrors, SendEmailActivity activity)
            {
                // Validate the SMTPHost property
                if (String.IsNullOrEmpty(activity.SmtpHost))
                    validationErrors.Add(ValidationError.GetNotSetValidationError(SendEmailActivity.SmtpHostProperty.Name));

                // Validate the Port property
                if (activity.Port == 0)
                    validationErrors.Add(ValidationError.GetNotSetValidationError(SendEmailActivity.PortProperty.Name));
                else if (activity.Port < 1)
                    validationErrors.Add(new ValidationError("Invalid Port Number",
                        InvalidSMTPPort, false, SendEmailActivity.PortProperty.Name));
            }

        }

        #endregion
    }
}
