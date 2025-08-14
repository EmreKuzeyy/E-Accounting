using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.GetAllCompanies
{
    public sealed record GetAllCompaniesQuery : IRequest<Result<List<Company>>>;
}
