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
    public class UsersController : Controller
    {
        private newMusicEntities db = new newMusicEntities();


        /*
         * show list to buy by user 
         */
        public ActionResult ShowListToBuyByUser(int? id)
        {
            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            var purchasedSongs = new HashSet<Song>(user.Boughts.Select(b => b.Song).ToList());

            var res = db.Songs.ToList();
            foreach (var s in purchasedSongs)
            {
                if (res.Contains(s))
                    res.Remove(s);
            }

            ViewBag.User = user;
            ViewBag.SongId = new SelectList(res, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult ShowListToBuyByUser(int? Id, int SongId)
        {
            var song = db.Songs.Find(SongId);
            if (song == null)
                return HttpNotFound();

            var user = db.Users.Find(Id);
            if (user == null)
                return HttpNotFound();

            if (user.Money >= song.Price)
            {
                var newBought = new Bought();
                newBought.User = user;
                newBought.Song = song;
                newBought.RatingValue = 0;
                user.Money -= song.Price;

                db.Boughts.Add(newBought);
                db.SaveChanges();

                ViewBag.Status = "Success";
            }
            else
            {
                ViewBag.Status = "Not enough money";
            }

            var purchasedSongs = new HashSet<Song>(user.Boughts.Select(b => b.Song).ToList());

            var res = db.Songs.ToList();
            foreach (var s in purchasedSongs)
            {
                if (res.Contains(s))
                    res.Remove(s);
            }

            ViewBag.User = user;
            ViewBag.SongId = new SelectList(res, "Id", "Name");

            db.SaveChanges();
            return View(user);
        }


        /*  
         * show refund page
         */
        public ActionResult RefundList(int? id)
        {
            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            var purchasedSongs = new HashSet<Song>(user.Boughts.Select(b => b.Song).ToList());

            ViewBag.User = user;
            ViewBag.SongId = new SelectList(purchasedSongs, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult RefundList(int? Id, int SongId)
        {
            var song = db.Songs.Find(SongId);
            if (song == null)
                return HttpNotFound();

            var user = db.Users.Find(Id);
            if (user == null)
                return HttpNotFound();
            
            var previousPurchased = user.Boughts.FirstOrDefault(b => b.Song == song);
            DateTime purchasedDate = previousPurchased.PurchaseDate.Value;
            if ((DateTime.Now - purchasedDate).Days <= 30)
            {
                user.Money += song.Price;
                db.Boughts.Remove(previousPurchased);
                db.SaveChanges();

                ViewBag.Status = "Refund suceess";
            }
            else
            {
                ViewBag.Status = "Over 30 days can not refund";
            }

            var purchasedSongs = new HashSet<Song>(user.Boughts.Select(b => b.Song).ToList());

            ViewBag.User = user;
            ViewBag.SongId = new SelectList(purchasedSongs, "Id", "Name");

            db.SaveChanges();
            return View(user);
        }



        /*
         * Create a page where you select a user, then once you press 
         * “submit”, you will view all the songs this user bought.
         */
        public ActionResult ShowUserBought()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult ShowUserBought(int UserId)
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");

            var user = db.Users.Find(UserId);
            if (user == null)
                return HttpNotFound();
            var res = user.Boughts.ToList();

            return View(res);
        }


        /*
         * Create a page where you select a song, then once you 
         * press “Submit”, you will see how many people bought this song.
         */
        public ActionResult ShowHowManyUserBuy()
        {
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult ShowHowManyUserBuy(int SongId)
        {
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Name");

            var song = db.Songs.Find(SongId);
            if (song == null)
                return HttpNotFound();
            var res = song.Boughts.Count();

            return View(res);
        }


        /*
         * Create a page (Only GET) to show us the song with the top 
         * number of sales.
         */
        public ActionResult TopSong()
        {
            var song = db.Songs
                .OrderByDescending(s => s.Boughts.Count).Take(1).ToList();
            return View(song);
        }


        /* 
         *  Create a page (Only GET) to show us the song with the top 
         *  number of sales.
         */
        public ActionResult TopArtist()
        {
            var allSongsInBought = db.Boughts;

            var dict = new Dictionary<Song, int>();
            foreach (var bought in allSongsInBought)
            {
                if (dict.ContainsKey(bought.Song))
                    dict[bought.Song]++;
                else
                    dict.Add(bought.Song, 1);
            }

            var artistDict = new Dictionary<Artist, int>();
            foreach (var pair in dict)
            {
                if (artistDict.ContainsKey(pair.Key.Artist))
                    artistDict[pair.Key.Artist] += pair.Value;
                else
                    artistDict.Add(pair.Key.Artist, pair.Value);
            }
            var maxCount = artistDict.Max(a => a.Value);
            var result = artistDict.Where(a => a.Value == maxCount)
                .Select(a => a.Key).ToList();

            return View(result);
        }


        /* 
         *  Create a page (GET only) to show the top rated 3 songs.
         */
        public ActionResult Top3Songs()
        {
            var songs = db.Songs.OrderByDescending(s => s.OverallRating)
                .Take(3).ToList();
            return View("~/Views/Songs/Index.cshtml", songs);
        }



        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Money")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Money")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
