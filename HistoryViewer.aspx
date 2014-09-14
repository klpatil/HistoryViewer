<%@ Page Language="C#" AutoEventWireup="True" Inherits="SC7.Web.Tools.HistoryViewer" CodeFile="HistoryViewer.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <title>Sitecore History Viewer</title>
    <link rel="stylesheet" type="text/css" href="//cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/2.3.2/css/bootstrap.min.css" />
    <%--http://getbootstrap.com/2.3.2/components.html--%>
    <link href="Resources/slider.css" rel="stylesheet" />

    <link rel="stylesheet" type="text/css" href="Resources/DT_bootstrap.css">
    <script type="text/javascript" charset="utf-8" language="javascript" src="Resources/jquery.js"></script>
    <script type="text/javascript" charset="utf-8" language="javascript" src="Resources/jquery.dataTables.js"></script>
    <script type="text/javascript" charset="utf-8" language="javascript" src="Resources/DT_bootstrap.js"></script>
    
    <script type="text/javascript" charset="utf-8" language="javascript" src="Resources/HistoryViewer.js"></script>
    <script src="Resources/bootstrap-slider.js"></script>

    <%--http://www.eyecon.ro/bootstrap-slider/--%>
    
</head>
<body>
    <form id="form1" runat="server">




        <div class="container" style="margin-top: 30px">

            <fieldset>
                <legend>Your selection</legend>
                <div class="row">
                    <div class="span4">
                        <div class="control-group">
                            <label class="control-label" for="ddlDatabase">Select Database : </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlDatabases" runat="server">

                                </asp:DropDownList>
                              
                            </div>
                        </div>

                    </div>
                    <div class="span5">

                        <div class="control-group">
                            <label class="control-label" for="txtLastDays">Duration (Last Days) : </label>
                            <div class="controls">
                                <asp:TextBox ID="txtLastDays" runat="server" CssClass="slider"
                                    data-slider-min="1" 
                                    data-slider-max="30"
                                    data-slider-step="1" 
                                    data-slider-value="1"
                                    data-slider-orientation="horizontal"
                                    data-slider-selection="after" data-slider-tooltip="show" />
                            </div>
                        </div>
                    </div>
                    <div class="span3">
                        <div class="control-group">
                            <div class="controls">
                                <asp:Button ID="btnShow" runat="server" Text="Show History" CssClass="btn btn-default"
                                    
                                    OnClick="btnShow_Click" />
                            </div>

                        </div>
                    </div>
                </div>
            </fieldset>
            <center>
                <asp:Label ID="lblMessage" runat="server" Text="" 
                    role="alert"
                    CssClass="text-info" Font-Bold="false" Visible="false" />
            </center>
            <fieldset>
                <legend>Your results</legend>

                <asp:Table ID="tblHistoryDetails"
                    CellPadding="0" CellSpacing="0" runat="server"
                    CssClass="table table-striped table-bordered" Visible="false">
                    <asp:TableHeaderRow TableSection="TableHeader">
                        
                        <asp:TableHeaderCell>ItemPath</asp:TableHeaderCell>
                        <asp:TableHeaderCell>ItemLanguage</asp:TableHeaderCell>
                        <asp:TableHeaderCell>ItemVersion</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Action</asp:TableHeaderCell>
                        <asp:TableHeaderCell>TaskStack</asp:TableHeaderCell>
                        <asp:TableHeaderCell>UserName</asp:TableHeaderCell>
                        
                        <asp:TableHeaderCell>Created</asp:TableHeaderCell>
                        <asp:TableHeaderCell>AdditionalInfo</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </fieldset>
        </div>
    </form>
</body>
