﻿using Microsoft.Extensions.Logging;
using PSSLGame.Domain.Common;
using PSSLGame.Domain.Configuration;
using PSSLGame.Domain.Entities;
using PSSLGame.Domain.Services;
using System.Net.Http.Json;

namespace RPSSLGame.Infrastructure.Services;

public class ChoiceGenerator : IChoiceGenerator
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ChoiceGenerator> _logger;
    private readonly AppSettings _settings;

    public ChoiceGenerator(HttpClient httpClient, ILogger<ChoiceGenerator> logger, AppSettings settings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _settings = settings;
    }

    /// <summary>Generates random choice.</summary>
    public async Task<Choices> GenerateChoice()
    {
        Choices? choice = null;

        // Using this external service for getting random number is for showcase purpose
        RandomChoiceResponse? response = null;
        try
        {
            // Get random number from external service
            response = await _httpClient.GetFromJsonAsync<RandomChoiceResponse>(_settings.RandomNumberUrl);
        }
        catch (Exception ex)
        {
            // Log error
            _logger.LogInformation("Error getting random number from url {0}: {1}", _settings.RandomNumberUrl, ex.Message);
        }
        var randomNumber = response?.random_number;
        // If random number is valid, use it to generate choice
        if (randomNumber != null && randomNumber <= Enum.GetValues<Choices>().Length)
            choice = (Choices)randomNumber;
        else
        {
            // Generate random number using local random generator
            var random = new Random();
            randomNumber = random.Next(1, Enum.GetValues<Choices>().Length + 1);
            choice = (Choices)randomNumber;
        }
        return choice.Value;
    }
}

public class RandomChoiceResponse
{
    public int random_number { get; set; }
}
