using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using msit59_vita.Models.ViewModels;

namespace msit59_vita.Controllers
{
	public class NotifyController : Controller
	{
		private readonly VitaContext _context;
		private List<NotifyViewModel> _nowNotifyList;

		public NotifyController(VitaContext context) { 
			_context = context;
			_nowNotifyList = NotifyList.NowNotifyList ?? new List<NotifyViewModel>();
		}

		public IActionResult GetNotifyList()
		{
			if (!User.Identity?.IsAuthenticated ?? false)
			{
				//未登入狀態
				return PartialView("_NotifyPartial", new List<NotifyViewModel>());
			}

			var query = from c in _context.Customers
						where c.CustomerEmail == User.Identity.Name
						select c.CustomerId;
			int NowCustomerId = query.FirstOrDefault();

			var customerMessages = from m in _context.OrderMessages
								   join o in _context.Orders on m.OrderId equals o.OrderId
								   join c in _context.Customers on o.CustomerId equals c.CustomerId
								   where c.CustomerId == NowCustomerId
								   orderby m.MessageId descending
								   select new NotifyViewModel
								   {
									   OrderId = m.OrderId,
									   MessageInformedTime = m.MessageInformedTime,
									   MessageContent = m.MessageContent,
									   MessageStatus = m.MessageStatus,
									   DeliveryVia = o.OrderDeliveryVia
								   };
			_nowNotifyList = customerMessages.ToList();
			NotifyList.NowNotifyList = _nowNotifyList;
            ViewBag.NotifyList = _nowNotifyList;
            return PartialView("_NotifyPartial", _nowNotifyList);
		}

		[HttpPost]
		public IActionResult SetRead()
		{
			var query = from c in _context.Customers
						where c.CustomerEmail == User.Identity.Name
						select c.CustomerId;
			int NowCustomerId = query.FirstOrDefault();

			var messages = from m in _context.OrderMessages
						   join o in _context.Orders on m.OrderId equals o.OrderId
						   where o.CustomerId == NowCustomerId
						   select m;

			foreach (var message in messages)
			{
				message.MessageStatus = true;
			}

			_context.UpdateRange(messages);
			_context.SaveChanges();

			return RedirectToAction("getNotifyList");
		}

		public int GetUnreadNum()
		{
			return _nowNotifyList.Where(m => m.MessageStatus == false).Count();
		}
	}
}
