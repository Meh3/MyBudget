using System;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Ninject;
using MyBudget.UI.Views;

namespace MyBudget.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            SetLocaleFromConfigIfExist("locale");
            SetNinjectContainer();
        }

        private void SetLocaleFromConfigIfExist(string setttingName)
        {
            var localeFromConfig = ConfigurationManager.AppSettings[setttingName]?.ToString();

            if (!string.IsNullOrEmpty(localeFromConfig))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(localeFromConfig); ;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(localeFromConfig); ;
            }

        }

        private void SetNinjectContainer()
        {
            NinjectKernel.Initialize(Assembly.GetExecutingAssembly());

            var mainWindw = NinjectKernel.Get<MainWindow>();
            Application.Current.MainWindow = mainWindw;
            Application.Current.MainWindow.Show();
        }
    }
}
