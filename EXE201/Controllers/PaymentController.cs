using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using System.Text.Json;
using System.Threading.Tasks;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSController : ControllerBase
    {
        private readonly PayOS _payOS;

        public PayOSController(IConfiguration configuration)
        {
            string clientId = configuration["PayOS:ClientId"];
            string apiKey = configuration["PayOS:ApiKey"];
            string checksumKey = configuration["PayOS:ChecksumKey"];
            _payOS = new PayOS(clientId, apiKey, checksumKey);
        }


        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment()
        {
            try
            {
                var item = new ItemData("1 anh Kaveh 6 múi yêu bạn", 1, 2000); // 100,000 VND
                var items = new List<ItemData> { item };

                var paymentData = new PaymentData(
                    orderCode: 5, // Mã đơn hàng
                    amount: 2000, // Số tiền
                    description: "Chuyen Tien Cho Hien ", // Giới hạn 25 ký tự
                    items: items,
                    cancelUrl: "https://www.youtube.com",
                    returnUrl: "https://www.facebook.com/NguyenHieuHien.Profile"
                );

                var createPayment = await _payOS.createPaymentLink(paymentData);

                if (createPayment == null || string.IsNullOrEmpty(createPayment.checkoutUrl))
                {
                    return BadRequest(new { message = "Lỗi khi tạo link thanh toán" });
                }

                return Ok(new { checkoutUrl = createPayment.checkoutUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                var receivedData = JsonSerializer.Deserialize<WebhookResponse>(body);

                if (receivedData == null || string.IsNullOrEmpty(receivedData.orderCode))
                {
                    return BadRequest("Dữ liệu webhook không hợp lệ");
                }

                // Kiểm tra trạng thái thanh toán
                if (receivedData.status == "PAID")
                {
                    // TODO: Cập nhật trạng thái đơn hàng trong database
                    return Ok(new { message = "Thanh toán thành công" });
                }

                return BadRequest("Thanh toán thất bại hoặc chưa hoàn tất");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }
    }

    public class WebhookResponse
    {
        public string orderCode { get; set; }
        public string status { get; set; }
    }
}
