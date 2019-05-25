<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="DataImporting.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <div class="jumbotron">
        <div class="col-sm-8 mx-auto">
          <h1><asp:Label runat="server" ID="error"></asp:Label></h1>
            <p><asp:HyperLink runat="server" ID="retru"></asp:HyperLink></p>
        </div>
      </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
