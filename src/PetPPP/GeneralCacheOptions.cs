namespace PetPPP;

public class GeneralCacheOptions
{
    public int SlidingExpiration { get; init; }
    public int AbsoluteExpiration { get; init; }
    public int AbsoluteExpirationRelativeToNow { get; init; }
}