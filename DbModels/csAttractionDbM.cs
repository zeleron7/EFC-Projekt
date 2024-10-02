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
    public override List<IComment> Comments { get => CommentDbM?.ToList<IComment>(); set => throw new NotImplementedException(); }

    [NotMapped]
    public override ILocation Locations { get => LocationDbM; set => throw new NotImplementedException(); }

    [JsonIgnore]
    public virtual List<csCommentDbM> CommentDbM { get; set; } = null;

    [JsonIgnore]
    public virtual csLocationDbM LocationDbM { get; set; } = null; 

    public override csAttractionDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }
}