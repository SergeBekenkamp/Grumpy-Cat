using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grumpy_Cat.UserAccount
{
    public class UserAccount
    {
        public ulong ID { get; set; }

        public ulong Level { get; set; }

        public ulong XP { get; set; }

        public DateTime TimeConnected { get; set; }

        public ulong TotalTimeConntected { get; set; }
    }
}
