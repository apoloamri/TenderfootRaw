using MiniCmsLibrary;
using MiniCmsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace MiniCms.Models.Admin
{
    public class AdminPagesModel : TfModel<AdminLibrary>, IAdmin
    {
        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }
    }
}