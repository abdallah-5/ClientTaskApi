namespace ClientTask.Core.Helpers.APIResponse
{
    public interface IResponse
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
    }
}