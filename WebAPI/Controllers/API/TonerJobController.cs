﻿using AutoMapper;
using DA.ClientManagement.Data;
using DA.SharedKernel.Interfaces;
using DA.TonerJobManagement.Core.Aggregates.Models;
using DA.TonerJobManagement.Core.Interfaces;
using DA.TonerJobManagement.Data;
using DA.TonerJobManagement.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers.API
{
    public class TonerJobController : ApiController
    {
        public IRepository<TonerJob> repository;
        private readonly ITonerRepository tonerRepository;
        private readonly IStockItemRepository stockItemRepository;

        public TonerJobController(IRepository<TonerJob> repository,ITonerRepository tonerRepository, IStockItemRepository stockItemRepository)
        {
            this.repository = repository;
            this.tonerRepository = tonerRepository;
            this.stockItemRepository = stockItemRepository;
        }

        // GET: api/TonerJob
        public IEnumerable<TonerJobViewModel> Get(DateTime dateTime=new DateTime())
        {
            var tonerJobs=repository.Get(filter: (t) => t.In >= dateTime && t.Out <= dateTime, includeProperties: "PurchasedItems,Toners");
            return tonerJobs.Select(t=>t.ToViewModel());
        }

        // GET: api/Employee/5
        public TonerJobViewModel Get(int id)
        {
            var tonerJob = repository.GetByID(id);
            return tonerJob.ToViewModel();
        }

        // POST: api/Employee
        public TonerJobViewModel Post([FromBody]TonerJobViewModel value)
        {
            var tonerJob = TonerJob.Create(
                value.ClientId,
                GetToners(value.Toners),
                value.CollectedById,
                value.DeliveredById,
                value.In,
                value.Out,
                CreatePurchasedItems(value.PurchasedItems),
                value.Remarks       ,
                value.OtherCharges  
                );
            repository.Insert(tonerJob);
            return tonerJob.ToViewModel();
        }

        // PUT: api/Employee/5
        public TonerJobViewModel Put(long id, [FromBody]TonerJobViewModel value)
        {
            var tonerJob = repository.GetByID(value.Id);
            tonerJob.UpdateAmount(value.Target);
            tonerJob.UpdateIn(value.In);
            tonerJob.UpdateOut(value.Out);
            tonerJob.UpdatePurchaseItems(CreatePurchasedItems(value.PurchasedItems));
            repository.Update(tonerJob);
            return tonerJob.ToViewModel();
        }

        // DELETE: api/Employee/5
        public void Delete(int id)
        {
            repository.Delete(id);
        }

        private List<Toner> GetToners(List<TonerViewModel> toners)
        {
            return toners.Select(t => tonerRepository.GetTonerById(t.Id)).ToList();
        }

        private List<PurchaseItem> CreatePurchasedItems(List<PurchaseItemViewModel> purchasedItems)
        {
            return purchasedItems.Select(p => PurchaseItem.Create(stockItemRepository.GetStockItemById(p.StockItem.Id),p.Quantity)).ToList();
        }
    }

    public static class TonerJobExtension
    {
        public static TonerJobViewModel ToViewModel(this TonerJob tonerJob) {
            var clientContext = new ClientContext();
            var tonerJobViewModel = Mapper.Map<TonerJobViewModel>(tonerJob);
            tonerJobViewModel.ClientName = clientContext.Clients.Where(c => c.Id == tonerJob.ClientId).First().Name;
            return tonerJobViewModel;
        }
    }
}