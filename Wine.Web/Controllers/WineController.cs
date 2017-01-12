using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Wine.Core.DataAccess;
using Wine.Core.Helpers;
using Wine.Web.Models;

namespace Wine.Web.Controllers
{
    public class WineController : Controller
    {
        private readonly WineDbContext _dbContext;

        public WineController()
        {
            _dbContext = new WineDbContext();
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var wine = _dbContext.Wines.Find(id);

            return View("Details", wine);
        }

        public ActionResult Remove([DataSourceRequest] DataSourceRequest request, int id)
        {
            var wine = _dbContext.Wines.Find(id);
            _dbContext.Wines.Remove(wine);
            _dbContext.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CreateWine wine)
        {
            var entity = ToEntityWine(wine);
            _dbContext.Wines.Add(entity);
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { entity.Id });
        }

        private Wine.Core.Entities.Wine ToEntityWine(CreateWine wine)
        {
            byte[] imgData;

            using (var reader = new BinaryReader(wine.Thumbnail.InputStream))
            {
                imgData = reader.ReadBytes(wine.Thumbnail.ContentLength);
            }

            var entity = new Wine.Core.Entities.Wine();
            entity.Name = wine.Name;
            entity.Description = wine.Description;
            entity.Region = wine.Region;
            entity.Category = wine.Category;
            entity.Varietal = wine.Varietal;
            entity.Vintage = wine.Vintage;
            entity.Thumbnail = imgData;

            return entity;
        }

        public ActionResult AddReview(int id, string review, string username)
        {
            var wine = _dbContext.Wines.Find(id);
            var user = _dbContext.Users.Include("Rewievs").First(x => x.Username == HttpContext.User.Identity.Name);
            wine.AddReview(review, user);

            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { id });
        }

        public ActionResult GetWineList([DataSourceRequest] DataSourceRequest request, string filterCritera)
        {
            var wines = _dbContext.Wines.Where(x => x.Search.Contains(filterCritera)).ToList();

            var list = wines.Select(ToListItem);
            var dataSourceResult = list.ToDataSourceResult(request);
            return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        }

        private WineListItem ToListItem(Wine.Core.Entities.Wine entity)
        {
            var item = new WineListItem();
            item.Name = entity.Name;
            item.Category = entity.Category.GetDescription();
            item.Varietal = entity.Varietal.GetDescription();
            item.Description = entity.Description;
            item.Id = entity.Id;
            item.Region = entity.Region;
            item.ThumbnailBase64 = Convert.ToBase64String(entity.Thumbnail);

            return item;
        }

        public new void Dispose()
        {
            _dbContext.Dispose();
            base.Dispose();
        }
    }
}