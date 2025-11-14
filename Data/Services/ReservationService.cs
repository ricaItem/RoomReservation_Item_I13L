using Microsoft.EntityFrameworkCore;
using RoomReservation_Item_I13L.Data.Entities;

namespace RoomReservation_Item_I13L.Data.Services;

public class ReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _context.Reservations
            .Where(r => r.Status != "Archived" && r.Status != "Deleted")
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Reservation?> GetReservationByIdAsync(int id)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Reservation>> SearchReservationsAsync(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
            return await GetAllReservationsAsync();

        return await _context.Reservations
            .Where(r => (r.Status != "Archived" && r.Status != "Deleted") &&
                       (r.CustomerName.Contains(searchQuery)
                || r.RoomName.Contains(searchQuery)
                || (r.Email != null && r.Email.Contains(searchQuery))
                || (r.ContactNumber != null && r.ContactNumber.Contains(searchQuery))))
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetReservationsByStatusAsync(string status)
    {
        if (status == "Archived" || status == "Deleted")
        {
            return await _context.Reservations
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
        else
        {
            return await _context.Reservations
                .Where(r => r.Status == status && r.Status != "Archived" && r.Status != "Deleted")
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }

    public async Task<List<Reservation>> GetReservationsByDateRangeAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Reservations
            .Where(r => r.Status != "Archived" && r.Status != "Deleted")
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(r => r.CheckInDate >= startDate.Value || r.CheckOutDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.CheckInDate <= endDate.Value || r.CheckOutDate <= endDate.Value);
        }

        return await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public async Task<List<Reservation>> GetReservationsByRoomTypeAsync(string roomType)
    {
        return await _context.Reservations
            .Where(r => (r.Status != "Archived" && r.Status != "Deleted"))
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Reservation>> FilterReservationsAsync(
        string? searchQuery = null,
        string? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? roomType = null)
    {
        var query = _context.Reservations.AsQueryable();

        if (status == "Archived" || status == "Deleted")
        {
            query = query.Where(r => r.Status == status);
        }
        else
        {
            query = query.Where(r => r.Status != "Archived" && r.Status != "Deleted");
        }

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(r => r.CustomerName.Contains(searchQuery)
                || r.RoomName.Contains(searchQuery)
                || (r.Email != null && r.Email.Contains(searchQuery))
                || (r.ContactNumber != null && r.ContactNumber.Contains(searchQuery)));
        }

        if (!string.IsNullOrWhiteSpace(status) && status != "Archived" && status != "Deleted")
        {
            query = query.Where(r => r.Status == status);
        }

        if (startDate.HasValue)
        {
            query = query.Where(r => r.CheckInDate >= startDate.Value || r.CheckOutDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.CheckInDate <= endDate.Value || r.CheckOutDate <= endDate.Value);
        }

        return await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public async Task<Reservation> AddReservationAsync(Reservation reservation)
    {
        if (string.IsNullOrWhiteSpace(reservation.RoomName))
            throw new Exception("Room name is required");
        
        if (string.IsNullOrWhiteSpace(reservation.CustomerName))
            throw new Exception("Customer name is required");
        
        if (reservation.CheckOutDate <= reservation.CheckInDate)
            throw new Exception("Check-out date must be after check-in date");

        reservation.CreatedAt = DateTime.Now;
        reservation.UpdatedAt = DateTime.Now;
        
        if (string.IsNullOrWhiteSpace(reservation.Status))
        {
            reservation.Status = "Pending";
        }
        
        if (string.IsNullOrWhiteSpace(reservation.PaymentStatus))
        {
            reservation.PaymentStatus = "Pending";
        }

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task UpdateReservationAsync(Reservation updatedReservation)
    {
        var existingReservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == updatedReservation.Id);
        if (existingReservation == null)
            throw new Exception("Reservation not found");

        if (updatedReservation.CheckOutDate <= updatedReservation.CheckInDate)
            throw new Exception("Check-out date must be after check-in date");

        existingReservation.RoomName = updatedReservation.RoomName;
        existingReservation.CustomerName = updatedReservation.CustomerName;
        existingReservation.ContactNumber = updatedReservation.ContactNumber;
        existingReservation.Email = updatedReservation.Email;
        existingReservation.CheckInDate = updatedReservation.CheckInDate;
        existingReservation.CheckOutDate = updatedReservation.CheckOutDate;
        existingReservation.Status = updatedReservation.Status;
        existingReservation.PaymentStatus = updatedReservation.PaymentStatus;
        existingReservation.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
    }

    public async Task ArchiveReservationAsync(int id)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
        if (reservation == null)
            throw new Exception("Reservation not found");

        reservation.Status = "Archived";
        reservation.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReservationAsync(int id)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
        if (reservation == null)
            throw new Exception("Reservation not found");

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetDistinctRoomTypesAsync()
    {
        return await _context.Rooms
            .Where(r => r.RoomType != null)
            .Select(r => r.RoomType!)
            .Distinct()
            .OrderBy(r => r)
            .ToListAsync();
    }
}

