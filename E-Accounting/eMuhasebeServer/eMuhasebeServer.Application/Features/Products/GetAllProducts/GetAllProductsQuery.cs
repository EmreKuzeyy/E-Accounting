using eMuhasebeServer.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products.GetAllProducts;

public sealed record GetAllProductsQuery : IRequest<Result<List<Product>>>;
