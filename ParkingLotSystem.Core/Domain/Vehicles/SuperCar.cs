namespace ParkingLotSystem.Vehicles;

public record SuperCar : Vehicle
{
    public SuperCar(string LicensePlate, bool IsDirty = false, bool IsTracked = false)
        : base(LicensePlate, VehicleSize.SuperCar)
    {
        this.IsDirty = IsDirty;
        this.IsTracked = IsTracked;
    }
}
