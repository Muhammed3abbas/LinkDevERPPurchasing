using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing.Domain.Helper
{
    public static class CodeGenerator
    {
        private static readonly Random Random = new Random();

        public static string GeneratePurchaseOrderCode() => $"PO{Random.Next(1000000, 10000000)}";
        public static string GeneratePurchaseOrderItemCode() => $"POI{Random.Next(1000000, 10000000)}";
    }
}
