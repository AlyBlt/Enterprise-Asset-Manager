using AssetManager.Application.DTOs.User;
using AssetManager.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
     : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllWithDetailsAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
