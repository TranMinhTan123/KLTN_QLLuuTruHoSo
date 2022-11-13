using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KLTN_QuanLyLuuTruHoSo.Models;
namespace KLTN_QuanLyLuuTruHoSo.Controllers
{
    public class DMLT_HopCapController : Controller
    {
        //
        // GET: /DMLT_HopCap/
        QLLuuTruHoSoDataContext data = new QLLuuTruHoSoDataContext();
        public ActionResult Index()
        {
            List<HOP> h = data.HOPs.ToList();
            return View(h);
        }
        public ActionResult ThemHop()
        {
            ViewBag.ID_KE = new SelectList(data.KEs.ToList(), "ID", "TENKE");
            return View();
        }
        [HttpPost]
        public ActionResult XuLyThemHop(HOP h, FormCollection col)
        {
            h.NGAYTAO = DateTime.Now;
            h.NGAYCAPNHAT = DateTime.Now;
            DateTime ngayketthuc = DateTime.Parse(col["dtNgayKT"]);
            List<HOSO> hs = data.HOSOs.Where(c => c.ID_HOP == h.ID).ToList();
            int sohopht = hs.Count();
            h.NGAYBD = DateTime.Now;
            h.NGAYKT = ngayketthuc;
            h.SLHOSO_HT = sohopht;
            data.HOPs.InsertOnSubmit(h);
            data.SubmitChanges();
            return RedirectToAction("Index", "DMLT_HopCap");
        }
        //XÓA HỌP
        public ActionResult XoaHop(int id)
        {
            try
            {
                HOP del = data.HOPs.SingleOrDefault(t => t.ID == id);
                if (del != null)
                {
                    data.HOPs.DeleteOnSubmit(del);
                    data.SubmitChanges();
                }
            }
            catch { }
            return RedirectToAction("Index", "DMLT_HopCap");
        }
        // Sửa HỌP
        public ActionResult SuaHop(int id)
        {
            ViewBag.ID_KE = new SelectList(data.KEs.ToList(), "ID", "TENKE");
            HOP h = data.HOPs.SingleOrDefault(t => t.ID == id);
            return View(h);
        }
        [HttpPost]
        public ActionResult XuLySuaHop(FormCollection col)
        {
            int id = int.Parse(col["ID"]);
            int id_ke = int.Parse(col["ID_KE"]);
            var tenhop = col["TENHOP"];
            int sohosodat = int.Parse(col["SLHOSO_DAT"]);
            List<HOSO> hs = data.HOSOs.Where(c => c.ID_HOP == id).ToList();
            int sohosoht = hs.Count();
            DateTime ngaykt = DateTime.Parse(col["dtNgayKT"]);
            DateTime ngaycapnhat = DateTime.Now;
            bool trangthai = bool.Parse(col["TRANGTHAI"]);

            if (tenhop != null)
            {
                HOP h = data.HOPs.SingleOrDefault(c => c.ID == id);
                h.ID_KE = id_ke;
                h.TENHOP = tenhop;
                h.SLHOSO_DAT = sohosodat;
                h.SLHOSO_HT = sohosoht;
                h.NGAYKT = ngaykt;
                //không cập nhật ngày tạo
                h.NGAYCAPNHAT = ngaycapnhat;
                h.TRANGTHAI = trangthai;

                data.SubmitChanges();
            }
            List<HOP> sh = data.HOPs.Where(t => t.ID != null).ToList();
            return RedirectToAction("Index", "DMLT_HopCap", sh);
        }
        public ActionResult TimKiemHop()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XuLyTimKiemHop(FormCollection col)
        {
            string tk = col["txtTuKhoa"].ToString();
            List<HOP> dsHop = data.HOPs.Where(t => t.TENHOP.Contains(tk) == true).ToList();

            return View("Index", dsHop);
        }
    }
}
