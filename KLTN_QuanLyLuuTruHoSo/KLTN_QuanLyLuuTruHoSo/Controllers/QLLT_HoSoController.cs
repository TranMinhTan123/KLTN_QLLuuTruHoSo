using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KLTN_QuanLyLuuTruHoSo.Models;
namespace KLTN_QuanLyLuuTruHoSo.Controllers
{
    public class QLLT_HoSoController : Controller
    {
        //
        // GET: /QLLT_HoSo/
        QLLuuTruHoSoDataContext data = new QLLuuTruHoSoDataContext();
        public ActionResult Index()
        {
            List<HOSO> hs = data.HOSOs.ToList();
            return View(hs);
        }
        public ActionResult ThemHoSo()
        {
            ViewBag.ID_HOP = new SelectList(data.HOPs.ToList(), "ID", "TENHOP");
            ViewBag.ID_LOAIHS = new SelectList(data.LOAIHOSOs.ToList(), "ID", "TENLOAI");
            ViewBag.ID_PHONGBAN = new SelectList(data.PHONGBANs.ToList(), "ID", "TENPHONG");
            return View();
        }
        [HttpPost]
        public ActionResult XuLyThemHoSo(HOSO hs, FormCollection col)
        {
            hs.NGAYTAO = DateTime.Now;
            hs.NGAYCAPNHAT = DateTime.Now;
            DateTime ngayketthuc = DateTime.Parse(col["dtNgayKT"]);
            List<TAILIEU> tl = data.TAILIEUs.Where(c => c.ID_HOSO == hs.ID).ToList();
            int sotl = tl.Count();
            hs.SLTAILIEU = sotl;
            hs.NGAYBD = DateTime.Now;
            hs.NGAYKT = ngayketthuc;

            TimeSpan tgbq = ngayketthuc -  DateTime.Now;
            hs.THOIGIANBAOQUAN = tgbq.Days + " Ngày";

            data.HOSOs.InsertOnSubmit(hs);
            data.SubmitChanges();
            return RedirectToAction("Index", "QLLT_HoSo");
        }
        public ActionResult XoaHoSo(int id)
        {
            try
            {
                HOSO del = data.HOSOs.SingleOrDefault(t => t.ID == id);
                if (del != null)
                {
                    data.HOSOs.DeleteOnSubmit(del);
                    data.SubmitChanges();
                }
            }
            catch { }
            return RedirectToAction("Index", "QLLT_HoSo");
        }
        public ActionResult SuaHoSo(int id)
        {
            ViewBag.ID_HOP = new SelectList(data.HOPs.ToList(), "ID", "TENHOP");
            ViewBag.ID_LOAIHS = new SelectList(data.LOAIHOSOs.ToList(), "ID", "TENLOAI");
            ViewBag.ID_PHONGBAN = new SelectList(data.PHONGBANs.ToList(), "ID", "TENPHONG");
            HOSO hs = data.HOSOs.SingleOrDefault(t => t.ID == id);
            return View(hs);
        }
        [HttpPost]
        public ActionResult XuLySuaHoSo(FormCollection col)
        {
            int id = int.Parse(col["ID"]);
            int id_hop = int.Parse(col["ID_HOP"]);
            int id_loaihs = int.Parse(col["ID_LOAIHS"]);
            int id_phongban = int.Parse(col["ID_PHONGBAN"]);

            var tenhs = col["TENHOSO"];
            List<TAILIEU> tl = data.TAILIEUs.Where(c => c.ID_HOSO == id).ToList();
            int sotl = tl.Count();
            var thoigianbaoquan = col["THOIGIANBAOQUAN"];
            DateTime ngaykt = DateTime.Parse(col["dtNgayKT"]);
            DateTime ngaycapnhat = DateTime.Now;
            bool trangthai = bool.Parse(col["TRANGTHAI"]);

            if (tenhs != null)
            {
                HOSO hs = data.HOSOs.SingleOrDefault(c => c.ID == id);
                hs.ID_HOP = id_hop;
                hs.ID_LOAIHS = id_loaihs;
                hs.ID_PHONGBAN = id_phongban;
                hs.TENHOSO = tenhs;
                hs.SLTAILIEU = sotl;
                hs.NGAYKT = ngaykt;

                TimeSpan tgbq = ngaykt - DateTime.Now;
                hs.THOIGIANBAOQUAN = tgbq.Days + " Ngày";
 
                //không cập nhật ngày tạo
                hs.NGAYCAPNHAT = ngaycapnhat;
                hs.TRANGTHAI = trangthai;

                data.SubmitChanges();
            }
            List<HOSO> shs = data.HOSOs.Where(t => t.ID != null).ToList();
            return RedirectToAction("Index", "QLLT_HoSo", shs);
        }
        public ActionResult TimKiemHoSo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XuLyTimKiemHoSo(FormCollection col)
        {
            string tk = col["txtTuKhoa"].ToString();
            List<HOSO> dsHoSo = data.HOSOs.Where(t => t.TENHOSO.Contains(tk) == true).ToList();

            return View("Index", dsHoSo);
        }
    }
}
