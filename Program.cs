using System;
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
            WriteLine("Hello World!");
        }
    }

    public static class PaymentStrategyFactory
    {
        public static IPaymentStrategy Create(string paymentProviderName)
        {
            switch (paymentProviderName)
            {
                case "Paypal": return new PaypalPaymentStrategy();
                case "ApplePay": return new ApplePayPaymentStrategy();
                default: return NullPaymentStrategy.Instance;
            }
        }
    }

    // "NoOp" means No Operation: https://en.wikipedia.org/wiki/NOP
    public enum ProcessStatus { Successful, Failed, NoOp }

    public interface IPaymentStrategy
    {
        ProcessStatus ProcessPayment(double amount);
    }

    public abstract class RandomPaymentStrategy : IPaymentStrategy
    {
        public ProcessStatus ProcessPayment(double amount)
        {
            var random = new Random();
            return (ProcessStatus)random.Next(0, 1);
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

        public ProcessStatus ProcessPayment(double amount)
        {
            return ProcessStatus.NoOp;
        }
    }

}
