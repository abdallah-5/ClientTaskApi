using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientTask.Core.Models;

namespace ClientTask.Core.Specifications.ClientSpecifications
{
    public class ClientWithFiltersForCountSpecification : BaseSpecification<Client>
    {
        public ClientWithFiltersForCountSpecification(PaginationSpecParams paginationSpecParams) : base(x =>
            (string.IsNullOrEmpty(paginationSpecParams.Search) || x.FirstName.ToLower().Contains(paginationSpecParams.Search)
        || x.LastName.ToLower().Contains(paginationSpecParams.Search)))
            
        {

        }
    }
}