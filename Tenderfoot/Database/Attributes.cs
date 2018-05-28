using System;

namespace Tenderfoot.Database
{
    /// <summary>
    /// Sets the default value of the property when written.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : Attribute
    {
        public string DefaultObject { get; set; }

        public DefaultAttribute(string defaultValue)
        {
            this.DefaultObject = defaultValue;
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
    public class LengthAttribute : Attribute
    {
        public int LengthCount { get; set; }

        public LengthAttribute(int lenth)
        {
            this.LengthCount = lenth;
        }
    }

    /// <summary>
    /// Sets as the primary key for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute { }

    /// <summary>
    /// Sets as NOT NULL for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullAttribute : Attribute { }

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
    public class UniqueAttribute : Attribute { }

    /// <summary>
    /// Sets as TEXT type for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TextAttribute : Attribute { }
}
