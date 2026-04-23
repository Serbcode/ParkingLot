namespace ParkingLotSystem.Vehicles;

public record Truck(string LicensePlate) : Vehicle(LicensePlate, VehicleSize.Large);
