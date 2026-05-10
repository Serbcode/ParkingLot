namespace ParkingLotSystem.Vehicles;

public record Car(string LicensePlate) : Vehicle(LicensePlate, VehicleSize.Medium);
