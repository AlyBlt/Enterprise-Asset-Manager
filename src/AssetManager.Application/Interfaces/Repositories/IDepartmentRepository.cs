using AssetManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<DepartmentEntity>
    {
        // İhtiyaç halinde department detaylı sorgular buraya.
    }
}
