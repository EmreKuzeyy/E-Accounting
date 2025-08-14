using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails.UpdateCashRegisterDetail;

internal sealed class UpdateCashRegisterDetailCommandHandler(
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<UpdateCashRegisterDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCashRegisterDetailCommand request, CancellationToken cancellationToken)
    {
        CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (cashRegisterDetail is null)
        {
            return Result<string>.Failure("Kasa Hareketi Kaydı Bulunamadı");
        }

        CashRegister? cashRegister = await cashRegisterRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.CashRegisterId, cancellationToken);

        if (cashRegister is null)
        {
            return Result<string>.Failure("Kasa Kaydı Bulunamadı");
        }

        cashRegister.DepositAmount -= cashRegisterDetail.DepositAmount;
        cashRegister.WithDrawalAmount -= cashRegisterDetail.WithDrawalAmount;

        cashRegister.DepositAmount += request.Type == 0 ? request.Amount : 0;
        cashRegister.WithDrawalAmount += request.Type == 1 ? request.Amount : 0;

        cashRegisterDetail.DepositAmount = request.Type == 0 ? request.Amount : 0;
        cashRegisterDetail.WithDrawalAmount = request.Type == 1 ? request.Amount : 0;
        cashRegisterDetail.Description = request.Description;
        cashRegisterDetail.Date = request.Date;

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("cashRegisters");

        return "Kasa Hareketi Başarıyla Güncellendi";
    }
}


