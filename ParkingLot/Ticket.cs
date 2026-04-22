namespace ParkingLotSystem;

public record Ticket(string TicketNumber, Vehicle Vehicle, ParkingSpot Spot, DateTime EntryTime, DateTime ExitTime);