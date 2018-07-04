using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.TfSystem;

namespace Tenderfoot.Mvc
{
    public class TfLibrary : TfBaseModel
    {
        public virtual IEnumerable<ValidationResult> AutoValidate<I>(I model) 
            where I : ITfModel
        {
            return new List<ValidationResult>();
        }
    }
}
