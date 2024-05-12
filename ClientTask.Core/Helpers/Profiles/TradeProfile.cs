using AutoMapper;
using ClientTask.Core.DTOs.RequestDTOs.ClientDTOs;
using ClientTask.Core.DTOs.ResponseDTOs.ClientDTOs;
using ClientTask.Core.DTOs.ResponseDTOs.PolygonDTOs;
using ClientTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTask.Core.Helpers.Profiles
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<TradeResponseDTO, Trade>();

        }
    }
}
