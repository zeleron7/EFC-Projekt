using System.Security.Cryptography.X509Certificates;
using Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class csCities :ICities, ISeed<csCities>
{
    [Key]
    public virtual Guid CitiesId {get; set;} 
    public virtual string Name {get; set;}
    public virtual int ZipCode {get; set;}
    public virtual string StreetAddress {get; set;}

     public virtual List<IAttraction> Attraction {get; set;}

    public virtual ICountry Country {get; set;}

    #region seeder
    public bool Seeded { get; set; } = false;

    public virtual csCities Seed (csSeedGenerator _seeder)
    {
        Seeded = true;
        CitiesId = Guid.NewGuid();
        Name = _seeder.City();
        ZipCode = _seeder.Next(1,10);
        StreetAddress = _seeder.StreetAddress();

        return this;
    }
    #endregion
    
}