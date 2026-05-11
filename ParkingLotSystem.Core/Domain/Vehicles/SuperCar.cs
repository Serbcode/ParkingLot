namespace ParkingLotSystem.Vehicles;

public record SuperCar(string LicensePlate) : Vehicle(LicensePlate, VehicleSize.SuperCar);