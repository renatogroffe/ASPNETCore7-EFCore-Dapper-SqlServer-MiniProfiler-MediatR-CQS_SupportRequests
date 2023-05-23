using Microsoft.AspNetCore.Mvc;
using MediatR;
using APISupport.Models;
using APISupport.Domain.Commands;
using APISupport.Domain.Queries;

namespace APISupport.Controllers;

[ApiController]
[Route("[controller]")]
public class SupportRequestController : ControllerBase
{
    private readonly ILogger<SupportRequestController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public SupportRequestController(ILogger<SupportRequestController> logger,
        IConfiguration configuration, IMediator mediator)
    {
        _logger = logger;
        _configuration = configuration;
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IEnumerable<SupportRequestDetails>> Get()
    {
        _logger.LogInformation("Iniciando a consulta aos ultimos chamados...");

        var query = new LastSupportRequestsQuery()
        {
            NumberLastSupportRequests = Convert.ToInt32(
                _configuration["NumberLastSupportRequests"])
        };
        var result = await _mediator.Send(query);

        _logger.LogInformation(
            $"No. de registros encontrados: {result.Count()} | " +
            $"Qtde. maxima retornada por pesquisa: {query.NumberLastSupportRequests}");

        return result.Select(r => new SupportRequestDetails()
        {
            Id = r.Id,
            RequestDate = r.RequestDate,
            Email = r.Email,
            Problem = r.Problem
        });
    }

    [HttpPost]
    [ProducesResponseType(typeof(SupportRequestSubmissionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SupportRequestSubmissionResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SupportRequestSubmissionResult>> Post(SupportRequestSubmission submission)
    {
        _logger.LogInformation("Iniciando a inclusao do chamado...");

        var resultCommand = await _mediator.Send(new CreateSupportRequestCommand()
        {
            Email = submission.Email,
            Problem = submission.Problem
        });

        var result = new SupportRequestSubmissionResult()
        {
            Success = resultCommand.Success,
            Message = resultCommand.Message
        };

        if (result.Success)
        {
            _logger.LogInformation(result.Message);
            return result;
        }
        else
        {
            _logger.LogError(result.Message);
            return BadRequest(result);
        }        
    }
}