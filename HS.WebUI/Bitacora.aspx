<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Bitacora.aspx.vb" Inherits="HS.WebUI.Bitacora" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section  class="hs-bitacora">
        <h2>Bitacora</h2>
    
        <article>
            <h3>
                <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
            </h3>

            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="ViewBitacora" runat="server">
                    <asp:GridView ID="grillaBitacora" runat="server" autogeneratecolumns="false" CssClass="hs-data-tables">
                    <columns>
                        <asp:boundfield datafield="Fecha" readonly="true" headertext="Fecha" />
                        <asp:boundfield datafield="Autor" convertemptystringtonull="true" HeaderStyle-Width ="50px" headertext="Usuario"/>
                        <asp:boundfield datafield="Descripcion" convertemptystringtonull="true" headertext="Descripcion"/>
                        <asp:boundfield datafield="Criticidad" convertemptystringtonull="true" headertext="Criticidad"/>
                    </columns>
                </asp:GridView> 
                </asp:View>
            </asp:MultiView>

            <div><asp:Button ID="btnVolver" runat="server" Text="Volver al inicio" /></div>
        </article>
    </section>    
</asp:Content>
