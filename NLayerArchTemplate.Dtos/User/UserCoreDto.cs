namespace NLayerArchTemplate.Dtos.User;

public record UserCoreDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsActive { get; set; }
    //public DateTime CreatedDate { get; set; }
    //public string CreatedBy { get; set; }
    //public DateTime? ModifiedDate { get; set; }
    //public string ModifiedBy { get; set; }
}
