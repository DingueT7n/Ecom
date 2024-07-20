
namespace Ecom.Core.Sharing
{
    public class ProductParams
    {
        //string sort, int? categoryid, int pagenumber, int pagesize
        
        public int MaxPageSize { get; set; } = 15;
        
        private int _pagesize =6;
        public int PageSiz
        {
            get { return _pagesize; }
            set { this._pagesize = value>MaxPageSize? MaxPageSize:value; }
        }

        public int Pagenumber { get; set; } = 1;
        public int? CategoryId { get; set; }
        public string Sort { get; set; }
        private string _search;
            
        public string Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }


    }
}
