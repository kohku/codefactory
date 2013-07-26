<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/themes/CodeFactoryDefault/CodeFactoryDefault.master" CodeFile="ProjectWizard.aspx.cs" Inherits="_ProjectWizard" Title="CodeFactory" UICulture="Auto" Async="true" %>
<asp:Content ContentPlaceHolderID="Main" ID="Main" runat="server">
<div class="project_wizard_main">
    <!--OnActiveStepChanged="ProjectWizard_ActiveStepChanged"-->
    <asp:Wizard ID="ProjectWizard" runat="server" ActiveStepIndex="0"
        Width="900px" OnFinishButtonClick="ProjectWizard_FinishButtonClick" 
        CancelDestinationPageUrl="~/Default.aspx" 
        FinishDestinationPageUrl="~/Default.aspx" 
        OnCancelButtonClick="ProjectWizard_CancelButtonClick" 
        OnNextButtonClick="ProjectWizard_NextButtonClick">
        <SideBarStyle VerticalAlign="Top" Width="200px" HorizontalAlign="Right"/>
        <StepStyle VerticalAlign="Top" Width="700px" HorizontalAlign="Left" />
        <NavigationStyle Width="500px" />
        <SideBarTemplate>
            <asp:DataList ID="SideBarList" runat="server" HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:LinkButton ID="SideBarButton" runat="server" CssClass="wizard_sidebar" CausesValidation="false"></asp:LinkButton>
                </ItemTemplate>
                <SelectedItemStyle Font-Bold="True" />
            </asp:DataList>
        </SideBarTemplate>
        <WizardSteps>
            <asp:WizardStep ID="InformationStep" runat="server" StepType="Start" Title="<%$ Resources:InformationStep.Title %>">
                <div class="wizard_step">
                    <div class="wizard_step_row">
                        <asp:Label ID="TitleLabel" runat="server" Text="<%$ Resources:LocalizedText, Title %>" CssClass="wizard_title"></asp:Label>
                    </div>
                    <div class="wizard_step_row">
                        <asp:TextBox ID="TitleTextBox" runat="server" Width="300px" CssClass="wizard_text" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TitleValidator" runat="server" ControlToValidate="TitleTextBox"
                            CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="wizard_step_row">
                        <asp:Label ID="DescriptionLabel" runat="server" Text="<%$ Resources:LocalizedText, Description %>" CssClass="wizard_title"></asp:Label>
                    </div>
                    <div class="wizard_step_row">
                        <asp:TextBox ID="DescriptionTextBox" runat="server" Width="400px" CssClass="wizard_text" MaxLength="512"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="DescriptionValidator" runat="server" ControlToValidate="DescriptionTextBox"
                            CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="wizard_step_row">
                        <asp:Label ID="ContentLabel" runat="server" Text="<%$ Resources:LocalizedText, Content %>" CssClass="wizard_title"></asp:Label>
                        &nbsp;
                    </div>
                    <div class="wizard_step_row" style="height: 130px; vertical-align: top;">
                        <asp:TextBox ID="ContentTextBox" runat="server" Rows="5" TextMode="MultiLine" Width="400px" CssClass="wizard_text" MaxLength="1024"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="CommentValidator" runat="server" ControlToValidate="ContentTextBox"
                            CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="ContentTextBox"
                            CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"
                            ValidationExpression="^[\s\S]{1,1024}$"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="FilesStep" runat="server" StepType="Step" Title="<%$ Resources:FilesStep.Title %>">
                <div class="wizard_step">
                    <div class="wizard_step_row">
                        <asp:DetailsView ID="ThumbnailsDetailsView" runat="server" AutoGenerateRows="False"
                        DataSourceID="ThumbnailsDataSource" DefaultMode="Insert" Width="600px" BorderStyle="None" 
                        DataKeyNames="ID" BorderWidth="0px" OnItemInserting="ThumbnailsDetailsView_ItemInserting" 
                        OnItemDeleted="ThumbnailsDetailsView_ItemDeleted" GridLines="None" 
                            OnItemInserted="ThumbnailsDetailsView_ItemInserted">
                        <Fields>
                            <asp:TemplateField HeaderText="File Name">
                                <HeaderStyle Width="100px" HorizontalAlign="Right" CssClass="wizard_detailsview_header_style"/>
                                <ItemStyle Width="500px" />
                                <HeaderTemplate>
                                    <asp:Label ID="FileNameTitle" runat="server" CssClass="wizard_title" Text="<%$ Resources:LocalizedText, FileName %>"></asp:Label>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="FileNameEditTextBox" runat="server" CssClass="wizard_text" Text='<%# Bind("FileName") %>'
                                        ValidationGroup="Files"></asp:TextBox><asp:RequiredFieldValidator ID="FileNameEditValidator"
                                            runat="server" ControlToValidate="FileNameEditTextBox" CssClass="validator" Display="Dynamic"
                                            Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="FileNameInsertTextBox" runat="server" CssClass="wizard_text" Text='<%# Bind("FileName") %>' ValidationGroup="Files"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="FileNameValidator" runat="server" CssClass="validator"
                                        Display="Dynamic" ControlToValidate="FileNameInsertTextBox" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <HeaderStyle Width="100px" HorizontalAlign="Right" CssClass="wizard_detailsview_header_style"/>
                                <ItemStyle Width="500px" />
                                <HeaderTemplate>
                                    <asp:Label ID="DescriptionTitleLabel" runat="server" CssClass="wizard_text" Text='<%$ Resources:LocalizedText, Description %>'></asp:Label>
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="DescriptionEditTextBox" runat="server" CssClass="wizard_text" Text='<%# Bind("Description") %>'
                                        ValidationGroup="Files"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="DescriptionEditValidator" runat="server" ControlToValidate="DescriptionEditTextBox"
                                        CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="DescriptionInsertTextBox" runat="server" CssClass="wizard_text"
                                        Text='<%# Bind("Description") %>' ValidationGroup="Files"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="DescriptionValidator" runat="server" ControlToValidate="DescriptionInsertTextBox"
                                        CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File">
                                <HeaderStyle Width="100px" HorizontalAlign="Right" CssClass="wizard_detailsview_header_style"/>
                                <ItemStyle Width="500px" />
                                <HeaderTemplate>
                                    <asp:Label ID="FileHeaderLabel" runat="server" CssClass="wizard_text" Text="<%$ Resources:LocalizedText, File %>"></asp:Label>
                                </HeaderTemplate>
                                <InsertItemTemplate>
                                    <asp:FileUpload runat="server" ID="ProjectFileUpload" CssClass="wizard_text" />
                                    <asp:RequiredFieldValidator ID="FileUploadValidator" runat="server" ControlToValidate="ProjectFileUpload"
                                        CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions">
                                <HeaderStyle Width="100px" HorizontalAlign="Right" CssClass="wizard_detailsview_header_style"/>
                                <ItemStyle Width="500px" />
                                <HeaderTemplate>
                                    &nbsp;
                                </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="SaveLinkButton" runat="server" CommandName="Update"
                                        CssClass="wizard_link" Text="<%$ Resources:LocalizedText, Save %>"></asp:LinkButton>
                                    <asp:LinkButton ID="CancelLinkButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                        CssClass="wizard_link" Text="<%$ Resources:LocalizedText, Cancel %>"></asp:LinkButton>
                                    <asp:LinkButton ID="DeleteLinkButton" runat="server" CausesValidation="False" CommandName="Delete"
                                        CssClass="wizard_link" Text="<%$ Resources:LocalizedText, Delete %>"></asp:LinkButton>
                                    <asp:LinkButton ID="NewLinkButton" runat="server" CausesValidation="False" CommandName="New"
                                        CssClass="wizard_link" Text="<%$ Resources:LocalizedText, New %>"></asp:LinkButton>&nbsp;
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:LinkButton ID="InsertLinkButton" runat="server" CommandName="Insert"
                                        CssClass="wizard_link" Text="<%$ Resources:LocalizedText, Insert %>"></asp:LinkButton>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                    </div>
                    <div class="wizard_step_row">
                    </div>
                    <div class="wizard_step_row">
                        <asp:DataList ID="ThumbnailsDataList" runat="server" DataSourceID="ThumbnailsDataSource"
                            RepeatLayout="Flow" CellPadding="0" RepeatColumns="4" 
                            OnSelectedIndexChanged="ThumbnailsDataList_SelectedIndexChanged" Width="679px">
                            <ItemTemplate>
                                <div class="wizard_thumbnail">
                                    <asp:ImageButton ID="ThumbnailImageButton" ImageUrl='<%# Eval("ThumbnailLink") %>' runat="server" Height="115px" Width="100px" CommandName="Select" CausesValidation="False" /><br />
                                    <asp:Label ID="FileNameTitle" runat="server" Font-Bold="true" Text="<%$ Resources:LocalizedText, FileName %>"></asp:Label>:&nbsp;
                                    <asp:Label ID="FileNameLabel" runat="server" Text='<%# Eval("FileName") %>'></asp:Label><br />
                                    <asp:Label ID="DescriptionTitle" runat="server" Font-Bold="true" Text="<%$ Resources:LocalizedText, Description %>"></asp:Label>:&nbsp;
                                    <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>'></asp:Label><br />
                                    <asp:Label ID="ContentLengthTitle" runat="server" Font-Bold="true" Text="<%$ Resources:LocalizedText, ContentLength %>"></asp:Label>:&nbsp;
                                    <asp:Label ID="ContentLengthLabel" runat="server" Text='<%# Eval("ContentLength") %>'></asp:Label><br />
                                    <asp:Label ID="ContentTypeTitle" runat="server" Font-Bold="true" Text="<%$ Resources:LocalizedText, ContentType %>"></asp:Label>:&nbsp;
                                    <asp:Label ID="ContentTypeLabel" runat="server" Text='<%# Eval("ContentType") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="CommentsStep" runat="server" StepType="Step" Title="<%$ Resources:CommentsStep.Title %>">
                <div class="wizard_step">
                <asp:DetailsView ID="CommentsDetailsView" runat="server" AutoGenerateRows="False"
                    DataSourceID="CommentsDataSource" DefaultMode="Insert" Height="50px" OnItemInserted="CommentsDetailsView_ItemInserted"
                    Width="696px" BorderStyle="None" BorderWidth="0px" CssClass="wizard_text" GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="Title" SortExpression="Title">
                            <HeaderStyle Width="100px" HorizontalAlign="Right" CssClass="wizard_detailsview_header_style"/>
                            <ItemStyle Width="500px" />
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="TitleInsertTextBox" runat="server" Text='<%# Bind("Title") %>' MaxLength="100" Width="232px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="TitleInsertValidator" runat="server" ControlToValidate="TitleInsertTextBox"
                                    CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"
                                    ValidationGroup="InsertComment"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Content" SortExpression="Content">
                            <HeaderStyle Width="100px" HorizontalAlign="Right" CssClass="wizard_detailsview_header_style"/>
                            <ItemStyle Width="500px" />
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="ContentInsertTextBox" runat="server" Text='<%# Bind("Content") %>' MaxLength="1024" 
                                onkeypress="if(this.value.length>=100) return false;" Rows="7" TextMode="MultiLine" Width="436px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="CommentValidator" runat="server" ControlToValidate="ContentInsertTextBox"
                                        CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="ContentInsertTextBox"
                                            CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"
                                            ValidationExpression="^[\s\S]{1,1024}$"></asp:RegularExpressionValidator>
                            </InsertItemTemplate>
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle Width="100px" HorizontalAlign="Right" CssClass="wizard_detailsview_header_style"/>
                            <ItemStyle Width="500px" />
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <InsertItemTemplate>
                                <asp:LinkButton ID="InsertLinkButton" runat="server" CommandName="Insert" Text="<%$ Resources:LocalizedText, Insert %>"></asp:LinkButton>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView><div style="display: block; float: none; width: 696px; height: 18px">
                </div>
                <asp:GridView ID="CommentsGridView" runat="server" AutoGenerateColumns="False" DataSourceID="CommentsDataSource"
                    AllowPaging="True" DataKeyNames="DateCreated" BorderStyle="Solid" BorderWidth="1px" CssClass="wizard_text" Width="696px" BorderColor="White">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ValidationGroup="SingleComment" />
                        <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                        <asp:BoundField DataField="Content" HeaderText="Content" SortExpression="Content" />
                        <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" ReadOnly="True" />
                        <asp:BoundField DataField="Author" HeaderText="Author" SortExpression="Author" ReadOnly="True" />
                        <asp:BoundField DataField="IPAddress" HeaderText="IPAddress" SortExpression="IPAddress" ReadOnly="True" />
                    </Columns>
                    <EmptyDataTemplate>
                        &nbsp;
                    </EmptyDataTemplate>
                </asp:GridView>
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="UsersStep" runat="server" StepType="Step" Title="<%$ Resources:UsersStep.Title %>">
                <div style="display: block; float: left; width: 190px; height: 300px">
                    <asp:ListBox ID="AllUsersList" runat="server" Height="280px" Width="150px" DataSourceID="AllUsersDataSource" DataTextField="UserName" DataValueField="UserName"></asp:ListBox>
                </div>
                <div style="display: block; float: left; width: 115px; height: 300px; text-align: left">
                    <asp:Button ID="AddUserButton" runat="server" Text="<%$ Resources:LocalizedText, Add %>" CausesValidation="False" Width="75px" OnClick="AddUserButton_Click" />
                    <br />
                    <asp:Button ID="RemoveUserButton" runat="server" Text="<%$ Resources:LocalizedText, Remove %>" CausesValidation="False" Width="75px" OnClick="RemoveUserButton_Click" />
                </div>
                <div style="display: block; float: left; width: 200px; height: 300px">
                    <asp:ListBox ID="UsersListBox" runat="server" Height="280px" Width="150px" DataSourceID="UsersDataSource"></asp:ListBox>
                </div>
            </asp:WizardStep>
            <asp:WizardStep runat="server" StepType="Finish" Title="<%$ Resources:CompletedStep.Title %>" ID="CompleteStep">
                <div class="wizard_step">
                    <div class="wizard_step_row" style="height: 40px">
                        <asp:CheckBox ID="VisibleCheckBox" runat="server" Text="<%$ Resources:LocalizedText, IsVisible %>" CssClass="wizard_title" Checked="True"/></div>
                    <div class="wizard_step_row">
                        <asp:Label ID="StatusLabel" runat="server" Text="Status" CssClass="wizard_title"/></div>
                    <div class="wizard_step_row" style="height: 70px">
                        <asp:DropDownList ID="ProjectStatusList" runat="server" Width="175px">
                            <asp:ListItem Value="0">Dise&#241;o</asp:ListItem>
                            <asp:ListItem Value="1">Modificaciones</asp:ListItem>
                            <asp:ListItem Value="2">Autorizado</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="StatusValidator" runat="server" ControlToValidate="ProjectStatusList"
                            CssClass="validator" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="wizard_step_row" style="height: 40px">
                        <asp:Label ID="CompleteLabel" runat="server" Text="<%$ Resources:WizardCompleted.Text %>" CssClass="wizard_title" Height="34px" Width="528px" OnPreRender="CompleteLabel_PreRender"></asp:Label></div>
                </div>
            </asp:WizardStep>
        </WizardSteps>
        <StartNavigationTemplate>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 546px">
                <tr>
                    <td style="width: 90px"></td>
                    <td style="width: 90px"></td>
                    <td></td>
                    <td style="width: 90px"></td>
                    <td style="width: 90px">
                        <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="<%$ Resources:LocalizedText, Next %>" Width="80px" /></td>
                    <td style="width: 90px">
                        <asp:Button ID="CancelButton" CausesValidation="False" runat="server" CommandName="Cancel" Text="<%$ Resources:LocalizedText, Cancel %>" Width="80px"/></td>
                </tr>
            </table>
        </StartNavigationTemplate>
        <FinishNavigationTemplate>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 546px">
                <tr>
                    <td style="width: 90px">
                        <asp:Button ID="DeleteButton" runat="server" OnClick="DeleteButton_Click" Text="<%$ Resources:LocalizedText, Delete %>"
                            Width="80px" OnPreRender="DeleteButton_PreRender" OnClientClick="javascript:return ConfirmDeletion();" /></td>
                    <td style="width: 90px">
                        </td>
                    <td>
                    </td>
                    <td style="width: 90px">
                        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious" Text="<%$ Resources:LocalizedText, Previous %>" Width="80px" /></td>
                    <td style="width: 90px">
                        <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="<%$ Resources:LocalizedText, Finish %>" Width="80px" OnPreRender="FinishButton_PreRender" /></td>
                    <td style="width: 90px">
                        <asp:Button ID="CancelButton" CausesValidation="False" runat="server" CommandName="Cancel" Text="<%$ Resources:LocalizedText, Cancel %>" Width="80px"/></td>
                </tr>
            </table>
        </FinishNavigationTemplate>
        <StepNavigationTemplate>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 546px">
                <tr>
                    <td style="width: 90px; height: 21px;"></td>
                    <td style="width: 90px; height: 21px;"></td>
                    <td style="height: 21px"></td>
                    <td style="width: 90px; height: 21px;"></td>
                    <td style="width: 90px; height: 21px;">
                        <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious" Text="<%$ Resources:LocalizedText, Previous %>" Width="80px" /></td>
                    <td style="width: 90px; height: 21px;">
                        <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="<%$ Resources:LocalizedText, Next %>" Width="80px" CausesValidation="False" /></td>
                </tr>
            </table>
        </StepNavigationTemplate>
    </asp:Wizard>
    <asp:ObjectDataSource ID="ThumbnailsDataSource" runat="server" DataObjectTypeName="CodeFactory.Web.Storage.UploadedFile"
        DeleteMethod="DeleteFile" InsertMethod="InsertFile" OnObjectCreating="ProjectDataSource_ObjectCreating"
        SelectMethod="GetFiles" TypeName="UploadedFilesResult" UpdateMethod="UpdateFile"
        SelectCountMethod="TotalCount">
        <UpdateParameters>
            <asp:Parameter DbType="Guid" Name="id" />
        </UpdateParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="CommentsDataSource" runat="server" DataObjectTypeName="CodeFactory.Gallery.Core.Comment"
        DeleteMethod="DeleteComment" InsertMethod="InsertComment" SelectMethod="GetComments"
        TypeName="CommentsResult" UpdateMethod="UpdateComment" OnObjectCreating="CommentsDataSource_ObjectCreating" EnablePaging="True" SelectCountMethod="TotalCount">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="AllUsersDataSource" runat="server" SelectMethod="GetUsers"
        TypeName="UsersResult"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="UsersDataSource" runat="server" SelectMethod="GetUsers"
        TypeName="ProjectUsersResult" OnObjectCreating="UsersDataSource_ObjectCreating"></asp:ObjectDataSource>
</div>
<script type="text/javascript" language="javascript">
    function ConfirmDeletion()
    {
        return confirm("¿Está seguro que desea eliminar el proyecto?");
    }
</script>
</asp:Content>
