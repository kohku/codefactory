using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Security;
using System.Net.Mail;

namespace CodeFactory.Web.PostOffice
{
    public class SmtpPostOfficeProvider : PostOfficeProvider
    {
        protected override void Prepare(HtmlMailMessage message, object holder, Stream template, Dictionary<string, string> parameters)
        {
            Prepare(message, holder, template, parameters, false);
        }

        protected void Prepare(HtmlMailMessage message, object holder, Stream template, Dictionary<string, string> parameters, bool closeStream)
        {
            try
            {
                PrepareWithStringBuilders(message, holder, template, parameters);
            }
            finally
            {
                if (closeStream)
                    template.Close();
            }
        }

        private static void PrepareWithStringBuilders(HtmlMailMessage message, object holder, Stream template, Dictionary<string, string> parameters)
        {
            if (template == null)
                throw new ArgumentNullException("template");

            if (holder == null)
                throw new ArgumentNullException("holder");

            if (parameters == null)
                throw new ArgumentNullException("parameters");

            try
            {
                // Reset postition of the current stream to fist byte.
                if (template.Position != 0)
                    template.Position = 0;

                // Initializing a new instance of the XslCompiledTransform
                XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();

                // Creating fast, non-cached, forward-only reader to access to the XSL resource.
                XmlReader reader = XmlReader.Create(template);

                // Loads and compiles de style sheet
                try
                {
                    xslCompiledTransform.Load(reader);
                }
                finally
                {
                    reader.Close();
                }

                // Initialize a new instance of a string builder.
                StringBuilder builder = new StringBuilder();
                // Creating a fast, non-cached, forward-only writer for generating streams containing XML data.
                XmlWriter writer = XmlWriter.Create(builder);

                // Initializing the serializer.
                XmlSerializer serializer = new XmlSerializer(holder.GetType());
                // Generating the stream with the holder serialized.
                serializer.Serialize(writer, holder);

                // Initializing an instance of a XML document.
                XmlDocument document = new XmlDocument();
                // Loading the XML already generated with the serializer.
                document.LoadXml(builder.ToString());

                // Creates a navigator for navigating through the document.
                XPathNavigator navigator = document.CreateNavigator();
                // Creates a new instance of a XSLT argument list.
                XsltArgumentList arguments = new XsltArgumentList();

                // Adds parameters specified in parameters.
                foreach (KeyValuePair<string, string> entry in parameters)
                    arguments.AddParam(entry.Key, string.Empty, entry.Value);

                // Creates a new stream for store the XML result.
                StringBuilder result = new StringBuilder();
                // Create a fast, non-cached, forward-only writer for generating streams containint XML data.
                XmlWriter writerResult = XmlWriter.Create(result);

                // Executes the transform using the navigator, the documents.
                xslCompiledTransform.Transform(navigator, arguments, writerResult);

                // Generating email message.
                XmlDocument emailMessage = new XmlDocument();
                emailMessage.LoadXml(result.ToString());

                //Create an XmlNamespaceManager for resolving namespaces.
                XmlNamespaceManager manager = new XmlNamespaceManager(emailMessage.NameTable);
                manager.AddNamespace("html", "http://www.w3.org/1999/xhtml");

                XmlNode subject = emailMessage.SelectSingleNode("//html:title", manager);

                if (subject != null)
                    message.Subject = subject.InnerText.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);

                // Generating email body.
                XmlNode body = emailMessage.SelectSingleNode("//html:body", manager);

                if (body != null)
                {
                    message.Body = body.InnerXml;
                    if (message.Body.Length > 0)
                        message.Body = message.Body.Replace("&amp;", "&");
                }
            }
            catch (IOException ex)
            {
                // Set stream position to 0;
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (NotSupportedException ex)
            {
                // Set stream position to 0;
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (ObjectDisposedException ex)
            {
                // Set stream position to 0;
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (SecurityException ex)
            {
                // Loading template failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (XsltException ex)
            {
                // Loading template failed.
                // Transform failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (XmlException ex)
            {
                // Loading document serialized.
                // Loading transformed document failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (InvalidOperationException ex)
            {
                // Serializer failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (ArgumentException ex)
            {
                // Add parameter failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (XPathException ex)
            {
                // Processing message.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
        }

        private void PrepareWithStreams(HtmlMailMessage message, object holder, Stream template, Dictionary<string, string> parameters)
        {
            if (template == null)
                throw new ArgumentNullException("template");

            if (holder == null)
                throw new ArgumentNullException("holder");

            if (parameters == null)
                throw new ArgumentNullException("parameters");

            try
            {
                // Reset postition of the current stream to fist byte.
                if (template.Position != 0)
                    template.Position = 0;

                // Initializing a new instance of the XslCompiledTransform
                XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();

                // Creating fast, non-cached, forward-only reader to access to the XSL resource.
                XmlReader reader = XmlReader.Create(template);

                // Loads and compiles de style sheet
                try
                {
                    xslCompiledTransform.Load(reader);
                }
                finally
                {
                    reader.Close();
                }

                // Creates a new stream for store the XML serialized holder.
                MemoryStream ticketStream = new MemoryStream();

                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    // The xml declaration is already included.
                    settings.OmitXmlDeclaration = true;
                    // It is a document.
                    settings.ConformanceLevel = ConformanceLevel.Document;
                    // Do not close the memory stream.
                    settings.CloseOutput = false;
                    // Using same encoding as paramater stream.
                    settings.Encoding = xslCompiledTransform.OutputSettings.Encoding;

                    // Creates a  new fast, non-cached, forward-only generator of XML stream.
                    XmlWriter writer = XmlWriter.Create(ticketStream, settings);

                    try
                    {
                        // Initializing the serializer.
                        XmlSerializer serializer = new XmlSerializer(holder.GetType());
                        // Generating the stream with the holder serialized.
                        serializer.Serialize(writer, holder);
                    }
                    finally
                    {
                        writer.Close();
                    }

                    // Reset the memory stream position because we're gonna user with a reader instead the writer.
                    ticketStream.Position = 0;

                    // Initializing an instance of a XML document.
                    XmlDocument document = new XmlDocument();
                    // Creates a fast non-cached, forward-only reader.
                    reader = XmlReader.Create(ticketStream);

                    // Loading the XML already generated with the serializer.
                    try
                    {
                        document.Load(reader);
                    }
                    finally
                    {
                        reader.Close();
                    }

                    // Creates a navigator for navigating through the document.
                    XPathNavigator navigator = document.CreateNavigator();
                    // Creates a new instance of a XSLT argument list.
                    XsltArgumentList arguments = new XsltArgumentList();

                    // Adds parameters specified in parameters.
                    foreach (KeyValuePair<string, string> entry in parameters)
                        arguments.AddParam(entry.Key, string.Empty, entry.Value);

                    MemoryStream resultStream = new MemoryStream();

                    try
                    {
                        XmlWriter writerResult = XmlWriter.Create(resultStream, settings);

                        try
                        {
                            // Executes the transform using the navigator, the documents.
                            xslCompiledTransform.Transform(navigator, arguments, writerResult);
                        }
                        finally
                        {
                            writerResult.Close();
                        }

                        resultStream.Position = 0;

                        // Generating email message.
                        XmlDocument emailMessage = new XmlDocument();
                        // Creates a fast, non-cached, forward-only reader.
                        XmlReader readerResult = XmlReader.Create(resultStream);

                        try
                        {
                            emailMessage.Load(readerResult);
                        }
                        finally
                        {
                            readerResult.Close();
                        }

                        //Create an XmlNamespaceManager for resolving namespaces.
                        XmlNamespaceManager manager = new XmlNamespaceManager(emailMessage.NameTable);
                        manager.AddNamespace("html", "http://www.w3.org/1999/xhtml");

                        XmlNode subject = emailMessage.SelectSingleNode("//html:title", manager);

                        if (subject != null)
                            message.Subject = subject.InnerText;

                        // Generating email body.
                        XmlNode body = emailMessage.SelectSingleNode("//html:body", manager);

                        if (body != null)
                        {
                            message.Body = body.InnerXml;
                            if (message.Body.Length > 0)
                                message.Body = message.Body.Replace("&amp;", "&");
                        }
                    }
                    finally
                    {
                        resultStream.Close();
                    }
                }
                finally
                {
                    ticketStream.Close();
                }
            }
            catch (IOException ex)
            {
                // Set stream position to 0;
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (NotSupportedException ex)
            {
                // Set stream position to 0;
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (ObjectDisposedException ex)
            {
                // Set stream position to 0;
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (SecurityException ex)
            {
                // Loading template failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (XsltException ex)
            {
                // Loading template failed.
                // Transform failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (XmlException ex)
            {
                // Loading document serialized.
                // Loading transformed document failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (InvalidOperationException ex)
            {
                // Serializer failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (ArgumentException ex)
            {
                // Add parameter failed.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
            catch (XPathException ex)
            {
                // Processing message.
                throw new ApplicationException("Error while preparing mailMessage.", ex);
            }
        }

        public override void SendMessage(HtmlMailMessage message)
        {
            if (message.To.Count > 0)
            {
                SmtpClient client = new SmtpClient();

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(
                    message.Body, null, System.Net.Mime.MediaTypeNames.Text.Html);

                string textBody = "You must use an e-mail client that supports HTML messages";

                // Create an alternate view for unsupported clients
                AlternateView avText = AlternateView.CreateAlternateViewFromString(
                    textBody, null, System.Net.Mime.MediaTypeNames.Text.Plain);

                foreach (KeyValuePair<string, Stream> item in message.LinkedResources)
                {
                    LinkedResource linkedresource = new LinkedResource(
                        item.Value, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                    linkedresource.ContentId = item.Key;
                    avHtml.LinkedResources.Add(linkedresource);
                }

                message.AlternateViews.Add(avHtml);
                message.AlternateViews.Add(avText);

                client.Send(message);
            }
        }
    }
}
