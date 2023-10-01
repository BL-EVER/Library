using AutoMapper;
using Common.DTOs;
using Common.Repository;
using Newtonsoft.Json;


namespace Common.Service
{
    public class GenericService<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey>
    : IGenericService<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey> where TEntity : class
    {
        private readonly IGenericRepository<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey> _repository;
        private readonly ICacheRepository _cache;
        private readonly IMapper _mapper;
        private readonly string CACHE_KEY_PREFIX = $"{typeof(TEntity).Name}";

        public GenericService(IGenericRepository<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey> repository, ICacheRepository cache, IMapper mapper)
        {
            _repository = repository;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<TReadDto>> GetAll(int page, int pageSize, TQueryParamsDto queryParams)
        {
            var cacheKey = $"{CACHE_KEY_PREFIX}-GetAll-{page}-{pageSize}-{queryParams.ToString()}";

            if (_cache.isCacheAvailable())
            {
                var cacheResponse = await _cache.GetCacheValueAsync(cacheKey);

                if (!string.IsNullOrEmpty(cacheResponse))
                {
                    return JsonConvert.DeserializeObject<PaginatedResult<TReadDto>>(cacheResponse);
                }
            }

            var response = await _repository.GetAll(page, pageSize, queryParams);

            if (_cache.isCacheAvailable())
            {
                await _cache.SetCacheValueAsync(cacheKey, JsonConvert.SerializeObject(response));
            }
                
            return response;
        }

        public async Task<TReadDto> Get(TKey id)
        {
            var cacheKey = $"{CACHE_KEY_PREFIX}-Get-{id}";

            if (_cache.isCacheAvailable())
            {
                var cacheResponse = await _cache.GetCacheValueAsync(cacheKey);

                if (!string.IsNullOrEmpty(cacheResponse))
                {
                    return JsonConvert.DeserializeObject<TReadDto>(cacheResponse);
                }
            }

            var response = await _repository.Get(id);
            if (_cache.isCacheAvailable())
            {
                await _cache.SetCacheValueAsync(cacheKey, JsonConvert.SerializeObject(response));
            }

            return response;
        }

        public async Task<TReadDto> Create(TCreateDto createDto)
        {
            var response = await _repository.Create(createDto);
            await RemoveAllCache();
            return response;
        }

        public async Task<TReadDto> UpdatePartial(TKey id, TEditPartialDto editPartialDto)
        {
            var response = await _repository.UpdatePartial(id, editPartialDto);
            await RemoveAllCache();
            return response;
        }
        
        public async Task<TReadDto> Update(TKey id, TEditDto editDto)
        {
            var response = await _repository.Update(id, editDto);
            await RemoveAllCache();
            return response;
        }

        public async Task Delete(TKey id)
        {
            await _repository.Delete(id);
            await RemoveAllCache();
        }

        private async Task RemoveAllCache()
        {
            if (!_cache.isCacheAvailable()) return;

            var cacheKeys = new List<string>
                {
                    $"{CACHE_KEY_PREFIX}-Get-",
                    $"{CACHE_KEY_PREFIX}-GetAll-"
                };

            foreach (var baseKey in cacheKeys)
            {
                await _cache.RemoveByPrefixAsync(baseKey);
            }
        }
    }
}
