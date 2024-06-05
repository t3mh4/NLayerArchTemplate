namespace NLayerArchTemplate.Dtos.User
{
    public class UserSaveDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
