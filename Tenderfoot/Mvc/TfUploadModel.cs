using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Mvc
{
    public class TfUploadModel : TfModel
    {
        public IFormFile File { get; set; }
        
        [Output]
        [Input(InputType.FileName)]
        public string FileName { get; set; } = Path.GetRandomFileName();

        [Output]
        [Input(InputType.FilePath)]
        public string FilePath { get; set; }

        [Output]
        public string FullUrl { get; set; }

        [Input(InputType.String)]
        public FileType? FileType { get; set; } = Mvc.FileType.Image;

        public override void BeforeStartUp()
        {
            if (!this.FileName.IsEmpty())
            {
                this.FileName = this.FileName
                    .Replace("/", "")
                    .Replace("\\", "");
            }
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.File == null || this.File?.Length == 0)
            {
                yield return TfValidationResult.Compose("RequiredField", nameof(this.File));
            }
            else
            {
                switch (this.FileType)
                {
                    case Mvc.FileType.Image:
                        if (this.File.ContentType != "image/gif" &&
                            this.File.ContentType != "image/jpeg" &&
                            this.File.ContentType != "image/png" &&
                            this.File.ContentType != "image/svg+xml")
                        {
                            yield return TfValidationResult.Compose("InvalidInput", nameof(this.File), nameof(this.File));
                        }
                        break;
                    case Mvc.FileType.CSV:
                        if (this.File.ContentType != "text/csv")
                        {
                            yield return TfValidationResult.Compose("InvalidInput", nameof(this.File), nameof(this.File));
                        }
                        break;
                }
            }
        }

        public override void HandleModel()
        {
            this.Upload();
        }

        private void Upload()
        {
            var file = Path.ChangeExtension(this.FileName, Path.GetExtension(this.File.FileName));
            var fullPath = Path.Combine(this.FilePath, file);
            
            Directory.CreateDirectory(this.FilePath);

            if (this.File.Length > 0)
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    this.File.CopyTo(stream);
                }
            }
            
            this.FullUrl = $"{TfSettings.Web.ApiUrl}/{fullPath.Replace("\\", "/")}";
        }
    }
}
