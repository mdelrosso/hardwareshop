<%@ Page Title="Hardware" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Hardware.aspx.vb" Inherits="HS.WebUI.Hardware" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
    
    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 500px;
            height: 200px;
        }
    </style>
    <table>
        <tr>
            <td>
                <h2>Hardware a la venta</h2>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                 <table>
                        <tr>
                            <td><h3>Nuestros productos</h3></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grillaHardware" runat="server" AutoGenerateColumns="False">
                            
                                <columns>
                                      <asp:boundfield datafield="Id"
                                        readonly="true"      
                                        headertext="Id"
                                        />   
                                      <asp:boundfield datafield="Descripcion"
                                        readonly="true"      
                                        headertext="Descripción del producto"/>
                                      <asp:boundfield datafield="Precio"
                                        convertemptystringtonull="true"
                                        headertext="Precio"/>
                                    <asp:TemplateField HeaderText="" SortExpression="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" Text = "Editar" OnClick = "Edit"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                   </columns>
                                
                                </asp:GridView>
                            </td>
                        </tr>
                  </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center">
                <asp:Button ID="btnVolver" runat="server" Text="Volver"/>
                <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" />
            </td>
        </tr>
    </table>
 
    <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup" style = "display:none">
        <asp:Label Font-Bold = "true" ID = "Label4" runat = "server" Text = "Edición Hardware" ></asp:Label>
        <br />
        <table align = "center">
            <tr>
                <td>
                    <asp:Label ID = "Label1" runat = "server" Text = "Id" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtId" runat="server" Visible="False"></asp:TextBox>   
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID = "Label2" runat = "server" Text = "Descripción" ></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescripcion" runat="server"></asp:TextBox>   
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID = "Label3" runat = "server" Text = "Precio" ></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPrecio" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick = "Save" />
                </td>
                <td>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick = "return Hidepopup()"/>
                </td>
            </tr>
            
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                            PopupControlID="pnlAddEdit" TargetControlID = "lnkFake"
                            BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>

   
</asp:Content>
