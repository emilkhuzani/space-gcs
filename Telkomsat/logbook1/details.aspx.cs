﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Telkomsat.logbook1
{
    public partial class details : System.Web.UI.Page
    {
        SqlConnection sqlCon2 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GCSConnectionString"].ConnectionString);

        //SqlConnection sqlCon2 = new SqlConnection(@"Data Source=DESKTOP-K0GET7F\SQLEXPRESS; Initial Catalog=GCS; Integrated Security = true;");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("~/error.aspx");
            }

            string formattanggal = Session["tanggal"].ToString();
            //DateTime newFormat = DateTime.ParseExact("09/12/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
            string tanggalnew = Convert.ToDateTime(formattanggal).ToString("dd/MM/yyyy");
            hfContactID.Value = Session["hf"].ToString();
            txtTanggal.Text = tanggalnew;
            txtEvent.Text = Session["event"].ToString();
            ddlUnit.Text = Session["unit"].ToString();
            ddlStatus.Text = Session["status"].ToString();
            txtOG.Text = Session["OG"].ToString();
            txtOS.Text = Session["OS"].ToString();
            txtInfo.Text = Session["info"].ToString();
            lblKategori.Text = Session["kategori"].ToString();
            lblSN.Text = Session["SN"].ToString();
            lblSN1.Text = Session["SN1"].ToString();
            lblEstimasi.Text = Session["estimasi"].ToString();

            if (lblSN.Text == "")
            {
                tdlabel.Visible = false;
                tdlabel1.Visible = false;
                tdlabel2.Visible = false;
            }

            if (lblSN1.Text == "")
            {
                td1.Visible = false;
                td2.Visible = false;
                td3.Visible = false;
            }

            if (lblEstimasi.Text == "")
            {
                td4.Visible = false;
                td5.Visible = false;
                td6.Visible = false;
            }
            //if (lblEstimasi.Text == "")
            //{
            //}
            if (txtOS.Text != Session["username"].ToString())
            {
                Button1.Visible = false;
                if (Session["jenis1"].ToString() == "og")
                    Button1.Visible = true;
            }


            int ID = Convert.ToInt32(hfContactID.Value);
            if (sqlCon2.State == ConnectionState.Closed)
                sqlCon2.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("LoViewByIDSorting", sqlCon2);
            sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlda.SelectCommand.Parameters.AddWithValue("@ID", ID);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);
            sqlCon2.Close();
            hfContactID.Value = ID.ToString();
            dtContact.DataSource = dtbl;
            dtContact.DataBind();

            sqlCon2.Open();
            SqlDataAdapter da = new SqlDataAdapter("LoViewFile", sqlCon2);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@ID2", ID);
            DataTable dtbl2 = new DataTable();
            da.Fill(dtbl2);
            sqlCon2.Close();
            hfContactID.Value = ID.ToString();
            if (da != null)
            {
                gvFile.Visible = true;
                gvFile.DataSource = dtbl2;
                gvFile.DataBind();
            }
            

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/logbook1/editlogbook.aspx");
        }

        protected void linkDownloadFile_Click(object sender, EventArgs e)
        {
            int ImageId = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName;
            sqlCon2.Open();
            SqlCommand cmd = new SqlCommand("fileDownload", sqlCon2);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID2", ImageId);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            fileName = sdr["NameFile"].ToString();
            bytes = (byte[])sdr["FileA"];
            sqlCon2.Close();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
    }
}