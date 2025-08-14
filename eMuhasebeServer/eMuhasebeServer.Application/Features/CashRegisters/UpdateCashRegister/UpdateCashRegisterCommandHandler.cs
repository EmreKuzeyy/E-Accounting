using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.UpdateCashRegister
{
    internal sealed class UpdateCashRegisterCommandHandler(
        ICashRegisterRepository cashRegisterRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper,
        ICacheService cacheService) : IRequestHandler<UpdateCashRegisterCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCashRegisterCommand request, CancellationToken cancellationToken)
        {
            CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(u=> u.Id == request.Id, cancellationToken);
            if (cashRegister == null)
            {
                return Result<string>.Failure("Kasa kaydı bulunamadı");
            }
            if (cashRegister.Name != request.Name)
            {
                bool isNameExists = await cashRegisterRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

                if (isNameExists)
                {
                    Result<string>.Failure("Bu Kasa Adı Daha Önce Kullanılmıştır");
                }
            }

            mapper.Map(request,cashRegister);
            
            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            cacheService.Remove("cashRegisters");

            return "Kasa kaydı başarıyla güncellendi";


        }
    }
}
