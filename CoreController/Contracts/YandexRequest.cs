namespace CoreController.Contracts;

public class YandexRequest
{
    public Session Session { get; set; }
    public Request Request { get; set; }
}

public class Session
{
    public bool New { get; set; }
    public string session_id { get; set; }
    public User User { get; set; }
}

public class User
{
    public string user_id { get; set; }
}

public class Request
{
    public string Command { get; set; }
    public string Type { get; set; }
}




