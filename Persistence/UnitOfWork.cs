using Core;
using Core.IRepositories;
using Persistence.Repositories;

namespace Persistence
{
    public class UnitOfWork : IUnityOfWork
    {


        public ICompanyRepository Company { get; }
        public ICategoryRepository Category { get; }
        public IDiscountRepository Discount { get; }
        public ILocationRepository Location { get; }
        public IPlayerRepository Player { get; }
        public IReportRepository Report { get; }
        public IDiscountCodeRepository DiscountCode { get; }
        public IUserRepository User { get; }
        public IInvitationCodeRepository InvitationCode { get; }
        public IPurchaseRepository Purchase { get; }

        private readonly VoucherContext _voucherContext;


        public UnitOfWork(VoucherContext voucherContext)
        {
            _voucherContext = voucherContext;
            Company = new CompanyRepository(voucherContext);
            Category = new CategoryRepository(voucherContext);
            Discount = new DiscountRepository(voucherContext);
            Location = new LocationRepository(voucherContext);
            Player = new PlayerRepository(voucherContext);
            Report = new ReportRepository(voucherContext);
            DiscountCode = new DiscountCodeRepository(voucherContext);
            User = new UserRepository(voucherContext);
            InvitationCode  = new InvitationCodeRepository(voucherContext);
            Purchase = new PurchaseRepository(voucherContext);
        }




        public void Dispose()
        {
            GC.SuppressFinalize(this);

        }

        public async Task Complete()
        {
            await _voucherContext.SaveChangesAsync();
        }
    }
}
