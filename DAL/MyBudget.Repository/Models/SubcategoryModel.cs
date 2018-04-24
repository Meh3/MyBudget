using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Repository
{
    public class SubcategoryModel
    {
        public CategoryModel Parent { get; private set; }
        public string Name { get; private set; }

        public SubcategoryModel(string name, CategoryModel parent)
        {
            Name = name;
            Parent = parent;
        }
    }
}
