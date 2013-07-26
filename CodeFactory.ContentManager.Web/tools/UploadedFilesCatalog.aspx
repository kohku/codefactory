<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadedFilesCatalog.aspx.cs"
    Inherits="_UploadedFilesCatalog" Theme="" StylesheetTheme="Default" %>

<%@ Register TagPrefix="fjx" Namespace="com.flajaxian" Assembly="com.flajaxian.fileuploader" %>
<%@ Register TagPrefix="cfjx" Namespace="CodeFactory.ContentManager.Flajaxian" Assembly="CodeFactory.ContentManager" %>
<%@ Register Assembly="CodeFactory.ContentManager" Namespace="CodeFactory.ContentManager.WebControls"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="TheForm" runat="server">
    <asp:ScriptManager ID="TheScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="TheUpdatePanel" runat="server">
        <ContentTemplate>
            <div id="__uploadedfiles_main">
                <div id="__address_bar">
                    <asp:Label ID="PathLabel" runat="server" Text="Path"></asp:Label>
                    <asp:TextBox runat="server" ID="PathTextBox" Width="600px" />
                    <asp:Button ID="FindPathButton" runat="server" OnClick="FindPathButton_Click" Text="Go" />
                </div>
                <div id="__tree">
                    <asp:TreeView ID="SectionTreeView" runat="server" Width="100%" DataSourceID="TheSectionDataSource"
                        ExpandDepth="3" OnSelectedNodeChanged="SectionTreeView_SelectedNodeChanged">
                    </asp:TreeView>
                </div>
                <div id="__content">
                    <asp:DataList ID="FileUploadDataList" runat="server" DataSourceID="TheUploadedFilesDataSource"
                        DataKeyField="ID" Height="100%" Width="100%" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <ItemTemplate>
                            <div id="__item">
                                <asp:Image ImageUrl="~/images/noThumbnailAvailable.jpg" runat="server" ID="ThumbnailImage"
                                    ImageAlign="Middle" AlternateText='<%# Eval("Description") + "<br/>" + Eval("ContentLength") %>' /><br />
                                <asp:HyperLink ID="FileNameLabel" runat="server" Text='<%# Eval("FileName") %>' NavigateUrl='<%# Eval("DownloadLink") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div id="__tools">
                    <asp:DetailsView ID="FileUploadDetailsView" runat="server" AutoGenerateRows="False"
                        DataKeyNames="ID" DataSourceID="TheUploadedFilesDataSource" DefaultMode="Insert">
                        <Fields>
                            <asp:TemplateField HeaderText="File">
                                <InsertItemTemplate>
                                    <fjx:FileUploader ID="TheFileUploader" runat="server" UseInsideUpdatePanel="true"
                                        IsSingleFileMode="true" MaxFileSize="50MB" MaxFileSizeReachedMessage="No files bigger than {0} are allowed"
                                        OnFileReceived="TheFileUploader_OnFileReceived" JsFunc_FileStateChanged="FileStateChanged">
                                        <Adapters>
                                            <cfjx:UploadStorageAdapter FolderName="~/App_Data/Files" />
                                        </Adapters>
                                    </fjx:FileUploader>
                                    <asp:HiddenField ID="__UploadStatus" runat="server" />
                                    <asp:HiddenField ID="__SectionId" runat="server" />
                                </InsertItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                </div>
                <div id="__status_bar">
                    <div id="__progress_bar">
                        <asp:UpdateProgress ID="TheUpdateProgress" runat="server" AssociatedUpdatePanelID="TheUpdatePanel">
                            <ProgressTemplate>
                                <asp:Image ID="ProgressImage" runat="server" ImageUrl="~/images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        &nbsp;
                    </div>
                    <asp:Label ID="StatusBar" runat="server" Text="Ready"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="TheUploadedFilesDataSource" runat="server" EnablePaging="True"
        SelectMethod="GetCatalog" TypeName="UploadedFilesCatalogSource" OnObjectCreating="TheUploadedFilesDataSource_ObjectCreating"
        SelectCountMethod="TotalCount" DataObjectTypeName="CodeFactory.Web.Storage.UploadedFile"
        DeleteMethod="DeleteFile" InsertMethod="InsertFile" OldValuesParameterFormatString="{0}"
        UpdateMethod="UpdateFile"></asp:ObjectDataSource>
    <cc1:SectionDataSource ID="TheSectionDataSource" runat="server">
    </cc1:SectionDataSource>
    </form>
</body>
</html>
