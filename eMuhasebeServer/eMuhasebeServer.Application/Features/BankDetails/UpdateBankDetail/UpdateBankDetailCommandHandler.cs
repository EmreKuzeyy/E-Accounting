using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.BankDetails.UpdateBankDetail;

internal sealed class UpdateBankDetailCommandHandler(
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<UpdateBankDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateBankDetailCommand request, CancellationToken cancellationToken)
    {
        BankDetail? bankDetail = await bankDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (bankDetail is null)
        {
            return Result<string>.Failure("Banka Hareketi Kaydı Bulunamadı");
        }

        Bank? bank = await bankRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == bankDetail.BankId, cancellationToken);

        if (bank is null)
        {
            return Result<string>.Failure("Banka Kaydı Bulunamadı");
        }

        bank.DepositAmount -= bankDetail.DepositAmount;
        bank.WithDrawalAmount -= bankDetail.WithDrawalAmount;

        bank.DepositAmount += request.Type == 0 ? request.Amount : 0;
        bank.WithDrawalAmount += request.Type == 1 ? request.Amount : 0;

        bankDetail.DepositAmount = request.Type == 0 ? request.Amount : 0;
        bankDetail.WithDrawalAmount = request.Type == 1 ? request.Amount : 0;
        bankDetail.Description = request.Description;
        bankDetail.Date = request.Date;

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("banks");

        return "Banka Hareketi Başarıyla Güncellendi";
    }
}


