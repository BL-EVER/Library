using Common.DTOs;
using Common.Attributes;

namespace LibraryCatalogService.DTOs
{
    public class CreateBookDTO
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int? CategoryId { get; set; }
    }

    public class EditBookDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int? CategoryId { get; set; }
    }

    public class EditPartialBookDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
        public int? CategoryId { get; set; }
    }

    public class QueryParamsBookDTO
    {
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public int? Stock { get; set; } = null;

        public override string? ToString()
        {
            return $"{Title}-{Description}-{Stock}";
        }
    }

    public class ReadBookDTO : ReadTimeStampBaseDTO
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }

        [IncludeProperty]
        public ReadPartialCategoryDTO? Category { get; set; }
    }

    public class ReadPartialBookDTO : ReadTimeStampBaseDTO
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }
}
