﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Telkomsat
{
    public partial class dash : System.Web.UI.Page
    {
        SqlDataAdapter dashift, da1, da5, da6, da7, da8, dabjm;
        DataSet dsshift = new DataSet();
        DataSet dspekerjaan = new DataSet();
        string query, iduser, tanggal, style1, style, style3, agenda, dataagenda, pilihicon, icon1, queryaddev, queryev, IDdata;

        int output, output1, output2, output3, output4;
        string bwilayah, bruangan, brak, queryhistory, queryfungsi, querylain, enddate, datadeskripsi, stylebg, deskripsi, judul, datajudul, user;
        StringBuilder htmlTableShift = new StringBuilder();
        StringBuilder htmlDeadline = new StringBuilder();
        StringBuilder htmlNow = new StringBuilder();
        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
                Response.Redirect("~/login.aspx");

            if (Session["previllage"].ToString() == "adminme" || Session["previllage"].ToString() == "super")
                ashiftme.Visible = true;

            lblProfile1.Text = Session["nama1"].ToString();

            user = Session["username"].ToString();
            if (!IsPostBack)
            {
                sqlCon.Open();
                SqlDataAdapter sqlCmd = new SqlDataAdapter("ProViewByUser", sqlCon);
                sqlCmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlCmd.SelectCommand.Parameters.AddWithValue("@user", user);
                DataTable dtbl1 = new DataTable();
                sqlCmd.Fill(dtbl1);
                dtContact.DataSource = dtbl1;
                dtContact.DataBind();
                sqlCon.Close();
            }
            tableshift();
            lblwaktu1.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));
            string tanggal5 = DateTime.Now.ToString("yyyy/MM/dd");
            queryev = $"SELECT  tanggal, event, jam, lokasi, penyelenggara, icon, ID_Event, statususer FROM Event WHERE TANGGAL >= '{tanggal5}' order by ID_Event desc";
            sqlCon.Open();
            SqlDataAdapter sqlCmd4 = new SqlDataAdapter(queryev, sqlCon);
            DataTable dtbl4 = new DataTable();
            sqlCmd4.Fill(dtbl4);
            dtEvent.DataSource = dtbl4;
            dtEvent.DataBind();
            sqlCon.Close();
            logbookonprogress();
            logdeadline();
            lognow();
            checklist();
            ticket();

        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/login.aspx");
        }

        void logbookonprogress()
        {
            string myquery = @"select top 1 (SELECT count(status)[total] from tabel_logbook where status='On Progress')[Utama], 
                                (select count(*) from table_pekerjaan p where p.status = 'On Progress' and p.jenis_pekerjaan = 'konfigurasi')[konfigurasi],
                                (select count(*) from table_pekerjaan p where p.status = 'On Progress' and p.jenis_pekerjaan = 'mutasi')[mutasi],
                                (select count(*) from table_pekerjaan p where p.status = 'On Progress' and p.jenis_pekerjaan= 'lain-lain')[lain-lain],
                                (select count(*) from table_pekerjaan p where p.status = 'On Progress' and p.jenis_pekerjaan= 'fungsi & status')[status]
                                from tabel_logbook l full join table_pekerjaan p on p.id_logbook=l.id_logbook";
            SqlCommand cmd = new SqlCommand(myquery, sqlCon);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                lbutama.Text = ds.Tables[0].Rows[0]["Utama"].ToString();
                lbkonfig.Text = ds.Tables[0].Rows[0]["konfigurasi"].ToString();
                lbmutasi.Text = ds.Tables[0].Rows[0]["mutasi"].ToString();
                lblain.Text = ds.Tables[0].Rows[0]["lain-lain"].ToString();
                lbStatus.Text = ds.Tables[0].Rows[0]["status"].ToString();
            }

        }

        void lognow()
        {
            string mytanggal = DateTime.Now.ToString("yyyy/MM/dd");
            DateTime sekarang = DateTime.Now;
            string myquery = $"select * from tabel_logbook where tanggal = '{mytanggal}'";

            sqlCon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter(myquery, sqlCon);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            sqlCon.Close();

            htmlNow.Append("<table id=\"example2\" width=\"100%\" class=\"table\">");
            htmlNow.Append("<thead>");
            htmlNow.Append("<tr><th>Unit</th><th>Tipe</th><th>Judul</th><th>Action</th>");
            htmlNow.Append("</thead>");
            htmlNow.Append("<tbody>");
            if (!object.Equals(ds.Tables[0], null))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblEvent.Visible = false;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        judul = ds.Tables[0].Rows[i]["judul_logbook"].ToString();

                        if (judul.Length >= 40)
                        {
                            datajudul = judul.Substring(0, 30) + ".....";
                        }
                        else
                        {
                            datajudul = judul;
                        }

                        stylebg = "font-size:12px; font-weight:normal";
                        IDdata = ds.Tables[0].Rows[i]["id_logbook"].ToString();
                        htmlNow.Append("<td>" + $"<label style=\"{stylebg}\">" + ds.Tables[0].Rows[i]["unit"].ToString() + "</label>" + "</td>");
                        htmlNow.Append("<td>" + $"<label style=\"{stylebg}\">" + ds.Tables[0].Rows[i]["tipe_logbook"].ToString() + "</label>" + "</td>");
                        htmlNow.Append("<td>" + $"<label style=\"{stylebg}\">" + datajudul + "</label>" + "</td>");
                        htmlNow.Append("<td>" + $"<label style=\"{stylebg}\">" + $"<a href=\"../datalogbook/detail.aspx?idlog={IDdata}\" style=\"margin-right:10px\">" + "View" + "</a>" + "</td>");

                        htmlNow.Append("</tr>");
                    }
                    htmlNow.Append("</tbody>");
                    htmlNow.Append("</table>");
                    PlaceholderNow.Controls.Add(new Literal { Text = htmlNow.ToString() });
                }
            }
        }

        void ticket()
        {
            string myquery = @"select SUM(CASE WHEN status = 'reject' THEN 1 ELSE 0 END) as reject, SUM(CASE WHEN status = 'sent' THEN 1 ELSE 0 END) as sent, 
                            SUM(CASE WHEN status = 'accept' THEN 1 ELSE 0 END) as accept, SUM(CASE WHEN status = 'confirm' THEN 1 ELSE 0 END) as confirm,
                            SUM(CASE WHEN status = 'complete' THEN 1 ELSE 0 END) as complete from ticket_user";
            SqlCommand cmd = new SqlCommand(myquery, sqlCon);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtpie.Text = "ada";
                txtreject.Text = ds.Tables[0].Rows[0]["reject"].ToString();
                txtconfirm.Text = ds.Tables[0].Rows[0]["confirm"].ToString();
                txtcomplete.Text = ds.Tables[0].Rows[0]["complete"].ToString();
                txtaccept.Text = ds.Tables[0].Rows[0]["accept"].ToString();
                txtsent.Text = ds.Tables[0].Rows[0]["sent"].ToString();
            }
            else
            {
                txtpie.Text = "";
            }
        }

        void logdeadline()
        {
            string mytanggal = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            DateTime sekarang = DateTime.Now;
            string myquery = $"select * from table_pekerjaan where enddate >= '{mytanggal}' and status = 'On Progress'";

            sqlCon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter(myquery, sqlCon);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            sqlCon.Close();

            htmlDeadline.Append("<table id=\"example2\" width=\"100%\" class=\"table\">");
            htmlDeadline.Append("<tbody>");
            if (!object.Equals(ds.Tables[0], null))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lbldeadline.Visible = false;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        deskripsi = ds.Tables[0].Rows[i]["deskripsi"].ToString();

                        if (deskripsi.Length >= 40)
                        {
                            datadeskripsi = deskripsi.Substring(0, 40) + "....";
                        }
                        else
                        {
                            datadeskripsi = deskripsi;
                        }

                        stylebg = "font-size:12px; font-weight:normal";
                        IDdata = ds.Tables[0].Rows[i]["id_logbook"].ToString();

                        DateTime start = (DateTime)ds.Tables[0].Rows[i]["enddate"];
                        TimeSpan t = start - sekarang;
                        if (t.Days == 1)
                        {
                            enddate = "1 hari";
                            style3 = "label label-warning";
                        }
                        else if (t.Days == 0)
                        {
                            enddate = "0 hari";
                            style3 = "label label-danger";
                        }
                        else if (t.Days <= -1)
                        {
                            enddate = "melebihi";
                            style3 = "label label-danger";
                        }

                        if (t.Days <= 1)
                        {
                            htmlDeadline.Append("<tr>");
                            htmlDeadline.Append("<td>" + $"<label class=\"{style3}\">" + enddate + "</label>" + "</td>");
                            htmlDeadline.Append("<td>" + $"<label style=\"{stylebg}\">" + ds.Tables[0].Rows[i]["jenis_pekerjaan"].ToString() + "</label>" + "</td>");
                            htmlDeadline.Append("<td>" + $"<label style=\"{stylebg}; white-space:pre-line; position:relative\">" + datadeskripsi + "</label>" + "</td>");
                            htmlDeadline.Append("<td>" + $"<label style=\"{stylebg}\">" + $"<a href=\"../datalogbook/detail.aspx?idlog={IDdata}\" style=\"margin-right:10px\">" + "View" + "</a>" + "</td>");
                            htmlDeadline.Append("</tr>");
                        }
                    }
                    htmlDeadline.Append("</tbody>");
                    htmlDeadline.Append("</table>");
                    PlaceHolderDeadline.Controls.Add(new Literal { Text = htmlDeadline.ToString() });
                }
            }
        }

        void checklist()
        {
            SqlDataAdapter counthk, countme, countbjm;
            string tanggalku = DateTime.Now.ToString("yyyy/MM/dd");
            string tanggalkumalam = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            DateTime wib = DateTime.UtcNow + new TimeSpan(7, 0, 0);
            string mytanggal = wib.ToString("yyyy/MM/dd");

            sqlCon.Open();
            string srcountme = "select count(*) as total from checkme_parameter";
            DataSet dscountme = new DataSet();
            SqlCommand cmdme = new SqlCommand(srcountme, sqlCon);
            countme = new SqlDataAdapter(cmdme);
            countme.Fill(dscountme);
            sqlCon.Close();
            int totalme = Convert.ToInt32(dscountme.Tables[0].Rows[0]["total"]);

            sqlCon.Open();
            string srcounthk = @"select count(*) as total from checkhk_parameter r join checkhk_perangkat t on t.id_perangkat=r.id_perangkat
                                where t.id_perangkat not like '%' + 'bjm' + '%' ";
            DataSet dscounthk = new DataSet();
            SqlCommand cmdhk = new SqlCommand(srcounthk, sqlCon);
            counthk = new SqlDataAdapter(cmdhk);
            counthk.Fill(dscounthk);
            sqlCon.Close();
            int totalhk = Convert.ToInt32(dscounthk.Tables[0].Rows[0]["total"]);

            sqlCon.Open();
            string srcountbjm = @"select count(*) as total from checkhk_parameter r join checkhk_perangkat t on t.id_perangkat=r.id_perangkat
                                where t.id_perangkat like '%' + 'bjm' + '%' ";
            DataSet dscountbjm = new DataSet();
            SqlCommand cmdbjm = new SqlCommand(srcountbjm, sqlCon);
            countbjm = new SqlDataAdapter(cmdbjm);
            countbjm.Fill(dscountbjm);
            sqlCon.Close();
            int totalbjm = Convert.ToInt32(dscountbjm.Tables[0].Rows[0]["total"]);

            sqlCon.Open();
            string querycheck = $@"select count(*) as total, nama from checkme_data d join checkme_parameter r on d.id_parameter=r.id_parameter
                        join checkme_perangkat p on p.id_perangkat=r.id_perangkat join Profile pro on pro.id_profile=d.id_profile  where d.tanggal='{tanggalku}' 
                        and d.nilai != '' and d.waktu='siang'
                        group by d.tanggal, nama";
            DataSet ds5 = new DataSet();
            SqlCommand cmd = new SqlCommand(querycheck, sqlCon);
            da5 = new SqlDataAdapter(cmd);
            da5.Fill(ds5);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
            if (ds5.Tables[0].Rows.Count > 0)
                output = Convert.ToInt32(ds5.Tables[0].Rows[0]["total"].ToString());
            double hasil, tampil;
            if (output > 0)
            {
                hasil = ((double)output / totalme) * 100;
                tampil = Math.Round(hasil);
                divsiang.Style.Add("width", $"{tampil}%");
                lblsiangme.Text = $"{tampil}% oleh {ds5.Tables[0].Rows[0]["nama"].ToString()}";
                asiang.Attributes["href"] = $"../checklistme/dashboard.aspx?waktu=siang&tanggal={mytanggal}";
            }
            else
            {
                asiang.Attributes["href"] = "../checklistme/harian.aspx";
            }


            sqlCon.Open();
            string querycheckpagi = $@"select count(*) as total, nama from checkme_data d join checkme_parameter r on d.id_parameter=r.id_parameter
                        join checkme_perangkat p on p.id_perangkat=r.id_perangkat join Profile pro on pro.id_profile=d.id_profile 
                        where d.tanggal='{tanggalku}' and d.nilai != '' and d.waktu='pagi'
                        group by d.tanggal, nama";
            DataSet ds6 = new DataSet();
            SqlCommand cmd1 = new SqlCommand(querycheckpagi, sqlCon);
            da6 = new SqlDataAdapter(cmd1);
            da6.Fill(ds6);
            cmd1.ExecuteNonQuery();
            sqlCon.Close();
            if (ds6.Tables[0].Rows.Count > 0)
                output1 = Convert.ToInt32(ds6.Tables[0].Rows[0]["total"].ToString());
            double hasil1, tampil1;
            if (output1 > 0)
            {
                hasil1 = ((double)output1 / totalme) * 100;
                tampil1 = Math.Round(hasil1);
                divpagi.Style.Add("width", $"{tampil1}%");
                lblpagime.Text = $"{tampil1}% oleh {ds6.Tables[0].Rows[0]["nama"].ToString()}";
                apagi.Attributes["href"] = $"../checklistme/dashboard.aspx?waktu=pagi&tanggal={mytanggal}";
            }
            else
            {
                apagi.Attributes["href"] = "../checklistme/harian.aspx";
            }

            sqlCon.Open();
            string querycheckmalam = $@"select count(*) as total, nama from checkme_data d join checkme_parameter r on d.id_parameter=r.id_parameter
                        join checkme_perangkat p on p.id_perangkat=r.id_perangkat join Profile pro on pro.id_profile=d.id_profile 
                        where d.tanggal='{tanggalkumalam}' and d.nilai != '' and d.waktu='malam'
                        group by d.tanggal, nama";
            DataSet ds7 = new DataSet();
            SqlCommand cmd2 = new SqlCommand(querycheckmalam, sqlCon);
            da7 = new SqlDataAdapter(cmd2);
            da7.Fill(ds7);
            cmd2.ExecuteNonQuery();
            sqlCon.Close();
            if (ds7.Tables[0].Rows.Count > 0)
                output2 = Convert.ToInt32(ds7.Tables[0].Rows[0]["total"].ToString());
            double hasil2, tampil2;
            if (output2 > 0)
            {
                hasil2 = ((double)output2 / totalme) * 100;
                tampil2 = Math.Round(hasil2);
                divmalam.Style.Add("width", $"{tampil2}%");
                lblmalamme.Text = $"{tampil2}% oleh {ds7.Tables[0].Rows[0]["nama"].ToString()}";
                amalam.Attributes["href"] = $"../checklistme/dashboard.aspx?waktu=malam&tanggal={tanggalkumalam}";
            }
            else
            {
                amalam.Attributes["href"] = "../checklistme/harian.aspx";
            }


            sqlCon.Open();              //Harkat cibinong
            string querycheckhk = $@"select count(*) as total, nama from checkhk_data d join Profile p on p.id_profile=d.id_profile join checkhk_parameter r
									on r.id_parameter=d.id_parameter
                                    where d.tanggal >= '{tanggalku} 00:00:00' and d.tanggal <= '{tanggalku} 23:59:59'
									and r.id_perangkat not like '%' + 'bjm' + '%' and d.data != ''
                                    group by CAST(d.tanggal as date), nama";
            DataSet ds8 = new DataSet();
            SqlCommand cmd3 = new SqlCommand(querycheckhk, sqlCon);
            da8 = new SqlDataAdapter(cmd3);
            da8.Fill(ds8);
            cmd3.ExecuteNonQuery();
            sqlCon.Close();
            if (ds8.Tables[0].Rows.Count > 0)
                output3 = Convert.ToInt32(ds8.Tables[0].Rows[0]["total"].ToString());
            double hasil3, tampil3;
            if (output3 > 0)
            {
                hasil3 = ((double)output3 / totalhk) * 100;
                tampil3 = Math.Round(hasil3);
                divhk.Style.Add("width", $"{tampil3}%");
                lblchharkat.Text = $"{tampil3}% oleh {ds8.Tables[0].Rows[0]["nama"].ToString()}";
                aharkat.Attributes["href"] = $"../checkhk/dashboard.aspx?tanggal={tanggalku}";
            }
            else
            {
                aharkat.Attributes["href"] = $"../checkhk/dashboard.aspx?tanggal={tanggalku}";
            }


            sqlCon.Open();              //Harkat Banjarmasin
            string querycheckbjm = $@"select count(*) as total, nama from checkhk_data d join Profile p on p.id_profile=d.id_profile join checkhk_parameter r
									on r.id_parameter=d.id_parameter
                                    where d.tanggal >= '{tanggalku} 00:00:00' and d.tanggal <= '{tanggalku} 23:59:59'
									and r.id_perangkat like '%' + 'bjm' + '%' and d.data != ''
                                    group by CAST(d.tanggal as date), nama";
            DataSet dsbjm = new DataSet();
            SqlCommand cmdbjm2 = new SqlCommand(querycheckbjm, sqlCon);
            dabjm = new SqlDataAdapter(cmdbjm2);
            dabjm.Fill(dsbjm);
            cmdbjm2.ExecuteNonQuery();
            sqlCon.Close();
            if (dsbjm.Tables[0].Rows.Count > 0)
                output4 = Convert.ToInt32(dsbjm.Tables[0].Rows[0]["total"].ToString());
            double hasilbjm, tampilbjm;
            if (output4 > 0)
            {
                hasilbjm = ((double)output4 / totalbjm) * 100;
                tampilbjm = Math.Round(hasilbjm);
                divbjm.Style.Add("width", $"{tampilbjm}%");
                lblbjm.Text = $"{tampilbjm}% oleh {dsbjm.Tables[0].Rows[0]["nama"].ToString()}";
                a1.Attributes["href"] = $"../checkbjm/dashboardbjm.aspx?tanggal={tanggalku}";
            }
            else
            {
                a1.Attributes["href"] = $"../checkbjm/dashboardbjm.aspx?tanggal={tanggalku}";
            }
            //Response.Write(output);
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            if (Request.Form["icon"] != null)
            {
                pilihicon = Request.Form["icon"].ToString();
                //Response.Write(pilihicon);
            }

            if (pilihicon == "sport")
                icon1 = "~/img/sport.png";
            else if (pilihicon == "makan")
                icon1 = "~/img/makan.png";
            else if (pilihicon == "rapat")
                icon1 = "~/img/meeting.png";
            else if (pilihicon == "holiday")
                icon1 = "~/img/holiday.jpg";
            else if (pilihicon == "manuver")
                icon1 = "~/img/rocket.png";
            else if (pilihicon == "vendor")
                icon1 = "~/img/vendor.png";

            string indexedit = Session["indexEvent"].ToString();

            int IDEdit = Convert.ToInt32(indexedit);
            sqlCon.Open();
            SqlCommand sqlCmd5 = new SqlCommand("EvUpdate", sqlCon);
            sqlCmd5.CommandType = CommandType.StoredProcedure;
            sqlCmd5.Parameters.AddWithValue("@ID", IDEdit);
            sqlCmd5.Parameters.AddWithValue("@event", txtEven.Text);
            sqlCmd5.Parameters.AddWithValue("@penyelenggara", txtPenyelenggara.Text);
            sqlCmd5.Parameters.AddWithValue("@jam", txttime.Value);
            sqlCmd5.Parameters.AddWithValue("@lokasi", txtLokasi.Text);
            sqlCmd5.Parameters.AddWithValue("@tanggal2", DateTime.ParseExact(txtttl.Value, "dd/MM/yyyy", null));
            sqlCmd5.Parameters.AddWithValue("@icon", icon1);
            sqlCmd5.Parameters.AddWithValue("@statususer", Session["jenis1"].ToString());
            sqlCmd5.ExecuteNonQuery();
            sqlCon.Close();
            Response.Redirect("dashboard2.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string indexedit = Session["indexEvent"].ToString();

            int IDEdit = Convert.ToInt32(indexedit);
            sqlCon.Open();
            SqlCommand sqlCmd5 = new SqlCommand("EvDelete", sqlCon);
            sqlCmd5.CommandType = CommandType.StoredProcedure;
            sqlCmd5.Parameters.AddWithValue("@ID", IDEdit);
            sqlCmd5.ExecuteNonQuery();
            sqlCon.Close();
            Response.Redirect("dashboard2.aspx");
        }


        void tableshift()
        {
            var mulai = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var sekarang = DateTime.Now.ToString("yyyy/MM/dd");
            var akhir = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            querylain = $@"SELECT m.tanggal_shift, jadwal, STUFF((SELECT ',  ' + p.petugas 
                      FROM shiftme s left join shiftme_petugas p on s.id_petugas = p.id_petugas
					  WHERE m.jadwal = s.jadwal and m.tanggal_shift = s.tanggal_shift
                      FOR XML PATH('')), 1, 1, '') [petugas] FROm shiftme m WHERE tanggal_shift >= '{mulai}' and tanggal_shift <= '{akhir}'
					  group by tanggal_shift, jadwal
                      order by tanggal_shift asc, jadwal asc ";

            SqlCommand cmd = new SqlCommand(querylain, sqlCon);
            dashift = new SqlDataAdapter(cmd);
            dashift.Fill(dsshift);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();
            style = "font-size:12px; font-weight:normal";

            for (int i = 0; i < dsshift.Tables[0].Rows.Count; i++)
            {
                DateTime datee = (DateTime)dsshift.Tables[0].Rows[i]["tanggal_shift"];
                string mydate = datee.ToString("yyyy/MM/dd");
                string myjadwal = dsshift.Tables[0].Rows[i]["jadwal"].ToString();
                if (mydate == mulai && myjadwal == "Pagi")
                    pagikemaren.InnerHtml = dsshift.Tables[0].Rows[i]["petugas"].ToString();
                else if (mydate == mulai && myjadwal == "Sore")
                    sorekemaren.InnerHtml = dsshift.Tables[0].Rows[i]["petugas"].ToString();
                else if (mydate == sekarang && myjadwal == "Pagi")
                    paginow.InnerHtml = dsshift.Tables[0].Rows[i]["petugas"].ToString();
                else if (mydate == sekarang && myjadwal == "Sore")
                    sorenow.InnerHtml = dsshift.Tables[0].Rows[i]["petugas"].ToString();
                else if (mydate == akhir && myjadwal == "Pagi")
                    pagitomorrow.InnerHtml = dsshift.Tables[0].Rows[i]["petugas"].ToString();
                else if (mydate == akhir && myjadwal == "Sore")
                    soretomorrow.InnerHtml = dsshift.Tables[0].Rows[i]["petugas"].ToString();

                /*if (i==0)
                    pagikemaren.InnerHtml = dsshift.Tables[0].Rows[0]["petugas"].ToString();
                else if(i==1)
                    sorekemaren.InnerHtml = dsshift.Tables[0].Rows[1]["petugas"].ToString();
                else if(i==2)
                    paginow.InnerHtml = dsshift.Tables[0].Rows[2]["petugas"].ToString();
                else if (i == 3)
                    sorenow.InnerHtml = dsshift.Tables[0].Rows[3]["petugas"].ToString();
                else if (i == 4)
                    pagitomorrow.InnerHtml = dsshift.Tables[0].Rows[4]["petugas"].ToString();
                else if (i == 5)
                    soretomorrow.InnerHtml = dsshift.Tables[0].Rows[5]["petugas"].ToString();*/
            }
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "id")
            {

                int index = Convert.ToInt32(e.CommandArgument);

                sqlCon.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("EvViewByID", sqlCon);
                sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlda.SelectCommand.Parameters.AddWithValue("@IDEvent", index);
                DataTable dtbl8 = new DataTable();
                sqlda.Fill(dtbl8);
                sqlCon.Close();
                string tanggal1 = dtbl8.Rows[0]["tanggal"].ToString();
                string tanggalacara = tanggal1.Remove(10, 9);
                txtEven.Text = dtbl8.Rows[0]["event"].ToString();
                txtPenyelenggara.Text = dtbl8.Rows[0]["penyelenggara"].ToString();
                txtttl.Value = tanggalacara;
                txttime.Value = dtbl8.Rows[0]["jam"].ToString();
                txtLokasi.Text = dtbl8.Rows[0]["lokasi"].ToString();

                btnedit.Visible = true;
                btnhapus.Visible = true;
                btntambah.Visible = false;

                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#modalupdate1').modal('show')", true);

                Session["indexEvent"] = index;
                //Label labelEdit = (Label)e.Item.FindControl("lbEdit");
                //labelSunting.Visible = false;
                //labelEdit.Visible = true;
            }
        }

        protected void btntambah_Click(object sender, EventArgs e)
        {
            if (Request.Form["icon"] != null)
            {
                pilihicon = Request.Form["icon"].ToString();
                //Response.Write(pilihicon);
            }

            if (pilihicon == "sport")
                icon1 = "~/img/sport.png";
            else if (pilihicon == "makan")
                icon1 = "~/img/makan.png";
            else if (pilihicon == "rapat")
                icon1 = "~/img/meeting.png";
            else if (pilihicon == "holiday")
                icon1 = "~/img/holiday.jpg";
            else if (pilihicon == "manuver")
                icon1 = "~/img/rocket.png";
            else if (pilihicon == "vendor")
                icon1 = "~/img/vendor.png";

            //string indexedit = Session["indexEvent"].ToString();

            queryaddev = $@"INSERT INTO Event (tanggal, jam, event, lokasi, icon, penyelenggara, statususer) VALUES
                           ('{txtttl.Value}', '{txttime.Value}', '{txtEven.Text}', '{txtLokasi.Text}', '{icon1}', '{txtPenyelenggara.Text}', '{Session["jenis1"].ToString()}')";
            sqlCon.Open();
            SqlCommand sqlCmd5 = new SqlCommand(queryaddev, sqlCon);
            sqlCmd5.ExecuteNonQuery();
            sqlCon.Close();
            Response.Redirect("dashboard2.aspx");
        }
    }
}