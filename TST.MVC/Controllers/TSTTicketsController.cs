using Microsoft.AspNet.Identity;
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
    public class TSTTicketsController : Controller
    {
        private TSTEntities db = new TSTEntities();

        // GET: TSTTickets
        public ActionResult Index()
        {
            var tSTTickets = db.TSTTickets.Include(t => t.TSTEmployee).Include(t => t.TSTEmployee1).Include(t => t.TSTPriorite).Include(t => t.TSTTicketStatus);
            return View(tSTTickets.ToList());
        }

        // GET: TSTTickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicket tSTTicket = db.TSTTickets.Find(id);
            if (tSTTicket == null)
            {
                return HttpNotFound();
            }
            return View(tSTTicket);
        }

        // GET: TSTTickets/Create
        public ActionResult Create()
        {
        
         
        
            return View();
        }

        // POST: TSTTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketID,TicketSubject,TicketDescription,SubmittedByID,TechID,TicketSubmitted,TicketResolved,TicketStatusID,Image,PriorityID")] TSTTicket tSTTicket)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();

                //create user employee relationship by matching user id to user id of employee

                TSTEmployee e = db.TSTEmployees.FirstOrDefault(x => x.EmpUserID == currentUserId);

                tSTTicket.SubmittedByID = e.EmpID;
                tSTTicket.TicketSubmitted = DateTime.Now;
                tSTTicket.TicketStatusID = 1;
                tSTTicket.Image = "Noimage.png";
                tSTTicket.PriorityID = 1;

                db.TSTTickets.Add(tSTTicket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubmittedByID = new SelectList(db.TSTEmployees, "EmpID", "EmpFname", tSTTicket.SubmittedByID);
            ViewBag.TechID = new SelectList(db.TSTEmployees, "EmpID", "EmpFname", tSTTicket.TechID);
            ViewBag.PriorityID = new SelectList(db.TSTPriorites, "PriorityID", "PriorityName", tSTTicket.PriorityID);
            ViewBag.TicketStatusID = new SelectList(db.TSTTicketStatuses, "TicketStatusID", "TicketStatusName", tSTTicket.TicketStatusID);
            return View(tSTTicket);
        }

        // GET: TSTTickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicket tSTTicket = db.TSTTickets.Find(id);
            if (tSTTicket == null)
            {
                return HttpNotFound();
            }
 
            ViewBag.TechID = new SelectList(db.TSTEmployees.Where(x => x.DepartmentID == 3), "EmpID", "EmpFname", tSTTicket.TechID);
            ViewBag.PriorityID = new SelectList(db.TSTPriorites, "PriorityID", "PriorityName", tSTTicket.PriorityID);
            ViewBag.TicketStatusID = new SelectList(db.TSTTicketStatuses.Where(x => x.TicketStatusID !=1), "TicketStatusID", "TicketStatusName", tSTTicket.TicketStatusID);
            return View(tSTTicket);
        }

        // POST: TSTTickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketID,TicketSubject,TicketDescription,SubmittedByID,TechID,TicketSubmitted,TicketResolved,TicketStatusID,Image,PriorityID")] TSTTicket tSTTicket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tSTTicket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TechID = new SelectList(db.TSTEmployees.Where(x => x.DepartmentID == 3), "EmpID", "EmpFname", tSTTicket.TechID);
            ViewBag.PriorityID = new SelectList(db.TSTPriorites, "PriorityID", "PriorityName", tSTTicket.PriorityID);
            ViewBag.TicketStatusID = new SelectList(db.TSTTicketStatuses.Where(x => x.TicketStatusID != 1), "TicketStatusID", "TicketStatusName", tSTTicket.TicketStatusID);
            return View(tSTTicket);
        }

        // GET: TSTTickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicket tSTTicket = db.TSTTickets.Find(id);
            if (tSTTicket == null)
            {
                return HttpNotFound();
            }
            return View(tSTTicket);
        }

        // POST: TSTTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TSTTicket tSTTicket = db.TSTTickets.Find(id);
            var currentUserId = User.Identity.GetUserId();

            //create user employee relationship by matching user id to user id of employee

            TSTEmployee e = db.TSTEmployees.FirstOrDefault(x => x.EmpUserID == currentUserId);
            if(e.DepartmentID == 1 || e.DepartmentID == 3)
            {
                switch (tSTTicket.TicketStatusID)
                {
                    case 1:
                        //assign ticket
                        tSTTicket.TicketStatusID = 2;
                        tSTTicket.TSTEmployee1 = e;
                        break;
                    case 2:
                        tSTTicket.TicketStatusID = 3;
                        //move it to in progress status
                        break;
                    case 3:
                        tSTTicket.TicketStatusID = 4;
                        //resolve the ticket
                        tSTTicket.TicketResolved = DateTime.Now;
                        tSTTicket.PriorityID = 1;
                        break;
                    case 4:
                        tSTTicket.TicketStatusID = 1;
                        //reopen ticket un asssign from tech. Put in very high priority
                        tSTTicket.TechID = null;
                        tSTTicket.PriorityID = 4;
                        break;
                    default:
                        break;
                }
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
