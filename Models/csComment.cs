using System.ComponentModel.DataAnnotations;
using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class csComment : IComment, ISeed<csComment>
{
    [Key]
    public virtual Guid CommentId {get; set;} //Primary Key
    public virtual string Comment { get; set; }
    public virtual DateTime Date { get; set; }
    public bool Seeded { get; set; } = false;
    public virtual IUser User {get; set;}
    public virtual IAttraction Attraction { get; set; }

    //Seeds random data
    public virtual csComment Seed(csSeedGenerator _seeder)
    {
        Seeded = true;
        CommentId = Guid.NewGuid();
        Comment = _seeder.LatinSentence;
        Date = _seeder.DateAndTime(2023, 2024);

        return this;
    }
}