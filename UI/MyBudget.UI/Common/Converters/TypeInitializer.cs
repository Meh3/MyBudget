using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MyBudget.UI;
using System.Windows.Markup;

namespace MyBudget.UI.Common
{
    public class TypeInitializer : MarkupExtension
    {
        private Type type;

        public TypeInitializer(Type type) => this.type = type;

        public override object ProvideValue(IServiceProvider serviceProvider) =>
            type != null ? NinjectKernel.Get(type) : null;
    }
}
