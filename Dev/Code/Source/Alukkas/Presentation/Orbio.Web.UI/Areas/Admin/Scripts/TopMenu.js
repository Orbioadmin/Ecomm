
var pageName = (function () {
    var a = window.location.href,
        b = a.indexOf("/Admin/");
    return a.substr(b + 1);
}());
$(".nav li").removeClass("active");
if (pageName == "Admin/Catalog/ManageCategories" || pageName == "Admin/Catalog/ManageManufacturer" || pageName == "Admin/Product/List" || pageName == "Admin/Product/Create" || pageName.indexOf("Product") > 0 || pageName == "Admin/Attribute/ProductAttribute" || pageName == "Admin/Attribute/ListSpecificationAttribute")
{
    $("#catelog").addClass("active");
}
else if (pageName == "Admin/Component/ProductComponent" || pageName == "Admin/Component/PriceComponent") {
    $("#catelog").addClass("active");
}
else if (pageName == "Admin/Order/List" || pageName == "Admin/Shipment/List" || pageName == "Admin/Sales/GiftCards" || pageName == "Admin/ShoppingCart/CurrentCarts" || pageName == "Admin/ShoppingCart/CurrentWishLists" || pageName == "Admin/Order/BestSellersReport" || pageName == "Admin/Order/NeverSoldReport")
{
    $("#sales").addClass("active");
}
else if (pageName == "Admin/Customer/ListCustomer" || pageName == "Admin/Customer/AddCustomer" || pageName == "Admin/Customer/ListCustomerRole" || pageName == "Admin/Customer/CustomerReport" || pageName == "Admin/Customer/NewsletterSubscribers")
{
    $("#customer").addClass("active");
}
else if (pageName == "Admin/Discount/List" || pageName == "Admin/Discount/Create")
{
    $("#promotion").addClass("active");
}
else if (pageName == "Admin/MessageTemplate/List" || pageName == "Admin/MessageTemplate/Add") {
    $("#contentmanagement").addClass("active");
}
else if (pageName.indexOf("Tax") > 0 || pageName.indexOf("Shipping/ShippingMethods") > 0 || pageName.indexOf("Shipping/DeliveryDate") > 0 || pageName.indexOf("Shipping/Warehouses") > 0 || pageName.indexOf("Shipping/AddWarehouse") > 0 || pageName.indexOf("Shipping/EditWarehouse") > 0)
{
    $("#configuration").addClass("active");
}
else
{
    $("#dashboard").addClass("active");
}