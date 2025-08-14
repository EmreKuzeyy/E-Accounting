using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.CreateCashRegister
{
    internal sealed class CreateCashRegisterCommandHandler(
        ICashRegisterRepository cashRegisterRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper,
        ICacheService cacheService) : IRequestHandler<CreateCashRegisterCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCashRegisterCommand request, CancellationToken cancellationToken)
        {
           bool isNameExists = await cashRegisterRepository.AnyAsync(p=> p.Name == request.Name,cancellationToken);

            if (isNameExists)
            {
                Result<string>.Failure("Bu Kasa Adı Daha Önce Kullanılmıştır");
            }

            CashRegister cashRegister = mapper.Map<CashRegister>(request);

            await cashRegisterRepository.AddAsync(cashRegister);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            cacheService.Remove("cashRegisters");

            return "Kasa Kayıt İşlemi Başarıyla Tamamlandı";
        }
    }
}
