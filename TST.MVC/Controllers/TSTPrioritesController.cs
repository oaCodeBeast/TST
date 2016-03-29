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
    [Authorize(Roles = "Admin, Tech")]
    public class TSTPrioritesController : Controller
    {
        private TSTEntities db = new TSTEntities();

        // GET: TSTPriorites
        public ActionResult Index()
        {
            return View(db.TSTPriorites.ToList());
        }

 

        // GET: TSTPriorites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TSTPriorites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PriorityID,PriorityName,PriorityDescription")] TSTPriorite tSTPriorite)
        {
            if (ModelState.IsValid)
            {
                db.TSTPriorites.Add(tSTPriorite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tSTPriorite);
        }

        // GET: TSTPriorites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTPriorite tSTPriorite = db.TSTPriorites.Find(id);
            if (tSTPriorite == null)
            {
                return HttpNotFound();
            }
            return View(tSTPriorite);
        }

        // POST: TSTPriorites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PriorityID,PriorityName,PriorityDescription")] TSTPriorite tSTPriorite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tSTPriorite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tSTPriorite);
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
