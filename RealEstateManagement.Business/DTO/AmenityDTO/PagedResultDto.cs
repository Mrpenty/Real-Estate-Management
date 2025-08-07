namespace RealEstateManagement.Business.DTO.AmenityDTO
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
