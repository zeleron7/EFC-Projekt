using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Seido.Utilities.SeedGenerator;


namespace DbModels;

public class csAttractionDbM : csAttraction, ISeed<csAttractionDbM>
{
    [Key]
    public override Guid AttractionId { get; set; }

    [NotMapped]
    public override List<IComment> Comments { get => commentDbM?.ToList<IComment>(); set => throw new NotImplementedException(); }

    [NotMapped]

    //public override ICities Cities { get => base.Cities; set => base.Cities = value; }

    public override ICities Cities {  get => CitiesDbM; set => throw new NotImplementedException(); }

    [NotMapped]

    public override ICountry Country {  get => CountryDbM; set => throw new NotImplementedException(); }

    [JsonIgnore]
    public  List<csCommentDbM> commentDbM { get; set; }

    public csCitiesDbM CitiesDbM { get; set; }

    public csCountryDbM CountryDbM { get; set; }
    public override csAttractionDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }
}