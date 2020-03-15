using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public interface IBaseBetListMetaModel : IBaseMetaModel
    {
        bool HasPlayerCom { get; set; }
        string AgentListXml { get; set; }
    }
}
