using PrayerForumsLibrary.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools.Extensions;

namespace PrayerForums.Models.Prayer
{
    public class InsertPraiseModel : TfModel
    {
        [Input]
        [Output]
        public Praises Praise { get; set; } = new Praises();
        
        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void HandleModel()
        {
            var praises = _Schemas.Praises;
            praises.Entity.SetValuesFromModel(this.Praise);
            praises.Insert();
            var id = praises.Entity.id;
            var imageUrl = praises.Entity.image_url;
            praises.Clear();
            praises.Entity.image_url = this.ImageUrl(imageUrl, id);
            praises.Case.Where(praises.Column(x => x.id), Is.EqualTo, id);
            praises.Update();
        }

        private string ImageUrl(string imageUrl, int? id)
        {
            if (this.Praise.image_url.IsEmpty())
            {
                return string.Empty;
            }

            var tempPath = TfSettings.GetSettings("PrayerForums", "ImageUploadTemp");
            var imagePath = TfSettings.GetSettings("PrayerForums", "ImageUploadPraise");
            var fileName = Path.GetFileName(imageUrl);

            Directory.CreateDirectory(imagePath);

            var oldPath = Path.Combine(tempPath, fileName);
            var newPath = Path.Combine(imagePath, $"praise_{id}{Path.GetExtension(fileName)}");

            File.Move(oldPath, newPath);

            return $"{TfSettings.Web.SiteUrl}/{newPath.Replace("\\", "/").Replace("wwwroot/", "")}";
        }
    }
}