using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.DeleteCompanyById
{
    internal sealed class DeleteCompanyByIdCommandHandler(
        ICompanyRepository companyRepository,
        ICacheService cacheService,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteCompanyByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCompanyByIdCommand request, CancellationToken cancellationToken)
        {
            Company company = await companyRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);

            if (company == null)
            {
                return Result<string>.Failure("Şirket Bulunamadı");
            }

            company.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            cacheService.Remove("companies");

            return "Şirket Kaydı Başarıyla Silindi";
        }
    }
}
