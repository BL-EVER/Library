using Common.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey> : ControllerBase where TEntity : class
    {
        private readonly IGenericService<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto,  TKey> _service;

        public GenericController(IGenericService<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<TReadDto>> GetAll([FromQuery] TQueryParamsDto queryParamsDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var paginatedResult = await _service.GetAll(page, pageSize, queryParamsDto);

            HttpContext.Response.Headers.Add("X-TOTAL-COUNT", paginatedResult.TotalRecords.ToString());
            HttpContext.Response.Headers.Add("X-CURRENT-PAGE", paginatedResult.Page.ToString());
            HttpContext.Response.Headers.Add("X-PAGE-SIZE", paginatedResult.PageSize.ToString());

            return Ok(paginatedResult.Data);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TReadDto>> Get(TKey id)
        {
            return Ok(await _service.Get(id));
        }

        [HttpPost]
        [Authorize]
        [AuthorizeAndPopulateOwnedResource]
        public virtual async Task<IActionResult> Create(TCreateDto createDto)
        {
            var readDto = await _service.Create(createDto);

            return new ObjectResult(readDto) { StatusCode = StatusCodes.Status201Created };

        }

        [HttpPut("{id}")]
        [Authorize]
        [AuthorizeAndPopulateOwnedResource]
        
        public virtual async Task<IActionResult> Update(TKey id, TEditDto editDto)
        {
            if (!EnsureOwnerAuthorization.Authorize(await _service.Get(id), User)) return Unauthorized();
            return Ok(await _service.Update(id, editDto));
        }

        [HttpPatch("{id}")]
        [Authorize]
        [AuthorizeAndPopulateOwnedResource]
        public virtual async Task<IActionResult> UpdatePartial(TKey id, TEditPartialDto editPartialDto)
        {
            if (!EnsureOwnerAuthorization.Authorize(await _service.Get(id), User)) return Unauthorized();
            return Ok(await _service.UpdatePartial(id, editPartialDto));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            if (!EnsureOwnerAuthorization.Authorize(await _service.Get(id), User)) return Unauthorized();
            await _service.Delete(id);
            return NoContent();
        }
    }
}
