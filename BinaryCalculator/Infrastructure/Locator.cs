using BinaryCalculator.Application.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BinaryCalculator.Application.Infrastructure
{
    public class Locator
    {
        private readonly IServiceCollection _services;

        public Locator()
        {
            _services = new ServiceCollection().AddScoped<CalculatorViewModel>();
        }

        public CalculatorViewModel CalculatorViewModel => _services.BuildServiceProvider().GetService<CalculatorViewModel>() 
            ?? throw new ArgumentNullException(nameof(CalculatorViewModel));
    }
}
