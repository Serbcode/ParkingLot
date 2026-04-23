namespace ParkingLotSystem.Vehicles;

public record Motorcycle(string LicensePlate) : Vehicle(LicensePlate, VehicleSize.Small);
