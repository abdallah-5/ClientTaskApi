using AutoMapper;
using ClientTask.Core.DTOs.ResponseDTOs.PolygonDTOs;
using ClientTask.Core.Helpers.APIResponse;
using ClientTask.Core.Interfaces.IRepositories;
using ClientTask.Core.Interfaces.IServices;
using ClientTask.Core.Models;
using System.Text.Json;
using Hangfire;

namespace ClientTask.Core.Services
{
    public class PolygonService : IPolygonService
    {
        private readonly HttpClient _httpClient;
        private readonly IGenericRepo<Trade> _tradeRepo;
        private readonly IGenericRepo<Client> _clientRepo;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSender;
        public PolygonService(HttpClient httpClient, IGenericRepo<Trade> tradeRepo, IMapper mapper, IEmailSenderService emailSender, IGenericRepo<Client> clientRepo)
        {
            _httpClient = httpClient;
            _tradeRepo = tradeRepo;
            _mapper = mapper;
            _emailSender = emailSender;
            _clientRepo = clientRepo;
        }







        public async Task<IResponse> GetLastTradeData()
        {
            try
            {

                // Retrieve all clients from the database
                var clients = await _clientRepo.ListAll();

                // Check if any clients exist
                if (clients == null || !clients.Any())
                {
                    return FailResponse.Error(new List<string> { "No clients found" }, StatusCodeAndErrorsMessagesStandard.NotFound);
                }

                string endpoint = $"https://api.polygon.io/v2/aggs/ticker/AAPL/range/1/day/2023-01-09/2023-01-09?adjusted=true&sort=asc&apiKey=yY6vsWrAoQaJY5E6M3pDpZ3sKSEkcHm7";

                // Create an HttpClient instance
                using HttpClient client = new HttpClient();

                // Make a GET request to the API
                HttpResponseMessage response = await client.GetAsync(endpoint);

                // Check if the request was successful (status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into the desired object
                    var jsonResponse = JsonSerializer.Deserialize<dynamic>(responseData);
                    var resultsArray = jsonResponse.GetProperty("results");
                    // Deserialize the 'results' array from the JSON response into a list of TradeResponseDTO objects
                    var lastTradeData = JsonSerializer.Deserialize<List<TradeResponseDTO>>(resultsArray.ToString());

                    // Save the last trade data into the database
                    var data = _mapper.Map<List<Trade>>(lastTradeData);
                    await _tradeRepo.CreateEntitiesAsync(data);
                    await _tradeRepo.SaveChangesAsync();

                    //Send Mail For All users
                    try
                    {

                        foreach (var ourClient in clients)
                        {
                            string body =
       @"<table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                            <tbody>";

                            body +=
                            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                                             <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Last Trade Data: </td>
                                             <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                                                 <ul>";

                            foreach (var trade in lastTradeData)
                            {
                                body += $"<li>Volume: {trade.V}, Volume Weighted Average Price: {trade.VW}, Open: {trade.O}, Close: {trade.C}, High: {trade.H}, Low: {trade.L}, Timestamp: {trade.T}, Number of Items: {trade.N}</li>";
                            }

                            body += @"</ul></td></tr></tbody></table>";
                            //SendMail("testtittest40@gmail.com", ourClient.Email, "Last Trade Data", body);
                            await _emailSender.SendMailAsync("testtittest40@gmail.com", ourClient.Email, "Last Trade Data", body);

                        }



                    }
                    catch (Exception ex)
                    {
                        return FailResponse.Error(new List<string> { $"An error occurred: {ex.Message}" }, StatusCodeAndErrorsMessagesStandard.InternalServerError);

                    }


                    // Return success response with deserialized data
                    return SingleResponse<List<TradeResponseDTO>>.Success(lastTradeData, StatusCodeAndErrorsMessagesStandard.OK);
                }
                else
                {
                    // Return error response
                    return FailResponse.Error(new List<string> { $"Error: {response.StatusCode}" }, StatusCodeAndErrorsMessagesStandard.BadRequest);
                }
            }
            catch (Exception ex)
            {
                // Return error response for exception
                return FailResponse.Error(new List<string> { $"An error occurred: {ex.Message}" }, StatusCodeAndErrorsMessagesStandard.InternalServerError);
            }
        }


        public void ScheduleJob()
        {
            RecurringJob.AddOrUpdate<IPolygonService>("GetLastTradeData", x => x.GetLastTradeData(), Cron.Minutely);
        }


    }
}
