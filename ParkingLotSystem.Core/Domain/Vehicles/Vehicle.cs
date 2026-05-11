namespace ParkingLotSystem.Vehicles;

public abstract record Vehicle(string LicensePlate, VehicleSize Size)
{
    public bool IsDirty { get; set; } = false;
    public bool IsTracked { get; set; } = false;
}

