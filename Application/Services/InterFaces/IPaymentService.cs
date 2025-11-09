using Application.Dtos.Payment;
using Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces
{
    public interface IPaymentService
    {
        Task <Result<int>> Add(AddPaymentDto Payment);

    }
}
