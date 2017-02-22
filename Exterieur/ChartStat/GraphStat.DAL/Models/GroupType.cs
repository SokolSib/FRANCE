using System.Collections.ObjectModel;

namespace ChartStat.Model.Models
{
    public class GroupType
    {
        public GroupType(int groupId, string groupName, int id, string subGroupName)
        {
            Id = groupId;
            Name = groupName;
            SubGroups = new Collection<SubGroupType> {new SubGroupType(id, subGroupName, groupId)};
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public Collection<SubGroupType> SubGroups { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}