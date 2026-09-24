using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EntityUserResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CommissionAmount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Amount { get; set; }
    }
    public class EntityWalletResult
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
    }
}
