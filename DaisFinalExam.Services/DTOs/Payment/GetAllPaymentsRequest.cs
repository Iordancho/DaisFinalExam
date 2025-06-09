using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisFinalExam.Services.DTOs.Payment
{
    public class GetAllPaymentsRequest
    {
        public int UserId { get; set; }
        public string SortingParameter { get; set; }
    }
}
