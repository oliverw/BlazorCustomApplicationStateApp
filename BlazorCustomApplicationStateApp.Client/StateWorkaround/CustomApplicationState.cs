using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;

namespace BlazorCustomApplicationStateApp.Client.StateWorkaround;

public static class CustomApplicationState
{
    public static bool IsPreRender(IServiceProvider services)
    {
        return services.GetService<IIsPreRender>() != null;
    }

    public static MarkupString Serialize(string functionName, object value)
    {
        var json = JsonSerializer.Serialize(value);
        var bytes = Encoding.UTF8.GetBytes(json);
        var base64 = Convert.ToBase64String(bytes);

        return new MarkupString(functionName + " = () => \"" + base64 + "\"");
    }

    public static async Task<T> DeserializeAsync<T>(string functionName, IJSRuntime runtime)
    {
        try
        {
            var base64 = await runtime.InvokeAsync<string>(functionName);

            var bytes = Convert.FromBase64String(base64);
            var json = Encoding.UTF8.GetString(bytes);

            return JsonSerializer.Deserialize<T>(json);
        }

        catch (Exception)
        {
            return default;
        }
    }
}
