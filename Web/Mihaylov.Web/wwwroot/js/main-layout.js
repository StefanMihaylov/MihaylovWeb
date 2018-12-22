'use strict';
var main = {};
var constants = {};
$(document).ready(function () {
    constants = (function () {
        return {
            BLOCK_UI_DURATION: 0
        };
    })();

    main = (function () {
        var getUrl = function (urlExtension) {
            var url = $('.serverUrl').data('url'),
                newUrl;

            if (url.slice(-1) != '/') {
                url += '/';
            }

            newUrl = url + urlExtension;
            return newUrl;
        }

        var changeUrlId = function (url, method, id, separator) {
            separator = separator || '/';
            var lastIndex = url.lastIndexOf(method);
            var end = lastIndex + method.length;
            var newUrl = url.substr(0, end);

            if (id) {
                newUrl = newUrl + separator + id;
            }

            return newUrl;
        }

        // block UI
        $('.block-ui-btn').on('click', function () {
            $('.loader-gr-fixed-backgraund').show(constants.BLOCK_UI_DURATION);
        });

        var indexPageInitialization = function () {
            resizeMainContainer('.scrollable-content');
            $(window).resize(function () {
                resizeMainContainer('.scrollable-content');
            })
        };

        return {
            getUrl: getUrl,
            changeUrlId: changeUrlId,
            indexPageInitialization: indexPageInitialization,
        };

        // private
        function resizeMainContainer(container) {
            var bodyHeight = parseFloat($('body').height()),
                bodyWidth = parseFloat($('body').width());

            var height = '';
            var childHeight = '';
            var mapHeight = 200;
            if (bodyWidth > 992) {
                height = bodyHeight - 25;
                childHeight = height - 2;
                mapHeight = childHeight - 80;
            }

            $(container).height(height);
            $(container + '-child').height(childHeight);
        }
    })();

    //var errorLogger = (function attachEvent() {
    //    window.onerror = function (errorMsg, url, lineNumber, column, errorObj) {
    //        var encodedStack;
    //        if (errorObj) {
    //            var stack = errorObj.hasOwnProperty('stack') ? errorObj.stack : errorObj;
    //            encodedStack = $('<div/>').text(stack.toString()).html();
    //        }

    //        var ajaxData = {
    //            errorMessage: errorMsg,
    //            scriptUrl: url,
    //            lineNumber: lineNumber,
    //            columnNumber: column,
    //            stackTrace: encodedStack,
    //            browserUserAgent: window.navigator.userAgent
    //        };

    //        var checkPrinterUrl = main.getUrl('ClientError/Write');
    //        if (window.XMLHttpRequest) {
    //            var xhr = new XMLHttpRequest();

    //            // serialize the POST params
    //            var params = 'errorMessage=' + ajaxData.errorMessage + '&scriptUrl=' + ajaxData.scriptUrl + '&lineNumber=' + ajaxData.lineNumber +
    //                '&columnNumber=' + ajaxData.columnNumber + '&stackTrace=' + encodeURIComponent(ajaxData.stackTrace) +
    //                '&browserUserAgent=' + ajaxData.browserUserAgent;

    //            // open an asynchronous connection
    //            xhr.open("POST", checkPrinterUrl, true);
    //            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    //            xhr.send(params);
    //        }

    //        // returning false triggers the execution of the built-in error handler
    //        return false;
    //    }
    //})();

});