using AssetManager.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.User.Queries.GetUserById
{
    public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;
}
