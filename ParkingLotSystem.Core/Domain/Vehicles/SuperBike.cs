namespace ParkingLotSystem.Vehicles;

public record SuperBike : Vehicle
{
    public SuperBike(string LicensePlate, bool IsDirty = false, bool IsTracked = false)
        : base(LicensePlate, VehicleSize.SuperBike)
    {
        this.IsDirty = IsDirty;
        this.IsTracked = IsTracked;
    }
}