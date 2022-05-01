using BinaryCalculator.Application.Calculators;
using BinaryCalculator.Application.Calculators.Interfaces;
using BinaryCalculator.Application.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BinaryCalculator.Application.Infrastructure
{
    public class Locator
    {
        private readonly IServiceProvider _provider;

        public Locator()
        {
            var services = new ServiceCollection().AddScoped<ICalculator, Calculators.BinaryCalculator>()
                                                  .AddScoped<CalculatorViewModel>();
            _provider = services.BuildServiceProvider();
        }

        public CalculatorViewModel CalculatorViewModel => _provider.GetService<CalculatorViewModel>() 
            ?? throw new ArgumentNullException(nameof(CalculatorViewModel));
    }
}
