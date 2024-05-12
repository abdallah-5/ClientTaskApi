using ClientTask.Core.DTOs.RequestDTOs.ClientDTOs;
using ClientTask.Core.Helpers.APIResponse;
using ClientTask.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTask.Core.Interfaces.IServices
{
    public interface IClientService
    {
        Task<IResponse> GetClients(PaginationSpecParams clientParams);
        Task<IResponse> GetClient(int clientId);
        Task<IResponse> CreateClient(ClientRequestDto requestDto);
        Task<IResponse> UpdateClient(int clientId, ClientRequestDto requestDto);

        Task<IResponse> DeleteClient(int clientId);

    }
}
