namespace ParkingLotSystem;

public abstract record Vehicle(string LicensePlate, VehicleSize Size);

public enum VehicleSize
{
    Small = 1,
    Medium = 2,
    Large = 3
}

public record Truck(string LicensePlate) : Vehicle(LicensePlate, VehicleSize.Large);

public record Motorcycle(string LicensePlate) : Vehicle(LicensePlate, VehicleSize.Small);

public record Car(string LicensePlate) : Vehicle(LicensePlate, VehicleSize.Medium);
