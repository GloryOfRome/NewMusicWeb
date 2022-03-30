using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewMusicWeb;

namespace NewMusicWeb.Controllers
{
    public class BoughtsController : Controller
    {
        private newMusicEntities db = new newMusicEntities();

        // GET: Boughts
        public ActionResult Index()
        {
            var boughts = db.Boughts.Include(b => b.Song).Include(b => b.User);
            return View(boughts.ToList());
        }

        // GET: Boughts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bought bought = db.Boughts.Find(id);
            if (bought == null)
            {
                return HttpNotFound();
            }
            return View(bought);
        }

        // GET: Boughts/Create
        public ActionResult Create()
        {
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Boughts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RatingValue,UserId,SongId,PurchaseDate")] Bought bought)
        {
            if (ModelState.IsValid)
            {
                var song = db.Songs.FirstOrDefault(t => t.Id == bought.SongId);
                var songPrice = song?.Price;
                var user = db.Users.FirstOrDefault(t => t.Id == bought.UserId);
                var userMoney = user?.Money;

                if (songPrice > userMoney)
                {
                    return HttpNotFound();
                }

                db.Entry(user).Entity.Money -= songPrice.Value; 


                db.Boughts.Add(bought);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SongId = new SelectList(db.Songs, "Id", "Name", bought.SongId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", bought.UserId);
            return View(bought);
        }

        // GET: Boughts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bought bought = db.Boughts.Find(id);
            if (bought == null)
            {
                return HttpNotFound();
            }
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Name", bought.SongId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", bought.UserId);
            return View(bought);
        }

        // POST: Boughts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RatingValue,UserId,SongId,PurchaseDate")] Bought bought)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bought).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Name", bought.SongId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", bought.UserId);
            return View(bought);
        }

        // GET: Boughts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bought bought = db.Boughts.Find(id);
            if (bought == null)
            {
                return HttpNotFound();
            }
            return View(bought);
        }

        // POST: Boughts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bought bought = db.Boughts.Find(id);
            db.Boughts.Remove(bought);
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
