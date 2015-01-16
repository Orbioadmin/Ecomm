$(document).ready(function () {
    $(".nav li").children("ul").hide();
    $(".nav li").hover(function () {
        $(this).children("ul").stop(true, true).delay(800).slideDown(100);
    }, function () {
        $(this).children("ul").stop(true, true).slideUp(100);
    });
    $('.containerBlock').hover(function () {
        $('.dropdown-menu').hide();
    });

   

    $(".increase").click(function () {
        var b = $(this).parents(".quantity_main").find(".quantityBox").val();
        b++;
        $(this).parents(".quantity_main").find(".quantityBox").val(b);
        var a = $(this).parents(".quantity_main").find(".sub_total_price").html();
        var c = basicprice * b;
        $(this).parents(".quantity_main").find(".sub_total_price").html(c)
    });
    $(".decrease").click(function () {
        var b = $(this).parents(".quantity_main").find(".quantityBox").val();
        if (b > 1) {
            b--;
            $(this).parents(".quantity_main").find(".quantityBox").val(b);
            var a = $(this).parents(".quantity_main").find(".sub_total_price").html();
            var c = basicprice * b;
            $(this).parents(".quantity_main").find(".sub_total_price").html(c)
        }
    });
    $(".colorSelected").click(function () {
        $(this).parent(".optionValues").find(".selectedOption").removeClass("selectedOption");
        $(this).addClass("selectedOption")
    });
    $(".colorSelected2").click(function () {
        $(this).parent(".optionValues").find(".selectedOption").removeClass("selectedOption");
        $(this).addClass("selectedOption")
    });
    $("#themesBtn").click(function () {
        $("#secectionBox").animate({right: "0"}, 500, function () {
        });
        $("#themesBtn").animate({right: "-80"}, 100, function () {
        })
    });
    $("#hideme").click(function () {
        $("#secectionBox").animate({right: "-999"}, 500, function () {
        });
        $("#themesBtn").animate({right: "0"}, 700, function () {
        })
    });
    $("#myTab a").click(function () {
        $("#myTab a").removeClass("active");
        $(this).addClass("active")
    });
    $(".jqzoom").jqzoom({zoomType: "standard", lens: true, preloadImages: false, alwaysOn: false})
});
$(document.body).on("click", ".dropdown-menu li", function (b) {

    var a = $(b.currentTarget);
    a.closest(".btn-group").find('[data-bind="label"]').text(a.text()).end().children(".dropdown-toggle").dropdown("toggle");
    return false
});
$(document).ready(function () {
    var a = setInterval(function () {
        if ($(".loading").isOnScreen()) {
            var b = $('<li class="span3 listItem"><div class="thumbnail pull-left"><a href="detailPage.html"><img  src="common/images/temp/mobile-4.jpg" class="imageNormal" data-alt-src="common/images/temp/mobile-5.jpg"  alt="image"></a><div class="caption pull-left"><span class="actualPrice">4,300.00 AED -</span><span class="offPrice">30%<sup>OFF</sup></span><span class="sellingPrice">3,300.00 AED</span><span class="productDesc pull-left">Apple iphone 5s(64GB, 4G LTE + WiFi, Space Gray)</span><div class="pull-left rating"><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"> </span> <span class="icon-star yellow pull-left"> </span><span class="icon-star gray pull-left"></span><p class="pull-left">(67 ratings)</p></div><a href="#" class="buttonShopNow"><span class="iconShopNow"></span>SHOP NOW</a></div></div><div class="quickView"><a class="btn btn-primary" href="#quickView" data-toggle="modal">Quick View</a></div></li>').hide();
            $("#grid").append(b);
            $(".listItem").fadeIn(2000)
        } else {
            clearInterval(a)
        }
    }, 500)
});
jQuery(window).scroll(function () {
    if ($(".loading").isOnScreen()) {
        var a = $('<li class="span3 listItem"><div class="thumbnail pull-left"><a href="detailPage.html"><img  src="common/images/temp/mobile-4.jpg" class="imageNormal" data-alt-src="common/images/temp/mobile-5.jpg"  alt="image"></a><div class="caption pull-left"><span class="actualPrice">4,300.00 AED -</span><span class="offPrice">30%<sup>OFF</sup></span><span class="sellingPrice">3,300.00 AED</span><span class="productDesc pull-left">Apple iphone 5s(64GB, 4G LTE + WiFi, Space Gray)</span><div class="pull-left rating"><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"> </span> <span class="icon-star yellow pull-left"> </span><span class="icon-star gray pull-left"></span><p class="pull-left">(67 ratings)</p></div><a href="#" class="buttonShopNow"><span class="iconShopNow"></span>SHOP NOW</a></div></div><div class="quickView"><a class="btn btn-primary" href="#quickView" data-toggle="modal">Quick View</a></div></li>').hide();
        $(".listItem").fadeIn(2000);
        $("#grid").append(a)
    }
});
$.fn.isOnScreen = function () {
    var b = $(window);
    var a = {top: b.scrollTop()};
    a.bottom = b.scrollTop() + b.height();
    var c = this.offset();
    c.bottom = c.top + this.outerHeight();
    return !(a.bottom < c.top || a.top > c.bottom)
};
$(document).ready(function () {
    var a = setInterval(function () {
        if ($(".loading").isOnScreen()) {
            var b = $('<li class="span12 gridItem"><div class="thumbnail pull-left"><a href="detailPage.html"><img  src="common/images/temp/mobile-4.jpg" class="imageNormal" data-alt-src="common/images/temp/mobile-5.jpg"  alt="image"></a><div class="caption pull-left"><span class="productDesc pull-left">Apple iphone 5s(64GB, 4G LTE + WiFi, Space Gray)</span><div class="pull-left rating"><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"></span><span class="icon-star gray pull-left"></span><p class="pull-left">(67 ratings)</p></div><p class="pull-left productDescription">Newer, faster, better A 64-bit architecture chip, fingerprint identity technology, the finest optic engineering, and fully loaded OS come together to make the iPhone 5S. The phone is eons ahead of its time, and is quite the technological marvel. The colors of the iPhone 5S have been chosen based on how they stand in a worldly view- how much of the color is seen around us, how appealing is it to the human eye.</p><div class="offerBlock pull-left"><div class="offerBlockInside span3 pull-left"><span class="actualPrice pull-left">4,300.00 AED -</span><span class="offPrice pull-left">30%<sup>OFF</sup></span><span class="sellingPrice pull-left clearfix">3,300.00 AED</span></div><a href="#" class="buttonShopNow"><span class="iconShopNow"></span>SHOP NOW</a></div></div></div><div class="quickView"><a class="btn btn-primary" href="#quickView" data-toggle="modal">Quick View</a></div><div class="surpriseGift"><img src="common/images/surprise-gifit-bg.png" alt="image"><span class="giftBox"></span></div></li>').hide();
            $("#list").append(b);
            $(".gridItem").fadeIn(2000)
        } else {
            clearInterval(a)
        }
    }, 500)
});
jQuery(window).scroll(function () {
    if ($(".loading").isOnScreen()) {
        var a = $('<li class="span12 gridItem"><div class="thumbnail pull-left"><a href="detailPage.html"><img  src="common/images/temp/mobile-4.jpg" class="imageNormal" data-alt-src="common/images/temp/mobile-5.jpg"  alt="image"></a><div class="caption pull-left"><span class="productDesc pull-left">Apple iphone 5s(64GB, 4G LTE + WiFi, Space Gray)</span><div class="pull-left rating"><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"></span><span class="icon-star yellow pull-left"></span><span class="icon-star gray pull-left"></span><p class="pull-left">(67 ratings)</p></div><p class="pull-left productDescription">Newer, faster, better A 64-bit architecture chip, fingerprint identity technology, the finest optic engineering, and fully loaded OS come together to make the iPhone 5S. The phone is eons ahead of its time, and is quite the technological marvel. The colors of the iPhone 5S have been chosen based on how they stand in a worldly view- how much of the color is seen around us, how appealing is it to the human eye.</p><div class="offerBlock pull-left"><div class="offerBlockInside span3 pull-left"><span class="actualPrice pull-left">4,300.00 AED -</span><span class="offPrice pull-left">30%<sup>OFF</sup></span><span class="sellingPrice pull-left clearfix">3,300.00 AED</span></div><a href="#" class="buttonShopNow"><span class="iconShopNow"></span>SHOP NOW</a></div></div></div><div class="quickView"><a class="btn btn-primary" href="#quickView" data-toggle="modal">Quick View</a></div><div class="surpriseGift"><img src="common/images/surprise-gifit-bg.png" alt="image"><span class="giftBox"></span></div></li>').hide();
        $(".gridItem").fadeIn(2000);
        $("#list").append(a)
    }
});
$.fn.isOnScreen = function () {
    var b = $(window);
    var a = {top: b.scrollTop()};
    a.bottom = b.scrollTop() + b.height();
    var c = this.offset();
    c.bottom = c.top + this.outerHeight();
    return !(a.bottom < c.top || a.top > c.bottom)
};
var sourceSwap = function () {
    var b = $(this);
    var a = b.data("alt-src");
    b.data("alt-src", b.attr("src"));
    b.attr("src", a)
};
$("img.imageNormal").live("hover",sourceSwap, sourceSwap);


