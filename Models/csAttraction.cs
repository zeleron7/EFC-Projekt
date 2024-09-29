using System.Security.Cryptography.X509Certificates;
using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class csAttraction :ISeed<csAttraction> , IAttraction
{
   public virtual Guid AttractionId { get; set; }
   public virtual string Name { get; set; }
   public virtual string Description { get; set; }
   public virtual ILocation Locations{ get; set; }
   public virtual List<IComment> Comments { get; set; } = null;

    #region seeder
    public virtual bool Seeded { get; set; } = false;

    public virtual csAttraction Seed (csSeedGenerator _seeder)
    {
        var _name = _seeder.LatinWords(1);
        
        Seeded = true;
        AttractionId = Guid.NewGuid();
        Name = _name[0];
        Description = _seeder.LatinSentence;
        
        return this;
    }
    #endregion
    
}