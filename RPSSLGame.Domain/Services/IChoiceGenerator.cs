using PSSLGame.Domain.Common;
using PSSLGame.Domain.Entities;

namespace PSSLGame.Domain.Services;

public interface IChoiceGenerator
{
    /// <summary>Generates random choice.</summary>
    Task<Choices> GenerateChoice();
}
