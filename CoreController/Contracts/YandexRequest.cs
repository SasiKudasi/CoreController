namespace CoreController.Contracts;

public class YandexRequest
{
    public Session Session { get; set; }
    public Request Request { get; set; }
}

public class Session
{
    public bool New { get; set; }
    public string SessionId { get; set; }
    public User User { get; set; }
}

public class User
{
    public string UserId { get; set; }
}

public class Request
{
    public string Command { get; set; }
    public string Type { get; set; }
}