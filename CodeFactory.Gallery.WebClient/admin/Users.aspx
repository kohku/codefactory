<%@ Page Language="C#" MasterPageFile="~/themes/CodeFactoryDefault/CodeFactoryDefault.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="admin_Users" Title="CodeFactory - Administración de Usuarios" %>
<asp:Content ID="Main" ContentPlaceHolderID="Main" Runat="Server">
    &nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:TitlePageUsers %>" CssClass="label"></asp:Label>
    <asp:DetailsView ID="MembershipUserDetailsView" runat="server" AutoGenerateRows="False"
        DataSourceID="MembershipUsersDataSource" DefaultMode="Insert" Height="50px" Width="125px" DataKeyNames="UserName" OnItemInserting="MembershipUserDetailsView_ItemInserting" OnItemInserted="MembershipUserDetailsView_ItemInserted" OnPageIndexChanging="MembershipUserDetailsView_PageIndexChanging">
        <Fields>
            <asp:TemplateField HeaderText="UserName" SortExpression="UserName">
                <EditItemTemplate>
                    &nbsp;
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UserName") %>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
                        Display="Dynamic" ValidationGroup="insert" CssClass="validator" Text="<%$ Resources:Required %>"></asp:RequiredFieldValidator>
                </InsertItemTemplate>
                <ItemTemplate>
                    &nbsp;
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email" SortExpression="Email">
                <EditItemTemplate>
                    &nbsp; &nbsp;
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBox2"
                        Display="Dynamic" ErrorMessage="<%$ Resources:BadMail %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ValidationGroup="insert" CssClass="validator"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox2"
                        Display="Dynamic" ErrorMessage="<%$ Resources:Required %>" ValidationGroup="insert" CssClass="validator"></asp:RequiredFieldValidator>
                </InsertItemTemplate>
                <ItemTemplate>
                    &nbsp;
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Comment") %>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Comment") %>'></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Comment") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Password">
                <InsertItemTemplate>
                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox><br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="PasswordTextBox"
                        Display="Dynamic" ErrorMessage="<%$ Resources:BadPassword %>" ValidationExpression="\w{7,10}"
                        ValidationGroup="insert" CssClass="validator"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="PasswordTextBox"
                        Display="Dynamic" ErrorMessage="<%$ Resources:Required %>" ValidationGroup="insert" CssClass="validator"></asp:RequiredFieldValidator>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <InsertItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Insert"
                        Text="<%$ Resources:LinkInsertUser %>" ValidationGroup="insert" CssClass="wizard_link"></asp:LinkButton>&nbsp;
                </InsertItemTemplate>
            </asp:TemplateField>
        </Fields>
        <FieldHeaderStyle CssClass="wizard_text" />
    </asp:DetailsView>
    <asp:Label ID="Label4" runat="server" Text="Label" CssClass="validator"></asp:Label>&nbsp;
    <br />
    <asp:GridView ID="MembershipUsersGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataSourceID="MembershipUsersDataSource" DataKeyNames="UserName" OnRowUpdating="MembershipUsersGridView_RowUpdating" OnSelectedIndexChanged="MembershipUsersGridView_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" SelectText="Rol" />
            <asp:TemplateField HeaderText="UserName" SortExpression="UserName">
                <EditItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />
            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="IsLockedOut" SortExpression="IsLockedOut" />
            <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" ReadOnly="True"
                SortExpression="CreationDate" />
            <asp:BoundField DataField="LastLoginDate" HeaderText="LastLoginDate" ReadOnly="True"
                SortExpression="LastLoginDate" />
            <asp:BoundField DataField="LastActivityDate" HeaderText="LastActivityDate" ReadOnly="True"
                SortExpression="LastActivityDate" />
        </Columns>
        <RowStyle CssClass="wizard_text" />
        <HeaderStyle CssClass="wizard_title" />
        <PagerStyle CssClass="wizard_link" />
    </asp:GridView>
    <br />
    <asp:ObjectDataSource ID="MembershipUsersDataSource" runat="server" DataObjectTypeName="CodeFactoryUser"
        DeleteMethod="DeleteUser" EnablePaging="True" InsertMethod="InsertUser" SelectCountMethod="TotalCount"
        SelectMethod="GetUsers" TypeName="UsersResult" UpdateMethod="UpdateUser">
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="username" Type="String" />
            <asp:Parameter Name="password" Type="String" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:GridView ID="UsersRoleGridView" runat="server" AutoGenerateColumns="False" OnRowDataBound="UsersRoleGridView_RowDataBound" CssClass="wizard_title">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:TitleInRoles %>" SortExpression="Length">
                <ItemTemplate>
                    <asp:CheckBox ID="RoleCheckBox" runat="server" Text='<%# Container.DataItem %>' AutoPostBack="true" OnCheckedChanged="RoleCheckBox_CheckedChanged"></asp:CheckBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="wizard_text" />
    </asp:GridView>
    <asp:ObjectDataSource ID="RolesDataSource" runat="server" DeleteMethod="DeleteRole"
        InsertMethod="InsertRole" SelectMethod="GetRoles" TypeName="RolesResult">
        <DeleteParameters>
            <asp:Parameter Name="role" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="role" Type="String" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <br />
    <br />
    <asp:Button ID="BackButton" runat="server" OnClick="BackButton_Click" Text="<%$ Resources:BackMenu.Text %>"
        Width="150px" />
</asp:Content>

