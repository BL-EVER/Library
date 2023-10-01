namespace Common.DTOs
{
    public class CreateOwnedResourceDTO
    {
        public string? Owner { get; private set; }

        public void SetOwner(string owner)
        {
            this.Owner = owner;
        }
    }
    public class EditOwnedResourceDTO
    {
        public string? Owner { get; private set; }

        public void SetOwner(string owner)
        {
            this.Owner = owner;
        }
    }
    public class ReadOwnedResourceDTO : ReadTimeStampBaseDTO
    {
        public string Owner { get; set; }
    }
    public class QueryParamsOwnedResourceDTO
    {
        public string? Owner { get; set; } = null;
    }
}
