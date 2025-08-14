using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enums;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.BankDetails.CreateBankDetail;

internal sealed class CreateBankDetailCommandHandler(
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<CreateBankDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateBankDetailCommand request, CancellationToken cancellationToken)
    {
        Bank? bank = await bankRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.BankId, cancellationToken);

        bank.DepositAmount += (request.Type == 0 ? request.Amount : 0);
        bank.WithDrawalAmount += (request.Type == 1 ? request.Amount : 0);


        BankDetail bankDetail = new()
        {
            Date = request.Date,
            DepositAmount = request.Type == 0 ? request.Amount : 0,
            WithDrawalAmount = request.Type == 1 ? request.Amount : 0,
            Description = request.Description,
            BankId = request.BankId
        };

        await bankDetailRepository.AddAsync(bankDetail, cancellationToken);

        if (request.OppositeBankId is not null)
        {
            Bank oppositeBank = await bankRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.OppositeBankId, cancellationToken);

            oppositeBank.DepositAmount += (request.Type == 1 ? request.OppositeAmount : 0);
            oppositeBank.WithDrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

            BankDetail oppositeBankDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithDrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                BankDetailId = bankDetail.Id,
                Description = request.Description,
                BankId = (Guid)request.OppositeBankId
            };

            bankDetail.BankDetailId = oppositeBankDetail.Id;

            await bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);
        }


        if (request.OppositeCashRegisterId is not null)
        {
            CashRegister oppositeCashRegister = await cashRegisterRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.OppositeCashRegisterId, cancellationToken);

            oppositeCashRegister.DepositAmount += (request.Type == 1 ? request.OppositeAmount : 0);
            oppositeCashRegister.WithDrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

            CashRegisterDetail oppositeCashRegisterDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithDrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                BankDetailId = bankDetail.Id,
                Description = request.Description,
                CashRegisterId = (Guid)request.OppositeCashRegisterId
            };

            bankDetail.CashRegisterDetailId = oppositeCashRegisterDetail.Id;

            await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);

            cacheService.Remove("cashRegisters");
        }

        if (request.OppositeCustomerId is not null)
        {
            Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c=> c.Id == request.OppositeCustomerId, cancellationToken);
            if (customer == null)
            {
                return Result<string>.Failure("Cari Kaydı Bukunamadı");
            }
            customer.DepositAmount += request.Type == 1 ? request.Amount : 0;
            customer.WithDrawalAmount += request.Type == 0 ? request.Amount : 0;

            CustomerDetail customerDetail = new()
            {
                CustomerId = customer.Id,
                BankDetailId = bankDetail.Id,
                Date = request.Date,
                Description = request.Description,
                DepositAmount = request.Type == 1 ? request.Amount : 0,
                WithDrawalAmount = request.Type == 0 ? request.Amount : 0,
                Type = CustomerDetailTypeEnum.Bank

            };

            bankDetail.CustomerDetailId = customerDetail.Id;

            cacheService.Remove("customers");

            await customerDetailRepository.AddAsync(customerDetail, cancellationToken);
        }

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("banks");
        

        return "Banka Hareketi Başarıyla Kaydedildi";
    }
}
