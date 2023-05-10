public class Team
{
    public Team(string name, Roles roles)
    {
        Name = name;
        Roles = roles;
    }
    
    public Team(string name, Roles roles, byte score)
    {
        Name = name;
        Roles = roles;
        Score = score;
    }

    public string Name;
    public Roles Roles;
    public byte Score;
}