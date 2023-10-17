using GyMeApplication.Models.InternalManagement;
using Stripe.Checkout;

namespace GyMeApplication.IServices;

public interface IPaymentService
{
    Task<Session> CreateRedirectToPayment(PaymentRequestModel paymentRequest);
    Task WebhookStripePayments();
}