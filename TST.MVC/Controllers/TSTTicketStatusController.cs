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
    public class TSTTicketStatusController : Controller
    {
        private TSTEntities db = new TSTEntities();

        // GET: TSTTicketStatus
        public ActionResult Index()
        {
            return View(db.TSTTicketStatuses.ToList());
        }

        // GET: TSTTicketStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicketStatus tSTTicketStatus = db.TSTTicketStatuses.Find(id);
            if (tSTTicketStatus == null)
            {
                return HttpNotFound();
            }
            return View(tSTTicketStatus);
        }

        // GET: TSTTicketStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TSTTicketStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketStatusID,TicketStatusName,TicketStatusDescription")] TSTTicketStatus tSTTicketStatus)
        {
            if (ModelState.IsValid)
            {
                db.TSTTicketStatuses.Add(tSTTicketStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tSTTicketStatus);
        }

        // GET: TSTTicketStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicketStatus tSTTicketStatus = db.TSTTicketStatuses.Find(id);
            if (tSTTicketStatus == null)
            {
                return HttpNotFound();
            }
            return View(tSTTicketStatus);
        }

        // POST: TSTTicketStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketStatusID,TicketStatusName,TicketStatusDescription")] TSTTicketStatus tSTTicketStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tSTTicketStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tSTTicketStatus);
        }

        // GET: TSTTicketStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicketStatus tSTTicketStatus = db.TSTTicketStatuses.Find(id);
            if (tSTTicketStatus == null)
            {
                return HttpNotFound();
            }
            return View(tSTTicketStatus);
        }

        // POST: TSTTicketStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TSTTicketStatus tSTTicketStatus = db.TSTTicketStatuses.Find(id);
            db.TSTTicketStatuses.Remove(tSTTicketStatus);
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
