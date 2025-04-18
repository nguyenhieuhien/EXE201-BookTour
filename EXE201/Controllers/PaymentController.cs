using EXE201.Controllers.DTO.Payment;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IBookingService _bookingService;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PayOSController> _logger;
        private readonly IConfiguration _configuration;

        public PayOSController(IConfiguration configuration, IBookingService bookingService, IPaymentService paymentService, ILogger<PayOSController> logger)
        {
            _configuration = configuration;
            string clientId = configuration["PayOS:ClientId"];
            string apiKey = configuration["PayOS:ApiKey"];
            string checksumKey = configuration["PayOS:ChecksumKey"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(checksumKey))
            {
                throw new ArgumentException("PayOS configuration is missing or invalid.");
            }

            _payOS = new PayOS(clientId, apiKey, checksumKey);
            _bookingService = bookingService;
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("create-payment/{bookingId}")]
        public async Task<IActionResult> CreatePayment(long bookingId)
        {
            try
            {
                var booking = await _bookingService.GetBookingById(bookingId);
                if (booking == null)
                {
                    _logger.LogWarning("Booking not found for ID: {BookingId}", bookingId);
                    return NotFound(new { message = "Không tìm thấy đơn đặt." });
                }

                if (booking.TotalPrice <= 0)
                {
                    _logger.LogWarning("Invalid TotalPrice for booking ID: {BookingId}, TotalPrice: {TotalPrice}", bookingId, booking.TotalPrice);
                    return BadRequest(new { message = "Giá trị thanh toán phải lớn hơn 0." });
                }

                var items = new List<ItemData>
                {
                    new ItemData($"Booking {booking.Id} - {booking.Description}", 1, (int)booking.TotalPrice)
                };

                var customOrderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var paymentData = new PaymentData(
                    orderCode: customOrderCode,
                    amount: (int)booking.TotalPrice,
                    description: $"Thanh toán Booking {booking.Id}",
                    items: items,
                    cancelUrl: _configuration["PayOS:CancelUrl"],
                    returnUrl: _configuration["PayOS:ReturnUrl"]
                );

                var createPayment = await _payOS.createPaymentLink(paymentData);
                if (createPayment == null || string.IsNullOrEmpty(createPayment.checkoutUrl))
                {
                    _logger.LogError("Failed to create payment link for booking ID: {BookingId}", bookingId);
                    return BadRequest(new { message = "Lỗi khi tạo link thanh toán" });
                }

                var payment = new Models.Payment
                {
                    BookingId = booking.Id,
                    OrderCode = customOrderCode,
                    Amount = (int)booking.TotalPrice,
                    Description = paymentData.description,
                    Items = $"Booking {booking.Id} - {booking.Description}",
                    CancelUrl = paymentData.cancelUrl,
                    ReturnUrl = paymentData.returnUrl,
                    PaymentLink = createPayment.checkoutUrl,
                    Status = "PENDING",
                    CreatedAt = DateTime.UtcNow
                };

                await _paymentService.CreatePaymentAsync(payment);
                _logger.LogInformation("Payment created successfully for booking ID: {BookingId}, OrderCode: {OrderCode}", bookingId, customOrderCode);

                return Ok(new
                {
                    bookingId = booking.Id,
                    orderCode = customOrderCode,
                    checkoutUrl = createPayment.checkoutUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment for booking ID: {BookingId}", bookingId);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPut("update")]

        public async Task<IActionResult> ConfirmPayOsTransaction(PaymentDTO paymentDTO)
        {


            // Kiểm tra trạng thái thanh toán
            var transactionInfo = await _payOS.getPaymentLinkInformation(paymentDTO.OrderCode);
            

            if  (transactionInfo.status == "CANCELLED") {
                var payment = await _paymentService.GetByOrderCodeAsync(paymentDTO.OrderCode);
                payment.Status = "CANCELLED";
                await _paymentService.UpdatePaymentAsync(payment);
                return Ok(new { message = "Đã cập nhật trạng thái" });

            }
            if (transactionInfo.status == "PAID")
            {
                var payments = await _paymentService.GetByOrderCodeAsync(paymentDTO.OrderCode);
                payments.Status = "PAID";
                await _paymentService.UpdatePaymentAsync(payments);
                return Ok(new { message = "Đã cập nhật trạng thái" });
            }


            return BadRequest(new { message = "Đã thanh toán" });

        }
    }
}


                //[HttpPost("webhook")]
                //public async Task<IActionResult> HandleWebhook([FromBody] WebhookType webhookBody)
                //{
                //    try
                //    {
                //        // Xác minh webhook từ PayOS
                //        var verifiedData = _payOS.verifyPaymentWebhookData(webhookBody);
                //        if (verifiedData == null)
                //        {
                //            _logger.LogWarning("Invalid webhook data or signature for OrderCode: {OrderCode}", webhookBody?.data?.orderCode);
                //            return BadRequest(new { message = "Dữ liệu webhook không hợp lệ hoặc chữ ký không khớp." });
                //        }

                //        // Tìm kiếm thanh toán theo OrderCode
                //        var payment = await _paymentService.GetByOrderCodeAsync(verifiedData.orderCode);
                //        if (payment == null)
                //        {
                //            _logger.LogWarning("Payment not found for OrderCode: {OrderCode}", verifiedData.orderCode);
                //            return NotFound(new { message = "Không tìm thấy thanh toán." });
                //        }

                //        // Kiểm tra trạng thái thanh toán thành công
                //        if (webhookBody.code == "00" && webhookBody.success)
                //        {
                //            // Chỉ cập nhật nếu trạng thái hiện tại không phải là "PAID"
                //            if (payment.Status != "PAID")
                //            {
                //                payment.Status = "PAID";
                //                await _paymentService.UpdatePaymentAsync(payment);
                //                _logger.LogInformation("Payment status updated to PAID for OrderCode: {OrderCode}", verifiedData.orderCode);
                //            }
                //            else
                //            {
                //                _logger.LogInformation("Payment already in PAID status for OrderCode: {OrderCode}", verifiedData.orderCode);
                //            }
                //            return Ok(new { message = "Cập nhật trạng thái thanh toán thành công." });
                //        }

                //        // Trường hợp không phải thanh toán thành công
                //        _logger.LogInformation("Webhook processed but no status update for OrderCode: {OrderCode}", verifiedData.orderCode);
                //        return Ok(new { message = "Webhook đã được xử lý." });
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, "Error processing webhook for OrderCode: {OrderCode}", webhookBody?.data?.orderCode);
                //        return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
                //    }
                //}
            