var offsetxpoint = -60
var offsetypoint = 20
var ie = document.all
var ns6 = document.getElementById && !document.all
var enabletip = false
if (ie || ns6)
    var tipobj = document.all ? document.all["dhtmltooltip"] : document.getElementById ? document.getElementById("dhtmltooltip") : ""
if (tipobj) {
    document.body.appendChild(tipobj)
}

function ietruebody() {
    return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
}

function ddrivetip(thetext, thecolor, thewidth) {
   
    if (ns6 || ie) {
        if (typeof thewidth != "undefined") tipobj.style.width = thewidth + "px"
        if (typeof thecolor != "undefined" && thecolor != "") tipobj.style.backgroundColor = "#fff"
        tipobj.innerHTML = thetext
        enabletip = true
        return false
    }
}

function positiontip(e) {
    if (enabletip) {
        var curX = (ns6) ? e.pageX : event.clientX + ietruebody().scrollLeft;
        var curY = (ns6) ? e.pageY : event.clientY + ietruebody().scrollTop;

        var rightedge = ie && !window.opera ? ietruebody().clientWidth - event.clientX - offsetxpoint : window.innerWidth - e.clientX - offsetxpoint - 20
        var bottomedge = ie && !window.opera ? ietruebody().clientHeight - event.clientY - offsetypoint : window.innerHeight - e.clientY - offsetypoint - 20

        var leftedge = (offsetxpoint < 0) ? offsetxpoint * (-1) : -1000

        if (rightedge < tipobj.offsetWidth)
            tipobj.style.left = ie ? ietruebody().scrollLeft + event.clientX - tipobj.offsetWidth + "px" : window.pageXOffset + e.clientX - tipobj.offsetWidth + "px"
        else if (curX < leftedge)
            tipobj.style.left = "5px"
        else
            tipobj.style.left = curX + offsetxpoint + "px"

        if (bottomedge < tipobj.offsetHeight)
            tipobj.style.top = ie ? ietruebody().scrollTop + event.clientY - tipobj.offsetHeight - offsetypoint + "px" : window.pageYOffset + e.clientY - tipobj.offsetHeight - offsetypoint + "px"
        else
            tipobj.style.top = curY + offsetypoint + "px"
        tipobj.style.visibility = "visible"
    }
}

function hideddrivetip() {
    if (ns6 || ie) {
        enabletip = false
        tipobj.style.visibility = "hidden"
        tipobj.style.left = "-1000px"
        tipobj.style.backgroundColor = ''
        tipobj.style.width = ''
    }
}

document.onmousemove = positiontip
$(document).ready(function () {
    //$('.popupRelate').on('click', function (e) {
    //    e.preventDefault();
    //    var page = $(this).data('href');
    //    $(this).attr('data-modal','modal-xlg')
    //    var datatitle = $(this).attr('title');
    //    if (!datatitle) {
    //        datatitle = 'Nội dung tham chiếu, sửa đổi';
    //    }
    //   return modalChildGet(this, page, datatitle)
       
    //});

    //$('.popupRelate').mouseover(function () {
    //    var color = 'yellow';
    //    if (typeof $(this).css('background-color') !== "undefined") {
    //        if ($(this).css('background-color').replace(/\s+/g, '') === 'rgba(0,0,0,0)') {
    //            color = '#F5F5DC';
    //        } else color = $(this).css('background-color');
    //    }
    //    var title = typeof $(this).attr('data-title') === "undefined" ? $(this).attr('title') : $(this).attr('data-title');
    //    ddrivetip(title, color, 300);
    //});
    //$('.popupRelate').mouseout(function () {
    //    hideddrivetip();
    //});
    //$('.popupRelate').hover(function (e) {
    //    $(this).attr('data-title', $(this).attr('title'));
    //    $(this).removeAttr('title');
    //},
    //    function (e) {
    //        $(this).attr('title', $(this).attr('data-title'));
    //    });
    // div
    $('.popupRelate2').on('click', function (e) {
        e.preventDefault();
        var page = $(this).data('href');
        $(this).attr('data-modal', 'modal-xlg')
        var datatitle = $(this).attr('title');
        if (!datatitle) {
            datatitle = 'Nội dung hướng dẫn';
        }
        hideddrivetip();
        return modalChildGet(this, page, datatitle)
        
    });
    $('.popupRelate').on('click', function (e) {
        e.preventDefault();
        var page = $(this).data('href');
        var datatitle = $(this).attr('title');
        if (!datatitle) {
            datatitle = 'Nội dung tham chiếu, sửa đổi';
        }
        $(this).attr('data-modal', 'modal-xlg')
        hideddrivetip();
        return modalChildGet(this, page, datatitle)
       
    });

    $('.popupRelate').mouseover(function () {
        var color = 'yellow';
        if (typeof $(this).css('background-color') !== "undefined") {
            if ($(this).css('background-color').replace(/\s+/g, '') === 'rgba(0,0,0,0)') {
                color = '#F5F5DC';
            } else color = $(this).css('background-color');
        }
        var title = typeof $(this).attr('data-title') === "undefined" ? $(this).attr('title') : $(this).attr('data-title');
        ddrivetip(title, color, 300);
    });
    $('.popupRelate').mouseout(function () {
        hideddrivetip();
    });
    $('.popupRelate').hover(function (e) {
        $(this).attr('data-title', $(this).attr('title'));
        $(this).removeAttr('title');
    },
        function (e) {
            $(this).attr('title', $(this).attr('data-title'));
        });

    $('.popupRelate2').mouseover(function () {
        var color = '#44a7eb';
        if (typeof $(this).css('background-color') !== "undefined") {
            if ($(this).css('background-color').replace(/\s+/g, '') === 'rgba(0,0,0,0)') {
                color = '#44a7eb';
            } else color = $(this).css('background-color');
        }
        var title = typeof $(this).attr('data-title') === "undefined" ? $(this).attr('title') : $(this).attr('data-title');
        console.log(title)
        ddrivetip(title, color, 300);
    });
    $('.popupRelate2').mouseout(function () {
        hideddrivetip();
    });
    $('.popupRelate2').hover(function (e) {
        $(this).attr('data-title', $(this).attr('title'));
        $(this).removeAttr('title');
    },
        function (e) {
            $(this).attr('title', $(this).attr('data-title'));
        });
});
$(document).on('click',
    '.item-article',
    function (e) {
        e.preventDefault();
        try {
            var self = $(this),
                className = self.attr('class').replace(/\s{2,}/g, ' ').split(' '),
                target = $('#' + className[1], $('.the-article-body'));
            //$('#mucluc_noidung').toggleClass('target-expanded').css('display', '');
            $('html, body').animate({
                scrollTop: target.offset().top - 50
            }, 400);
        } catch (e) {

        }
    });
$('span.dlpopup').on('click', function (e) {
    e.preventDefault();

    var page = $(this).data('url');
    console.log(page)
    $(this).attr('data-modal', 'modal-xlg')
    return modalGetOnText(this, page, 'Nội dung tham chiếu')

});
$(document).on('click', '.doclink.redirect', function () {
    var self = $(this),
        url = self.data('url');
    console.log(typeof (url))
    console.log(url)
    if (!url.includes('noi-dung-tham-chieu') && url.includes('https')) {
        window.open(url, "_blank");
    }
})