using AutoMapper;
using FlightBoard.API.Hubs;
using FlightBoard.Application.Services;
using FlightBoard.Domain.Models;
using FlightBoard.Domain.Models.DTOs;
using FlightBoard.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
public class FlightStatusBroadcastService : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly IHubContext<FlightHub> _hub;
    private readonly FlightStatusCalculator _calc;
    private readonly ILogger<FlightStatusBroadcastService> _logger;

    // keep last-known statuses in-memory (we don’t persist status in DB)
    private readonly Dictionary<int, FlightStatus> _last = new();

    public FlightStatusBroadcastService(
        IServiceProvider sp,
        IHubContext<FlightHub> hub,
        FlightStatusCalculator calc,
        ILogger<FlightStatusBroadcastService> logger)
    {
        _sp = sp;
        _hub = hub;
        _calc = calc;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // run every 30 seconds (adjust as you like)
        var interval = TimeSpan.FromSeconds(30);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IFlightRepository>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var flights = await repo.GetAllAsync();
                var now = DateTime.UtcNow;

                foreach (var f in flights)
                {
                    var newStatus = _calc.CalculateStatus(f.DepartureTime, now);

                    if (!_last.TryGetValue(f.Id, out var prev) || prev != newStatus)
                    {
                        _last[f.Id] = newStatus;

                        var dto = mapper.Map<FlightDto>(f);
                        dto.Status = newStatus.ToString();

                        await _hub.Clients.All.SendAsync("FlightStatusUpdated", dto, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Status broadcaster failed");
            }

            try { await Task.Delay(interval, stoppingToken); } catch { /* cancelled */ }
        }
    }
}
