using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.MigrateAllCompanies
{
    internal sealed class MigrateAllCompaniesCommandHandler(
        ICompanyService companyService,
        ICompanyRepository companyRepository) : IRequestHandler<MigrateAllCompaniesCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(MigrateAllCompaniesCommand request, CancellationToken cancellationToken)
        {
            List<Company> companies = await companyRepository.GetAll().ToListAsync(cancellationToken);

            companyService.MigrateAll(companies);

            return "Şirket Databaseleri Başarıyla Güncellendi";
        }
    }
}
