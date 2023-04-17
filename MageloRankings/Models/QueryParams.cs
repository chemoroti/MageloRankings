namespace MageloRankings.Models
{
    public class QueryParams
    {
        public int Take = 5;
        public List<Filter> Filters { get; set; }
        public FieldEnum SortField { get; set; }
        public SortOrderEnum SortOrder { get; set; }

        public QueryParams()
        {
            Filters = new List<Filter>();
        }
    }

    public class Filter
    {
        public FieldEnum Field { get; set; }
        public OperatorEnum Operator { get; set; }
        public string Value { get; set; }
    }

    public enum OperatorEnum
    {
        Equal,
        NotEqual,
        LessThan,
        GreaterThan
    }

    public enum SortOrderEnum { 
        Ascending, 
        Descending
    }

    public enum FieldEnum
    {
        Clss,
        HP,
        Mana
    }
}
