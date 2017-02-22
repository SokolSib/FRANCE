using ChartStat.Model.Models;

namespace ChartStat.Controls.Filters.GroupsControl
{
    public class GroupTypeModel
    {
        public GroupType Group { get; private set; }

        public GroupTypeModel(GroupType group)
        {
            Group = group;
            group.SubGroups.Insert(0, new SubGroupType(-1, "Все подгруппы", group.Id));
        }

        public override string ToString()
        {
            return Group.ToString();
        }
    }
}
