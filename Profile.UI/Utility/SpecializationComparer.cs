using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.UI.Utility
{
    public class SpecializationComparer : IComparer<Specialization>
    {
        private List<string> specializations;

        public SpecializationComparer()
        {
            specializations = new List<string>(8);

            specializations.Add(Specialization.BA);
            specializations.Add(Specialization.Design);
            specializations.Add(Specialization.Frontend);
            specializations.Add(Specialization.BackendNET);
            specializations.Add(Specialization.BackendAndroid);
            specializations.Add(Specialization.BackendJava);
            specializations.Add(Specialization.BackendPHP);
            specializations.Add(Specialization.QA);
        }

        public int Compare(Specialization x, Specialization y)
        {
            int indexX = specializations.IndexOf(x.Abbreviation);
            int indexY = specializations.IndexOf(y.Abbreviation);

            return indexX.CompareTo(indexY);
        }
    }
}