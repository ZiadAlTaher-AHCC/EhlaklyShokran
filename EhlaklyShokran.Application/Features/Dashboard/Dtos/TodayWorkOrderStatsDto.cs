using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Features.Dashboard.Dtos
{
    public sealed class TodayWorkOrderStatsDto
    {
        public DateOnly Date { get; init; }
        public int Total { get; init; }
        public int Scheduled { get; init; }
        public int InProgress { get; init; }
        public int Completed { get; init; }
        public int Cancelled { get; init; }
        public decimal TotalRevenue { get; init; }
        public decimal TotalCosmeticsCost { get; init; }
        public decimal TotalLaborCost { get; init; }
        public int UniqueCustomers { get; init; }
        public decimal NetProfit { get; init; }
        public decimal ProfitMargin { get; init; }
        public decimal CompletionRate { get; init; }
        public decimal AverageRevenuePerOrder { get; init; }
        public decimal OrdersPerCustomer { get; init; }
        public decimal CosmeticsCostRatio { get; init; }
        public decimal LaborCostRatio { get; init; }
        public decimal CancellationRate { get; init; }
    }
}
