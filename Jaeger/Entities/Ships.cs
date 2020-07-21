using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using MeerJager.Entities;

namespace MeerJager.Data
{
    public class Ships
    {
        public List<EnemyType> Frigates { get; set; }
        
    }
    

    public class EnemyType
    {
        public int EnemyTypeId { get; set; }
        public string UIName { get; set; }
        public Range Health { get; set; }
        public Range Profile { get; set; }
    }
}
