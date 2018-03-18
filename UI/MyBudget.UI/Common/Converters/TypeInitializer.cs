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
    public class TypeInit : MarkupExtension
    {
        private readonly Type type;

        public TypeInit(Type type) => this.type = type;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return type != null ? NinjectKernel.Get(type) : null;
        }
    }
}
