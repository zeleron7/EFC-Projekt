using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Seido.Utilities.SeedGenerator;


namespace DbModels;

public class csCitiesDbM : csCities, ISeed<csCitiesDbM>
{
    [Key]
    public override Guid CitiesId { get; set; }

    #region adding more readability to an enum type in the database
    public virtual string strName
    {
        get => Name.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    public virtual string strZipCode
    {
        get => ZipCode.ToString();
        set { } //set is needed by EFC to include in the database, so I make it to do nothing
    }
    public virtual string strStreetAddress
    {
        get => StreetAddress.ToString();
        set { } //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion

    [NotMapped]

    public override ICountry Country { get => CountryDbM; set => throw new NotImplementedException(); }

    [NotMapped]

    public override List<IAttraction> Attraction { get => AttractionDbM?.ToList<IAttraction>(); set => throw new NotImplementedException(); }

    [JsonIgnore]
    
    public List<csAttractionDbM> AttractionDbM { get; set; }

    public csCountryDbM CountryDbM { get; set; }


    //Used to stop recursion in DbRepos when using .Include in many-to-many relationships
    /*public csCitiesDbM ExludeNavProps() 
    {
        CitiesDbM = null;
        return this;
    }*/

    public override csCitiesDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }
}