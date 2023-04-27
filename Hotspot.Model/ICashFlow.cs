using Hotspot.Model.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ICashFlow
    {
        //CashFlow
        //Create
        Task Add(CashFlow cashFlow);

        //Update - Not Needed

        //Recovery
        IEnumerable<CashFlow> GetAll();
        Task<CashFlow> GetById(int id);

        //Delete
        Task Delete(int id);
        Task Delete(CashFlow cashFlow);

        //Flow
        //Create
        Task AddFlow(CashFlow cashFlow, Flow flow);
        Task AddFlow(int cashFlowId, Flow flow);

        //Update - NotNeeded

        //Recovery
        IEnumerable<Flow> GetAllFlow();
        Task<Flow> GetFlowById(string id);
        IEnumerable<Flow> GetFlowByCashFlow(CashFlow cashFlow);
        IEnumerable<Flow> GetFlowByCashFlow(int cashFlowId);
        IEnumerable<Flow> GetFlowByDate(int cashFlowId, DateTime startDate, DateTime endDate);

        //Delete
        Task DeleteFlow(Flow flow);
        Task DeleteFlow(string id);
    }
}
