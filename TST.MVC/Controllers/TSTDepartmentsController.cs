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
    public class TSTDepartmentsController : Controller
    {
        private TSTEntities db = new TSTEntities();

        // GET: TSTDepartments
        public ActionResult Index()
        {
            return View(db.TSTDepartments.ToList());
        }

        // GET: TSTDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTDepartment tSTDepartment = db.TSTDepartments.Find(id);
            if (tSTDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tSTDepartment);
        }

        // GET: TSTDepartments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TSTDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,DepartmentName,DepartmentDescription,IsActive")] TSTDepartment tSTDepartment)
        {
            if (ModelState.IsValid)
            {
                tSTDepartment.IsActive = true;
                db.TSTDepartments.Add(tSTDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tSTDepartment);
        }

        // GET: TSTDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTDepartment tSTDepartment = db.TSTDepartments.Find(id);
            if (tSTDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tSTDepartment);
        }

        // POST: TSTDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,DepartmentName,DepartmentDescription,IsActive")] TSTDepartment tSTDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tSTDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tSTDepartment);
        }

        // GET: TSTDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTDepartment tSTDepartment = db.TSTDepartments.Find(id);
            if (tSTDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tSTDepartment);
        }

        // POST: TSTDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TSTDepartment tSTDepartment = db.TSTDepartments.Find(id);

            if(tSTDepartment.IsActive == true)
            {
                tSTDepartment.IsActive = false;
            }
            else
            {
                tSTDepartment.IsActive = true;
            }

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
