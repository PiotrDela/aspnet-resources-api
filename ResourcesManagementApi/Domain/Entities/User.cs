namespace ResourcesManagementApi.Domain.Entities
{
    public class User: EntityBase
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
