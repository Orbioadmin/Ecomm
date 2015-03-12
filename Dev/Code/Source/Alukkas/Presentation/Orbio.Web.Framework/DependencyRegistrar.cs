using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Orbio.Core;
using Orbio.Services.Security;
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

            builder.RegisterType<OBS.Checkout.CheckoutService>().As<OBS.Checkout.ICheckoutService>().InstancePerHttpRequest();
            //added by sankar
            builder.RegisterType<OBS.Orders.ShoppingCartService>().As<OBS.Orders.IShoppingCartService>().InstancePerHttpRequest();
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
    //                var currentStoreId = c.Resolve<IStoreContext>().CurrentStore.Id;
    //                //uncomment the code below if you want load settings per store only when you have two stores installed.
    //                //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
    //                //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

    //                //although it's better to connect to your database and execute the following SQL:
    //                //DELETE FROM [Setting] WHERE [StoreId] > 0
    //                return c.Resolve<ISettingService>().LoadSetting<TSettings>(currentStoreId);
    //            })
    //            .InstancePerHttpRequest()
    //            .CreateRegistration();
    //    }

    //    public bool IsAdapterForIndividualComponents { get { return false; } }
    //}

}
