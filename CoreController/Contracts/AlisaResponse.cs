namespace CoreController.Contracts;

public class AlisaResponse
{
    public string Version { get; set; }
    public Session Session { get; set; }
    public Response Response { get; set; }
}

public class Response
{
    public string Text { get; set; }
    public bool End_Session { get; set; }
}
