using AutoMapper;
using ClientTask.Core.Models;
using ClientTask.Core.Specifications;

namespace ClientTask.Core.Specifications.ClientSpecifications
{
    public class ClientSpecification : BaseSpecification<Client>
    {
        private readonly IMapper _mapper;

        public ClientSpecification(PaginationSpecParams paginationSpecParams) : base(
            x => (string.IsNullOrEmpty(paginationSpecParams.Search) || x.FirstName.ToLower().Contains(paginationSpecParams.Search)
        || x.LastName.ToLower().Contains(paginationSpecParams.Search)))
        {
            ApplyPaging(paginationSpecParams.PageSize * (paginationSpecParams.PageIndex - 1), paginationSpecParams.PageSize);

        }


        public ClientSpecification(int ClientId) : base(x => x.Id == ClientId && x.IsDeleted == false)
        {

        }

        public ClientSpecification(string email) : base(x => x.Email == email && x.IsDeleted == false)
        {

        }


    }
}
