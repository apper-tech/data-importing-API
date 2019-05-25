<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="DataImporting.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <form id="form1" runat="server">
    <div class="jumbotron">
        <div class="col-sm-12 mx-auto">
          <h1>Properties Table</h1>
          <p>This is a data extraction website form the real estate website Akaratak.com</p>
        </div>
      </div>
     <div class="jumbotron">
        <div class="col-sm-12 mx-auto">
          
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" CellPadding="4" DataKeyNames="PropertyID" DataSourceID="RealData" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="PropertyID" HeaderText="PropertyID" InsertVisible="False" ReadOnly="True" SortExpression="PropertyID" />
                    <asp:BoundField DataField="Property_Category_ID" HeaderText="Property_Category_ID" SortExpression="Property_Category_ID" />
                    <asp:BoundField DataField="Property_Type_ID" HeaderText="Property_Type_ID" SortExpression="Property_Type_ID" />
                    <asp:BoundField DataField="Property_Size" HeaderText="Property_Size" SortExpression="Property_Size" />
                    <asp:BoundField DataField="Date_Added" HeaderText="Date_Added" SortExpression="Date_Added" />
                    <asp:BoundField DataField="Floor" HeaderText="Floor" SortExpression="Floor" />
                    <asp:CheckBoxField DataField="Has_Garage" HeaderText="Has_Garage" SortExpression="Has_Garage" />
                    <asp:CheckBoxField DataField="Has_Garden" HeaderText="Has_Garden" SortExpression="Has_Garden" />
                    <asp:BoundField DataField="Num_Bedrooms" HeaderText="Num_Bedrooms" SortExpression="Num_Bedrooms" />
                    <asp:BoundField DataField="Num_Bathrooms" HeaderText="Num_Bathrooms" SortExpression="Num_Bathrooms" />
                    <asp:BoundField DataField="Expire_Date" HeaderText="Expire_Date" SortExpression="Expire_Date" />
                    <asp:BoundField DataField="Contract_Type" HeaderText="Contract_Type" SortExpression="Contract_Type" />
                    <asp:BoundField DataField="City_ID" HeaderText="City_ID" SortExpression="City_ID" />
                    <asp:BoundField DataField="Country_ID" HeaderText="Country_ID" SortExpression="Country_ID" />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                    <asp:BoundField DataField="Zip_Code" HeaderText="Zip_Code" SortExpression="Zip_Code" />
                    <asp:BoundField DataField="Other_Details" HeaderText="Other_Details" SortExpression="Other_Details" />
                    <asp:BoundField DataField="Sale_Price" HeaderText="Sale_Price" SortExpression="Sale_Price" />
                    <asp:BoundField DataField="Rent_Price" HeaderText="Rent_Price" SortExpression="Rent_Price" />
                    <asp:BoundField DataField="Num_Floors" HeaderText="Num_Floors" SortExpression="Num_Floors" />
                    <asp:BoundField DataField="User_ID" HeaderText="User_ID" SortExpression="User_ID" />
                    <asp:BoundField DataField="Property_Photo" HeaderText="Property_Photo" SortExpression="Property_Photo" />
                    <asp:BoundField DataField="Url_ext" HeaderText="Url_ext" SortExpression="Url_ext" />
                    <asp:BoundField DataField="Property_Id_ext" HeaderText="Property_Id_ext" SortExpression="Property_Id_ext" />
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
            <asp:SqlDataSource ID="RealData" runat="server" ConnectionString="<%$ ConnectionStrings:DB_9FEE73_RealEstateDBConnectionString %>" SelectCommand="SELECT * FROM [Properties] ORDER BY [PropertyID]"></asp:SqlDataSource>
          
        </div>
      </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>