using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class HttpClientPolicies
    {
        public static void AddCustomHttpClientPolicies(this IServiceCollection services)
        {
            var policyRegistry = services.AddPolicyRegistry();

            policyRegistry.Add("Regular", Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)));
            policyRegistry.Add("Long", Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30)));

            policyRegistry.Add("CircuitBreaker", HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            //Opens the circuit afet 5 consecutive transient HTTP errors and keeps it open for 30 seconds


            services.AddHttpClient("PollyRegistryRegular")
                .AddPolicyHandlerFromRegistry("Regular");

            services.AddHttpClient("PollyRegistryLong")
                .AddPolicyHandlerFromRegistry("Long");

            services.AddHttpClient("PollyRegistryCircuitBreaker")
                .AddPolicyHandlerFromRegistry("CircuitBreaker");
        }
    }
}
