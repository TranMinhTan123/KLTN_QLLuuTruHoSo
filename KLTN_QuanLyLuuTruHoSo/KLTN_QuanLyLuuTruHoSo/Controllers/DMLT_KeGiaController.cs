using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KLTN_QuanLyLuuTruHoSo.Models;
namespace KLTN_QuanLyLuuTruHoSo.Controllers
{
    public class DMLT_KeGiaController : Controller
    {
        //
        // GET: /DMLT_KeGia/
        QLLuuTruHoSoDataContext data = new QLLuuTruHoSoDataContext();
        public ActionResult Index()
        {
            List<KE> k = data.KEs.ToList();
            return View(k);
        }
        public ActionResult ThemKe()
        {
            ViewBag.ID_TU = new SelectList(data.TUs.ToList(), "ID", "TENTU");
            return View();
        }
        [HttpPost]
        public ActionResult XuLyThemKe(KE ke, FormCollection col)
        {
            ke.NGAYTAO = DateTime.Now;
            ke.NGAYCAPNHAT = DateTime.Now;
            DateTime ngayketthuc = DateTime.Parse(col["dtNgayKT"]);
            List<HOP> h = data.HOPs.Where(c => c.ID_KE == ke.ID).ToList();
            int sohopht = h.Count();
            ke.NGAYBD = DateTime.Now;
            ke.NGAYKT = ngayketthuc;
            ke.SLHOP_HT = sohopht;
            data.KEs.InsertOnSubmit(ke);
            data.SubmitChanges();
            return RedirectToAction("Index", "DMLT_KeGia");
        }
        public ActionResult XoaKe(int id)
        {
            try
            {
                KE del = data.KEs.SingleOrDefault(t => t.ID == id);
                if (del != null)
                {
                    data.KEs.DeleteOnSubmit(del);
                    data.SubmitChanges();
                }
            }
            catch { }
            return RedirectToAction("Index", "DMLT_KeGia");
        }
        // Sửa Kệ
        public ActionResult SuaKe(int id)
        {
            ViewBag.ID_TU = new SelectList(data.TUs.ToList(), "ID", "TENTU");
            KE ke = data.KEs.SingleOrDefault(t => t.ID == id);
            return View(ke);
        }
        [HttpPost]
        public ActionResult XuLySuaKe(FormCollection col)
        {
            int id = int.Parse(col["ID"]);
            int id_tu = int.Parse(col["ID_TU"]);
            var tenke = col["TENKE"];
            int sohopdat = int.Parse(col["SLHOP_DAT"]);
            List<HOP> h = data.HOPs.Where(c => c.ID_KE == id).ToList();
            int sohopht = h.Count();
            DateTime ngaykt = DateTime.Parse(col["dtNgayKT"]);
            DateTime ngaycapnhat = DateTime.Now;
            bool trangthai = bool.Parse(col["TRANGTHAI"]);

            if (tenke != null)
            {
                KE k = data.KEs.SingleOrDefault(c => c.ID == id);
                k.ID_TU = id_tu;
                k.TENKE = tenke;
                k.SLHOP_DAT = sohopdat;
                k.SLHOP_HT = sohopht;
                k.NGAYKT = ngaykt;
                //không cập nhật ngày tạo
                k.NGAYCAPNHAT = ngaycapnhat;
                k.TRANGTHAI = trangthai;

                data.SubmitChanges();
            }
            List<KE> sk = data.KEs.Where(t => t.ID != null).ToList();
            return RedirectToAction("Index", "DMLT_KeGia", sk);
        }
        public ActionResult TimKiemKe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XuLyTimKiemKe(FormCollection col)
        {
            string tk = col["txtTuKhoa"].ToString();
            List<KE> dsKe = data.KEs.Where(t => t.TENKE.Contains(tk) == true).ToList();

            return View("Index", dsKe);
        }
    }
}
