using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Core.Entities;
using AssetManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Repositories
{
    internal class DepartmentRepository(AssetManagerDbContext context)
    : GenericRepository<DepartmentEntity>(context), IDepartmentRepository
    {
    }
}
