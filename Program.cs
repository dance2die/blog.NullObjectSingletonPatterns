using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using static System.Console;

namespace blog.NullObjectSingletonPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            AssertSingleton();
            ProcessPayments();
        }

        private static void ProcessPayments()
        {
            var paymentProviders = new[] {
                "Paypal", "ApplePay", "GooglePay",
                "Braintree", "PayPal", "ApplePay",
                "ApplePay", "BadPaymentProvider" };
            foreach (var paymentProvider in paymentProviders)
            {
                var paymentProcessor = PaymentStrategyFactory.Create(paymentProvider);
                WriteLine($"===== Processing with '{paymentProvider}' Provider =====");

                // There is no need to check if the payment processor is null here
                // Reduced Cyclomatic Complexity.
                var paymentStatus = paymentProcessor.Process(111);
                WriteLine($"--> {paymentStatus}");
            }
        }

        private static void AssertSingleton()
        {
            var nullProcessor1 = PaymentStrategyFactory.Create("Non Existence Payment Provider111");
            var nullProcessor2 = PaymentStrategyFactory.Create("Non Existence Payment Provider222");
            Debug.Assert(nullProcessor1 == nullProcessor2);
        }
    }

    public static class PaymentStrategyFactory
    {
        public static IPaymentStrategy Create(string paymentProvider)
        {
            switch (paymentProvider)
            {
                case "Paypal": return new PaypalPaymentStrategy();
                case "ApplePay": return new ApplePayPaymentStrategy();
                default: return NullPaymentStrategy.Instance;
            }
        }
    }

    // "NoOp" means No Operation: https://en.wikipedia.org/wiki/NOP
    // First value is assigned a value of 0 by default.
    public enum ProcessStatus { Successful, Failed, NoOp }

    public interface IPaymentStrategy
    {
        ProcessStatus Process(double amount);
    }

    public abstract class RandomPaymentStrategy : IPaymentStrategy
    {
        public ProcessStatus Process(double amount)
        {
            var random = new Random();
            // Random number between 0 & 1 for either "Successful" or "Failed" status.
            return (ProcessStatus)random.Next(0, 2);
        }
    }

    public class PaypalPaymentStrategy : RandomPaymentStrategy { }

    public class ApplePayPaymentStrategy : RandomPaymentStrategy { }

    public class NullPaymentStrategy : IPaymentStrategy
    {
        // .NET optimized version of Singleton
        // Reference: http://www.dofactory.com/net/singleton-design-pattern
        public static readonly IPaymentStrategy Instance = new NullPaymentStrategy();
        private NullPaymentStrategy() { }

        public ProcessStatus Process(double amount)
        {
            return ProcessStatus.NoOp;
        }
    }
}
