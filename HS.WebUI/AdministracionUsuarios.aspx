<%@ Page Title="Administración de Usuarios" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AdministracionUsuarios.aspx.vb" Inherits="HS.WebUI.AdministracionUsuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section  class="hs-adminusers">
        <h2>Administración de Usuarios</h2>

        <article>
            <p><asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label></p>
            
            <asp:Button ID="btnBuscar" runat="server" Text="Consultar" />

            <asp:MultiView ID="MultiView1" runat="server">
                    <view>
                        <asp:View ID="ViewBuscar" runat="server">
                            <h3>Buscar Usuarios</h3>
                                
                            <asp:GridView ID="grillaUsuarios" runat="server" AutoGenerateSelectButton="True" AutoGenerateColumns="False" CssClass="hs-data-tables">
                                <columns>
                                    <asp:boundfield datafield="Id" readonly="true" headertext="Id"/>
                                    <asp:boundfield datafield="Nombre" readonly="true" headertext="Nombre de Usuario"/>
                                    <asp:boundfield datafield="Email" convertemptystringtonull="true" headertext="Email"/>
                                    <asp:boundfield datafield="Bloqueado" convertemptystringtonull="true" headertext="¿Cuenta Bloqueada?"/>
                                    <asp:boundfield datafield="Eliminado" convertemptystringtonull="true" headertext="¿Cuenta Eliminada?"/>
                                </columns>
                            </asp:GridView>
                        </asp:View>


                        <asp:View ID="ViewEditar" runat="server">
                            <h3>Editar usuario</h3>
                            <div class="hs-form">

                                <div class="hs-form-control">
                                    <label>ID:</label>
                                    <div><asp:Label ID="lblId" runat="server" Text="(nuevo)"></asp:Label></div>
                                </div>

                                <div class="hs-form-control">
                                    <label>Nombre:</label>
                                    <div><asp:TextBox ID="txtNombre" runat="server" MaxLength="16"></asp:TextBox></div>
                                </div>
                                
                                <div class="hs-form-control">
                                    <label>Clave:</label>
                                    <div>
                                        <asp:TextBox ID="txtClave" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txtClave" ID="RegularExpressionValidator1" ValidationExpression = "^[\s\S]{6,16}$" runat="server" ForeColor ="Red" ErrorMessage="Ingrese una clave con un minimo de 6 caracteres y un maximo de 16."></asp:RegularExpressionValidator>
                                    </div>
                                </div>

                                <div class="hs-form-control">
                                    <label>Confirmar Clave:</label>
                                    <div><asp:TextBox ID="txtReClave" runat="server" TextMode="Password"></asp:TextBox></div>
                                </div>

                                <div class="hs-form-control">
                                    <label>E-Mail:</label>
                                    <div><asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="200px"></asp:TextBox></div>
                                </div>
                               
                                <div class="hs-form-control">
                                    <label>Bloqueado:</label>
                                    <div><asp:CheckBox ID="chkBloqueado" runat="server"></asp:CheckBox></div>
                                </div>

                                <div class="hs-form-actions">
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"></asp:Button>
                                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar"></asp:Button>
                                </div>
                            </div>
                        </asp:View>
                    </view>
                </asp:MultiView>
            
            <asp:Button ID="btnVolver" runat="server" Text="Volver al inicio"/>
        </article>

    </section>
</asp:Content>



