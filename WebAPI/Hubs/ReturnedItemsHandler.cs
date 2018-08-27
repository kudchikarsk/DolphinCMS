﻿using DA.SharedKernel.Interfaces;
using DA.StockManagement.Core.Models;
using DA.StockManagement.Data;
using DA.StockManagement.Data.Repositories;
using DA.TonerJobManagement.Core.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Hubs
{
    public class ReturnedItemsHandler : IHandle<ReturnedItemsEvent>
    {
        IRepository<StockItem> repository;

        public ReturnedItemsHandler(IRepository<StockItem> repository)
        {
            this.repository = repository;
        }

        public void Handle(ReturnedItemsEvent args)
        {
            foreach (var item in args.ReturnedItems)
            {
                var stockItem = repository.GetByID(item.StockItem.Id);
                stockItem.ReturnedQuantity(item.Quantity);
                repository.Update(stockItem);
            }
        }
    }
}