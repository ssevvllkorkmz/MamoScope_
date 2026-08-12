using MaMoScope_.Controls;
using MaMoScope_.Core.Interface;
using MaMoScope_.Factories;
using MaMoScope_.Services;
using MaMoScope_.ViewModels;
using MaMoScope_.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Http;



namespace MaMoScope_
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var servicollection = new ServiceCollection();
            ConfigureServices(servicollection);
            ServiceProvider = servicollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpClient<IMotorDriversService, MotorDriversService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5132/");
            });

            
            services.AddSingleton<MotorDriversStore>();
            services.AddSingleton<INavigationService, NavigationService>();
            


            services.AddTransient<PastRecordsViewModel>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<TestRecordsViewModel>();

            services.AddTransient<MainWindowView>();
            services.AddTransient<PastRecordsView>();
            services.AddTransient<TestRecordsView>();

            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddTransient<RecordDetailsViewModel>();
            services.AddTransient<RecordDetailsView>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<NavbarViewModel>();
            services.AddTransient<NavbarView>();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var navService = ServiceProvider.GetRequiredService<INavigationService>();
            var mainVM = ServiceProvider.GetRequiredService<MainWindowViewModel>();

            navService.NavigateTo<TestRecordsViewModel>();

            var mainLayout = ServiceProvider.GetRequiredService<MainWindowView>();


            mainLayout.DataContext = mainVM;
            mainLayout.Show();

            _ = Task.Run(async () =>
            {
                var service = ServiceProvider.GetRequiredService<IMotorDriversService>();
                var store = ServiceProvider.GetRequiredService<MotorDriversStore>();

                var allRecords = await service.GetPastRecordAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {

                    foreach (var record in allRecords)
                        store.Records.Add(record);

                });
            });
        }
    }


}
