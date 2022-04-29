using BinaryCalculator.Application.ViewModels;
using BinaryCalculator.Converters;
using BinaryCalculator.Converters.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BinaryCalculator.Application.Infrastructure
{
    public class Locator
    {
        private readonly IServiceProvider _provider;

        public Locator()
        {
            var services = new ServiceCollection().AddScoped<IConverter, BinaryConverter>()
                                                  .AddScoped<CalculatorViewModel>();
            _provider = services.BuildServiceProvider();
        }

        public CalculatorViewModel CalculatorViewModel => _provider.GetService<CalculatorViewModel>() 
            ?? throw new ArgumentNullException(nameof(CalculatorViewModel));
    }
}
