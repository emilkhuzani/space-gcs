﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace Telkomsat.checklistme.semester
{
    public partial class isidata : System.Web.UI.Page
    {
        SqlDataAdapter da, da5;
        DataSet ds = new DataSet();
        DataSet ds5 = new DataSet();
        StringBuilder htmlTable = new StringBuilder();
        StringBuilder htmlTable2 = new StringBuilder();
        string IDdata = "kitaa", Perangkat = "st", querytanggal = "a", query, waktu = "", nilai = "", style4 = "a", style3, SN = "a", statusticket = "a", queryfav, queydel, jenisview = "";
        string Parameter = "a", query2 = "A", idddl = "s", value = "1", idtxt = "A", loop = "", ruangan, tipe, satuan, room, query1, valuetgl, inisial,tahun, semester, user, tanggal1;
        string[] words = { "a", "a" };
        string[] akhir;
        double hasil, tampil, total, diisi, weeknumber, weeknum;
        int j = 0, k, m;
        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            string query5;

            if (Request.QueryString["room"] != null)
            {
                room = Request.QueryString["room"];
                lblroom.Text = room;
            }

            if (Session["iduser"] != null)
            {
                user = Session["iduser"].ToString();
            }

            DateTime now = DateTime.Now;
            DateTime startdate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime middate = new DateTime(DateTime.Now.Year, 6, 30);
            DateTime enddate = new DateTime(DateTime.Now.Year, 12, 30);
            tahun = DateTime.Now.Year.ToString();

            if (now > startdate && now <= middate)
            {
                semester = "1";
            }
            else if (now > middate && now <= enddate)
            {
                semester = "2";
            }

            query5 = $@"select * from checkme_datawmy d join checkme_parameterwmy r on r.id_parameter=d.id_parameter join
                        checkme_perangkatwmy p on p.id_perangkat=r.id_perangkat where semester = '{semester}' and tahun = '{tahun}' and ruangan='{room}' order by r.id_perangkat";
            sqlCon.Open();
            SqlCommand cmd5 = new SqlCommand(query5, sqlCon);
            da5 = new SqlDataAdapter(cmd5);
            da5.Fill(ds5);
            cmd5.ExecuteNonQuery();
            sqlCon.Close();

            if (ds5.Tables[0].Rows.Count > 0)
                Response.Redirect($"updatedata.aspx?room={room}");
            tablepersen();
            tableticket();
        }

        void tablepersen()
        {
            DateTime now = DateTime.Now;
            DateTime startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime enddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            string style, class1, ruang, alias, querytotal, queryisi;
            SqlDataAdapter daheader, dapersen, dabar;
            DataSet dsheader = new DataSet();
            DataSet dspersen = new DataSet();
            DataSet dsbar = new DataSet();
            DateTime wib;
            double hari = Convert.ToInt32(DateTime.Now.DayOfYear.ToString());
            double minggu = (double)Math.Ceiling(hari / 7);

            string queryheader = $@"select ruangan from checkme_perangkatwmy t join checkme_parameterwmy r on t.id_perangkat=r.id_perangkat
                                    where r.kategori='semester' group by ruangan";
            sqlCon.Open();
            SqlCommand cmdheader = new SqlCommand(queryheader, sqlCon);
            daheader = new SqlDataAdapter(cmdheader);
            daheader.Fill(dsheader);
            cmdheader.ExecuteNonQuery();
            sqlCon.Close();
            style = "font-weight:bold";
            wib = DateTime.UtcNow + new TimeSpan(7, 0, 0);
            tanggal1 = wib.ToString("yyyy/MM/dd");
            if (dsheader.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsheader.Tables[0].Rows.Count; ++i)
                {
                    dsbar.Clear();
                    ruang = dsheader.Tables[0].Rows[i]["ruangan"].ToString();
                    querytotal = $@"select COUNT(*) as total from checkme_parameterwmy r join checkme_perangkatwmy t on r.id_perangkat=t.id_perangkat where ruangan = '{ruang}' and r.kategori='semester'";
                    sqlCon.Open();
                    SqlCommand cmdruang = new SqlCommand(querytotal, sqlCon);
                    dapersen = new SqlDataAdapter(cmdruang);
                    dapersen.Fill(dspersen);
                    cmdruang.ExecuteNonQuery();
                    sqlCon.Close();

                    queryisi = $@"select COUNT(*) as isi from checkme_datawmy d join checkme_parameterwmy r on d.id_parameter=r.id_parameter
                                    join checkme_perangkatwmy t on r.id_perangkat=t.id_perangkat where ruangan = '{ruang}' and d.nilai not like '%' + 'no' + '%' and semester='{semester}' and tahun='{tahun}'";
                    sqlCon.Open();
                    SqlCommand cmdisi = new SqlCommand(queryisi, sqlCon);
                    dabar = new SqlDataAdapter(cmdisi);
                    dabar.Fill(dsbar);
                    cmdisi.ExecuteNonQuery();
                    sqlCon.Close();
                    if (dsbar.Tables[0].Rows.Count > 0)
                    {
                        diisi = Convert.ToDouble(dsbar.Tables[0].Rows[0]["isi"].ToString());
                    }
                    else
                    {
                        diisi = 0;
                    }

                    total = Convert.ToDouble(dspersen.Tables[0].Rows[i]["total"].ToString());

                    hasil = ((double)diisi / total) * 100;

                    if (hasil == 100)
                    {
                        class1 = "progress-bar-green";
                    }
                    else
                    {
                        class1 = "progress-bar-red";
                    }
                    tampil = Math.Round(hasil);
                    if ((i % 2) == 0)
                    {
                        if (i > 1)
                            htmlTable2.Append("</div>");
                        htmlTable2.Append("<div class=\"col-md-12\">");
                    }
                    htmlTable2.Append("<div class=\"progress-group\">");
                    htmlTable2.Append($"<a class=\"link\" href=\"isidata.aspx?room={ruang}\" style=\"font-size:12px;\">" + ruang + "</a>");
                    htmlTable2.Append($"<span class=\"progress-number\">" + tampil + "%</span>");
                    htmlTable2.Append("<div class=\"progress sm\">");
                    htmlTable2.Append($"<div class=\"progress-bar {class1}\" style=\"width:{tampil}% \"></div>");
                    htmlTable2.Append($"</div></div>");
                    //Response.Write(queryisi);
                }

                PlaceHolder1.Controls.Add(new Literal { Text = htmlTable2.ToString() });
            }
            /*Label[] arr = new Label[] { lblheader1, lblheader2, lblheader3, lblheader4, lblheader5, lblheader6, lblheader7, lblheader8, lblheader9, lblheader10,
            lblheader11, lblheader12, lblheader13, lblheader14, lblheader15, lblheader16, lblheader17, lblheader18, lblheader19, lblheader20,
            lblheader21, lblheader22, lblheader23};
            for (int i = 0; i < dsheader.Tables[0].Rows.Count; ++i)
            {
                arr[i].Text = dsheader.Tables[0].Rows[i]["ruangan"].ToString();
                arr[i].Font.Bold = true;
            }*/
        }


        protected void Pilih_Click(object sender, EventArgs e)
        {
            
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect($"editharian.aspx?room={room}&waktu=6");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            string data = string.Join(",", akhir);
            query1 = $"insert into checkme_datawmy (tanggal, id_profile, id_parameter, jenis, nilai, semester, tahun, tanggal_kerja) values {data}";
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(query1, sqlCon);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
            Session["inisialsemester"] = null;

            string tanggalku = DateTime.Now.ToString("yyyy/MM/dd");
            string querylog = $@"Insert into log (id_profile, tanggal, tipe, judul) values
                                ('{user}', '{tanggalku}', 'mainme', 'maintenance semester ME')";
            sqlCon.Open();
            SqlCommand cmdlog = new SqlCommand(querylog, sqlCon);
            cmdlog.ExecuteNonQuery();
            sqlCon.Close();

            this.ClientScript.RegisterStartupScript(this.GetType(), "clientClick", "fungsi()", true);
            Response.Redirect($"checkdata.aspx");
        }

        void tableticket()
        {
            if (Session["inisialsemester"] != null)
                query = $@"select r.id_parameter, p.Perangkat, r.satuan, p.sn, p.ruangan, r.parameter, r.tipe, d.nilai from checkme_parameterwmy r left join
                            checkme_perangkatwmy p on p.id_perangkat = r.id_perangkat left join checkme_datawmy d on d.id_parameter = r.id_parameter
                            where ruangan = '{room}' AND d.tanggal = (SELECT MAX(tanggal) from checkme_datawmy d LEFT join checkme_parameterwmy r 
                            on r.id_parameter=d.id_parameter left join checkme_perangkatwmy p on p.ID_Perangkat = r.ID_Perangkat
                            where p.ruangan = '{room}' and d.nilai is not null) and d.jenis = 'semester' order by r.id_perangkat";
            else
                query = $@"select r.id_parameter, p.Perangkat, r.satuan, p.sn, p.ruangan, r.parameter, r.tipe from checkme_parameterwmy r left join
                        checkme_perangkatwmy p on p.id_perangkat = r.id_perangkat where ruangan = '{room}' and kategori = 'semester' order by r.id_perangkat";


            string tanggal = DateTime.Now.ToString("yyyy/MM/dd");

            SqlCommand cmd = new SqlCommand(query, sqlCon);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();


            htmlTable.Append("<table id=\"example2\" width=\"100%\" class=\"table table-bordered table-hover table-striped\">");
            htmlTable.Append("<thead>");
            htmlTable.Append("<tr><th>Perangkat</th><th>Serial Number</th><th>Parameter</th><th style=\"min-width:100px\">Nilai</th><th>Satuan</th><th>Tanggal Kerja</th></tr>");
            htmlTable.Append("</thead>");

            htmlTable.Append("<tbody>");

            int count = ds.Tables[0].Rows.Count;
            string[] looping = new string[count];
            string[] loopingtgl = new string[count];
            akhir = new string[count];
            if (!object.Equals(ds.Tables[0], null))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        IDdata = ds.Tables[0].Rows[i]["id_parameter"].ToString();
                        Perangkat = ds.Tables[0].Rows[i]["Perangkat"].ToString();
                        SN = ds.Tables[0].Rows[i]["sn"].ToString();
                        Parameter = ds.Tables[0].Rows[i]["Parameter"].ToString();
                        ruangan = ds.Tables[0].Rows[i]["ruangan"].ToString();
                        tipe = ds.Tables[0].Rows[i]["tipe"].ToString();
                        satuan = ds.Tables[0].Rows[i]["satuan"].ToString();

                        style3 = "font-weight:normal";

                        idtxt = "txt" + IDdata;
                        idddl = "ddl" + IDdata;

                        //Response.Write(Session["jenis1"].ToString());
                        //HiddenField1.Value = IDdata;
                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + Perangkat + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + SN + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + Parameter + "</label>" + "</td>");
                        if (tipe == "N")
                            htmlTable.Append("<td>" + $"<input type =\"text\" value=\"{nilai}\" runat=\"server\" class=\"form-control\" name=\"idticket\" id={idtxt}>" + "</td>");
                        else if (tipe == "BN")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value =\"NO BIR\"> NO BIR </option><option value=\"BIR\" > BIR </option></select > " + " </td>");
                        else if (tipe == "OC")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value =\"CLOSE\"> CLOSE </option><option value=\"OPEN\" > OPEN </option></select > " + " </td>");
                        else if (tipe == "FL")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value =\"LOW\"> LOW </option><option value=\"FULL\" > FULL </option></select > " + " </td>");
                        else if (tipe == "AN")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value =\"NOT YET\"> NOT YET </option><option value=\"ALREADY\" > ALREADY </option></select > " + " </td>");

                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + satuan + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<input type=\"text\" class=\"form-control tgldata\" name=\"tgl\" autocomplete=\"off\" />" + "</td>");
                        htmlTable.Append("</tr>");
                        value = Request.Form["idticket"];
                        //Response.Write(value);
                        valuetgl = Request.Form["tgl"];
                        looping[i] = IDdata;

                        //Response.Write( "bisa" + words[1]);
                        int j = i - 1;
                        //Response.Write(seats);
                        //looping = "('" + IDdata + "'," + seats + "),";
                        loop = IDdata + "," + loop;

                    }
                    if (valuetgl != null)
                    {
                        string[] datelines = Regex.Split(valuetgl, ",");

                        foreach (string dateline in datelines)
                        {
                            loopingtgl[m] = dateline;
                            m++;
                        }
                    }

                    if (value != null)
                    {
                        string[] lines = Regex.Split(value, ",");

                        foreach (string line in lines)
                        {
                            //Response.Write(line);
                            akhir[j] = "('" + tanggal + "','" + "3" + "','" + looping[j] + "','" + "semester" + "','" + line + "','" + semester + "','" + tahun + "','" + loopingtgl[j] + "')";
                            j++;
                        }
                    }
                    //Response.Write(string.Join("\n", akhir));
                    htmlTable.Append("</tbody>");
                    htmlTable.Append("</table>");

                    DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
                }
            }
        }
    }
}