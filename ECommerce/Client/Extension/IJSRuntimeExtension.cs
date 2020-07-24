using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Client.Extension
{
    public static class IJSRuntimeExtension
    {
        public static void SetBlazorCulture(this IJSRuntime jsRuntime, CultureInfo culture)
        {
            var js = (IJSInProcessRuntime)jsRuntime;
            js.InvokeVoid("blazorCulture.set", culture.Name);
        }

        public static ValueTask<string> GetBlazorCulture(this IJSRuntime jsRuntime)
        {
            return jsRuntime.InvokeAsync<string>("blazorCulture.get");
        }
    }
}
