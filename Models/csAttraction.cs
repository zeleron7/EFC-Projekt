using System.Security.Cryptography.X509Certificates;
using Configuration;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class csAttraction :ISeed<csAttraction> , IAttraction
{
   [Key]
   public virtual Guid AttractionId { get; set; } //Primary Key
   public virtual string Name { get; set; }
   public virtual string Description { get; set; }
   public virtual string Category { get; set; } 
   public virtual string Title { get; set; } 
   public virtual ILocation Locations{ get; set; }
   public virtual List<IComment> Comments { get; set; } = null;
   public virtual bool Seeded { get; set; } = false;

    //Seeds random data
    public virtual csAttraction Seed (csSeedGenerator _seeder)
    {
        var _name = _seeder.LatinWords(1);
        
        Seeded = true;
        AttractionId = Guid.NewGuid();
        Name = _name[0];
        Description = _seeder.LatinSentence;
        Category = _seeder.LatinSentence;  
        Title = _seeder.LatinSentence; 
        
        return this;
    }
}