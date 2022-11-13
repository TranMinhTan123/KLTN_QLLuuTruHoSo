using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KLTN_QuanLyLuuTruHoSo.Models;
namespace KLTN_QuanLyLuuTruHoSo.Controllers
{
    public class DMLT_TuLuuTruController : Controller
    {
        //
        // GET: /DMLT_TuLuuTru/
        QLLuuTruHoSoDataContext data = new QLLuuTruHoSoDataContext();
        public ActionResult Index()
        {
            List<TU> t = data.TUs.ToList();
            return View(t);
        }
        public ActionResult ThemTu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XuLyThemTu(TU tu, FormCollection col)
        {
            tu.NGAYTAO = DateTime.Now;
            tu.NGAYCAPNHAT = DateTime.Now;
            List<KE> k = data.KEs.Where(c => c.ID_TU == tu.ID).ToList();
            int sokeht = k.Count();
            tu.SLKE_HT = sokeht;
            data.TUs.InsertOnSubmit(tu);
            data.SubmitChanges();
            return RedirectToAction("Index", "DMLT_TuLuuTru");
        }
        public ActionResult XoaTu(int id)
        {
            try
            {
                TU del = data.TUs.SingleOrDefault(t => t.ID == id);
                if (del != null)
                {
                    data.TUs.DeleteOnSubmit(del);
                    data.SubmitChanges();
                }
            }
            catch { }
            return RedirectToAction("Index", "DMLT_TuLuuTru");
        }
        // sửa Tủ
        public ActionResult SuaTu(int id)
        {
            TU tu = data.TUs.SingleOrDefault(t => t.ID == id);
            return View(tu);
        }
        [HttpPost]
        public ActionResult XuLySuaTu(FormCollection col)
        {
            int id = int.Parse(col["ID"]);
            var tentu = col["TENTU"];
            int songan = int.Parse(col["SONGAN"]);
            int sokedat = int.Parse(col["SLKE_DAT"]);
            DateTime ngaycapnhat = DateTime.Now;
            bool trangthai = bool.Parse(col["TRANGTHAI"]);

            if (tentu != null)
            {
                TU t = data.TUs.SingleOrDefault(c => c.ID == id);
                List<KE> k = data.KEs.Where(c => c.ID_TU == id).ToList();
                int sokeht = k.Count();
                t.TENTU = tentu;
                t.SONGAN = songan;
                t.SLKE_DAT = sokedat;
                t.SLKE_HT = sokeht;
                //không cập nhật ngày tạo
                t.NGAYCAPNHAT = ngaycapnhat;
                t.TRANGTHAI = trangthai;

                data.SubmitChanges();
            }
            List<TU> st = data.TUs.Where(t => t.ID != null).ToList();
            return RedirectToAction("Index", "DMLT_TuLuuTru", st);
        }
        public ActionResult TimKiemTu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XuLyTimKiemTu(FormCollection col)
        {
            string tk = col["txtTuKhoa"].ToString();
            List<TU> dsTu = data.TUs.Where(t => t.TENTU.Contains(tk) == true).ToList();

            return View("Index", dsTu);
        }
    }
}
