using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using ChartStat.Model.Models;

namespace ChartStat.Model.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GroupRepozitory : RepositoryBase<GroupType>
    {
        public GroupRepozitory(IDbConnection connection)
            : base(connection)
        {
        }

        public override ICollection<GroupType> GetAllEntities()
        {
            var rows = base.GetAllEntities();

            var result = new Collection<GroupType>();
            foreach (var row in rows)
            {
                if (result.Count > 0 && result[result.Count - 1].Id == row.Id)
                    result[result.Count - 1].SubGroups.Add(row.SubGroups[0]);
                else
                    result.Add(row);
            }

            return result;
        }

        protected override string GetCommandText()
        {
            return @"SELECT   
      g.Id AS groupId
      ,g.GroupName
      ,s.Id
      ,s.SubGroupName
  FROM dbo.SubGrpNameSet AS s 
  JOIN dbo.GrpProductSet AS g ON s.GrpProductId = g.Id
  ORDER BY g.GroupName";
        }
    }
}
