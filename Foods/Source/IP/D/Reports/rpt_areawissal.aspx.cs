﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;

using NHibernate;
using NHibernate.Criterion;
using Foods;
using DataAccess;

namespace Foods
{
    public partial class rpt_areawissal : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db = new DBConnection();
        string usrid, fdat, ldat, monid, yrid, areaid, proid;

        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            if (check != null)
            {
                usrid = Request.QueryString["USRID"];
                fdat = Request.QueryString["FDAT"];
                ldat = Request.QueryString["LDAT"];
                monid = Request.QueryString["MONID"];
                yrid = Request.QueryString["YRID"];
                usrid = Request.QueryString["USRID"];
                areaid = Request.QueryString["AREAID"];
                proid = Request.QueryString["PROID"];

                if (areaid != null && fdat != null && ldat != null)
                {
                    get_areasal(areaid, fdat, ldat);
                }
                else
                {
                    get_areasal(areaid);
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void LinkBtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                string FileName = "UserList.xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                
                GV_UsrWis.GridLines = GridLines.Both;
                GV_UsrWis.HeaderStyle.Font.Bold = true;

                GV_UsrWis.RenderControl(htmltextwrtter);

                Response.Write(strwritter.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                throw;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //lblalert.Text = ex.Message;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        private void get_areasal(string Areaid)
        {

            try
            {
                dt_ = new DataTable();
                //string str = "SELECT * FROM v_incomexp where accno ='" + acc + "' and expensesdat between '" + fdat + "' and '" + tdat + "'  order by expenceid asc";


                ////dt_ = DBConnection.GetQueryData(" SELECT * FROM v_profloss where accno = " + acc + " and Date between '" + fdat + "' and '" + tdat + "'");
                //dt_ = DBConnection.GetQueryData(str);


                using (var cmd = new SqlCommand("sp_rptareawissale", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@area ", Areaid.Trim());
                    da.Fill(dt_);
                }

                if (dt_.Rows.Count > 0)
                {
                    lbl_area.Text = dt_.Rows[0]["area_"].ToString();

                    GV_UsrWis.DataSource = dt_;
                    GV_UsrWis.DataBind();

                    //Get Total

                    decimal GTotal = 0;
                    decimal QGTotal = 0;
                    // Total
                    for (int j = 0; j < GV_UsrWis.Rows.Count; j++)
                    {
                        Label total = (Label)GV_UsrWis.Rows[j].FindControl("lbl_Amt");
                        GTotal += Convert.ToDecimal(total.Text);
                    }

                    //Quantity
                    for (int j = 0; j < GV_UsrWis.Rows.Count; j++)
                    {
                        Label totalqty = (Label)GV_UsrWis.Rows[j].FindControl("lbl_Qty");
                        QGTotal += Convert.ToDecimal(totalqty.Text);
                    }
                    ttl_qty.Text = QGTotal.ToString();
                    lbl_ttl.Text = GTotal.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void get_areasal(string Areaid, string FDAT, string LDAT)
        {
            try
            {
                dt_ = new DataTable();
                //string str = "SELECT * FROM v_incomexp where accno ='" + acc + "' and expensesdat between '" + fdat + "' and '" + tdat + "'  order by expenceid asc";


                ////dt_ = DBConnection.GetQueryData(" SELECT * FROM v_profloss where accno = " + acc + " and Date between '" + fdat + "' and '" + tdat + "'");
                //dt_ = DBConnection.GetQueryData(str);


                using (var cmd = new SqlCommand("sp_rptareasale", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@area ", Areaid.Trim());
                    cmd.Parameters.AddWithValue("@fdat ", FDAT.Trim());
                    cmd.Parameters.AddWithValue("@ldat ", LDAT.Trim());
                    da.Fill(dt_);
                }

                if (dt_.Rows.Count > 0)
                {
                    lbl_area.Text = dt_.Rows[0]["area_"].ToString();

                    GV_UsrWis.DataSource = dt_;
                    GV_UsrWis.DataBind();

                    //Get Total

                    decimal GTotal = 0;
                    decimal QGTotal = 0;
                    // Total
                    for (int j = 0; j < GV_UsrWis.Rows.Count; j++)
                    {
                        Label total = (Label)GV_UsrWis.Rows[j].FindControl("lbl_Amt");
                        GTotal += Convert.ToDecimal(total.Text);
                    }

                    //Quantity
                    for (int j = 0; j < GV_UsrWis.Rows.Count; j++)
                    {
                        Label totalqty = (Label)GV_UsrWis.Rows[j].FindControl("lbl_Qty");
                        QGTotal += Convert.ToDecimal(totalqty.Text);
                    }
                    ttl_qty.Text = QGTotal.ToString();
                    lbl_ttl.Text = GTotal.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}