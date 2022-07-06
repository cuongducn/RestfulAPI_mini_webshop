namespace API_Alluring.Models.WrapParameters
{
    public class QueryParameter
    {
        string? search;
        string? sortBy;
        decimal? from, to;
        int? page;
        bool? gender;

        public QueryParameter() { }

        public QueryParameter(string? search, string? sortBy, decimal? from, decimal? to, bool? gender, int? page = 1)
        {
            this.Search = search;
            this.SortBy = sortBy;
            this.From = from;
            this.To = to;
            this.Page = page;
            this.Gender = gender;
        }

        public string? Search { get => search; set => search = value; }
        public string? SortBy { get => sortBy; set => sortBy = value; }
        public decimal? From { get => from; set => from = value; }
        public decimal? To { get => to; set => to = value; }
        public int? Page { get => page; set => page = value; }
        public bool? Gender { get => gender; set => gender = value; }
    }
}
