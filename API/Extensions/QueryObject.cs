namespace API.Extensions
{
    public class QueryObject
    {
        public string? SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        private int _page;
        public int Page
        {
            get { return _page; }
            set
            {
                if (value <= 0)
                    _page = 1;
                else
                    _page = value;
            }
        }

        private byte _pageSize;
        public byte PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value <= 0)
                    _pageSize = 10;
                else
                    _pageSize = value;
            }
        }
    }
}