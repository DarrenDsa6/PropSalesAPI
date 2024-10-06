using System;
using System.Collections.Generic;
using PropertySales.Models.Domain;

namespace PropertySales.Models.DTO
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int PropertyId { get; set; }
        public int BuyerId { get; set; }
        public int BrokerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
    }

    public class CreateTransactionDto
    {
        public int PropertyId { get; set; }
        public int BuyerId { get; set; }
        public int BrokerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
    }

    public class UpdateTransactionDto
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
