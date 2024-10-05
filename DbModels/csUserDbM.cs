using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Models;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

public class csUserDbM : csUser, ISeed<csUserDbM>
{
    [Key]
    public override Guid UserId { get; set; }
   
    [NotMapped]
    public override List<IComment> Comments { get => CommentDbM?.ToList<IComment>(); set => throw new NotImplementedException(); }

    [JsonIgnore]
    public virtual List<csCommentDbM> CommentDbM { get; set; } = null;

    public override csUserDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }
}