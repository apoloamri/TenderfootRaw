namespace Tenderfoot.Database.Tables
{
    public class InformationSchema : Entity
    {
        public string table_catalog { get; set; }
        public string table_schema { get; set; }
        public string table_name { get; set; }
        public string table_type { get; set; }

        public string column_name { get; set; }
        public int? ordinal_position { get; set; }
        public string column_default { get; set; }
        public string is_nullable { get; set; }
        public string data_type { get; set; }
        public int? character_maximum_length { get; set; }
    }
}
