using AutoMapper; 
using ClientTask.Core.Helpers.APIResponse;
using ClientTask.Core.Interfaces.IRepositories;
using ClientTask.Core.Interfaces.IServices;
using ClientTask.Core.Models;
using ClientTask.Core.Specifications;
using ClientTask.Core.Specifications.ClientSpecifications;

using ClientTask.Core.DTOs.ResponseDTOs.ClientDTOs;
using System.Transactions;
using ClientTask.Core.DTOs.RequestDTOs.ClientDTOs;

namespace ClientTask.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IGenericRepo<Client> _clientRepo;
        private readonly IMapper _mapper;

        public ClientService(
            IGenericRepo<Client> clientRepo,
            IMapper mapper
            )
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
        }
        public async Task<IResponse> GetClients(PaginationSpecParams paginationSpecParams)
        {
            var spec = new ClientSpecification(paginationSpecParams);
            var countSpec = new ClientWithFiltersForCountSpecification(paginationSpecParams);
            var totalItems = await _clientRepo.CountAsync(countSpec);
            var items = await _clientRepo.ListAsync(spec);
            if (items.Count() <= 0) return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.NoItem }, StatusCodeAndErrorsMessagesStandard.NotFound);


            var data = _mapper.Map<IReadOnlyList<ClientResponseDTO>>(items);


            return PaginationResponse<ClientResponseDTO>.Success(
                   data != null,
                   StatusCodeAndErrorsMessagesStandard.OK,
                   data,
                   paginationSpecParams.PageIndex,
                   paginationSpecParams.PageSize,
                   totalItems
               );



        }

        public async Task<IResponse> GetClient(int clientId)
        {
            var spec = new ClientSpecification(clientId);
            var item = await _clientRepo.GetEntityWithSpec(spec);
            if (item == null) return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.NoItem }, StatusCodeAndErrorsMessagesStandard.NotFound);
            var itemDto = _mapper.Map<ClientResponseDTO>(item);
            return SingleResponse<ClientResponseDTO>.Success(itemDto, StatusCodeAndErrorsMessagesStandard.OK);
        }

        public async Task<IResponse> CreateClient(ClientRequestDto requestDto)
        {

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var specItemExist = new ClientSpecification(requestDto.Email);
                    var itemExist = await _clientRepo.GetEntityWithSpec(specItemExist);
                     if(itemExist != null)
                    {
                        return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.ItemAlreadyExist }, StatusCodeAndErrorsMessagesStandard.BadRequest);
                    }

                    var request = _mapper.Map<Client>(requestDto);
                    var newItem = await _clientRepo.CreateEntityAsync(request);
                    if (newItem == null) return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.ItemNotCreated }, StatusCodeAndErrorsMessagesStandard.BadRequest);

                   

                    await _clientRepo.SaveChangesAsync();

                   
                    var ClientDto = _mapper.Map<ClientResponseDTO>(newItem);
                    scope.Complete();
                    return SingleResponse<ClientResponseDTO>.Success(ClientDto, StatusCodeAndErrorsMessagesStandard.Created);
                }
                catch (Exception)
                {
                    return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.ItemNotCreated }, StatusCodeAndErrorsMessagesStandard.InternalServerError);
                }
            }
        }


        public async Task<IResponse> UpdateClient(int clientId, ClientRequestDto requestDto)
        {

            try
            {
                var existingItem = await _clientRepo.GetById(clientId);

                if (existingItem == null)
                    return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.NoItem }, StatusCodeAndErrorsMessagesStandard.NotFound);

                _mapper.Map(requestDto, existingItem);

                _clientRepo.UpdateEntity(existingItem);
                await _clientRepo.SaveChangesAsync();

                var updatedDto = _mapper.Map<ClientResponseDTO>(existingItem);

                return SingleResponse<ClientResponseDTO>.Success(updatedDto, StatusCodeAndErrorsMessagesStandard.OK);
            }
            catch (Exception)
            {
                return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.ItemNotUpdated }, StatusCodeAndErrorsMessagesStandard.BadRequest);
            }
        }


        public async Task<IResponse> DeleteClient(int clientId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var existingItem = await _clientRepo.GetById(clientId);

                    if (existingItem == null)
                        return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.NoItem }, StatusCodeAndErrorsMessagesStandard.NotFound);

                   
                    _clientRepo.DeleteSoftEntity(existingItem);

                  

                    await _clientRepo.SaveChangesAsync();
                  
                    scope.Complete();
                    return BaseSuccessResponse.Success(StatusCodeAndErrorsMessagesStandard.OK);
                }
                catch (Exception)
                {
                    return FailResponse.Error(new List<string> { StatusCodeAndErrorsMessagesStandard.ItemNotDeleted }, StatusCodeAndErrorsMessagesStandard.BadRequest);
                }
            }


        }



    }
}
