using System.Net.Http.Headers; // (مهم للتوكن)
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using SharedMessages;
using FluentAssertions;
using Application.Dtos.Login;
using Application.Dtos.CartItem;
using ECommerceTestN1.The_Fixtures;
using ECommerceTestN1.The_Spy;

namespace ECommerceTestN1.TheTestScenario
{
    public class CheckoutScenarioTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public CheckoutScenarioTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task FullScenario_Login_AddToCart_And_Checkout_ShouldPublishEvent()
        {
            // ----- 1. Arrange (التجهيز) -----

            // 1.1: إعادة ضبط "الجاسوس" ليكون جاهزًا لاستقبال إشارة جديدة
            TestOrderPlacedConsumer.Reset();

            // 1.2: تجهيز الـ DTOs للطلبات
            var loginRequest = new LoginDto { UserName = "testuser@example.com", Password = "Password123" }; // 🛑 عدّل هذا
            var addToCartRequest = new AddCartItemDto { ProductId = 1, Quantity = 2 }; // 🛑 عدّل هذا
            var checkoutRequest = new { }; // 🛑 عدّل هذا

            // ----- 2. Act 1: تسجيل الدخول (Login) -----
            // (افترض أن لديك Register Endpoint إذا كانت الداتا بيز لا تحتوي على مستخدم جاهز)
            var loginResponse = await _httpClient.PostAsJsonAsync("/api/shop/login", loginRequest); // 🛑 عدّل المسار
            loginResponse.EnsureSuccessStatusCode();

            // 2.1: قراءة التوكن من الرد
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>(); // 🛑 عدّل الـ DTO
            var token = loginResult.Token;

            // ----- 3. Act 2: إضافة التوكن للطلبات القادمة -----
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // ----- 4. Act 3: إضافة المنتج للكارت (Endpoint محمي) -----
            var cartResponse = await _httpClient.PostAsJsonAsync("/api/shop/Cart/AddItem", addToCartRequest); // 🛑 عدّل المسار
            cartResponse.EnsureSuccessStatusCode();

            // ----- 5. Act 4: تأكيد الطلب (Checkout) (Endpoint محمي) -----
            var checkoutResponse = await _httpClient.PostAsJsonAsync("/api/shop/order/CheckOUt", checkoutRequest); // 🛑 عدّل المسار

            // ----- 6. Assert (التأكيد) -----

            // 6.1: التأكد من نجاح الـ API
            checkoutResponse.EnsureSuccessStatusCode();

            // 6.2: التأكد من أن "الجاسوس" استلم الرسالة (انتظار 10 ثواني كحد أقصى)
            OrderPlacedEvent receivedMessage;
            try
            {
                receivedMessage = await TestOrderPlacedConsumer.MessageReceived.WaitAsync(TimeSpan.FromSeconds(10));
            }
            catch (TimeoutException)
            {
                // فشل الاختبار: الرسالة لم تصل في الوقت المحدد
                throw new Exception("Test failed: Message was not received from RabbitMQ within the timeout period.");
            }

            // 6.3: التأكد من أن بيانات الرسالة صحيحة
            receivedMessage.Should().NotBeNull();
            receivedMessage.OrderId.Should().BeGreaterThan(0); // (لأننا لا نعرف الـ ID بالضبط)
            receivedMessage.UserName.Should().Be("testuser"); // (من بيانات الـ Seeding)
        }

        // كلاس DTO وهمي للمساعدة في قراءة رد اللوجن
        private class LoginResponseDto
        {
            public string Token { get; set; }
            // ... (أضف أي خصائص أخرى يرجعها الـ Login)
        }
    }
}