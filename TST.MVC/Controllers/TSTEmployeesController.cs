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
//------------------------------
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentitySample.Models;
//----------------------------
//for connecting identity to TST

namespace TST.MVC.Controllers
{
    public class TSTEmployeesController : Controller
    {
        private TSTEntities db = new TSTEntities();

        // GET: TSTEmployees
        public ActionResult Index()
        {
            var tSTEmployees = db.TSTEmployees.Include(t => t.TSTDepartment).Include(t => t.TSTEmployeeStatus);
            return View(tSTEmployees.ToList());
        }
    
     
        // GET: TSTEmployees/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployee tSTEmployee = db.TSTEmployees.Find(id);
            var tSTTickets = db.TSTTickets.Include(t => t.TSTEmployee).Include(t => t.TSTEmployee1).Include(t => t.TSTPriorite).Include(t => t.TSTTicketStatus);
            ViewBag.PassMyTickets = tSTTickets.ToList();
            ViewBag.PassImg = tSTEmployee.EmpPhoto;

            if (tSTEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tSTEmployee);
        }

        // GET: TSTEmployees/Create
        public ActionResult Create()
        {
            var RoleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();


            ViewBag.RoleID = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");

            //will eventually populate a checkboxlist
         
            ViewBag.DepartmentID = new SelectList(db.TSTDepartments, "DepartmentID", "DepartmentName");
            ViewBag.EmpStatusID = new SelectList(db.TSTEmployeeStatuses, "EmpStatusID", "EmpStatusName");
            return View();
        }

        // POST: TSTEmployees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "EmpID,EmpFname,EmpLname,DepartmentID,EmpStatusID,EmpAddress1,EmpAddress2,EmpCity,EmpState,EmpPhoto,EmpUserID,EmpDateOfBirth,EmpDateOfHire,EmpEndDate,EmpEmail,EmpPhone,EmpNotes")] TSTEmployee tSTEmployee)
        {
            ModelState.Clear();
            //programmatically generate email from first letter of first name + lname@greendale.com
            string email = tSTEmployee.EmpFname.ToLower().First() + tSTEmployee.EmpLname.ToLower() + "@greendale.com";
            tSTEmployee.EmpEmail = email;
            //ModelState.Add()
            //ModelState.
            //set hire date
            tSTEmployee.EmpDateOfHire = DateTime.Now;


            //set status ID
            //if student enrolled status if employee active status
            if (tSTEmployee.DepartmentID == 6)
            {
                tSTEmployee.EmpStatusID = 2;
            }
            else
            {
                tSTEmployee.EmpStatusID = 1;
            }
            //set default no image

            tSTEmployee.EmpPhoto = "noPhoto.png";
           

            if (ModelState.IsValid)
            {
                try
                {
                    //similar code can be found in the users admin controller

                    //create the user manager
                    var userManager = System.Web.HttpContext.Current.GetOwinContext()
                        .GetUserManager<ApplicationUserManager>();
                    //create new app user and assign username email
                    var newUser = new ApplicationUser()
                    {
                        //object initialization syntax
                        UserName = tSTEmployee.EmpEmail,
                        Email = tSTEmployee.EmpEmail
                    };
                    //send back to identitiy default pass
                    userManager.Create(newUser, "P@ssw0rd");
                    //add the user to selected roles

                    switch (tSTEmployee.DepartmentID)
                    {
                        case 1://admin
                            userManager.AddToRole(newUser.Id, "Admin");
                            break;
                        case 3://teacher
                            userManager.AddToRole(newUser.Id, "Tech");
                            break;
                        case 4://HR
                            userManager.AddToRole(newUser.Id, "Teacher");
                            break;
                        case 5:
                            userManager.AddToRole(newUser.Id, "HR");
                            break;
                        default://student
                            userManager.AddToRole(newUser.Id, "Student");
                            break;
                    }
                    //default to a role if none provided

                    //assign employee identity id
                    tSTEmployee.EmpUserID = newUser.Id;


                    db.TSTEmployees.Add(tSTEmployee);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = tSTEmployee.EmpID });
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("modelstate?", e);
                }
                

   
                }
            
             

            var RoleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();


            ViewBag.RoleID = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");

            ViewBag.DepartmentID = new SelectList(db.TSTDepartments, "DepartmentID", "DepartmentName", tSTEmployee.DepartmentID);
            return View(tSTEmployee);
        }

        // GET: TSTEmployees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployee tSTEmployee = db.TSTEmployees.Find(id);
            //grab image string
           ViewBag.PassImg = tSTEmployee.EmpPhoto;
           
            if (tSTEmployee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.TSTDepartments.Where(d => d.DepartmentID != tSTEmployee.DepartmentID), "DepartmentID", "DepartmentName", tSTEmployee.DepartmentID);

            if( tSTEmployee.DepartmentID == 6)
            {
                ViewBag.EmpStatusID = new SelectList(db.TSTEmployeeStatuses.Where(s => s.EmpStatusID.Equals(2) || s.EmpStatusID.Equals(4) || s.EmpStatusID.Equals(6)), "EmpStatusID", "EmpStatusName", tSTEmployee.EmpStatusID);

            }
            else{
                ViewBag.EmpStatusID = new SelectList(db.TSTEmployeeStatuses.Where(s => s.EmpStatusID.Equals(1) || s.EmpStatusID.Equals(3) || s.EmpStatusID.Equals(5)), "EmpStatusID", "EmpStatusName", tSTEmployee.EmpStatusID);

            }
            return View(tSTEmployee);
        }

        // POST: TSTEmployees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpID,EmpFname,EmpLname,DepartmentID,EmpStatusID,EmpAddress1,EmpAddress2,EmpCity,EmpState,EmpPhoto,EmpUserID,EmpDateOfBirth,EmpDateOfHire,EmpEndDate,EmpPhone,EmpEmail,EmpNotes")] TSTEmployee tSTEmployee, string img, string grabPic)
        {
            if (ModelState.IsValid)
            {
                string imageName = grabPic;
                //get images for delete
                //check if file is empty
                if (img != null)
                {
                    imageName = img.Substring(img.LastIndexOf('/') + 1);
                    
                }
                tSTEmployee.EmpPhoto = imageName;

                db.Entry(tSTEmployee).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.TSTDepartments, "DepartmentID", "DepartmentName", tSTEmployee.DepartmentID);
            ViewBag.EmpStatusID = new SelectList(db.TSTEmployeeStatuses, "EmpStatusID", "EmpStatusName", tSTEmployee.EmpStatusID);
            return View(tSTEmployee);
        }

        // GET: TSTEmployees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployee tSTEmployee = db.TSTEmployees.Find(id);
            if (tSTEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tSTEmployee);
        }

        // POST: TSTEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TSTEmployee tSTEmployee = db.TSTEmployees.Find(id);
            if(tSTEmployee.DepartmentID == 6)
            {
                tSTEmployee.EmpStatusID = 4;
            }
            else
            {
                tSTEmployee.EmpStatusID = 3;
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
                url = "/Content/TSTUserImages/" + fileName
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
            var folder = Server.MapPath("/Content/TSTUserImages/");
            string croppedPath = Path.Combine(folder, fileName);
            targetImage.Save(croppedPath);
            
            return fileName;

        }
   
        public JsonResult ChangePhoto(int Id, string img)
        {
            TSTEmployee emp = db.TSTEmployees.Single(x => x.EmpID == Id);
            var currentUserId = User.Identity.GetUserId();

            //create user employee relationship by matching user id to user id of employee

            TSTEmployee e = db.TSTEmployees.FirstOrDefault(x => x.EmpUserID == currentUserId);
            if (img != null)
            {
               string imageName = img.Substring(img.LastIndexOf('/') + 1);
                emp.EmpPhoto = imageName;
            
                db.SaveChanges();
                var data = new
                {
                    Id = emp.EmpID,
                    Photo = emp.EmpPhoto,
                    ModelUserID = emp.EmpUserID,
                    CurrentUserID = e.EmpUserID
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
           

            return null;

        }
    }
}
