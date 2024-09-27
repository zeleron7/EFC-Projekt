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

    #region adding more readability to an enum type in the database
    public virtual string strFirstName
    {
        get => FirstName.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    public virtual string strLastName
    {
        get => LastName.ToString();
        set { } //set is needed by EFC to include in the database, so I make it to do nothing
    }
    public virtual string strAge
    {
        get => Age.ToString();
        set { } //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion
    
    [NotMapped]
    public override List<IComment> Comments { get => CommentDbM?.ToList<IComment>(); set => throw new NotImplementedException(); }

    [JsonIgnore]
    public  List<csCommentDbM> CommentDbM { get; set; }

    //Used to stop recursion in DbRepos when using .Include in many-to-many relationships
    /*public csAnimalDbM ExludeNavProps() 
    {
        ZooDbM = null;
        return this;
    }*/

    public override csUserDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }
}