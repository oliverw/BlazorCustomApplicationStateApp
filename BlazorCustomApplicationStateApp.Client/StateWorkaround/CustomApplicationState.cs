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

    private const string AccessorFuncName = "__cps";
    private const string IdPrefix = "__cps_";

    private static readonly MarkupString empty = new MarkupString();

    public static MarkupString Serialize(string id, object value)
    {
        if (value == null)
            return empty;

        var json = JsonSerializer.Serialize(value);
        var bytes = Encoding.UTF8.GetBytes(json);
        var base64 = Convert.ToBase64String(bytes);

        return new MarkupString($"<script id=\"{IdPrefix + id}\" type=\"text/template\">{base64}</script>");
    }

    public static MarkupString RenderAccessor()
    {
        return new MarkupString($"<script>{AccessorFuncName} = (id) => document.getElementById(id).innerText.trim()</script>");
    }

    public static async Task<T> DeserializeAsync<T>(string id, IJSRuntime runtime)
    {
        try
        {
            var base64 = await runtime.InvokeAsync<string>(AccessorFuncName, IdPrefix + id);

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
