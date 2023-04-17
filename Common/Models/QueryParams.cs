namespace MageloRankings.Models
{
    public class QueryParams
    {
        public int Take = 50;
        public List<Filter> Filters { get; set; }
        public FieldEnum SortField { get; set; }
        public SortOrderEnum SortOrder { get; set; }

        public QueryParams()
        {
            SortField = FieldEnum.hp_max_total;
            SortOrder = SortOrderEnum.Descending;
            Filters = new List<Filter>();
        }
    }

    public class Filter
    {
        public FieldEnum Field { get; set; }
        public OperatorEnum Operator { get; set; }
        public string Value { get; set; }
        public Filter() 
        {
            Value = "";
        }
    }

    public enum OperatorEnum
    {
        Equal = 1,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }

    public enum SortOrderEnum { 
        Ascending = 1, 
        Descending
    }

    public enum FieldEnum
    {
        @class = 1,
        hp_max_total,
        mana_max_total,
        avg_resists,
        guild_name,
        level,
        race,
        ac_total,
        atk_total,
        mana_regen_item,
        hp_regen_item
    }
}
