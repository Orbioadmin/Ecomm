using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Orbio.Core;
using Orbio.Services.Security;
using System;
using System.Collections.Generic;
using System.Reflection;
using OBS = Orbio.Services;
namespace Orbio.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //added by madhu mb 
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerHttpRequest();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerHttpRequest();
            builder.RegisterType<WebStoreContext>().As<IStoreContext>().InstancePerHttpRequest();

            //added by roshni
            builder.RegisterType<OBS.Checkout.CheckoutService>().As<OBS.Checkout.ICheckoutService>().InstancePerHttpRequest();

            builder.RegisterType<OBS.Orders.OrderService>().As<OBS.Orders.IOrderService>().InstancePerHttpRequest();
            //added by roshni
            builder.RegisterType<OBS.Email.EmailService>().As<OBS.Email.IEmailService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Orders.ShoppingCartService>().As<OBS.Orders.IShoppingCartService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Common.GenericAttributeService>().As<OBS.Common.IGenericAttributeService>().InstancePerHttpRequest();
            //added by madhu
            builder.RegisterType<OBS.Orders.PriceCalculationService>().As<OBS.Orders.IPriceCalculationService>().InstancePerHttpRequest();
              //added by madhu
            builder.RegisterType<OBS.Checkout.TransientCartService>().As<OBS.Checkout.ITransientCartService>().InstancePerHttpRequest();
            //added by madhu
            builder.RegisterType<OBS.Taxes.TaxCalculationService>().As<OBS.Taxes.ITaxCalculationService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Helpers.DateTimeHelper>().As<OBS.Helpers.IDateTimeHelper>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Orders.OrderReportService>().As<OBS.Admin.Orders.IOrderReportService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Customers.CustomerReportService>().As<OBS.Admin.Customers.ICustomerReportService>().InstancePerHttpRequest();
            //added by madhu
            builder.RegisterType<OBS.Logging.DefaultLogger>().As<OBS.Logging.ILogger>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Orders.OrderService>().As<OBS.Admin.Orders.IOrderService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Address.AddressService>().As<OBS.Admin.Address.IAddressService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Directory.CountryService>().As<OBS.Directory.ICountryService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Directory.StateProvinceService>().As<OBS.Directory.IStateProvinceService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Orders.OrderProcessingService>().As<OBS.Admin.Orders.IOrderProcessingService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Orders.ShoppingCartService>().As<OBS.Admin.Orders.IShoppingCartService>().InstancePerHttpRequest();
            
            builder.RegisterType<OBS.Admin.Catalog.CategoryServices>().As<OBS.Admin.Catalog.ICategoryServices>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Catalog.ManufacturerService>().As<OBS.Admin.Catalog.IManufacturerService>().InstancePerHttpRequest();
            //added by roshni
            builder.RegisterType<OBS.Admin.Attributes.ProductAttributeService>().As<OBS.Admin.Attributes.IProductAttributeService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Orders.ShippingService>().As<OBS.Admin.Orders.IShippingService>().InstancePerHttpRequest();
            //added by roshni
            builder.RegisterType<OBS.Admin.Attributes.SpecificationAttributeService>().As<OBS.Admin.Attributes.ISpecificationAttributeService>().InstancePerHttpRequest();
            //added by roshni
            builder.RegisterType<OBS.Admin.Attributes.CheckoutAttributeService>().As<OBS.Admin.Attributes.ICheckoutAttributeService>().InstancePerHttpRequest();
            //added by roshni
            builder.RegisterType<OBS.Admin.Components.ProductComponentService>().As<OBS.Admin.Components.IProductComponentService>().InstancePerHttpRequest();
            //added by roshni
            builder.RegisterType<OBS.Admin.Components.PriceComponentService>().As<OBS.Admin.Components.IPriceComponentService>().InstancePerHttpRequest();
            //added by roshni
            builder.RegisterType<OBS.Admin.Customers.CustomerRoleService>().As<OBS.Admin.Customers.ICustomerRoleService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Products.ProductService>().As<OBS.Admin.Products.IProductService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Admin.Tax.TaxCategoryService>().As<OBS.Admin.Tax.ITaxCategoryService>().InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 0; }
        }
    }


    //public class SettingsSource : IRegistrationSource
    //{
    //    static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
    //        "BuildRegistration",
    //        BindingFlags.Static | BindingFlags.NonPublic);

    //    public IEnumerable<IComponentRegistration> RegistrationsFor(
    //            Service service,
    //            Func<Service, IEnumerable<IComponentRegistration>> registrations)
    //    {
    //        var ts = service as TypedService;
    //        if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
    //        {
    //            var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
    //            yield return (IComponentRegistration)buildMethod.Invoke(null, null);
    //        }
    //    }

    //    static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
    //    {
    //        return RegistrationBuilder
    //            .ForDelegate((c, p) =>
    //            {
    //                //var currentStoreId = c.Resolve<IStoreContext>().CurrentStore.Id;
    //                //uncomment the code below if you want load settings per store only when you have two stores installed.
    //                //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
    //                //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

    //                //although it's better to connect to your database and execute the following SQL:
    //                //DELETE FROM [Setting] WHERE [StoreId] > 0
    //                //return c.Resolve<ISettingService>().LoadSetting<TSettings>(currentStoreId);
    //                return c.Resolve<IStoreContext>().CurrentStore.Id;
    //            })
    //            .InstancePerHttpRequest()
    //            .CreateRegistration();
    //    }

    //    public bool IsAdapterForIndividualComponents { get { return false; } }
    //}


}
