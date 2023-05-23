using System.Text;
using MediatR;
using APISupport.Data;
using APISupport.Domain.Validators;

namespace APISupport.Domain.Commands;

public class CreateSupportRequestCommandHandler :
    IRequestHandler<CreateSupportRequestCommand, CreateSupportRequestCommandResult>
{
    private readonly SupportContext _context;

    public CreateSupportRequestCommandHandler(SupportContext context)
    {
        _context = context;
    }

    public Task<CreateSupportRequestCommandResult> Handle(CreateSupportRequestCommand request, CancellationToken cancellationToken)
    {
        var validationResult =
            new SupportRequestValidator().Validate(request);
        if (validationResult.IsValid)
        {
            _context.SupportRequests!.Add(new ()
            {
                RequestDate = DateTime.Now,
                Email = request.Email,
                Problem = request.Problem
            });
            _context.SaveChanges();

            return Task.FromResult(new CreateSupportRequestCommandResult()
            {
                Success = true,
                Message = "Inclusao realizada com sucesso"
            });
        }
        else
        {
            var errorDescriptions = new StringBuilder();
            foreach (var error in validationResult.Errors)
            {
                if (errorDescriptions.Length > 0)
                    errorDescriptions.Append(" | ");
                errorDescriptions.Append(error.ErrorMessage);
            }

            return Task.FromResult(new CreateSupportRequestCommandResult()
            {
                Success = false,
                Message = $"Dados invalidos: {errorDescriptions.ToString()}"
            });
        }
    }
}