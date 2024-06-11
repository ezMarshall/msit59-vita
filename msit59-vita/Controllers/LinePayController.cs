using msit59_vita.Dtos;
using msit59_vita.Domain;
using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LinePayController : ControllerBase
    {
        private readonly LinePayService _linePayService;
        public LinePayController()
        {
            _linePayService = new LinePayService();
        }

        [HttpPost("Create")]
        public async Task<PaymentResponseDto> CreatePayment(PaymentRequestDto dto)
        {
            return await _linePayService.SendPaymentRequest(dto);
        }

        [HttpPost("Confirm")]
        public async Task<PaymentConfirmResponseDto> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId, PaymentConfirmDto dto)
        {
            return await _linePayService.ConfirmPayment(transactionId, orderId, dto);
        }

        [HttpGet("CheckRegKey/{regKey}")]
        public async Task<PaymentConfirmResponseDto> CheckRegKey(string regKey)
        {
            return await _linePayService.CheckRegKey(regKey);
        }

        [HttpPost("PayPreapproved/{regKey}")]
        public async Task<PaymentConfirmResponseDto> PayPreapproved([FromRoute] string regKey, PayPreapprovedDto dto)
        {
            return await _linePayService.PayPreapproved(regKey, dto);
        }

        [HttpPost("ExpireRegKey/{regKey}")]
        public async Task<PaymentConfirmResponseDto> ExpireRegKey(string regKey)
        {
            return await _linePayService.ExpireRegKey(regKey);
        }

        [HttpGet("Cancel")]
        public async void CancelTransaction([FromQuery] string transactionId)
        {
            _linePayService.TransactionCancel(transactionId);
        }
    }
}

