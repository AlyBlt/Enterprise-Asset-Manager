using AssetManager.Application.DTOs.User;
using AssetManager.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.Id);
            return mapper.Map<UserDto>(user);
        }
    }
}
