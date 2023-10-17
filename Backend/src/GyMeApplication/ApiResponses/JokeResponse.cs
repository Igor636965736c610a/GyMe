namespace GyMeApplication.ApiResponses;

public class JokeResponse
{
    public bool Error { get; set; }
    public string Category { get; set; }
    public JokeType Type { get; set; }
    public string Setup { get; set; }
    public string Joke { get; set; }
    public string Delivery { get; set; }
    public FlagsResponse Flags { get; set; }
    public int Id { get; set; }
    public bool Safe { get; set; }
    public string Lang { get; set; }
}

public class FlagsResponse
{
    public bool Nsfw { get; set; }
    public bool Religious { get; set; }
    public bool Political { get; set; }
    public bool Racist { get; set; }
    public bool Sexist { get; set; }
    public bool Explicit { get; set; }
}

public enum JokeType
{
    single,
    twopart
}