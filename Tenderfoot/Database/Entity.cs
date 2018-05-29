using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;

namespace Tenderfoot.Database
{
    public abstract class Entity : TfBaseModel
    {
        [Serial]
        [PrimaryKey]
        [NotNull]
        public int? id { get; set; }

        [NotNull]
        [Default("Now()")]
        public virtual DateTime? insert_time { get; set; }

        public string[] GetColumns()
        {
            return this
                .GetType()
                .GetProperties()
                .Where(x =>
                {
                    if (x.GetCustomAttribute<NonTableColumnAttribute>(false) == null)
                    {
                        return true;
                    }
                    return false;
                })
                .Select(x => x.Name)
                .ToArray();
        }

        public string[] GetInputColumns()
        {
            return this
                .GetType()
                .GetProperties()
                .Where(x =>
                {
                    if (x.GetCustomAttribute<InputAttribute>(false) != null)
                    {
                        return true;
                    }
                    return false;
                })
                .Select(x => x.Name)
                .ToArray();
        }
        
        public override bool HasAttribute(PropertyInfo property)
        {
            return
                property != null &&
                property?.GetCustomAttribute<NonTableColumnAttribute>(false) == null;
        }
    }
}
