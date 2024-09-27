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

    #region adding more readability to an enum type in the database
    public virtual string strComment
    {
        get => Comment.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    public virtual string strDate
    {
        get => Date.ToString();
        set { } //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion
    
    [NotMapped]
    public override IUser User { get => UserDbM; set => throw new NotImplementedException(); }
    
    [NotMapped]

    public override IAttraction Attraction { get => AttractionDbM; set => throw new NotImplementedException(); }


    [JsonIgnore]
    public  csUserDbM UserDbM { get; set; }

    public csAttractionDbM AttractionDbM { get; set; }

    //Used to stop recursion in DbRepos when using .Include in many-to-many relationships
    /*public csAnimalDbM ExludeNavProps() 
    {
        ZooDbM = null;
        return this;
    }*/

    public override csCommentDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }
}