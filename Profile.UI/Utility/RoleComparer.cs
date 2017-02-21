using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.UI.Utility
{
    public class RoleComparer : IComparer<RoleType>
    {
        private const int DefaultRank = 0;

        private readonly Dictionary<RoleType, int> _rankedRoles;

        /// <summary>
        /// The constructor need a dictionary that is used for sorting roles by their ranks
        /// </summary>
        /// <param name="rankedRoles">
        /// The dictionary value contains role rank that describes order of role which is kept into key
        /// </param>
        public RoleComparer(Dictionary<RoleType, int> rankedRoles)
        {
            _rankedRoles = rankedRoles;
        }

        public int Compare(RoleType x, RoleType y)
        {
            int rankX = _rankedRoles.ContainsKey(x) ? _rankedRoles[x] : DefaultRank;
            int rankY = _rankedRoles.ContainsKey(y) ? _rankedRoles[y] : DefaultRank;

            return rankX.CompareTo(rankY);
        }
    }
}