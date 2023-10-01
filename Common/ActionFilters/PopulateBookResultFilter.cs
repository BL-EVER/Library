using Common.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.ActionFilters
{
    public class PopulateBookResultFilter : IAsyncResultFilter
    {
        private readonly IPopulateBookService _populateBookService;

        public PopulateBookResultFilter(IPopulateBookService populateBookService)
        {
            _populateBookService = populateBookService;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var result = context.Result;
            if (result is ObjectResult objectResult)
            {
                var value = objectResult.Value;
                if (value is IEnumerable<object> values)
                {
                    foreach (var valueItem in values)
                    {
                        await _populateBookService.PopulateBookAsync(valueItem);
                    }
                }
                else
                {
                    await _populateBookService.PopulateBookAsync(value);
                }
            }
            await next();
        }
    } 
}
