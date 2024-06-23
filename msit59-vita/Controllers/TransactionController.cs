using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using msit59_vita.Models;
using static System.TimeZoneInfo;

namespace msit59_vita.Controllers
{
    public class TransactionController : Controller
    {

        private readonly VitaContext _context;

        public TransactionController(VitaContext context)
        {
            _context = context;
		}

        [HttpPost]
        public async Task<List<TransactionRecord>> InitTransactionRecord(int OrderId, string TransactionId, int TransactionType, string Timestamp, bool TransactionResult)
        //public async Task<List<TransactionRecord>> InitTransactionRecord(int OrderId =10000422, string TransactionId = "2024062302146385910", int TransactionType = 1,string Timestamp = "1719136121956", bool TransactionResult = false)
        {
            TransactionRecord transactionRecord = new TransactionRecord
            {
                //TransactionRecordsId = 1,
                OrderId = OrderId,
                TransactionResult = TransactionResult,
                TransactionTime = DateTime.Now,
                TransactionTimestamp = Timestamp,
                TransactionType = (byte)TransactionType,
                TransactionId = TransactionId

            };

            _context.TransactionRecords.Add(transactionRecord);
            _context.SaveChanges();


            var query = from t in _context.TransactionRecords
                        where t.TransactionResult == TransactionResult && t.TransactionId == TransactionId
                        select t;
			return await query.ToListAsync();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
