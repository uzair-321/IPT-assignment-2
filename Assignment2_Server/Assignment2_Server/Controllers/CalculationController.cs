using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Assignment2_Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CalculationController : ControllerBase
    {
        [HttpPost]
        [Route("percentage")]
        public ResponseModel<PercentageModel> GetPercentage(RequestModel request)
        {
            if (!validateRequest(request))
            {
                return new ResponseModel<PercentageModel>{Code = HttpStatusCode.InternalServerError, Message = "Validation failed"};
            }

            var max = request.Subjects.FirstOrDefault(x => x.ObtainedMarks == request.Subjects.Max(x => x.ObtainedMarks));
            var min = request.Subjects.FirstOrDefault(x => x.ObtainedMarks == request.Subjects.Min(x => x.ObtainedMarks));

            if (min?.Name == max?.Name)
            {
                max = request.Subjects.OrderByDescending(x => x.Name).FirstOrDefault(x => x.ObtainedMarks == request.Subjects.Max(x => x.ObtainedMarks));
                min = request.Subjects.OrderBy(x => x.Name).FirstOrDefault(x => x.ObtainedMarks == request.Subjects.Min(x => x.ObtainedMarks));
            }

            var marks = Convert.ToDecimal(request.Subjects.Sum(x => x.ObtainedMarks)) / Convert.ToDecimal(request.Subjects.Count() * 100);
            var percentage = marks * 100;

            return new ResponseModel<PercentageModel>
            {
                Message = "Success",
                Code = HttpStatusCode.OK,
                Data = new PercentageModel()
                {
                    MaxMarkSubject = max,
                    MinMarkSubject = min,
                    Percentage = percentage
                }
            };
        }

        private bool validateRequest(RequestModel request)
        {
            if (request.Subjects.Any(x => string.IsNullOrEmpty(x.Name)))
                return false;

            if (request.Subjects.Any(x => x.ObtainedMarks > 100 || x.ObtainedMarks < 0))
                return false;

            return true;
        }
    }
}
