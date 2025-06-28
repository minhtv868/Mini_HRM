$(function () {
    $(document).ajaxError(function (event, jqxhr, settings, exception) {
        removeLoading(elementAction)

        if (jqxhr.status == 404) {
            toastMessage(false, `Không tìm thấy dữ liệu theo đường dẫn: ${settings.url}`)
        } else if (jqxhr.status == 401) {
            const returnUrl = `${window.location.pathname}${window.location.search}`
            modalResetSize()
            modalGet(null, `/Identity/Account/Login?ReturnUrl=${encodeURIComponent(returnUrl)}&Modal=true`, 'Đăng nhập hệ thống')
        }
        else if (jqxhr.status == 403) {
            toastMessage(false, 'Bạn không có quyền thực hiện chức năng này.')
        }
        else if (jqxhr.statusText != 'abort') {
            toastMessage(false, 'Vui lòng thử lại sau.')
        }

        if (typeof CKEDITOR != 'undefined') {

            var currentDialog = CKEDITOR.dialog.getCurrent()

            if (currentDialog != null) {
                currentDialog.hide()
            }
        }
    });

    toastMsg.on('hidden.bs.toast', function () {
        toastBody.text('')
    })

    modalPopup.on('hidden.bs.modal', function () {
        modalReset()
    })

    modalDataPopup.on('hidden.bs.modal', function () {
        mediaSelected = []
        newsSelected = []
        //modalReset($('#modalDataPopup'))
    })

    $(document).on('click', '.search-btn', function (event) {
        event.preventDefault()
        var self = $(this), parent = self.closest('.card'), searchText = $('.search', parent).val().toLowerCase()
        $('.list .form-check', parent).each(function () {
            var label = $(this).find('.form-check-label').text().toLowerCase()
            if (label.normalize("NFD").replace(/[\u0300-\u036f]/g, "").includes(searchText.normalize("NFD").replace(/[\u0300-\u036f]/g, ""))) {
                $(this).show()
            } else {
                $(this).hide()
            }
        })
    })
    $(document).on('keypress', '.search', function (event) {
        if (event.which == 13) {
            var self = $(this), parent = self.closest('.card')
            $('.search-btn', parent).click()
        }
    })
    $(document).on('change', 'table .row-checkbox', function () {
        let table = $(this).closest('table')
        selectRow(table)
    })

    $(document).on('change', 'table thead input[class*="form-check-input"]', function () {
        let table = $(this).closest('table'),
            isChecked = $(this).prop('checked')
        table.find('.row-checkbox').prop('checked', isChecked)
        selectRow(table)
    })

    $(document).on('change', '[data-bulk-select]', () => {
        let values = []
        const dataBulk = JSON.parse($('input[data-bulk-select]').attr('data-bulk-select') || '{}'),
            state = $('[data-bulk-select]').is(':checked');
        if (state) {
            $(`#${dataBulk.actions}`).removeClass('d-none')
            $(`#${dataBulk.replacedElement}`).addClass('d-none')
        } else {
            $(`#${dataBulk.actions}`).addClass('d-none')
            $(`#${dataBulk.replacedElement}`).removeClass('d-none')
        }

        $('[data-bulk-select-row]').prop('checked', state);

        values = $('input[type="checkbox"][name="ChkActionIds"]:checked').map(function () {
            return this.value
        }).get().join(',')

        $('input[type="hidden"][name="ChkActionIds"]').val(values)
    })

    $(document).on('change', '[data-bulk-select-row]', () => {
        let values = []
        const dataBulk = JSON.parse($('input[data-bulk-select]').attr('data-bulk-select') || '{}')

        $('input[type="hidden"][name="ChkActionIds"]').val('')

        values = $('input[type="checkbox"][name="ChkActionIds"]:checked').map(function () {
            return this.value
        }).get().join(',')

        $('input[type="hidden"][name="ChkActionIds"]').val(values)

        if (values.length) {
            $(`#${dataBulk.actions}`).removeClass('d-none')
            $(`#${dataBulk.replacedElement}`).addClass('d-none')
        } else {
            $(`#${dataBulk.actions}`).addClass('d-none')
            $(`#${dataBulk.replacedElement}`).removeClass('d-none')
        }
    })

    $('.collapse-filter').click(function () {
        var icon = $(this).find('.material-icons').first(),
            filterText = $(this).find('.filter-text').first(),
            title = $(this).attr('title') == 'Thêm điều kiện lọc' ? 'Ẩn bớt điều kiện lọc' : 'Thêm điều kiện lọc'

        $(this).attr('title', title)
        filterText.text(title)
        if (icon) {
            icon.text(icon.text() == 'expand_more' ? 'expand_less' : 'expand_more')
        }
    })

    $(document).on('blur', '.to-slug', (e) => {
        var target = e.target, self = $(target), isChange = self.data('change') || false,
            slug = $('.slug'), slugTag = $('.slug-tag'), commandSlug = $('.Command.Slug'),
            slugContainer = $('#slug-container')

        if (slug.val().trim().length == 0 || isChange) {
            let slugValue = toSlug(self.val(), '-')
            slug.val(slugValue)
            if (slugTag.length > 0) {
                slugTag.text(slugValue)
            }
            if (commandSlug.length > 0 && commandSlug.val().trim().length == 0) {
                commandSlug.val(slugValue)
            }
            if (slugContainer.length > 0) {
                slugContainer.attr('data-slug', slugValue)
            }
        }
    })

    $(document).on('click', 'a[href^="#collapseBock_"]', (e) => {
        var target = e.target,
            title = $(target).attr('title') == 'Hiển thị dữ liệu' ? 'Ẩn dữ liệu' : 'Hiển thị dữ liệu'

        $(target).attr('title', title)

        $(target).html(title == 'Ẩn dữ liệu' ? '<i class="fas fa-angle-up text-900 fs-2"></i>' : '<i class="fas fa-angle-down text-900 fs-2"></i>')
    })

    $(document).on('change', '[data-required="true"]', (e) => {
        var target = $(e.target),
            fieldValidationError = target.closest('.did-floating-label-content').
                find('.field-validation-error').first()

        if (target.val().trim().length > 0) {

            fieldValidationError.addClass('d-none')

        } else {

            fieldValidationError.removeClass('d-none')

        }
    })

    $(document).on('change', '.select-position', function () {
        var selectedValue = $(this).val();
        if (selectedValue == '4') {
            $('#displayOrderContainer').show();
            $('#displayOrderAfterContainer').hide();
        }
        else if (selectedValue == '3') {
            $('#displayOrderAfterContainer').show();
            $('#displayOrderContainer').hide();
        }
        else {
            $('#displayOrderContainer').hide();
            $('#displayOrderAfterContainer').hide();
        }
    });
    $(document).on('click', '.btn-add-doc', (e) => {
        var self = $(e.target)
        var docId = $("#Query_DocId").val()
        $('.text-validate .text-required').hide();
        var relateTypeId = self.data('relatetypeid');
        var docReferenceIds;
        var docIdentity = self.parent().prev().find(".select2-selection__choice__remove").text();

        var url = $(".reoloadUrl").data('ajax-success-url'),
            docReferenceIds = self.parent().prev().find(".select2-auto-complete-modal").val();
        var command = {
            docId: parseInt(docId),
            relateTypeId: parseInt(relateTypeId),
            docReferenceIds: docReferenceIds,
            reviewStatusId: 1
        }

        if ((docReferenceIds == '' || !docReferenceIds)) {
            self.closest('.box-relate').next('.text-validate ').find('.text-required').show();
            return;
        } else {
            self.closest('.box-relate').next('.text-validate').find('.text-required').hide();

            $.ajax({
                url: "/BongDa24hDocs/DocRelates?handler=AddDocRelates",
                type: "POST",
                contentType: 'application/json',
                dataType: "json",
                data: JSON.stringify(command),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                },
                success: function (response) {
                    if (response && response.Succeeded) {

                        fetchData({
                            type: 'GET',
                            url: url,
                            dataType: 'html',
                        }).then(response => {
                            $("#tblDocRelate").html(response);
                            $('.modal-body').addClass('scroll-item')
                            $('.modal-body').attr('data-select2-id', '')
                            $('.scroll-item').on('scroll', function () {
                                console.log('scrolling');
                            });

                        })
                    }
                    toastMessage(response.Succeeded, response.Messages.join('<br />'))
                    self.parent().prev().find(".select2-auto-complete-modal").val(0).trigger("change");
                },
                error: function (xhr, status, error) {
                    toastMessage(false, "Vui lòng thử lại");
                }
            })
        }
    })

    $(document).on('click', '#categoryTree .toggle-icon', function () {
        const self = $(this),
            parentLi = self.parent('li')
        childUl = parentLi.children('ul')

        //if (childUl.is(':visible')) {
        //    childUl.slideUp('fast')
        //    self.text('+')
        //} else {
        //    childUl.slideDown('fast')
        //    self.text('−')
        //}

        parentLi.toggleClass('expanded');
        self.toggleClass('collapsed expanded');

        parentLi.children('ul').slideToggle('fast');
    })

    $(document).on('click', '#categoryTree .category-checkbox', function () {
        const isChecked = $(this).is(':checked')

        $(this).siblings('ul').find('.category-checkbox').prop('checked', isChecked)

        if (!isChecked) {
            $(this).parents('ul').prev('.category-checkbox').prop('checked', false)
        }
    })

    $(document).on('input', '#categorySearch', function () {
        const searchText = normalizeText($(this).val().toLowerCase())

        $('#categoryTree label').each(function () {
            var label = $(this), text = label.text(),
                normalizedText = normalizeText(text.toLowerCase()),
                highlightedText = text
            if (searchText.length > 0) {
                var index = normalizedText.indexOf(searchText)

                if (index !== -1) {
                    var beforeText = text.substring(0, index)
                    var matchText = text.substring(index, index + searchText.length)
                    var afterText = text.substring(index + searchText.length)

                    highlightedText = beforeText + '<span class="highlight">' + matchText + '</span>' + afterText
                }
            }
            $('.treeview li').each(function () {
                var $this = $(this)
                if ($this.find('ul').length) {
                    $this.find('ul').show()
                }
            })
            label.html(highlightedText)
        })
    })

    $(document).on('change', '.select-link', (e) => {
        var self = $(e.target), id = self.data('id'),
            value = self.find('option:selected').val(),
            urlRequest = self.data('url') || '', linkTo = self.data('link-to'),
            currentValue = $(linkTo).data('value') || '', isFilter = self.data('filter') || false,
            { callbacks } = self.get(0).dataset

        if (value == "...") value = 0

        $(`${linkTo} option`).filter(function () {
            return $(this).val() != '0' && $(this).val() != ''
        }).remove()

        if (typeof id != 'undefined' && typeof linkTo != 'undefined' && urlRequest.trim().length > 0) {

            var data = {}

            data[id] = value

            if (value >= 0) {

                fetchData({
                    url: urlRequest,
                    type: 'get',
                    dataType: 'json',
                    data: data,
                }).then(response => {

                    $.each(response.Data, function (key, value) {
                        $(linkTo)
                            .append($('<option></option>')
                                .attr('value', value.id)
                                .text(value.text))
                    })

                    if (response.Data.length > 0) {
                        if (isFilter) {
                            currentValue = $(`${linkTo} option:first`).val()
                        } else if (response.Data.filter(x => x.id == currentValue).length == 0) {
                            currentValue = response.Data[0].id
                        }

                        $(linkTo).val(currentValue).trigger('change')
                    }
                    else {
                        $(linkTo).val(0).trigger('change')
                    }

                    if (callbacks) {
                        const callbackObjects = JSON.parse(callbacks)
                        callbackObjects.forEach(callbackObj => {
                            const callback = window[callbackObj.name]
                            if (typeof callback === 'function') {
                                const params = callbackObj.params.map(param => {
                                    if (param === '#SiteId#') return value
                                    return param
                                })

                                callback(...params)
                            }
                        })
                    }
                })

            }
        }
    })

    $(document).on('change', '.block-data-sticky', (e) => {
        var target = e.target, self = $(target),
            currentRow = $(target).closest('.row'),
            blockDataActions = currentRow.find('.block-data-actions').first()
        if (blockDataActions) {
            blockDataActions.toggleClass('d-none')
        }
    })

    $(document).on('change', '.rd-dataId', (e) => {
        var target = e.target, modal = $(target).closest('.modal'),
            form = modal.find('.modal-footer form').first()

        if (form) {
            form.find('input[name="DataId"]').val($(target).val())
        }
    })

    $(document).on('change', '.chk-dataId', (e) => {
        var target = e.target, modal = $(target).closest('.modal'),
            form = modal.find('.modal-footer form').first(),
            values = ''

        values = $('input.chk-dataId:checked').map(function () {
            return this.value
        }).get().join(',')

        if (form) {
            form.find('input[name="DataId"]').val(values);
            var dataIdViews = $('input[name="DataId-View"]').val();
            form.find('input[name="DataIdView"]').val(dataIdViews)
        }

    })

    $(document).on('change', '.form-select-submit', (e) => {
        var target = $(e.target), form = target.closest('form')
        if (form) {
            if (target.hasClass('reset-related-select')) {
                $('.related-select option')
                    .removeAttr('selected')
                    .find(':first')
                    .attr('selected', 'selected')
            }

            form.submit()
        }
    })

    $(document).on('click', '.json-view-detail', (e) => {
        const target = e.target, title = $(target).attr('title') || '',
            jsonData = $(target).closest('td').find('.json-view')

        if (jsonData) {
            modalSetSize(target)
            modalPopupTitle.text(title)
            modalPopup.find('.modal-body').first().html('')

            modalPopup.find('.modal-body').first().jsonView(jsonData.text())

            modalPopup.modal('show')
        }
    })

    $(document).on('blur', 'input[id^="selectImgInput"]', (e) => {
        var self = $(e.currentTarget), id = self.attr('id') + '',
            mediaKey = id.replace('selectImgInput', ''),
            selectMediaView = $(`#selectImgView${mediaKey}`),
            inputValue = self.val()

        if (inputValue) {
            selectMediaView.attr('src', `${inputValue}?w=564&h=317`)
            selectMediaView.removeClass('d-none')
        } else {
            selectMediaView.addClass('d-none')
        }
    })

    $(document).on('blur', 'input[id^="SelectMediaInput"]', (e) => {
        var self = $(e.currentTarget), modalDataPopup = $('#modalDataPopup'),
            mediaKey = modalDataPopup.attr('data-key') || '',
            mediaTypeId = modalDataPopup.attr('data-type') || 0,
            mediaTypeName = getMediaType(mediaTypeId),
            selectMediaView = $(`#SelectMediaView${mediaKey}`),
            inputValue = self.val()

        if (mediaTypeName.trim().length > 0) {
            if (mediaTypeName == 'image') {
                if (inputValue) {
                    selectMediaView.attr('src', `${inputValue}?w=564&h=317`)
                    selectMediaView.attr('src-view', `${inputValue}`)
                    selectMediaView.removeClass('d-none')
                } else {
                    selectMediaView.addClass('d-none')
                }
            } else if (mediaTypeName == 'audio') {

            }
        }
    })

    $(document).on('click', 'button[type="submit"]', (e) => {
        elementAction = $(e.currentTarget)
    })

    $(document).on('change', '.editor-media-checkbox', (e) => {
        let self = $(e.currentTarget), isChecked = self.is(':checked'),
            mediaId = self.val(), mediaName = self.attr('data-name') || '',
            mediaPath = self.attr('data-path') || ''

        isExist = mediaSelected.filter(x => x.id && x.id == mediaId)

        if (mediaId.trim().length == 0 || mediaPath.trim().length == 0) {
            showMessage({ message: 'Vui lòng thử lại sau.' })
            return
        }

        if (isChecked) {
            if (isExist.length == 0) {
                mediaSelected.push({
                    id: mediaId,
                    name: mediaName,
                    path: mediaPath
                })
            }
        } else {
            if (isExist.length > 0) {
                mediaSelected.splice(isExist, 1)
            }
        }
    })
    $(document).on('change', '.searchBy', (e) => {
        var self = $(e.target),
            value = self.val(),
            isApi = $('input[name*=isUseApi]').is(":checked");
        console.log(value)
        $('.searchDocRef').data('url', '/BongDa24hDocs/Docs?handler=SearchDocs&isUseApi=' + isApi + '&searchBy=' + value + '')
        initSelect2AutoComplete($('.searchDocRef'))
    })
    $(document).on('change', '.change-site', (e) => {
        var self = $(e.target),
            value = self.val(),
            changeOption = self.data('change');

        if (value > 0) {
            var elements = $('.' + changeOption)
            if (elements.length > 0) {
                elements.each(function (index) {

                    var element = $(this);
                    element.select2('destroy')
                    element.attr("data-siteid", value);
                    element.data("siteid", value);
                    // element.val('').trigger('change')
                    if (element.attr('data-value')) {

                        element.attr('data-value', '');
                        element.data("value", "");
                        var searchUrl = element.data('url');
                        if (searchUrl) {
                            fetchData({
                                type: 'GET',
                                url: searchUrl + "&siteid=" + value,
                                dataType: 'json',
                            }).then(response => {

                                if (response.Data[0]) {
                                    var result = {
                                        id: response.Data[0].id,
                                        text: response.Data[0].text
                                    }
                                    console.log(result)
                                    element.attr('data-value', (JSON.stringify(result)));
                                    element.data("value", JSON.parse(JSON.stringify(result)));
                                    element.data("format", "resultFormatData");
                                    element.attr("data-format", "resultFormatData");
                                    element.data("format-selection", "resultFormatDataSelection");
                                    element.attr("data-format-selection", "resultFormatDataSelection");
                                    element.val(result.id).trigger('change')
                                }
                            })

                        }
                    }
                    initSelect2AutoComplete(element)

                })
            }

        }

    })
    $(document).on('change', '.set-session', (e) => {
        var self = $(e.target),
            value = self.val();
        var elementImgs = $('.upload-icon');
        console.log(value)
        if (value > 0) {
            $.ajax({
                url: '/BongDa24hDocs/Docs/Index?handler=SetSession',
                type: 'GET',
                dataType: 'json',
                data: { siteId: parseInt(value) },
                success: function (response) {

                },
                error: function (xhr, status, error) {
                    console.error('Error setting session:', error);
                }
            });
            if (elementImgs.length > 0) {
                elementImgs.each(function () {
                    var element = $(this);
                    element.attr("data-siteid", value);
                    element.data("siteid", value);
                })
            }

        }
    })
    $(document).on('change', '.select2-auto-complete-modal', (e) => {
        var self = $(e.target), parent = self.closest('.did-floating-label-content'), value = self.val();
        var validate = parent.find('.text-danger');
        if (validate.length > 0) {
            if (value > 0) {
                validate.addClass('text-danger d-none')
            }
            else {
                validate.removeClass('d-none')

            }
        }
    })
    $(document).on('click', '.previewhtml', function (e) {
        var url = $(e.target).data('url'), elementid = $(e.target).data('success-id');

        if (url) {
            fetchData({
                type: 'GET',
                url: url,
                dataType: 'json',
                beforeSend: function () {
                    if ($(e.target).prop('tagName') != 'FORM') {
                        $(e.target).attr('disabled', 'disabled')
                        $(e.target).data('og', $(e.target).html())
                        if ($(e.target).hasClass('cke_element')) {
                            $(e.target).html('<span style="padding: 0 5px;">Vui lòng đợi...</span>')
                        } else if ($(e.target).hasClass('cke_button')) {
                            $(e.target).html('<span style="padding: 0 2px;">...</span>')
                        } else {
                            $(e.target).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>')
                        }
                    }
                },
            }).then(response => {
                $("#" + elementid).addClass('p-5')
                $("#" + elementid).html(response.Data)
                removeLoading($(e.target))
                $('html, body').animate({
                    scrollTop: $("#" + elementid).offset().top - 50
                }, 400);
            })
        }
    })
    $(document).on('click', '.btn-up-file, .btn-up-file *', (e) => {
        var target = e.target, self = $(target),
            ip = $(e.target).hasClass('btn-up-file') ? $(e.target) : $(e.target).closest('.btn-up-file'),
            docId = ip.attr('data-docid'),
            ajaxUrl = ip.attr('data-ajax-url'),
            siteId = ip.attr('data-siteid'),
            parent = self.closest('.uploadgroup'),
            dataSourceId = parent.find("select[name*=DataSourceId]").first().find(":selected").val(),
            fileTypeId = parent.find("select[name*=FileTypeId]").first().find(":selected").val(),
            isFileStandard = parent.find("input[name*=IsFileStandard]").first().prop('checked'),
            fileElement = parent.find("input[name*=FromFiles]").first();
        var files = $(fileElement).prop('files');

        if (files.length > 0) {

            elementAction = ip;

            if (docId && siteId) {

                $('#FileCommand_DocId').val(docId);
                $('#FileCommand_SiteId').val(siteId);
                $('#FileCommand_DataSourceId').val(dataSourceId);
                $('#FileCommand_FileTypeId').val(fileTypeId);
                $('#FileCommand_IsFileStandard').val(isFileStandard);
                $('#FileUploads').closest('form').attr('data-ajax-progress', 'progress_' + docId);
                $('#FileUploads').closest('form').attr('data-ajax-url', ajaxUrl);
                //$('#FileUploads').trigger('click');
                $('#FileUploads').val('');
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    $('#FileUploads').prop('files', files);
                };
                $('#FileUploads').closest('form').submit();

                return false;
            }
        } else {
            toastMessage(false, "Vui lòng chọn file!")
        }
    })
    //$(document).on('click', '.btn-addmore', function () {
    //    var element = $(this);
    //    $('input[type="checkbox"][name*="AddMoreData"]').attr('checked', 'checked')

    //})
    if (cardHeaderSecondary.length) {
        $(window).scroll(function () {
            cardHeaderSecondary.toggleClass('fix-header', $(window).scrollTop() > cardHeaderSecondary.position().top);
        });
    }

    verticalNavbarInit()


    flatpickrInit()

    initViewMaxLength()

    initCkEditor()

    initSelect2AutoComplete()

    initTooltip()

    initSystemNotification()
    stickyHeaderTableInit()

})
var systemNotiInterval;
var NotifyMaintenanceKey = "NOTIFY_MAINTENANCE_CMS";

let dataSystemUpdate = () => {
    return fetchData({
        type: 'get',
        url: '/?handler=SystemUpdate',
        dataType: 'json',
        cache: true
    })
}

var initSystemNotification = () => {
    //try {
    //    if (systemNotiInterval) {
    //        clearInterval(systemNotiInterval)
    //    }
    //    getCmsUpdateNoti();

    //    systemNotiInterval = setInterval(getCmsUpdateNoti, 30000);

    //} catch {
    //    if (systemNotiInterval) {
    //        clearInterval(systemNotiInterval)
    //    }
    //}

}
var getCmsUpdateNoti = async () => {
    let [dataSystemUpdateResponse] = await Promise.all([dataSystemUpdate()]).catch((e) => { console.log(e) });
    if (dataSystemUpdateResponse.Succeeded && dataSystemUpdateResponse.Data && dataSystemUpdateResponse.Data.SettingValue.length > 0 && dataSystemUpdateResponse.Data.SettingValue != '0') {
        showToastSystem(false, dataSystemUpdateResponse.Data.SettingValue);
    } else {
        localStorage.setItem(NotifyMaintenanceKey, '1');
        hideToastSystem();
    }
}

var initTooltip = (parent) => {
    if (parent) {
        $('[data-bs-toggle="tooltip"]', parent).tooltip('dispose')
        $('[data-bs-toggle="tooltip"]', parent).tooltip()
    }
    else {
        $('[data-bs-toggle="tooltip"]').tooltip('dispose')
        $('[data-bs-toggle="tooltip"]').tooltip()
    }
}
var initViewMaxLength = () => {
    if ($('div.view-maxlength').length) {
        $('div.view-maxlength').maxlength()
    }
};

var initCkEditor = () => {
    const editors = $('.ckeditor-input')
    if (editors.length > 0) {

        editors.each((index, item) => {
            var input = $(item), config = input.data('config') || 'basic', mode = input.data('mode') || ''
            maxlength = input.attr('maxlength'), backgroundColor = input.data('bg-color') || '#fff'
            height = input.data('height') || '200px'
            if (mode == 'inline') {

                CKEDITOR.inline(input.attr('id'), {
                    disableNativeSpellChecker: true,
                    versionCheck: false,
                    scayt_autoStartup: false,
                    customConfig: `/vendors/ckeditor/configs/${config}.js`,
                    maxLength: maxlength,
                    height: height
                })

            } else {

                CKEDITOR.replace(input.attr('id'), {
                    entities: false,
                    disableNativeSpellChecker: true,
                    versionCheck: false,
                    scayt_autoStartup: false,
                    customConfig: `/vendors/ckeditor/configs/${config}.js`,
                    maxLength: maxlength,
                    height: height,
                    on: {
                        instanceReady: function () {
                            this.editable().setStyle('background-color', backgroundColor);
                        },
                    }
                })

            }
        })
    }
    const editors2 = $('.ckeditor-input-2')
    if (editors2.length > 0) {

        editors2.each((index, item) => {
            var input = $(item), config = input.data('config') || 'basic', mode = input.data('mode') || ''
            maxlength = input.attr('maxlength'), backgroundColor = input.data('bg-color') || '#fff'
            height = input.data('height') || '200px'

            CKEDITOR.replace(input.attr('id'), {
                entities: false,
                disableNativeSpellChecker: true,
                removePlugins: 'a11ychecker',
                versionCheck: false,
                scayt_autoStartup: false,
                customConfig: `/vendors/ckeditor/configs/${config}.js`,
                toolbar: [
                    { name: 'styles', items: ['Styles'] },
                    { name: 'basicstyles', items: ['Bold', 'Italic'] },
                    { name: 'colors', items: ['TextColor', 'BGColor'] },
                    { name: 'document', items: ['Source'] }
                ],
                maxLength: maxlength,
                height: height,
                extraPlugins: 'stylescombo', // Thêm plugin stylescombo
                stylesSet: [ // Định nghĩa các style tùy chỉnh
                    { name: 'Mục lục 1', element: 'span', attributes: { 'class': 'heading1' } },
                    { name: 'Mục lục 2', element: 'span', attributes: { 'class': 'heading2' } },
                    { name: 'Mục lục 3', element: 'span', attributes: { 'class': 'heading3' } },
                    { name: 'Mục lục 4', element: 'span', attributes: { 'class': 'heading4' } },
                    { name: 'Mục lục 5', element: 'span', attributes: { 'class': 'heading5' } },
                    { name: 'Mục lục 6', element: 'span', attributes: { 'class': 'heading6' } }
                ],
                on: {
                    instanceReady: function () {
                        this.editable().setStyle('background-color', backgroundColor);
                    },
                }
            })
        })
    }

}

var verticalNavbarInit = () => {
    const linkActive = $('.nav-link.active', '#navbarVerticalNav')

    if (linkActive.length > 0) {
        const parent = linkActive.closest('ul.nav.parent'),
            children = linkActive.closest('ul.nav.children')

        if (parent.length > 0) {
            parent.addClass('show')
            parent.prev('.nav-link.dropdown-indicator').addClass('active')
        }

        if (children.length > 0) {
            children.addClass('show')
            children.prev('.nav-link.dropdown-indicator').addClass('active')
        }
    }
}

var stickyHeaderTableInit = () => {
    if ($('#sticky-table').length) {
        $('#sticky-table').bootstrapTable({
            stickyHeader: true,
            stickyHeaderOffsetY: stickyHeaderOffsetY
        })
    }
}

var modalPopup = $('#modalPopup'),
    modalDialog = modalPopup.find('.modal-dialog').first(),
    modalPopupTitle = modalPopup.find('.modal-title').first(),
    modalPopupContent = modalPopup.find('.modal-content').first(),
    modalDataPopup = $('#modalDataPopup'),
    modalDataDialog = modalDataPopup.find('.modal-dialog').first(),
    modalDataPopupTitle = modalDataPopup.find('.modal-title').first(),
    modalDataPopupContent = modalDataPopup.find('.modal-content').first(),
    modalSpotlightPopup = $('#modalSpotlightPopup'),
    modalSpotlightDialog = modalSpotlightPopup.find('.modal-dialog').first(),
    modalSpotlightPopupTitle = modalSpotlightPopup.find('.modal-title').first(),
    modalSpotlightPopupContent = modalSpotlightPopup.find('.modal-content').first(),
    modalDataChildPopup = $('#modalDataChildPopup'),
    modalDataChildDialog = modalDataChildPopup.find('.modal-dialog').first(),
    modalDataChildPopupTitle = modalDataChildPopup.find('.modal-title').first(),
    modalDataChildPopupContent = modalDataChildPopup.find('.modal-content').first(),
    spotlightArray = [],
    toastMsg = $('#toastMsg'),
    toastHeader = toastMsg.find('.toast-header'),
    toastBody = toastMsg.find('.toast-body'), elementAction,
    toastSystem = $('#toastMsgSystem'),
    toastSystemHeader = toastSystem.find('.toast-header'),
    toastSystemBody = toastSystem.find('.toast-body'),
    toastSystemMini = $('#minNotifyMsgSystem'),
    tableActions = $('#table-actions'),
    tableReplaceElement = $('#table-replace-element'),
    stickyTable = $('#sticky-table'),
    cardHeaderSecondary = $('.card-header.secondary'),
    mediaTypeArray = [
        { id: 1, name: 'image', desc: 'ảnh' }, { id: 2, name: 'video', desc: 'video' }, { id: 3, name: 'audio', desc: 'audio' },
        { id: 4, name: 'document', desc: 'văn bản' }, { id: 5, name: 'others', desc: 'loại khác' }, { id: 5, name: 'voiceaudio', desc: 'giọng đọc' }
    ]
stickyHeaderOffsetY = 58, mediaSelected = [], newsSelected = [];

toastMessage = (status, message) => {
    if (status == true) {
        toastHeader.removeClass('bg-danger').addClass('bg-primary')
    } else {
        toastHeader.removeClass('bg-primary').addClass('bg-danger')
    }

    toastBody.html(message)
    toastMsg.toast('show');
}
showToastSystem = (status, message) => {

    if (status == true) {
        toastSystemHeader.removeClass('bg-danger').addClass('bg-primary');
        toastSystemBody.removeClass('bg-danger-subtle').addClass('bg-primary-subtle').removeClass('text-danger').addClass('text-primary');
    } else {
        toastSystemHeader.removeClass('bg-primary').addClass('bg-danger');
        toastSystemBody.removeClass('bg-primary-subtle').addClass('bg-danger-subtle').removeClass('text-primary').addClass('text-danger');
    }

    toastSystemBody.html(message);
    toastSystemMini.find('.rounded-start').attr('data-bs-original-title', message).attr('aria-label', message);
    const show = localStorage.getItem(NotifyMaintenanceKey);

    if (show == null || show == '1') {

        toastSystem.toast('show');
        toastSystemMini.hide();
    } else {
        toastSystemMini.show();
        toastSystem.toast('hide');

    }
    toastSystemMini.find('.rounded-start').off('click').on('click', function () {
        toastSystemMini.hide();
        toastSystem.toast('show');
        localStorage.setItem(NotifyMaintenanceKey, '1');
    });
    toastSystem.find('.btn-close').off('click').on('click', function () {
        localStorage.setItem(NotifyMaintenanceKey, '0');
        toastSystem.toast('hide');
        toastSystemMini.show();
    });
}
hideToastSystem = () => {
    toastSystem.toast('hide');
    toastSystemMini.hide();
}

onBegin = form => {
    let element = $(form),
        buttonAction = element,
        page = element.find('input[type="hidden"][name="page"]')

    if (element.prop('tagName') == 'FORM') {
        const buttonSubmit = element.find('button[type="submit"]');
        if (buttonSubmit.length == 1) {
            buttonAction = buttonSubmit
        } else {
            buttonAction = elementAction
        }
    }

    if (page.length > 0) {
        page.val(1)
    }

    elementAction = buttonAction

    if ($(buttonAction).prop('tagName') != 'FORM') {
        $(buttonAction).attr('disabled', 'disabled')
        $(buttonAction).data('og', $(buttonAction).html())
        if ($(buttonAction).hasClass('cke_element')) {
            $(buttonAction).html('<span style="padding: 0 5px;">Vui lòng đợi...</span>')
        } else if ($(buttonAction).hasClass('cke_button')) {
            $(buttonAction).html('<span style="padding: 0 2px;">...</span>')
        } else {
            $(buttonAction).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>')
        }
    }
}

onBegin2 = form => {
    let element = $(form),
        buttonAction = element,
        page = element.find('input[type="hidden"][name="page"]')

    if (element.prop('tagName') == 'FORM') {
        const buttonSubmit = element.find('button[type="submit"]');
        if (buttonSubmit.length == 1) {
            buttonAction = buttonSubmit
        } else {
            buttonAction = elementAction
        }
    }

    if (page.length > 0) {
        page.val(1)
    }

    elementAction = buttonAction
}

removeLoading = form => {
    let timeout
    let element = $(form),
        buttonAction = element
    elementAction = form
    console.log(buttonAction)
    if (element.prop('tagName') == 'FORM') {
        buttonAction = element.find('button[type="submit"]:disabled')
    }

    if (timeout) {
        clearTimeout(timeout)
    }

    timeout = setTimeout(() => {
        buttonAction.removeAttr('disabled')
        buttonAction.html(buttonAction.data('og'))
        timeout = null
    }, 300)
}

setRequestHeader = (form, xhr) => {
    xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
    onBegin(form)
}

onLoginSuccessed = (form, xhr) => {
    try {
        elementAction = form

        if (!xhr.responseJSON.succeeded) {
            removeLoading(form)
        }

        if (xhr.responseJSON.messages) {
            toastMessage(xhr.responseJSON.succeeded, xhr.responseJSON.messages)
        }

        if (xhr.responseJSON.returnUrl) {
            window.location.href = xhr.responseJSON.returnUrl
        }
    } catch (e) {
        console.log(e)
    }
}

onSuccessed = (form, xhr, resetHiddenField = false, isBindData = true) => {
    try {

        if (xhr.responseJSON.Succeeded) {
            const modal = $(form).closest('.modal'), modalId = modal.attr('id'),
                modalParentId = modal.data('pid'), { callbacks, refresh } = form.dataset

            if ($('input[type="checkbox"][name*="AddMoreData"]:checked').length) {
                resetForm(form)
            }
            else if (!refresh && !xhr.responseJSON.ReturnUrl) {
                modalPopup.modal('hide')
                $(form).closest('.modal').modal('hide')
            }

            if (isBindData) {
                bindData(xhr.responseJSON.Id)
            }
            else {
                location.reload();
            }
            if (xhr.responseJSON.ReturnUrl) {
                window.open(xhr.responseJSON.ReturnUrl)
            }

            if (refresh) {
                refreshModal(modalId)
            }

            if (modalParentId) {
                refreshModal(modalParentId)
            }
        }

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        elementAction = form

        removeLoading(form)

        resetTableActions()

        if (resetHiddenField) {
            resetHiddenInput(form)
        }
    } catch (e) {
        console.log(e)
    }
}

onSuccessed2 = (form, xhr, resetHiddenField = false, isBindData = true, advertId) => {
    try {

        if (xhr.responseJSON.Succeeded) {
            const modal = $(form).closest('.modal'), modalId = modal.attr('id'),
                modalParentId = modal.data('pid'), { callbacks, refresh } = form.dataset

            resetForm(form)

            if (isBindData) {
                bindData2(advertId, xhr.responseJSON.Id)
            }
            else {
                location.reload();
            }
            if (xhr.responseJSON.ReturnUrl) {
                window.open(xhr.responseJSON.ReturnUrl)
            }

            if (refresh) {
                refreshModal(modalId)
            }

            if (modalParentId) {
                refreshModal(modalParentId)
            }
        }

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        elementAction = form

        removeLoading(form)

        resetTableActions()

        if (resetHiddenField) {
            resetHiddenInput(form)
        }
    } catch (e) {
        console.log(e)
    }
}

onSuccessed3 = (form, xhr, resetHiddenField = false, isBindData = true, siteId = 0, advertPositionId = 0, categoryId = 0, dataTypeId = 0) => {
    try {

        if (xhr.responseJSON.Succeeded) {
            const modal = $(form).closest('.modal'), modalId = modal.attr('id'),
                modalParentId = modal.data('pid'), { callbacks, refresh } = form.dataset

            resetForm(form)

            if (isBindData) {
                bindData3(siteId, advertPositionId, categoryId, dataTypeId, xhr.responseJSON.Id)
            }
            else {
                location.reload();
            }
            if (xhr.responseJSON.ReturnUrl) {
                window.open(xhr.responseJSON.ReturnUrl)
            }

            if (refresh) {
                refreshModal(modalId)
            }

            if (modalParentId) {
                refreshModal(modalParentId)
            }
        }

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        elementAction = form

        removeLoading(form)

        resetTableActions()

        if (resetHiddenField) {
            resetHiddenInput(form)
        }
    } catch (e) {
        console.log(e)
    }
}

onSuccessed4 = (form, xhr, resetHiddenField = false, isBindData = true, siteId = 0, advertPositionId = 0, categoryId = 0, dataTypeId = 0) => {
    try {
        if (xhr.responseJSON.Succeeded) {
            const modal = $(form).closest('.modal'), modalId = modal.attr('id'),
                modalParentId = modal.data('pid'), { callbacks, refresh } = form.dataset

            if ($('input[type="checkbox"][name*="AddMoreData"]:checked').length) {
                resetForm(form)
            }
            else if (!refresh && !xhr.responseJSON.ReturnUrl) {
                /*modalPopup.modal('hide')*/
                $(form).closest('.modal').modal('hide')
            }

            if (isBindData) {
                bindData3(siteId, advertPositionId, categoryId, dataTypeId, xhr.responseJSON.Id)
            }
            else {
                location.reload();
            }
            if (xhr.responseJSON.ReturnUrl) {
                window.open(xhr.responseJSON.ReturnUrl)
            }

            if (refresh) {
                refreshModal(modalId)
            }

            if (modalParentId) {
                refreshModal(modalParentId)
            }
        }

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        elementAction = form

        removeLoading(form)

        resetTableActions()

        if (resetHiddenField) {
            resetHiddenInput(form)
        }
    } catch (e) {
        console.log(e)
    }
}

bindData2 = (advertId, id, element) => {
    id = id || ''
    element = element || $('.table-responsive2')

    const url = new URL(window.location.href),
        path = url.pathname,
        bindDataParamsElement = $('.bulk-select-replace-element'),
        { callbacks } = $(element).get(0).dataset

    if (bindDataParamsElement) {

        const bindDataParams = bindDataParamsElement.data('params')

        if (bindDataParams) {
            $.each(bindDataParams, function (name, value) {
                url.searchParams.set(name, value)
            });
        }
    }

    var query = url.search

    //if ($('.not-found').length > 0) {
    //    resetForm('.search-form')
    //    historyPushState(path)
    //}
    if (query.length > 0) {
        query = `&${query.substring(1)}`
    }
    fetchData({
        type: 'GET',
        url: `${path}/Position?handler=BindDataAdvertPosition${query}` + `&Query.AdvertId=${advertId}`,
        dataType: 'html'
    }).then(response => {

        $(element).html(response)

        const isEmpty = $(response).filter('table').
            find('.not-found').first().length > 0

        if (!isEmpty) {


            if (id.length > 0) {
                hightLightRow(id, element)
            }

            initTooltip()

            if (callbacks) {
                const callbackObjects = JSON.parse(callbacks)
                callbackObjects.forEach(callbackObj => {
                    const callback = window[callbackObj.name]
                    if (typeof callback === 'function') {
                        callback(...callbackObj.params)
                    }
                })
            }
            stickyHeaderTableInit()
        }
    })
}
bindData3 = (siteId, advertPositionId, categoryId, dataTypeId, id, element) => {
    id = id || ''
    element = element || $('.table-responsive2')

    const url = new URL(window.location.href),
        path = url.pathname,
        bindDataParamsElement = $('.bulk-select-replace-element'),
        { callbacks } = $(element).get(0).dataset

    if (bindDataParamsElement) {

        const bindDataParams = bindDataParamsElement.data('params')

        if (bindDataParams) {
            $.each(bindDataParams, function (name, value) {
                url.searchParams.set(name, value)
            });
        }
    }

    var query = url.search

    //if ($('.not-found').length > 0) {
    //    resetForm('.search-form')
    //    historyPushState(path)
    //}
    if (query.length > 0) {
        query = `&${query.substring(1)}`
    }
    fetchData({
        type: 'GET',
        url: `${path}/AddAdvert?handler=BindData${query}&Query.SiteId=${siteId}&Query.AdvertPositionId=${advertPositionId}&Query.CategoryId=${categoryId}&Query.DataTypeId=${dataTypeId}`,
        dataType: 'html'
    }).then(response => {

        $(element).html(response)

        const isEmpty = $(response).filter('table').
            find('.not-found').first().length > 0

        if (!isEmpty) {


            if (id.length > 0) {
                hightLightRow(id, element)
            }

            initTooltip()

            if (callbacks) {
                const callbackObjects = JSON.parse(callbacks)
                callbackObjects.forEach(callbackObj => {
                    const callback = window[callbackObj.name]
                    if (typeof callback === 'function') {
                        callback(...callbackObj.params)
                    }
                })
            }
            stickyHeaderTableInit()
        }
    })
}

onUploadFileSuccessed = (element, xhr) => {
    try {
        removeLoading(element);
        const btnUpFile = $('.btn-up-file');
        if (btnUpFile.length) {
            removeLoading(btnUpFile);
            console.log(123, btnUpFile)
        }
        var btnUpFileTextbox = $('.btn-up-file-to-textbox');
        if (btnUpFileTextbox.length) {
            removeLoading(btnUpFileTextbox);
        }
        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }
        console.log(234, element)
        if (xhr.responseJSON.Data != null) {
            for (var i = 0; i < xhr.responseJSON.Data.length; i++) {
                let isExist = mediaSelected.filter(x => x.id && x.id == xhr.responseJSON.Data[i].id)

                if (isExist.length == 0) {
                    mediaSelected.push({
                        id: xhr.responseJSON.Data[i].id,
                        name: xhr.responseJSON.Data[i].name,
                        path: xhr.responseJSON.Data[i].path
                    })
                }
            }
        }

        const getBlockDataButton = $('.media-search').first()

        if (getBlockDataButton) {
            $(getBlockDataButton).trigger('click')
        }
        //refresh
        var docId, siteId, isFileStandard;
        if ($('#FileCommand_DocId').length) {
            docId = $('#FileCommand_DocId').val();
        }
        if ($('#FileCommand_SiteId').length) {
            siteId = $('#FileCommand_SiteId').val();
        }
        if ($('#FileCommand_IsFileStandard').length) {
            isFileStandard = $('#FileCommand_IsFileStandard').val();
        }
        if (docId != undefined && docId != undefined && docId != undefined) {
            //Nếu là box uploadfile
            removeLoading(element);
            var idTable = 'table_' + docId;
            var table = $('#' + idTable);
            console.log(idTable)
            if (table.length) {
                var url = table.attr('data-ajax-url');
                if (url != null) {
                    fetchData({
                        type: 'GET',
                        url: url,
                        dataType: 'html'
                    }).then(response => {
                        $(table).html(response);
                    })
                }
            }
        }

    } catch (e) {
        console.log(e)
    }
}

onSuccessedDataPopup = (form, xhr, resetHiddenField = false, isBindData = true) => {
    try {
        var url = $(form).data('ajax-success-url');
        var elementId = $(form).data('ajax-success-id');
        var elementIdList = $(form).data('ajax-success-idlist');
        var elementHidden = $(form).data('hidden');
        if (xhr.responseJSON.Succeeded) {

            if ($(form).find('input[type="checkbox"][name*="AddMoreData"]:checked').length) {
                resetForm(form);
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
                if ($(elementHidden).length > 0) {
                    $(elementHidden).html('')
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

onSuccessedDataChildPopup = (form, xhr, resetHiddenField = false, isBindData = true) => {
    try {
        var url = $(form).data('ajax-success-url');
        var elementId = $(form).data('ajax-success-id');
        var elementIdList = $(form).data('ajax-success-idlist');
        if (xhr.responseJSON.Succeeded) {

            if ($(form).find('input[type="checkbox"][name*="AddMoreData"]:checked').length) {
                resetForm(form);
            } else if (!xhr.responseJSON.ReturnUrl) {
                modalDataChildPopup.modal('hide')
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
                bindData(xhr.responseJSON.Id);
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

        removeLoading(elementAction)

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

onSuccessedNativeAdvert = (form, xhr, resetHiddenField = false, isBindData = true) => {
    try {

        removeLoading(form)

        if (xhr.responseJSON.Succeeded) {

            if ($('input[type="checkbox"][name*="AddMoreData"]:checked').length) {
                resetForm(form)
            } else if (!xhr.responseJSON.ReturnUrl) {
                modalPopup.modal('hide')
            }

            if (isBindData && !xhr.responseJSON.ReturnUrl && xhr.responseJSON.Data == null) {
                bindData(xhr.responseJSON.Id)
            }

            if (xhr.responseJSON.Data != null && xhr.responseJSON.Data == 'Preview') {

                let previewIframe = document.getElementById('preview-ads');

                previewIframe.innerHTML = xhr.responseJSON.ReturnUrl;

                let previewCode = document.getElementById('preview-ads-code');

                previewCode.innerHTML = xhr.responseJSON.ReturnUrl.replaceAll("?preview=1", "");

                document.getElementById('view-code').style.display = "block";

                $('#preview-ads').contents().find("html, body").animate({ scrollTop: 500 }, { duration: 'medium', easing: 'swing' })
            }
            else if (xhr.responseJSON.ReturnUrl) {

                window.open(xhr.responseJSON.ReturnUrl)

            }
        }

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        elementAction = form

        resetTableActions()

        if (resetHiddenField) {
            resetHiddenInput(form)
        }
    } catch (e) {
        console.log(e)
    }
}

nativeAdvertPreviewSubmit = (element) => {
    var form = $(element).closest('form')

    if (form) {

        elementAction = form

        form.find('#Command_Preview').val(true)

        form.submit()
    }

    return false
}

onBlockDataSortableSuccessed = (element, xhr) => {
    try {
        removeLoading(element)
        flatpickrTimeInit()
        sortableBlockData('.block-data-item')
        $(element).closest('.sortable-item').find('.card-body').addClass('show')
    } catch (e) {
        console.log(e)
    }
}

onBlockDataRefresh = (element, xhr) => {
    try {
        var sortableItem = $(element).closest('.sortable-item'),
            cardBody = sortableItem.find('.card-body').first(),
            getBlockDataButton = sortableItem.find('.get-block-data').first(),
            urlRequest = getBlockDataButton ? getBlockDataButton.data('ajax-url') || '' : ''

        removeLoading(element)

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        if (xhr.responseJSON.Succeeded) {
            blockDataBinData(urlRequest, cardBody)
        }
    } catch (e) {
        console.log(e)
    }
}

onBlockAddDataSuccessed = (element, xhr) => {
    try {
        removeLoading(element)

        if (xhr.responseJSON.Succeeded) {
            modalPopup.modal('hide')
        }

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        if (xhr.responseJSON.Id) {
            const sortableItem = $(`.sortable-item[id="${xhr.responseJSON.Id}"]`),
                getBlockDataButton = sortableItem.find('.get-block-data').first()

            if (getBlockDataButton) {
                $(getBlockDataButton).trigger('click')
            }
        }
    } catch (e) {
        console.log(e)
    }
}

onBlockAddDataFilterSuccessed = (element, xhr) => {
    try {
        removeLoading(element)

        //if (xhr.responseJSON.Messages) {
        //    toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        //}

        if (xhr.responseJSON.Succeeded) {
            const currentPage = $('#Item2_Page').val()
            $('input[type="hidden"][name="page"]').val(currentPage)
            $('.btn-news-block-data-not-display-filter').trigger('click')
        }
        //$('.modal-footer input[type="hidden"][name="DataId"]').val('')

    } catch (e) {
        console.log(e)
    }
}

onBindDataFilterSuccessed = (element, xhr) => {
    try {
        removeLoading(element)

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        if (xhr.responseJSON.Succeeded) {
            const parent = $(element).closest('.tab-content'),
                currentPage = parent.find('.page-input').first().val()

            $('input[type="hidden"][name="page"]').val(currentPage)

            $('.btn-binddata-filter').trigger('click')
        }
        //$('.modal-footer input[type="hidden"][name="DataId"]').val('')

    } catch (e) {
        console.log(e)
    }
}

onBlockDataNotDisplaySuccessed = (element, xhr) => {
    try {
        removeLoading(element)

        //var values = $('input.chk-dataId:checked').map(function () {
        //    return this.value
        //}).get().join(',')

        //$('.modal-footer input[type="hidden"][name="DataId"]').val(values)

    } catch (e) {
        console.log(e)
    }
}

onFavoriteFunctionSuccessed = (element, xhr) => {
    try {
        removeLoading(element)

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        const title = $(element).attr('title')

        if (xhr.responseJSON.Succeeded) {

            $(element).attr('title',
                title == 'Thêm chức năng ưa thích'
                    ? 'Bỏ chức năng ưa thích'
                    : 'Thêm chức năng ưa thích')

            $(element).toggleClass('active')
        }

    } catch (e) {
        console.log(e)
    }
}

onBindDataSelectSuccessed = (element, xhr) => {
    try {
        //console.log(xhr)
        var resetElement = $(element).data('ajax-reset') || false;
        removeLoading(element)
        resetTableActions()
        if (resetElement) {
            resetForm($(element))
        }
        console.log(xhr)
        if (xhr.responseText) {

            const response = $(xhr.responseText)

            const isEmpty = response.filter('table').
                find('.not-found').first().length > 0

            if (!isEmpty) {

                if ($('#sticky-table-modal').length) {
                    $('#sticky-table-modal').bootstrapTable({
                        stickyHeader: true,
                        stickyHeaderOffsetY: stickyHeaderOffsetY
                    })
                }

                initTooltip(modalDataPopupContent)
            }

            const paginationAjax = response.filter('#PaginationAjaxPartial')

            if (paginationAjax.length > 0) {

                modalDataPopup.find('.modal-footer').first().html(paginationAjax.html())

            }

            if (mediaSelected.length > 0) {
                $('.editor-media-checkbox').each(function () {
                    let id = $(this).val()

                    $(this).prop('checked', mediaSelected.findIndex(x => x.id == id) != -1)
                });
            }

            if (newsSelected.length > 0) {
                $('.editor-news-checkbox').each(function () {
                    let id = $(this).val()

                    $(this).prop('checked', newsSelected.findIndex(x => x.id == id) != -1)
                });
            }

        }
        if (xhr) {
            toastMessage(xhr.Succeeded, xhr.Messages.join('<br />'))
        }

    } catch (e) {
        console.log(e)
    }
}

onUploadSuccessed = (element, xhr) => {
    try {
        removeLoading(element)

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        if (xhr.responseJSON.Data != null) {
            for (var i = 0; i < xhr.responseJSON.Data.length; i++) {
                let isExist = mediaSelected.filter(x => x.id && x.id == xhr.responseJSON.Data[i].id)

                if (isExist.length == 0) {
                    mediaSelected.push({
                        id: xhr.responseJSON.Data[i].id,
                        name: xhr.responseJSON.Data[i].name,
                        path: xhr.responseJSON.Data[i].path
                    })
                }
            }
        }

        const getBlockDataButton = $('.media-search').first()

        if (getBlockDataButton) {
            $(getBlockDataButton).trigger('click')
        }

    } catch (e) {
        console.log(e)
    }
}

blockDataBinData = (urlRequest, element) => {
    fetchData({
        type: 'GET',
        url: urlRequest,
        dataType: 'html'
    }).then(response => {
        $(element).html(response)
        flatpickrTimeInit()
        sortableBlockData('#block-data')
        sortableBlockData('.block-data-item')
    })
}

resetHiddenInput = form => {
    if (form) {
        $(form).find('input[type="hidden"][name!="__RequestVerificationToken"][name!="Command.Id"]').val('')
    }
}

resetTableActions = () => {
    if (tableActions) {
        tableActions.addClass('d-none')
    }

    if (tableReplaceElement) {
        tableReplaceElement.removeClass('d-none')
    }
}

function debounce(func, delay = 500) {
    let timeout

    return function executedFunc(...args) {
        if (timeout) {
            clearTimeout(timeout)
        }

        timeout = setTimeout(() => {
            func(...args)
            timeout = null
        }, delay)
    }
}

GetCategoryTreeview = (siteId, articleId) => {
    if (siteId > 0) {
        fetchData({
            type: 'GET',
            url: `/BongDa24hCMS/Categories?handler=CategoryTreeView`,
            data: { siteId, articleId },
            dataType: 'html'
        }).then(response => {
            $('#categoryTreeView').html(response)
        })
    }
}

normalizeText = text => {
    return text.normalize('NFD').replace(/[\u0300-\u036f]/g, '')
}

flatpickrInit = (element) => {
    element = element || '.datepicker'
    const format = $(element).data('format') || 'd-m-Y';
    if (typeof $.fn.flatpickr !== 'undefined') {
        $(element).flatpickr({
            dateFormat: format,
            'locale': 'vn',
            allowInput: true,
            disableMobile: true,
            showYearDropdown: true,
            changeYear: true,
        });
    }
}

flatpickrTimeInit = (element) => {
    element = element || '.datepicker-time',

        $(element).each((index, item) => {
            const format = $(item).data('format') || 'd-m-Y H:i',
                enableSeconds = $(item).data('enable-seconds') || false

            if (typeof $.fn.flatpickr !== 'undefined') {
                $(item).flatpickr({
                    dateFormat: format,
                    enableTime: true,
                    enableSeconds: enableSeconds,
                    allowInput: true,
                    disableMobile: true,
                    'locale': 'vn'
                });
            }
        })
}

sortableBlock = element => {
    var arraySortable = new Array(), pos = $(element).data('pos') || 'y'
    $(element).sortable({
        axis: pos,
        opacity: 0.6,
        cursor: 'move',
        scrollSensitivity: 40,
        update: function (event, ui) {
            if (!confirm('Xác nhận thay đổi vị trí Block ?')) {
                event.preventDefault()
                return
            }

            let _this = $(this), urlRequest = _this.data('url')

            arraySortable = []

            $.map(_this.find('.sortable-item'), function (el) {
                arraySortable.push({
                    id: parseInt(el.id), displayOrder: parseInt($(el).index() + 1)
                })
            })

            fetchData({
                type: 'POST',
                url: urlRequest,
                dataType: 'json',
                data: { commands: arraySortable, pageId: $('#Query_UiPageId option:selected').val() },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                }
            }).then(response => {
                if (response.Messages) {
                    toastMessage(response.Succeeded, response.Messages.join('<br />'))
                }
            })
        }
    })
}

sortableBlockTab = element => {
    var arraySortable = new Array()
    $(element).sortable({
        axis: 'x',
        opacity: 0.6,
        cursor: 'move',
        scrollSensitivity: 40,
        update: function (event, ui) {
            if (!confirm('Xác nhận thay đổi vị trí Tab ?')) {
                event.preventDefault()
                return
            }

            let _this = $(this), urlRequest = _this.data('url')

            arraySortable = []

            $.map(_this.find('.sortable-item'), function (el) {
                arraySortable.push({
                    id: parseInt(el.id), displayOrder: parseInt($(el).index() + 1)
                })
            })

            fetchData({
                type: 'POST',
                url: urlRequest,
                dataType: 'json',
                data: { commands: arraySortable },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                }
            }).then(response => {
                if (response.Messages) {
                    toastMessage(response.Succeeded, response.Messages.join('<br />'))
                }
            })
        }
    })
}

sortableBlockTabDownload = () => {
    var arraySortable = new Array()
    $('.block-tabs-download').sortable({
        axis: 'y',
        opacity: 0.6,
        cursor: 'move',
        scrollSensitivity: 40,
        update: function (event, ui) {
            if (!confirm('Xác nhận thay đổi vị trí Section tải danh sách VB ?')) {
                event.preventDefault()
                return
            }

            let _this = $(this), urlRequest = _this.data('url')

            arraySortable = []

            $.map(_this.find('.sortable-item'), function (el) {
                arraySortable.push({
                    id: parseInt(el.id), displayOrder: parseInt($(el).index() + 1)
                })
            })

            fetchData({
                type: 'POST',
                url: urlRequest,
                dataType: 'json',
                data: { commands: arraySortable },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                }
            }).then(response => {
                if (response.Messages) {
                    toastMessage(response.Succeeded, response.Messages.join('<br />'))
                }
            })
        }
    })
}

sortableBlockData = element => {
    $(element).sortable({
        axis: 'y',
        opacity: 0.6,
        cursor: 'move',
        scrollSensitivity: 40,
        update: function (event, ui) {
            if (!confirm('Xác nhận thay đổi vị trí ?')) {
                event.preventDefault();
            }
            else {
                $.map($(this).find('.sortable-bock-data'), function (el) {
                    $(el).find('input[name="ItemPosition"]').val(parseInt($(el).index() + 1))
                });
            }
        },
        stop: function (event, ui) {
            const self = $(ui.item),
                form = self.find('form').first(),
                btnDelete = form.find('a.btn-link')

            if (btnDelete.length > 0)
                form.submit()
        }
    });
}

sortableBlockChildrent = element => {
    var arraySortable = new Array()
    $(element).sortable({
        opacity: 0.6,
        cursor: 'move',
        scrollSensitivity: 40,
        update: function (event, ui) {
            if (!confirm('Xác nhận thay đổi vị trí ?')) {
                event.preventDefault();
            } else {
                arraySortable = []

                $.map($(this).find('.block-data-children-item'), function (el) {
                    arraySortable.push({
                        id: el.id, displayOrder: parseInt($(el).index() + 1)
                    })
                });

                fetchData({
                    url: $(this).data('url'),
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                    },
                    type: 'post',
                    dataType: 'json',
                    data: { commands: arraySortable, pageId: $('#Query_UiPageId option:selected').val() },
                }).then(response => {
                    if (response.Messages) {
                        toastMessage(response.Succeeded, response.Messages.join('<br />'))
                    }
                })
            }
        }
    });
}

sortableBlockGrandChildren = element => {
    var arraySortable = new Array()
    $(element).sortable({
        opacity: 0.6,
        cursor: 'move',
        scrollSensitivity: 40,
        update: function (event, ui) {
            if (!confirm('Xác nhận thay đổi vị trí ?')) {
                event.preventDefault();
            }

            arraySortable = []

            $.map($(this).find('.block-data-grandchildren-item'), function (el) {
                arraySortable.push({
                    id: el.id, displayOrder: parseInt($(el).index() + 1)
                })
            });

            fetchData({
                url: $(this).data('url'),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                },
                type: 'post',
                dataType: 'json',
                data: { commands: arraySortable, pageId: $('#Query_UiPageId option:selected').val() },
            }).then(response => {
                if (response.Messages) {
                    toastMessage(response.Succeeded, response.Messages.join('<br />'))
                }
            })
        }
    });
}

modalSetSize = (element, modal) => {
    var modalSize = $(element).data('modal') || '',
        modal = modal || $('#modalPopup'),
        dialog = modal.find('.modal-dialog').first()

    if (modalSize.length > 0) {
        dialog
            .removeClass('modal-sm')
            .removeClass('modal-lg')
            .removeClass('modal-xl')
            .removeClass('modal-xlg')
            .removeClass('modal-xxlg')
            .removeClass('modal-fullscreen')
            .addClass(modalSize)
    }
}

modalResetSize = (modal) => {
    modal = modal || $('#modalPopup'),
        dialog = modal.find('.modal-dialog').first()

    dialog
        .removeClass('modal-sm')
        .removeClass('modal-lg')
        .removeClass('modal-xl')
        .removeClass('modal-xlg')
        .removeClass('modal-fullscreen')
}

modalReset = (modal) => {
    modal = modal || $('#modalPopup')
    var popupContent = modal.find('.modal-content').first()
    modalResetSize(modal);
    popupContent.html(
        `
            <div class="modal-header">
                <h5 class="modal-title"></h5>
                <button class="btn-close" type="button" data-bs-dismiss="modal" aria-label="Đóng"></button>
            </div>
            <div class="modal-body modal-dialog-scrollable mt-0">
                <div class="card-body">
                    <h5 class="card-title placeholder-glow"><span class="placeholder col-6"></span></h5>
                    <p class="card-text placeholder-glow"><span class="placeholder col-7"></span><span class="placeholder col-4"></span><span class="placeholder col-4"></span><span class="placeholder col-6"></span><span class="placeholder col-8"></span></p>
                </div>
            </div>
            `
    )
}

hightLightRow = (id, element) => {
    element = element || $('.table-responsive')

    const table = element.find('table'),
        rowById = table.find(`tr[id='${id}']`).first()

    if (rowById) {
        rowById.addClass('hight-light')

        const timeOutHightLight = setTimeout(() => {
            rowById.removeClass('hight-light')
            clearTimeout(timeOutHightLight)
        }, 3000)
    }
}

updateTotalRows = response => {
    let totalRows
    const elementTotalRows = $('.total-rows'),
        table = $(response).filter('table')

    if (table) {
        totalRows = table.data('rows')
    }

    elementTotalRows.html(totalRows ? `<h6 class="mb-0 text-nowrap total-rows fw-semi-bold">Tìm thấy  <span class="text-danger">${totalRows}</span> bản ghi</h6>` : '')
}

historyPushState = path => {
    if ('undefined' !== typeof history.pushState) {
        history.pushState(null, document.title, path);
    } else {
        window.location.assign(path);
    }
}

bindData = (id, element) => {
    id = id || ''
    element = element || $('.table-responsive')

    const url = new URL(window.location.href),
        path = url.pathname,
        bindDataParamsElement = $('.bulk-select-replace-element'),
        { callbacks } = $(element).get(0).dataset

    if (bindDataParamsElement) {

        const bindDataParams = bindDataParamsElement.data('params')

        if (bindDataParams) {
            $.each(bindDataParams, function (name, value) {
                url.searchParams.set(name, value)
            });
        }
    }

    var query = url.search

    //if ($('.not-found').length > 0) {
    //    resetForm('.search-form')
    //    historyPushState(path)
    //}

    if (query.length > 0) {
        query = `&${query.substring(1)}`
    }

    fetchData({
        type: 'GET',
        url: `${path}?handler=BindData${query}`,
        dataType: 'html'
    }).then(response => {

        $(element).html(response)

        const isEmpty = $(response).filter('table').
            find('.not-found').first().length > 0

        if (!isEmpty) {


            if (id.length > 0) {
                hightLightRow(id, element)
            }

            initTooltip()

            if (callbacks) {
                const callbackObjects = JSON.parse(callbacks)
                callbackObjects.forEach(callbackObj => {
                    const callback = window[callbackObj.name]
                    if (typeof callback === 'function') {
                        callback(...callbackObj.params)
                    }
                })
            }
            stickyHeaderTableInit()
        }
    })
}

advertBindData = (id, element, url, query) => {
    element = element || $('.update-model .update-data')
    var keyworks = $('#select-keyworks').val();
    fetchData({
        type: 'GET',
        url: `${url}?${query}&Keywords=` + keyworks,
        dataType: 'html'
    }).then(response => {
        updateTotalRows(response)
        var elements = $(response);
        var tbody = $('.update-data', elements);
        $(element).html(tbody.html())

        if (id.length > 0) {
            hightLightRow(id)
        }
    })
}

getadvertBindData = (element, url) => {
    element = element || $('.update-model .update-data')

    fetchData({
        type: 'GET',
        url: `${url}`,
        dataType: 'html'

    }).then(response => {
        var elements = $(response);
        var tbody = $('.update-data', elements);
        $(element).html(tbody.html())

    })
}

modalGet = (element, path, title) => {
    modalSetSize(element)
    const { modalSize, callbacks, refresh } = element.dataset;
    let timeout
    elementAction = element
    const elementHtml = $(element).html()
    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {
            modalPopupContent.html(response)

            modalPopupTitle.text(title)

            if (!modalPopupContent.hasClass('fs-0')) {
                modalPopupContent.addClass('fs-0')
            }
            if (callbacks) {
                const callbackObjects = JSON.parse(callbacks)
                callbackObjects.forEach(callbackObj => {
                    const callback = window[callbackObj.name]
                    if (typeof callback === 'function') {
                        callback(...callbackObj.params)
                    }
                })
            }
            $.validator.unobtrusive.parse(modalPopup.find('form').first())

            if (modalPopupContent.find('input.datepicker').length > 0) {
                flatpickrInit()
            }

            if (modalPopupContent.find('input.datepicker-time').length > 0) {
                flatpickrTimeInit()
            }

            if (modalPopupContent.find('.select-picker-modal').length > 0) {
                docReady(select2Init)
            }

            if (modalPopupContent.find('.scrollbar-overlay').length > 0) {
                docReady(scrollbarInit)
            }

            const jsonViewElement = modalPopupContent.find('.json-view').first()

            if (jsonViewElement.length > 0) {
                modalPopup.find('.modal-body').first().jsonView(jsonViewElement.text())
            }

            docReady(tooltipInit);

            initViewMaxLength()

            initCkEditor()

            initSelect2AutoComplete($('.select2-auto-complete-modal'))

            initAutocomplete()

            initTooltip()
            removeLoading(elementAction)
            modalPopup.modal('show')
        }), 1000
    )
}

modalChildGet = (element, path, title) => {
    modalSetSize(element, $('#modalDataPopup'))
    let timeout
    elementAction = element
    const elementHtml = $(element).html();
    console.log(1, elementAction)
    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {
            modalDataPopupContent.html(response)

            const isEmpty = $(response).filter('table').
                find('.not-found').first().length > 0

            if (!isEmpty) {
                if ($('#sticky-table-modal').length) {
                    $('#sticky-table-modal').bootstrapTable({
                        stickyHeader: true,
                        stickyHeaderOffsetY: stickyHeaderOffsetY
                    })
                }
            }

            modalDataPopup.find('.modal-title').first().text(title)

            $.validator.unobtrusive.parse(modalDataPopup.find('form').first())


            if (!modalDataPopupContent.hasClass('fs-0')) {
                modalDataPopupContent.addClass('fs-0')
            }
            if (modalDataPopupContent.find('input.datepicker').length > 0) {
                flatpickrInit()
            }

            if (modalDataPopupContent.find('input.datepicker-time').length > 0) {
                flatpickrTimeInit()
            }
            if (modalDataPopupContent.find('input.datepicker-time2').length > 0) {
                flatpickrTime2Init()
            }
            if (modalDataPopupContent.find('.select-picker-modal2').length > 0) {
                docReady(select3Init($('#modalDataPopup')))
            }
            if (modalDataPopupContent.find('.scrollbar-overlay').length > 0) {
                docReady(scrollbarInit)
            }

            const jsonViewElement = modalDataPopupContent.find('.json-view').first()

            if (jsonViewElement.length > 0) {
                modalDataPopup.find('.modal-body').first().jsonView(jsonViewElement.text())
            }

            docReady(tooltipInit);

            initViewMaxLength()

            initCkEditor()

            initSelect2AutoComplete($('.select3-auto-complete-modal'))

            initAutocomplete()

            initTooltip()
            removeLoading(elementAction)
            modalDataPopup.modal('show')
        })
    )
}
modalGetOnText = (element, path, title) => {
    modalSetSize(element)
    let timeout
    elementAction = element
    const elementHtml = $(element).html()
    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html'
            /*beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }*/
        }).then(response => {
            modalPopupContent.html(response)

            modalDataPopup.find('.modal-title').first().text(title)

            if (!modalPopupContent.hasClass('fs-0')) {
                modalPopupContent.addClass('fs-0')
            }

            $.validator.unobtrusive.parse(modalPopup.find('form').first())

            if (modalPopupContent.find('input.datepicker').length > 0) {
                flatpickrInit()
            }

            if (modalPopupContent.find('input.datepicker-time').length > 0) {
                flatpickrTimeInit()
            }

            if (modalPopupContent.find('.select-picker-modal').length > 0) {
                docReady(select2Init)
            }

            if (modalPopupContent.find('.scrollbar-overlay').length > 0) {
                docReady(scrollbarInit)
            }

            const jsonViewElement = modalPopupContent.find('.json-view').first()

            if (jsonViewElement.length > 0) {
                modalPopup.find('.modal-body').first().jsonView(jsonViewElement.text())
            }

            docReady(tooltipInit);

            initViewMaxLength()

            initCkEditor()

            initSelect2AutoComplete($('.select2-auto-complete-modal'))

            initAutocomplete()

            initTooltip()

            modalPopup.modal('show')
        }), 1000
    )
}
modalLoad = (element) => {
    const self = $(element), { title, className } = element,
        { modalSize, path, callbacks, refresh } = element.dataset,
        modalId = (Math.random() + 1).toString(36).substring(2)

    let timeout
    elementAction = element
    const elementHtml = $(element).html(), parent = self.closest('.modal'), parentId = parent.attr('id')
    $("body").append(`<div id="modalDataPopup_${modalId}" class="modal fade" data-bs-focus="false" data-bs-backdrop="static" data-keyboard="false" tabindex="-3" aria-hidden="true" style="z-index:10011" ${(!refresh ? `data-path="${path}"` : '')} ${(refresh && parentId ? `data-pid="${parentId}"` : '')} ${(callbacks ? `data-callbacks='${callbacks}'` : '')}>
        <div class="modal-dialog ${modalSize}">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Chọn dữ liệu</h5>
                    <button class="btn-close" type="button" data-bs-dismiss="modal" aria-label="Đóng"></button>
                </div>
                <div class="modal-body modal-dialog-scrollable mt-0">
                    <div class="card-body">
                        <h5 class="card-title placeholder-glow"><span class="placeholder col-6"></span></h5>
                        <p class="card-text placeholder-glow"><span class="placeholder col-7"></span><span class="placeholder col-4"></span><span class="placeholder col-4"></span><span class="placeholder col-6"></span><span class="placeholder col-8"></span></p>
                    </div>
                </div>
            </div>
        </div>
    </div>`)

    let modalDataPopup = $(`#modalDataPopup_${modalId}`)

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {
            let modalDataPopupContent = modalDataPopup.find('.modal-content').first(),
                modalDataPopupTitle = modalDataPopup.find('.modal-title').first()

            modalDataPopupContent.html(response)

            modalDataPopupTitle.text(title)

            if (!modalDataPopupContent.hasClass('fs-0')) {
                modalDataPopupContent.addClass('fs-0')
            }

            $.validator.unobtrusive.parse(modalDataPopup.find('form').first())

            if (modalDataPopupContent.find('.scrollbar-overlay').length > 0) {
                docReady(scrollbarInit)
            }

            if (callbacks) {
                const callbackObjects = JSON.parse(callbacks)
                callbackObjects.forEach(callbackObj => {
                    const callback = window[callbackObj.name]
                    if (typeof callback === 'function') {
                        callback(...callbackObj.params)
                    }
                })
            }
            docReady(tooltipInit);
            initCkEditor()

            initViewMaxLength()

            if (modalDataPopupContent.find('.select-picker-modal').length > 0) {
                docReady(select2Init(modalDataPopup))
            }

            modalDataPopup.modal('show')
        }), 1000
    )

    modalDataPopup.on('hidden.bs.modal', function () {
        $(this).remove()
    })
}
genModalPopup = ({ modalId, modalSize, title, parentId, path, callbacks, refresh }) => {
    $("body").append(`<div id="modalDataPopup_${modalId}" class="modal fade" data-bs-focus="false" data-bs-backdrop="static" data-keyboard="false" tabindex="-3" aria-hidden="true" style="z-index:10011" ${(!refresh ? `data-path="${path}"` : '')} ${(refresh && parentId ? `data-pid="${parentId}"` : '')} ${(callbacks ? `data-callbacks='${callbacks}'` : '')}>
        <div class="modal-dialog ${modalSize}">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">${title}</h5>
                    <button class="btn-close" type="button" data-bs-dismiss="modal" aria-label="Đóng"></button>
                </div>
                <div class="modal-body modal-dialog-scrollable mt-0">
                    <div class="card-body">
                        <h5 class="card-title placeholder-glow"><span class="placeholder col-6"></span></h5>
                        <p class="card-text placeholder-glow"><span class="placeholder col-7"></span><span class="placeholder col-4"></span><span class="placeholder col-4"></span><span class="placeholder col-6"></span><span class="placeholder col-8"></span></p>
                    </div>
                </div>
            </div>
        </div>
    </div>`)
}
modalMediaLoad = (element) => {
    const self = $(element), { title, className } = element,
        { modalSize, callbacks, refresh, siteid, mediatypeid, key } = element.dataset,
        modalId = (Math.random() + 1).toString(36).substring(2),
        mediaTypeParam = `?Query.SiteId=${siteid}&Query.MediaTypeId=${mediatypeid}&modalId=${modalId}`,
        path = `/BongDa24hCMS/Medias/Select${mediaTypeParam}`

    let timeout
    elementAction = element
    const elementHtml = $(element).html(), parent = self.closest('.modal'), parentId = parent.attr('id')

    genModalPopup({
        modalId, modalSize, title: 'Chọn Media', path
    })

    let modalDataPopup = $(`#modalDataPopup_${modalId}`)

    modalDataPopup.attr('data-key', key)

    modalDataPopup.attr('data-type', mediatypeid)

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {
            let modalDataPopupContent = modalDataPopup.find('.modal-content').first(),
                modalDataPopupTitle = modalDataPopup.find('.modal-title').first()

            modalDataPopupContent.html(response)

            modalDataPopupTitle.text(title)

            if (!modalDataPopupContent.hasClass('fs-0')) {
                modalDataPopupContent.addClass('fs-0')
            }

            if (callbacks) {
                const callbackObjects = JSON.parse(callbacks)
                callbackObjects.forEach(callbackObj => {
                    const callback = window[callbackObj.name]
                    if (typeof callback === 'function') {
                        callback(...callbackObj.params)
                    }
                })
            }

            modalDataPopup.modal('show')
        }), 1000
    )

    modalDataPopup.on('hidden.bs.modal', function () {
        $(this).remove()
    })
}

modalMediaGet = (element, key, mediaTypeId = 1, title = 'Chọn ảnh', siteid) => {
    modalSetSize(element)
    let timeout
    elementAction = element, { siteid } = element.dataset
    const elementHtml = $(element).html(),
        mediaTypeParam = `?Query.SiteId=${siteid}&Query.MediaTypeId=${mediaTypeId}`,
        path = `/BongDa24hCMS/Medias/Select${mediaTypeParam}`

    let modalDataPopup = $(`#modalDataPopup_${key}`)

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {
            let modalDataPopupContent = modalDataPopup.find('.modal-content').first(),
                modalDataPopupTitle = modalDataPopup.find('.modal-title').first()

            modalDataPopupContent.html(response)

            modalDataPopupTitle.text(title)

            if (!modalDataPopupContent.hasClass('fs-0')) {
                modalDataPopupContent.addClass('fs-0')
            }

            $.validator.unobtrusive.parse(modalDataPopup.find('form').first())

            if (modalDataPopupContent.find('.scrollbar-overlay').length > 0) {
                docReady(scrollbarInit)
            }

            if (callbacks) {
                const callbackObjects = JSON.parse(callbacks)
                callbackObjects.forEach(callbackObj => {
                    const callback = window[callbackObj.name]
                    if (typeof callback === 'function') {
                        callback(...callbackObj.params)
                    }
                })
            }

            modalDataPopup.modal('show')
        }), 1000
    )

    modalDataPopup.on('hidden.bs.modal', function () {
        $(this).remove()
    })
}
refreshModal = (modalId) => {
    const element = document.getElementById(modalId), modalDataPopup = $(element),
        { title, path, callbacks, refresh } = element.dataset

    if (path) {
        debounce(
            fetchData({
                url: path,
                type: 'GET',
                dataType: 'html',
                //beforeSend: () => {
                //    onBegin(element)
                //},
                //callback: () => {
                //    if (timeout) {
                //        clearTimeout(timeout)
                //    }

                //    timeout = setTimeout(() => {
                //        $(element).removeAttr('disabled')
                //        $(element).html(elementHtml)
                //        timeout = null
                //    }, 300)
                //}
            }).then(response => {
                let modalDataPopupContent = modalDataPopup.find('.modal-content').first(),
                    modalDataPopupTitle = modalDataPopup.find('.modal-title').first()

                modalDataPopupContent.html(response)

                modalDataPopupTitle.text(title)

                if (!modalDataPopupContent.hasClass('fs-0')) {
                    modalDataPopupContent.addClass('fs-0')
                }

                $.validator.unobtrusive.parse($(modalDataPopup).find('form').first())

                if (modalDataPopupContent.find('.scrollbar-overlay').length > 0) {
                    docReady(scrollbarInit)
                }

                if (callbacks) {
                    const callbackObjects = JSON.parse(callbacks)
                    callbackObjects.forEach(callbackObj => {
                        const callback = window[callbackObj.name]
                        if (typeof callback === 'function') {
                            callback(...callbackObj.params)
                        }
                    })
                }

                //modalDataPopup.modal('show')
            }), 1000
        )

        modalDataPopup.on('hidden.bs.modal', function () {
            $(this).remove()
        })
    }
}
showMessage = (object) => {

    let { title, message, icon, type, buttons, autoClose, escapeKey, columnClass } = object

    if (!buttons) {

        buttons = {
            close: {
                text: 'Đóng',
                btnClass: 'btn-secondary'
            }
        }
    }

    let hasCloseButton = false
    $.each(buttons, function (name, value) {
        if (name == 'close') {
            hasCloseButton = true
            'return false'
        }
    })

    if (!hasCloseButton) {
        buttons['close'] = {
            text: 'Đóng',
            btnClass: 'btn-secondary'
        }
    }

    let defaultOption = {
        theme: 'bootstrap',
        closeIcon: true,
        animation: 'scale',
        draggable: true,
    }

    let customOption = {
        title: title || 'Thông báo',
        icon: icon || 'far fa-bell',
        autoClose: autoClose || 'close|10000',
        escapeKey: escapeKey || 'close',
        type: type || 'orange',
        content: message,
        buttons: buttons
    }

    if (columnClass) {
        customOption.columnClass = columnClass
    }

    var setting = $.extend({}, defaultOption, customOption);

    $.confirm(setting)
}


selectRow = table => {
    let anyChecked = false, checkedIds = [], hiddenInput = table.find('input[name="ChkActionIds"]').first()
    table.find('.row-checkbox').each(function () {
        if ($(this).prop('checked')) {
            anyChecked = true
            checkedIds.push($(this).val())
        }
    })

    hiddenInput.val(checkedIds.join(','))

    let bulkActions = table.find('.bulk-select-actions').first()
    if (anyChecked) {
        bulkActions.removeClass('d-none')
    } else {
        bulkActions.addClass('d-none')
    }

    updateSelectAllCheckbox(table)
}

updateSelectAllCheckbox = table => {
    let selectAllCheckbox = table.find('thead input[class*="form-check-input"]'), checkboxes = table.find('.row-checkbox'),
        allChecked = true, anyChecked = false

    checkboxes.each(function () {
        if (!$(this).prop('checked')) {
            allChecked = false
        } else {
            anyChecked = true
        }
    })
    if (checkboxes.length == 0) {
        allChecked = false
    }
    if (allChecked) {
        selectAllCheckbox.prop('checked', true)
        selectAllCheckbox.prop('indeterminate', false)
    } else if (anyChecked) {
        selectAllCheckbox.prop('checked', false)
        selectAllCheckbox.prop('indeterminate', true)
    } else {
        selectAllCheckbox.prop('checked', false)
        selectAllCheckbox.prop('indeterminate', false)
    }
}

modalMediaEditorGet = (element, key, mediaTypeId = 1, title = 'Chọn ảnh') => {
    modalSetSize(element)
    let timeout
    elementAction = element
    const elementHtml = $(element).html(),
        mediaTypeParam = `?Query.MediaTypeId=${mediaTypeId}`,
        path = `/BongDa24hCMS/Medias/SelectEditor${mediaTypeParam}`

    modalDataChildPopup.removeAttr('data-key')
    modalDataChildPopup.removeAttr('data-type')

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {

            modalDataChildPopup.attr('data-key', key)

            modalDataChildPopup.attr('data-type', mediaTypeId)

            modalDataChildPopupContent.html(response)

            const isEmpty = $(response).filter('table').
                find('.not-found').first().length > 0

            if (!isEmpty) {
                if ($('#sticky-table-modal').length) {
                    $('#sticky-table-modal').bootstrapTable({
                        stickyHeader: true,
                        stickyHeaderOffsetY: stickyHeaderOffsetY
                    })
                }
            }

            modalDataChildPopup.find('.modal-title').first().text(title)

            if (!modalDataChildPopupContent.hasClass('fs-0')) {
                modalDataChildPopupContent.addClass('fs-0')
            }

            modalDataChildPopup.modal('show')
        })
    )
}

modalMediaGet2 = (element, key, mediaTypeId = 1, title = 'Chọn ảnh', siteId) => {
    modalSetSize(element)
    let timeout
    elementAction = element
    const elementHtml = $(element).html(),
        mediaTypeParam = `?Query.MediaTypeId=${mediaTypeId}&Query.SiteId=${siteId}`,
        path = `/BongDa24hCMS/Medias/Select${mediaTypeParam}`

    modalDataPopup.removeAttr('data-key')
    modalDataPopup.removeAttr('data-type')

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {

            modalDataPopup.attr('data-key', key)

            modalDataPopup.attr('data-type', mediaTypeId)

            modalDataPopupContent.html(response)

            const isEmpty = $(response).filter('table').
                find('.not-found').first().length > 0

            if (!isEmpty) {
                if ($('#sticky-table-modal').length) {
                    $('#sticky-table-modal').bootstrapTable({
                        stickyHeader: true,
                        stickyHeaderOffsetY: stickyHeaderOffsetY
                    })
                }
            }

            modalDataPopup.find('.modal-title').first().text(title)

            if (!modalDataPopupContent.hasClass('fs-0')) {
                modalDataPopupContent.addClass('fs-0')
            }

            modalDataPopup.modal('show')
        })
    )
}

uncheckRadio = (element, mediaKey) => {
    modalDataPopup.attr('data-key', mediaKey)

    const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
        mediaControl = $(`.SelectMediaInput${mediaKey}`).first()

    if (confirm('Xác nhận bỏ chọn radio ?') && selectMediaInput.length > 0 && mediaControl.length > 0) {

        selectMediaInput.val('')

        mediaControl.attr('src', '')

        $('input.duration').val('')

        $('#Command_Duration').val('')
        $('#Command_FileDuration').val('')

        spotlightArray = []

        bindDataSpotlight()

        $('.uncheck-radio').addClass('d-none')

        $('.spotlight').addClass('d-none')
    }
}

timeFormat = (currentTime) => {
    const duration = Math.floor(currentTime)
    const h = Math.floor(duration / 3600)
    const m = Math.floor((duration - h * 3600) / 60)
    const s = duration % 60
    const H = h < 10 ? `0${h}:` : `${h}:`
    const M = m < 10 ? `0${m}:` : `${m}:`
    const S = s < 10 ? `0${s}` : `${s}`

    return H + M + S
}

bindDataSpotlight = () => {

    const spotlightElement = $('#spotlight')

    spotlightElement.empty()

    spotlightArray.sort((a, b) => a.Time.localeCompare(b.Time));

    $.each(spotlightArray, (index, item) => {
        spotlightElement.append(`
            <div data-id="${item.Time}" class="timeline-item position-relative">
			    <div class="row g-0 align-items-center">
				    <div class="col-auto d-flex align-items-center">
					    <h6 class="timeline-item-date fs--2 text-900 text-truncate mb-0 me-1"> ${item.Time}</h6>
					    <div class="position-relative">
						    <div class="icon-item icon-item-md shadow-none bg-200"><span class="text-primary fas fa-hashtag"></span></div>
					    </div>
				    </div>
				    <div class="col ps-3 fs--1 text-900">
					    <div class="py-x1">
						    <div class="fs--1">${item.Content.replace(/\n/g, '<br />')}</div>
					    </div>
				    </div>
				    <div class="col-auto">
						<a href="javascript:void(0)" onclick="return modalSpotlightGet(this,'Cập nhật Spotlight', '${item.Time}')" class="btn btn-sm btn-link" title="Cập nhật"><span class="fs--1 fas fa-edit"></span></a>
						<a href="javascript:void(0)" class="btn btn-sm btn-link delete-spotlight text-danger" title="Xóa"><span class="fs--1 fas fa-trash-alt"></span></a>
					</div>
				    <hr class="text-200 my-0" />
			    </div>
		    </div>
        `)
    })

    if (spotlightArray.length > 0) {
        $('#Command_Summary').val(JSON.stringify(spotlightArray))
    } else {
        $('#Command_Summary').val('')
    }
}

validationOnError = (element, message) => {
    const parent = $(element).parent()

    if (parent.length > 0) {

        validationReset()

        parent.append(`<span class="text-danger field-validation-error voh">${message}</span>`)

    }
}

validationReset = () => {
    $('.text-danger.field-validation-error.voh').remove()
}

getDuration = (src) => {
    return new Promise((resolve, reject) => {

        try {

            var audio = new Audio()

            $(audio).on('loadedmetadata', function () {
                return resolve(audio.duration)
            })

            audio.src = src

        } catch (e) {
            return reject(null)
        }
    })
}

formatMediaDuration = (durationInput) => {
    let [hours, minutes, seconds] = durationInput.split(':')
    hours = parseInt(hours, 10)
    minutes = parseInt(minutes, 10)
    seconds = parseInt(seconds, 10)

    return [hours < 10 ? 0 : '', hours, ':', minutes < 10 ? 0 : '', minutes, ':', seconds < 10 ? 0 : '', seconds].join('')
}

validateMediaDuration = (durationInput) => {
    const mediaKey = modalDataPopup.attr('data-key') || '',
        media = document.getElementsByClassName(`SelectMediaInput${mediaKey}`)

    if (media.length > 0) {

        const duration = media[0].duration,
            [hours, minutes, seconds] = durationInput.split(':'),
            totalSeconds = (+parseInt(hours, 10)) * 60 * 60 + (+parseInt(minutes, 10)) * 60 + (+parseInt(seconds, 10))

        return duration >= totalSeconds
    }

    return false
}

modalSpotlightGet = (element, title, spotlightTime = '', mediaKey = '') => {

    const timeElement = modalSpotlightPopup.find('input#Time').first(),
        contentElement = modalSpotlightPopup.find('textarea#Content').first()

    if (mediaKey.trim().length == 0) {
        mediaKey = modalDataPopup.attr('data-key') || ''
    } else {
        modalDataPopup.attr('data-key', mediaKey)
    }

    timeElement.val('')

    contentElement.val('')

    modalSpotlightPopup.removeAttr('data-id')

    validationReset()

    if (spotlightTime.trim().length > 0) {

        var result = spotlightArray.find(item => item.Time === spotlightTime)

        if (result) {

            timeElement.val(result.Time)

            contentElement.val(result.Content)

        }

    } else {

        const mediaControl = document.getElementsByClassName(`SelectMediaInput${mediaKey}`)

        if (mediaControl.length > 0) {

            const currentTime = mediaControl[0].currentTime

            timeElement.val(timeFormat(currentTime))
        }

    }

    modalSetSize(element)

    modalSpotlightPopup.find('.modal-title').first().text(title)

    if (!modalSpotlightPopupContent.hasClass('fs-0')) {

        modalSpotlightPopupContent.addClass('fs-0')

    }

    modalSpotlightPopup.attr('data-id', spotlightTime)

    modalSpotlightPopup.modal('show')
}

getMediaType = (mediaTypeId = 1, getProp = 'name') => {
    const mediaType = mediaTypeArray.filter(x => x.id == mediaTypeId)

    if (mediaType) {
        return mediaType[0][getProp]
    }

    return ''
}

selectMedia2 = async (element, mediaType = 1, mediaPath, fileSize = 0, mediaName = '') => {
    if (typeof mediaPath == 'undefined' || mediaPath.trim().length <= 0) {
        return;
    }

    modalDataPopup.modal('hide')

    let mediaKey = modalDataPopup.attr('data-key') || ''
    const mediaTypeName = getMediaType(mediaType)

    if (mediaKey.trim().length > 0) {

        if (mediaKey.includes('key_cke_')) {
            $(`.${mediaKey}`).find('input').val(mediaPath)
            $($(`.${mediaKey}`).find('input')[0]).focus().blur()
            $(`.${mediaKey}.alt-input`).find('input').val(mediaName)
        }
        else if (mediaTypeName == 'audio') {

            const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
                mediaControl = $(`.SelectMediaInput${mediaKey}`).first()

            if (selectMediaInput.length > 0 && mediaControl.length > 0) {

                selectMediaInput.val(mediaPath)

                mediaControl.attr('src', mediaPath)

                if (duration <= 0) {

                    duration = await getDuration(mediaPath)

                    if (duration) {

                        $('input.duration').val(timeFormat(Math.floor(duration)))

                        $('#Command_Duration').val(Math.floor(duration))

                        $('#Command_FileDuration').val(Math.floor(duration))
                    }

                } else {

                    $('input.duration').val(timeFormat(duration))

                    $('#Command_Duration').val(duration)

                    $('#Command_FileDuration').val(duration)
                }

                spotlightArray = []

                bindDataSpotlight()

                $('.uncheck-radio').removeClass('d-none')

                $('.spotlight').removeClass('d-none')
            }
        } else if (mediaTypeName == 'video') {

            const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
                mediaControl = $(`.SelectMediaInput${mediaKey}`).first()

            if (selectMediaInput.length > 0 && mediaControl.length > 0) {

                selectMediaInput.val(mediaPath)

                mediaControl.attr('src', mediaPath)

                if (duration <= 0) {

                    duration = await getDuration(mediaPath)

                    if (duration) {

                        let durationTime = timeFormat(Math.floor(duration))

                        $('input.duration').val(durationTime)

                        $('#Command_NewDetailInput_AudioSize').val(Math.floor(fileSize))

                        $('#Command_NewDetailInput_AudioDuration').val(durationTime)
                    }

                } else {

                    let durationTime = timeFormat(duration)

                    $('input.duration').val(durationTime)

                    $('#Command_NewDetailInput_AudioSize').val(fileSize)

                    $('#Command_NewDetailInput_AudioDuration').val(durationTime)
                }

                $('.uncheck-video').removeClass('d-none')

                $('.video-preview').removeClass('d-none')
            }
        }
        else {
            const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
                mediaControl = $(`.SelectMediaInput${mediaKey}`).first()

            if (selectMediaInput.length > 0) {
                if (mediaKey == '_Thumnail_News_Create_') {
                    getImageSizeAsync(mediaPath)
                        .then(function (size) {
                            if (size.width < 800 || ((size.width / size.height) != (5 / 3))) {
                                toastMessage(false, "Ảnh thumb yêu cầu đúng tỷ lệ 5:3 và tối thiểu width là 800px");
                            } else {
                                selectMediaInput.val(mediaPath)
                                mediaControl.val(mediaPath)
                                selectMediaInput.trigger('change')
                                selectMediaInput.trigger('blur')
                                $('.remove-button').removeClass('d-none')
                            }
                        })
                        .catch(function (error) {
                            console.error(error);
                        });
                } else {
                    selectMediaInput.val(mediaPath)
                    mediaControl.val(mediaPath)
                    selectMediaInput.trigger('change')
                    selectMediaInput.trigger('blur')
                    $('.remove-button').removeClass('d-none')
                }
            }
        }
    }
    else if (mediaTypeName == 'audio') {

        const profileSelected = $('#Command_HostVoices option:selected'),
            profileId = profileSelected.val() || 0,
            profileName = profileSelected.text() || ''
        //voicTypeSelected = $('#VOHVoiceType option:selected'),
        //voiceTypeId = voicTypeSelected.val() || 0,
        //voicTypeName = voicTypeSelected.text() || ''

        if (profileId <= 0) {

            showMessage({ message: 'Vui lòng chọn biên tập viên.' })

            return
        }

        //var voiceTypeExists = newsVoicesVOH.filter(function (item) {
        //    return item.VoiceTypeId == voiceTypeId;
        //})[0];

        var voiceExists = newsVoicesVOH.filter(function (item) {
            return item.ProfileId == profileId;
        })[0];

        //if (voiceTypeExists) {

        //    showMessage({ message: `${voicTypeName} đã được thêm trước đó.\nVui lòng chọn giọng đọc khác.` })

        //    return
        //}

        if (voiceExists) {

            showMessage({ message: `Giọng đọc của biên tập viên: ${profileName} đã được thêm trước đó.\nVui lòng chọn giọng đọc khác.`, columnClass: 'col-md-6 col-md-offset-3', })

            return
        }

        if (duration <= 0) {
            duration = await getDuration(mediaPath)
        }

        newsVoicesVOH.push({
            'VoiceUrl': mediaPath,
            //'VoiceTypeId': voiceTypeId,
            'ProfileId': profileId,
            'ProfileName': profileName,
            'FileSize': fileSize,
            'DurationInSec': duration
        })

        bindNewsVoiceVOH()

        $('#Command_HostVoices').empty().trigger('change')
    }
    else if (mediaTypeName == 'video') {
        //Chọn video cho Video Form Create Update
        $('#Command_NewDetailInput_Content').val(mediaPath);

        $('#Command_NewDetailInput_AudioSize').val(fileSize);

        if (duration <= 0) {

            duration = await getDuration(mediaPath)

            if (duration) {

                let durationTime = timeFormat(Math.floor(duration));

                $('#Command_NewDetailInput_AudioDuration').val(durationTime);
            }

        } else {

            let durationTime = timeFormat(duration)

            $('#Command_NewDetailInput_AudioDuration').val(durationTime);
        }
    }
}

selectMedia = async (element) => {

    const { modalid: modalId, mediatypeid: mediaTypeId, mediapath: mediaPath, medianame: mediaName } = element.dataset
    if (typeof mediaPath == 'undefined' || mediaPath.trim().length <= 0) {
        return
    }
    if (modalId) {
        modalMedia = $(`#modalDataPopup_${modalId}`)
    }
    else {
        modalMedia = $(`#modalDataPopup`)
    }
    console.log(modalId)
    if (modalMedia.length > 0) {

        modalMedia.modal('hide')

        let mediaKey = modalMedia.attr('data-key') || ''

        if (mediaKey.trim().length > 0) {

            const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
                mediaControl = $(`#SelectMediaView${mediaKey}`)

            if (selectMediaInput.length > 0) {

                selectMediaInput.val(mediaPath)

                mediaControl.attr('src', mediaPath)
            }
        }
    }
}
modalDocsGet = (element, key, keywords = '', newsTypeId = 1, title = "Chọn văn bản", searchByDate = 1) => {
    element.attr('data-modal', 'modal-xl')
    modalSetSize(element, $('#modalDataChildPopup'))
    let timeout
    elementAction = element
    const elementHtml = $(element).html(),
        path = `/BongDa24hDocs/Docs/SelectDocs`

    modalDataChildPopup.removeAttr('data-key')
    modalDataChildPopup.removeAttr('data-type')

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {

            modalDataChildPopup.attr('data-key', key)

            modalDataChildPopup.attr('data-type', newsTypeId)

            modalDataChildPopupContent.html(response)

            const isEmpty = $(response).filter('table').
                find('.not-found').first().length > 0

            if (!isEmpty) {
                if ($('#sticky-table-modal').length) {
                    $('#sticky-table-modal').bootstrapTable({
                        stickyHeader: true,
                        stickyHeaderOffsetY: stickyHeaderOffsetY
                    })
                }

                initTooltip(modalDataChildPopupContent)
            }

            var selectModal = modalDataChildPopupContent.find('.select-editor-modal')

            selectModal.length && selectModal.each(function (index, value) {
                var $this = $(value);
                var options = $.extend({
                    dropdownParent: $('#modalDataChildPopup'),
                }, $this.data('options'))
                $this.select2(options)
            })

            if (modalDataChildPopupContent.find('input.datepicker').length > 0) {
                flatpickrInit()
            }

            modalDataChildPopup.find('.modal-title').first().text(title)

            if (!modalDataChildPopupContent.hasClass('fs-0')) {
                modalDataChildPopupContent.addClass('fs-0')
            }


            modalDataChildPopup.modal('show')
        })
    )
}
modalNewsGet = (element, key, keywords = '', newsTypeId = 1, title = "Chọn văn bản", searchByDate = 1) => {
    element.attr('data-modal', 'modal-xl')
    modalSetSize(element, $('#modalDataChildPopup'))
    let timeout
    elementAction = element
    const elementHtml = $(element).html(),
        path = `/BongDa24hCMS/Articles/SelectArticles`

    modalDataChildPopup.removeAttr('data-key')
    modalDataChildPopup.removeAttr('data-type')

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(element)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(element).removeAttr('disabled')
                    $(element).html(elementHtml)
                    timeout = null
                }, 300)
            }
        }).then(response => {

            modalDataChildPopup.attr('data-key', key)

            modalDataChildPopup.attr('data-type', newsTypeId)

            modalDataChildPopupContent.html(response)

            const isEmpty = $(response).filter('table').
                find('.not-found').first().length > 0

            if (!isEmpty) {
                if ($('#sticky-table-modal').length) {
                    $('#sticky-table-modal').bootstrapTable({
                        stickyHeader: true,
                        stickyHeaderOffsetY: stickyHeaderOffsetY
                    })
                }

                initTooltip(modalDataChildPopupContent)
            }

            var selectModal = modalDataChildPopupContent.find('.select-editor-modal')

            selectModal.length && selectModal.each(function (index, value) {
                var $this = $(value);
                var options = $.extend({
                    dropdownParent: $('#modalDataChildPopup'),
                }, $this.data('options'))
                $this.select2(options)
            })

            if (modalDataChildPopupContent.find('input.datepicker').length > 0) {
                flatpickrInit()
            }

            modalDataChildPopup.find('.modal-title').first().text(title)

            if (!modalDataChildPopupContent.hasClass('fs-0')) {
                modalDataChildPopupContent.addClass('fs-0')
            }


            modalDataChildPopup.modal('show')
        })
    )
}
selectData = (element, path, title = '') => {
    if (typeof path == 'undefined' || path.trim().length <= 0) {
        return;
    }

    modalDataChildPopup.modal('hide')

    let dataKey = modalDataChildPopup.attr('data-key') || ''

    if (dataKey.trim().length > 0) {

        if (dataKey.includes('key_cke_')) {
            $(`.${dataKey}`).find('input').val(path)
            $($(`.${dataKey}`).find('input')[0]).focus()
            $(`.${dataKey}.alternative-information`).find('input').val(title)
        }
    }
}
selectMedia3 = async (element, mediaType = 1, mediaPath, duration = null, fileSize = 0, mediaName = '') => {
    if (typeof mediaPath == 'undefined' || mediaPath.trim().length <= 0) {
        return;
    }

    modalDataChildPopup.modal('hide')

    let mediaKey = modalDataChildPopup.attr('data-key') || ''
    const mediaTypeName = getMediaType(mediaType)

    if (mediaKey.trim().length > 0) {

        if (mediaKey.includes('key_cke_')) {
            $(`.${mediaKey}`).find('input').val(mediaPath)
            $($(`.${mediaKey}`).find('input')[0]).focus().blur()
            $(`.${mediaKey}.alt-input`).find('input').val(mediaName)
        }
        else if (mediaTypeName == 'audio') {

            const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
                mediaControl = $(`.SelectMediaInput${mediaKey}`).first()

            if (selectMediaInput.length > 0 && mediaControl.length > 0) {

                selectMediaInput.val(mediaPath)

                mediaControl.attr('src', mediaPath)

                if (duration <= 0) {

                    duration = await getDuration(mediaPath)

                    if (duration) {

                        $('input.duration').val(timeFormat(Math.floor(duration)))

                        $('#Command_Duration').val(Math.floor(duration))

                        $('#Command_FileDuration').val(Math.floor(duration))
                    }

                } else {

                    $('input.duration').val(timeFormat(duration))

                    $('#Command_Duration').val(duration)

                    $('#Command_FileDuration').val(duration)
                }

                spotlightArray = []

                bindDataSpotlight()

                $('.uncheck-radio').removeClass('d-none')

                $('.spotlight').removeClass('d-none')
            }
        } else if (mediaTypeName == 'video') {

            const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
                mediaControl = $(`.SelectMediaInput${mediaKey}`).first()

            if (selectMediaInput.length > 0 && mediaControl.length > 0) {

                selectMediaInput.val(mediaPath)

                mediaControl.attr('src', mediaPath)

                if (duration <= 0) {

                    duration = await getDuration(mediaPath)

                    if (duration) {

                        let durationTime = timeFormat(Math.floor(duration))

                        $('input.duration').val(durationTime)

                        $('#Command_NewDetailInput_AudioSize').val(Math.floor(fileSize))

                        $('#Command_NewDetailInput_AudioDuration').val(durationTime)
                    }

                } else {

                    let durationTime = timeFormat(duration)

                    $('input.duration').val(durationTime)

                    $('#Command_NewDetailInput_AudioSize').val(fileSize)

                    $('#Command_NewDetailInput_AudioDuration').val(durationTime)
                }

                $('.uncheck-video').removeClass('d-none')

                $('.video-preview').removeClass('d-none')
            }
        }
        else {
            const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
                mediaControl = $(`.SelectMediaInput${mediaKey}`).first()

            if (selectMediaInput.length > 0) {
                selectMediaInput.val(mediaPath)
                mediaControl.val(mediaPath)
                selectMediaInput.trigger('change')
                selectMediaInput.trigger('blur')
                $('.remove-button').removeClass('d-none')
            }
        }
    }
}
editorSelectMedia = (element, mediaType = 1) => {
    try {

        const mediaTypeName = getMediaType(mediaType, 'desc')

        if (!window.CKEDITOR) {
            showMessage({ message: 'Không tìm thấy trình soạn thảo phù hợp.' })

            return
        }

        if (mediaSelected.length == 0) {

            showMessage({ message: `Chưa chọn file ${mediaTypeName}` })

            return
        }

        let htmlInsert = ''
        //mediaInsertPosition = $('input[type="radio"][name="EditorMediaPosition"]:checked').val()

        $.each(mediaSelected, function (index, media) {
            htmlInsert += `<figure class="image">
                            <img alt="${media.name}" src="${media.path}"  />
                            <figcaption> </figcaption>
                       </figure>`

            //if (mediaInsertPosition.trim().length > 0) {
            //    htmlInsert = `<div style="${mediaInsertPosition}">${htmlInsert}</div>`
            //}
        })

        for (var key in CKEDITOR.instances) {
            CKEDITOR.instances[key].insertHtml(htmlInsert);
        }

        modalDataPopup.modal('hide')

    } catch (e) {
        console.log(e)
    }
}

editorSelectNews = (element) => {
    try {

        if (!window.CKEDITOR) {
            showMessage({ message: 'Không tìm thấy trình soạn thảo phù hợp.' })

            return
        }

        if (newsSelected.length == 0) {

            showMessage({ message: 'Chưa chọn bài viết.' })

            return
        }

        let counter = $('.cke_wysiwyg_frame').eq(0).contents().find('a[id^="Article_Noi-dung-goi-y-intext"]').length,
            htmlInsert = `<div class="box-blockquote">
                                <blockquote>
                                    <p style="font-size: 18px;">Xem thêm:<br>`

        $.each(newsSelected, function (index, item) {
            htmlInsert += `<a href="${item.slug}" target="_blank" class="init-a" id="Article_Noi-dung-goi-y-intext_Item${counter + 1}">${item.title}</a><br>`
            counter++
        })

        htmlInsert += `</blockquote>
                            </div>`

        for (var key in CKEDITOR.instances) {
            CKEDITOR.instances[key].insertHtml(htmlInsert);
        }

        modalDataPopup.modal('hide')

    } catch (e) {
        console.log(e)
    }
}

bindNewsVoiceVOH = () => {
    const newsVoicesVOHElement = $('#news-voices-voh')
    let newsVoices = []
    newsVoicesVOHElement.empty()

    if (newsVoicesVOH && newsVoicesVOH.length > 0) {
        ad

        //if (voiceTypes && voiceTypes.length > 0) {
        //    newsVoicesVOH.forEach(function (newsVoice, i) {
        //        let voiceType = voiceTypes.filter(x => x.Id == newsVoice.VoiceTypeId)[0]
        //        if (voiceType) {
        //            newsVoice.VoiceTypeName = voiceType.TypeName
        //            newsVoice.DisplayOrder = voiceType.DisplayOrder
        //            newsVoices.push(newsVoice)
        //        }
        //    })
        //}

        //newsVoices.sort(function (a, b) { return a.DisplayOrder - b.DisplayOrder })

        $.each(newsVoicesVOH, (index, item) => {
            newsVoicesVOHElement.append(`
                    <div class="d-flex hover-actions-trigger btn-reveal-trigger gap-3 border-200 border-bottom mb-3">
				        <div class="mb-3">
					        <label class="form-check-label fs--1 w-100 pe-3"><span class="mb-1 text-700">
							                    ${item.ProfileName}
						        </span>
						        <span class="fs--2 text-600 lh-base font-base fw-normal d-block mb-2">
							        <audio controls>
								        <source src="${item.VoiceUrl}" type="audio/mpeg" controls>
								        Your browser does not support the audio element.
							        </audio>
                                    <input type="hidden" name="Command.NewsVoicesVOHInput[${index}].VoiceUrl" value="${item.VoiceUrl}" />
                                    <input type="hidden" name="Command.NewsVoicesVOHInput[${index}].ProfileId" value="${item.ProfileId}" />
                                    <input type="hidden" name="Command.NewsVoicesVOHInput[${index}].FileSize" value="${item.FileSize}" />
                                    <input type="hidden" name="Command.NewsVoicesVOHInput[${index}].DurationInSec" value="${item.DurationInSec}" />
						        </span>
					        </label>
				        </div>
				        <div class="voice-status end-0">
					        <button class="btn btn-link btn-sm pe-0" onclick="removeNewsVoicesVOH(${index})" type="button">Bỏ chọn</button>
				        </div>
			        </div>
                    `)
        })

        newsVoicesVOHElement.removeClass('d-none')
    } else {

        newsVoicesVOHElement.addClass('d-none')
    }
}

removeNewsVoicesVOH = (index) => {
    if (typeof index != 'undefined') {

        let buttons = {
            'confirm': {
                text: "Đồng ý",
                btnClass: 'btn btn-danger',
                keys: ['enter'],
                action: function () {
                    if (newsVoicesVOH.length > 0) {
                        newsVoicesVOH.splice(index, 1)
                        bindNewsVoiceVOH()
                    } else {
                        $('#news-voices-voh').addClass('d-none')
                    }
                }
            }
        }

        showMessage({
            title: 'Xác nhận xóa giọng đọc đã chọn ?',
            icon: 'fa fa-question-circle',
            columnClass: 'col-md-6 col-md-offset-3',
            message: 'Dữ liệu đã xóa sẽ không thể phục hồi !',
            buttons
        })
    }

    return false
}

quickSearch = (inputSearch = '#QuickSearch', filterItems = '[data-filter-item]', callBack) => {
    const keywords = toSlug($(inputSearch).val())
    var value = ''

    $(filterItems).each((index, item) => {
        value = toSlug($(item).text())

        if (value.includes(keywords)) {
            if (callBack) {
                callBack(item)
            } else {
                $(item).removeClass('d-none')
            }
        } else {
            $(item).addClass('d-none')
        }
    })
}

radioBlockCateNotDisplay = (item) => {
    const topic = $('#TopicId')
    if (topic.length > 0) {

        const topicId = topic.find('option:selected').val()

        if (topicId == 0) {
            $(item).removeClass('d-none')
        }
        else if ($(item).data('topic') && $(item).data('topic') == topicId) {
            $(item).removeClass('d-none')
        } else {
            $(item).addClass('d-none')
        }
    }
}

initSelect2AutoComplete = (elements) => {
    elements = elements || $('.select2-auto-complete');
    if (elements.length > 0) {
        elements.select2().on('select2:open', (elm) => {
            const targetLabel = $(elm.target).prev('label')
            targetLabel.addClass('selected')
        }).on('select2:select', (e) => {
            if (!e.params.data.tag) {
                return
            }

        }).on('select2:close', (elm) => {
            const target = $(elm.target)
            const targetLabel = target.prev('label')
            const targetOptions = $(elm.target.selectedOptions)
            if (targetOptions.length === 0) {
                targetLabel.removeClass('selected')
            }
        })
        //    .on('select2:unselect', (elm) => {
        //    const target = $(elm.target);
        //    target.remove(elm.CU);
        //})

        elements.each(function (index) {
            const self = $(this), urlRequest = self.data('url') || '',
                values = self.data('value') || [], isTag = self.data('tag') || false,
                displayName = self.data('display-name') || 'Name',
                displayValue = self.data('display-value') || 'Id',
                placeholder = self.attr('placeholder') || '',
                formatDataFunc = self.data('format') || 'formatRepo',
                formatDataSelectionFunc = self.data('format-selection') || 'formatRepoSelection',
                dropdownParent = self.data('parent') || '',
                minLength = self.data('min') | 0,
                isSearch = self.data('search') || false,
                allowClear = self.data('allowclear') || false,
                siteId = self.data('siteid') || 0
            maximumSelect = self.data('max-selected') || 0;

            var selected = []
            var initials = []
            for (var s in values) {
                if (typeof values[s].Type != 'undefined') {
                    initials.push({ 'id': values[s][displayValue], text: values[s][displayName], 'type': values[s]["Type"] })
                    selected.push(values[s][displayValue])
                } else if (typeof values[s].Id != 'undefined') {
                    initials.push({ id: values[s][displayValue], text: values[s][displayName] })
                    selected.push(values[s][displayValue])
                }
                else {
                    initials.push({ text: values[s] })
                    selected.push(values[s])
                }
            }

            if (initials.length > 0) {
                const targetLabel = self.prev('label');
                targetLabel.addClass('selected');
            }
            var addDataButton = '';
            if ($(this).attr('data-url-add')) {
                addDataButton = '<button class="btn btn-primary no-focus ms-2 pt-0 pb-0 ps-1 pe-2" type="button" data-modal="modal-lg" onclick="AddNewOnSelect(this)" title="Thêm mới">' +
                    '<i class="fas fa-plus"></i><span class="d-none d-sm-inline-block ms-1">Thêm</span></button>';
            }
            else { addDataButton = ''; }

            if (urlRequest.trim().length > 0) {

                let options = {
                    data: initials,
                    tags: isTag,
                    allowClear: allowClear,
                    placeholder: placeholder,
                    searching: function () {
                        return 'Đang tìm dữ liệu...'
                    },
                    language: {
                        noResults: function () {
                            return "Không tìm thấy kết quả! " + addDataButton;
                        },
                        inputTooLong: function (n) {
                            return "Vui lòng xóa bớt " + (n.input.length - n.maximum) + " ký tự"
                        },
                        inputTooShort: function (n) {
                            return "Vui lòng nhập thêm từ " + (n.minimum - n.input.length) + " ký tự trở lên"
                        },
                        loadingMore: function () {
                            return "Đang lấy thêm kết quả…"
                        },
                        maximumSelected: function (n) {
                            return "Chỉ có thể chọn được " + n.maximum + " lựa chọn"
                        },
                        removeAllItems: function () {
                            return "Xóa tất cả các mục"
                        }
                    },
                    formatNoMatches: function () {
                        return 'Không tìm thấy dữ liệu.'
                    },
                    ajax: {
                        url: urlRequest,
                        delay: 250,
                        async: true,
                        cache: true,
                        dataType: 'json',
                        params: {
                            contentType: 'application/json; charset=utf-8'
                        },
                        data: function (params) {
                            var query = {
                                keywords: params.term,
                                siteId: siteId
                            }
                            console.log(query)
                            return query;
                        },
                        processResults: function (response) {

                            if (isSearch) {
                                response.Data.unshift({
                                    id: '0',
                                    text: 'Tất cả'
                                })
                            }

                            return {
                                results: $.map(response.Data, item => item)
                            }
                        }
                    },
                    createTag: function (params) {

                        let term = $.trim(params.term)

                        if (term.length < 3) {
                            return null
                        }

                        return {
                            id: term,
                            text: term,
                            tag: true
                        }
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: minLength,
                    templateResult: window[formatDataFunc],
                    templateSelection: window[formatDataSelectionFunc],
                    maximumSelectionLength: maximumSelect,
                    sorter: function (data) {
                        return data
                    }
                }
                if (dropdownParent.length > 0) {
                    options.dropdownParent = $(dropdownParent)
                }

                self.select2(options).on("select2:select", function (e) {
                    var $container = $(this).next().find('.select2-selection__rendered');
                    $container.find('li.select2-selection__choice').sort(function (a, b) {
                        return -1;
                    }).prependTo($container);
                });
                self.select2(options).on('open', function () {
                    var search = $(this).attr('tabindex', 0);
                    //self.$search.focus(); remove this line
                    setTimeout(function () { $(this).search.focus(); }, 10);//add this line

                });
                self.select2(options).on("select2:clear", function (e) {
                    ////self.select2("val", "");
                    //self.select2("val", "");
                    //$(this).val(null).trigger('change');
                    //$(this).val([]).trigger('change');
                    //var select2 = $(this).data('select2');
                    //select2.$element.val(null).trigger('change');
                });

            } else {
                self.select2({
                    data: initials,
                    tags: true,
                    multiple: true,
                    tokenSeparators: [','],
                    placeholder: placeholder,
                    minimumResultsForSearch: -1,
                    dropdownCssClass: 'no-search',
                    dropdownParent: $('#modalPopup'),
                    "language": {
                        "noResults": function () { return ''; }
                    }
                })
            }

            if (urlRequest.trim().length == 0 || dropdownParent.length > 0) {

                self.val(selected).trigger('change')

            }
        })
    }
}

addItemAutocomplete = (element, ...data) => {

    const [Id, Title, Slug, inputName, itemsLimit] = data

    const self = $(element), parent = self.closest('.auto-complete'),
        inputAutoComplete = parent.find('.input-auto-complete').first(),
        resultElement = parent.find('.autocomplete-result').first(),
        selectedElement = parent.find('.autocomplete-selected').first(),
        autoCompleteElement = parent.find('.auto-complete-value'),
        autoCompleteValues = autoCompleteElement
            .map(function () { return $(this).val() }).get()

    if (selectedElement && selectedElement.children('.row').length >= itemsLimit) {

        alert(`Vui lòng chọn tối đa ${itemsLimit} dữ liệu.`);

        return false
    }

    if (autoCompleteValues.indexOf(Id + '') != -1) {

        alert(`Dữ liệu ${Title} đã được chọn.`);

        return false
    }

    inputAutoComplete.val('')

    resultElement.empty()

    resultElement.addClass('d-none')

    selectedElement.append(`
                <div class="row gx-2 flex-between-center">
                    <div class="col-sm-12">
                        <div class="d-flex flex-between-center">
                            <h6 class="mb-0 text-700"><a href="${Slug}" title="${Title}" target="_blank">${Title}</a></h6><a href="#!" class="btn btn-sm btn-link text-danger" onclick="removeItemAutocomplete(this, ${Id})" title="Xóa khỏi danh sách"><span class="fs--1 fas fa-trash-alt"></span></a>
                            <input type="hidden" class="auto-complete-value" name="${inputName}" value="${Id}" />
                        </div>
                    </div>
                </div>
            `)

    return false
}

removeItemAutocomplete = (element, id) => {
    const self = $(element),
        parent = self.closest('.row')

    if (parent) {
        let buttons = {
            'confirm': {
                text: "Đồng ý",
                btnClass: 'btn btn-danger',
                keys: ['enter'],
                action: function () {
                    fetchData({
                        url: '/NaviMS/FileUploads/Index?handler=Delete',
                        type: 'POST',
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                        },
                        dataType: 'json',
                        data: { 'id': id },
                    }).then(response => {
                        if (response && response.Succeeded) {
                            parent.remove()
                        }
                        toastMessage(response.Succeeded, response.Messages.join('<br />'));
                    })
                }
            }
        }

        showMessage({
            title: 'Xác nhận bỏ dữ liệu đã chọn ?',
            icon: 'fa fa-question-circle',
            columnClass: 'col-md-6 col-md-offset-3',
            message: 'Dữ liệu đã xóa sẽ không thể phục hồi !',
            buttons
        })
    }

    return false
}
initAutocomplete = () => {
    const element = $('.input-auto-complete')

    if (element.length > 0) {

        $(document).on('click', (event) => {
            if (!$(event.target).closest('.did-floating-label-content').length) {
                $('.autocomplete-result').addClass('d-none')
            }
        })

        element.on('focus', (e) => {
            let self = $(e.target), parent = self.closest('.auto-complete'),
                resultElement = parent.find('.autocomplete-result').first(),
                newsRelates = self.hasClass('news-relates'), tagIds = ''

            if (newsRelates) {
                tagIds = $('#Command_Tags option:selected').map(function () {
                    return $(this).val();
                }).get().join(',')
            }

            if (tagIds.trim().length == 0 && resultElement && resultElement.hasClass('d-none') && resultElement.children().length > 0) {
                resultElement.removeClass('d-none')
            }
        })

        element.on('search', function (e) {
            let self = $(e.target), parent = self.closest('.auto-complete'),
                resultElement = parent.find('.autocomplete-result').first(),
                inputValue = self.val().trim(),
                newsRelates = self.hasClass('news-relates'), tagIds = ''

            if (newsRelates) {
                tagIds = $('#Command_Tags option:selected').map(function () {
                    return $(this).val();
                }).get().join(',')
            }

            if (tagIds.trim().length == 0 && inputValue.length == 0 && resultElement) {
                resultElement.empty()
                resultElement.addClass('d-none')
            }
        });

        element.on('click keyup', debounce((e) => {
            let self = $(e.target), urlRequest = self.data('url'),
                itemsLimit = self.data('items') || 3, inputName = self.data('name') || '',
                inputValue = self.val().trim(), parent = self.closest('.auto-complete'),
                iconContainer = self.next('.icon-container'),
                resultElement = parent.find('.autocomplete-result').first(),
                newsRelates = self.hasClass('news-relates'), tagIds = ''

            // get keycode of current keypress event
            var code = (e.keyCode || e.which)

            if (newsRelates) {
                tagIds = $('#Command_Tags option:selected').map(function () {
                    return $(this).val();
                }).get().join(',')
            }

            if (tagIds.trim().length == 0 && inputValue.length == 0) {
                resultElement.empty()
                resultElement.addClass('d-none')
                return
            }

            // do nothing if it's an arrow key or enter
            if (code == 37 || code == 38 || code == 39 || code == 40 || code == 13) {
                return
            }

            fetchData({
                url: urlRequest,
                type: 'GET',
                dataType: 'json',
                data:
                {
                    keywords: inputValue,
                    tagIds: tagIds
                },
                beforeSend: () => {
                    iconContainer.removeClass('d-none')
                },
                callback: () => {
                    iconContainer.addClass('d-none')
                }
            }).then(response => {

                resultElement.empty()

                if (response.Data.length > 0) {
                    let htmlAppend = ''
                    for (var index = 0; index < response.Data.length; index++) {
                        htmlAppend = `<div data-id="${response.Data[index].id}" class="row g-3 align-items-center text-center text-md-start py-1 border-bottom border-200">
						    <div class="col-md-auto">
							    <div class="avatar">
									<img class="bg-attachment" src="${response.Data[index].thumbnail}?w=160&h=90" onerror="this.src='/img/no-image.png'" alt="${response.Data[index].title}" width="100%" />
							    </div>
						    </div>
						    <div class="col px-x1 py-1">
							    <div class="row">
								    <div class="col-12">
									    <h6 class="fs-0">${response.Data[index].title}</h6>
								    </div>
									    <div class="col-12">
										    <p class="fs--1 mb-1 text-800"><a href="${response.Data[index].slug}" target="_blank" title="${response.Data[index].title}">${response.Data[index].slug}</a></p>
									        <p class="fs--2 text-600">${response.Data[index].publishedat}</p>
                                            </div>
							    </div>
						    </div>
                            <div class="col-md-auto d-flex justify-content-center">`
                        if (response.Data[index].isrelated) {
                            htmlAppend += `<a href="javascript:void(0)" class="btn btn-falcon-default icon-item" title="Bài viết đã được chọn làm tin liên quan"><i class="fas fa-check-circle text-success"></i> </a>`
                        } else {
                            htmlAppend += `<a href="javascript:void(0)" class="btn btn-falcon-default icon-item focus-bg-primary" onclick="return addItemAutocomplete(this, ${response.Data[index].id}, '${response.Data[index].title}', '${response.Data[index].slug}', '${inputName}', ${itemsLimit})" title="Thêm vào danh sách"><i class="fas fa-plus"></i> </a>`
                        }
                        `</div>
					    </div>`

                        resultElement.append(htmlAppend)
                    }

                    resultElement.removeClass('d-none')
                }
                else {
                    resultElement.addClass('d-none')
                }

            }).catch((error) => {

                resultElement.addClass('d-none')

            })

        }, 500))
    }
}

onExportWithQuery = (element, url) => {
    var form = $(element).closest('form')
    if (form) {

        var formDataArray = $(form).serializeArray();
        var formData = new FormData();
        $.each(formDataArray, function (i, field) {
            formData.append(field.name, field.value);
        });

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                onBegin(element);
            },
            processData: false,
            contentType: false,
            success: function (response) {
                removeLoading(form);
                if (response && response.Succeeded) {
                    var a = document.createElement('a');
                    a.href = response.Data;
                    var arr = response.Data.split("/");

                    a.download = arr.length ? arr[arr.length - 1] : "export_file_" + getCurrentDateTime() + ".xlsx";
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                }
                toastMessage(response.Succeeded, response.Messages.join('<br />'))
            },
            error: function (xhr, status, error) {
                removeLoading(form);

                toastMessage(false, "Vui lòng thử lại");
            }
        });
        return true
    }
    return false
}
function profileFormatData(data) {
    if (!data.id || data.id <= 0) { return data.text; }

    let itemHtml = `<div class="row profile align-items-center text-center text-md-start p-1 border-bottom border-200">
								<div class="col-md-auto">
									<div class="avatar avatar-3xl">`
    if (data.Avatar.length > 0) {
        itemHtml += `<img class="rounded-circle" src="${data.Avatar}?w=160&h=90" onerror="this.src='/img/avatar.png'" alt="${data.text}" />`
    } else {
        itemHtml += `<img class="rounded-circle" src="/img/avatar.png" alt="${data.text}" />`
    }
    itemHtml += `</div>
								</div>
								<div class="col px-x1- py-2-">
									<div class="row">
										<div class="col-12">
											<h6 class="fs-0">${data.text}</h6>
										</div>
										<div class="col-md-12">
											<p class="fs--1 text-800">${data.FullName}</p>
										</div>
										<div class="col-12">
											<div class="fs--2 text-600 d-flex flex-column flex-md-row align-items-center gap-2">
												${(data.DepartmentName.length > 0 ? `<p title="Phòng / ban" class="mb-0 ms-1"><i class="fas fa-building text-500 fa-sm"></i> ${data.DepartmentName}</p>` : '')}
												${(data.Job.length > 0 ? `<p title="Nghề nghiệp" class="mb-0 ms-1"><i class="fas fa-building text-500 fa-sm"></i> ${data.Job}</p>` : '')}
											</div>
										</div>
									</div>
								</div>
							</div>`

    return $(
        itemHtml
    )
}

function profileFormatDataSelection(data) {
    if (data.FullName && data.text.trim() != data.FullName.trim()) {
        return `${data.text} ( ${data.FullName.trim()})`
    }
    return data.text;
}

function formatRepo(data, container) {

    if (data.type) {
        $(container).addClass(`profile-selected-${data.type}`)
    }

    return data.text;
}
function formatRepoSelection(data, container) {

    if (data.type) {
        $(container).addClass(`profile-selected-${data.type}`)
    }

    return data.text;
}
function resultFormatData(data) {
    if (!data.id || data.id <= 0) { return data.text; }

    let itemHtml = `<div class="row profile align-items-center text-center text-md-start p-1 border-bottom border-200">
                            <div class="col px-x1- py-2-">
                                <div class="row">
                                    <div class="col-12">
                                        <h6 class="fs-0">${data.text}</h6>
                                    </div>
                                </div>
                            </div>
                        </div>`

    return $(
        itemHtml
    )
}
function resultFormatDataSelection(data) {
    if (data.text == null || data.text == '') {
        data.text = data.title;
    }
    if (data.text && data.text.trim()) {
        return `${data.text}`
    }
    return data.text
}
function docFormatData(data) {
    if (!data.id || data.id <= 0) { return data.text; }

    let itemHtml = `<div class="row profile align-items-center text-center text-md-start p-1 border-bottom border-200">
                            <div class="col px-x1- py-2-">
                                <div class="row">
                                    <div class="col-12 mb-0 mt-0">
                                        <div class="fs--1">${data.text} (<i>${data.docIdentity}</i> - ${data.issuedate})</h6>
                                    </div>
                                </div>
                            </div>
                        </div>`
    //dành cho doclink 
    return $(
        itemHtml
    )
}
function docFormatDataSelection(data) {
    var id = data.id.split('site')[0]
    $("#modalPopup").find('input[name*="Command.DocName"]').val(data.text);
    $("#modalPopup").find('input[name*="Command.TextLink"]').val(data.docIdentity);

    $("#modalPopup").find('input[name*="Command.DocReferenceId"]').val(parseInt(id));
    $("#modalPopup").find('input[name*="Command.UrlLink"]').val(data.docurl);
    $("#modalPopup").find('input[name*="Command.SiteReferenceId"]').val(data.siteid);
    $("#modalPopup").find('input[name*="Command.SiteIdPatten"]').val(data.siteid);
    $("#modalPopup").find('input[name*="Command.DocIdPatten"]').val(data.docid);
    $("#modalPopup").find('input[name*="Command.DocUrl"]').val(data.docurl);
    if (data.siteid) {
        $('#Command_DocReferenceIds').prop('data-siteref', data.siteid);
        $('#Command_DocReferenceIds').data('siteref', data.siteid);
    }
    else {
        $('#Command_DocReferenceIds').prop('data-siteref', 0);
        $('#Command_DocReferenceIds').data('siteref', 0);
    }
    if (data.text == null) {
        data.text = data.title;
    }
    if (data.docIdentity && data.text.trim() != data.docIdentity.trim()) {
        return `${data.docIdentity.trim()}`
    }
    return data.text
}
function formatOption(option) {
    if (!option.id) {
        return option.text;  // Nếu không có giá trị, trả về tên tùy chọn
    }
    // Tạo phần tử checkbox với tùy chọn
    var $option = $(
        '<span><input type="checkbox" style="margin-right: 8px;" /> ' + option.text + '</span>'
    );
    return $option;
}

function formatSelected(option) {
    var count = option.length;
    if (count > 1) {
        option.text = "Đã chọn " + count + " mục "
    }
    return option.text;  // Trả về tên tùy chọn đã chọn
}
toSlug = (str, characterReplace = ' ') => {

    str = str.toLowerCase();

    str = str
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')

    str = str.replace(/[đĐ]/g, 'd')

    str = str.replace(/([^0-9a-z-\s])/g, '')

    str = str.replace(/(\s+)/g, `${characterReplace}`)

    str = str.replace(/-+/g, `${characterReplace}`)

    str = str.replace(/^-+|-+$/g, '')

    return str
}

resetForm = form => {

    $(form).find(':input')
        .not(':button, :submit, :reset, :hidden, :input[name="Command.Id"],:input[name="Command.DocId"], :input[name*="AddMoreData"],:input[name*="Command.IsDisplay"], select, textarea,iframe')
        .val('')
        .prop('checked', false)

    $(form).find('img')
        .attr('src', '')
        .css('border', 'none')

    $(form).find('.profile-pic')
        .attr('src', '/img/avatar.png')
    $(form).find('select[class*=select-picker-index]').not('[class*= "form-select-submit"]').val(0).trigger("change");
    $('textarea').val('')

    if (typeof CKEDITOR != 'undefined') {

        for (instance in CKEDITOR.instances) {

            CKEDITOR.instances[instance].setData(" ")

        }

    }
    $(form).find('.form-select').each(function () {
        var element = $(this);
        var value = element.val();
        var resetUrl = element.data('reset-url');
        if (resetUrl) {
            $.ajax({
                url: resetUrl,
                type: "GET",
                contentType: 'application/json',
                dataType: "json",
                success: function (response) {
                    if (response) {
                        var html = '<option value="0">...</option>';
                        $.each(response.Data, function (e, value) {
                            html += '<option value="' + value.id + '">' + value.text + '</option>'
                        })
                        element.html(html)

                    }
                },
                error: function (xhr, status, error) {
                    toastMessage(false, "Vui lòng thử lại");
                }
            })
        }

        element.val(value).trigger("change")
    })
}

fetchData = options => {
    return new Promise(function (resolve, reject) {
        $.ajax(options).done(resolve).fail(reject).always(options.callback || {})
    })
}
cleanHtml = (content) => {
    try {
        if (content) {
            let element = $(content), root = $('<div></div>'),
                whitelist = ['p,b,i,u,h2,h3,h4,h5,h6,br,hr,ol,ul,li,table,tbody,th,tr,td'],
                result = ''

            root.append(element)

            //let googleDoc = root.find('b[id^="docs-internal-guid-"]').first()

            root.find('a').unwrap()

            root.find('*').each(function () {

                if (this.nodeType === Node.COMMENT_NODE || this.nodeName.toLowerCase() == 'v:shape' || this.nodeName.toLowerCase() == 'v:imagedata' || this.nodeName.toLowerCase() == 'img') {
                    $(this).remove()
                }

                if (this.attributes.id && this.attributes.id.nodeValue.match(/^docs\-internal\-guid\-/)) {
                    $(this).replaceWith(function () { return $(this).contents().wrapInner('') })
                }

                if (whitelist.includes(this.nodeName.toLowerCase()) || this.nodeName.toLowerCase() == 'span') {
                    let isBold = false
                    if (this.nodeName.toLowerCase() == 'span') {
                        let styleAttribute = this.attributes['style']
                        if (typeof styleAttribute != 'undefined') {

                            if (styleAttribute.nodeValue.indexOf('font-weight:700') != -1) {
                                isBold = true
                            }
                        }
                    }

                    $(this).replaceWith(function () { return isBold ? $(this).wrapInner('<b></b>').contents() : $(this).contents() })
                }
            })
            // processLink
            root.find("a").each(function () {
                let $link = $(this);
                let href = $link.attr("href");

                if (href && href.includes("#") && !$link.html().includes("<sup") && $link.text().includes("[") && $link.text().includes("]")) {
                    $link.html($link.html().replace($link.text(), `<sup>${$link.text()}</sup>`));
                }
                ////loại bỏ các điểm neo
                //if ($link.attr("name")) {
                //    $link.remove()
                //}
            });
            //process table
            root.find("table").each(function () {
                let $table = $(this);
                let tableStyle = $table.attr("style") || "";

                if (tableStyle) {
                    let newStyle = "";
                    tableStyle.split(";").forEach(function (style) {
                        if (style.includes("border")) {
                            newStyle = newStyle ? newStyle + ";" + style : style;
                        }
                    });
                    $table.attr("style", newStyle);
                }

                $table.attr("width", "100%");
            });
            // process img 
            root.find("img").each(function () {
                let $img = $(this);
                let imgUrl = $img.attr("src") || "";

                if (!imgUrl || imgUrl.includes("file:///")) {
                    console.error(`Removing image with URL: ${imgUrl}`);
                    $img.remove();
                }
            });

            result = root.html()
            result = result.replace(/<\!--.*?-->/g, '')
            return result
        }

    } catch (e) {

    }
}
$(document).ready(function () {
    $('.max-length').each(function () {
        var maxLength = $(this).attr('maxlength');
        var currentLength = $(this).val().length;
        var counterEle = $(this).siblings('.counter');
        counterEle.text(currentLength + '/' + maxLength);
    });
    $('.max-length').on('input', function () {
        var maxLength = $(this).attr('maxlength');
        var currentLength = $(this).val().length;
        var counterEle = $(this).siblings('.counter');
        counterEle.text(currentLength + '/' + maxLength);
    });
});

function selectAllRow() {
    var chkActionIds = '';
    $('.main-table>tbody>tr').each(function (index) {

        var chk = $(this).find('.form-check-input:first')

        if (chk.prop('disabled') == false) {

            $(chk).prop('checked', $(event.target).is(":checked"))

            if ($(event.target).is(":checked")) {

                if (chkActionIds.length > 0) chkActionIds += ','

                chkActionIds += $(chk).attr('value')

            }
        }
    });

    if (chkActionIds.trim().length > 0) {

        $('.ChkActionIds').attr('value', chkActionIds)

        $('.bulk-select-actions').removeClass('d-none')

    } else {
        $('.bulk-select-actions').addClass('d-none')
    }
}

function selectRow() {
    var allChecked = true;
    var hasCheck = false;
    var chkActionIds = '';
    $('.main-table>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check-input:first');
        if (!chk.is(":checked")) {
            allChecked = false;
        }
        else {
            hasCheck = true;
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(chk).attr('value');
        }
    });
    $('.ChkActionIds').attr('value', chkActionIds);
    if (hasCheck) {
        $('.bulk-select-actions').removeClass('d-none');
    }
    else {
        $('.bulk-select-actions').addClass('d-none');
    }
    if (allChecked) {
        $('.main-table>thead>tr').each(function (index) {
            var chk = $(this).find('.form-check-input:first');
            if (chk) {
                chk.prop('checked', true);
            }
        });
    }
    else {
        $('.main-table>thead>tr').each(function (index) {
            var chk = $(this).find('.form-check-input:first');
            if (chk) {
                chk.prop('checked', false);
            }
        });
    }
}
function selectAllRowModal() {
    var chkActionIds = '';
    $('.table-modal>tbody>tr').each(function (index) {

        var chk = $(this).find('.form-check-input:first')

        if (chk.prop('disabled') == false) {

            $(chk).prop('checked', $(event.target).is(":checked"))

            if ($(event.target).is(":checked")) {

                if (chkActionIds.length > 0) chkActionIds += ','

                chkActionIds += $(chk).attr('value')

            }
        }
    });
    if (chkActionIds.trim().length > 0) {

        $('.modalChkActionIds').attr('value', chkActionIds)

        $('.bulk-select-actions-modal').removeClass('d-none')

    } else {
        $('.bulk-select-actions-modal').addClass('d-none')
    }
}
$(document).on('change', 'input[name*="hideElement"]', function () {
    var elementShow = $(this).data('show');
    var elementHide = $(this).data('hide');
    var selectboxHide = $(elementHide).find('select')
    var selectboxShow = $(elementShow).find('select')
    if ($(this).is(":checked")) {
        $(elementShow).removeClass('d-none');
        $(elementHide).addClass('d-none');
    } else {
        $(elementShow).addClass('d-none');
        $(elementHide).removeClass('d-none');
    }
    if ($(elementShow).hasClass('d-none')) {
        console.log(1, selectboxShow)
        if (selectboxShow) {
            selectboxShow.attr('disabled', 'disabled')
        }
    }
    else {
        selectboxShow.removeAttr('disabled')
    }
    if ($(elementHide).hasClass('d-none')) {
        console.log(2, selectboxHide)
        if (selectboxHide) {
            selectboxHide.attr('disabled', 'disabled')
        }
    }
    else {
        selectboxHide.removeAttr('disabled')
    }
})
$(document).on('mouseover', '.show-prop', function (e) {
    var id = $(this).attr('data-id');

    if ($('#doc_' + id).html() == null || $('#doc_' + id).html().length <= 0) {
        var isapi = $(this).attr('data-isapi') || false;
        var siteidapi = $(this).attr('data-siteidapi') || 0;
        var query = { id: parseInt(id.split('_')[0].replace(/\D/g, "")), isapi: isapi, siteidapi: siteidapi };
        $.ajax({
            url: "/BongDa24hDocs/Docs/Index?handler=DocProperties",
            type: "GET",
            contentType: 'application/json',
            dataType: "html",
            data: query,
            success: function (response) {
                if (response) {
                    var html = `<div class="div1-custom-hover"></div><div class="d-flex" onclick="event.stopPropagation()">`;
                    html += response;
                    html += '</div>'
                    $('#doc_' + id).addClass('card card-bg');
                    $('#doc_' + id).html(html)
                }
            },
            error: function (xhr, status, error) {
                toastMessage(false, "Vui lòng thử lại");
            }
        })
    }
    $(e.target).css('text-decoration', 'none');
    if (e?.fromElement?.classList.contains('custom-hover')) return
    let eleTarget = e.currentTarget
    let bound = eleTarget.getBoundingClientRect()
    let eleHover = e.currentTarget.nextElementSibling
    var elementItem = document.getElementById('div_' + id)
    var boundEleItem = elementItem.getBoundingClientRect()
    if (!eleHover) return
    eleHover.style.bottom = 'auto'
    eleHover.style.right = 'auto'
    eleHover.style.left = boundEleItem.right + 'px';
    eleHover.style.top = bound.y + 'px';

    let div1 = eleHover.getElementsByClassName('div1-custom-hover')[0]
    if (div1) {
        var heightdiv1 = $(this).height() + 20;
        if (heightdiv1 < 50) {
            heightdiv1 = 50
        }
        div1.style.height = heightdiv1 + 'px'
        div1.style.width = '40px'
        div1.style.right = (window.innerWidth - boundEleItem.right - 20) + 'px';
        div1.style.left = 'auto';
        // div1.style.top = boundEleItem.top + 'px';
    }
    let boundEleH = eleHover.getBoundingClientRect()
    if (boundEleH.right >= (window.innerWidth || document.documentElement.clientHeightWidth)) {
        eleHover.style.right = (window.innerWidth - boundEleItem.left - 20) + 'px';
        eleHover.style.left = 'auto'
        if (div1) {
            div1.style.right = 'auto';
            div1.style.left = boundEleItem.left + 'px'
        }

    }
    if (boundEleH.bottom >= (window.innerHeight || document.documentElement.clientHeight)) {
        eleHover.style.bottom = '20px'
        eleHover.style.top = 'auto'
    }

})
$(document).on('mouseover', '.show-prop-2', function (e) {
    var id = $(this).attr('data-id');
    $(e.target).css('text-decoration', 'none');
    if (e?.fromElement?.classList.contains('custom-hover')) return;

    let eleTarget = e.currentTarget;
    let bound = eleTarget.getBoundingClientRect();
    let eleHover = e.currentTarget.nextElementSibling;
    var elementItem = document.getElementById('div_' + id);
    var boundEleItem = elementItem.getBoundingClientRect();

    if (!eleHover) return;
    eleHover.style.bottom = 'auto';
    eleHover.style.right = 'auto';
    if (bound.bottom + eleHover.offsetHeight < window.innerHeight) {
        eleHover.style.top = bound.bottom + 'px';
    } else {
        eleHover.style.top = (bound.top - eleHover.offsetHeight) + 'px';
    }

    eleHover.style.left = bound.left + 'px';

    let div1 = eleHover.getElementsByClassName('div1-custom-hover')[0];
    if (div1) {
        var heightdiv1 = $(this).height() + 20;
        if (heightdiv1 < 50) {
            heightdiv1 = 50;
        }
        div1.style.height = heightdiv1 + 'px';
        div1.style.width = '40px';
        div1.style.right = (window.innerWidth - boundEleItem.right - 20) + 'px';
        div1.style.left = 'auto';
    }

    let boundEleH = eleHover.getBoundingClientRect();
    if (boundEleH.right >= (window.innerWidth || document.documentElement.clientWidth)) {
        eleHover.style.right = (window.innerWidth - boundEleItem.left - 20) + 'px';
        eleHover.style.left = 'auto';
        if (div1) {
            div1.style.right = 'auto';
            div1.style.left = boundEleItem.left + 'px';
        }
    }

    if (boundEleH.bottom >= (window.innerHeight || document.documentElement.clientHeight)) {
        eleHover.style.bottom = '20px';
        eleHover.style.top = 'auto';
    }
    if (boundEleH.left <= bound.right) {
        eleHover.style.left = boundEleItem.left + 'px';
        eleHover.style.right = 'auto';
    }
});

function selectRowModal() {
    var allChecked = true;
    var hasCheck = false;
    var chkActionIds = '';
    $('.table-modal>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check-input:first');
        if (!chk.is(":checked")) {
            allChecked = false;
        }
        else {
            hasCheck = true;
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(chk).attr('value');
        }
    });
    $('.modalChkActionIds').attr('value', chkActionIds);
    if (hasCheck) {
        $('.bulk-select-actions-modal').removeClass('d-none');
    }
    else {
        $('.bulk-select-actions-modal').addClass('d-none');
    }

    if (allChecked) {
        $('.table-modal>thead>tr').each(function (index) {
            var chk = $(this).find('.form-check-input:first');
            if (chk) {
                chk.prop('checked', true);
            }
        });
    }
    else {
        $('.table-modal>thead>tr').each(function (index) {
            var chk = $(this).find('.form-check-input:first');
            if (chk) {
                chk.prop('checked', false);
            }
        });
    }
}
function removeSignatureForURL(str) {
    if (str == '') {
        return str;
    }
    var input = removerSignature(str);
    input = input.replaceAll("?", "").replaceAll("&", "").replaceAll("\"", "").replaceAll("'", "");
    input = input.replaceAll(".", "").replaceAll(",", "").replaceAll(";", "");
    input = input.replaceAll("@", "").replaceAll("$", "").replaceAll("%", "").replaceAll("*", "");
    input = input.replaceAll("~", "").replaceAll("\\", "").replaceAll("/", "").replaceAll("!", "");
    input = input.replaceAll(":", "").replaceAll(")", "").replaceAll("(", "").replaceAll("+", "");
    input = input.replaceAll("`", "").replaceAll("|", "");
    while (input == "  ") {
        input = input.replaceAll("  ", " ");
    }
    input = input.replaceAll(" ", "-");
    return input;
}

function searchDocRelate(e) {
    var self = $(e),
        url = self.data('url');
    var relateType = $('.select-relatetype').find(":selected").val();
    var reviewStatusId = parseInt($('.select-review').val());
    var docIdentity = $('.search-docidentity').val();
    var query = {
        relateTypeId: relateType.toString(),
        reviewStatusId: reviewStatusId,
        docIdentity: docIdentity
    }

    $.ajax({
        url: url,
        type: "GET",
        contentType: 'application/json',
        dataType: "html",
        data: query,
        success: function (response) {
            if (response) {

                $("#tblDocRelate").html(response);
            }
        },
        error: function (xhr, status, error) {
            toastMessage(false, "Vui lòng thử lại");
        }
    })

}

function removerSignature(str) {
    if (str == '') {
        return str;
    }
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, "");
    str = str.replace(/\u02C6|\u0306|\u031B/g, "");
    return str;
}

bindUrlWithUtm = (inputUrl, utmQueryParamValue) => {
    if (inputUrl != null && inputUrl.length > 0 && $(inputUrl).length) {
        var ipUrl = $(inputUrl);

        var url = ipUrl.val();
        url = removeQueryParam(url, "utm_source");
        url = removeQueryParam(url, "utm_medium");
        url = removeQueryParam(url, "utm_campaign");
        if (url != null && url.length > 0 && utmQueryParamValue != null && utmQueryParamValue.length > 0) {
            url = url + (url.indexOf('?') > 0 ? '&' : '?') + utmQueryParamValue;
        }
        ipUrl.val(url);
    }
}
function removeQueryParam(url, keyToRemove) {
    var urlParts = url.split('?');

    if (urlParts.length < 2) {
        return url;
    }

    var baseUrl = urlParts[0];
    var queryParams = urlParts[1].split('&');
    var updatedParams = [];

    for (var i = 0; i < queryParams.length; i++) {
        var param = queryParams[i].split('=');
        var paramName = param[0];
        if (paramName !== keyToRemove) {
            updatedParams.push(queryParams[i]);
        }
    }

    var updatedUrl = baseUrl + (updatedParams.length > 0 ? '?' + updatedParams.join('&') : '');

    return updatedUrl;
}
function onGetByCheckBox(e) {
    var self = $(e), url = self.data('ajax-url'), elementId = self.data('success-id');
    var ckReviewStatus = selectReview();
    var query = {
        ReviewStatusIds: ckReviewStatus
    }
    console.log(self, ckReviewStatus);
    if (url) {
        $.ajax({
            url: url,
            type: "GET",
            contentType: 'application/json',
            dataType: "html",
            data: query,
            success: function (response) {
                if (response) {
                    $(elementId).html(response);
                    $("#Query_ReviewStatusIds").val(ckReviewStatus)
                }
            },
            error: function (xhr, status, error) {
                toastMessage(false, "Vui lòng thử lại");
            }
        })
    }
}
function selectReview() {
    var chkActionIds = '';
    var checkbox = $('.checkReviewIds')
    checkbox.each(function () {
        if ($(this).is(":checked")) {
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(this).attr('value');
        }
    })
    return chkActionIds;
}
function removeFile(element, id) {
    const self = $(element),
        parent = self.closest('.row')

    if (parent) {
        let buttons = {
            'confirm': {
                text: "Đồng ý",
                btnClass: 'btn btn-danger',
                keys: ['enter'],
                action: function () {
                    fetchData({
                        url: '/BongDa24hDocs/DocFiles/Index?handler=Delete',
                        type: 'POST',
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                        },
                        dataType: 'json',
                        data: { 'id': id },
                    }).then(response => {
                        if (response && response.Succeeded) {
                            parent.remove()
                        }
                        toastMessage(response.Succeeded, response.Messages.join('<br />'));
                    })
                }
            }
        }

        showMessage({
            title: 'Xác nhận bỏ dữ liệu đã chọn ?',
            icon: 'fa fa-question-circle',
            columnClass: 'col-md-6 col-md-offset-3',
            message: 'Dữ liệu đã xóa sẽ không thể phục hồi !',
            buttons
        })
    }

    return false
}
function showMoreSites(e) {
    $(e).parents('div').next('.more-site').removeClass("d-none")
    $(e).addClass('d-none')
}
function HideMoreSites(e) {
    $(e).parents('div.more-site').addClass("d-none")
    $(e).parents('div.more-site').prev('.current-site').find(".show-more").removeClass("d-none")
}
function removeFaqFile(element, id) {
    const self = $(element),
        parent = self.closest('.row')

    if (parent) {
        let buttons = {
            'confirm': {
                text: "Đồng ý",
                btnClass: 'btn btn-danger',
                keys: ['enter'],
                action: function () {
                    fetchData({
                        url: '/BongDa24hDocs/Faqs/Index?handler=DeleteFaqFile',
                        type: 'POST',
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                        },
                        dataType: 'json',
                        data: { 'id': id },
                    }).then(response => {
                        if (response && response.Succeeded) {
                            parent.remove()
                        }
                        toastMessage(response.Succeeded, response.Messages.join('<br />'));
                    })
                }
            }
        }

        showMessage({
            title: 'Xác nhận bỏ dữ liệu đã chọn ?',
            icon: 'fa fa-question-circle',
            columnClass: 'col-md-6 col-md-offset-3',
            message: 'Dữ liệu đã xóa sẽ không thể phục hồi !',
            buttons
        })
    }

    return false
}
function toggleEditMode(id) {
    var displayElement = document.getElementById('display-' + id);
    var inputElement = document.getElementById('input-' + id);
    var editButton = document.getElementById('edit-' + id);
    var saveButton = document.getElementById('save-' + id);
    var closeButton = document.getElementById('close-' + id);
    var container = displayElement.closest('.edit-container');

    displayElement.classList.toggle('d-none');
    inputElement.classList.toggle('d-none');
    editButton.classList.toggle('d-none');
    saveButton.classList.toggle('d-none');
    closeButton.classList.toggle('d-none');
    container.classList.toggle('edit-mode');
    // container.closest('tr').classList.toggle('editing');
}