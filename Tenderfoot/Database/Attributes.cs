using System;
using Tenderfoot.Database.System;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Database
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BaseAttribute : Attribute
    {
        public virtual string GetValue() { return string.Empty; }
    }

    /// <summary>
    /// Sets the default value of the property when written.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : BaseAttribute
    {
        public string DefaultObject { get; set; }
        public DefaultAttribute(string defaultValue)
        {
            this.DefaultObject = defaultValue;
        }
        public override string GetValue()
        {
            var attributeString = string.Empty;
            if (!this.DefaultObject.IsEmpty())
            {
                attributeString += $"DEFAULT ";

                if (this.DefaultObject.Contains("(") && this.DefaultObject.Contains(")"))
                {
                    attributeString += $"{this.DefaultObject}";
                }
                else
                {
                    attributeString += $"'{this.DefaultObject}'";
                }
            }
            return attributeString;
        }
    }

    /// <summary>
    /// Encrypts the property when it is written. Then decrypts when it is read.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptAttribute : Attribute { }

    /// <summary>
    /// Sets the length of the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LengthAttribute : BaseAttribute
    {
        public int LengthCount { get; set; }
        public LengthAttribute(int lenth)
        {
            this.LengthCount = lenth;
        }
        public override string GetValue()
        {
            return $"({this.LengthCount}) ";
        }
    }

    /// <summary>
    /// Sets as the primary key for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : BaseAttribute
    {
        public override string GetValue()
        {
            return $"{ColumnAttributes.PRIMARY_KEY.GetString()} ";
        }
    }

    /// <summary>
    /// Sets as NOT NULL for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullAttribute : BaseAttribute
    {
        public override string GetValue()
        {
            return $"{ColumnAttributes.NOT_NULL.GetString()} ";
        }
    }

    /// <summary>
    /// Sets as SERIAL type for number type properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SerialAttribute : Attribute { }

    /// <summary>
    /// Ignores as column when the entity is created.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NonTableColumnAttribute : Attribute { }

    /// <summary>
    /// Sets as UNIQUE for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : BaseAttribute
    {
        public override string GetValue()
        {
            return $"{ColumnAttributes.UNIQUE} ";
        }
    }

    /// <summary>
    /// Sets as TEXT type for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TextAttribute : Attribute { }
}
