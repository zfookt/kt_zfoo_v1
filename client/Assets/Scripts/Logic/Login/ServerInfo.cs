using System.Collections.Generic;

public class Root
{
    public int code { get; set; }
    public string message { get; set; }
    public Data data { get; set; }
}

public class Data
{
    public string token { get; set; }
    public List<ServerInfo> serverInfoList { get; set; }
}

public class ServerInfo
{
    public string name { get; set; }
    public string ip { get; set; }
    public int port { get; set; }
}