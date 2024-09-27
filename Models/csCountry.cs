using Configuration;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class csCountry : ICountry, ISeed<csCountry>
{
    [Key]
    public virtual Guid CountryId {get; set;} 
    public virtual string Name {get; set;}

    //requires a primary key to be defined
    public virtual List<ICities> Cities {get; set;}

    public virtual List<IAttraction> Attraction {get; set;}

    #region seeder
    public bool Seeded { get; set; } = false;

    public virtual csCountry Seed (csSeedGenerator _seeder)
    {
        Seeded = true;
        CountryId = Guid.NewGuid();
        Name = _seeder.FromString("France");

        return this;
    }
    #endregion
    
}