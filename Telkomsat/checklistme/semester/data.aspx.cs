﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Telkomsat.checklistme.semester
{
    public partial class data : System.Web.UI.Page
    {
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        StringBuilder htmlTable = new StringBuilder();
        StringBuilder htmlTable2 = new StringBuilder();
        string IDdata = "kitaa", Perangkat = "st", style1 = "a", query, waktu = "", nilai = "", style4 = "a", style3, SN = "a", statusticket = "a", queryfav, queydel, jenisview = "";
        string Parameter = "a", query2 = "A", tanggalku = "s", value = "1", idtxt = "A", loop = "", ruangan, tipe, satuan, room, start, end, inisial, siang = "", malam = "", tanggal1;
        string[] words = { "a", "a" };
        string[] akhir;
        string startsemester, endsemester, mingguan, semester, tahunan;
        double hasil, tampil, total, diisi, weeknumber, weeknum;
        int j = 0, startbulan, endint;
        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["semester"] != null)
            {
                semester = Request.QueryString["semester"];
                tahunan = Request.QueryString["tahun"];
            }

            if (Request.QueryString["room"] != null)
            {
                room = Request.QueryString["room"].ToString();
            }

            lblroom.Text = room;
            lblsemester.Text = "Data Semester " + semester + " " + tahunan;
            tanggalku = tanggal1;
            query = $@"select p.ruangan, r.id_parameter, p.Perangkat, r.satuan, p.sn, r.parameter, r.tipe, d.nilai, d.tanggal, d.tanggal_kerja from checkme_parameterwmy r left join
                    checkme_perangkatwmy p on p.id_perangkat = r.id_perangkat left join checkme_datawmy d on d.id_parameter = r.id_parameter
                    and d.semester = '{semester}' and d.tahun = '{tahunan}' where ruangan='{room}' and r.kategori='semester' order by r.id_perangkat";
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

            DateTime dta = new DateTime(2020, 1, 1);
            while (dta.DayOfWeek != DayOfWeek.Monday)
            {
                dta = dta.AddDays(1);
            }

            weeknumber = Convert.ToInt32(DateTime.Now.DayOfYear) - Convert.ToInt32(dta.ToString("dd"));                                                                     //Mingguan
            weeknum = (double)Math.Ceiling(weeknumber / 7);
            mingguan = weeknum.ToString();

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
                                    join checkme_perangkatwmy t on r.id_perangkat=t.id_perangkat where ruangan = '{ruang}' and d.nilai not like '%' + 'no' + '%' and semester='{semester}' and tahun='{tahunan}'";
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
                    htmlTable2.Append($"<a class=\"link\" href=\"data.aspx?room={ruang}&semester={semester}&tahun={tahunan}\" style=\"font-size:12px;\">" + ruang + "</a>");
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


        protected void Filter_ServerClick(object sender, EventArgs e)
        {
            /*if (ddlruang.SelectedValue == "ruangan")
                room = "ruangan";
            else
                room = "'" + ddlruang.Text + "'";

            if (ddlbulan.Text == "Semester 1")
            {
                startsemester = "1";
                endsemester = "6";
                endint = Convert.ToInt32(ddltahun.Text);
            }
            else
            {
                startsemester = "6";
                endsemester = "1";
                endint = Convert.ToInt32(ddltahun.Text) + 1;
            }

            start = ddltahun.Text + "/" + startsemester + "/01";
            end = endint + "/" + endsemester + "/01";
            */
            query = $@"select p.ruangan, r.id_parameter, p.Perangkat, r.satuan, p.sn, r.parameter, r.tipe, d.nilai, d.tanggal, d.tanggal_kerja from checkme_parameterwmy r left join
                    checkme_perangkatwmy p on p.id_perangkat = r.id_perangkat left join checkme_datawmy d on d.id_parameter = r.id_parameter
                    where '{start}' <= d.tanggal and d.tanggal < '{end}' and p.ruangan = {room} and d.jenis='semester' order by r.id_perangkat";
            tableticket();
        }

        void tableticket()
        {

            string tanggal = DateTime.Now.ToString("yyyy/MM/dd");

            SqlCommand cmd = new SqlCommand(query, sqlCon);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();


            htmlTable.Append("<table id=\"example2\" width=\"100%\" class=\"table table-bordered table-hover table-striped\">");
            htmlTable.Append("<thead>");
            htmlTable.Append("<tr><th>Perangkat</th><th>Serial Number</th><th>Parameter</th><th>Nilai</th><th>Tanggal Kerja</th></tr>");
            htmlTable.Append("</thead>");

            htmlTable.Append("<tbody>");

            int count = ds.Tables[0].Rows.Count;
            string[] looping = new string[count];
            akhir = new string[count];
            if (!object.Equals(ds.Tables[0], null))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblkosong.Visible = false;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DateTime odate = (DateTime)ds.Tables[0].Rows[i]["tanggal_kerja"];
                        tanggal = odate.ToString("dd/MM/yyyy");
                        IDdata = ds.Tables[0].Rows[i]["id_parameter"].ToString();
                        Perangkat = ds.Tables[0].Rows[i]["Perangkat"].ToString();
                        SN = ds.Tables[0].Rows[i]["sn"].ToString();
                        Parameter = ds.Tables[0].Rows[i]["Parameter"].ToString();
                        ruangan = ds.Tables[0].Rows[i]["ruangan"].ToString();
                        tipe = ds.Tables[0].Rows[i]["tipe"].ToString();
                        satuan = ds.Tables[0].Rows[i]["satuan"].ToString();

                        if (tanggal == "01/01/1900")
                            tanggal = "";

                        style3 = "font-weight:normal";
                        nilai = ds.Tables[0].Rows[i]["nilai"].ToString();

                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + Perangkat + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + SN + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + Parameter + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + nilai + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + tanggal + "</label>" + "</td>");
                        htmlTable.Append("</tr>");
                        value = Request.Form["idticket"];
                    }
                    htmlTable.Append("</tbody>");
                    htmlTable.Append("</table>");

                    DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
                }
                else
                {
                    
                }
            }
        }
    }
}