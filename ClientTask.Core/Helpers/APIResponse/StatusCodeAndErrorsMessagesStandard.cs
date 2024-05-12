

namespace ClientTask.Core.Helpers.APIResponse
{
    public static class StatusCodeAndErrorsMessagesStandard
    {
        // StatusCodes

        // Start Success Codes
        public const int OK = 200;
        public const int Created = 201;
        public const int NoContent = 204;

        // Start Errors Codes
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int InternalServerError = 500;


        // ErrorsMessages

        public static string NoItem = "Item Not Found";
        
        public static string InternalServerErrorMessage = "Internal Server Error";

        public static string ItemNotCreated = "Item Not Created";
        public static string ItemNotUpdated = "Item Not Updated";
        public static string ItemNotDeleted = "Item Not Deleted";

        public static string ItemAlreadyExist = "Item Already Exist";


        

    }
}
