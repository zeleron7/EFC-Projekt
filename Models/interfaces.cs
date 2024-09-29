namespace Models;
public enum enAnimalKind {Zebra, Elephant, Lion, Leopard, Gasell}
public enum enAnimalMood { Happy, Hungry, Lazy, Sulky, Buzy, Sleepy };

public interface IAttraction
{
  public Guid AttractionId { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public string Category { get; set; } 
   public string Title { get; set; } 
  public ILocation Locations{ get; set; }
  public List<IComment> Comments { get; set; } 

}
public interface ILocation 
{
  public Guid LocationId { get; set; }
  public string Country { get; set; }  
  public string City { get; set; }
  public string StreetAddress { get; set; }
  public List<IAttraction> Attractions { get; set; }

}   

public interface IComment
{
    public Guid CommentId {get; set;}
    public string Comment { get; set; }
    public DateTime Date { get; set; }
    
    public IUser User { get; set; }
    public IAttraction Attraction { get; set; }
}
public interface IUser
{
    public Guid UserId {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    
    public DateTime Age {get; set;}
     public List<IComment> Comments {get; set;}
}

