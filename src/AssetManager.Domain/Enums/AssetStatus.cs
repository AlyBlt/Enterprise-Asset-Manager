using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Domain.Enums
{
    public enum AssetStatus
    {
        Active = 1,      // Kullanımda
        InRepair = 2,    // Tamirde
        Retired = 3,     // Hurdaya ayrıldı/Emekli
        Lost = 4,        // Kayıp
        InStock = 5,     // Stokta (Henüz atanmamış)
        Assigned = 6    //zimmetli
    }
}
