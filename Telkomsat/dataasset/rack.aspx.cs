﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Configuration;

namespace Telkomsat.dataasset
{
    public partial class rack : System.Web.UI.Page
    {
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        StringBuilder htmlTable = new StringBuilder();
        string IDdata = "kitaa", bangunan = "st", style1 = "a", query, divisi = "", style2 = "a", style3, prioritas = "a", statusticket = "a", tanggal, queydel, jenisview = "";
        string ruangan1, ruangan, rak1;
        string qr = "a", divisireply = "A";
        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            tableticket();
        }

        public class inisial
        {
            public string idruangan { get; set; }
            public string ruangan1 { get; set; }
        }

        [WebMethod]
        public static List<inisial> GetID()
        {
            string constr = ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * from as_ruangan"))
                {
                    cmd.Connection = con;
                    List<inisial> mydata = new List<inisial>();
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            mydata.Add(new inisial
                            {
                                idruangan = sdr["id_ruangan"].ToString(),
                                ruangan1 = sdr["nama_ruangan"].ToString(),
                            });
                        }
                    }
                    con.Close();
                    return mydata;
                }
            }
        }

        protected void Edit_ServerClick(object sender, EventArgs e)
        {
            var datetime1 = DateTime.Now.ToString("yyyy/MM/dd h:m:s");
            sqlCon.Open();
            string query = $@"UPDATE as_ruangan SET id_bangunan = '{TextBox2.Text}', nama_ruangan = '{txtwiayah.Text}', qr_ruangan ='{txtqr.Text}', tanggal = '{datetime1}' where id_ruangan = '{txtid.Text}'";
            SqlCommand sqlcmd = new SqlCommand(query, sqlCon);
            sqlcmd.ExecuteNonQuery();
            sqlCon.Close();
            Response.Redirect("../dataasset/ruangan.aspx");
        }

        public class databangunan
        {
            public string idrak { get; set; }
            public string rak { get; set; }
            public string qr { get; set; }
        }

        [WebMethod]
        public static List<databangunan> Getbangunan(string videoid)
        {
            string constr = ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM as_rak where id_rak = '{videoid}'"))
                {
                    cmd.Connection = con;
                    List<databangunan> dabangunan = new List<databangunan>();
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            dabangunan.Add(new databangunan
                            {
                                idrak = sdr["id_rak"].ToString(),
                                rak = sdr["nama_rak"].ToString(),
                                qr = sdr["qr_rak"].ToString(),
                            });
                        }
                    }
                    con.Close();
                    return dabangunan;
                }
            }
        }

        protected void disp1_ServerClick(object sender, EventArgs e)
        {
            tableticket();
        }

        void tableticket()
        {
            query = "SELECT b.tanggal, w.id_ruangan, b.id_rak, w.nama_ruangan, b.nama_rak, b.qr_rak FROM as_rak b join as_ruangan w on b.id_ruangan=w.id_ruangan ";

            SqlCommand cmd = new SqlCommand(query, sqlCon);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();
            htmlTable.Append("<table id=\"example2\" width=\"100%\" class=\"table table-bordered table-hover table-striped\">");
            htmlTable.Append("<thead>");
            htmlTable.Append("<tr><th>Tanggal</th><th>Nama Ruangan</th><th>Nama Rack</th><th>QR Rack</th><th>Action</th></tr>");
            htmlTable.Append("</thead>");

            htmlTable.Append("<tbody>");
            if (!object.Equals(ds.Tables[0], null))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        IDdata = ds.Tables[0].Rows[i]["id_rak"].ToString();
                        ruangan = ds.Tables[0].Rows[i]["nama_ruangan"].ToString();
                        tanggal = ds.Tables[0].Rows[i]["tanggal"].ToString();
                        qr = ds.Tables[0].Rows[i]["qr_rak"].ToString();
                        rak1 = ds.Tables[0].Rows[i]["nama_rak"].ToString();
                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + "<label style=\"font-size:10px; color:#a9a9a9; font-color width:70px;\">" + ds.Tables[0].Rows[i]["tanggal"] + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + ruangan + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + rak1 + "</label>" + "</td>");
                        htmlTable.Append("<td>" + $"<label style=\"{style3}\">" + qr + "</label>" + "</td>");
                        htmlTable.Append($"<td style=\"visibility: {jenisview};\"> " + $"<a onclick=\"confirmdelete('../dataasset/hapus.aspx?idrak={IDdata}')\" class=\"btn btn-sm btn-danger\" style=\"margin-right:10px\">" + "Delete" + "</a>");
                        htmlTable.Append($"<button type=\"button\"  value=\"{IDdata}\" class=\"btn btn-sm btn-warning datawil\" data-toggle=\"modal\" data-target=\"#modalupdate\" id=\"edit\">" + "Edit" + "</button></td>");
                        htmlTable.Append("</tr>");
                    }
                    htmlTable.Append("</tbody>");
                    htmlTable.Append("</table>");
                    DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
                }
            }
        }
    }
}