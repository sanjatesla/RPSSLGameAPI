using PSSLGame.Domain.Entities;

namespace PSSLGame.Domain.Services;

public interface IChoiceGenerator
{
    Task<Choice> GenerateChoice();
}
