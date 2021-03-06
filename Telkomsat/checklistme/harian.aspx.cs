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

namespace Telkomsat.checklistme
{
    public partial class harian : System.Web.UI.Page
    {
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        StringBuilder htmlTable1 = new StringBuilder();
        StringBuilder htmlTable = new StringBuilder();
        string IDdata = "kitaa", Perangkat = "st", querytanggal = "a", query, waktu = "", nilai = "", style4 = "a", style3, SN = "a", statusticket = "a", queryfav, queydel, jenisview = "";
        string saat, user, datee ;
        
        string Parameter = "a", query2 = "A", idddl = "s", value = "1", idtxt = "A", loop = "", ruangan, tipe, satuan, room, query1, date, inisial;
        string[] words = { "a", "a" };
        string[] akhir;
        int j = 0, k;
        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["room"] != null)
            {
                room = Request.QueryString["room"];
                lblroom.Text = room;
                if (!IsPostBack)
                    Session["inisialmeh"] = null;
            }
            else
            {
                Button1.Visible = false;
            }

            if (Request.QueryString["inisialmeh"] == null)
            {
                query = $@"select r.id_parameter, p.Perangkat, r.satuan, p.alias, p.sn, p.ruangan, r.parameter, r.tipe from checkme_parameter r left join
                        checkme_perangkat p on p.id_perangkat = r.id_perangkat where ruangan = '{room}' order by r.urutan, r.id_perangkat";
            }
            else
            {
                query = $@"select r.id_parameter, p.Perangkat, r.satuan, p.alias, p.sn, p.ruangan, r.parameter, r.tipe, d.nilai from checkme_parameter r left join
                    checkme_perangkat p on p.id_perangkat = r.id_perangkat left join checkme_data d on d.id_parameter = r.id_parameter
                    where ruangan = '{room}' AND d.tanggal = (SELECT MAX(tanggal) from checkme_data d LEFT join checkme_parameter r 
					on r.ID_Parameter=d.id_parameter left join checkme_perangkat p on p.ID_Perangkat = r.ID_Perangkat
					where p.ruangan = '{room}' and d.nilai is not null and d.waktu='siang') and d.waktu = 'siang' order by r.urutan, r.id_perangkat";
            }

            if (Session["iduser"] != null)
            {
                user = Session["iduser"].ToString();
            }

            TimeSpan satu = new TimeSpan(5, 0, 0); //10 o'clock
            TimeSpan dua = new TimeSpan(13, 0, 0); //12 o'clock
            TimeSpan tiga = new TimeSpan(19, 0, 0); //12 o'clock
            TimeSpan empat = new TimeSpan(24, 0, 0);
            
            if (!IsPostBack)
            {
                DateTime wib = DateTime.UtcNow + new TimeSpan(7, 0, 0);
                TimeSpan wibTime = wib.TimeOfDay;

                if ((wibTime >= satu) && (wibTime < dua))
                {
                    date = wib.ToString("yyyy/MM/dd");
                    waktu = "pagi";
                    DropDownList1.Text = "pagi";
                }
                else if ((wibTime >= dua) && (wibTime < tiga))
                {
                    date = wib.ToString("yyyy/MM/dd");
                    waktu = "siang";
                    DropDownList1.Text = "siang";
                }
                else if ((wibTime >= tiga) && (wibTime < empat))
                {
                    date = wib.ToString("yyyy/MM/dd");
                    waktu = "malam";
                    DropDownList1.Text = "malam";
                }
                else
                {
                    date = (wib - new TimeSpan(1, 0, 0, 0)).ToString("yyyy/MM/dd");
                    waktu = "malam";
                    DropDownList1.Text = "malam";
                }
            }

            query2 = $@"select count(*) from checkme_parameter r left join
                    checkme_perangkat p on p.id_perangkat = r.id_perangkat left join checkme_data d on d.id_parameter = r.id_parameter
                    where ruangan = '{room}' AND d.tanggal = '{date}' and d.waktu = '{waktu}'";
            sqlCon.Open();
            SqlCommand cmd5 = new SqlCommand(query2, sqlCon);
            string output = cmd5.ExecuteScalar().ToString();
            int jenischeck = Convert.ToInt32(output);
            sqlCon.Close();
            if (jenischeck >= 1)
            {
                lbledit.Visible = true;
                lbledit.Text = $"Data checklist sudah terisi untuk tanggal {date} pada waktu {waktu} klik untuk ";
                LinkButton1.Visible = true;
                Button1.Visible = false;
            }
            else
            {           
                tableticket();
            }
            mytable();
               
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            TimeSpan satu = new TimeSpan(6, 0, 0); //10 o'clock
            TimeSpan dua = new TimeSpan(13, 0, 0); //12 o'clock
            TimeSpan tiga = new TimeSpan(19, 0, 0); //12 o'clock
            TimeSpan empat = new TimeSpan(24, 0, 0);
            DateTime wib = DateTime.UtcNow + new TimeSpan(7, 0, 0);
            TimeSpan wibTime = wib.TimeOfDay;

            if ((wibTime >= satu) && (wibTime < dua))
            {
                date = wib.ToString("yyyy/MM/dd");
                waktu = "pagi";
                DropDownList1.Text = "pagi";
            }
            else if ((wibTime >= dua) && (wibTime < tiga))
            {
                date = wib.ToString("yyyy/MM/dd");
                waktu = "siang";
                DropDownList1.Text = "siang";
            }
            else if ((wibTime >= tiga) && (wibTime < empat))
            {
                date = wib.ToString("yyyy/MM/dd");
                waktu = "malam";
                DropDownList1.Text = "malam";
            }
            else if ((wibTime >= empat) && (wibTime < satu))
            {
                date = (wib - new TimeSpan(1, 0, 0, 0)).ToString("yyyy/MM/dd");
                waktu = "malam";
                DropDownList1.Text = "malam";
            }
            Response.Redirect($"editharian.aspx?room={room}&waktu={DropDownList1.Text}");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DateTime wib = DateTime.UtcNow + new TimeSpan(7, 0, 0);

            TimeSpan satu = new TimeSpan(5, 0, 0); //10 o'clock
            TimeSpan dua = new TimeSpan(13, 0, 0); //12 o'clock
            TimeSpan tiga = new TimeSpan(19, 0, 0); //12 o'clock
            TimeSpan empat = new TimeSpan(24, 0, 0);
            TimeSpan wibTime = wib.TimeOfDay;
            string mydate = wib.ToString("yyyy/MM/dd");

            /*querytanggal = $"insert into checkme_tanggal (me_tanggal, id_profile) values ('{date}', '3')";
            
                //Console.Write(query1);
            sqlCon.Open();
            SqlCommand cmd7 = new SqlCommand(querytanggal, sqlCon);
            cmd7.ExecuteNonQuery();
            sqlCon.Close();*/

            string myquery = $"SELECT * FROM checkme_rekap WHERE tanggal = '{mydate}'";
            string myquery2;
            sqlCon.Open();
            SqlCommand mycmd = new SqlCommand(myquery, sqlCon);
            SqlDataAdapter da2 = new SqlDataAdapter(mycmd);
            mycmd.ExecuteNonQuery();
            sqlCon.Close();
            DataSet ds2 = new DataSet();
            da2.Fill(ds2);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if ((wibTime >= satu) && (wibTime < dua))
                {
                    myquery2 = $"UPDATE checkme_rekap SET pagi={user} WHERE id_rekap = {ds2.Tables[0].Rows[0]["id_rekap"].ToString()}";
                }
                else if ((wibTime >= dua) && (wibTime < tiga))
                {
                    myquery2 = $"UPDATE checkme_rekap SET siang={user} WHERE id_rekap = {ds2.Tables[0].Rows[0]["id_rekap"].ToString()}";
                }
                else if ((wibTime >= tiga) && (wibTime < empat))
                {
                    myquery2 = $"UPDATE checkme_rekap SET malam={user} WHERE id_rekap = {ds2.Tables[0].Rows[0]["id_rekap"].ToString()}";
                }
                else
                    myquery2 = $"UPDATE checkme_rekap SET malam={user} WHERE tanggal = '{date}'";
            }
            else
            {
                if ((wibTime >= satu) && (wibTime < dua))
                {
                    myquery2 = $"INSERT INTO checkme_rekap (tanggal, pagi) VALUES ('{mydate}', {user})";
                }
                else if ((wibTime >= dua) && (wibTime < tiga))
                {
                    myquery2 = $"INSERT INTO checkme_rekap (tanggal, siang) VALUES ('{mydate}', {user})";
                }
                else if ((wibTime >= tiga) && (wibTime < empat))
                {
                    myquery2 = $"INSERT INTO checkme_rekap (tanggal, malam) VALUES ('{mydate}', {user})";
                }
                else
                    myquery2 = $"INSERT INTO checkme_rekap (tanggal, malam) VALUES ('{date}', {user})";
            }
            sqlCon.Open();
            SqlCommand newcmd = new SqlCommand(myquery2, sqlCon);
            newcmd.ExecuteNonQuery();
            sqlCon.Close();

            string data = string.Join(",", akhir);
            query1 = $"insert into checkme_data (tanggal, id_profile, id_parameter, waktu, nilai) values {data}";
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(query1, sqlCon);
            cmd.ExecuteNonQuery();
            sqlCon.Close();

            string tanggalku = DateTime.Now.ToString("yyyy/MM/dd");
            string query5 = $"select * from log where judul='harian me' and tanggal = '{tanggalku}' and keterangan = '{DropDownList1.Text}'";
            SqlDataAdapter da5;
            DataSet ds5 = new DataSet();
            SqlCommand cmd5 = new SqlCommand(query5, sqlCon);
            da5 = new SqlDataAdapter(cmd5);
            da5.Fill(ds5);
            sqlCon.Open();
            cmd5.ExecuteNonQuery();
            sqlCon.Close();


            if (ds5.Tables[0].Rows.Count == 0)
            {
                string querylog = $@"Insert into log (id_profile, tanggal, tipe, judul, keterangan) values
                                ('{user}', '{tanggalku}', 'tchme', 'checklist harian me', '{DropDownList1.Text}' )";
                sqlCon.Open();
                SqlCommand cmdlog = new SqlCommand(querylog, sqlCon);
                cmdlog.ExecuteNonQuery();
                sqlCon.Close();
            }

            lblsave.Visible = true;
            Button1.Enabled = true;
            Session["inisialmeh"] = null;
            this.ClientScript.RegisterStartupScript(this.GetType(), "clientClick", "fungsi()", true);
            Response.Redirect($"dashboard.aspx?tanggal={mydate}&waktu={DropDownList1.Text}");
        }

        protected void inisialisasi_Click(object sender, EventArgs e)
        {
            Response.Redirect($"harian.aspx?room={room}&inisialmeh=ya");
        }

        void mytable()
        {
            SqlDataAdapter da, da1;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            string myquery, query, color, namaall, room;

            myquery = $@"select ruangan from checkme_perangkat group by ruangan"; 

            SqlCommand cmd = new SqlCommand(myquery, sqlCon);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();

            htmlTable1.Append("<ul class=\"uli\">");
            if (!object.Equals(ds.Tables[0], null))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        room = ds.Tables[0].Rows[i]["ruangan"].ToString();
                        ds1.Clear();
                        namaall = ds.Tables[0].Rows[i]["ruangan"].ToString();
                        query = $@"select t.ruangan from checkme_data d join checkme_parameter p on p.ID_Parameter=d.id_parameter join checkme_perangkat t
                                on t.id_perangkat=p.ID_Perangkat where d.tanggal = '{date}' and d.waktu = '{waktu}' and t.ruangan = '{namaall}'  group by t.ruangan";

                        SqlCommand cmd1 = new SqlCommand(query, sqlCon);
                        da1 = new SqlDataAdapter(cmd1);
                        da1.Fill(ds1);
                        sqlCon.Open();
                        cmd1.ExecuteNonQuery();
                        sqlCon.Close();

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            color = "green; color:white";
                        }
                        else
                        {
                            color = "white; color:black";
                        }
                        htmlTable1.Append($"<li class=\"myli\" style=\"background-color:{color}\"><a style=\"color:{color}\" href=\"harian.aspx?room={room}\">{room}</a></li>");
                    }
                    htmlTable1.Append("</ul>");
                    PlaceHolder1.Controls.Add(new Literal { Text = htmlTable1.ToString() });
                }
            }
        }


        void tableticket()
        {

            string tanggal = DateTime.Now.ToString("yyyy/MM/dd");
            //Response.Write(query);
            SqlCommand cmd = new SqlCommand(query, sqlCon);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();


            htmlTable.Append("<table id=\"example2\" width=\"100%\" class=\"table table-bordered table-hover table-striped\">");
            htmlTable.Append("<thead>");
            htmlTable.Append("<tr><th>Perangkat</th><th>Serial Number</th><th>AS</th><th>Parameter</th><th style=\"min-width:100px\">Nilai Parameter</th><th>Satuan</th></tr>");
            htmlTable.Append("</thead>");

            htmlTable.Append("<tbody>");

            int count = ds.Tables[0].Rows.Count;
            string[] looping = new string[count];
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

                        style3 = "font-weight:normal; font-size:12px;";

                        idtxt = "txt" + IDdata;
                        idddl = "ddl" + IDdata;

                        if(Request.QueryString["inisialmeh"] != null)
                            nilai = ds.Tables[0].Rows[i]["nilai"].ToString();
                        //Response.Write(Session["jenis1"].ToString());
                        //HiddenField1.Value = IDdata;
                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + Perangkat + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + SN + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + ds.Tables[0].Rows[i]["alias"].ToString() + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + Parameter + "</label>" + "</td>");
                        if (tipe == "N")
                            htmlTable.Append("<td>" + $"<input type =\"text\" value=\"{nilai}\" runat=\"server\" class=\"form-control\" name=\"idticket\" id={idtxt}>" + "</td>");
                        else if(tipe == "BN")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"BIR\" > BIR </option><option value =\"NO BIR\"> NO BIR </option></select > " + " </td>");
                        else if (tipe == "OC")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"OPEN\" > OPEN </option><option value =\"CLOSE\"> CLOSE </option></select > " + " </td>");
                        else if (tipe == "FL")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"FULL\" > FULL </option><option value =\"LOW\"> LOW </option></select > " + " </td>");
                        else if (tipe == "AMO")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"AUTO\" > AUTO </option><option value =\"MANUAL\"> MANUAL </option><option value =\"OFF\"> OFF </option></select > " + " </td>");
                        else if (tipe == "ARS")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"AUTO\" > AUTO </option><option value =\"RUN\"> RUN </option><option value =\"STOP\"> STOP </option></select > " + " </td>");
                        else if (tipe == "LH")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"LITTER\" > LITTER </option><option value =\"HOURS\"> HOURS </option></select > " + " </td>");
                        else if (tipe == "MMH")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"MAX\" > MAX </option><option value =\"MIN\"> MIN </option><option value =\"HIGH\"> HIGH </option></select > " + " </td>");
                        else if (tipe == "N123")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"1-2-3\" > 1-2-3 </option><option value =\"1-3-2\"> 1-3-2 </option><option value =\"2-1-3\"> 2-1-3 </option><option value=\"2-3-1\" > 2-3-1 </option><option value =\"3-1-2\"> 3-1-2 </option><option value =\"3-2-1\"> 3-2-1 </option></select > " + " </td>");
                        else if (tipe == "N2N")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"2-3-4\" > 2-3-4 </option><option value =\"NO\"> NO </option></select > " + " </td>");
                        else if (tipe == "N48")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"4\" > 4 </option><option value =\"8\"> 8 </option></select > " + " </td>");
                        else if (tipe == "OO")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"ON\" > ON </option><option value =\"OFF\"> OFF </option></select > " + " </td>");
                        else if (tipe == "OOS")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"ON\" > ON </option><option value =\"OFF\"> OFF </option><option value =\"STANDBY\"> STANDBY </option></select > " + " </td>");
                        else if (tipe == "PG")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"PLN\" > PLN </option><option value =\"GENSET\"> GENSET </option></select > " + " </td>");
                        else if (tipe == "SS")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"SS1\" > SS1 </option><option value =\"SS2\"> SS2 </option></select > " + " </td>");
                        else if (tipe == "UD")
                            htmlTable.Append("<td>" + $"<select class=\"form-control dropdown\" onchange=\"SetDropDownListColor(this)\" id=\"{idddl}\" name=\"idticket\"><option value=\"UP\" > UP </option><option value =\"DOWN\"> DOWN </option></select > " + " </td>");

                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + satuan + "</label>" + "</td>");
                        htmlTable.Append("</tr>");
                        value = Request.Form["idticket"];
                        //Response.Write(value);

                        looping[i] = IDdata;

                        //Response.Write( "bisa" + words[1]);
                        int j = i - 1;
                        //Response.Write(seats);
                        //looping = "('" + IDdata + "'," + seats + "),";
                        loop = IDdata + "," + loop;

                    }
                    if (value != null)
                    {
                        string[] lines = Regex.Split(value, ",");

                        foreach (string line in lines)
                        {
                            //Response.Write(line);
                            akhir[j] = "('" + tanggal + "','" + user + "','" + looping[j] +  "','" + DropDownList1.Text + "','" + line + "')";
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