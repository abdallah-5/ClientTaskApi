

using ClientTask.Core.DTOs.RequestDTOs.ClientDTOs;
using ClientTask.Core.DTOs.ResponseDTOs.ClientDTOs;
using ClientTask.Core.DTOs.ResponseDTOs.PolygonDTOs;
using ClientTask.Core.Helpers.APIResponse;
using ClientTask.Core.Interfaces.IServices;
using ClientTask.Core.Services;
using ClientTask.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ClientTask.API.Controllers

{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IPolygonService _polygonService;

        public ClientController(IClientService clientService, IPolygonService polygonService)
        {
            _clientService = clientService;
            _polygonService = polygonService;

        }

        [HttpPost("createClient")]

        [ProducesResponseType(typeof(SingleResponse<ClientResponseDTO>), StatusCodeAndErrorsMessagesStandard.OK)]
        [ProducesResponseType(typeof(FailResponse), StatusCodeAndErrorsMessagesStandard.BadRequest)]
        
        public async Task<ActionResult<IResponse>> CreateClient(ClientRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(y => y.Errors).Select(e => e.ErrorMessage).ToList();
                var response = new FailResponse { ErrorMessages = errors, StatusCode = StatusCodeAndErrorsMessagesStandard.BadRequest, Status = false };
                return BadRequest(response);
            }
            var item = await _clientService.CreateClient(requestDto);
            if (item.StatusCode == StatusCodeAndErrorsMessagesStandard.BadRequest)
                return BadRequest(item);
           

            return Ok(item);
        }

        [HttpPut("updateClient/{clientId}")]
        [ProducesResponseType(typeof(SingleResponse<ClientRequestDto>), StatusCodeAndErrorsMessagesStandard.OK)]
        [ProducesResponseType(typeof(FailResponse), StatusCodeAndErrorsMessagesStandard.NotFound)]
        public async Task<ActionResult<IResponse>> UpdateClient([FromRoute] int clientId, ClientRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(y => y.Errors).Select(e => e.ErrorMessage).ToList();
                var response = new FailResponse { ErrorMessages = errors, StatusCode = StatusCodeAndErrorsMessagesStandard.BadRequest, Status = false };
                return BadRequest(response);
            }

            var item = await _clientService.UpdateClient(clientId, requestDto);

            if (item.StatusCode == StatusCodeAndErrorsMessagesStandard.NotFound)
                return NotFound(item);

            if (item.StatusCode == StatusCodeAndErrorsMessagesStandard.BadRequest)
                return BadRequest(item);


            return Ok(item);
        }



        [HttpGet("getClients")]
       
        [ProducesResponseType(typeof(PaginationResponse<ClientResponseDTO>), StatusCodeAndErrorsMessagesStandard.OK)]
        [ProducesResponseType(typeof(FailResponse), StatusCodeAndErrorsMessagesStandard.NotFound)]
       
        public async Task<ActionResult<IResponse>> GetClients([FromQuery] PaginationSpecParams paginationSpecParams)
        {
            var item = await _clientService.GetClients(paginationSpecParams);

            if (item.StatusCode == StatusCodeAndErrorsMessagesStandard.NotFound)
                return NotFound(item);

            return Ok(item);
        }



        [HttpGet("getClientById/{clientId}")]
        [ProducesResponseType(typeof(SingleResponse<ClientResponseDTO>), StatusCodeAndErrorsMessagesStandard.OK)]
        [ProducesResponseType(typeof(FailResponse), StatusCodeAndErrorsMessagesStandard.NotFound)]
        
        public async Task<ActionResult<IResponse>> GetClient([FromRoute] int clientId)
        {
            var item = await _clientService.GetClient(clientId);
            if (item.StatusCode == StatusCodeAndErrorsMessagesStandard.NotFound)
                return NotFound(item);

            return Ok(item);
        }


        [HttpDelete("deleteClient/{clientId}")]
        [ProducesResponseType(typeof(BaseSuccessResponse), StatusCodeAndErrorsMessagesStandard.OK)]
        [ProducesResponseType(typeof(FailResponse), StatusCodeAndErrorsMessagesStandard.NotFound)]
        public async Task<ActionResult<IResponse>> DeleteClient([FromRoute] int clientId)
        {

            var item = await _clientService.DeleteClient(clientId);

            if (item.StatusCode == StatusCodeAndErrorsMessagesStandard.NotFound)
                return NotFound(item);

            if (item.StatusCode == StatusCodeAndErrorsMessagesStandard.BadRequest)
                return BadRequest(item);


            return Ok(item);
        }


        [HttpGet("getLastTradeData")]
        [ProducesResponseType(typeof(SingleResponse<TradeResponseDTO>), StatusCodeAndErrorsMessagesStandard.OK)]
        [ProducesResponseType(typeof(FailResponse), StatusCodeAndErrorsMessagesStandard.BadRequest)]
        [ProducesResponseType(typeof(FailResponse), StatusCodeAndErrorsMessagesStandard.InternalServerError)]
        public async Task<ActionResult<IResponse>> GetLastTradeData()
        {
           

            try
            {
                // Call the service method to get last trade data
                var response = await _polygonService.GetLastTradeData();

                // Check the response status code
                if (response.StatusCode == StatusCodeAndErrorsMessagesStandard.OK)
                {
                    // Return OK with data
                    return Ok(response);
                }
                else
                {
                    // Return appropriate error response
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                // Return Internal Server Error if an exception occurs
                return StatusCode(StatusCodeAndErrorsMessagesStandard.InternalServerError, FailResponse.Error(new List<string> { $"An error occurred: {ex.Message}" }, StatusCodeAndErrorsMessagesStandard.InternalServerError));
            }
        }


    }
}
