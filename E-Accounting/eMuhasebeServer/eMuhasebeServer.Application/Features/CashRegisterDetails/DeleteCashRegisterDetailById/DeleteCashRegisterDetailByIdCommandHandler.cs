using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails.DeleteCashRegisterDetailById;

internal sealed class DeleteCashRegisterDetailByIdCommandHandler(
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<DeleteCashRegisterDetailByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteCashRegisterDetailByIdCommand request, CancellationToken cancellationToken)
    {
        CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository
            .GetByExpressionWithTrackingAsync(p=> p.Id == request.Id, cancellationToken);

        if (cashRegisterDetail is null)
        {
            return Result<string>.Failure("Kasa Hareketi Kaydı Bulunamadı");
        }

        CashRegister? cashRegister = await cashRegisterRepository
            .GetByExpressionWithTrackingAsync(p=> p.Id == cashRegisterDetail.CashRegisterId,cancellationToken);

        if (cashRegister is null)
        {
            return Result<string>.Failure("Kasa Kaydı Bulunamadı");
        }

        cashRegister.DepositAmount -= cashRegisterDetail.DepositAmount;
        cashRegister.WithDrawalAmount -= cashRegisterDetail.WithDrawalAmount;

        if (cashRegisterDetail.CashRegisterDetailId is not null)
        {
            CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.CashRegisterDetailId, cancellationToken);

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
        }

        if (cashRegisterDetail.BankDetailId is not null)
        {
            BankDetail? oppositeBankDetail = await bankDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.BankDetailId, cancellationToken);

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

        if (cashRegisterDetail.CustomerDetailId is not null)
        {
            CustomerDetail? customerDetail = await customerDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.CustomerDetailId, cancellationToken);

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


        cashRegisterDetailRepository.Delete(cashRegisterDetail);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("cashRegisters");
        cacheService.Remove("banks");

        return "Kasa Hareketi Başarıyla Silindi";
    }
}


