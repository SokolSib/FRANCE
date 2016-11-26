namespace ChartStat.Model.Models
{
    public class SubGroupType
    {
        public SubGroupType(int id, string subGroupName, int grpProductId)
        {
            Id = id;
            Name = subGroupName;
            GroupId = grpProductId;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int GroupId { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}