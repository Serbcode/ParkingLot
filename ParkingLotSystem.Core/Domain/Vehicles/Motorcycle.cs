namespace ParkingLotSystem.Vehicles;

public record Motorcycle : Vehicle
{
    public Motorcycle(string LicensePlate, bool IsDirty = false, bool IsTracked = false)
        : base(LicensePlate, VehicleSize.Small)
    {
        this.IsDirty = IsDirty;
        this.IsTracked = IsTracked;
    }
}



