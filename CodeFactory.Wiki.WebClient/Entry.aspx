<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Entry.aspx.cs" Inherits="Entry"
    Theme="Default" StylesheetTheme="Default" ValidateRequest="false" MasterPageFile="~/WikiMaster.master" %>

<%@ Register Assembly="CodeFactory.Web" Namespace="CodeFactory.Web.Controls" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="head" ID="header" runat="server">
    <style type="text/css">
        #wiki
        {
            display: block;
            float: none;
            padding-top: 10px;
            padding-bottom: 10px;
        }
        #wiki #controls
        {
            display: block;
            float: none;
            width: 731px;
            padding-left: 18px;
        }
        #wiki #content
        {
            display: block;
            float: none;
            text-align: left;
        }
        #wiki #content #top
        {
            display: block;
            float: none;
            width: 770px;
        }
        #wiki #content #main
        {
            display: block;
            float: none;
            width: 770px;
            background-image: url(<%= CodeFactory.Web.Utils.RelativeWebRoot + "images/fondo_tabla.jpg" %>);
            background-repeat: repeat-y;
            padding-left: 7px;
        }
        #wiki #content #bottom
        {
            display: block;
            float: none;
            width: 770px;
        }
        #wiki #content #main .maintitle
        {
            display: block;
            float: none;
            width: 745px;
            background-color: #e9e9e9;
            border: solid 1px #dddddd;
            padding: 5px 5px 5px 5px;
            margin: 5px 0px 5px 0px;
        }
        #wiki #content #main #container
        {
            width: 731px;
            text-align: left;
        }
        #wiki #content #main #container #left
        {
            width: 533px;
            vertical-align: top;
        }
        #wiki #content #main #container #middle
        {
            width: 18px;
        }
        #wiki #content #main #container #right
        {
            width: 180px;
            vertical-align: top;
            background-color: #F0F0F0;
        }
        
        #wiki #controls .timer
        {
        	padding-left: 429px;
        }
        
        .descriptionLabel
        {
            width: 160px;
        }
        .style5
        {
            width: 358px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainPlaceHolder" ID="MainHolder" runat="server">
    <asp:ScriptManager ID="TheScriptManager" runat="server">
    </asp:ScriptManager>
    <div id="wiki">
        <div id="controls">
            <asp:Button ID="ContentButton" runat="server" SkinID="Button_Standard" Text="Contenido"
                OnClick="ContentButton_Click" CausesValidation="False" />
            <asp:Button ID="EditButton" runat="server" SkinID="Button_Standard" Text="Editar"
                CausesValidation="False" />
            <asp:UpdatePanel ID="TheUpdatePanel" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Label runat="server" ID="TimeStamp" Text="DateTime.Now" CssClass="timer"></asp:Label>
                    <asp:Timer ID="TheTimer" runat="server">
                    </asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="content">
            <div id="top">
                <asp:Image ID="ContentTopImage" runat="server" ImageUrl="~/images/fondo_tabla_top.jpg" /></div>
            <div id="main">
                <div class="maintitle">
                    <span class="titles_green">En esta sección puedes editar el contenido</span></div>
                <table style="width: 755px;">
                    <tr>
                        <td style="width: 85px;">
                            <asp:Label ID="TitleLabel" runat="server" Text="Título/Palabra"></asp:Label>
                        </td>
                        <td style="width: 355px;">
                            <asp:Label ID="MessagesBoardLabel" runat="server" ForeColor="Red"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TitleTextBox"
                                ErrorMessage="&nbsp;Introduce un título." Display="Dynamic">Introduce un título.</asp:RequiredFieldValidator>
                            <asp:TextBox ID="TitleTextBox" runat="server" SkinID="TextBox_Standard" Width="342px"
                                MaxLength="256"></asp:TextBox>
                        </td>
                        <td style="width: 100px;">
                            <asp:Label ID="AuthorLabel" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="DateLabel" runat="server" Text="Fecha {0}"></asp:Label>
                        </td>
                    </tr>
                </table>
                <cc1:HtmlEditor ID="ContentHtmlEditor" runat="server" Height="250px" Plugins="safari,style,layer,table,save,advhr,advimage,advlink,inlinepopups,media,searchreplace,contextmenu,paste,directionality,fullscreen,noneditable,xhtmlxtras"
                    ThemeAdvancedButtons1="fullscreen,code,|,cut,copy,paste,|,undo,redo,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist,outdent,indent,|,formatselect,fontselect,fontsizeselect"
                    ThemeAdvancedButtons2="search,replace,|,link,unlink,anchor,image,|,forecolor,backcolor,|,tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,media,advhr,|,styleprops"
                    ThemeAdvancedButtons3="" ThemeAdvancedButtons4="" ThemeAdvancedResizing="False"
                    Width="755px" Language="es" />
                <div class="maintitle">
                    <span class="titles_green">Especifica las características del contenido</span></div>
                <table style="text-align: left; width: 755px;">
                    <tr>
                        <td class="descriptionLabel">
                            <asp:Label ID="DescriptionLabel" runat="server" Text="Breve descripción"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="DescriptionTextBox" runat="server" Rows="3" SkinID="TextBox_Standard"
                                TextMode="MultiLine" Width="423px" MaxLength="1024"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="DescriptionValidator" runat="server" ControlToValidate="DescriptionTextBox"
                                ErrorMessage="*">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="descriptionLabel">
                            <asp:Label ID="CategoryLabel" runat="server" Text="Categoría"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="CategoryList" runat="server" SkinID="DropDownList_Standard"
                                Width="200px" DataSourceID="CategoriesDataSource" DataTextField="Key" DataValueField="Value">
                                <asp:ListItem Selected="True">Seleccione...</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="CategoryValidator" runat="server" ControlToValidate="CategoryList"
                                Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                            <asp:Label ID="SpecifyLabel" runat="server" Text="&amp;nbsp;Especifica:"></asp:Label>
                            <asp:TextBox ID="SpecifyTextBox" runat="server" SkinID="TextBox_Standard" Width="151px"></asp:TextBox>
                            <asp:CustomValidator ID="SpecifyValidator" runat="server" ClientValidationFunction="validateothercategories"
                                ControlToValidate="SpecifyTextBox" Display="Dynamic" ErrorMessage="*" OnServerValidate="SpecifyValidator_ServerValidate"
                                ValidateEmptyText="True">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="descriptionLabel">
                            <asp:Label ID="ReachLevelLabel" runat="server" Text="El contenido se publicará en"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:RadioButtonList ID="ReachLevelRadioButtonList" runat="server" RepeatDirection="Horizontal"
                                Width="420px">
                                <asp:ListItem Value="0" Selected="True">Intranet</asp:ListItem>
                                <asp:ListItem Value="1">System</asp:ListItem>
                                <asp:ListItem Value="2">Internet</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="EditableTableRow" runat="server">
                        <td class="descriptionLabel">
                            <asp:Label ID="EditableLabel" runat="server" Text="Editable"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:CheckBox ID="EditableCheckBox" runat="server" Checked="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="descriptionLabel">
                            <asp:Label ID="KeywordsLabel" runat="server" Text="Palabras Clave"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="KeywordsTextBox" runat="server" SkinID="TextBox_Standard" Width="420px"
                                MaxLength="1024"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="KeywordsValidator" runat="server" ControlToValidate="KeywordsTextBox"
                                Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="KeywordHelperLabel" runat="server" SkinID="Label_Standard" Text="Separa cada palabra o frase clave por un punto y coma."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="descriptionLabel">
                            <asp:Label ID="EditorInputLabel" runat="server" Text="Autor"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="EditorTextBox" runat="server" SkinID="TextBox_Standard" Width="420px"
                                MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EditorValidator" runat="server" ControlToValidate="EditorTextBox"
                                Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="descriptionLabel">
                            <asp:Label ID="AreaInputLabel" runat="server" Text="Área"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="AreaTextBox" runat="server" SkinID="TextBox_Standard" Width="420px"
                                MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="AreaValidator" runat="server" ControlToValidate="AreaTextBox"
                                Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <div class="maintitle">
                    <span class="titles_green">Agrega imágenes o carga un archivo</span></div>
                <table style="text-align: left; width: 755px;">
                    <tr>
                        <td class="descriptionLabel">
                            <span>
                                <asp:Label ID="UploadImageLabel" runat="server" Text="Cargar Imagen"></asp:Label>
                            </span>
                        </td>
                        <td style="text-align: left;" class="style5">
                            <asp:FileUpload ID="ImageUpload" runat="server" SkinID="FileUpload_Standard" Width="324px" />
                            <asp:RequiredFieldValidator ID="ImageUploadValidator" runat="server" ControlToValidate="ImageUpload"
                                Display="Dynamic" ValidationGroup="ImageUpload">*&nbsp;&nbsp;</asp:RequiredFieldValidator>
                        </td>
                        <td style="text-align: left;">
                            <asp:Button ID="UploadImageButton" runat="server" OnClick="UploadImageButton_Click"
                                SkinID="Button_Standard" Text="Agregar" ValidationGroup="ImageUpload" />
                            <asp:Label ID="ImageUploadBoard" runat="server" ForeColor="Red" SkinID="Label_Standard"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="descriptionLabel">
                            <asp:Label ID="Label2" runat="server" Text="Cargar Archivo"></asp:Label>
                        </td>
                        <td style="text-align: left;" class="style5">
                            <asp:FileUpload ID="FileUpload" runat="server" SkinID="FileUpload_Standard" Width="324px" />
                            <asp:RequiredFieldValidator ID="FileUploadValidator" runat="server" ControlToValidate="FileUpload"
                                Display="Dynamic" ValidationGroup="FileUpload">*&nbsp;&nbsp;</asp:RequiredFieldValidator>
                        </td>
                        <td style="text-align: left;">
                            <asp:Button ID="UploadFileButton" runat="server" OnClick="UploadFileButton_Click"
                                SkinID="Button_Standard" Text="Agregar" ValidationGroup="FileUpload" />
                            <asp:Label ID="FileUploadBoard" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="FilesGridView" runat="server" AutoGenerateColumns="False" DataSourceID="FilesDataSource"
                    Width="755px" OnRowDataBound="FilesGridView_RowDataBound" OnRowDeleting="FilesGridView_RowDeleting"
                    OnSelectedIndexChanging="FilesGridView_SelectedIndexChanging" DataKeyNames="ID">
                    <Columns>
                        <asp:TemplateField HeaderText="Archivo">
                            <ItemTemplate>
                                <asp:Label ID="FileNameLabel" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="281px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ContentType" HeaderText="Tipo" SortExpression="ContentType">
                            <ItemStyle Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Tamaño" SortExpression="ContentLength">
                            <ItemTemplate>
                                <asp:Label ID="ContentLengthLabel" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:CommandField DeleteText="Eliminar" ShowDeleteButton="True">
                            <ItemStyle Width="50px" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="FilesDataSource" runat="server" OnObjectCreating="FilesDataSource_ObjectCreating"
                    SelectMethod="GetFiles" TypeName="FilesResult" DeleteMethod="DeleteFile"></asp:ObjectDataSource>
                <asp:ObjectDataSource ID="CategoriesDataSource" runat="server" DeleteMethod="DeleteCategory"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetCategories" TypeName="WikiCategoriesResult">
                    <DeleteParameters>
                        <asp:Parameter Name="category" Type="String" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                <table style="width: 755px;">
                    <tr>
                        <td style="text-align: center; height: 45px; vertical-align: bottom;">
                            <asp:Button ID="SaveButton" runat="server" SkinID="Button_Standard" Text="Guardar"
                                Width="60px" OnClick="SaveButton_Click" />&nbsp;<asp:Button ID="CancelButton" runat="server"
                                    SkinID="Button_Standard" Text="Cancelar" Width="60px" OnClick="CancelButton_Click"
                                    CausesValidation="False" />&nbsp;<asp:Button ID="PreviewButton" runat="server" SkinID="Button_Standard"
                                        Text="Vista Previa" Width="90px" OnClick="PreviewButton_Click" />&nbsp;<asp:Button
                                            ID="DeleteButton" runat="server" OnClick="DeleteButton_Click" SkinID="Button_Standard"
                                            Text="Eliminar" Width="60px" CausesValidation="False" OnClientClick="return confirm(&quot;¿Estás seguro que deseas eliminarlo?&quot;);"
                                            Visible="False" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="bottom">
                <asp:Image ID="ContentBottomImage" runat="server" ImageUrl="~/images/fondo_tabla_bott.jpg" /></div>
        </div>
    </div>
</asp:Content>
