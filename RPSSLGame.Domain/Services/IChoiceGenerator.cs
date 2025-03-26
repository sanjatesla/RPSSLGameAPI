using PSSLGame.Domain.Common;
using PSSLGame.Domain.Entities;

namespace PSSLGame.Domain.Services;

public interface IChoiceGenerator
{
    Task<Choices> GenerateChoice();
}
