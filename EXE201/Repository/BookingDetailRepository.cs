using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EXE201.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repositories;

public class BookingDetailRepository : IBookingDetailRepository
{
    private readonly EXE201Context _context;

    public BookingDetailRepository(EXE201Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookingDetail>> GetAllBookingDetails()
    {
        return await _context.BookingDetails.Include(bd => bd.Booking)
                                            .Include(bd => bd.Package)
                                            .ToListAsync();
    }

    public async Task<BookingDetail?> GetBookingDetailById(long id)
    {
        return await _context.BookingDetails.Include(bd => bd.Booking)
                                            .Include(bd => bd.Package)
                                            .FirstOrDefaultAsync(bd => bd.Id == id);
    }

    public async Task<IEnumerable<BookingDetail>> GetBookingDetailsByBookingId(long bookingId)
    {
        return await _context.BookingDetails.Include(bd => bd.Package)
                                            .Where(bd => bd.BookingId == bookingId)
                                            .ToListAsync();
    }

    public async Task AddBookingDetail(BookingDetail bookingDetail)
    {
        await _context.BookingDetails.AddAsync(bookingDetail);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookingDetail(BookingDetail bookingDetail)
    {
        _context.BookingDetails.Update(bookingDetail);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookingDetail(long id)
    {
        var bookingDetail = await _context.BookingDetails.FindAsync(id);
        if (bookingDetail != null)
        {
            _context.BookingDetails.Remove(bookingDetail);
            await _context.SaveChangesAsync();
        }
    }
}
