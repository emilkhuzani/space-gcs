﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using System.Threading;

namespace Telkomsat.logbook1
{
    public partial class tambah : System.Web.UI.Page
    {
        Nullable<int> i = null;
        Nullable<int> j = null;
        string tanggal;
        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString);
        //SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-K0GET7F\SQLEXPRESS; Initial Catalog=GCS; Integrated Security = true;");
        protected void Page_Load(object sender, EventArgs e)
        {
            

            txtTanggal.Enabled = false;
            txtTanggal.Text = DateTime.Now.ToString("dd/MM/yyyy");
            fillgridview();
            if(Session["jenis1"].ToString() == "os")
            {
                txtOS.Text = Session["username"].ToString();
            }
            else if (Session["jenis1"].ToString() == "og")
            {
                txtOG.Text = Session["username"].ToString();
            }

            if(!IsPostBack)
                fillgridview();

            //txtOS.Text = Session["user"].ToString();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            tanggal = txtTanggal.Text;
            Byte[] File1, File2, image1, image2, image3, image4;
            Stream s1 = FileUpload1.PostedFile.InputStream;
            Stream s2 = FileUpload2.PostedFile.InputStream;
            Stream i1 = ImgUpload1.PostedFile.InputStream;
            Stream i2 = ImgUpload2.PostedFile.InputStream;
            Stream i3 = ImgUpload3.PostedFile.InputStream;
            Stream i4 = ImgUpload4.PostedFile.InputStream;
            BinaryReader br1 = new BinaryReader(s1);
            BinaryReader br2 = new BinaryReader(s2);
            BinaryReader bri1 = new BinaryReader(i1);
            BinaryReader bri2 = new BinaryReader(i2);
            BinaryReader bri3 = new BinaryReader(i1);
            BinaryReader bri4 = new BinaryReader(i2);
            File1 = br1.ReadBytes((Int32)s1.Length);
            File2 = br2.ReadBytes((Int32)s2.Length);
            image1 = bri1.ReadBytes((Int32)i1.Length);
            image2 = bri2.ReadBytes((Int32)i2.Length);
            image3 = bri3.ReadBytes((Int32)i3.Length);
            image4 = bri4.ReadBytes((Int32)i4.Length);
            if (ImgUpload1.HasFile == true || ImgUpload2.HasFile == true ||
                ImgUpload1.HasFile == true || ImgUpload2.HasFile == true)
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("LoAddGambar", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@File1", File1);
                sqlCmd.Parameters.AddWithValue("@File2", File2);
                sqlCmd.Parameters.AddWithValue("@Gambar1", image1);
                sqlCmd.Parameters.AddWithValue("@Gambar2", image2);
                sqlCmd.Parameters.AddWithValue("@Gambar3", image3);
                sqlCmd.Parameters.AddWithValue("@Gambar4", image4);

                i = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }

            if (FileUpload1.HasFile == true || FileUpload2.HasFile == true)
            {
                string FN = "";
                FN = Path.GetFileName(FileUpload1.PostedFile.FileName);
                sqlCon.Close();
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("LoAddFile", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@Nama", FN);
                sqlCmd.Parameters.AddWithValue("@File1", File1);
                sqlCmd.Parameters.AddWithValue("@File2", File2);

                j = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }

            sqlCon.Close();
            sqlCon.Open();
            SqlCommand cmdLog = new SqlCommand("LoAddLogbook", sqlCon);
            cmdLog.CommandType = CommandType.StoredProcedure;
            if (ImgUpload1.HasFile == false && ImgUpload2.HasFile == false &&
                ImgUpload1.HasFile == false && ImgUpload2.HasFile == false)
            {
                cmdLog.Parameters.AddWithValue("@ID_gb", DBNull.Value);
            }
            else if (ImgUpload1.HasFile == true || ImgUpload2.HasFile == true ||
                ImgUpload1.HasFile == true || ImgUpload2.HasFile == true)
            {
                cmdLog.Parameters.AddWithValue("@ID_gb", i);
            }

            if (FileUpload1.HasFile == false && FileUpload2.HasFile == false)
            {
                cmdLog.Parameters.AddWithValue("@ID_file", DBNull.Value);
            }
            else if (FileUpload1.HasFile == true || FileUpload2.HasFile == true)
            {
                cmdLog.Parameters.AddWithValue("@ID_file", j);
            }

            cmdLog.Parameters.AddWithValue("@tanggal", tanggal);
            cmdLog.Parameters.AddWithValue("@event", txtEvent.Text.Trim());
            cmdLog.Parameters.AddWithValue("@Status", ddlStatus.Text.Trim());
            cmdLog.Parameters.AddWithValue("@Unit", ddlUnit.Text.Trim());
            cmdLog.Parameters.AddWithValue("@Kategori", ddlKategori.Text.Trim());
            cmdLog.Parameters.AddWithValue("@info", txtInfo.Text.Trim());
            cmdLog.Parameters.AddWithValue("@PIC_OS", txtOS.Text.Trim());
            cmdLog.Parameters.AddWithValue("@PIC_OG", txtOG.Text.Trim());
            //cmdLog.Parameters.AddWithValue("@Estimasi", txtHarga.Text.Trim());
            //if(txtSN1.Text != "")
            
            if(ddlKategori.Text == "Penggantian")
                cmdLog.Parameters.AddWithValue("@SN", txtSN2.Text.Trim());
            else
                cmdLog.Parameters.AddWithValue("@SN", txtSN1.Text.Trim());

            cmdLog.Parameters.AddWithValue("@SN1", txtSN3.Text.Trim());

            if (checkbox1.Checked)
                cmdLog.Parameters.AddWithValue("@estimasi", txtHarga.Text.Trim());
            else
                cmdLog.Parameters.AddWithValue("@estimasi", "");
            //else
            //cmdLog.Parameters.AddWithValue("@SN", "");
            /*if(txtSN2.Text != "")
            {
                cmdLog.Parameters.AddWithValue("@SN", txtSN2.Text.Trim());
                cmdLog.Parameters.AddWithValue("@SNB", txtSN3.Text.Trim());
            }*/
            //else
            //cmdLog.Parameters.AddWithValue("@SNB", "");
            cmdLog.ExecuteNonQuery();
            sqlCon.Close();
            
            lblUpdate.Text = "Berhasil Menyimpan";
            lblUpdate.ForeColor = System.Drawing.Color.Green;
            clear();
            fillgridview();
        }

        void fillgridview()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            tanggal = txtTanggal.Text;
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("LoViewIDTerakhir", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@waktu", tanggal);
            SqlDataAdapter sqlda = new SqlDataAdapter(cmd);
            sqlda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            gvLog.DataSource = dt;
            gvLog.DataBind();
            sqlCon.Close();
        }

        protected void btnGanti_Click(object sender, EventArgs e)
        {
            txtTanggal.Enabled = true;
        }

        void clear()
        {
            txtEvent.Text = "";
            txtInfo.Text = "";
            ddlStatus.Text = "";
            ddlKategori.Text = "";
        }

        protected void gvLog_RowData(object sender, GridViewRowEventArgs e)
        {
            for (int rowindex = gvLog.Rows.Count - 2; rowindex >= 0; rowindex--)
            {
                GridViewRow rows = gvLog.Rows[rowindex];
                GridViewRow previousrows = gvLog.Rows[rowindex + 1];

                if (rows.Cells[1].Text == previousrows.Cells[1].Text)
                {
                    rows.Cells[1].RowSpan = previousrows.Cells[1].RowSpan < 2 ? 2 :
                        previousrows.Cells[1].RowSpan + 1;
                    previousrows.Cells[1].Visible = false;
                }
            }

            for (int rowindex = gvLog.Rows.Count - 2; rowindex >= 0; rowindex--)
            {
                GridViewRow rows = gvLog.Rows[rowindex];
                GridViewRow previousrows = gvLog.Rows[rowindex + 1];

                if (rows.Cells[0].Text == previousrows.Cells[0].Text)
                {
                    rows.Cells[0].RowSpan = previousrows.Cells[0].RowSpan < 2 ? 2 :
                        previousrows.Cells[0].RowSpan + 1;
                    previousrows.Cells[0].Visible = false;
                }
            }
        }

        protected void Ink_OnClick1(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("LoViewIDLogbook", sqlCon);
            sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlda.SelectCommand.Parameters.AddWithValue("@ID", ID);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);
            sqlCon.Close();
            hfContactID.Value = ID.ToString();
            Session["hf"] = hfContactID.Value;
            //Response.Redirect("~/details.aspx?equ=" + dtbl.Rows[0]["Equipment"].ToString() + "merk=" + dtbl.Rows[0]["Merk"].ToString());
            Session["tanggal"] = dtbl.Rows[0]["tanggal"].ToString();
            Session["event"] = dtbl.Rows[0]["event"].ToString();
            Session["unit"] = dtbl.Rows[0]["unit"].ToString();
            Session["kategori"] = dtbl.Rows[0]["kategori"].ToString();
            Session["status"] = dtbl.Rows[0]["status"].ToString();
            Session["OS"] = dtbl.Rows[0]["PIC_OS"].ToString();
            Session["OG"] = dtbl.Rows[0]["PIC_OG"].ToString();
            Session["info"] = dtbl.Rows[0]["info"].ToString();
            Session["SN"] = dtbl.Rows[0]["S/N"].ToString();
            Session["SN1"] = dtbl.Rows[0]["SN"].ToString();
            Session["estimasi"] = dtbl.Rows[0]["estimasi"].ToString();

            //Response.Redirect("~/details.aspx?" + dtbl.Rows[0]["Merk"].ToString());
            Response.Redirect("~/logbook1/details.aspx");

        }

        protected void btnID_Click(object sender, EventArgs e)
        {
            Session["SN"] = txtSN.Text.Trim();
            Response.Redirect("~/logbook1/idasset.aspx");

        }
    }
}