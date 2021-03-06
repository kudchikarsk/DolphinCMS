﻿using AutoMapper;
using C=DA.ClientManagement.Core.Models;
using DA.SharedKernel;
using S=DA.StockManagement.Core.Models;
using T=DA.TonerJobManagement.Core.Aggregates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAPI.Models;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = IOC.Initialize();
            DomainEvents.Container = container;
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);

            Mapper.Initialize(cfg => {
                cfg.CreateMap<C.Client, ClientViewModel>();
                cfg.CreateMap<C.Toner, TonerViewModel>();
                cfg.CreateMap<C.Employee, EmployeeViewModel>();
                cfg.CreateMap<S.StockItem, StockItemViewModel>();
                cfg.CreateMap<T.TonerJob, TonerJobViewModel>();
                cfg.CreateMap<T.PurchaseItem, PurchaseItemViewModel>();
                cfg.CreateMap<T.StockItem, StockItemViewModel>();
                cfg.CreateMap<T.Toner, TonerViewModel>();
            });
        }
    }
}
