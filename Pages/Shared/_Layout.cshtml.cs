using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReceptHemsida.Pages.Shared
{
    public class _Layout : PageModel
    {
        private readonly ILogger<_Layout> _logger;


        public _Layout(ILogger<_Layout> logger)
        {
            _logger = logger;
        }



    }
}
