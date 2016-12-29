/*!	panoStudioViewer.js is based on SWFObject v2.2 <http://code.google.com/p/swfobject/> 
	which is released under the MIT License <http://www.opensource.org/licenses/mit-license.php> 
*/
var panoStudioViewer = function () {
    var F = "undefined",
        s = "object",
        U = "Shockwave Flash",
        Y = "ShockwaveFlash.ShockwaveFlash",
        r = "application/x-shockwave-flash",
        T = "SWFObjectExprInst",
        y = "onreadystatechange",
        Q = window,
        k = document,
        u = navigator,
        V = false,
        W = [i],
        p = [],
        P = [],
        K = [],
        m, S, G, D, L = false,
        a = false,
        o, I, n = true,
        O = function () {
            var ac = typeof k.getElementById != F && typeof k.getElementsByTagName != F && typeof k.createElement != F,
                aj = u.userAgent.toLowerCase(),
                aa = u.platform.toLowerCase(),
                ag = aa ? /win/.test(aa) : /win/.test(aj),
                ae = aa ? /mac/.test(aa) : /mac/.test(aj),
                ah = /webkit/.test(aj) ? parseFloat(aj.replace(/^.*webkit\/(\d+(\.\d+)?).*$/, "$1")) : false,
                Z = !+"\v1",
                ai = [0, 0, 0],
                ad = null;
            if (typeof u.plugins != F && typeof u.plugins[U] == s) {
                ad = u.plugins[U].description;
                if (ad && !(typeof u.mimeTypes != F && u.mimeTypes[r] && !u.mimeTypes[r].enabledPlugin)) {
                    V = true;
                    Z = false;
                    ad = ad.replace(/^.*\s+(\S+\s+\S+$)/, "$1");
                    ai[0] = parseInt(ad.replace(/^(.*)\..*$/, "$1"), 10);
                    ai[1] = parseInt(ad.replace(/^.*\.(.*)\s.*$/, "$1"), 10);
                    ai[2] = /[a-zA-Z]/.test(ad) ? parseInt(ad.replace(/^.*[a-zA-Z]+(.*)$/, "$1"), 10) : 0
                }
            } else {
                if (typeof Q.ActiveXObject != F) {
                    try {
                        var af = new ActiveXObject(Y);
                        if (af) {
                            ad = af.GetVariable("$version");
                            if (ad) {
                                Z = true;
                                ad = ad.split(" ")[1].split(",");
                                ai = [parseInt(ad[0], 10), parseInt(ad[1], 10), parseInt(ad[2], 10)]
                            }
                        }
                    } catch (ab) { }
                }
            }
            return {
                w3: ac,
                pv: ai,
                wk: ah,
                ie: Z,
                win: ag,
                mac: ae
            }
        }(),
        l = function () {
            if (!O.w3) {
                return
            }
            if ((typeof k.readyState != F && k.readyState == "complete") || (typeof k.readyState == F && (k.getElementsByTagName("body")[0] || k.body))) {
                g()
            }
            if (!L) {
                if (typeof k.addEventListener != F) {
                    k.addEventListener("DOMContentLoaded", g, false)
                }
                if (O.ie && O.win) {
                    k.attachEvent(y, function () {
                        if (k.readyState == "complete") {
                            k.detachEvent(y, arguments.callee);
                            g()
                        }
                    });
                    if (Q == top) {
                        (function () {
                            if (L) {
                                return
                            }
                            try {
                                k.documentElement.doScroll("left")
                            } catch (Z) {
                                setTimeout(arguments.callee, 0);
                                return
                            }
                            g()
                        })()
                    }
                }
                if (O.wk) {
                    (function () {
                        if (L) {
                            return
                        }
                        if (!/loaded|complete/.test(k.readyState)) {
                            setTimeout(arguments.callee, 0);
                            return
                        }
                        g()
                    })()
                }
                t(g)
            }
        }();

    function g() {
        if (L) {
            return
        }
        try {
            var ab = k.getElementsByTagName("body")[0].appendChild(E("span"));
            ab.parentNode.removeChild(ab)
        } catch (ac) {
            return
        }
        L = true;
        var Z = W.length;
        for (var aa = 0; aa < Z; aa++) {
            W[aa]()
        }
    }

    function M(Z) {
        if (L) {
            Z()
        } else {
            W[W.length] = Z
        }
    }

    function t(aa) {
        if (typeof Q.addEventListener != F) {
            Q.addEventListener("load", aa, false)
        } else {
            if (typeof k.addEventListener != F) {
                k.addEventListener("load", aa, false)
            } else {
                if (typeof Q.attachEvent != F) {
                    j(Q, "onload", aa)
                } else {
                    if (typeof Q.onload == "function") {
                        var Z = Q.onload;
                        Q.onload = function () {
                            Z();
                            aa()
                        }
                    } else {
                        Q.onload = aa
                    }
                }
            }
        }
    }

    function i() {
        if (V) {
            X()
        } else {
            J()
        }
    }

    function X() {
        var Z = k.getElementsByTagName("body")[0];
        var ac = E(s);
        ac.setAttribute("type", r);
        ac.setAttribute("quality", "high");
        var ab = Z.appendChild(ac);
        if (ab) {
            var aa = 0;
            (function () {
                if (typeof ab.GetVariable != F) {
                    var ad = ab.GetVariable("$version");
                    if (ad) {
                        ad = ad.split(" ")[1].split(",");
                        O.pv = [parseInt(ad[0], 10), parseInt(ad[1], 10), parseInt(ad[2], 10)]
                    }
                } else {
                    if (aa < 10) {
                        aa++;
                        setTimeout(arguments.callee, 10);
                        return
                    }
                }
                Z.removeChild(ac);
                ab = null;
                J()
            })()
        } else {
            J()
        }
    }

    function J() {
        var ai = p.length;
        if (ai > 0) {
            for (var ah = 0; ah < ai; ah++) {
                var aa = p[ah].id;
                var ad = p[ah].callbackFn;
                var ac = {
                    success: false,
                    id: aa
                };
                if (O.pv[0] > 0) {
                    var ag = c(aa);
                    if (ag) {
                        if (H(p[ah].swfVersion) && !(O.wk && O.wk < 312)) {
                            x(aa, true);
                            if (ad) {
                                ac.success = true;
                                ac.ref = A(aa);
                                ad(ac)
                            }
                        } else {
                            if (p[ah].expressInstall && C()) {
                                var ak = {};
                                ak.data = p[ah].expressInstall;
                                ak.width = ag.getAttribute("width") || "0";
                                ak.height = ag.getAttribute("height") || "0";
                                if (ag.getAttribute("class")) {
                                    ak.styleclass = ag.getAttribute("class")
                                }
                                if (ag.getAttribute("align")) {
                                    ak.align = ag.getAttribute("align")
                                }
                                var aj = {};
                                var Z = ag.getElementsByTagName("param");
                                var ae = Z.length;
                                for (var af = 0; af < ae; af++) {
                                    if (Z[af].getAttribute("name").toLowerCase() != "movie") {
                                        aj[Z[af].getAttribute("name")] = Z[af].getAttribute("value")
                                    }
                                }
                                R(ak, aj, aa, ad)
                            } else {
                                q(ag);
                                if (ad) {
                                    ad(ac)
                                }
                            }
                        }
                    }
                } else {
                    x(aa, true);
                    if (ad) {
                        var ab = A(aa);
                        if (ab && typeof ab.SetVariable != F) {
                            ac.success = true;
                            ac.ref = ab
                        }
                        ad(ac)
                    }
                }
            }
        }
    }

    function A(ac) {
        var Z = null;
        var aa = c(ac);
        if (aa && aa.nodeName == "OBJECT") {
            if (typeof aa.SetVariable != F) {
                Z = aa
            } else {
                var ab = aa.getElementsByTagName(s)[0];
                if (ab) {
                    Z = ab
                }
            }
        }
        return Z
    }

    function C() {
        return !a && H("6.0.65") && (O.win || O.mac) && !(O.wk && O.wk < 312)
    }

    function R(ac, ad, Z, ab) {
        a = true;
        G = ab || null;
        D = {
            success: false,
            id: Z
        };
        var ag = c(Z);
        if (ag) {
            if (ag.nodeName == "OBJECT") {
                m = h(ag);
                S = null
            } else {
                m = ag;
                S = Z
            }
            ac.id = T;
            if (typeof ac.width == F || (!/%$/.test(ac.width) && parseInt(ac.width, 10) < 310)) {
                ac.width = "310"
            }
            if (typeof ac.height == F || (!/%$/.test(ac.height) && parseInt(ac.height, 10) < 137)) {
                ac.height = "137"
            }
            k.title = k.title.slice(0, 47) + " - Flash Player Installation";
            var af = O.ie && O.win ? "ActiveX" : "PlugIn",
                ae = "MMredirectURL=" + Q.location.toString().replace(/&/g, "%26") + "&MMplayerType=" + af + "&MMdoctitle=" + k.title;
            if (typeof ad.flashvars != F) {
                ad.flashvars += "&" + ae
            } else {
                ad.flashvars = ae
            }
            if (O.ie && O.win && ag.readyState != 4) {
                var aa = E("div");
                Z += "SWFObjectNew";
                aa.setAttribute("id", Z);
                ag.parentNode.insertBefore(aa, ag);
                ag.style.display = "none";
                (function () {
                    if (ag.readyState == 4) {
                        ag.parentNode.removeChild(ag)
                    } else {
                        setTimeout(arguments.callee, 10)
                    }
                })()
            }
            v(ac, ad, Z)
        }
    }

    function q(aa) {
        if (O.ie && O.win && aa.readyState != 4) {
            var Z = E("div");
            aa.parentNode.insertBefore(Z, aa);
            Z.parentNode.replaceChild(h(aa), Z);
            aa.style.display = "none";
            (function () {
                if (aa.readyState == 4) {
                    aa.parentNode.removeChild(aa)
                } else {
                    setTimeout(arguments.callee, 10)
                }
            })()
        } else {
            aa.parentNode.replaceChild(h(aa), aa)
        }
    }

    function h(ae) {
        var ad = E("div");
        if (O.win && O.ie) {
            ad.innerHTML = ae.innerHTML
        } else {
            var aa = ae.getElementsByTagName(s)[0];
            if (aa) {
                var af = aa.childNodes;
                if (af) {
                    var Z = af.length;
                    for (var ab = 0; ab < Z; ab++) {
                        if (!(af[ab].nodeType == 1 && af[ab].nodeName == "PARAM") && !(af[ab].nodeType == 8)) {
                            ad.appendChild(af[ab].cloneNode(true))
                        }
                    }
                }
            }
        }
        return ad
    }

    function v(ak, ai, aa) {
        var Z, ac = c(aa);
        if (O.wk && O.wk < 312) {
            return Z
        }
        if (ac) {
            if (typeof ak.id == F) {
                ak.id = aa
            }
            if (O.ie && O.win) {
                var aj = "";
                for (var ag in ak) {
                    if (ak[ag] != Object.prototype[ag]) {
                        if (ag.toLowerCase() == "data") {
                            ai.movie = ak[ag]
                        } else {
                            if (ag.toLowerCase() == "styleclass") {
                                aj += ' class="' + ak[ag] + '"'
                            } else {
                                if (ag.toLowerCase() != "classid") {
                                    aj += " " + ag + '="' + ak[ag] + '"'
                                }
                            }
                        }
                    }
                }
                var ah = "";
                for (var af in ai) {
                    if (ai[af] != Object.prototype[af]) {
                        ah += '<param name="' + af + '" value="' + ai[af] + '" />'
                    }
                }
                ac.outerHTML = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"' + aj + ">" + ah + "</object>";
                P[P.length] = ak.id;
                Z = c(ak.id)
            } else {
                var ab = E(s);
                ab.setAttribute("type", r);
                for (var ae in ak) {
                    if (ak[ae] != Object.prototype[ae]) {
                        if (ae.toLowerCase() == "styleclass") {
                            ab.setAttribute("class", ak[ae])
                        } else {
                            if (ae.toLowerCase() != "classid") {
                                ab.setAttribute(ae, ak[ae])
                            }
                        }
                    }
                }
                for (var ad in ai) {
                    if (ai[ad] != Object.prototype[ad] && ad.toLowerCase() != "movie") {
                        f(ab, ad, ai[ad])
                    }
                }
                ac.parentNode.replaceChild(ab, ac);
                Z = ab
            }
        }
        return Z
    }

    function f(ab, Z, aa) {
        var ac = E("param");
        ac.setAttribute("name", Z);
        ac.setAttribute("value", aa);
        ab.appendChild(ac)
    }

    function z(aa) {
        var Z = c(aa);
        if (Z && Z.nodeName == "OBJECT") {
            if (O.ie && O.win) {
                Z.style.display = "none";
                (function () {
                    if (Z.readyState == 4) {
                        b(aa)
                    } else {
                        setTimeout(arguments.callee, 10)
                    }
                })()
            } else {
                Z.parentNode.removeChild(Z)
            }
        }
    }

    function b(ab) {
        var aa = c(ab);
        if (aa) {
            for (var Z in aa) {
                if (typeof aa[Z] == "function") {
                    aa[Z] = null
                }
            }
            aa.parentNode.removeChild(aa)
        }
    }

    function c(ab) {
        var Z = null;
        try {
            Z = k.getElementById(ab)
        } catch (aa) { }
        return Z
    }

    function E(Z) {
        return k.createElement(Z)
    }

    function j(ab, Z, aa) {
        ab.attachEvent(Z, aa);
        K[K.length] = [ab, Z, aa]
    }

    function H(ab) {
        var aa = O.pv,
            Z = ab.split(".");
        Z[0] = parseInt(Z[0], 10);
        Z[1] = parseInt(Z[1], 10) || 0;
        Z[2] = parseInt(Z[2], 10) || 0;
        return (aa[0] > Z[0] || (aa[0] == Z[0] && aa[1] > Z[1]) || (aa[0] == Z[0] && aa[1] == Z[1] && aa[2] >= Z[2])) ? true : false
    }

    function w(ae, aa, af, ad) {
        if (O.ie && O.mac) {
            return
        }
        var ac = k.getElementsByTagName("head")[0];
        if (!ac) {
            return
        }
        var Z = (af && typeof af == "string") ? af : "screen";
        if (ad) {
            o = null;
            I = null
        }
        if (!o || I != Z) {
            var ab = E("style");
            ab.setAttribute("type", "text/css");
            ab.setAttribute("media", Z);
            o = ac.appendChild(ab);
            if (O.ie && O.win && typeof k.styleSheets != F && k.styleSheets.length > 0) {
                o = k.styleSheets[k.styleSheets.length - 1]
            }
            I = Z
        }
        if (O.ie && O.win) {
            if (o && typeof o.addRule == s) {
                o.addRule(ae, aa)
            }
        } else {
            if (o && typeof k.createTextNode != F) {
                o.appendChild(k.createTextNode(ae + " {" + aa + "}"))
            }
        }
    }

    function x(ab, Z) {
        if (!n) {
            return
        }
        var aa = Z ? "visible" : "hidden";
        if (L && c(ab)) {
            c(ab).style.visibility = aa
        } else {
            w("#" + ab, "visibility:" + aa)
        }
    }

    function N(aa) {
        var ab = /[\\\"<>\.;]/;
        var Z = ab.exec(aa) != null;
        return Z && typeof encodeURIComponent != F ? encodeURIComponent(aa) : aa
    }
    var e = function () {
        if (O.ie && O.win) {
            window.attachEvent("onunload", function () {
                var ae = K.length;
                for (var ad = 0; ad < ae; ad++) {
                    K[ad][0].detachEvent(K[ad][1], K[ad][2])
                }
                var ab = P.length;
                for (var ac = 0; ac < ab; ac++) {
                    z(P[ac])
                }
                for (var aa in O) {
                    O[aa] = null
                }
                O = null;
                for (var Z in swfobject) {
                    swfobject[Z] = null
                }
                swfobject = null
            })
        }
    }();

    function B(aa, ae, Z) {
        var af = document.getElementById("panostudioviewerA");
        af.setAttribute("src", aa);
        af.innerHTML = "";
        var ag = document.getElementById("panostudioviewerB");
        ag.innerHTML = "";
        var ac = document.body.innerHTML;
        var ad = document.getElementsByTagName("head")[0];
        var ab = ad ? ad.innerHTML : null;
        document.open();
        document.write(ac);
        document.close();
        ad = document.getElementsByTagName("head")[0];
        if (ad) {
            ad.innerHTML = ab
        }
    }

    function d(aj, af, ai, Z, ab, ae) {
        var ak = document.createElement("style");
        ak.type = "text/css";
        var ah = document.createTextNode("html { height: 100%; }\nbody { height: 100%; overflow: hidden; margin: 0px; }");
        if (ak.styleSheet) {
            ak.styleSheet.cssText = ah.nodeValue
        } else {
            ak.appendChild(ah)
        }
        document.getElementsByTagName("head")[0].appendChild(ak);
        var ad = document.createElement("meta");
        ad.name = "viewport";
        ad.content = "width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0, target-densitydpi=device-dpi";
        document.getElementsByTagName("head")[0].appendChild(ad);
        var ac = document.createElement("meta");
        ac.name = "apple-mobile-web-app-capable";
        ac.content = "yes";
        document.getElementsByTagName("head")[0].appendChild(ac);
        var aa = document.getElementById(aj);
        aa.id = "panoStudioViewerDIV";
        aa.setAttribute("data-panosrc", Z);
        var ag = document.createElement("div");
        ag.id = aj;
        ag.setAttribute("style", "display: none;");
        aa.appendChild(ag);
        M(function () {
            B("panoStudioViewerHTML.js", aa, Z)
        })
    }
    return {
        registerObject: function (ad, Z, ac, ab) {
            if (O.w3 && ad && Z) {
                var aa = {};
                aa.id = ad;
                aa.swfVersion = Z;
                aa.expressInstall = ac;
                aa.callbackFn = ab;
                p[p.length] = aa;
                x(ad, false)
            } else {
                if (ab) {
                    ab({
                        success: false,
                        id: ad
                    })
                }
            }
        },
        getObjectById: function (Z) {
            if (O.w3) {
                return A(Z)
            }
        },
        embed: function (an, ak, am, Z, ae, ag, ad, aa) {
            var ab = u.userAgent.toLowerCase();
            var aj = (ab.indexOf("safari") != -1) && (ab.indexOf("applewebkit") != -1) && (ab.indexOf("android") == -1) && (ab.indexOf("chrome") == -1);
            var ah = "9.0.28";
            if (aj && ae == "true" && (!H(ah) || (ag == "true"))) {
                d(an, ak, am, Z, ae, ag)
            } else {
                if (am == "100%") {
                    var ao = document.createElement("style");
                    ao.type = "text/css";
                    var al = document.createTextNode("html { height: 100%; }\nbody { height: 100%; overflow: hidden; margin: 0px; }");
                    if (ao.styleSheet) {
                        ao.styleSheet.cssText = al.nodeValue
                    } else {
                        ao.appendChild(al)
                    }
                    document.getElementsByTagName("head")[0].appendChild(ao);
                    var af = document.createElement("meta");
                    af.name = "viewport";
                    af.content = "width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0, target-densitydpi=device-dpi";
                    document.getElementsByTagName("head")[0].appendChild(af)
                }
                var ai = {
                    allowScriptAccess: "always",
                    allowNetworking: "all",
                    allowFullScreen: "true",
                    bgcolor: "#000000",
                    wmode: "gpu"
                };
                for (var ac in ad) {
                    ai[ac] = ad[ac]
                }
                this.embedSWF("panoStudioViewer.swf", an, ak, am, ah, "", {
                    pano: Z
                }, ai, {
                    style: "margin: auto; padding:0;"
                }, function (aq) {
                    if (aq.success == false) {
                        var ap = document.getElementById(an);
                        ap.id = "panoStudioViewerDIV";
                        if (aa != undefined && aa != "") {
                            ap.innerHTML = aa
                        } else {
                            ap.innerHTML = '<p><a href="http://www.adobe.com/go/getflashplayer"><img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" /></a></p>'
                        }
                    }
                })
            }
        },
        panoInit: function () {
            panoInit_()
        },
        embedSWF: function (ad, aj, ag, ai, aa, ac, ab, af, ah, ae) {
            var Z = {
                success: false,
                id: aj
            };
            if (O.w3 && !(O.wk && O.wk < 312) && ad && aj && ag && ai && aa) {
                x(aj, false);
                M(function () {
                    ag += "";
                    ai += "";
                    var al = {};
                    if (ah && typeof ah === s) {
                        for (var an in ah) {
                            al[an] = ah[an]
                        }
                    }
                    al.data = ad;
                    al.width = ag;
                    al.height = ai;
                    var ao = {};
                    if (af && typeof af === s) {
                        for (var am in af) {
                            ao[am] = af[am]
                        }
                    }
                    if (ab && typeof ab === s) {
                        for (var ak in ab) {
                            if (typeof ao.flashvars != F) {
                                ao.flashvars += "&" + ak + "=" + ab[ak]
                            } else {
                                ao.flashvars = ak + "=" + ab[ak]
                            }
                        }
                    }
                    if (H(aa)) {
                        var ap = v(al, ao, aj);
                        if (al.id == aj) {
                            x(aj, true)
                        }
                        Z.success = true;
                        Z.ref = ap
                    } else {
                        if (ac && C()) {
                            al.data = ac;
                            R(al, ao, aj, ae);
                            return
                        } else {
                            x(aj, true)
                        }
                    }
                    if (ae) {
                        ae(Z)
                    }
                })
            } else {
                if (ae) {
                    ae(Z)
                }
            }
        },
        switchOffAutoHideShow: function () {
            n = false
        },
        ua: O,
        getFlashPlayerVersion: function () {
            return {
                major: O.pv[0],
                minor: O.pv[1],
                release: O.pv[2]
            }
        },
        hasFlashPlayerVersion: H,
        createSWF: function (ab, aa, Z) {
            if (O.w3) {
                return v(ab, aa, Z)
            } else {
                return undefined
            }
        },
        showExpressInstall: function (ab, ac, Z, aa) {
            if (O.w3 && C()) {
                R(ab, ac, Z, aa)
            }
        },
        removeSWF: function (Z) {
            if (O.w3) {
                z(Z)
            }
        },
        createCSS: function (ac, ab, aa, Z) {
            if (O.w3) {
                w(ac, ab, aa, Z)
            }
        },
        addDomLoadEvent: M,
        addLoadEvent: t,
        getQueryParamValue: function (ac) {
            var ab = k.location.search || k.location.hash;
            if (ab) {
                if (/\?/.test(ab)) {
                    ab = ab.split("?")[1]
                }
                if (ac == null) {
                    return N(ab)
                }
                var aa = ab.split("&");
                for (var Z = 0; Z < aa.length; Z++) {
                    if (aa[Z].substring(0, aa[Z].indexOf("=")) == ac) {
                        return N(aa[Z].substring((aa[Z].indexOf("=") + 1)))
                    }
                }
            }
            return ""
        },
        expressInstallCallback: function () {
            if (a) {
                var Z = c(T);
                if (Z && m) {
                    Z.parentNode.replaceChild(m, Z);
                    if (S) {
                        x(S, true);
                        if (O.ie && O.win) {
                            m.style.display = "block"
                        }
                    }
                    if (G) {
                        G(D)
                    }
                }
                a = false
            }
        }
    }
}();