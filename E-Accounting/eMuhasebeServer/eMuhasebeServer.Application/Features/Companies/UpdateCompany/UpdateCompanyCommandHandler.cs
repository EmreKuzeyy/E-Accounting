using AutoMapper;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.UpdateCompany
{
    internal sealed class UpdateCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IMapper mapper) : IRequestHandler<UpdateCompanyCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            Company company = await companyRepository.GetByExpressionWithTrackingAsync(c=> c.Id == request.Id,cancellationToken);
            if (company == null)
            {
                return Result<string>.Failure("Şirket Bulunamadı");
            }
            if (company.TaxNumber != request.TaxNumber)
            {
                bool isTaxNumberExists = await companyRepository.AnyAsync(c => c.TaxNumber == request.TaxNumber, cancellationToken);
                if (isTaxNumberExists)
                {
                    return Result<string>.Failure("Bu Vergi Numarası Daha Önce Kaydedilmiş.");
                }
            }

            mapper.Map(request,company);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // GetByExpressionWithTrackingAsync kullandığımız için update methodunu çağırmadan hallettik.

            cacheService.Remove("companies"); //cache den kaldırma işlemini yaptık (update,delete ve create de) get all da yazdık

            return "Şirket Kaydı Başarıyla Güncellendi";
        }
    }
}
