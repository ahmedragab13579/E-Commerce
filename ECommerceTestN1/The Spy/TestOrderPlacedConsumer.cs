using MassTransit;
using SharedMessages; // 🛑 تأكد من استيراد المشروع الذي يحتوي على OrderPlacedEvent
using System.Threading.Tasks;

namespace ECommerceTestN1.The_Spy
{
    /// <summary>
    /// هذا "جاسوس" أو مستهلك مزيف. كل وظيفته هي الاستماع لرسالة
    /// OrderPlacedEvent وإرسال "إشارة" عند استلامها.
    /// </summary>
    public class TestOrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        // 1. هذه هي "الإشارة" السحرية.
        // TaskCompletionSource هو وسيلة لإنشاء "مهمة" (Task) يمكننا التحكم فيها يدويًا.
        // سنجعلها static حتى نتمكن من الوصول إليها من كلاس الاختبار.
        private static TaskCompletionSource<OrderPlacedEvent> _messageReceivedTcs = new();

        /// <summary>
        /// هذا هو الـ "Task" الذي سينتظره الاختبار.
        /// </summary>
        public static Task<OrderPlacedEvent> MessageReceived => _messageReceivedTcs.Task;

        /// <summary>
        /// يتم استدعاء هذه الميثود تلقائيًا بواسطة MassTransit عند وصول رسالة.
        /// </summary>
        public Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            // 2. عند استلام الرسالة، نقوم بـ "إطلاق الإشارة"
            // ونرسل الرسالة (context.Message) معها.
            // أي اختبار كان "ينتظر" (await) الـ Task، سيكمل الآن.
            _messageReceivedTcs.SetResult(context.Message);

            return Task.CompletedTask;
        }

        /// <summary>
        /// نحتاج إلى ميثود لـ "إعادة ضبط" الإشارة قبل كل اختبار.
        /// </summary>
        public static void Reset()
        {
            // أنشئ إشارة جديدة للاختبار التالي
            _messageReceivedTcs = new TaskCompletionSource<OrderPlacedEvent>();
        }
    }
}