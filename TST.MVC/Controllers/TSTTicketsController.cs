using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
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
        
        public TSTEmployee GetCurrentEmployee()
        {

            var currentUserId = User.Identity.GetUserId();

            //create user employee relationship by matching user id to user id of employee

            TSTEmployee e = db.TSTEmployees.FirstOrDefault(x => x.EmpUserID == currentUserId);
            return e;
        }

        [Authorize(Roles = "Admin, Tech")]
        public JsonResult AddTechNote(int ticketId, string note)
        {
            //get the ticket that was passed in and pull the associated record

            //get the current logged on employee (the tech working on the ticket)

            //this only works cause of stuff we made happen yeah 


            TSTTicket ticket = db.TSTTickets.Single(x => x.TicketID == ticketId);

            TSTEmployee tech = GetCurrentEmployee();
            if(tech != null)
            {
                TSTTechNote newNote = new TSTTechNote()
                {
                    TicketID = ticketId,
                    TechID = tech.EmpID,
                    NotationDate = DateTime.Now,
                    Notation = note
                };
                db.TSTTechNotes.Add(newNote);
                db.SaveChanges();
                var data = new
                {
                    TechNotes = newNote.Notation,
                    Tech = newNote.TSTEmployee.GetFullName,
                    Date = string.Format("{0:g}", newNote.NotationDate)


                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            return null;
            
        }

        [HttpPost]

        public string UploadOriginalImage(HttpPostedFileBase img)
        {
            string folder = Server.MapPath("~/Content/Temp");
            string ext = Path.GetExtension(img.FileName);
            string pic = System.IO.Path.GetFileName(Guid.NewGuid().ToString());
            var tempPath = Path.ChangeExtension(pic, ext);
            string tempFilePath = System.IO.Path.Combine(folder, tempPath);
            img.SaveAs(tempFilePath);
            var image = System.Drawing.Image.FromFile(tempFilePath);
            var result = new
            {
                status = "success",
                width = image.Width,
                height = image.Height,
                url = "/Content/Temp/" + tempPath
            };
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string CroppedImage(string imgUrl, int imgInitW, int imgInitH, double imgW, double imgH, int imgY1, int imgX1, int cropH, int cropW)
        {
            var originalFilePath = Server.MapPath(imgUrl);
            var fileName = CropImage(originalFilePath, imgInitW, imgInitH, (int)imgW, (int)imgH, imgY1, imgX1, cropH, cropW);
            var result = new
            {
                status = "success",
                url = "/Content/TSTTickets/" + fileName
            };
            return JsonConvert.SerializeObject(result);
        }

        private string CropImage(string originalFilePath, int origW, int origH, int targetW, int targetH, int cropStartY, int cropStartX, int cropW, int cropH)
        {
            var originalImage = Image.FromFile(originalFilePath);

            var resizedOriginalImage = new Bitmap(originalImage, targetW, targetH);
            var targetImage = new Bitmap(cropW, cropH);

            using (var g = Graphics.FromImage(targetImage))
            {
                g.DrawImage(resizedOriginalImage, new Rectangle(0, 0, cropW, cropH), new Rectangle(cropStartX, cropStartY, cropW, cropH), GraphicsUnit.Pixel);
            }
            string fileName = Path.GetFileName(originalFilePath);
            var folder = Server.MapPath("/Content/TSTTickets/");
            string croppedPath = Path.Combine(folder, fileName);
            targetImage.Save(croppedPath);

            return fileName;

        }

        public JsonResult ChangePhoto(int Id, string img)
        {
          
            if (img != null)
            {
                TSTTicket tSTTicket = db.TSTTickets.Find(Id);

                string imageName = img.Substring(img.LastIndexOf('/') + 1);
                tSTTicket.Image = imageName;

                db.SaveChanges();
                var data = new
                {
                    Id = tSTTicket.TicketID,
                    Image = tSTTicket.Image
                  
                  
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }


            return null;

        }

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
            ViewBag.PassImg = tSTTicket.Image;
    
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
                TSTEmployee e = GetCurrentEmployee();
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
        [Authorize(Roles = "Admin, Tech")]
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
        [Authorize(Roles = "Admin, Tech")]
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
        [Authorize(Roles = "Admin, Tech")]
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
        [Authorize(Roles = "Admin, Tech")]
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
