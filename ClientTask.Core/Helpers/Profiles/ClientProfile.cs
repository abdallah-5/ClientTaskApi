

using AutoMapper;
using ClientTask.Core.DTOs.RequestDTOs.ClientDTOs;
using ClientTask.Core.DTOs.ResponseDTOs.ClientDTOs;
using ClientTask.Core.Models;

namespace ClientTask.Core.Helpers.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientRequestDto, Client>();
            CreateMap<Client, ClientResponseDTO>();
                
        }
    }
}