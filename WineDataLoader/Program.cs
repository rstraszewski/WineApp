using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Flurl.Http;
using Wine.Core.DataAccess;
using Wine.Core.Entities;

namespace WineDataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = "http://services.wine.com/api/beta2/service.svc/json/catalog?apikey=0f354f5704c4dc771135836f2f84a997&size=1000&offset=0".GetJsonAsync().Result;
            var wineList = ((IEnumerable<dynamic>)result.Products.List).Where(x => x.Type == "Wine");

            var wines = wineList.AsParallel().Select(ToWineEntity).ToList();

            var dbContext = new WineDbContext();
            dbContext.Wines.AddRange(wines);
            dbContext.SaveChanges();
            dbContext.Dispose();
        }

        private static Wine.Core.Entities.Wine ToWineEntity(dynamic wine)
        {
            string url = wine.Labels[0].Url;
            var result = url.GetBytesAsync().Result;

            var entity = new Wine.Core.Entities.Wine();
            entity.Thumbnail = result;
            entity.Category = GetCategory(wine.ProductAttributes);
            entity.Varietal = GetVarietal(wine.Varietal);
            entity.Name = ((string)wine.Name).Replace($" {wine.Vintage}", "");
            entity.Vintage = wine.Vintage != "Non-Vintage" ? ushort.Parse(wine.Vintage) : 2016;
            entity.Region = wine.Appellation.Region.Name;
            entity.Description = string.IsNullOrWhiteSpace(wine.Description) == false 
                ? Regex.Replace(wine.Description, "<.*?>", string.Empty) 
                : $"{entity.Name} has a deep black-purple crimson color that alludes to its aromas of boysenberry, forest floor and Asian spice and flavors ripe blackberry.";

            return entity;
        }

        private static Varietal GetVarietal(dynamic varietal)
        {
            string type = varietal.WineType.Name;

            switch (type)
            {
                case "Champagne & Sparkling":
                    return Varietal.Sparkling;
                case "Red Wines":
                    return Varietal.Red;
                case "White Wines":
                    return Varietal.White;
                case "Rosé Wine":
                    return Varietal.Rose;
                default:
                    return Varietal.Red;
            }
        }

        private static Category GetCategory(IEnumerable<dynamic> attributes)
        {
            string categoryName = attributes.FirstOrDefault(x => x.Id >= 600 && x.Id < 700)?.Name;

            if (categoryName == null)
            {
                return Category.Dry;
            }

            switch (categoryName)
            {
                case "Big &amp; Bold":
                case "Rich &amp; Creamy":
                    return Category.Dry;
                case "Light &amp; Crisp":
                case "Light &amp; Fruity":
                case "Sweet":
                    return Category.Sweet;
                case "Fruity &amp; Smooth":
                case "Smooth &amp; Supple":
                case "Earthy &amp; Spicy":
                    return Category.SemiDry;
                default:
                    return Category.Dry;    

            }
        }
    }
}
