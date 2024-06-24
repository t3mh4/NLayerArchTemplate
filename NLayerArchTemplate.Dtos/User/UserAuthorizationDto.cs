﻿namespace NLayerArchTemplate.Dtos.User;

public record UserAuthorizationDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public string UserFullName { get => string.Format("{0} {1}", Name, Surname); }
}
