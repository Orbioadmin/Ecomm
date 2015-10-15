
var pageName = (function () {
    var a = window.location.href,
        b = a.indexOf("/Admin/");
    return a.substr(b + 1);
}());
$(".nav li").removeClass("active");
if (pageName.indexOf("Catalog") > 0 || pageName.indexOf("Product") > 0 || pageName.indexOf("Attribute") > 0 || pageName.indexOf("Component") > 0)
{
    $("#catelog").addClass("active");
}
else if (pageName.indexOf("Order") > 0 || pageName.indexOf("Shipment") > 0 || pageName.indexOf("Sales") > 0 || pageName.indexOf("ShoppingCart") > 0 )
{
    $("#sales").addClass("active");
}
else if (pageName.indexOf("Customer") > 0)
{
    $("#customer").addClass("active");
}
else if (pageName.indexOf("Discount") > 0)
{
    $("#promotion").addClass("active");
}
else if (pageName.indexOf("MessageTemplate") > 0) {
    $("#contentmanagement").addClass("active");
}
else
{
    $("#dashboard").addClass("active");
}