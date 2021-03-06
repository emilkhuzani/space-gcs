﻿<%@ Page Title="Checklist Data ME" Language="C#" MasterPageFile="~/CHECKLISTME.Master" AutoEventWireup="true" CodeBehind="checkharian.aspx.cs" Inherits="Telkomsat.checklistme.checkharian" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .link{
            color:black;
            font-size:12px;
            font-weight:bold;
        }
        .link:hover{
            color:black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <link rel="stylesheet" href="../assets/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css">
<div class="row" style="padding:20px">
<div class="nav-tabs-custom col-lg-12 no-padding">
    <!-- Tabs within a box -->
    <ul class="nav nav-tabs pull-right">
        <li class="pull-left header"><i class="fa fa-filter"></i> Filter</li>
    </ul>
    <div class="tab-content no-padding">
        <!-- Morris chart - Sales -->
        <div class="box-body">
        
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="exampleInputEmail1">Start Date</label>
                        <input type="text" class="form-control pull-right" id="txtsdate" autocomplete="off" runat="server"/>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label for="exampleInputEmail1">End Date</label>
                        <input type="text" class="form-control pull-right" id="dateend" autocomplete="off" runat="server"/>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="exampleInputEmail1">Kategori</label>
                        <asp:DropDownList ID="ddlKategori" CssClass="form-control" runat="server" onchange="status(this)">
                            <asp:ListItem Value="nama">--Petugas--</asp:ListItem>
                            <asp:ListItem Value="sumanto">Sumanto</asp:ListItem>
                            <asp:ListItem Value="putra">Pratama Putra</asp:ListItem>
                            <asp:ListItem Value="herman">Herman</asp:ListItem>
                            <asp:ListItem Value="maman">Maman</asp:ListItem>
                            <asp:ListItem Value="charla">Charla</asp:ListItem>
                            <asp:ListItem Value="soeryana">Soeryana</asp:ListItem>
                            <asp:ListItem Value="radius">Radius</asp:ListItem>
                            <asp:ListItem Value="saepul">Saepul</asp:ListItem>
                            <asp:ListItem Value="dodo">Dodo</asp:ListItem>
                            <asp:ListItem Value="ibnu">Ibnu</asp:ListItem>
                            <asp:ListItem Value="syehab">Syehab</asp:ListItem>
                            <asp:ListItem Value="Badrudin">Badrudin</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <label for="exampleInputEmail1">Filter</label>
                        <br />
                        <button type="submit" class="btn btn-primary" runat="server" onserverclick="Filter_ServerClick" >Submit</button>
                    </div>
                </div>
        <!-- /.table -->
            </div>
        </div>
    </div>
</div>

<div class="col-lg-12 connectedSortable">
        <!-- Custom tabs (Charts with tabs)-->
    <div class="box box-primary">
    <!-- Tabs within a box -->
    <div class="box-header">
        <i class="fa fa-database"></i> Checklist Harian
    </div>
    <div class="box-body">

                <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>  
    </div>
    </div>
</div>
</div>
    <script src="../assets/mylibrary/rowsgroup.js"></script>
    <script>
        $('#<%=txtsdate.ClientID%>').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy',
            orientation: "bottom"
        }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $('#<%=dateend.ClientID%>').datepicker('setStartDate', minDate);
        });
        $('#' + '<%=dateend.ClientID%>').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy',
            orientation: "bottom"
        }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $('#<%=txtsdate.ClientID%>').datepicker('setEndDate', minDate);
        });
    </script>
    <script>
        $(function () {
          $("#example3").DataTable({
          "paging": true,
          "searching": true,
          "scrollX": true,
          "order": [[ 0, 'desc' ]]
          });
           $('.dataTables_length').addClass('bs-select');
        });
    </script>
</asp:Content>
