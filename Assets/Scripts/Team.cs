using Unity.Netcode;

public class Team : INetworkSerializable
{
    public Team() { }

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

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Name);
        serializer.SerializeValue(ref Roles);
        serializer.SerializeValue(ref Score);
    }
}