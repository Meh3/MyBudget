using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Repository
{
    public class SubcategoryModel
    {
        public CategoryModel Parent { get; }
        public string Name { get; }

        public SubcategoryModel(string name = null, CategoryModel parent = null)
        {
            Name = name;
            Parent = parent;
        }
    }
}
