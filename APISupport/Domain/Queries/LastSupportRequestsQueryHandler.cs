using System.Data;
using Microsoft.Data.SqlClient;
using MediatR;
using Dapper;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace APISupport.Domain.Queries;

public class LastSupportRequestsQueryHandler :
    IRequestHandler<LastSupportRequestsQuery, IEnumerable<LastSupportRequestsQueryResult>>
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public LastSupportRequestsQueryHandler(IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public async Task<IEnumerable<LastSupportRequestsQueryResult>> Handle(LastSupportRequestsQuery request, CancellationToken cancellationToken)
    {
        using var connection = GetConnection();

        connection.Open();
        var result = await connection.QueryAsync<LastSupportRequestsQueryResult>(
            "SELECT TOP (@NumberLastSupportRequests) * " +
            "FROM dbo.SupportRequests " +
            "ORDER BY Id DESC",
            new { NumberLastSupportRequests = request.NumberLastSupportRequests });
        connection.Close();

        return result;
    }

    private IDbConnection GetConnection()
    {
        var connection = new SqlConnection(
            _configuration.GetConnectionString("DBSupport"));

        if (_environment.IsDevelopment())
            return new ProfiledDbConnection(connection, MiniProfiler.Current);

        return connection;
    }
}