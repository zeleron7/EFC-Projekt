using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Seido.Utilities.SeedGenerator;


namespace DbModels;

public class csCommentDbM : csComment, ISeed<csCommentDbM>
{
    [Key]
    public override Guid CommentId { get; set; }

    [NotMapped]
    public override IUser User { get => UserDbM; set => throw new NotImplementedException(); }
    
    [NotMapped]
    public override IAttraction Attraction { get => AttractionDbM; set => throw new NotImplementedException(); }

    [JsonIgnore]
    public virtual csUserDbM UserDbM { get; set; } = null;
    
    [JsonIgnore]
    public virtual csAttractionDbM AttractionDbM { get; set; } = null;

    public override csCommentDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }
}