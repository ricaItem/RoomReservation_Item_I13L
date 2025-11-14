using Microsoft.EntityFrameworkCore;
using RoomReservation_Item_I13L.Data.Entities;

namespace RoomReservation_Item_I13L.Data.Services;

public class RoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _context.Rooms
            .Where(r => r.Status != "Archived" && r.Status != "Deleted")
            .OrderBy(r => r.RoomName)
            .ToListAsync();
    }

    public async Task<Room?> GetRoomByIdAsync(int id)
    {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Room> AddRoomAsync(Room room)
    {
        if (string.IsNullOrWhiteSpace(room.RoomName))
            throw new Exception("Room name is required");
        
        if (string.IsNullOrWhiteSpace(room.RoomType))
            throw new Exception("Room type is required");
        
        if (room.Capacity <= 0)
            throw new Exception("Capacity must be greater than 0");

        room.CreatedAt = DateTime.Now;
        room.UpdatedAt = DateTime.Now;
        
        if (string.IsNullOrWhiteSpace(room.Status))
        {
            room.Status = "Available";
        }

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task UpdateRoomAsync(Room updatedRoom)
    {
        var existingRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == updatedRoom.Id);
        if (existingRoom == null)
            throw new Exception("Room not found");

        existingRoom.RoomName = updatedRoom.RoomName;
        existingRoom.RoomType = updatedRoom.RoomType;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.Status = updatedRoom.Status;
        existingRoom.Description = updatedRoom.Description;
        existingRoom.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
    }

    public async Task ArchiveRoomAsync(int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null)
            throw new Exception("Room not found");

        room.Status = "Archived";
        room.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRoomAsync(int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null)
            throw new Exception("Room not found");

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Room>> SearchRoomsAsync(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
            return await GetAllRoomsAsync();

        return await _context.Rooms
            .Where(r => (r.Status != "Archived" && r.Status != "Deleted") &&
                       (r.RoomName.Contains(searchQuery) 
                || (r.RoomType != null && r.RoomType.Contains(searchQuery))
                || (r.Description != null && r.Description.Contains(searchQuery))))
            .OrderBy(r => r.RoomName)
            .ToListAsync();
    }

    public async Task<List<Room>> GetRoomsByStatusAsync(string status)
    {
        if (status == "Archived" || status == "Deleted")
        {
            return await _context.Rooms
                .Where(r => r.Status == status)
                .OrderBy(r => r.RoomName)
                .ToListAsync();
        }
        else
        {
            return await _context.Rooms
                .Where(r => r.Status == status && r.Status != "Archived" && r.Status != "Deleted")
                .OrderBy(r => r.RoomName)
                .ToListAsync();
        }
    }

    public async Task<List<string>> GetAvailableRoomNamesAsync()
    {
        return await _context.Rooms
            .Where(r => r.Status == "Available")
            .Select(r => r.RoomName)
            .OrderBy(r => r)
            .ToListAsync();
    }
}
