using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyBudget.UI.Common
{
    class DoubleToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var doubleValue = System.Convert.ToDouble(value);
            var sideValues = new Dictionary<Direction?, double>
            {
                { Direction.Left, doubleValue},
                { Direction.Up, 0},
                { Direction.Right, 0},
                { Direction.Down, 0}
            };

            var side = parameter as Direction?;
            if (sideValues.ContainsKey(side))
            {
                sideValues[side] = doubleValue;
            }

            return new Thickness(sideValues[Direction.Left], sideValues[Direction.Up], sideValues[Direction.Right], sideValues[Direction.Down]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
