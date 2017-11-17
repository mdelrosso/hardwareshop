<%@ Page Title="Carrito" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" EnableEventValidation="false" CodeBehind="Carrito.aspx.vb" Inherits="HS.WebUI.Carrito" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                                <asp:GridView ID="grillaHardware" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                              runat="server" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound">
                                <columns>
                                      <asp:boundfield datafield="Id"
                                        readonly="true"      
                                        headertext="Id"
                                        Visible="false"/>   
                                      <asp:boundfield datafield="Descripcion"
                                        readonly="true"      
                                        headertext="Descripción del producto"/>
                                      <asp:boundfield datafield="Precio"
                                        convertemptystringtonull="true"
                                        headertext="Precio"/>
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
            </td>
        </tr>
    </table>
</asp:Content>
