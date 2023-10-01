using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service
{
    public interface IGenericService<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey> where TEntity : class
    {
        Task<PaginatedResult<TReadDto>> GetAll(int page, int pageSize, TQueryParamsDto queryParams);
        Task<TReadDto> Get(TKey id);
        Task<TReadDto> Create(TCreateDto createDto);
        Task<TReadDto> UpdatePartial(TKey id, TEditPartialDto editPartialDto);
        Task<TReadDto> Update(TKey id, TEditDto editDto);
        Task Delete(TKey id);
    }
}
