$(document).on('click', '.button-get', (e) => {
    var target = e.currentTarget, self = $(target), url = self.data('url'), elementId = self.data('success-id'), value = self.data('value');
    var query = JSON.parse('{' + value + '}')
    console.log(url)
    if (url) {
        $.ajax({
            url: url,
            type: "GET",
            contentType: 'application/json',
            dataType: "html",
            data: query,
            beforeSend: function () {
                if ($(target).prop('tagName') != 'FORM') {
                    $(target).attr('disabled', 'disabled')
                    $(target).data('og', $(target).html())
                    if ($(target).hasClass('cke_element')) {
                        $(target).html('<span style="padding: 0 5px;">Vui lòng đợi...</span>')
                    } else if ($(target).hasClass('cke_button')) {
                        $(target).html('<span style="padding: 0 2px;">...</span>')
                    } else {
                        $(target).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>')
                    }
                }
            },
            success: function (response) {
                if (response) {

                    $(elementId).html(response);
                    removeLoading($(target))
                    $('input[name*=DocRefeId]').val(query.DocId);
                }
            },
            error: function (xhr, status, error) {
                toastMessage(false, "Vui lòng thử lại");
            }
        })
    }
})

$(document).on('change', 'input[type=checkbox][id*=showFooter]', function () {
    if (this.checked) {
        console.log($('.table-group').find('tr[class*=phuluc-footer]'))
        $('.table-group').find('tr[class*=phuluc-footer]').removeClass('d-none')
    }
    else {
        $('.table-group').find('tr[class*=phuluc-footer]').addClass('d-none')
    }
})
$(document).on('change', '.row-groupB', function () {
    var chkActionIds = '';
    $('.table-groupB>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check-input:first');
        if (chk.is(":checked")) {
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(chk).attr('value');
        }

        $('#Command_DocItemIds').attr('value', chkActionIds);
        $('#Command_DocItemFullIds').attr('value', chkActionIds);
    })
})
$(document).on('change', '.row-groupA', function () {
    var chkActionIds = '';
    $('.table-groupA>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check-input:first');
        if (chk.is(":checked")) {
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(chk).attr('value');
        }

        $('#Command_DocItemRefIds').attr('value', chkActionIds);
        $('#Command_DocItemRefFullIds').attr('value', chkActionIds);
    })
})
$(document).on('change', '.relateType', function () {
    var value = $(this).val();
    var docitemId = $('#Command_DocItemFullIds').val()
    console.log(docitemId)
    $('.table-groupB>tbody>tr').each(function (index) {
        var isChecked = false;
        if ($(this).find('.form-check-input:first').val() == docitemId) {
            isChecked = true;
        }
        if (value == 45) {
            if ($(this).hasClass('huongdan')) {
                $(this).removeClass('d-none');
            }

            $(this).find('.form-check-input:first').attr('type', 'checkbox').prop('checked', isChecked);
        }
        else {
            if ($(this).hasClass('huongdan')) {
                $(this).addClass('d-none');
            }
            $(this).find('.form-check-input:first').attr('type', 'radio').prop('checked', isChecked);
        }
    })
    $('#Command_ReplacePhrases').val(''); $('#Command_PositionAdd').val(''); $('#Command_WithPhrase').val('');
    $('label[class*=replace-phrases]').text('Cụm từ bị thay thế');
    $('label[class*=width-phrase]').text('Cụm từ được thay thế');
    if (value == 49 || value == 52) { //dinh chinh || thay the cum tu || bổ xung cụm từ
        $('div[class*=div-replace-phrases]').removeClass('d-none');
        $('div[class*=div-width-phrase]').removeClass('d-none')
    }
    else if (value == 57) { //bổ xung cụm từ
        $('label[class*=replace-phrases]').text('Cụm từ');
        $('label[class*=width-phrase]').text('Cụm từ bổ sung');
        $('div[class*=div-replace-phrases]').removeClass('d-none');
        $('div[class*=div-width-phrase]').removeClass('d-none');
        $('div[class*=positonadd]').removeClass('d-none');
    }
    else if (value == 56) {//bãi bỏ cụm từ
        $('label[class*=replace-phrases]').text('Cụm từ bị bãi bỏ');
        $('div[class*=div-replace-phrases]').removeClass('d-none');
        $('div[class*=div-width-phrase]').addClass('d-none');
        $('div[class*=positonadd]').addClass('d-none');
    }
    else {

        $('div[class*=div-replace-phrases]').addClass('d-none');
        $('div[class*=div-width-phrase]').addClass('d-none');
        $('div[class*=positonadd]').addClass('d-none');
    }
})
$(document).on('change', '.relateTypeEdit', function () {
    var value = $(this).val();
    $('#Command_ReplacePhrases').val(''); $('#Command_PositionAdd').val(''); $('#Command_WithPhrase').val('');
    $('label[class*=replace-phrases]').text('Cụm từ bị thay thế');
    $('label[class*=width-phrase]').text('Cụm từ được thay thế');
    if (value == 49 || value == 52) { //dinh chinh || thay the cum tu || bổ xung cụm từ
        $('div[class*=div-replace-phrases]').removeClass('d-none');
        $('div[class*=div-width-phrase]').removeClass('d-none')
    }
    else if (value == 57) { //bổ xung cụm từ
        $('label[class*=replace-phrases]').text('Cụm từ bổ sung');
        $('label[class*=width-phrase]').text('Cụm từ');
        $('div[class*=div-replace-phrases]').removeClass('d-none');
        $('div[class*=div-width-phrase]').removeClass('d-none');
        $('div[class*=positonadd]').removeClass('d-none');
        $('div[class*=div-referenceText]').addClass('d-none');
    }
    else if (value == 56) {//bãi bỏ cụm từ
        $('label[class*=replace-phrases]').text('Cụm từ bị bãi bỏ');
        $('div[class*=div-replace-phrases]').removeClass('d-none');
        $('div[class*=div-width-phrase]').addClass('d-none');
        $('div[class*=positonadd]').addClass('d-none');
        $('div[class*=div-referenceText]').removeClass('d-none');
    }
    else {
        $('div[class*=div-replace-phrases]').removeClass('d-none');
        $('div[class*=div-width-phrase]').removeClass('d-none');
        $('div[class*=positonadd]').addClass('d-none');
        $('div[class*=div-referenceText]').removeClass('d-none');
    }
})
function getDocItemRelate() {
    var docId = $('input[name*=DocId]').val();
    var docRefId = $('input[name*=DocRefeId]').val();
    var showDocItemRelate = $('input[name*=showDocItemRelate]').is(":checked") ? true : false;
    var relateTypeId = $('.select-relatetype').val()
    var query = {
        docId: parseInt(docId),
        docRefId: parseInt(docRefId),
        showDocItemRelate: showDocItemRelate,
        relateTypeId: relateTypeId
    }
    $.ajax({
        url: "/BongDa24hDocItems/DocItemRelates/Index?handler=AjaxDocItemRelate",
        type: "GET",
        contentType: 'application/json',
        dataType: "html",
        data: query,
        success: function (response) {
            if (response) {

                $('#tblDocItemRelates').html(response);
            }
        },
        error: function (xhr, status, error) {
            toastMessage(false, "Vui lòng thử lại");
        }
    })
}
function getDocItemRelatePopup() {
    var docId = $('input[name*=docId]').val();
    var docItemId = $('input[name*=docItemId]').val();
    var relateTypeId = $('select[name*=relateTypeId]').val();
    var reviewstatusId = $('select[name*=reviewStatusId]').val();
    var query = {
        docId: docId,
        docItemId: docItemId,
        relateTypeId: relateTypeId,
        reviewstatusId: reviewstatusId
    }
    console.log(query)
    $.ajax({
        url: "/BongDa24hDocItems/DocItemRelates/DocItemRelateList?handler=BinDataDocItemRelate",
        type: "GET",
        contentType: 'application/json',
        dataType: "html",
        data: query,
        success: function (response) {
            if (response) {

                $('#tblDocItemRelatePopup').html(response);
            }
        },
        error: function (xhr, status, error) {
            toastMessage(false, "Vui lòng thử lại");
        }
    })
}
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
$(document).on('change', '.checkcontent', function () {
    if ($(this).is(":checked")) {
        $('.contentItem').addClass('d-none');
        $('.contentHtml').removeClass('d-none');
    }
    else {
        $('.contentItem').removeClass('d-none');
        $('.contentHtml').addClass('d-none');
    }
})
$(document).on('change', '.checkcontent', function () {
    if ($(this).is(":checked")) {
        $('.contentItem').addClass('d-none');
        $('.contentHtml').removeClass('d-none');
    }
    else {
        $('.contentItem').removeClass('d-none');
        $('.contentHtml').addClass('d-none');
    }
})
$(document).on('change', '.checkIndex', function () {
    if ($(this).is(":checked")) {
        $('.docItemLead').addClass('d-none');
        $('.docIndex').removeClass('d-none');
    }
    else {
        $('.docItemLead').removeClass('d-none');
        $('.docIndex').addClass('d-none');
    }
})
///Tạo doclink
$(document).on('change', '#Command_LinkType', function () {
    var value = $(this).val();
    var parent = $(this).closest('form');
    var ajaxSuccesUrl = parent.data('ajax-success-url');

    if (value == 'link') {
        $('#tblDoc').removeClass('d-none');
        $('#tblDocItemA').addClass('d-none');
        ajaxSuccesUrl = ajaxSuccesUrl.replace('popup', 'link');

    }
    if (value == 'popup') {
        $('#tblDoc').addClass('d-none');
        $('#tblDocItemA').removeClass('d-none');
        ajaxSuccesUrl = ajaxSuccesUrl.replace('link', 'popup');
        $('.table-docItemRefer>tbody>tr').each(function (index) {
            $(this).find('.form-check-input:first').attr('type', 'checkbox').prop('checked', false);
        })
    }
    if (value == "linkdocitem") {

        $('#tblDoc').addClass('d-none');
        $('#tblDocItemA').removeClass('d-none');
        ajaxSuccesUrl = ajaxSuccesUrl.replace('popup', 'link');
        $('.table-docItemRefer>tbody>tr').each(function (index) {
            console.log(123322)
            $(this).find('.form-check-input:first').attr('type', 'radio').prop('checked', false);
        })
    }
    $('#QueryDocLink_LinkType').val(value).trigger('change')
    parent.data('ajax-success-url', ajaxSuccesUrl);
    parent.attr('data-ajax-success-url', ajaxSuccesUrl);
})
document.addEventListener("selectionchange", event => {
    let selection = window.getSelection();
    // Kiểm tra xem có văn bản được bôi đen hay không
    if (selection.rangeCount > 0) {
        // Lấy phần tử chứa đoạn văn bản được bôi đen
        let selectedElement = selection.anchorNode.parentElement;
        if (selectedElement.tagName === "INPUT" || selectedElement.tagName === "TEXTAREA") {
            return; // Bỏ qua nếu văn bản bôi đen nằm trong một input hoặc textarea
        }
        let trElement = selectedElement.closest("tr.trdocItem");
        if (trElement) {
            let tableElement = trElement.closest("table.table-docItem");
            if (tableElement) {
                var checkboxes = $(tableElement).find(':checkbox');
                checkboxes.prop('checked', false);
            }
            if (trElement) {
                let checkbox = trElement.querySelector("input[type='checkbox']");

                // Checkbox tự động chọn, thực hiện hành động
                if (checkbox) {
                    $(checkbox).prop("checked", true);
                    let selectedText = selection.toString();
                    if (selectedText.length > 0) {
                        $('#Command_TextLink').text(selectedText);
                        $('#Command_TextLink').val(selectedText);
                        if ($('#QueryGetDocGetByKeyWord_KeyWords') && $('#QueryGetDocGetByKeyWord_KeyWords').val() == '') {
                            $('#QueryGetDocGetByKeyWord_KeyWords').text(selectedText);
                            $('#QueryGetDocGetByKeyWord_KeyWords').val(selectedText);
                        }

                    }
                    $('#Command_DocItemIds').attr('value', $(checkbox).val());
                }
            }
        }
    }
});

$(document).on('change', '.row-docItemRefer', function () {
    var chkActionIds = '';
    var docIdRef = 0;
    var siteId = 0
    $('.table-docItemRefer>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check-input:first');
        if (chk.is(":checked")) {

            docIdRef = $(chk).data('docid');
            siteId = $(chk).data('siteid');
            console.log(docIdRef)
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(chk).attr('value');

        }
        if (siteId > 0) {
            $('#Command_SiteReferenceId').attr('value', siteId);
        }
        $('#Command_DocItemReferIds').attr('value', chkActionIds);
    })
    $('#Command_DocReferenceId').attr('value', docIdRef)
})
$(document).on('change', '.row-docItem', function () {
    var chkActionIds = '';
    $('.table-docItem>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check-input:first');
        if (chk.is(":checked")) {
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(chk).attr('value');
        }

        $('#Command_DocItemIds').attr('value', chkActionIds);
    })

})
$(document).on('change', '.row-selectDoc', function () {
    var chkActionIds = '';
    console.log(1)
    $('.table-docLink>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check-input:first');
        if (chk.is(":checked")) {

            $('#Command_DocReferenceId').attr('value', $(chk).attr('value'));
            $('#Command_SiteReferenceId').attr('value', $(chk).data('siteid'));
            $('#Command_UrlLink').attr('value', $(chk).data('url'));
        }


    })

})
$(document).on('change', '#Command_DocReferenceIds', function () {
    var value = $(this).val();
    var siteid = $(this).data('siteid')
    var siteRef = $(this).data('siteref') || 0;
    var linktype = $('#Command_LinkType').val();
    console.log(2, siteRef)
    if (value > 0) {
        $.ajax({
            url: "/BongDa24hDocs/DocStandardFulls/DocItemRefer?handler=BindDataItemRefer&siteId=" + siteid + "&siteRefId=" + siteRef + "&linkType=" + linktype,
            type: "GET",
            contentType: 'application/json',
            dataType: "html",
            data: { docId: value },
            success: function (response) {
                if (response) {
                    html = ` <div class="col-auto col-sm-auto d-flex align-items-center pe-0 mb-2">
                        <h5 class="mb-0 text-nowrap total-rows fw-semi-bold ps-1 fs--1"> <b>Văn bản được dẫn chiếu</b></h5>
                    </div>`
                    $('#tblDocItemA').html(html + response);
                }
            },
            error: function (xhr, status, error) {
                toastMessage(false, "Vui lòng thử lại");
            }
        })
    }
})
$(document).on('change', '.input-relate', function () {
    var chkActionIds = '';
    $('.checkRelateType').each(function () {
        var chk = $(this).find('.form-check-input:first');
        if (chk.is(":checked")) {
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(chk).attr('value');
        }
        $('#Query_RelateTypeIds').attr('value', chkActionIds)
    })
})
onSuccessedDataDocItem = (form, xhr, resetHiddenField = false, isBindData = true) => {
    try {
        var url = $(form).data('ajax-success-url');
        var elementId = $(form).data('ajax-success-id');
        var elementIdList = $(form).data('ajax-success-idlist');
        if (xhr.responseJSON.Succeeded) {
            //Dành cho tạo tham chiếu nôi dung
            $('.table-docItemRefer>tbody>tr').each(function (index) {
                var chk = $(this).find('.form-check-input:first').prop('checked', false);
                //$('.table-docItemRefer').animate({
                //    scrollTop: chk.offset().top - 50
                //}, 400);
                $('#Command_DocItemReferIds').attr('value', '');
            })
            $('.table-docItem>tbody>tr').each(function (index) {
                var chk = $(this).find('.form-check-input:first');
                let selection = window.getSelection();
                if (chk.is(":checked")) {

                    if (selection.rangeCount > 0) {
                        //let selectedText = selection.toString();
                        const div = document.createElement("div");
                        const fragment = selection.getRangeAt(0).cloneContents()
                        div.appendChild(fragment)
                       let selectedText = div.innerHTML
                        if (selectedText.length > 0) {
                            //selectedText = selection.getRangeAt(0).commonAncestorContainer.outerHTML
                           
                            if (div.innerHTML.length > 0) {
                            }
                            $(this).each(function () {

                                var content = $(this).html();
                                var regex = new RegExp("(" + selectedText + ")", "gi");
                                var newContent = content.replace(regex, "<span class='text-link noi-dung-tham-chieu'>$1</span>");
                                $(this).html(newContent);
                            });
                        }
                        else {
                            selectedText = $('#Command_TextLink').val();
                            $(this).each(function () {

                                var content = $(this).html();
                                var regex = new RegExp("(" + selectedText + ")", "gi");
                                var newContent = content.replace(regex, "<span class='text-link noi-dung-tham-chieu'>$1</span>");
                                $(this).html(newContent);
                            });

                        }
                    }
                    //$('.table-docItem').scrollTop();
                    chk.prop('checked', false)
                }
                $('#Command_DocItemIds').attr('value', '');

            })
            if ($(form).find('input[type="checkbox"][name*="AddMoreData"]:checked').length) {
                resetForm(form);
                //Dành cho Tạo liên kết số hóa
                $('.table-groupB>tbody>tr').each(function (index) {
                    var chkB = $(this).find('.form-check-input:first');
                    if (chkB.is(":checked")) {
                        $(this).addClass('item-selected')
                        chkB.prop('checked', false)
                    }

                    $('#Command_DocItemIds').attr('value', '');
                    $('#Command_DocItemFullIds').attr('value', '');
                })
                $('.table-groupA>tbody>tr').each(function (index) {
                    var chkA = $(this).find('.form-check-input:first');
                    if (chkA.is(":checked")) {
                        $(this).addClass('item-selected')
                        chkA.prop('checked', false)
                    }

                    $('#Command_DocItemRefIds').attr('value', '');
                    $('#Command_DocItemRefFullIds').attr('value', '');
                })
            }
            else if (!xhr.responseJSON.ReturnUrl) {
                modalDataPopup.modal('hide')
            }
            if (typeof elementIdList !== "undefined") {
                var elementList = $('#' + elementIdList);
                if ($(elementList).length) {
                    var currentListValue = $(elementList).val();
                    if (typeof currentListValue !== "undefined") {
                        var separator = currentListValue ? "," : "";
                        var idlist = currentListValue + separator + xhr.responseJSON.Id;
                        $(elementList).val(idlist);
                        url = url + "&idlist=" + idlist;
                    }
                }
            }

            if (isBindData) {
                //bindData(xhr.responseJSON.Id);
                var element = $('#' + elementId);
                if (element) {
                    fetchData({
                        type: 'GET',
                        url: url,
                        dataType: 'html'
                    }).then(response => {

                        $(element).html(response);
                    })
                }

            }

            if (xhr.responseJSON.ReturnUrl) {
                window.open(xhr.responseJSON.ReturnUrl)
            }
        }

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        elementAction = form

        removeLoading(form)
        resetTableActions()

        if (resetHiddenField) {
            resetHiddenInput(form);
            var parent = $(form).closest('.bulk-select-actions');
            if (typeof parent !== 'undefined' && parent.length) {
                resetHiddenInput(parent);
            }
        }

    } catch (e) {
        console.log(e)
    }
}
$(document).on('change', 'input[type=checkbox][name*=showOrginalItem]', function () {
    var tr = $('.table-docItem').find('tr[class*=originalItem]');
    originId = tr.data('origin');
    displayId = tr.data('displayid');
    var td = tr.find('td[class*=td-content');
    if (this.checked) {
        tr.removeClass('d-none')
        if (originId > 0) {
            td.addClass('item-selected')
        }
        if (displayId != 2) {
            td.addClass('bg-original')
        }
    }
    else {
        if (originId > 0) {
            tr.addClass('d-none')
        }
        td.removeClass('item-selected')
        td.removeClass('bg-original')
    }
})