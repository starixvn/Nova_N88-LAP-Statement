using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    public class RefNoDataResult
    {
        public IList<TicketText> Data { get; set; }

        public int TotalRows { get; set; }
    }
}
