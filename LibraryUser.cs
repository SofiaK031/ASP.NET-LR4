namespace LR4;
public class LibraryUser
{
    public int Id {get; set;}
    public string Name {get; set;}

    public override string ToString()
    {
        return "Id: " + Id + "<br>" + "Name: " + Name;
    }
}