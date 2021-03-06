﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class TonerJobViewModel
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string ClientName { get; set; }
        public long CollectedById { get; set; }
        public string CollectedByName { get; set; }
        public long DeliveredById { get; set; }
        public string DeliveredByName { get; set; }
        public string Remarks { get; set; }
        public int OtherCharges { get; set; }
        public double Discount { get; set; }

        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public DateTime In { get; set; }
        public DateTime Out { get; set; }
        public List<PurchaseItemViewModel> PurchasedItems { get; set; }
        public List<TonerViewModel> Toners { get; set; }

        public int Target
        {
            get
            {
                var grossTotal = PurchasedItems.Sum(s => s.StockItem.SellingPrice * s.StockItem.Quantity) + OtherCharges;
                return (int)Math.Ceiling(grossTotal - (Discount * grossTotal));
            }
        }

    }
}