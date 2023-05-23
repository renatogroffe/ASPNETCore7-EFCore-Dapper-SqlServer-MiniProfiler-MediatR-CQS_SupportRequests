using FluentValidation;
using APISupport.Domain.Commands;

namespace APISupport.Domain.Validators;

public class SupportRequestValidator :
    AbstractValidator<CreateSupportRequestCommand>
{
    public SupportRequestValidator()
    {
        RuleFor(c => c.Email).NotEmpty().WithMessage("Preencha o campo 'Email'")
            .EmailAddress().WithMessage("Formato invalido para o campo 'Email'")
            .MaximumLength(100).WithMessage("O campo 'Email' deve possuir no maximo 100 caracteres");

        RuleFor(c => c.Problem).NotEmpty().WithMessage("Preencha o campo 'Problem'")
            .MinimumLength(15).WithMessage("O campo 'Problem' deve possuir no minimo 15 caracteres")
            .MaximumLength(500).WithMessage("O campo 'Problem' deve possuir no maximo 500 caracteres");
    }
}