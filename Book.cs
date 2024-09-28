namespace LR4;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string ToString(){
        return "Id: " + Id.ToString() + "<br>Name: " + Name + "<br>Author: " + Author;
    }
}