using System;

namespace DeliveryClient.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}


//https://www.c-sharpcorner.com/UploadFile/3d39b4/crud-operations-using-linq-to-sql-in-mvc/