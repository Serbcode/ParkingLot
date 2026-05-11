using ParkingLotSystem.Vehicles;
using ParkingLotSystem.ParkingSpots.States;
using ParkingLotSystem.Core.Domain;

namespace ParkingLotSystem.ParkingSpots;

/// <summary>
/// tracks size, availability, and assigned vehicle.
/// </summary>
public class ParkingSpot(int spotNumber, VehicleSize size, bool isVip = false)
{
    private IParkingSpotState _state = AvailableParkingSpotState.Instance;

    public readonly int SpotNumber = spotNumber;
    public readonly VehicleSize Size = size;
    public bool IsVip { get; } = isVip;

    public Vehicle? AssignedVehicle => _state.AssignedVehicle;
    public ParkingSpotStatus Status => _state.Status;

    public virtual void AssignVehicle(Vehicle vehicle)
    {
        _state.AssignVehicle(this, vehicle);
    }

    public bool IsAvailable => _state.IsAvailable;

    public bool IsTaken => !IsAvailable;

    public virtual void Release()
    {
        _state.Release(this);
    }

    public void MarkAvailable()
    {
        EnsureNoAssignedVehicle();
        SetState(AvailableParkingSpotState.Instance);
    }

    public void MarkUnderConstruction()
    {
        EnsureNoAssignedVehicle();
        SetState(UnderConstructionParkingSpotState.Instance);
    }

    public void MarkCleaning()
    {
        if (AssignedVehicle is not null)
        {
            throw new DomainError($"The spot {SpotNumber} must be released before changing state!");
        }
        EnsureNoAssignedVehicle();
        SetState(CleaningParkingSpotState.Instance);
    }

    internal void SetState(IParkingSpotState state)
    {
        _state = state;
    }

    private void EnsureNoAssignedVehicle()
    {
        if (AssignedVehicle is not null)
        {
            throw new DomainError($"The spot {SpotNumber} must be released before changing state!");
        }
    }

    public override string ToString()
    {
        var vipLabel = IsVip ? " VIP" : string.Empty;
        var stateLabel = Status switch
        {
            ParkingSpotStatus.UnderConstruction => "Under construction",
            ParkingSpotStatus.Cleaning => "Cleaning",
            _ => AssignedVehicle?.LicensePlate
        };

        return this switch
        {
            CompactSpot => $"[🏍️- {stateLabel}{vipLabel}]",
            RegularSpot => $"[🚗 - {stateLabel}{vipLabel}]",
            OversizedSpot => $"[🚚 - {stateLabel}{vipLabel}]",
            HandicappedSpot => $"[♿ - {stateLabel}{vipLabel}]",
            _ => $"[Unknown Spot {SpotNumber}]"
        };
    }
}

