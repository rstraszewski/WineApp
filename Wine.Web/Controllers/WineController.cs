using System;
using System.Data.SqlClient;
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

        [Authorize(Roles = "Viewer")]
        public ActionResult Index()
        {
            return View("Index");
        }

        [Authorize(Roles = "Viewer")]
        public ActionResult Details(int id)
        {
            var wine = _dbContext.Wines.Include("Reviews").Include("Reviews.User").First(x => x.Id == id);

            return View("Details", wine);
        }

        [Authorize(Roles = "WineManager")]
        public ActionResult Remove([DataSourceRequest] DataSourceRequest request, int id)
        {
            var wine = _dbContext.Wines.Find(id);
            _dbContext.Wines.Remove(wine);
            _dbContext.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize(Roles = "WineManager")]
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
            var user = _dbContext.Users.First(x => x.Username == HttpContext.User.Identity.Name);
            wine.AddReview(review, user);

            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Viewer")]
        public ActionResult GetWineList([DataSourceRequest] DataSourceRequest request, string filterCritera)
        {
            filterCritera = string.IsNullOrWhiteSpace(filterCritera) ? null : filterCritera;
            var wines = _dbContext.Database.SqlQuery<Core.Entities.Wine>("GetWineList @page = {0}, @pageSize = {1}, @filterCriteria = {2}", 
                    request.Page - 1, request.PageSize, filterCritera);
            var list = wines.Select(ToListItem);
            var dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = list;
            dataSourceResult.Total = _dbContext.Wines.Count();
            
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