using Common.Attributes;
using Common.DTOs;

namespace LibraryCatalogService.DTOs
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
    }
    public class EditCategoryDTO
    {
        public string Name { get; set; }
    }
    public class EditPartialCategoryDTO
    {
        public string? Name { get; set; }
    }
    public class QueryParamsCategoryDTO
    {
        public string? Name { get; set; } = null;
        public override string? ToString()
        {
            return Name;
        }
    }
    public class ReadCategoryDTO : ReadTimeStampBaseDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        [IncludeProperty]
        public ICollection<ReadPartialBookDTO>? Books { get; set; }
    }
    public class ReadPartialCategoryDTO : ReadTimeStampBaseDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }

}
