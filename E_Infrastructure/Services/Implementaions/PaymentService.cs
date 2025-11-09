using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.Orders;
using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.Order;
using Application.Dtos.Payment;
using Application.Results;
using Application.Services.InterFaces;
using Application.Services.InterFaces.Mapping;
using E_Domain.Models;
using System.Transactions;

namespace E_Infrastructure.Services.Implementaions
{
    public class PaymentService : IPaymentService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMappingServices mappingServices;
        public PaymentService(IUnitOfWork unitOfWork, IMappingServices mappingServices)
        {
            this.unitOfWork = unitOfWork;
            this.mappingServices = mappingServices;

        }







        public async Task<Result<int>> Add(AddPaymentDto NewPayment)
        {
            try
            {
                Order order = await unitOfWork.Orders.GetByIdAsync(NewPayment.OrderId);
                if (order == null)
                    return Result<int>.Fail("NOT_FOUND", "Order Not Found");
                var ActualPayment = new Payment(order, NewPayment.PaymentMethod);
                await unitOfWork.Payment.AddAsync(ActualPayment);
                unitOfWork.Orders.Update(order);
                int Rowaffected = await unitOfWork.CompleteTask();
                if (Rowaffected > 0)
                {


                    return Result<int>.Ok(ActualPayment.Id, "Payment Added Successfully");
                }
                else
                {
                    return Result<int>.Fail("PAYMENT_FAILED", "Could not save the payment record.");
                }


            }
            catch(ArgumentNullException ex)
            {
                return Result<int>.Fail("EXCEPTION", ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Result<int>.Fail("EXCEPTION", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Result<int>.Fail("EXCEPTION", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("EXCEPTION", ex.Message);
            }
           

        }


    }
}
