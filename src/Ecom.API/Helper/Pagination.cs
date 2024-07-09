namespace Ecom.API.Helper
{
    public class Pagination<T>
    {
        public Pagination(int pageNumber, int pageSize, int count, IEnumerable<T> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
        

    }
}
