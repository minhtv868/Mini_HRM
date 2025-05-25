namespace IC.Application.DTOs
{
    public class TrackChangeDto
    {
        public string TableName { get; set; }
        public string ClassName { get; set; }
        public int DisplayOrder { get; set; }
        public string SqlGetTextValue { get; set; }
        public string DbName { get; set; }
        public string ChangedProperty { get; set; }
        public string OriginalValue { get; set; }
        public string ChangedValue { get; set; }
    }
}
