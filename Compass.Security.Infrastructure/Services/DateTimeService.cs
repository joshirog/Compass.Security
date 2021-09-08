using System;
using Compass.Security.Application.Commons.Interfaces;

namespace Compass.Security.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}