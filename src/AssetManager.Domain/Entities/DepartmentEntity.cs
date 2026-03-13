using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Domain.Entities
{
    public class DepartmentEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
      
        // Navigation Property: Bu departmandaki kullanıcılar
        public ICollection<AppUserEntity> Users { get; set; } = new List<AppUserEntity>();
    }
}
