<%@ Page Language="C#" MasterPageFile="~/themes/CodeFactoryDefault/CodeFactoryDefault.master"
    AutoEventWireup="true" CodeFile="Project.aspx.cs" Inherits="_Project" Title="Gallery" Async="true" %>

<%@ Register Src="Gallery.ascx" TagName="Gallery" TagPrefix="uc1" %>
<asp:Content ID="Main" ContentPlaceHolderID="Main" runat="Server">
    <table id="table_main" style="width: 967px; vertical-align: top;" cellpadding="0"
        cellspacing="10">
        <tbody>
            <tr>
                <td style="text-align: center;" valign="top">
                    <div id="project_title" style="width: 626px">
                        <div style="width: 294px; margin-left: 38px;">
                            <asp:Image ID="Image4" runat="server" ImageUrl="~/images/proyecto.jpg" /></div><div style="width: 294px; margin-left: 38px;">
                                &nbsp;<asp:Label ID="ProjectTitleLabel" runat="server"></asp:Label></div>
                    </div>
                    <div id="gallery" style="width: 626px">
                        <uc1:Gallery ID="GalleryControl" runat="server" />
                    </div>
                    <div id="history" style="width: 626px; text-align: center;">
                        <div class="history_separator" style="width: 626px;">
                            <asp:Image ID="HistorySeparator" runat="server" Height="4px" ImageUrl="~/images/linea.jpg" Width="557px" /></div>
                        <div id="project_status" style="width: 546px; height: 58px;">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 546px">
                                <tr>
                                    <td style="width: 200px; height: 30px;" valign="top">
                                        <asp:Image ID="ProjectStatusImage" runat="server" ImageUrl="~/images/Statusproyecto.jpg" ImageAlign="Left" /></td>
                                    <td style="width: 330px; height: 30px;" valign="top">
                                        &nbsp;<asp:Button ID="ManageButton" runat="server" Text="Administrar" OnClick="ManageButton_Click" Width="100px" /></td>
                                    <td style="height: 30px" valign="top">&nbsp;</td>                                    
                                </tr>
                            </table>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 546px">
                                <tr>
                                    <td style="text-align: left; width: 182px;">
                                <asp:CheckBox ID="DesingCheckBox" runat="server" Text="Diseño" CssClass="project_status_enum" EnableViewState="False" /></td>
                                    <td style="text-align: left; width: 182px;">
                                <asp:CheckBox ID="ChangesCheckBox" runat="server" Text="<%$ Resources:ChangesStatus.Text %>" CssClass="project_status_enum" EnableViewState="False" /></td>
                                    <td style="text-align: left;">
                                <asp:CheckBox ID="AuthorizedCheckBox" runat="server" Text="<%$ Resources:AuthorizedStatus.Text %>" CssClass="project_status_enum" EnableViewState="False" /></td>
                                </tr>
                            </table>
                        </div>
                        <div class="history_separator" style="width: 626px;">
                            <asp:Image ID="Separator1Image" runat="server" Height="4px" ImageUrl="~/images/linea.jpg" Width="557px" /></div>
                        <asp:GridView ID="CommentsGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataSourceID="CommentsDataSource" PageSize="5" Width="626px" BorderStyle="None"
                            BorderWidth="0px" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="Comentarios" SortExpression="Title">
                                    <HeaderTemplate>
                                        <div id="project_comment_header" style="height: 30px; width: 540px;"><asp:Label ID="CommentsTitleLabel" runat="server" Text="<%$ Resources:CommentsTitle.Text %>"></asp:Label></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div id="project_comment_title" style="height: 30px; width: 546px;"><asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>'></asp:Label>&nbsp;by&nbsp;<asp:Label ID="AuthorLabel" runat="server" Text='<%# Eval("Author") %>'></asp:Label></div>
                                        <div id="project_comment_content" style="width: 546px">
                                            <asp:Label ID="ContentLabel" runat="server" Text='<%# Eval("Content") %>'></asp:Label></div>
                                        <div class="project_comment_emptyrow" style="width: 546px">&nbsp;</div>
                                        <div id="project_comment_datecreated" style="width: 546px">
                                            <asp:Label ID="DateCreatedLabel" runat="server" Text='<%# Eval("DateCreated", "{0:D}") %>'></asp:Label>&nbsp;|
                                            <asp:Label ID="TimeCreatedLabel" runat="server" Text='<%# Eval("DateCreated", "{0:t}") %>'></asp:Label></div>
                                        <div class="project_comment_emptyrow" style="width: 626px">
                                            <asp:Image ID="Separator2Image" runat="server" Height="4px" ImageUrl="~/images/linea.jpg"
                                                Width="557px" /></div>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="project_comment_paging" HorizontalAlign="Center" />
                        </asp:GridView></div>
                </td>
                <td valign="top">
                    <div id="comment_bar" style="width: 297px; height: 450px">
                        <div id="comments_title" style="width: 286px; height: 39px;">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/comentarios.jpg" /></div>
                        <table style="width: 295px">
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="TitleLabel" runat="server" Text="<%$ Resources:Title.Text %>" CssClass="label"></asp:Label>
                                    <asp:RequiredFieldValidator
                                        ID="TitleValidator" runat="server" ControlToValidate="TitleTextBox" CssClass="validator" ValidationGroup="InsertComment" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:TextBox ID="TitleTextBox" runat="server" Width="232px" MaxLength="100" ValidationGroup="InsertComment"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="ContentLabel" runat="server" Text="<%$ Resources:CommentsText.Text %>" CssClass="label"></asp:Label>
                                    <asp:RequiredFieldValidator ID="CommentValidator" runat="server" ControlToValidate="ContentTextBox" CssClass="validator" ValidationGroup="InsertComment" Display="Dynamic" Text="<%$ Resources:RequiredField %>"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="validator"
                                        Display="Dynamic" ControlToValidate="ContentTextBox" ValidationExpression="^[\s\S]{1,1024}$" ValidationGroup="InsertComment" Text="<%$ Resources:RequiredField %>"></asp:RegularExpressionValidator></td>
                            </tr>
                            <tr>
                                <td colspan="3" valign="top" style="vertical-align: top">
                                    <asp:TextBox ID="ContentTextBox" runat="server" Rows="7" TextMode="MultiLine" Width="280px" MaxLength="1024" ValidationGroup="InsertComment" onkeypress="if(this.value.length>=100) return false;"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 180px">
                                </td>
                                <td style="width: 18px">
                                </td>
                                <td style="width: 95px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px; text-align: right">
                                    <asp:Button ID="SendButton" runat="server" Text="<%$ Resources:SendAction.Text %>" ValidationGroup="InsertComment" OnClick="SendButton_Click" Width="75px" /></td>
                                <td style="width: 18px; text-align: center">
                                    |</td>
                                <td style="width: 95px">
                                    <input id="Reset" type="reset" value="<%$ Resources:ResetAction.Text %>" runat="server" style="width: 75px"/></td>
                            </tr>
                            <tr>
                                <td style="width: 180px">
                                </td>
                                <td style="width: 18px">
                                </td>
                                <td style="width: 95px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <img height="1" src="images/lineaH.jpg" width="235" alt="" /></td>
                            </tr>
                            <tr>
                                <td style="width: 180px; text-align: right">
                                </td>
                                <td style="width: 18px; text-align: center">
                                </td>
                                <td style="width: 95px">
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left" colspan="3">
                                    <asp:Label ID="MessagesBoard" runat="server" Visible="False" CssClass="messagesBoard" Text="<%$ Resources:MessagesBoard.Text %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 180px; text-align: right; height: 118px;">
                                </td>
                                <td style="width: 18px; text-align: center; height: 118px;">
                                </td>
                                <td style="height: 118px; width: 95px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px; text-align: right">
                                    <asp:Button ID="BackButton" runat="server" OnClick="BackButton_Click" Text="<%$ Resources:BackMenu.Text %>" Width="150px" /></td>
                                <td style="width: 18px; text-align: center">
                                </td>
                                <td style="width: 95px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px; text-align: right">
                                    </td>
                                <td style="width: 18px; text-align: center">
                                </td>
                                <td style="width: 95px">
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:ObjectDataSource ID="CommentsDataSource" runat="server" EnablePaging="True"
        OnObjectCreating="CommentsDataSource_ObjectCreating" SelectCountMethod="TotalCount"
        SelectMethod="GetComments" TypeName="CommentsResult">
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
