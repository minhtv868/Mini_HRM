namespace Web.Application.Features.Finance.Articles.DTOs
{
    public class ArticleGetPageDto : ArticleDto//, IAuditable
    {
        //public string SendMethod { get; set; }
        public string CrUser { get; set; }
        public string CategoryName { get; set; }
        //public int? CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public int? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public AuditableInfoDto AuditableInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
