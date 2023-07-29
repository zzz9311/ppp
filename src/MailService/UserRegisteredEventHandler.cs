using Rebus.Handlers;
using ServiceContracts;

namespace MailService;

public class UserRegisteredEventHandler : IHandleMessages<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent message)
    {
        Console.WriteLine($"Через брокера пришло сообщение = {message.UserName} в {DateTime.Now}");
    }
}