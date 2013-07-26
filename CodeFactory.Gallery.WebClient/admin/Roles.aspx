<%@ Page Language="C#" MasterPageFile="~/themes/CodeFactoryDefault/CodeFactoryDefault.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="admin_Roles" Title="CodeFactory - Administración de Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
// <!CDATA[

function Checkbox1_onclick() {

}

// ]]>
</script>
    <asp:Label ID="Label2" runat="server" CssClass="label" Text="<%$ Resources:TitleManageRoles %>"></asp:Label><br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="RolesDataSource" OnRowDeleting="GridView1_RowDeleting">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:TemplateField HeaderText="<%$ Resources:RoleName %>" SortExpression="Length">
                <EditItemTemplate>
                    &nbsp;
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItem %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="wizard_text" />
        <HeaderStyle CssClass="wizard_title" />
    </asp:GridView>
    &nbsp; &nbsp;
    &nbsp; &nbsp;
    <asp:ObjectDataSource ID="UsersDataSource" runat="server" SelectMethod="GetUsers"
        TypeName="UsersResult"></asp:ObjectDataSource>
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataSourceID="RolesDataSource"
        DefaultMode="Insert" Height="50px" Width="125px" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:TemplateField HeaderText="Length" SortExpression="Length">
                <HeaderTemplate>
                    Role
                </HeaderTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Container.DataItem %>'></asp:TextBox>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <InsertItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert"
                        Text="Insert"></asp:LinkButton>&nbsp;
                </InsertItemTemplate>
                <HeaderStyle CssClass="wizard_title" />
            </asp:TemplateField>
        </Fields>
        <RowStyle CssClass="wizard_text" />
    </asp:DetailsView>
    <asp:ObjectDataSource ID="RolesDataSource" runat="server" DeleteMethod="DeleteRole"
        InsertMethod="InsertRole" SelectMethod="GetRoles" TypeName="RolesResult">
        <DeleteParameters>
            <asp:Parameter Name="role" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="role" Type="String" />
        </InsertParameters>
    </asp:ObjectDataSource>

</asp:Content>
