namespace server.data;

public enum RecieveType
{
    Init
}

public interface IRecieveType
{
    public RecieveType Type { get; set; }
}
public class InitData
{
    public string Name { get; set; }
    public string Color { get; set; }
}
public class RecieveInitData : IRecieveType
{
    public RecieveType Type { get; set; }
    public InitData? Data { get; set; }
}