(function ($) {
    var isIE6 = $.browser.msie && $.browser.version < 7;
    var body = $(document.body);
    var window = $(window);
    var jqzoompluging_disabled = false;
    $.fn.jqzoom = function (e) {
        return this.each(function () {
            var t = this.nodeName.toLowerCase();
            if (t == "a") {
                new jqzoom(this, e)
            }
        })
    };
    jqzoom = function (el, options) {
        function Smallimage(image) {
            var $obj = this;
            this.node = image[0];
            this.findborder = function () {
                var bordertop = 0;
                bordertop = image.css("border-top-width");
                btop = "";
                var borderleft = 0;
                borderleft = image.css("border-left-width");
                bleft = "";
                if (bordertop) {
                    for (i = 0; i < 3; i++) {
                        var x = [];
                        x = bordertop.substr(i, 1);
                        if (isNaN(x) == false) {
                            btop = btop + "" + bordertop.substr(i, 1)
                        } else {
                            break
                        }
                    }
                }
                if (borderleft) {
                    for (i = 0; i < 3; i++) {
                        if (!isNaN(borderleft.substr(i, 1))) {
                            bleft = bleft + borderleft.substr(i, 1)
                        } else {
                            break
                        }
                    }
                }
                $obj.btop = btop.length > 0 ? eval(btop) : 0;
                $obj.bleft = bleft.length > 0 ? eval(bleft) : 0
            };
            this.fetchdata = function () {
                $obj.findborder();
                $obj.w = image.width();
                $obj.h = image.height();
                $obj.ow = image.outerWidth();
                $obj.oh = image.outerHeight();
                $obj.pos = image.offset();
                $obj.pos.l = image.offset().left + $obj.bleft;
                $obj.pos.t = image.offset().top + $obj.btop;
                $obj.pos.r = $obj.w + $obj.pos.l;
                $obj.pos.b = $obj.h + $obj.pos.t;
                $obj.rightlimit = image.offset().left + $obj.ow;
                $obj.bottomlimit = image.offset().top + $obj.oh
                var marginTop = (450 / 2) - ($obj.h / 2);
                var Top = (450 / 2) - ($obj.h / 2);
                $('.zoomPad').css({
                    'width': $obj.w,
//                    'margin-top': marginTop
                });
                $('.zoomWindow').css({
//                    'top': -Top
                });
            };
            this.node.onerror = function () {
                alert("Problems while loading image.");
                throw "Problems while loading image."
            };
            this.node.onload = function () {
                $obj.fetchdata();
                if ($(".zoomPad", el).length == 0) obj.create()
            };
            return $obj
        }

        function Loader() {
            var e = this;
            this.append = function () {
                this.node = $("<div/>").addClass("zoomPreload").css("visibility", "hidden").html(settings.preloadText);
                $(".zoomPad", el).append(this.node)
            };
            this.show = function () {
                this.node.top = (smallimage.oh - this.node.height()) / 2;
                this.node.left = (smallimage.ow - this.node.width()) / 2;
                this.node.css({
                    top: this.node.top,
                    left: this.node.left,
                    position: "absolute",
                    visibility: "visible"
                })
            };
            this.hide = function () {
                this.node.css("visibility", "hidden")
            };
            return this
        }

        function Lens() {
            var e = this;
            this.node = $("<div/>").addClass("zoomPup");
            this.append = function () {
                $(".zoomPad", el).append($(this.node).hide());
                if (settings.zoomType == "reverse") {
                    this.image = new Image;
                    this.image.src = smallimage.node.src;
                    $(this.node).empty().append(this.image)
                }
            };
            this.setdimensions = function () {
                this.node.w = parseInt(settings.zoomWidth / el.scale.x) > smallimage.w ? smallimage.w : parseInt(settings.zoomWidth / el.scale.x);
                this.node.h = parseInt(settings.zoomHeight / el.scale.y) > smallimage.h ? smallimage.h : parseInt(settings.zoomHeight / el.scale.y);
                this.node.top = (smallimage.oh - this.node.h - 2) / 2;
                this.node.left = (smallimage.ow - this.node.w - 2) / 2;
                this.node.css({
                    top: 0,
                    left: 0,
                    width: this.node.w + "px",
                    height: this.node.h + "px",
                    position: "absolute",
                    display: "none",
                    borderWidth: 1 + "px"
                });
                if (settings.zoomType == "reverse") {
                    this.image.src = smallimage.node.src;
                    $(this.node).css({
                        opacity: 1
                    });
                    $(this.image).css({
                        position: "absolute",
                        display: "block",
                        left: -(this.node.left + 1 - smallimage.bleft) + "px",
                        top: -(this.node.top + 1 - smallimage.btop) + "px"
                    })
                }
            };
            this.setcenter = function () {
                this.node.top = (smallimage.oh - this.node.h - 2) / 2;
                this.node.left = (smallimage.ow - this.node.w - 2) / 2;
                this.node.css({
                    top: this.node.top,
                    left: this.node.left
                });
                if (settings.zoomType == "reverse") {
                    $(this.image).css({
                        position: "absolute",
                        display: "block",
                        left: -(this.node.left + 1 - smallimage.bleft) + "px",
                        top: -(this.node.top + 1 - smallimage.btop) + "px"
                    })
                }
                largeimage.setposition()
            };
            this.setposition = function (e) {
                function r(e) {
                    return el.mousepos.x - e.w / 2 < smallimage.pos.l
                }

                function i(e) {
                    return el.mousepos.x + e.w / 2 > smallimage.pos.r
                }

                function s(e) {
                    return el.mousepos.y - e.h / 2 < smallimage.pos.t
                }

                function o(e) {
                    return el.mousepos.y + e.h / 2 > smallimage.pos.b
                }
                el.mousepos.x = e.pageX;
                el.mousepos.y = e.pageY;
                var t = 0;
                var n = 0;
                t = el.mousepos.x + smallimage.bleft - smallimage.pos.l - (this.node.w + 2) / 2;
                n = el.mousepos.y + smallimage.btop - smallimage.pos.t - (this.node.h + 2) / 2;
                if (r(this.node)) {
                    t = smallimage.bleft - 1
                } else if (i(this.node)) {
                    t = smallimage.w + smallimage.bleft - this.node.w - 1
                }
                if (s(this.node)) {
                    n = smallimage.btop - 1
                } else if (o(this.node)) {
                    n = smallimage.h + smallimage.btop - this.node.h - 1
                }
                this.node.left = t;
                this.node.top = n;
                this.node.css({
                    left: t + "px",
                    top: n + "px"
                });
                if (settings.zoomType == "reverse") {
                    if ($.browser.msie && $.browser.version > 7) {
                        $(this.node).empty().append(this.image)
                    }
                    $(this.image).css({
                        position: "absolute",
                        display: "block",
                        left: -(this.node.left + 1 - smallimage.bleft) + "px",
                        top: -(this.node.top + 1 - smallimage.btop) + "px"
                    })
                }
                largeimage.setposition()
            };
            this.hide = function () {
                img.css({
                    opacity: 1
                });
                this.node.hide()
            };
            this.show = function () {
                if (settings.zoomType != "innerzoom" && (settings.lens || settings.zoomType == "drag")) {
                    this.node.show()
                }
                if (settings.zoomType == "reverse") {
                    img.css({
                        opacity: settings.imageOpacity
                    })
                }
            };
            this.getoffset = function () {
                var t = {};
                t.left = e.node.left;
                t.top = e.node.top;
                return t
            };
            return this
        }

        function Stage() {
            var e = this;
            this.node = $("<div class='zoomWindow'><div class='zoomWrapper'><div class='zoomWrapperTitle'></div><div class='zoomWrapperImage'></div></div></div>");
            this.ieframe = $('<iframe class="zoomIframe" src="javascript:\'\';" marginwidth="0" marginheight="0" align="bottom" scrolling="no" frameborder="0" ></iframe>');
            this.setposition = function () {
                this.node.leftpos = 0;
                this.node.toppos = 0;
                if (settings.zoomType != "innerzoom") {
                    switch (settings.position) {
                        case "left":
                            this.node.leftpos = smallimage.pos.l - smallimage.bleft - Math.abs(settings.xOffset) - settings.zoomWidth > 0 ? 0 - settings.zoomWidth - Math.abs(settings.xOffset) : smallimage.ow + Math.abs(settings.xOffset);
                            this.node.toppos = Math.abs(settings.yOffset);
                            break;
                        case "top":
                            this.node.leftpos = Math.abs(settings.xOffset);
                            this.node.toppos = smallimage.pos.t - smallimage.btop - Math.abs(settings.yOffset) - settings.zoomHeight > 0 ? 0 - settings.zoomHeight - Math.abs(settings.yOffset) : smallimage.oh + Math.abs(settings.yOffset);
                            break;
                        case "bottom":
                            this.node.leftpos = Math.abs(settings.xOffset);
                            this.node.toppos = smallimage.pos.t - smallimage.btop + smallimage.oh + Math.abs(settings.yOffset) + settings.zoomHeight < screen.height ? smallimage.oh + Math.abs(settings.yOffset) : 0 - settings.zoomHeight - Math.abs(settings.yOffset);
                            break;
                        default:
                            this.node.leftpos = smallimage.rightlimit + Math.abs(settings.xOffset) + settings.zoomWidth < screen.width ? smallimage.ow + Math.abs(settings.xOffset) : 0 - settings.zoomWidth - Math.abs(settings.xOffset);
                            this.node.toppos = Math.abs(settings.yOffset);
                            break
                    }
                }
                this.node.css({
                    left: this.node.leftpos + "px",
                    top: this.node.toppos + -10 + "px"
                });
                return this
            };
            this.append = function () {
                $(".zoomPad", el).append(this.node);
                this.node.css({
                    position: "absolute",
                    display: "none",
                    zIndex: 5001
                });
                if (settings.zoomType == "innerzoom") {
                    this.node.css({
                        cursor: "default"
                    });
                    var t = smallimage.bleft == 0 ? 1 : smallimage.bleft;
                    $(".zoomWrapper", this.node).css({
                        borderWidth: t + "px"
                    })
                }
                $(".zoomWrapper", this.node).css({
                    width: Math.round(settings.zoomWidth) + "px",
                    borderWidth: t + "px"
                });
                $(".zoomWrapperImage", this.node).css({
                    width: "100%",
                    height: Math.round(settings.zoomHeight) + "px"
                });
                $(".zoomWrapperTitle", this.node).css({
                    width: "100%",
                    position: "absolute"
                });
                $(".zoomWrapperTitle", this.node).hide();
                if (settings.title && zoomtitle.length > 0) {
                    $(".zoomWrapperTitle", this.node).html(zoomtitle).show()
                }
                e.setposition()
            };
            this.hide = function () {
                switch (settings.hideEffect) {
                    case "fadeout":
                        this.node.fadeOut(settings.fadeoutSpeed, function () {});
                        break;
                    default:
                        this.node.hide();
                        break
                }
                this.ieframe.hide()
            };
            this.show = function () {
                switch (settings.showEffect) {
                    case "fadein":
                        this.node.fadeIn();
                        this.node.fadeIn(settings.fadeinSpeed, function () {});
                        break;
                    default:
                        this.node.show();
                        break
                }
                if (isIE6 && settings.zoomType != "innerzoom") {
                    this.ieframe.width = this.node.width();
                    this.ieframe.height = this.node.height();
                    this.ieframe.left = this.node.leftpos;
                    this.ieframe.top = this.node.toppos;
                    this.ieframe.css({
                        display: "block",
                        position: "absolute",
                        left: this.ieframe.left,
                        top: this.ieframe.top,
                        zIndex: 99,
                        width: this.ieframe.width + "px",
                        height: this.ieframe.height + "px"
                    });
                    $(".zoomPad", el).append(this.ieframe);
                    this.ieframe.show()
                }
            }
        }

        function Largeimage() {
            var e = this;
            this.node = new Image;
            this.loadimage = function (e) {
                loader.show();
                this.url = e;
                this.node.style.position = "absolute";
                this.node.style.border = "0px";
                this.node.style.display = "none";
                this.node.style.left = "-5000px";
                this.node.style.top = "0px";
                document.body.appendChild(this.node);
                this.node.src = e
            };
            this.fetchdata = function () {
                var t = $(this.node);
                var n = {};
                this.node.style.display = "block";
                e.w = t.width();
                e.h = t.height();
                e.pos = t.offset();
                e.pos.l = t.offset().left;
                e.pos.t = t.offset().top;
                e.pos.r = e.w + e.pos.l;
                e.pos.b = e.h + e.pos.t;
                n.x = e.w / smallimage.w;
                n.y = e.h / smallimage.h;
                el.scale = n;
                document.body.removeChild(this.node);
                $(".zoomWrapperImage", el).empty().append(this.node);
                lens.setdimensions()
            };
            this.node.onerror = function () {
                alert("Problems while loading the big image.");
                throw "Problems while loading the big image."
            };
            this.node.onload = function () {
                e.fetchdata();
                loader.hide();
                el.largeimageloading = false;
                el.largeimageloaded = true;
                if (settings.zoomType == "drag" || settings.alwaysOn) {
                    lens.show();
                    stage.show();
                    lens.setcenter()
                }
            };
            this.setposition = function () {
                var e = -el.scale.x * (lens.getoffset().left - smallimage.bleft + 1);
                var t = -el.scale.y * (lens.getoffset().top - smallimage.btop + 1);
                $(this.node).css({
                    left: e + "px",
                    top: t + "px"
                })
            };
            return this
        }
        var api = null;
        api = $(el).data("jqzoom");
        if (api) return api;
        var obj = this;
        var settings = $.extend({}, $.jqzoom.defaults, options || {});
        obj.el = el;
        el.rel = $(el).attr("rel");
        el.zoom_active = false;
        el.zoom_disabled = false;
        el.largeimageloading = false;
        el.largeimageloaded = false;
        el.scale = {};
        el.timer = null;
        el.mousepos = {};
        el.mouseDown = false;
        $(el).css({
            "outline-style": "none",
            "text-decoration": "none"
        });
        var img = $("img:eq(0)", el);
        el.title = $(el).attr("title");
        el.imagetitle = img.attr("title");
        var zoomtitle = $.trim(el.title).length > 0 ? el.title : el.imagetitle;
        var smallimage = new Smallimage(img);
        var lens = new Lens;
        var stage = new Stage;
        var largeimage = new Largeimage;
        var loader = new Loader;
        $(el).bind("click", function (e) {
            e.preventDefault();
            return false
        });
        var zoomtypes = ["standard", "drag", "innerzoom", "reverse"];
        if ($.inArray($.trim(settings.zoomType), zoomtypes) < 0) {
            settings.zoomType = "standard"
        }
        $.extend(obj, {
            create: function () {
                if ($(".zoomPad", el).length == 0) {
                    el.zoomPad = $("<div/>").addClass("zoomPad");
                    img.wrap(el.zoomPad)
                }
                if (settings.zoomType == "innerzoom") {
                    settings.zoomWidth = smallimage.w;
                    settings.zoomHeight = smallimage.h
                }
                if ($(".zoomPup", el).length == 0) {
                    lens.append()
                }
                if ($(".zoomWindow", el).length == 0) {
                    stage.append()
                }
                if ($(".zoomPreload", el).length == 0) {
                    loader.append()
                }
                if (settings.preloadImages || settings.zoomType == "drag" || settings.alwaysOn) {
                    obj.load()
                }
                obj.init()
            },
            init: function () {
                if (settings.zoomType == "drag") {
                    $(".zoomPad", el).mousedown(function () {
                        el.mouseDown = true
                    });
                    $(".zoomPad", el).mouseup(function () {
                        el.mouseDown = false
                    });
                    document.body.ondragstart = function () {
                        return false
                    };
                    $(".zoomPad", el).css({
                        cursor: "default"
                    });
                    $(".zoomPup", el).css({
                        cursor: "move"
                    })
                }
                if (settings.zoomType == "innerzoom") {
                    $(".zoomWrapper", el).css({
                        cursor: "crosshair"
                    })
                }
                $(".zoomPad", el).bind("mouseenter mouseover", function (e) {
                    img.attr("title", "");
                    $(el).attr("title", "");
                    el.zoom_active = true;
                    smallimage.fetchdata();
                    if (el.largeimageloaded) {
                        obj.activate(e)
                    } else {
                        obj.load()
                    }
                });
                $(".zoomPad", el).bind("mouseleave", function (e) {
                    obj.deactivate()
                });
                $(".zoomPad", el).bind("mousemove", function (e) {
                    if (e.pageX > smallimage.pos.r || e.pageX < smallimage.pos.l || e.pageY < smallimage.pos.t || e.pageY > smallimage.pos.b) {
                        lens.setcenter();
                        return false
                    }
                    el.zoom_active = true;
                    if (el.largeimageloaded && !$(".zoomWindow", el).is(":visible")) {
                        obj.activate(e)
                    }
                    if (el.largeimageloaded && (settings.zoomType != "drag" || settings.zoomType == "drag" && el.mouseDown)) {
                        lens.setposition(e)
                    }
                });
                var thumb_preload = new Array;
                var i = 0;
                var thumblist = new Array;
                thumblist = $("a").filter(function () {
                    var e = new RegExp("gallery[\\s]*:[\\s]*'" + $.trim(el.rel) + "'", "i");
                    var t = $(this).attr("rel");
                    if (e.test(t)) {
                        return this
                    }
                });
                if (thumblist.length > 0) {
                    var first = thumblist.splice(0, 1);
                    thumblist.push(first)
                }
                thumblist.each(function () {
                    if (settings.preloadImages) {
                        var thumb_options = $.extend({}, eval("(" + $.trim($(this).attr("rel")) + ")"));
                        thumb_preload[i] = new Image;
                        thumb_preload[i].src = thumb_options.largeimage;
                        i++
                    }
                    $(this).click(function (e) {
                        if ($(this).hasClass("zoomThumbActive")) {
                            return false
                        }
                        thumblist.each(function () {
                            $(this).removeClass("zoomThumbActive")
                        });
                        e.preventDefault();
                        obj.swapimage(this);
                        return false
                    })
                })
            },
            load: function () {
                if (el.largeimageloaded == false && el.largeimageloading == false) {
                    var e = $(el).attr("href");
                    el.largeimageloading = true;
                    largeimage.loadimage(e)
                }
            },
            activate: function (e) {
                clearTimeout(el.timer);
                lens.show();
                stage.show()
            },
            deactivate: function (e) {
                switch (settings.zoomType) {
                    case "drag":
                        break;
                    default:
                        img.attr("title", el.imagetitle);
                        $(el).attr("title", el.title);
                        if (settings.alwaysOn) {
                            lens.setcenter()
                        } else {
                            stage.hide();
                            lens.hide()
                        }
                        break
                }
                el.zoom_active = false
            },
            swapimage: function (link) {
                el.largeimageloading = false;
                el.largeimageloaded = false;
                var options = new Object;
                options = $.extend({}, eval("(" + $.trim($(link).attr("rel")) + ")"));
                if (options.smallimage && options.largeimage) {
                    var smallimage = options.smallimage;
                    var largeimage = options.largeimage;
                    $(link).addClass("zoomThumbActive");
                    $(el).attr("href", largeimage);
                    img.attr("src", smallimage);
                    lens.hide();
                    stage.hide();
                    obj.load()
                } else {
                    alert("ERROR :: Missing parameter for largeimage or smallimage.");
                    throw "ERROR :: Missing parameter for largeimage or smallimage."
                }
                return false
            }
        });
        if (img[0].complete) {
            smallimage.fetchdata();
            if ($(".zoomPad", el).length == 0) obj.create()
        }
        $(el).data("jqzoom", obj)
    };
    $.jqzoom = {
        defaults: {
            zoomType: "standard",
            zoomWidth: 300,
            zoomHeight: 485,
            xOffset: 10,
            yOffset: 0,
            position: "right",
            preloadImages: true,
            preloadText: "Loading zoom",
            title: true,
            lens: true,
            imageOpacity: .4,
            alwaysOn: false,
            showEffect: "show",
            hideEffect: "hide",
            fadeinSpeed: "slow",
            fadeoutSpeed: "2000"
        },
        disable: function (e) {
            var t = $(e).data("jqzoom");
            t.disable();
            return false
        },
        enable: function (e) {
            var t = $(e).data("jqzoom");
            t.enable();
            return false
        },
        disableAll: function (e) {
            jqzoompluging_disabled = true
        },
        enableAll: function (e) {
            jqzoompluging_disabled = false
        }
    }
})(jQuery)
