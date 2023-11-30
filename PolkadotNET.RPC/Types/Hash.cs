namespace PolkadotNET.RPC.Types;

public readonly struct Hash
{
    private readonly byte[] _value;

    public Hash(byte[] value)
    {
        _value = value;
    }

    public Hash(string value)
    {
        value = value.StartsWith("0x") ? value[2..] : value;
        _value = Convert.FromHexString(value);
    }

    public override string ToString()
        => $"0x{Convert.ToHexString(_value).ToLower()}";
    
    public static implicit operator string(Hash h) => h.ToString();
}