﻿namespace Shopping_Tutorial.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode {  get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status {  get; set; }
    }
}
