using ClientTask.Core.Helpers.APIResponse;


namespace ClientTask.Core.Interfaces.IServices
{
    public interface IPolygonService
    {
        Task<IResponse> GetLastTradeData();
        void ScheduleJob();
    }
}
