namespace ResourcesManagementApi.Domain.Entities
{
    public class User: EntityBase, IEquatable<User>
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        public bool Equals(User other)
        {
            return other != null && other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
