using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PizzaOderingAppAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PizzaOderingAppAPI.Services;

public class PdfInvoiceService : IInvoiceService
{
    private readonly ApplicationDbContext _context;

    public PdfInvoiceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> GenerateInvoiceAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            throw new ArgumentException("Order not found");

        using var memoryStream = new MemoryStream();
        var writer = new PdfWriter(memoryStream);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        document.Add(new Paragraph($"Invoice #{order.Id}"));
        document.Add(new Paragraph($"Date: {order.OrderDate:d}"));
        document.Add(new Paragraph($"Customer: {order.User.Username}"));
        
        // Add order items
        var table = new Table(4);
        table.AddCell("Item");
        table.AddCell("Quantity");
        table.AddCell("Price");
        table.AddCell("Subtotal");

        foreach (var item in order.OrderItems)
        {
            table.AddCell(item.MenuItem.Name);
            table.AddCell(item.Quantity.ToString());
            table.AddCell($"${item.UnitPrice:F2}");
            table.AddCell($"${item.Subtotal:F2}");
        }

        document.Add(table);
        document.Add(new Paragraph($"Total Amount: ${order.TotalAmount:F2}"));
        
        document.Close();
        return memoryStream.ToArray();
    }
}
