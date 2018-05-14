using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Repository
{
    public class CategoryModel
    {
        public string Name { get; }
        public List<SubcategoryModel> Subcategories => new List<SubcategoryModel>();

        public CategoryModel(string name = null) => Name = name;
    }
}
