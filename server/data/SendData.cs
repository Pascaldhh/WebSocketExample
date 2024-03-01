namespace server.data;

public enum SendType
{
    InitConfirm, GameInfo
}
public interface ISendType
{
    public SendType Type { get; set; }
}
public class InitConfirmData
{
    public bool IsReady { get; set; }
}
public class SendData(SendType type, object data) : ISendType
{
    public SendType Type { get; set; } = type;
    public object Data { get; set; } = data;
}