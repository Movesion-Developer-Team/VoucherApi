using System.Runtime.CompilerServices;
using Core;
using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
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
        public IDiscountTypeRepository DiscountType { get; }
        public ISystemUpdateRepository SystemUpdate { get; }

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
            DiscountType = new DiscountTypeRepository(voucherContext);
            SystemUpdate = new SystemUpdateRepository(voucherContext);

            var lastUpdate = SystemUpdate.GetLastUpdate();
            var updateLimit = new TimeSpan(0, 30, 0);
            var awaiter1 = Discount.GetNumberOfActiveReservations().GetAwaiter();
            var activeReservationsCount = awaiter1.GetResult();

            if (lastUpdate == null)
            {

                var awaiter2 = Discount.Refresh().GetAwaiter();
                var result = awaiter2.GetResult() ?? 0;
                voucherContext.SystemUpdates.Add(new SystemUpdate
                {
                    RefreshedCodesQuantity = result,
                    ActiveReservations = activeReservationsCount
                });
                voucherContext.SaveChanges();
            }

            if (lastUpdate!= null && (DateTimeOffset.UtcNow.DateTime - lastUpdate.Value.UtcDateTime).TotalSeconds > updateLimit.TotalSeconds)
            {
                var awaiter = Discount.Refresh().GetAwaiter();
                var result = awaiter.GetResult() ?? 0;
                voucherContext.SystemUpdates.Add(new SystemUpdate
                {
                    RefreshedCodesQuantity = result,
                    ActiveReservations = activeReservationsCount
                });
                voucherContext.SaveChanges();
            }

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
