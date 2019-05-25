<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Nestoria_Control.ascx.cs" Inherits="DataImporting.User_Controls.Nestoria_Control" %>
<style type="text/css">
    .radioButtonList {
        list-style: none;
        margin: 0;
        padding: 0;
    }

    .button {
        padding: 12px;
    }

    .pager {
    }

    .radioButtonList.horizontal li {
        display: inline;
        padding-left: 15px;
    }

    .radioButtonList tr td {
        padding-left: 15px;
    }

    .radioButtonList label {
        display: inline;
    }
</style>

<div class="tab-pane active" id="panel1">
    <div class="row-fluid">
        <h4>Check or Modify Input Parameters</h4>

        <h4>
        <asp:DetailsView ID="DetailView" runat="server" AllowPaging="True" DataKeyNames="id"
                AutoGenerateRows="False" DataSourceID="SqlDataSource1" OnItemInserting="DetailView_ItemInserting" Width="100%" OnDataBound="DetailView_DataBound"
                OnItemUpdating="DetailsView1_ItemUpdating" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" HorizontalAlign="Center" />
            <EditRowStyle BackColor="#CCCCCC" />
            <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
            <Fields>
                <asp:TemplateField HeaderText="ID">
                     <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("id") %>'></asp:Label>
                        </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Place Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="form-control" Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtName"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("place") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                 <asp:TemplateField HeaderText="Listing Type">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlListing" runat="server" Width="100%" CssClass="form-control">
                                <asp:ListItem Value="buy">Buy</asp:ListItem>
                                <asp:ListItem Value="rent">Rent</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlListing"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblList" runat="server" Text='<%#Eval("listing") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Count">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCount" CssClass="form-control" runat="server" MaxLength="50" Width="100%" TextMode="Number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCount"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCount" runat="server" Text='<%#Eval("count") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                 <asp:TemplateField HeaderText="Country">
                        <ItemTemplate>
                            <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("country_description") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlCountry" CssClass="form-control" runat="server" Width="100%">
                                <asp:ListItem Value="1">England</asp:ListItem>
                                  <asp:ListItem Value="2">Germany</asp:ListItem>
                                <asp:ListItem Value="3">Spain</asp:ListItem>
                                <asp:ListItem Value="4">France</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                 <asp:TemplateField HeaderText="Keywords">
                        <ItemTemplate>
                            <asp:Label ID="lblkeywords"  runat="server" Text='<%#Eval("keywords") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBoxList runat="server" style="margin-top:5px" ID="chlkeywords" Width="100%">
                                <asp:ListItem Value="Apartment">Apartment</asp:ListItem>
                                 <asp:ListItem Value="Fireplace">Fireplace</asp:ListItem>
                                 <asp:ListItem Value="Flat">Flat</asp:ListItem>
                                 <asp:ListItem Value="Garage">Garage</asp:ListItem>
                                 <asp:ListItem Value="Garden">Garden</asp:ListItem>
                                 <asp:ListItem Value="Timeshare">Timeshare</asp:ListItem>
                            </asp:CheckBoxList>
                            
                        </EditItemTemplate>
                    </asp:TemplateField>
   
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowInsertButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-info" >
   
<ControlStyle CssClass="btn btn-info"></ControlStyle>
                </asp:CommandField>
   
            </Fields>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerSettings FirstPageImageUrl="~/Images/first.png" LastPageImageUrl="~/Images/last.png" Mode="NextPreviousFirstLast" NextPageImageUrl="~/Images/nxt.png" PreviousPageImageUrl="~/Images/prev.png" />
            <PagerStyle BackColor="#3399FF" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            </asp:DetailsView>
        </h4>
         <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SettingsConnectionString %>" DeleteCommand="DELETE FROM [Nestoria] WHERE [Id] = @original_Id" InsertCommand="INSERT INTO Nestoria(place, listing, count, keywords, country_id) VALUES (@place, @listing, @count, @keywords, @country)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT Nestoria.Id, Nestoria.place, Nestoria.listing, Nestoria.count, Nestoria.keywords, Country.country_description, Country.country_id FROM Country INNER JOIN Nestoria ON Country.country_id = Nestoria.country_id" UpdateCommand="UPDATE Nestoria SET place = @place, listing = @listing, count = @count, keywords = @keywords, country_id = @country WHERE (Id = @original_Id)">
            <DeleteParameters>
                <asp:Parameter Name="original_Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="place" Type="String" />
                <asp:Parameter Name="listing" Type="String" />
                <asp:Parameter Name="count" Type="Int32" />
                <asp:Parameter Name="keywords" Type="String" />
                <asp:Parameter Name="country" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="place" Type="String" />
                <asp:Parameter Name="listing" Type="String" />
                <asp:Parameter Name="count" Type="Int32" />
                <asp:Parameter Name="keywords" Type="String" />
                <asp:Parameter Name="country" Type="Int32" />
                <asp:Parameter Name="original_Id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SettingsConnectionString %>" SelectCommand="SELECT * FROM [Country]"></asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="False">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="place" HeaderText="Place Name" SortExpression="place" />
                <asp:BoundField DataField="listing" HeaderText="Listing type" SortExpression="listing" />
                <asp:BoundField DataField="count" HeaderText="Count" SortExpression="count" />
                <asp:BoundField DataField="country_description" HeaderText="Country Code" SortExpression="country_description" />
                <asp:BoundField DataField="keywords" HeaderText="Keywords" SortExpression="keywords" />
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
        <hr />
        <asp:Button runat="server" ID="view" OnClick="view_Click" CssClass="btn btn-info" Text="View Log" />
        <asp:Button runat="server" ID="down" CssClass="btn btn-info" Text="Download Log" OnClick="down_Click" />
        <asp:Label runat="server" ID="info" Text="Waiting" Style="float: right"></asp:Label>
        <asp:Button runat="server" ID="run" CssClass="btn btn-info" Text="Run Now!" OnClick="run_Click" />
         <asp:Button runat="server" ID="images" CssClass="btn btn-info" Text="Force Move Images" OnClick="images_Click" />
         <asp:Button runat="server" ID="del" CssClass="btn btn-info" Text="Delete ALL" OnClick="del_Click" OnClientClick="if (UserDeleteConfirmation()) return true;" />
       <%-- <asp:Button runat="server" ID="test" CssClass="btn btn-info" Text="Run Now!" OnClick="test_Click"  />--%>
    </div>
</div>
<script>
function UserDeleteConfirmation() {
    return confirm("Are you sure you want to delete ALL PROPERTIES from Nestoria?");
}
</script>
