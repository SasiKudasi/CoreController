using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace AgentClient;

public static class NotificationManager
{
    public static void ShowToastNotification(string title, string message)
    {
        try
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText(title)
                .AddText(message)
                .Show();
        
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка показа уведомления: {ex.Message}");
        }
    }
}
