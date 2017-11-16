<%@ Page Title="Administrar Perfiles" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IntegridadBD.aspx.vb" Inherits="HS.WebUI.IntegridadBD" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                <h2>
                    Integridad de la Base de Datos</h2>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3"><h3>Verificar integridad de la base de datos:</h3></td>
            <td >                                
                <asp:Button  ID="btnVerificarIntegridad" runat="server" Text="Ejecutar" />                                
            </td>
        </tr>                        
        <tr> <td></td> </tr>
        <tr>
            <td colspan="3"><h3>Recalcular digitos verificadores:</h3></td>
            <td >                                
                <asp:Button  ID="btnRecalcularDigitosVerificadores" runat="server" Text="Ejecutar" />                                
            </td>
        </tr>
        <tr> <td></td> </tr>
        <tr>
            <td colspan="3"><h3>Restauración el sistema:</h3></td>
            <td >                                
                <asp:Button  ID="btnRestaurar" runat="server" Text="Acceder" />                                
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align:center">
                <asp:Button ID="btnVolver" runat="server" Text="Volver" />
            </td>
        </tr>
       
    </table>
</asp:Content>