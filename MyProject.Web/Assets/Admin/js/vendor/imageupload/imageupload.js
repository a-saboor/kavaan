/*
 * HTML5 Image Upload
 * By STBeets
 * Purchase on http://codecanyon.net/item/html-5-upload-image-ratio-with-drag-and-drop/8712634?ref=stbeets
 * Version 1.4.2
 */
function empty(e) {
    var t, n, r, i;
    var s = [t, null, false, 0, "", "0"];
    for (r = 0, i = s.length; r < i; r++) {
        if (e === s[r]) {
            return true
        }
    }
    if (typeof e === "object") {
        for (n in e) {
            return false
        }
        return true
    }
    return false
} (function (e, t, n) {
    "use strict";
    t.html5imageupload = function (r, i) {
        this.element = i;
        this.options = t.extend(true, {}, t.html5imageupload.defaults, r, t(this.element).data());
        this.input = t(this.element).find("input[type=file]");
        var s = t(e);
        var o = this;
        this.interval = null;
        this.drag = false;
        this.widthPercentage = 100;
        this.button = {};
        this.button.edit = '<div class="btn btn-custom btn-edit"><i class="fa fa-check p-0 m-0 fa-sm text-white"></i></div>';
        this.button.saving = '<div class="btn btn-warning saving">' + (this.options.saveLabel || "Saving...") + ' <i class="fa-spin fa-circle-notch p-0 m-0 fa-sm"></i></div>';
        this.button.zoomin = '<div class="btn btn-default btn-zoom-in"><i class="fa fa-expand-alt p-0 m-0 fa-sm text-dark"></i></div>';
        this.button.zoomout = '<div class="btn btn-default btn-zoom-out"><i class="fa fa-compress-alt p-0 m-0 fa-sm text-dark"></i></div>';
        this.button.zoomreset = '<div class="btn btn-default btn-zoom-reset"><i class="fa fa-arrows-alt p-0 m-0 fa-sm text-dark"></i></div>';
        this.button.cancel = '<div class="btn btn-danger btn-cancel"><i class="fa fa-times p-0 m-0 fa-sm"></i></div>';
        this.button.done = '<div class="btn btn-custom btn-ok"><i class="fa fa-check p-0 m-0 fa-sm text-white"></i></div>';
        this.button.del = '<div class="btn btn-danger btn-del"><i class="fa fa-trash p-0 m-0 fa-sm"></i></div>';
        this.imageMimes = {
            png: "image/png",
            bmp: "image/bmp",
            gif: "image/gif",
            jpg: "image/jpeg",
            jpeg: "image/jpeg",
            tiff: "image/tiff"
        };
        o._init()
    };
    t.html5imageupload.defaults = {
        width: null,
        height: null,
        image: null,
        ghost: true,
        originalsize: true,
        url: false,
        removeurl: null,
        data: {},
        canvas: true,
        ajax: true,
        resize: false,
        dimensionsonly: false,
        editstart: false,
        saveOriginal: false,
        onAfterZoomImage: null,
        onAfterInitImage: null,
        onAfterMoveImage: null,
        onAfterProcessImage: null,
        onAfterResetImage: null,
        onAfterCancel: null,
        onAfterRemoveImage: null,
        onAfterSelectImage: null
    };
    t.html5imageupload.prototype = {
        _init: function () {
            //debugger ;
            var n = this;
            var r = n.options;
            var i = n.element;
            var s = n.input;
            if (empty(t(i))) {
                return false
            } else {
                t(i).children().css({
                    position: "absolute"
                })
            }
            //if (!(e.FormData && "upload" in t.ajaxSettings.xhr())) {
            //    t(i).empty().attr("class", "").addClass("alert alert-danger").html("HTML5 Upload Image: Sadly.. this browser does not support the plugin, update your browser today!");
            //    return false
            //}
            //if (!empty(r.width) && empty(r.height) && t(i).innerHeight() <= 0) {
            //    t(i).empty().attr("class", "").addClass("alert alert-danger").html("HTML5 Upload Image: Image height is not set and can not be calculated...");
            //    return false
            //}
            //if (!empty(r.height) && empty(r.width) && t(i).innerWidth() <= 0) {
            //    t(i).empty().attr("class", "").addClass("alert alert-danger").html("HTML5 Upload Image: Image width is not set and can not be calculated...");
            //    return false
            //}
            //if (!empty(r.height) && !empty(r.width) && !empty(t(i).innerHeight() && !empty(t(i).innerWidth()))) {
            //    if (r.width / r.height != t(i).outerWidth() / t(i).outerHeight()) {
            //        t(i).empty().attr("class", "").addClass("alert alert-danger").html("HTML5 Upload Image: The ratio of all sizes (CSS and image) are not the same...");
            //        return false
            //    }
            //}
            var o, u, a, f;
            r.width = a = r.width || t(i).outerWidth();
            r.height = f = r.height || t(i).outerHeight();
            if (t(i).innerWidth() > 0) {
                o = t(i).outerWidth()
            } else if (t(i).innerHeight() > 0) {
                o = null
            } else if (!empty(r.width)) {
                o = r.width
            }
            if (t(i).innerHeight() > 0) {
                u = t(i).outerHeight()
            } else if (t(i).innerWidth() > 0) {
                u = null
            } else if (!empty(r.height)) {
                u = r.height
            }
            u = u || o / (a / f);
            o = o || u / (f / a);
            if (o < 240) {
                t(i).addClass("smalltools smalltext")
            }
            t(i).css({
                height: u,
                width: o
            });
            n.widthPercentage = t(i).outerWidth() / t(i).offsetParent().width() * 100;
            if (r.resize == true) {
                t(e).resize(function () {
                    n.resize()
                })
            }
            n._bind();
            if (r.required || t(s).prop("required")) {
                n.options.required = true;
                t(s).prop("required", true)
            }
            if (!r.ajax) {
                n._formValidation()
            }
            if (!empty(r.image) && r.editstart == false) {
                t(i).data("name", r.image).append(t("<img />").addClass("preview").attr("src", r.image));
                var l = t('<div class="preview tools"></div>');
                var c = t("" + this.button.del + "");
                t(c).click(function (e) {
                    e.preventDefault();
                    n.reset()
                });
                if (r.buttonDel != false) {
                    t(l).append(c)
                }
                t(i).append(l)
            } else if (!empty(r.image)) {
                n.readImage(r.image, r.image, r.image, n.imageMimes[r.image.split(".").pop()])
            }
            if (n.options.onAfterInitImage) n.options.onAfterInitImage.call(n, n)
        },
        _bind: function () {
            var e = this;
            var n = e.element;
            var r = e.input;
            t(n).unbind("dragover").unbind("drop").unbind("mouseout").on({
                dragover: function (t) {
                    e.handleDrag(t)
                },
                drop: function (n) {
                    e.handleFile(n, t(this))
                },
                mouseout: function () {
                    e.imageUnhandle()
                }
            });
            t(r).unbind("change").change(function (r) {
                e.drag = false;
                e.handleFile(r, t(n))
            })
        },
        handleFile: function (e, n) {
            e.stopPropagation();
            e.preventDefault();
            var r = this;
            var i = this.options;
            var s = r.drag == false ? e.originalEvent.target.files : e.originalEvent.dataTransfer.files;
            r.drag = false;
            if (i.removeurl != null && !empty(t(n).data("name"))) {
                t.ajax({
                    type: "POST",
                    url: i.removeurl,
                    data: {
                        image: t(n).data("name")
                    },
                    success: function (e) {
                        if (r.options.onAfterRemoveImage) r.options.onAfterRemoveImage.call(r, e, r)
                    }
                })
            }
            t(n).removeClass("notAnImage").addClass("loading");
            for (var o = 0, u; u = s[o]; o++) {
                if (!u.type.match("image.*")) {
                    t(n).addClass("notAnImage");
                    continue
                }
                var a = new FileReader;
                a.onload = function (e) {
                    return function (i) {
                        t(n).find("img").remove();
                        r.readImage(a.result, i.target.result, e.name, e.type)
                    }
                }(u);
                a.readAsDataURL(u)
            }
            if (r.options.onAfterSelectImage) r.options.onAfterSelectImage.call(r, response, r)
        },
        readImage: function (e, n, r, i) {
            var s = this;
            var o = this.options;
            var u = this.element;
            s.drag = false;
            var a = new Image;
            a.onload = function (e) {
                var f = t('<img src="' + n + '" name="' + r + '" />');
                var l, c, h, p, d, v;
                l = h = a.width;
                c = p = a.height;
                d = l / c;
                v = t(u).outerWidth() / t(u).outerHeight();
                if (o.originalsize == false) {
                    h = t(u).outerWidth() + 40;
                    p = h / d;
                    if (p < t(u).outerHeight()) {
                        p = t(u).outerHeight() + 40;
                        h = p * d
                    }
                } else if (h < t(u).outerWidth() || p < t(u).outerHeight()) {
                    if (d < v) {
                        h = t(u).outerWidth();
                        p = h / d
                    } else {
                        p = t(u).outerHeight();
                        h = p * d
                    }
                }
                var m = parseFloat((t(u).outerWidth() - h) / 2);
                var g = parseFloat((t(u).outerHeight() - p) / 2);
                f.css({
                    left: m,
                    top: g,
                    width: h,
                    height: p
                });
                s.image = t(f).clone().data({
                    mime: i,
                    width: l,
                    height: c,
                    ratio: d,
                    left: m,
                    top: g,
                    useWidth: h,
                    useHeight: p
                }).addClass("main").bind("mousedown touchstart", function (e) {
                    s.imageHandle(e)
                });
                s.imageGhost = o.ghost ? t(f).addClass("ghost") : null;
                t(u).append(t('<div class="cropWrapper"></div>').append(t(s.image)));
                if (!empty(s.imageGhost)) {
                    t(u).append(s.imageGhost)
                }
                s._tools();
                t(u).removeClass("loading")
            };
            a.src = e
        },
        handleDrag: function (e) {
            var t = this;
            t.drag = true;
            e.stopPropagation();
            e.preventDefault();
            e.originalEvent.dataTransfer.dropEffect = "copy"
        },
        imageHandle: function (e) {
            e.preventDefault();
            var n = e.originalEvent.touches || e.originalEvent.changedTouches ? e.originalEvent.touches[0] || e.originalEvent.changedTouches[0] : e;
            var r = this;
            var i = this.element;
            var s = this.image;
            var o = s.outerHeight(),
                u = s.outerWidth(),
                a = s.offset().top + o - n.pageY,
                f = s.offset().left + u - n.pageX;
            s.on({
                "mousemove touchmove": function (e) {
                    e.stopImmediatePropagation();
                    e.preventDefault();
                    var n = e.originalEvent.touches || e.originalEvent.changedTouches ? e.originalEvent.touches[0] || e.originalEvent.changedTouches[0] : e;
                    var l = n.pageY + a - o,
                        c = n.pageX + f - u;
                    var h = t(i).outerWidth() != t(i).innerWidth();
                    if (parseInt(l - t(i).offset().top) > 0) {
                        l = t(i).offset().top + (h ? 1 : 0)
                    } else if (l + o < t(i).offset().top + t(i).outerHeight()) {
                        l = t(i).offset().top + t(i).outerHeight() - o + (h ? 1 : 0)
                    }
                    if (parseInt(c - t(i).offset().left) > 0) {
                        c = t(i).offset().left + (h ? 1 : 0);
                    } else if (c + u < t(i).offset().left + t(i).outerWidth()) {
                        c = t(i).offset().left + t(i).outerWidth() - u + (h ? 1 : 0);
                    }
                    s.offset({
                        top: l,
                        left: c
                    });
                    r._ghost()
                },
                "mouseup touchend": function () {
                    r.imageUnhandle()
                }
            })
        },
        imageUnhandle: function () {
            var e = this;
            var n = e.image;
            t(n).unbind("mousemove touchmove");
            if (e.options.onAfterMoveImage) e.options.onAfterMoveImage.call(e, e)
        },
        imageZoom: function (e) {
            var n = this;
            var r = n.element;
            var i = n.image;
            if (empty(i)) {
                n._clearTimers();
                return false
            }
            var s = i.data("ratio");
            var o = i.outerWidth() + e;
            var u = o / s;
            if (o < t(r).outerWidth()) {
                o = t(r).outerWidth();
                u = o / s;
                e = t(i).outerWidth() - o
            }
            if (u < t(r).outerHeight()) {
                u = t(r).outerHeight();
                o = u * s;
                e = t(i).outerWidth() - o
            }
            var a = Math.round(parseFloat(i.css("top")) - parseFloat(u - i.outerHeight()) / 2);
            var f = parseInt(i.css("left")) - e / 2;
            if (t(r).offset().left - f < t(r).offset().left) {
                f = 0
            } else if (t(r).outerWidth() > f + t(i).outerWidth() && e <= 0) {
                f = t(r).outerWidth() - o
            }
            if (t(r).offset().top - a < t(r).offset().top) {
                a = 0
            } else if (t(r).outerHeight() > a + t(i).outerHeight() && e <= 0) {
                a = t(r).outerHeight() - u
            }
            i.css({
                width: o,
                height: u,
                top: a,
                left: f
            });
            n._ghost()
        },
        imageCrop: function () {
            var e = this;
            var n = e.element;
            var r = e.image;
            var i = e.input;
            var s = e.options;
            var o = s.width != t(n).outerWidth() ? s.width / t(n).outerWidth() : 1;
            var u, a, f, l, c, h, p, d;
            u = s.width;
            a = s.height;
            f = parseInt(Math.round(parseInt(t(r).css("top")) * o)) * -1;
            l = parseInt(Math.round(parseInt(t(r).css("left")) * o)) * -1;
            c = parseInt(Math.round(t(r).width() * o));
            h = parseInt(Math.round(t(r).height() * o));
            p = t(r).data("width");
            d = t(r).data("height");
            f = f || 0;
            l = l || 0;
            var v = {
                name: t(r).attr("name"),
                imageOriginalWidth: p,
                imageOriginalHeight: d,
                imageWidth: c,
                imageHeight: h,
                width: u,
                height: a,
                left: l,
                top: f
            };
            t(n).find(".tools").children().toggle();
            t(n).find(".tools").append(t(e.button.saving));
            if (s.canvas == true) {
                var m = t('<canvas class="final" id="canvas_' + t(i).attr("name") + '" width="' + u + '" height="' + a + '" style="position:absolute; top: 0; bottom: 0; left: 0; right: 0; z-index:100; width: 100%; height: 100%;"></canvas>');
                t(n).append(m);
                var g = t(m)[0].getContext("2d");
                var y = new Image;
                y.onload = function () {
                    var o = t('<canvas width="' + c + '" height="' + h + '"></canvas>');
                    var p = t(o)[0].getContext("2d");
                    var d = t('<img src="" />');
                    p.drawImage(y, 0, 0, c, h);
                    var b = new Image;
                    b.onload = function () {
                        g.drawImage(b, l, f, u, a, 0, 0, u, a);
                        if (s.ajax == true) {
                            e._ajax(t.extend({
                                data: t(m)[0].toDataURL(r.data("mime"))
                            }, v))
                        } else {
                            var o = JSON.stringify(t.extend({
                                data: t(m)[0].toDataURL(r.data("mime"))
                            }, v));
                            t(i).after(t('<input type="text" name="' + t(i).attr("name") + '_values" class="final" />').val(o));
                            t(i).data("required", t(i).prop("required"));
                            t(i).prop("required", false);
                            t(i).wrap("<form>").parent("form").trigger("reset");
                            t(i).unwrap();
                            t(n).find(".tools .saving").remove();
                            t(n).find(".tools").children().toggle();
                            e.imageFinal()
                        }
                    };
                    b.src = t(o)[0].toDataURL(r.data("mime"))
                };
                y.src = t(r).attr("src");
                if (s.saveOriginal === true) {
                    v = t.extend({
                        original: t(r).attr("src")
                    }, v)
                }
            } else {
                if (s.ajax == true) {
                    e._ajax(t.extend({
                        data: t(r).attr("src"),
                        saveOriginal: s.saveOriginal
                    }, v))
                } else {
                    var b = t(n).find(".cropWrapper").clone();
                    t(b).addClass("final").show().unbind().children().unbind();
                    t(n).append(t(b));
                    e.imageFinal();
                    var w = JSON.stringify(v);
                    t(i).after(t('<input type="text" name="' + t(i).attr("name") + '_values" class="final" />').val(w))
                }
            }
        },
        _ajax: function (e) {
            var n = this;
            var r = n.element;
            var i = n.image;
            var s = n.options;
            if (s.dimensionsonly == true) {
                delete e.data
            }
            t.ajax({
                type: "POST",
                url: s.url,
                data: t.extend(e, s.data),
                success: function (e) {
                    if (e.status == "success") {
                        var i = e.url.split("?");
                        t(r).find(".tools .saving").remove();
                        t(r).find(".tools").children().toggle();
                        t(r).data("name", i[0]);
                        if (s.canvas != true) {
                            t(r).append(t('<img src="' + i[0] + '" class="final" style="width: 100%" />'))
                        }
                        n.imageFinal()
                    } else {
                        t(r).find(".tools .saving").remove();
                        t(r).find(".tools").children().toggle();
                        t(r).append(t('<div class="alert alert-danger">' + e.error + "</div>").css({
                            bottom: "10px",
                            left: "10px",
                            right: "10px",
                            position: "absolute",
                            zIndex: 99
                        }));
                        setTimeout(function () {
                            n.responseReset()
                        }, 2e3)
                    }
                },
                error: function (e, i) {
                    t(r).find(".tools .saving").remove();
                    t(r).find(".tools").children().toggle();
                    t(r).append(t('<div class="alert alert-danger"><strong>' + e.status + "</strong> " + e.statusText + "</div>").css({
                        bottom: "10px",
                        left: "10px",
                        right: "10px",
                        position: "absolute",
                        zIndex: 99
                    }));
                    setTimeout(function () {
                        n.responseReset()
                    }, 2e3)
                }
            })
        },
        imageReset: function () {
            var e = this;
            var n = e.image;
            var r = e.element;
            t(n).css({
                width: n.data("useWidth"),
                height: n.data("useHeight"),
                top: n.data("top"),
                left: n.data("left")
            });
            e._ghost();
            if (e.options.onAfterResetImage) e.options.onAfterResetImage.call(e, e)
        },
        imageFinal: function () {
            var e = this;
            var n = e.element;
            var r = e.input;
            var i = e.options;
            t(n).addClass("done");
            t(n).children().not(".final").hide();
            var s = t('<div class="tools final">');
            if (i.buttonEdit != false) {
                t(s).append(t(e.button.edit).click(function () {
                    t(n).children().show();
                    t(n).find(".final").remove();
                    t(r).data("valid", false)
                }))
            }
            if (i.buttonDel != false) {
                t(s).append(t(e.button.del).click(function (t) {
                    e.reset()
                }))
            }
            t(n).append(s);
            t(n).unbind();
            t(r).unbind().data("valid", true);
            if (e.options.onAfterProcessImage) e.options.onAfterProcessImage.call(e, e)
        },
        responseReset: function () {
            var e = this;
            var n = e.element;
            t(n).find(".alert").remove()
        },
        reset: function () {
            var e = this;
            var n = e.element;
            var r = e.input;
            var i = e.options;
            e.image = null;
            t(n).find(".preview").remove();
            t(n).removeClass("loading done").children().show().not("input[type=file]").remove();
            t(r).wrap("<form>").parent("form").trigger("reset");
            t(r).unwrap();
            t(r).prop("required", t(r).data("required") || i.required || false).data("valid", false);
            e._bind();
            if (i.removeurl != null && !empty(t(n).data("name"))) {
                t.ajax({
                    type: "POST",
                    url: i.removeurl,
                    data: {
                        image: t(n).data("name")
                    },
                    success: function (t) {
                        if (e.options.onAfterRemoveImage) e.options.onAfterRemoveImage.call(e, t, e)
                    }
                })
            }
            t(n).data("name", null);
            if (e.imageGhost) {
                t(e.imageGhost).remove();
                e.imageGhost = null
            }
            if (e.options.onAfterCancel) e.options.onAfterCancel.call(e)
        },
        resize: function () {
            var e = this;
            var n = e.options;
            var r = e.element;
            var i = e.image;
            if (n.resize !== true) return false;
            var s = t(r).outerWidth();
            var o = t(r).offsetParent().width() * (e.widthPercentage / 100);
            var u = o / s;
            var a = t(r).outerHeight() * u;
            t(r).css({
                height: a,
                width: o
            });
            if (o < 240) {
                if (!t(r).hasClass("smalltools smalltext")) {
                    t(r).addClass("smalltools smalltext smalladded")
                }
            } else {
                if (t(r).hasClass("smalladded")) {
                    t(r).removeClass("smalltools smalltext smalladded")
                }
            }
            if (!empty(i)) {
                t(i).css({
                    left: t(i).css("left").replace(/[^-\d\.]/g, "") * u + "px",
                    top: t(i).css("top").replace(/[^-\d\.]/g, "") * u + "px"
                });
                t(i).width(t(i).width() * u);
                t(i).height(t(i).height() * u);
                e._ghost()
            }
            return true
        },
        reinit: function () {
            this.resize();
            this._bind();
            return true
        },
        _ghost: function () {
            var e = this;
            var n = e.options;
            var r = e.image;
            var i = e.imageGhost;
            if (n.ghost == true && !empty(i)) {
                t(i).css({
                    width: r.css("width"),
                    height: r.css("height"),
                    top: r.css("top"),
                    left: r.css("left")
                })
            }
        },
        _tools: function () {
            var n = this;
            var r = n.element;
            var i = t('<div class="tools"></div>');
            var s = n.options;
            if (s.buttonZoomin != false) {
                t(i).append(t(n.button.zoomin).on({
                    "touchstart mousedown": function (t) {
                        t.preventDefault();
                        n.interval = e.setInterval(function () {
                            n.imageZoom(2)
                        }, 1)
                    },
                    "touchend mouseup mouseleave": function (t) {
                        t.preventDefault();
                        e.clearInterval(n.interval);
                        if (n.options.onAfterZoomImage) n.options.onAfterZoomImage.call(n, n)
                    }
                }))
            }
            if (s.buttonZoomreset != false) {
                t(i).append(t(n.button.zoomreset).on({
                    "touchstart click": function (e) {
                        e.preventDefault();
                        n.imageReset()
                    }
                }))
            }
            if (s.buttonZoomout != false) {
                t(i).append(t(n.button.zoomout).on({
                    "touchstart mousedown": function (t) {
                        t.preventDefault();
                        n.interval = e.setInterval(function () {
                            n.imageZoom(-2)
                        }, 1)
                    },
                    "touchend mouseup mouseleave": function (t) {
                        t.preventDefault();
                        e.clearInterval(n.interval);
                        if (n.options.onAfterZoomImage) n.options.onAfterZoomImage.call(n, n)
                    }
                }))
            }
            if (s.buttonCancel != false) {
                t(i).append(t(n.button.cancel).on({
                    "touchstart touchend click": function (e) {
                        e.preventDefault();
                        n.reset()
                    }
                }))
            }
            if (s.buttonDone != false) {
                t(i).append(t(n.button.done).on({
                    "touchstart click": function (e) {
                        e.preventDefault();
                        n.imageCrop()
                    }
                }))
            }
            t(r).append(t(i))
        },
        _clearTimers: function () {
            var t = e.setInterval("", 9999);
            for (var n = 1; n < t; n++) e.clearInterval(n)
        },
        _formValidation: function () {
            var e = this;
            var n = e.element;
            var r = e.input;
            t(n).closest("form").submit(function (e) {
                t(this).find("input[type=file]").each(function (n, r) {
                    if (t(r).prop("required")) {
                        if (t(r).data("valid") !== true) {
                            e.preventDefault();
                            return false
                        }
                    }
                });
                return true
            })
        }
    };
    t.fn.html5imageupload = function (e) {
        if (t.data(this, "html5imageupload")) return;
        return t(this).each(function () {
            t.data(this, "html5imageupload", new t.html5imageupload(e, this))
        })
    }
})(window, jQuery)