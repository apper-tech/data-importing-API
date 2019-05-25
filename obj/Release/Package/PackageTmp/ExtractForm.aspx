<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ExtractForm.aspx.cs" Inherits="DataImporting.ExtractForm" %>
<%@ Register Src="~/User Controls/Nestoria_Control.ascx" TagName="Nestoria" TagPrefix="ctr" %>
<%@ Register Src="~/User Controls/DataNerds_Control.ascx" TagName="DataNerds" TagPrefix="ctr" %>
<%@ Register Src="~/User Controls/HouseCanary_Control.ascx" TagName="HouseCanary" TagPrefix="ctr" %>
<%@ Register Src="~/User Controls/Daft_Control.ascx" TagName="Daft" TagPrefix="ctr" %>


<asp:Content ID="Content1" ContentPlaceHolderID="StyleSection" runat="server">
    <link href="Content/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="Content/preview.min.css" rel="stylesheet" />
    <link href='http://fonts.googleapis.com/css?family=PT+Sans:400,700' rel='stylesheet' type='text/css'>
    <link href="Content/font-awesome.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <form runat="server" id="form">
        <div class="row-fluid" id="demo-1">
            <div class="span10 offset1">
                <div class="tabbable custom-tabs tabs-animated  flat flat-all hide-label-980 shadow track-url auto-scroll">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#panel1" data-toggle="tab" class="active "><img src="Images/nestoria.png" style="width:20px"/>&nbsp;<span>Nestoria</span></a></li>
                        <li><a href="#panel2" data-toggle="tab"><img src="Images/datanerds.png" style="width:20px"/>&nbsp;<span>Data Nerds</span></a></li>
                        <li><a href="#panel3" data-toggle="tab"><img src="Images/housecanary.png" style="width:20px"/>&nbsp;<span>House Canary</span></a></li>
                        <li><a href="#panel4" data-toggle="tab"><img src="Images/daft.png" style="width:20px"/>&nbsp;<span>Daft</span></a></li>
                    </ul>
                    <div class="tab-content ">
                         <ctr:Nestoria runat="server" ID="pnl1"></ctr:Nestoria>
                         <ctr:DataNerds runat="server" ID="pnl2"></ctr:DataNerds>
                         <ctr:HouseCanary runat="server" ID="pnl3"></ctr:HouseCanary>
                         <ctr:Daft runat="server" ID="pnl4"></ctr:Daft>
                    </div>

                </div>
            </div>
        </div>
 
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
    <script src="Scripts/tabs-addon.js"></script>

    <script type="text/javascript">
        
        $(function ()
        {
            $("a[href^='#demo']").click(function (evt)
            {
                evt.preventDefault();
                var scroll_to = $($(this).attr("href")).offset().top;
                $("html,body").animate({ scrollTop: scroll_to - 80 }, 600);
            });
            $("a[href^='#bg']").click(function (evt)
            {
                evt.preventDefault();
                $("body").removeClass("light").removeClass("dark").addClass($(this).data("class")).css("background-image", "url('bgs/" + $(this).data("file") + "')");
                console.log($(this).data("file"));


            });
            $("a[href^='#color']").click(function (evt)
            {
                evt.preventDefault();
                var elm = $(".tabbable");
                elm.removeClass("grey").removeClass("dark").removeClass("dark-input").addClass($(this).data("class"));
                if (elm.hasClass("dark dark-input"))
                {
                    $(".btn", elm).addClass("btn-inverse");
                }
                else
                {
                    $(".btn", elm).removeClass("btn-inverse");

                }

            });
            $(".color-swatch div").each(function ()
            {
                $(this).css("background-color", $(this).data("color"));
            });
            $(".color-swatch div").click(function (evt)
            {
                evt.stopPropagation();
                $("body").removeClass("light").removeClass("dark").addClass($(this).data("class")).css("background-color", $(this).data("color"));
            });
            $("#texture-check").mouseup(function (evt)
            {
                evt.preventDefault();

                if (!$(this).hasClass("active"))
                {
                    $("body").css("background-image", "url(bgs/n1.png)");
                }
                else
                {
                    $("body").css("background-image", "none");
                }
            });

            $("a[href='#']").click(function (evt)
            {
                evt.preventDefault();

            });

            $("a[data-toggle='popover']").popover({
                trigger:"hover",html:true,placement:"top"
            });
        });

    </script>

</asp:Content>
