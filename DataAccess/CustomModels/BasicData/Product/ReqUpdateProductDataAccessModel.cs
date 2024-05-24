namespace DataAccess.CustomModels.BasicData
{
    public class ReqUpdateProductDataAccessModel
    {
        public string Oid { get; set; } = null!;

        public string LastModifier { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;
    }
}