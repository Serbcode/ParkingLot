namespace ParkingLotSystem.Vehicles;

public record Car : Vehicle
{
    public Car(string LicensePlate, bool IsDirty = false, bool IsTracked = false)
        : base(LicensePlate, VehicleSize.Medium)
    {
        this.IsDirty = IsDirty;
        this.IsTracked = IsTracked;
    }
}

