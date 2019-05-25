<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DataImporting.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <form class="form-signin" runat="server">
        <h2 class="form-signin-heading">Please sign in</h2>
        <label for="inputEmail" class="sr-only">Email address</label>
        <asp:TextBox runat="server" id="inputEmail" class="form-control" placeholder="UserName" required="" autofocus=""></asp:TextBox>
       <hr />
         <label for="inputPassword" class="sr-only">Password</label>
        <asp:TextBox runat="server" TextMode="password" id="inputPassword" class="form-control" placeholder="Password" required=""></asp:TextBox>
       <hr />
         <asp:Button runat="server" class="btn btn-lg btn-primary btn-block" Text="Sign In" OnClick="Unnamed_Click"></asp:Button>
      </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
