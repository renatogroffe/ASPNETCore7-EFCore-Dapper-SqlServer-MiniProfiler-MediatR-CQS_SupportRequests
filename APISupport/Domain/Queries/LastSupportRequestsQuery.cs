using MediatR;

namespace APISupport.Domain.Queries
{
    public class LastSupportRequestsQuery :
        IRequest<IEnumerable<LastSupportRequestsQueryResult>>
    {
        public int NumberLastSupportRequests { get; set; }
    }
}