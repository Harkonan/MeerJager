using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerJager.Entities
{
    public class MenuOption
    {
        public int Id { get; set; }
        public char Key { get; set; }
        public int MyProperty { get; set; }
        public string Display { get; set; }
        public Action<int> Action { get; set; }
    }
}
