using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class ContactCall
    {
        public int ContactCallId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Vehicle { get; set; }
        public DateTime SystemDate { get; set; }
        public DateTime? ContactDate { get; set; }
        public string ContactTime { get; set; }
        public string Commentary { get; set; }
        public int? ContactCallStateId { get; set; }
        public int? ManagerId { get; set; }
        public int? SupervisorId { get; set; }
        public int? SellerId { get; set; }
    }

    public class ContactCallState
    {
        public int ContactCallStateId{ get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }

    public class ContactCallSummary
    {
        public int ContactCallId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Vehicle { get; set; }
        public DateTime SystemDate { get; set; }
        public DateTime ContactDate { get; set; }
        public string ContactTime { get; set; }
        public string Commentary { get; set; }
        public int ContactCallStateId { get; set; }
        public string ContactCallState { get; set; }
        public string Color { get; set; }
        public int? ManagerId { get; set; }
        public string Manager { get; set; }
        public int? SupervisorId { get; set; }
        public string Supervisor { get; set; }
        public int? SellerId { get; set; }
        public string Seller { get; set; }
    }
}
