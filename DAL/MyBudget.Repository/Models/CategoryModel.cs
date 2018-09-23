using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using DotNetStandardExtensions.ErrorsCheck;

namespace MyBudget.Repository
{
    public class CategoryModel
    {
        public CategoryModel Parent { get; private set; }
        public string Name { get; }
        public ImmutableArray<CategoryModel> Subcategories { get; private set; } = new ImmutableArray<CategoryModel>();

        public CategoryModel(string name) => Name = name.ThrowIfNullOrEmpty(nameof(name));

        public CategoryModel(string name, IEnumerable<CategoryModel> subcategories)
        {
            Name = name.ThrowIfNullOrEmpty(nameof(name));
            foreach (var subCat in subcategories.ThrowIfNull(nameof(subcategories)))
            {
                subCat.Parent = this;
            }
            Subcategories = subcategories.ToImmutableArray();
        }
    }
}
