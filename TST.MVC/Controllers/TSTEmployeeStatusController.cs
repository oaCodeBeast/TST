using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TST.DAL;

namespace TST.MVC.Controllers
{
    public class TSTEmployeeStatusController : Controller
    {
        private TSTEntities db = new TSTEntities();

        // GET: TSTEmployeeStatus
        public ActionResult Index()
        {
            return View(db.TSTEmployeeStatuses.ToList());
        }

        // GET: TSTEmployeeStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployeeStatus tSTEmployeeStatus = db.TSTEmployeeStatuses.Find(id);
            if (tSTEmployeeStatus == null)
            {
                return HttpNotFound();
            }
            return View(tSTEmployeeStatus);
        }

        // GET: TSTEmployeeStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TSTEmployeeStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpStatusID,EmpStatusName,EmpStatusDescription")] TSTEmployeeStatus tSTEmployeeStatus)
        {
            if (ModelState.IsValid)
            {
                db.TSTEmployeeStatuses.Add(tSTEmployeeStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tSTEmployeeStatus);
        }

        // GET: TSTEmployeeStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployeeStatus tSTEmployeeStatus = db.TSTEmployeeStatuses.Find(id);
            if (tSTEmployeeStatus == null)
            {
                return HttpNotFound();
            }
            return View(tSTEmployeeStatus);
        }

        // POST: TSTEmployeeStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpStatusID,EmpStatusName,EmpStatusDescription")] TSTEmployeeStatus tSTEmployeeStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tSTEmployeeStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tSTEmployeeStatus);
        }

        // GET: TSTEmployeeStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployeeStatus tSTEmployeeStatus = db.TSTEmployeeStatuses.Find(id);
            if (tSTEmployeeStatus == null)
            {
                return HttpNotFound();
            }
            return View(tSTEmployeeStatus);
        }

        // POST: TSTEmployeeStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TSTEmployeeStatus tSTEmployeeStatus = db.TSTEmployeeStatuses.Find(id);
            db.TSTEmployeeStatuses.Remove(tSTEmployeeStatus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
