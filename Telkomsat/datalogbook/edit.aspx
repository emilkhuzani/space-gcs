﻿<%@ Page Title="Edit" Language="C#" MasterPageFile="~/LOGBOOK.Master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="Telkomsat.datalogbook.edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../assets/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css">
    <style type="text/css">
        .row{
            padding-bottom:16px;
        }
        .ui-autocomplete { z-index:2147483647; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:TextBox ID="txtdevice" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtsite" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtgedung" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtruangan" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtrak" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtruanganid" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtrakid" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtidp" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtidl" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="txtjenispekerjaan" runat="server" CssClass="hidden"></asp:TextBox>
    <div class="box box-default">
        <div class="box-header">
            <h4>Edit Logbook</h4>
        </div>
        <div class="box-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="alert alert-success alert-dismissable" id="divsuccess" runat="server" visible="false">
                        <h5><span class="fa fa-check"> Berhasil diedit</span></h5>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label1" runat="server" Text="Mulai Kegiatan" Font-Bold="true"></asp:Label><span style="color:red"> *</span>
                            <asp:TextBox ID="txtstartdate" autocomplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label2" runat="server" Text="Perkiraan selesai" Font-Bold="true"></asp:Label><span style="color:red"> *</span>
                            <asp:TextBox ID="txtduedate" autocomplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="Label3" runat="server" Text="Judul Kegiatan" Font-Bold="true"></asp:Label><span style="color:red"> *</span>
                            <asp:TextBox ID="txtjudul" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="Label4" runat="server" Text="Unit Divisi" Font-Bold="true"></asp:Label><span style="color:red"> *</span>
                            <asp:DropDownList ID="ddlunit" runat="server" CssClass="form-control">
                                <asp:ListItem>--Pilih Divisi--</asp:ListItem>
                                <asp:ListItem>Harkat</asp:ListItem>
                                <asp:ListItem>ME</asp:ListItem>
                                <asp:ListItem>SCO</asp:ListItem>
                                <asp:ListItem>Andat</asp:ListItem>
                                <asp:ListItem>Orbital</asp:ListItem>
                                <asp:ListItem>STS</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="Label5" runat="server" Text="Status Kegiatan" Font-Bold="true"></asp:Label><span style="color:red"> *</span>
                            <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control">
                                <asp:ListItem>--Pilih Status--</asp:ListItem>
                                <asp:ListItem>On Progress</asp:ListItem>
                                <asp:ListItem>Selesai</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label6" runat="server" Text="PIC Internal" Font-Bold="true"></asp:Label><span style="color:red"> *</span>
                            <asp:TextBox ID="txtint" placeholder="PIC bisa lebih dari 1 (pisahkan dengan tanda ;)" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label7" runat="server" Text="PIC External" Font-Bold="true"></asp:Label>
                            <asp:TextBox ID="txtext" placeholder="PIC bisa lebih dari 1 (pisahkan dengan tanda ;)" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="Label8" runat="server" Text="Kategori Kegiatan" Font-Bold="true"></asp:Label><span style="color:red"> *</span>
                            <asp:DropDownList ID="ddlkategori" runat="server" CssClass="form-control">
                                <asp:ListItem>--Pilih Kategori Kegiatan--</asp:ListItem>
                                <asp:ListItem>Pointing Antena</asp:ListItem>
                                <asp:ListItem>Perbaikan</asp:ListItem>
                                <asp:ListItem>Perawatan</asp:ListItem>
                                <asp:ListItem>Penggantian</asp:ListItem>
                                <asp:ListItem>Troubleshot</asp:ListItem>
                                <asp:ListItem>Update/Upgrade</asp:ListItem>
                                <asp:ListItem>Lain-lain</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                     <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="Label9" runat="server" Text="Kegiatan" Font-Bold="true"></asp:Label><span style="color:red"> *</span>
                            <asp:TextBox ID="txtAktivitas" TextMode="MultiLine" runat="server" Height="100px" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <label>Lampiran Gambar</label>
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        </div>
                    </div>
                    <br />
                        <div class="col-md-6 col-sm-12">
                            <label>Tambah Lampiran</label>
                            <div id="dvfiles">
                                <table class="table table-bordered kita" id="tableku" runat="server">
                                    <thead>
                                        <tr>
                                            <th>File</th>
                                            <th>Caption</th>
                                            <th>#</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                            <button id="addfile" type="button" class="btn-sm btn-default"><i class="fa fa-plus"></i></button> <br />
                    </div>
                </div>
            </div>
        </div>
        <div class="box-footer">
            <button type="submit" class="btn btn-default pull-right" runat="server" onserverclick="Unnamed_ServerClick">Submit</button>
        </div>
    </div>
   <script src="../assets/mylibrary/sweetalert.min.js"></script>
    <script>
        var i = 1;

        $(document).ready(function() {
            $("#addfile").click(function () {
                var markup = "<tr><td><input name=" + i + "fu type=file /></td><td><input type='text' name='caption' class='form-control' /></td>" +
                    "<td> <button type='button' name='record' onclick='newtest2(this)' class='btn-sm btn-default delete-row'><i class=fa>X</i></button></td></tr>";
                $('#' + '<%= tableku.ClientID%>').append(markup);
                i++;
                console.log('add');
            });
            
        });   

        function newtest2(e) {              //Add e as parameter
            $(e).parents('tr').remove();   //Use the e to delete
            //console.log('klkl');
        }

        $(function () {
            //CKEDITOR.replace('<txtAktivitas.ClientID%>');
        })


        $('#<%=txtstartdate.ClientID%>').datepicker({
            autoclose: true,
            format: 'yyyy/mm/dd',
            orientation: "bottom"
        }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $('#<%=txtduedate.ClientID%>').datepicker('setStartDate', minDate);
        });
        $('#' + '<%=txtduedate.ClientID%>').datepicker({
            autoclose: true,
            format: 'yyyy/mm/dd',
            orientation: "bottom"
        }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $('#<%=txtstartdate.ClientID%>').datepicker('setEndDate', minDate);
            });

        function confirmhapus(deleteurl) {
            swal({
                title: 'Apakah Anda Yakin ?',
                text: 'Data yang dihapus tidak akan kembali lagi',
                buttons: true,
                dangerMode: true,

            }).then((willDelete)=>{
                if (willDelete) {
                    document.location = deleteurl;
                }
            });
        }


    </script>
</asp:Content>
