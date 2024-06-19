using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class TransactionController : Controller
    {

        private readonly VitaContext _context;

        public TransactionController(VitaContext context)
        {
            _context = context;
        }


        public async Task<List<TransactionRecord>> InitTransactionRecord(int OrderId, string TransactionId,int TransactionType,string Timestamp,bool TransactionResult) 
        {
            var query = from t in _context.TransactionRecords
                        select t;

			TransactionRecord transactionRecord = new TransactionRecord
            {
                OrderId = OrderId,
                TransactionResult = TransactionResult,
                TransactionTime = DateTime.Now,
                TransactionTimestamp = Timestamp,
                TransactionType= (byte)TransactionType,
                TransactionId = TransactionId

			};


            _context.TransactionRecords.Add(transactionRecord);
            _context.SaveChanges();
			return await query.ToListAsync();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
