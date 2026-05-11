namespace ParkingLotSystem.Vehicles;

public record SuperBike(string LicensePlate) : Vehicle(LicensePlate, VehicleSize.SuperBike);