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

    public interface IOrderRepository
    {
        IOrder GetOrderById(int id);
    }

    public class OrderRepository : IOrderRepository
    {
        private static List<Order> Orders = new List<Order>{
            new Order(id: 1, quantity: 40, orderDate: new DateTime(2018, 1,1,1,1,1,1)),
            new Order(id: 2, quantity: 20, orderDate: new DateTime(2018, 2,2,2,2,2,2)),
            new Order(id: 3, quantity: 30, orderDate: new DateTime(2018, 3,3,3,3,3,3)),
            new Order(id: 4, quantity: 10, orderDate: new DateTime(2018, 4,4,4,4,4,4)),
            new Order(id: 5, quantity: 20, orderDate: new DateTime(2018, 5,5,5,5,5,5)),
        };

        public IOrder GetOrderById(int id)
        {
            // You might search database for an Order here.
            return Orders.FirstOrDefault(order => order.Id == id) ?? NullOrder.Instance;
        }
    }

    public interface IOrder
    {
        int Id { get; }
        int Quantity { get; }
        DateTime OrderDate { get; }
    }

    public class Order : IOrder
    {
        public int Id { get; }
        public int Quantity { get; }
        public DateTime OrderDate { get; }

        public Order(int id, int quantity, DateTime orderDate)
        {
            Id = id;
            Quantity = quantity;
            OrderDate = orderDate;
        }

        public override string ToString()
        {
            return $"Order ID: {Id}, Quantity: {Quantity}, Order Date: {OrderDate.ToString("dd MMM yyyy hh:mm tt p\\s\\t", CultureInfo.InvariantCulture)}";
        }
    }

    public class NullOrder : IOrder
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <remark>
        /// Reference: .NET Optimized version in <see ref="http://www.dofactory.com/net/singleton-design-pattern">Do Factory</see>
        /// </remark>
        public static readonly IOrder Instance { get; } = new NullOrder();

        private NullOrder() { }

        public int Id => throw new NotImplementedException();

        public int Quantity => throw new NotImplementedException();

        public DateTime OrderDate => throw new NotImplementedException();
    }
}
