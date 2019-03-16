using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Edu_Hutech.Models;

namespace Edu_Hutech.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        HutechEduDataContext db = new HutechEduDataContext();
        public ActionResult Dat_SV5T_Khoa()
        {
            if (Session["Khoa"] == null)
            {
                return Redirect("/UserLogin/UserLogin");
            }
            string makhoa = System.Web.HttpContext.Current.Session["Khoa"].ToString();
            int sl_tc = db.GetTaiLieu_SV5T_TheoCap("K").ToList().Count;
            var dsdk = db.Ds_Dat_SV5T_Khoa(makhoa, sl_tc).ToList();
            return View(dsdk);
        }
        public void ExportToExcel()
        {
            var ds_dk = new System.Data.DataTable("Danh Sách Đạt Sinh Viên 5 Tốt Cấp Khoa");
            ds_dk.Columns.Add("STT", typeof(int));
            ds_dk.Columns.Add("Họ", typeof(string));
            ds_dk.Columns.Add("Tên", typeof(string));
            ds_dk.Columns.Add("MSSV", typeof(string));
            ds_dk.Columns.Add("Lớp", typeof(string));
            ds_dk.Columns.Add("Năm Học", typeof(int));

            string makhoa = System.Web.HttpContext.Current.Session["Khoa"].ToString();
            int sl_tc = db.GetTaiLieu_SV5T_TheoCap("K").ToList().Count;
            var dsdk = db.Ds_Dat_SV5T_Khoa(makhoa, sl_tc).ToList();
            int i = 0;
            int row = 1;
            foreach( var item in dsdk)
            {
                ds_dk.Rows.Add(row,dsdk[i].Ho,dsdk[i].Ten,dsdk[i].MSSV,dsdk[i].MaLop,dsdk[i].NamHoc);                
                row++;
                i++;
            }
            var grid = new GridView();
            grid.DataSource = ds_dk;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DS_Dat_SV5T_CapKhoa.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.Write("<table><tr><td colspan='3'>Danh Sách Đạt SV5T Cấp Khoa</td></tr>");
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}