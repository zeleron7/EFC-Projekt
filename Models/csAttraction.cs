using System.ComponentModel.DataAnnotations;
using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class csAttraction : IAttraction, ISeed<csAttraction>
{
    [Key]
    public virtual Guid AttractionId {get; set;} 
    
    public virtual string Name { get; set; }

    //Navigation props
    public virtual List<IComment> Comments { get; set; }

    public virtual ICities Cities { get; set; }

    public virtual ICountry Country { get; set; }

    #region seeder
    public bool Seeded { get; set; } = false;

    public virtual csAttraction Seed (csSeedGenerator _seeder)
    {
        Seeded = true;
        AttractionId = Guid.NewGuid();
        Name = _seeder.FromString("Muju, Mammas cafe, coffee house");

        return this;
    }
    #endregion
    
}