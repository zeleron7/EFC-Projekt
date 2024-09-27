namespace Models;
public enum enAnimalKind {Zebra, Elephant, Lion, Leopard, Gasell}
public enum enAnimalMood { Happy, Hungry, Lazy, Sulky, Buzy, Sleepy };

public interface IAttraction
{
    //
    public Guid AttractionId {get; set;}
     public string Name { get; set; }

    //Navigation props
    public List<IComment> Comments { get; set; }
    public ICities Cities { get; set; }
    public  ICountry Country { get; set; }
}
    
public interface ICities
{
    //
    public Guid CitiesId {get; set;}
    public string Name {get; set;}
    public int ZipCode {get; set;}
    public string StreetAddress {get; set;}
    public ICountry Country {get; set;}

     public List<IAttraction> Attraction {get; set;}
}
public interface IComment
{
    //
    public Guid CommentId {get; set;}
    public string Comment { get; set; }
    public DateTime Date { get; set; }
    
    public IUser User { get; set; }
    //Navigation props
    public IAttraction Attraction { get; set; }
}
public interface ICountry
{
    //
    public Guid CountryId {get; set;}
    public string Name {get; set;}
    public List<ICities> Cities {get; set;}
    public List<IAttraction> Attraction {get; set;}
}
public interface IUser
{
    //
    public Guid UserId {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    
    public DateTime Age {get; set;}

     public List<IComment> Comments {get; set;}
}

