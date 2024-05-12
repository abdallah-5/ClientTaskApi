
using ClientTask.Core.Helpers.APIResponse;

namespace ClientTask.Core.Helpers.APIResponse
{
    public class FailResponse :IResponse
    {
        public bool Status { get; set; }

        public int StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }


        public static FailResponse Error(List<string> errorMessage, int statusCode)
        {
            return new FailResponse { StatusCode = statusCode, Status = false, ErrorMessages = errorMessage };
        }
    }
}
