using System.Security.Cryptography.X509Certificates;
using Configuration;
using Seido.Utilities.SeedGenerator;
using System.ComponentModel.DataAnnotations;

namespace Models;


public class csLocation : ISeed<csLocation> , ILocation, IEquatable<csLocation>
{
    [Key]
    public virtual Guid LocationId { get; set; } //Primary Key
    public virtual string Country { get; set; }  
    public virtual string City { get; set; }
    public string StreetAddress { get; set; }
    public virtual List<IAttraction> Attractions { get; set; }
    public virtual bool Seeded { get; set; } = false;

    public bool Equals(csLocation other) => (other != null) ? ((City, Country) ==
      (other.City, other.Country)) : false;

    public override bool Equals(object obj) => Equals(obj as csLocation);
    public override int GetHashCode() => (City, Country).GetHashCode();

    //Seeds random data
    public virtual csLocation Seed(csSeedGenerator _seed)
    {
      Seeded = true;
      Country = _seed.Country;
      StreetAddress = _seed.StreetAddress(Country);
      City = _seed.City(Country);
      LocationId = Guid.NewGuid();
      return this;
    }
}

    
    
    
