namespace TeaTime.Api.DataAccess.DbEntities
{
    public class StoreEntity
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string MenuUrl { get; set; } = string.Empty;
    }
}
