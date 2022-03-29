using Core.IRepositories;

namespace Core
{
    public interface IUnityOfWork : IDisposable
    {
        IAgencyRepository Agency { get; }
        ICategoryRepository Category { get; }
        IDiscountRepository Discount { get; }
        ILocationRepository Location { get; }
        IPlayerRepository Player { get; }
        IReportRepository Report { get; }
        IVoucherRepository Voucher { get; }

        Task Complete();

    }
}
