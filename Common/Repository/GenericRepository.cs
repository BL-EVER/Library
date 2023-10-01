using AutoMapper;
using Common.Attributes;
using Common.DTOs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Repository
{
    public class GenericRepository<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey>
        : IGenericRepository<TEntity, TCreateDto, TReadDto, TEditDto, TEditPartialDto, TQueryParamsDto, TKey>
        where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        // Add your mapping logic here
        private readonly IMapper _mapper;

        public GenericRepository(DbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _mapper = mapper;
        }

        /*public virtual async Task<PaginatedResult<TReadDto>> GetAll(int page, int pageSize)
        {
            // Find properties with [IncludeProperty] in TReadDto
            var includeProperties = typeof(TReadDto)
                .GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(IncludePropertyAttribute)) != null)
                .Select(p => p.Name)
                .ToList();

            // Build the query with includes
            var query = _dbSet.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var totalRecords = await query.CountAsync();
            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var mappedData = _mapper.Map<List<TReadDto>>(data);

            return new PaginatedResult<TReadDto>
            {
                Data = mappedData,
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize
            };
        }*/
        //TODO: Debug Query not properly returning values
        public virtual async Task<PaginatedResult<TReadDto>> GetAll(int page, int pageSize, TQueryParamsDto queryParams)
        {
            // Find properties with [IncludeProperty] in TReadDto
            var includeProperties = typeof(TReadDto)
                .GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(IncludePropertyAttribute)) != null)
                .Select(p => p.Name)
                .ToList();

            // Build the query with includes
            var query = _dbSet.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            // Dynamic query building based on QueryParamsDTO
            var paramProperties = typeof(TQueryParamsDto).GetProperties();
            foreach (var prop in paramProperties)
            {
                var value = prop.GetValue(queryParams);
                if (value != null)
                {
                    var parameterExp = Expression.Parameter(typeof(TEntity), "type");
                    var propertyExp = Expression.Property(parameterExp, prop.Name);
                    var constantExp = Expression.Constant(value);
                    var equalsExp = Expression.Equal(propertyExp, constantExp);

                    var lambda = Expression.Lambda<Func<TEntity, bool>>(equalsExp, parameterExp);
                    query = query.Where(lambda);
                }
            }

            // Get primary key name using EF Core
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var primaryKeyProp = entityType.FindPrimaryKey().Properties.Single().Name;

            var totalRecords = await query.CountAsync();
            List<TEntity> data;
            if(page == 0 && pageSize == 0)
            {
                data = await query.OrderBy(e => EF.Property<TKey>(e, primaryKeyProp)).ToListAsync();
            }
            else
            {
                data = await query.OrderBy(e => EF.Property<TKey>(e, primaryKeyProp)).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            var mappedData = _mapper.Map<List<TReadDto>>(data);

            return new PaginatedResult<TReadDto>
            {
                Data = mappedData,
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize
            };
        }
        public virtual async Task<TReadDto> Get(TKey id)
        {
            // Get primary key name using EF Core
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var primaryKeyProp = entityType.FindPrimaryKey().Properties.Single().Name;

            // Find properties with [IncludeProperty] in TReadDto
            var includeProperties = typeof(TReadDto)
                .GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(IncludePropertyAttribute)) != null)
                .Select(p => p.Name)
                .ToList();

            var query = _dbSet.AsQueryable();

            // Add includes to query
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, primaryKeyProp).Equals(id));

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            return _mapper.Map<TReadDto>(entity);
        }

        public virtual async Task<TReadDto> Create(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);

            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TReadDto>(entity);
        }



        public virtual async Task<TReadDto> UpdatePartial(TKey id, TEditPartialDto editPartialDto)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            _mapper.Map(editPartialDto, entity);

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TReadDto>(entity);
        }
        public virtual async Task<TReadDto> Update(TKey id, TEditDto editDto)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            // First, remove the current entity from the context so it won't be tracked anymore.
            _context.Entry(entity).State = EntityState.Detached;

            // Then, map the editDto to a new entity.
            var newEntity = _mapper.Map<TEntity>(editDto);

            // Find the name of the primary key property
            var keyName = _context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name).Single();

            // Update the primary key to the id of the entity being updated.
            _context.Entry(newEntity).Property(keyName).CurrentValue = id;

            // Now, attach the new entity to the context and set its state to Modified.
            _context.Attach(newEntity);
            _context.Entry(newEntity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return _mapper.Map<TReadDto>(newEntity);
        }



        public virtual async Task Delete(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
