using System.Collections.Generic;
using Compass.Security.Application.Commons.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Compass.Security.Application.Services.Accounts.Queries.Scheme
{
    public class SchemeQuery : IRequest<ResponseDto<List<AuthenticationScheme>>>
    {
        
    }
}