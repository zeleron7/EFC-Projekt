using Configuration;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class csUser : IUser, ISeed<csUser>
{
    [Key]
    public virtual Guid UserId {get; set;} 

    public virtual string FirstName {get; set;}
    public virtual string LastName {get; set;}
    
    public virtual DateTime Age {get; set;}

    public bool Seeded { get; set; } = false;

    public virtual List<IComment> Comments {get; set;}

    public virtual csUser Seed (csSeedGenerator _seeder)
    {
        Seeded = true;
        UserId = Guid.NewGuid();
        FirstName = _seeder.FirstName;
        LastName = _seeder.LastName;
        Age = _seeder.DateAndTime(1998,2002);

        return this;
    }
}