using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KLTN_QuanLyLuuTruHoSo.Models;
namespace KLTN_QuanLyLuuTruHoSo.Controllers
{
    public class QLLT_TaiLieuController : Controller
    {
        //
        // GET: /QLLT_TaiLieu/
        QLLuuTruHoSoDataContext data = new QLLuuTruHoSoDataContext();
        public ActionResult Index()
        {
            List<TAILIEU> tl = data.TAILIEUs.ToList();
            return View(tl);
        }
        public ActionResult ChiTietTaiLieu(int id)
        {
            List<TAILIEU> dstl = data.TAILIEUs.Where(c => c.ID == id).ToList();
            return PartialView(dstl);
        }

    }
}
