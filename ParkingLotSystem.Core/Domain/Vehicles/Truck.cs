namespace ParkingLotSystem.Vehicles;

public record Truck : Vehicle
{
    public Truck(string LicensePlate, bool IsDirty = false, bool IsTracked = false)
        : base(LicensePlate, VehicleSize.Large)
    {
        this.IsDirty = IsDirty;
        this.IsTracked = IsTracked;
    }
}
