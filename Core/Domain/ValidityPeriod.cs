﻿namespace Core.Domain
{
    public class ValidityPeriod
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public ValidityPeriod(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
