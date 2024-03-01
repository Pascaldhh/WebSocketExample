namespace server.data;

public enum RecieveType
{
    Init, Movement
}

public interface IRecieveType
{
    public RecieveType Type { get; set; }
}
public class Data
{
    public string Name { get; set; }
    public string Color { get; set; }
    public PlayerMovement? Movement { get; set; }
    public bool Jump { get; set; }
}
public class RecieveData : IRecieveType
{
    public RecieveType Type { get; set; }
    public Data? Data { get; set; }
}