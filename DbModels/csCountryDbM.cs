using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Seido.Utilities.SeedGenerator;


namespace DbModels;

public class csCountryDbM : csCountry, ISeed<csCountryDbM>
{
    [Key]
    public override Guid CountryId { get; set; }

    #region adding more readability to an enum type in the database
    public virtual string strName
    {
        get => Name.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    public virtual string strCities
    {
        get => Cities.ToString();
        set { } //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion
    
    [NotMapped]
    public override List<IAttraction> Attraction { get => AttractionDbM?.ToList<IAttraction>(); set => throw new NotImplementedException(); }

    [NotMapped]

    public override List<ICities> Cities { get => CitiesDbM?.ToList<ICities>(); set => throw new NotImplementedException(); }

    [JsonIgnore]
    public List <csAttractionDbM> AttractionDbM { get; set; }

    public List<csCitiesDbM> CitiesDbM { get; set; }

    //Used to stop recursion in DbRepos when using .Include in many-to-many relationships
    /*public csAnimalDbM ExludeNavProps() 
    {
        ZooDbM = null;
        return this;
    }*/

    public override csCountryDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }
}