using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.BankDetails.DeleteBankDetailById;

internal sealed class DeleteBankDetailByIdCommandHandler(
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<DeleteBankDetailByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteBankDetailByIdCommand request, CancellationToken cancellationToken)
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

        if (bankDetail.BankDetailId is not null)
        {
            BankDetail? oppositeBankDetail = await bankDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == bankDetail.BankDetailId, cancellationToken);

            if (oppositeBankDetail is null)
            {
                return Result<string>.Failure("Banka Hareketi Kaydı Bulunamadı");
            }

            Bank? oppositeBank = await bankRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == oppositeBankDetail.BankId, cancellationToken);

            if (oppositeBank is null)
            {
                return Result<string>.Failure("Banka Kaydı Bulunamadı");
            }
            oppositeBank.DepositAmount -= oppositeBankDetail.DepositAmount;
            oppositeBank.WithDrawalAmount -= oppositeBankDetail.WithDrawalAmount;

            bankDetailRepository.Delete(oppositeBankDetail);
        }

        if (bankDetail.CashRegisterDetailId is not null)
        {
            CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == bankDetail.CashRegisterDetailId, cancellationToken);

            if (oppositeCashRegisterDetail is null)
            {
                return Result<string>.Failure("Kasa Hareketi Kaydı Bulunamadı");
            }

            CashRegister? oppositeCashRegister = await cashRegisterRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == oppositeCashRegisterDetail.CashRegisterId, cancellationToken);

            if (oppositeCashRegister is null)
            {
                return Result<string>.Failure("Kasa Kaydı Bulunamadı");
            }
            oppositeCashRegister.DepositAmount -= oppositeCashRegisterDetail.DepositAmount;
            oppositeCashRegister.WithDrawalAmount -= oppositeCashRegisterDetail.WithDrawalAmount;

            cashRegisterDetailRepository.Delete(oppositeCashRegisterDetail);

            cacheService.Remove("cashRegisters");
        }

        if (bankDetail.CustomerDetailId is not null)
        {
            CustomerDetail? customerDetail = await customerDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == bankDetail.CustomerDetailId, cancellationToken);

            if (customerDetail is null)
            {
                return Result<string>.Failure("Cari Hareket Kaydı Bulunamadı");
            }

            Customer? customer = await customerRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == customerDetail.CustomerId, cancellationToken);

            if (customer is null)
            {
                return Result<string>.Failure("Cari Kaydı Bulunamadı");
            }
            customer.DepositAmount -= customerDetail.DepositAmount;
            customer.WithDrawalAmount -= customerDetail.WithDrawalAmount;

            customerDetailRepository.Delete(customerDetail);

            cacheService.Remove("customers");
        }

        bankDetailRepository.Delete(bankDetail);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("banks");
        
        return "Banka Hareketi Başarıyla Silindi";
    }
}
