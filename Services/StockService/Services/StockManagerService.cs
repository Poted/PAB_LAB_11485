using Grpc.Core;
namespace StockService.Services;

internal class StockManagerService : Stock.StockBase
{
    public override Task<GetStockReply> GetStock(GetStockRequest request, ServerCallContext context)
    {
        var stockQuantity = new Random().Next(0, 100);
        return Task.FromResult(new GetStockReply
        {
            QuantityOnHand = stockQuantity
        });
    }
}