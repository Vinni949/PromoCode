namespace PromoCode.Models
{
    /// <summary>
    /// Постраничная выгрузка
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T>: List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public PagedList(int currentPage, int totalItems,List<T> items,int pageSize)
        {
            CurrentPage = currentPage;
            this.AddRange(items);
            TotalCount = (int)(Math.Ceiling(totalItems / (double)(pageSize)));

        }

        public bool HasNextPage() => CurrentPage < TotalCount;
        public bool HasPreviousPage() => CurrentPage > 0;
    }
}
