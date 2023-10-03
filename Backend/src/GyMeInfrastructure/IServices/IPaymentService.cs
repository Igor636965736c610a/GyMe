using GyMeInfrastructure.Models.InternalManagement;
using Stripe.Checkout;

namespace GyMeInfrastructure.IServices;

public interface IPaymentService
{
    Task<Session> CreateRedirectToPayment(PaymentRequestModel paymentRequest);
    Task WebhookStripePayments();
}