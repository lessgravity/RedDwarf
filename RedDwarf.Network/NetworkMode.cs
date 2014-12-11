namespace RedDwarf.Network
{
    public enum NetworkMode : byte
    {
        Handshake = 0,
        Status = 1,
        Login = 2,
        Play = 3
    }
}
