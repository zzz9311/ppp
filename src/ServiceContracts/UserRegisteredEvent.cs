namespace ServiceContracts;

public class UserRegisteredEvent
{
    public UserRegisteredEvent(string userName)
    {
        UserName = userName;
    }
    
    public string UserName { get; }
}