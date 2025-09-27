var currentAbortController = null;
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

        if (currentAbortController) {
            currentAbortController.abort();
            currentAbortController = null;
            console.log('⛔ Stream bị hủy do đóng modal.');
        }
    })

    modalDataPopup.on('hidden.bs.modal', function () {
        mediaSelected = []
        newsSelected = []
        //modalReset($('#modalDataPopup'))

        if (currentAbortController) {
            currentAbortController.abort();
            currentAbortController = null;
            console.log('⛔ Stream bị hủy do đóng modal.');
        }
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
    $(document).on('change', '.check-docItem', function () {
        if (this.checked) {
            $('.check-docItem').not(this).prop('checked', false);
        }
    });
    $(document).on('click', '.btn-submit', function () {
        const form = $(this).closest('form')
        if (form.length) {
            form.submit()
        }
    });
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

    $(document).on('click', '#save-convert-doc', function (event) {
        const self = $(this),
            docTempId = self.data('id');

        // ✅ Nếu đang xử lý thì bỏ qua
        if (self.attr('data-loading') === 'true' || !docTempId) return;

        const token = $('input:hidden[name="__RequestVerificationToken"]').val();

        const originalHtml = self.html();

        function sendConvertRequest(isCompleted) {
            const url = `/LuatVietNamDocItem/DocItemFullTemps?DocTempId=${docTempId}` +
                (isCompleted ? '&isCompleted=true' : '') +
                '&handler=GenFileWord';

            fetchData({
                url: url,
                type: 'post',
                dataType: 'json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('RequestVerificationToken', token);
                    self.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
                    self.attr('data-loading', 'true');
                }
            }).then(({ Data, Succeeded, Messages }) => {
                if (Succeeded && Data?.FilePathDomain) {
                    window.location.href = Data.FilePathDomain;
                    return;
                } else {
                    const message = Array.isArray(Messages) && Messages.length > 0
                        ? Messages.join('<br />')
                        : 'Đã xảy ra lỗi không xác định.';
                    toastMessage(false, message);
                }
            })
                .finally(() => {
                    self.html(originalHtml);
                    self.removeAttr('data-loading');
                });
        }

        const buttons = {
            confirm: {
                text: "Đồng ý",
                btnClass: 'btn btn-danger',
                keys: ['enter'],
                action: function () {
                    sendConvertRequest(true);
                }
            },
            somethingElse: {
                text: 'Không, tôi chỉ muốn tải file',
                btnClass: 'btn-blue',
                keys: ['enter', 'shift'],
                action: function () {
                    sendConvertRequest(false);
                }
            }
        };

        showMessage({
            title: 'Xác nhận',
            icon: 'fa fa-question-circle',
            columnClass: 'col-md-6 col-md-offset-3',
            message: 'Xác nhận tải file và hoàn thành convert cho văn bản này',
            buttons: buttons
        });
    });

    $(document).on('change', '.select-change-content', (e) => {
        var self = $(e.target);
        var value = self.find('option:selected').val();
        var id = self.data('id');
        var idResult = self.data('idresult');
        var urlRequest = self.data('url') || '';
        if (urlRequest.trim().length > 0 && $('#' + idResult).length) {
            var data = {};
            data[id] = value;
            if (value > 0) {
                fetchData({
                    url: urlRequest,
                    type: 'get',
                    data: data,
                }).then(response => {
                    if (response.Succeeded) {
                        $('#' + idResult).val(response.Data);
                        var editor = CKEDITOR.instances[idResult];
                        if (editor) {
                            editor.setData(response.Data);
                        }
                    }
                }).catch(error => {
                    console.error("Error:", error);
                });
            }
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
    $(document).on('keypress keyup blur', '.genDocUrl', (e) => {
        var self = $(e.target);
        var value = self.val()
        var fieldName = $('#Command_FieldName').val();
        var docId = $('#Command_DocId').val();
        var docGroupId = $('#Command_DocGroupId').val();
        $('#Command_DocUrl').val(`/${toSlug(fieldName, '-')}/${toSlug(self.val(), '-')}-${docId}-d${docGroupId}.html`)

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

    $(document).on('click', '#btn-spotlight', (e) => {
        var target = e.target,
            spotlightIndex = modalSpotlightPopup.attr('data-id') || '',
            timeElement = modalSpotlightPopup.find('input#Time').first(),
            contentElement = modalSpotlightPopup.find('textarea#Content').first()

        if (timeElement.val().trim().length == 0) {

            timeElement.focus()

            validationOnError(timeElement, 'Chưa chọn khung thời gian.')

            return false
        } else {
            if (!validateMediaDuration(timeElement.val().trim())) {

                validationOnError(timeElement, 'Vượt khung thời gian audio.')

                return false
            }
        }

        if (contentElement.val().trim().length == 0) {

            contentElement.focus()

            validationOnError(contentElement, 'Chưa nhập nội dung.')

            return false
        }

        const timeFormat = formatMediaDuration(timeElement.val().trim());

        var timeIndexExists = spotlightArray.findIndex(item => item.Time == timeFormat);

        if (spotlightIndex.trim().length > 0) {

            var index = spotlightArray.findIndex(item => item.Time == spotlightIndex);

            if (index != -1) {

                if (timeIndexExists != -1 && index != timeIndexExists) {

                    validationOnError(timeElement, `Khung thời gian ${timeElement.val()} đã tồn tại.`)

                    return;
                }

                spotlightArray[index].Time = timeFormat

                spotlightArray[index].Content = contentElement.val()

            }

        } else {

            if (timeIndexExists != -1) {

                validationOnError(timeElement, `Khung thời gian ${timeFormat} đã tồn tại.`)

                return;
            }

            spotlightArray.push({
                'Time': timeFormat,
                'Content': contentElement.val()
            })

        }

        bindDataSpotlight()

        timeElement.val('')

        contentElement.val('')

        modalSpotlightPopup.modal('hide')
    })
    $(document).on('click', '#toggle-doc-content', (e) => {
        var self = $(e.target)
        var docId = self.data('id');
        if (docId) {
            updateDocSummaryLayout(docId);
        }
    })
    $(document).on('click', '#toggle-root-content', (e) => {
        var self = $(e.target)
        var contentId = self.data('content-id');
        if (contentId) {
            var editor = CKEDITOR.instances[contentId];
            if (editor) {
                const newsContentCol = $('#news-root-content-col');
                const docContentCol = $('#doc-content-col');
                const toggleLink = $('#toggle-root-content');
                const container = $('#news-root-stream-container');
                const rowParent = self.parent().closest('.row');
                const contentCols = rowParent.find('.content-col');


                var editorContent = editor.getData();
                if (container.length && editorContent.length > 0) {
                    container.html(editorContent);
                }
                const isVisible = newsContentCol.hasClass('d-none');
                if (isVisible) {
                    toggleLink.text('Ẩn nội dung tạo bài viêt');
                    newsContentCol.removeClass('d-none').addClass('col-md-4');
                    contentCols.removeClass('col-md-6').addClass('col-md-4');

                } else {
                    toggleLink.text('Hiển thị nội dung tạo bài viêt');
                    newsContentCol.addClass('d-none').removeClass('col-md-4');
                    //contentCols.removeClass('col-md-4').addClass('col-md-6');

                    if (docContentCol.hasClass('d-none')) {
                        contentCols.removeClass('col-md-4').addClass('col-md-6');
                    }
                }
            }
        }
    })
    $(document).on('click', '#toggle-content', (e) => {
        var self = $(e.target)
        var contentId = self.data('content-id');
        var contentBackUpId = self.data('content-backup-id');
        var title = self.data('title');
        var resultId = self.data('result-id');
        if (contentId) {
            var editor = CKEDITOR.instances[contentId];
            if (editor) {
                const container = $('#' + resultId);
                const newsContentCol = container.parent().closest('.resultparent');

                const rowParent = self.parent().closest('.row');
                const contentCols = rowParent.find('.content-col');

                var editorContent = editor.getData();
                if (editorContent.length == 0) {
                    var editorBackUp = CKEDITOR.instances[contentBackUpId];
                    if (editorBackUp) {
                        editorContent = editorBackUp.getData();
                    }
                }
                if (container.length && editorContent.length > 0) {
                    container.html(editorContent);
                }

                const isVisible = newsContentCol.hasClass('d-none');
                if (isVisible) {
                    self.text('Ẩn ' + title);
                    newsContentCol.removeClass('d-none').addClass('col-md-4');
                    contentCols.removeClass('col-md-6').addClass('col-md-4');

                } else {
                    self.text('Hiển thị ' + title);
                    newsContentCol.addClass('d-none').removeClass('col-md-4');
                    contentCols.removeClass('col-md-4').addClass('col-md-6');
                }
            }
        }
    })
    $(document).on('click', '#auto-add-doclink', (e) => {
        var self = $(e.target)
        var idResult = self.data('content-id');
        var urlRequest = self.data('url') || '';
        if (idResult) {
            var editor = CKEDITOR.instances[idResult];
            if (editor) {
                var editorContent = editor.getData();
                if (editorContent.length > 0) {
                    $.ajax({
                        url: urlRequest,
                        type: 'POST',
                        data: { content: editorContent },
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val());
                            self.attr('disabled', 'disabled')
                            self.data('og', self.html())
                            self.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
                        },
                        success: function (response) {
                            if (response.Data.length > 0) {
                                editor.setData(response.Data);
                            }
                            self.attr('disabled', 'false');
                            self.html(self.data('og'));
                        },
                        error: function (xhr, status, error) {
                            console.error("Error:", error);
                            self.attr('disabled', 'false');
                            self.html(self.data('og'));
                        }
                    });
                }
            }
        }
    })
    $(document).on('click', '.delete-spotlight', (e) => {
        var target = e.target,
            parent = $(target).closest('.timeline-item'),
            spotlightIndex = parent.attr('data-id') || ''

        if (confirm('Xác nhận xóa spotlight đã chọn ?') && spotlightIndex.trim().length > 0) {

            var index = spotlightArray.findIndex(item => item.Time == spotlightIndex);

            if (index != -1) {

                spotlightArray.splice(index, 1)

            }
        }

        bindDataSpotlight()

        modalSpotlightPopup.modal('hide')
    })

    $(document).on('change', '.input-reviewStatusIds', function () {

        const form = $(this).closest('form'),
            selectedValues = $('.input-reviewStatusIds').filter(':checked').map(function () {
                return this.value
            }).get()

        const valueToSet = selectedValues.length > 0 ? selectedValues.join(',') : '0'

        $('#Query_ReviewStatusIds').val(valueToSet)

        if (form.length) {
            form.submit()
        }
    })

    $(document).on('change', '.select-link', (e) => {
        var self = $(e.target), id = self.data('id'),
            value = self.find('option:selected').val(),
            urlRequest = self.data('url') || '', linkTo = self.data('link-to'),
            currentValue = $(linkTo).data('value') || '', isFilter = self.data('filter') || false

        if (typeof id != 'undefined' && typeof linkTo != 'undefined' && urlRequest.trim().length > 0) {

            var data = {}

            data[id] = value

            $(`${linkTo} option[value!=""][value!="0"]`).remove()

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

                    if (isFilter) {
                        currentValue = $(`${linkTo} option:first`).val()
                    } else if (response.Data.filter(x => x.id == currentValue).length == 0) {
                        currentValue = response.Data[0].id
                    }

                    $(linkTo).val(currentValue).trigger('change')
                })

            }
        }
    })
    $(document).on('change', '.select-link-2', (e) => {

        var self = $(e.target), id = self.data('id'),
            value = self.find('option:selected').val(),
            urlRequest = self.data('url') || '',
            urlRequestUpdate = self.data('url-update') || '',
            linkTo = self.data('link-to'),
            currentValue = $(linkTo).data('value') || '', isFilter = self.data('filter') || false;

        if (typeof urlRequestUpdate != 'undefined' && urlRequest != null) {
            urlRequest = urlRequestUpdate;
        }

        if (typeof id != 'undefined' && typeof linkTo != 'undefined' && urlRequest.trim().length > 0 && $(linkTo).length) {
            var data = {}

            data[id] = value

            $(`${linkTo} option[value!=""][value!="0"]`).remove()

            if (value >= 0) {

                fetchData({
                    url: urlRequest,
                    type: 'get',
                    dataType: 'json',
                    data: data,
                }).then(response => {
                    $.each(response.Data, function (key, value) {

                        var newOption = $('<option></option>')
                            .attr('value', value.id)
                            .text(value.text);
                        if (value.title.length) {
                            newOption.attr('data-FullName', value.title);
                        }
                        $(linkTo).append(newOption);
                    });

                    //Kiểm tra giá trị hiện tại có nằm trong danh sách mới không
                    if (!response.Data.some(item => item.id === currentValue)) {
                        //exists
                        currentValue = response.Data[0].id;
                    }

                    if (isFilter && response.Data.filter(x => x.id == currentValue).length == 0) {
                        currentValue = response.Data[0].id
                    }

                    $(linkTo).val(currentValue).trigger('change');
                })

            } else {
                $(linkTo).trigger('change');
            }
        }
    })
    $(document).on('change', '.select-link-docItem', (e) => {
        var self = $(e.target), id = self.data('id'),
            urlRequest = self.data('url') || '',
            urlRequestUpdate = self.data('url-update') || '',
            docItemId = self.data('dociemid') || 0,
            value = self.find('option:selected').val(),
            linkTo = self.data('link-to');
        if (typeof urlRequestUpdate != 'undefined' && urlRequest != null) {
            urlRequest = urlRequestUpdate;
        }
        if (typeof id != 'undefined' && urlRequest.trim().length > 0) {
            var data = {}

            data[id] = value;
            data['docItemId'] = docItemId;
            if (value > 0) {
                fetchData({
                    url: urlRequest,
                    type: 'get',
                    dataType: 'html',
                    data: data,
                }).then(response => {
                    $(linkTo).html(response)

                })
            }
            else {
                $(linkTo).html('')
            }
        }
    })
    $(document).on('change', '.changeDocName', (e) => {
        var lawJudgType = document.getElementById("Command_LawJudgTypeId");
        var lawJudgNumber = document.getElementById("Command_DocNumber");
        var trichYeu = document.getElementById("Command_TrichYeu");
        var publishDate = document.getElementById("Command_PublishDateText");
        var court = document.getElementById("Command_CourtId");
        var lawJudgName = document.getElementById("Command_Title");
        var lawJudgTypeName = '', courtName = '', lawJudgTypeId = 0, courtId = 0;
        var lawJudgNumberValue = '', trichYeuValue = '', publishDateValue = '';
        if ($(lawJudgType).length && lawJudgType.selectedIndex >= 0) {
            lawJudgTypeId = lawJudgType.value;
            lawJudgTypeName = lawJudgType.options[lawJudgType.selectedIndex].text == "..." ? "" : lawJudgType.options[lawJudgType.selectedIndex].text;
        }
        if ($(court).length && court.selectedIndex >= 0) {
            courtId = court.value;
            courtName = court.options[court.selectedIndex].text == "..." ? "" : court.options[court.selectedIndex].text;
        }
        if (lawJudgNumber != null && lawJudgNumber.length) {
            lawJudgNumberValue = lawJudgNumber.value;
        }
        if (publishDate != null && publishDate.length) {
            publishDateValue = publishDate.value.replaceAll('-', '/');
        }
        if (trichYeu != null && trichYeu.length) {
            trichYeuValue = trichYeu.value;
        }
        if (lawJudgName != null && lawJudgName.length) {
            lawJudgName.value = lawJudgTypeName + " số " + lawJudgNumberValue + " ngày " +
                publishDateValue + " của " + courtName + " về " + trichYeuValue;
        }


    })
    $(document).on('keypress keyup blur', '.data-maxlength', (e) => {
        var self = $(e.target)
        var value = self.val();

        var dataOverflow = self.data('length')
        var sib = self.siblings('.text-limit,.text-limit2')
        if (value.length > dataOverflow) {
            sib.addClass('maxlength-overflow')
        }
        else {
            sib.removeClass('maxlength-overflow')
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

    $(document).on('change', '.select-news-category', (e) => {
        var target = $(e.target), form = target.closest('form'),
            option = target.find('option:selected'),
            lft = $(option).attr('data-lft'), rgt = $(option).attr('data-rgt')

        if (form) {

            if (lft && rgt) {

                form.find('input#Query_lft').val(lft)

                form.find('input#Query_rgt').val(rgt)

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

    //$(document).on('click', '.contentRaw', (e) => {
    //    var target = $(e.currentTarget);
    //    console.log(target)
    //    var td = target.closest('td');
    //    console.log(1123, td)
    //    if (td.length > 0) {
    //        var content = td.find('.contenthtml')
    //        console.log(content)
    //        if (content.length > 0) {
    //            content.removeClass('d-none')
    //        }
    //    }
    //    initCkEditor($('.ckeditor-input-content'));
    //})
    $(document).on('click', '.btn-up-file-to-textbox, .btn-up-file-to-textbox *', (e) => {
        const ip = $(e.target).hasClass('btn-up-file-to-textbox') ? $(e.target) : $(e.target).closest('.btn-up-file-to-textbox');
        var dataType = ip.attr('data-DataType'),
            fileTypeId = ip.attr('data-FileTypeId'),
            dataId = ip.attr('data-DataId'),
            IsNoAutoReplaceName = ip.attr('data-IsNoAutoReplaceName') ?? 0;

        if (dataType == null || typeof dataType == undefined || dataType == '') {
            if ($('#Command_DataType').length) {
                dataType = $('#Command_DataType').val();
            }
        }
        if (dataId == null || typeof dataId == undefined || dataId == 0) {
            if ($('#Command_DataId').length) {
                dataId = $('#Command_DataId').val();
            }
        }

        var resultId = '';
        elementAction = ip;
        var parent = ip.closest('.box-upload');
        if (parent != null && parent.length) {
            var elementResult = parent.find('input').first();
            if (elementResult.length) {
                resultId = elementResult.attr('id');
            }
        }

        if (dataType && fileTypeId && dataId) {
            $('#fileUploadCreateCommand_DataType').val(dataType);
            $('#fileUploadCreateCommand_FileTypeId').val(fileTypeId);
            $('#fileUploadCreateCommand_DataId').val(dataId);
            $('#fileUploadCreateCommand_ElementResultId').val(resultId);

            if (IsNoAutoReplaceName > 0 && $('#fileUploadCreateCommand_IsNoAutoReplaceName').length) {
                $('#fileUploadCreateCommand_IsNoAutoReplaceName').val(true);
            }
            $('#FileUploads').closest('form').attr('data-ajax-progress', 'progress' + dataType + '_' + fileTypeId + '_' + dataId);
            $('#FileUploads').trigger('click');
            return false;
        }
    });

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
    $(document).on('change', '.searchBy', (e) => {
        var self = $(e.target),
            value = self.val();
        $('.searchDocRef').data('url', '/LuatVietNamDoc/Docs?handler=SearchDocs&searchBy=' + value + '')
        initSelect2AutoComplete($('.searchDocRef'))
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

    $(document).on('blur', '.gen-slug', (e) => {
        var slug = removeSignatureForURL($('.gen-slug').val());
        $('input[id ="Command_Slug"]').val(slug)
    })

    $(document).on('click', 'button[type="submit"]', (e) => {
        for (let instance in CKEDITOR.instances) {
            CKEDITOR.instances[instance].updateElement();
        }
        elementAction = $(e.currentTarget)
    })
    $(document).on('change', '.select-courtLevel', (e) => {
        var self = $(e.target);
        var value = self.find('option:selected').val();
        var courstId = self.data('courtid');
        var id = self.data('id');
        var urlRequest = self.data('url') || '';
        if (urlRequest.trim().length > 0) {
            var data = {};
            data[id] = value;
            if (value > 0) {
                fetchData({
                    url: urlRequest,
                    type: 'get',
                    data: data,
                }).then(response => {

                    var html = '<option value=0>...</option>'
                    $.each(response, function (index, data) {
                        html += '<option value=' + data.id + '>' + data.courtDesc + '</option>'
                    })

                    $('.select-court').html(html)
                    $(courstId).html(html)
                }).catch(error => {
                    console.error("Error:", error);
                });
            }
        }

    })

    $(document).on('mouseover', '.show-prop', function (e) {
        var id = $(this).attr('data-id');

        if ($('#doc_' + id).html() == null || $('#doc_' + id).html().length <= 0) {
            var query = { id: parseInt(id.split('_')[0].replace(/\D/g, "")) };
            $.ajax({
                url: "/LuatVietNamDoc/Docs/Index?handler=DocProperties",
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

    $(document).on('select2:select select2:unselect', '.select-cate', (e) => {
        var self = $(e.target);
        var values = self.val();
        var legalId = self.data('legalid');
        var id = self.data('id');
        var cateIds = "";
        $.each(values, function (item, value) {
            if (value > 0 && item < values.length - 1)
                cateIds += value + ','
            else {
                cateIds += value
            }
        })
        var urlRequest = self.data('url') || '';
        if (urlRequest.trim().length > 0) {
            var data = {
                cateIds: cateIds
            };

            if (cateIds.length > 0) {
                fetchData({
                    url: urlRequest,
                    type: 'get',
                    data: { cateIds: cateIds },
                }).then(response => {
                    var html = '<option value=0>...</option>'
                    $.each(response, function (index, data) {
                        html += '<option value=' + data.id + '>' + data.title + '</option>'
                    })
                    //$('.select-court').html(html)
                    $(legalId).html(html)
                }).catch(error => {
                    console.error("Error:", error);
                });
            }
        }

    })
    $(document).on('select2:select select2:unselect', '.select-field', (e) => {
        var self = $(e.target);
        var values = self.val();
        var laweId = self.data('lawerId');
        var id = self.data('id');
        var fieldIds = "";
        $.each(values, function (item, value) {
            if (value > 0 && item < values.length - 1)
                fieldIds += value + ','
            else {
                fieldIds += value
            }
        })
        var urlRequest = self.data('url') || '';
        if (urlRequest.trim().length > 0) {
            var data = {
                fieldIds: fieldIds
            };

            if (fieldIds.length > 0) {
                fetchData({
                    url: urlRequest,
                    type: 'get',
                    data: { fieldIds: fieldIds },
                }).then(response => {
                    var html = '<option value=0>...</option>'
                    $.each(response, function (index, data) {
                        html += '<option value=' + data.id + '>' + data.title + '</option>'
                    })
                    //$('.select-court').html(html)
                    $(laweId).html(html)
                }).catch(error => {
                    console.error("Error:", error);
                });
            }
        }

    })
    $(document).on('change', '#Command_LawJudgTypeId', (e) => {
        var self = $(e.target);
        value = self.val();

        if (parseInt(value) == 3) {
            $(".is-ban-an").hide()
            $(".is-an-le").show()
        }
        else {
            $(".is-ban-an").show()
            $(".is-an-le").hide()
        }
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

    $(document).on('change', '.editor-news-checkbox', (e) => {
        let self = $(e.currentTarget), isChecked = self.is(':checked'),
            newsId = self.val(), title = self.attr('data-title') || '',
            slug = self.attr('data-slug') || ''

        isExist = newsSelected.filter(x => x.id && x.id == newsId)

        if (newsId.trim().length == 0) {
            showMessage({ message: 'Vui lòng thử lại sau.' })
            return
        }

        if (isChecked) {
            if (isExist.length == 0) {
                newsSelected.push({
                    id: newsId,
                    title: title,
                    slug: slug
                })
            }
        } else {
            if (isExist.length > 0) {
                newsSelected.splice(isExist, 1)
            }
        }
    })
    $(document).on('change', '.select-lawjudgs', (e) => {
        var self = $(e.target)
        var lawJudgId = self.val();

        if (parseInt(lawJudgId) > 0) {
            $('.select-docs').attr('disabled', 'disabled')
        }
        else {
            $('.select-docs').removeAttr('disabled')
        }
    })
    $(document).on('change', '.select-docs', (e) => {
        var self = $(e.target)
        var docId = self.val();

        if (parseInt(docId) > 0) {
            $('.select-lawjudgs').attr('disabled', 'disabled')
        }
        else {
            $('.select-lawjudgs').removeAttr('disabled')
        }
    })
    $(document).on('click', '.btnCloseOffCanvas', (e) => {
        var self = $(e.target)
        if (CKEDITOR.instances['DocContent']) {
            CKEDITOR.instances['DocContent'].setData('');
        }

        var offcanvas = self.closest('.offcanvas');
        var bsOffcanvas = bootstrap.Offcanvas.getInstance(offcanvas[0]);

        if (bsOffcanvas) {
            bsOffcanvas.hide();
        }
    })
    $(document).on('click', 'button[data-confirm]', function (e) {
        var self = $(this);
        var confirmTitle = self.data('confirm');
        var message = self.data('message') || '';
        var form = self.closest('form');

        e.preventDefault();

        let buttons = {
            confirm: {
                text: "Đồng ý",
                btnClass: 'btn btn-danger',
                keys: ['enter'],
                action: function () {

                    // Đồng bộ dữ liệu CKEditor về textarea trước khi submit
                    for (let instance in CKEDITOR.instances) {
                        CKEDITOR.instances[instance].updateElement();
                    }

                    form.find('input[type="hidden"][data-temp-submit]').remove();

                    $('<input>', {
                        type: 'hidden',
                        name: self.attr('name'),
                        value: self.val(),
                        'data-temp-submit': true
                    }).appendTo(form);

                    form.trigger('submit');
                }
            }
        };

        showMessage({
            title: confirmTitle,
            icon: 'fa fa-question-circle',
            columnClass: 'col-md-6 col-md-offset-3',
            message: message,
            buttons: buttons
        });
    });
    $(document).on('change', '.select-lawjudgs-modal', (e) => {
        var self = $(e.target)
        var lawJudgId = self.val();

        if (parseInt(lawJudgId) > 0) {
            $('.select-docs-modal').attr('disabled', 'disabled')
        }
        else {
            $('.select-docs-modal').removeAttr('disabled')
        }
    })
    $(document).on('change', '.select-docs-modal', (e) => {
        var self = $(e.target)
        var docId = self.val();

        if (parseInt(docId) > 0) {
            $('.select-lawjudgs-modal').attr('disabled', 'disabled')
        }
        else {
            $('.select-lawjudgs-modal').removeAttr('disabled')
        }
    })
    $(document).on('click', '.btn-add-doc', (e) => {
        var self = $(e.target)
        var lawJudgId = $("#Query_LawJudgId").val()
        $('.text-validate .text-required').hide();
        var lawJudgRelateTypeId = self.data('type');
        var lawJudgRelateIds;
        var docRelateIds;
        var url = $(".reoloadUrl").data('ajax-success-url')
        if (parseInt(lawJudgRelateTypeId) == 2 || parseInt(lawJudgRelateTypeId) == 3) {
            docRelateIds = self.parent().prev().find(".select2-auto-complete-modal").val().join(",");
        }
        else {
            lawJudgRelateIds = self.parent().prev().find(".select2-auto-complete-modal").val().join(",")
        }
        var command = {
            lawJudgId: parseInt(lawJudgId),
            lawJudgRelateTypeId: parseInt(lawJudgRelateTypeId),
            lawJudgRelateIds: lawJudgRelateIds,
            docRelateIds: docRelateIds,
            reviewStatusId: 1
        }

        if ((lawJudgRelateIds == '' || !lawJudgRelateIds) && (!docRelateIds || docRelateIds == '')) {
            self.closest('.box-relate').next('.text-validate ').find('.text-required').show();
            return;
        } else {
            self.closest('.box-relate').next('.text-validate').find('.text-required').hide();

            $.ajax({
                url: "/LuatVietNamBanAn/LawJudgRelates?handler=AddLawJudgRelates",
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
                            $("#tblLawJudgRelate").html(response);
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
    $(document).on('click', '.btn-add-link', (e) => {
        var self = $(e.target);
        var urladd = self.data('ajax-url');
        var urlSuccess = self.data('ajax-success-url');
        var succesId = self.data('ajax-success-id')
        var lawJudgId = $('#Item3_LawJudgId').val();
        var textContent = $('#Item3_TextContent').val();
        var link = $('#Item3_Link').val();
        var linkLawJudgIds = $('#Item3_LinkLawJudgId').val();
        var linkDocIds = $('#Item3_LinkDocId').val();
        var linkDocItemId = $('#Item3_LinkDocItemId').val();

        var linkLawJudgId = linkLawJudgIds[0];
        var linkDocId = linkDocIds[0];
        var command = {
            LawJudgId: parseInt(lawJudgId),
            TextContent: textContent,
            Link: link,
            LinkLawJudgId: parseInt(linkLawJudgId),
            LinkDocId: parseInt(linkDocId),
            LinkDocItemId: parseInt(linkDocItemId)
        }
        $.ajax({
            url: urladd,
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
                        url: urlSuccess,
                        dataType: 'html',
                    }).then(response => {
                        $(succesId).html(response);
                    })
                }
                toastMessage(response.Succeeded, response.Messages.join('<br />'))
                $('#Item3_Link').val('');
                $('#Item3_TextContent').val('');
            },
            error: function (xhr, status, error) {
                toastMessage(false, "Vui lòng thử lại");
            }
        })
        initSelect2AutoComplete($('.select2-auto-complete-modal'))
        $('#Item3_Link').val('');
        $('#Item3_TextContent').val('');
    })
    if (cardHeaderSecondary.length) {
        $(window).scroll(function () {
            cardHeaderSecondary.toggleClass('fix-header', $(window).scrollTop() > cardHeaderSecondary.position().top);
        });
    }

    $(document).on('click', '.edit-parent', function (e) {
        var self = $(this);
        var td = self.closest('td');
        var parentName = td.find('.parentName')
        var newParent = td.find('.newParent')
        var docId = $(this).data('docid');
        var docitemId = $(this).data('docitemid')
        var parentId = parseInt($(this).data('parentid'))
        if (parentName.length > 0) {
            parentName.addClass('d-none')
        }
        if (newParent.length > 0) {
            newParent.removeClass('d-none')
        }
        $.ajax({
            type: 'GET',
            url: "/LuatVietNamDocItem/DocItemFulls/Index?handler=NeartestParent",
            data: {
                docId: docId,
                docItemId: docitemId
            },
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                var html = '<option value=0>...</option>'
                var selected = '';
                if (data) {
                    $.each(data, function (i, item) {
                        if (item.id == parentId) {
                            selected = 'selected';
                        }
                        html += '<option ' + selected + ' value=' + item.id + '>' + item.name + '</option>'
                    })
                    newParent.html(html)

                }
            }
        })
        $(document).one("click", function () {
            newParent.addClass('d-none');
            parentName.removeClass('d-none');

        });
        $('.newParent').on("click", function (e) {
            e.stopPropagation();
        });
    })
    $(document).on('click', '.edit-level', function (e) {
        var self = $(this);
        var td = self.closest('td');
        var levelName = td.find('.levelname')
        var newLevel = td.find('.newLevel')
        var levelId = parseInt($(this).data('levelid'))
        if (levelName.length > 0) {
            levelName.addClass('d-none')
        }
        if (newLevel.length > 0) {
            newLevel.removeClass('d-none')
        }
        $.ajax({
            type: 'GET',
            url: "/LuatVietNamDocItem/DocItemFulls/Index?handler=ListDocItemLevel",
            dataType: 'json',
            success: function (data) {
                var html = '<option value=0>...</option>'

                if (data) {
                    $.each(data, function (i, item) {
                        var selected = '';
                        if (item.id == levelId) {
                            selected = 'selected';
                        }
                        html += '<option ' + selected + ' value=' + item.id + '>' + item.name + '</option>'

                    })
                    newLevel.html(html)

                }
            }
        })
        $(document).one("click", function () {
            newLevel.addClass('d-none');
            levelName.removeClass('d-none');

        });
        $('.newLevel').on("click", function (e) {
            e.stopPropagation();
        });
    })
    $(".edit-info").on({
        mouseenter: function () {
            var self = $(this);
            var td = self.closest('td');
            var edit = td.find('.edit-level')
            if (edit.length > 0) {
                edit.removeClass('d-none')
            }
        },
        mouseleave: function () {
            var self = $(this);
            var td = self.closest('td');
            var edit = td.find('.edit-level')
            if (edit.length > 0) {
                edit.addClass('d-none')
            }
        }
    });
    $(document).on('click', '.edit-parent-temp', function (e) {
        var self = $(this);
        var td = self.closest('td');
        var parentName = td.find('.parentName')
        var newParent = td.find('.newParent')
        var docId = $(this).data('doctempid');
        var docitemId = $(this).data('id')
        var parentId = parseInt($(this).data('parentid'))
        if (parentName.length > 0) {
            parentName.addClass('d-none')
        }
        if (newParent.length > 0) {
            newParent.removeClass('d-none')
        }
        $.ajax({
            type: 'GET',
            url: "/LuatVietNamDocItem/DocItemFullTemps/Index?handler=NeartestParent",
            data: {
                docId: docId,
                docItemId: docitemId
            },
            contentType: 'application/json',
            dataType: 'json',
            success: function (data) {
                var html = '<option value=0>...</option>'
                var selected = '';
                if (data) {
                    $.each(data, function (i, item) {
                        if (item.id == parentId) {
                            selected = 'selected';
                        }
                        html += '<option ' + selected + ' value=' + item.id + '>' + item.name + '</option>'
                    })
                    newParent.html(html)

                }
            }
        })
        $(document).one("click", function () {
            newParent.addClass('d-none');
            parentName.removeClass('d-none');

        });
        $('.newParent').on("click", function (e) {
            e.stopPropagation();
        });
    })
    $(document).on('click', '.edit-level-temp', function (e) {
        var self = $(this);
        var td = self.closest('td');
        var levelName = td.find('.levelname')
        var newLevel = td.find('.newLevel')
        var levelId = parseInt($(this).data('levelid'))
        if (levelName.length > 0) {
            levelName.addClass('d-none')
        }
        if (newLevel.length > 0) {
            newLevel.removeClass('d-none')
        }
        $.ajax({
            type: 'GET',
            url: "/LuatVietNamDocItem/DocItemFullTemps/Index?handler=ListDocItemLevel",
            dataType: 'json',
            success: function (data) {
                var html = '<option value=0>...</option>'

                if (data) {
                    $.each(data, function (i, item) {
                        var selected = '';
                        if (item.id == levelId) {
                            selected = 'selected';
                        }
                        html += '<option ' + selected + ' value=' + item.id + '>' + item.name + '</option>'

                    })
                    newLevel.html(html)

                }
            }
        })
        $(document).one("click", function () {
            newLevel.addClass('d-none');
            levelName.removeClass('d-none');

        });
        $('.newLevel').on("click", function (e) {
            e.stopPropagation();
        });
    })
    $(".edit-info-temp").on({
        mouseenter: function () {
            var self = $(this);
            var td = self.closest('td');
            var edit = td.find('.edit-level-temp')
            if (edit.length > 0) {
                edit.removeClass('d-none')
            }
        },
        mouseleave: function () {
            var self = $(this);
            var td = self.closest('td');
            var edit = td.find('.edit-level-temp')
            if (edit.length > 0) {
                edit.addClass('d-none')
            }
        }
    });


    verticalNavbarInit()

    stickyHeaderTableInit()

    flatpickrInit()
    flatpickrTimeInit()

    initViewMaxLength()

    initCkEditor()
    initCkEditorInline()

    initSelect2AutoComplete()

    initAutocomplete()

    initTooltip()

    initDocReviewStatusCount()

    initDocReviewReviewStatusCount()

    initDocStandardReviewStatusCount()

    initTooltipDocProperties()

    initTooltipDocProperties2()

})

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
var initCkEditorInline = (element) => {
    const editorInline = element || $('.ckeditor-div-inline');

    if (editorInline.length > 0) {
        editorInline.each((index, item) => {
            var input = $(item), config = input.data('config') || 'basic', mode = input.data('mode') || ''
            maxlength = input.attr('maxlength'), backgroundColor = input.data('bg-color') || '#fff'
            height = input.data('height') || '200px',
                input.attr('contenteditable', 'true');
            if (CKEDITOR.instances[input.attr('id')]) {
                const editor = CKEDITOR.instances[input.attr('id')];
                //   editor.destroy(false);
            }
            setTimeout(() => {
                var editor = CKEDITOR.inline(input.attr('id'), {
                    disableAutoInline: true,
                    disableNativeSpellChecker: false,
                    removePlugins: 'scayt,wsc',        // Xóa các plugin kiểm tra chính tả nếu có
                    scayt_autoStartup: true,
                    versionCheck: false,
                    scayt_autoStartup: false,
                    customConfig: `/vendors/ckeditor/configs/${config}.js`,
                    maxLength: maxlength,
                    height: height,
                    shouldNotGroupWhenFull: true,
                    on: {
                        instanceReady: function (ev) {
                            const self = this;
                            const editable = self.editable();
                            const editableEl = editable.$;
                            const toolbar = self.ui.space('top');
                            const container = editableEl.closest('.contentItem');

                            // Tắt spellcheck của trình duyệt trong vùng soạn thảo
                            ev.editor.editable().$.setAttribute('spellcheck', 'false');

                            // 🎨 Nền editor
                            editable.setStyle('background-color', backgroundColor);

                            // 📐 Căn toolbar
                            function positionToolbar() {
                                if (!toolbar || !container) return;
                                const rect = container.getBoundingClientRect();
                                toolbar.setStyle('position', 'fixed');
                                toolbar.setStyle('top', '25px');
                                toolbar.setStyle('left', `${rect.left}px`);
                                toolbar.setStyle('width', `${rect.width}px`);
                                toolbar.setStyle('z-index', '9999');
                                toolbar.setStyle('background', '#fff');
                                toolbar.setStyle('box-shadow', '0 2px 5px rgba(0,0,0,0.1)');
                            }

                            positionToolbar();

                            // 🔁 Update toolbar position when resize
                            window.addEventListener('resize', positionToolbar);

                            // 👁‍🗨 Toggle toolbar
                            let toolbarHidden = false;
                            function toggleToolbar(hidden) {
                                const toolbarContainer = document.getElementById('cke_' + self.name);
                                if (!toolbarContainer) return;

                                toolbarContainer.querySelectorAll('.cke_toolbar').forEach(tb => {
                                    tb.style.display = hidden ? 'none' : '';
                                });

                                const topBar = toolbarContainer.querySelector('.cke_top');
                                if (topBar) topBar.style.display = hidden ? 'none' : '';

                                if (hidden) {
                                    editable.addClass('editor-hide-border');
                                } else {
                                    editable.removeClass('editor-hide-border');
                                }

                                toolbarHidden = hidden;
                            }

                            // 📌 Ẩn khi cuộn nhiều
                            let lastScrollY = window.scrollY;
                            const scrollThreshold = 30;
                            function handleScroll() {
                                const currentY = window.scrollY;
                                const delta = Math.abs(currentY - lastScrollY);
                                if (delta >= scrollThreshold && !toolbarHidden) {
                                    toggleToolbar(true);
                                }
                                lastScrollY = currentY;
                            }

                            // 👇 Ẩn khi giữ Shift/Alt
                            function checkModifierKeys(e) {
                                const shouldHide = e.getModifierState('Shift') || e.getModifierState('Alt');
                                if (shouldHide && !toolbarHidden) toggleToolbar(true);
                                else if (!shouldHide && toolbarHidden && self.focusManager.hasFocus) toggleToolbar(false);
                            }

                            // 👁‍🗨 Quan sát editor trong khung nhìn
                            const observer = new IntersectionObserver((entries) => {
                                entries.forEach(entry => {
                                    if (entry.target === editableEl) {
                                        if (!entry.isIntersecting && !toolbarHidden) toggleToolbar(true);
                                        else if (entry.isIntersecting && toolbarHidden && self.focusManager.hasFocus) toggleToolbar(false);
                                    }
                                });
                            }, { threshold: 0.01 });
                            observer.observe(editableEl);

                            // 🔗 Sự kiện focus/blur
                            self.on('focus', () => {
                                document.addEventListener('keydown', checkModifierKeys);
                                document.addEventListener('keyup', checkModifierKeys);
                                document.addEventListener('pointermove', checkModifierKeys);
                                window.addEventListener('scroll', handleScroll);
                                toggleToolbar(false);
                                positionToolbar();
                            });

                            self.on('blur', () => {
                                document.removeEventListener('keydown', checkModifierKeys);
                                document.removeEventListener('keyup', checkModifierKeys);
                                document.removeEventListener('pointermove', checkModifierKeys);
                                window.removeEventListener('scroll', handleScroll);
                                toggleToolbar(true);
                            });
                        },
                        afterCommandExec: function (evt) {
                            if (evt.data.name == 'indent' || evt.data.name == 'outdent') {
                                var selection = this.getSelection();
                                var ranges = selection.getRanges();
                                ranges.forEach(function (range) {
                                    var startContainer = range.startContainer.getAscendant('p', true);
                                    if (startContainer) {
                                        var style = startContainer.getAttribute('style');
                                        if (style && style.includes('text-indent')) {
                                            style = style.replace(/text-indent:\s*[^;]+;?/gi, '');
                                            if (style.trim() === '') {
                                                startContainer.removeAttribute('style');
                                            } else {
                                                startContainer.setAttribute('style', style);
                                            }
                                        }
                                    }
                                });
                            }
                        }

                    },
                    loaded: function () {
                        // Thêm CSS custom
                        CKEDITOR.addCss(
                            '.cke_toolbar { max-width: 600px !important; margin: 0 auto !important; }'
                        );
                    }
                })
                if (editor) {
                    editor.on('change', function () {
                        var newContent = editor.getData();
                        var id = input.data('id');
                        var url = input.data('update-url')
                        var command = {
                            id: parseInt(id),
                            docItemContentHtml: newContent
                        }
                        $.ajax({
                            url: url,
                            method: 'post',
                            dataType: 'json',
                            contentType: 'application/json',
                            data: JSON.stringify(command),
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
                            },
                            success: function (response) {
                                //toastMessage(response.Succeeded, response.Messages.join('<br />'))
                            },
                            error: function (xhr, status, error) {
                                toastMessage(false, "Vui lòng thử lại sau")
                            }
                        })
                    })
                }
            }, 1)
        })
    }
}
var initCkEditor = (element) => {
    const editors = element || $('.ckeditor-input')

    if (editors.length > 0) {

        editors.each((index, item) => {
            var input = $(item), config = input.data('config') || 'basic', mode = input.data('mode') || ''
            maxlength = input.attr('maxlength'), backgroundColor = input.data('bg-color') || '#fff'
            height = input.data('height') || '200px'
            if (mode == 'inline') {
                input.attr('contenteditable', 'true');
                CKEDITOR.inline(input.attr('id'), {
                    disableAutoInline: true,
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

            } else {

                CKEDITOR.replace(input.attr('id'), {
                    entities: false,
                    disableNativeSpellChecker: true,
                    versionCheck: false,
                    scayt_autoStartup: false,
                    removeEmpty: true,
                    extraAllowedContent: '',
                    customConfig: `/vendors/ckeditor/configs/${config}.js`,
                    maxLength: maxlength,
                    height: height,
                    on: {
                        instanceReady: function (evt) {
                            this.editable().setStyle('background-color', backgroundColor);
                            var editor = evt.editor;

                            var indentCommand = editor.getCommand('outdent');
                            indentCommand.refresh = function (editor, path) {
                                this.setState(CKEDITOR.TRISTATE_ON);
                            };
                        },

                        afterCommandExec: function (evt) {
                            if (evt.data.name == 'indent' || evt.data.name == 'outdent') {
                                var selection = this.getSelection();
                                var ranges = selection.getRanges();
                                ranges.forEach(function (range) {
                                    var startContainer = range.startContainer.getAscendant('p', true);
                                    if (startContainer) {
                                        var style = startContainer.getAttribute('style');
                                        if (style && style.includes('text-indent')) {
                                            style = style.replace(/text-indent:\s*[^;]+;?/gi, '');
                                            if (style.trim() === '') {
                                                startContainer.removeAttribute('style');
                                            } else {
                                                startContainer.setAttribute('style', style);
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                })

            }
        })
    }
    /* contentsCss: ['data:text/css,ol%2Cul%7Bfont-size%3Ainherit%20!important%3B%7D'],*/
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
                    { name: 'insert', items: ['ListTable'] },
                    { name: 'styles', items: ['Styles'] },
                    { name: 'basicstyles', items: ['Bold', 'Italic'] },
                    { name: 'colors', items: ['TextColor', 'BGColor'] },
                    { name: 'document', items: ['Source'] }
                ],
                maxLength: maxlength,
                height: height,
                extraPlugins: 'listtable',
                contentsCss: ['/css/ckeditor-custom.css'],
                on: {
                    instanceReady: function (evt) {
                        this.editable().setStyle('background-color', backgroundColor);
                        var editor = evt.editor;

                        var indentCommand = editor.getCommand('outdent');
                        indentCommand.refresh = function (editor, path) {
                            this.setState(CKEDITOR.TRISTATE_ON);
                        };
                    },
                    afterCommandExec: function (evt) {
                        if (evt.data.name == 'indent' || evt.data.name == 'outdent') {
                            var selection = this.getSelection();
                            var ranges = selection.getRanges();
                            ranges.forEach(function (range) {
                                var startContainer = range.startContainer.getAscendant('p', true);
                                if (startContainer) {
                                    var style = startContainer.getAttribute('style');
                                    if (style && style.includes('text-indent')) {
                                        style = style.replace(/text-indent:\s*[^;]+;?/gi, '');
                                        if (style.trim() === '') {
                                            startContainer.removeAttribute('style');
                                        } else {
                                            startContainer.setAttribute('style', style);
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
            })


        })
    }

    const editors3 = $('.ckeditor-input-3')
    if (editors3.length > 0) {
    }


    editors3.each((index, item) => {
        var input = $(item), config = input.data('config') || 'basic', mode = input.data('mode') || ''
        maxlength = input.attr('maxlength'), backgroundColor = input.data('bg-color') || '#fff'
        height = input.data('height') || '200px'

        CKEDITOR.replace(input.attr('id'), {
            entities: false,
            disableNativeSpellChecker: true,
            removePlugins: 'a11ychecker,toolbar',
            versionCheck: false,
            scayt_autoStartup: false,
            toolbar: [],
            maxLength: maxlength,
            height: height,
            on: {
                instanceReady: function (evt) {
                    this.editable().setStyle('background-color', backgroundColor);
                    var editor = evt.editor;

                    var indentCommand = editor.getCommand('outdent');
                    indentCommand.refresh = function (editor, path) {
                        this.setState(CKEDITOR.TRISTATE_ON);
                    };
                },
                afterCommandExec: function (evt) {
                    if (evt.data.name == 'indent' || evt.data.name == 'outdent') {
                        var selection = this.getSelection();
                        var ranges = selection.getRanges();
                        ranges.forEach(function (range) {
                            var startContainer = range.startContainer.getAscendant('p', true);
                            if (startContainer) {
                                var style = startContainer.getAttribute('style');
                                if (style && style.includes('text-indent')) {
                                    style = style.replace(/text-indent:\s*[^;]+;?/gi, '');
                                    if (style.trim() === '') {
                                        startContainer.removeAttribute('style');
                                    } else {
                                        startContainer.setAttribute('style', style);
                                    }
                                }
                            }
                        });
                    }
                }
            }
        })

    })
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
var stickyHeaderTableModalInit = (element) => {
    var self = element || modalPopup;
    if (self) {
        var offsetY = self.find('.modal-header').outerHeight();
        if ($('#sticky-table-modal').length) {
            var stickyHeadermodalOffsetY = self.offset().top + offsetY + window.scrollY;

            $('#sticky-table-modal').bootstrapTable({
                stickyHeader: true,
                stickyHeaderOffsetY: stickyHeaderOffsetY
            })
        }
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
    modalData2Popup = $('#modalData2Popup'),
    modalData2Dialog = modalData2Popup.find('.modal-dialog').first(),
    modalData2PopupTitle = modalData2Popup.find('.modal-title').first(),
    modalData2PopupContent = modalData2Popup.find('.modal-content').first(),
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

removeLoading = form => {
    let timeout
    let element = $(form),
        buttonAction = element
    elementAction = form

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

onSuccessed = (form, xhr, resetHiddenField = false, isBindData = true, isReload = true, callback) => {
    try {
        var elementId = $(form).data('ajax-success-id');
        if (xhr.responseJSON.Succeeded) {
            const modal = $(form).closest('.modal'), modalId = modal.attr('id'),
                refresh = xhr.responseJSON.RefreshModal,
                autoClose = xhr.responseJSON.AutoClose;

            if ($('input[type="checkbox"][name*="SubmitReload"]:checked').length) {
                toastMessage(true, "Thêm mới thành công");
                setTimeout(() => {
                    window.location.href = window.location.origin + window.location.pathname;
                }, 3000);
            }
            if ($('input[type="checkbox"][name*="AddMoreData"]:checked').length) {
                resetForm(form)
            } else if (!xhr.responseJSON.ReturnUrl) {
                if (autoClose && !refresh) {
                    modalPopup.modal('hide');
                    modal.modal('hide');
                }
            }

            if (isBindData) {
                if (elementId) {
                    var element = $('#' + elementId);
                    if (element) {
                        bindData(xhr.responseJSON.Id, element)
                    }
                    else {
                        bindData(xhr.responseJSON.Id)
                    }
                }
                else {
                    bindData(xhr.responseJSON.Id)
                }

            }
            else if (isReload) {
                window.location.reload()
            }

            if (xhr.responseJSON.ReturnUrl) {
                window.open(xhr.responseJSON.ReturnUrl)
            }

            if (xhr.responseJSON.RefreshModal) {
                refreshModal(modalId)
            }

            if (typeof callback === 'function') {
                callback();
            }
        }
        if (xhr.responseJSON.Messages && xhr.responseJSON != null && xhr.responseJSON.Data != null) {
            showValidationErrors(xhr.responseJSON.Data);
        }
        else if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages.join('<br />'))
        }

        elementAction = form

        removeLoading(form)

        resetTableActions()

        //initCkEditor()


        if (resetHiddenField) {
            resetHiddenInput(form)
        }
    } catch (e) {
        console.log(e)
    }
}
onDownloadFileSuccessed = (form, xhr, resetHiddenField = false) => {
    try {

        let result = xhr.responseJSON || xhr.responseText;
        if (typeof result === "string") result = JSON.parse(result);

        if (result.Succeeded) {
            window.location.href = result.Data?.FilePathDomain;
            // window.open(result.Data?.FilePathDomain, '_blank');
        } else {
            toastMessage(result.Succeeded, result.Messages.join('<br />'))
        }
        removeLoading(form)

    } catch (e) {
        console.log(e)
    }
}

onDocIndexesSuccessed = (form, xhr) => {
    try {

        if (xhr.responseJSON.succeeded) {

            toastMessage(true, "Cập nhật mục lục cho văn bản thành công");
            setTimeout(() => {
                window.location.reload()
            }, 3000);

        } else {
            toastMessage(false, "Cập nhật mục lục cho văn bản không thành công");
        }
        removeLoading(form)

    } catch (e) {
        console.log(e)
    }
}
function showValidationErrors(errors) {
    $(".field-validation-error").text("");

    $.each(errors, function (key, messages) {
        var errorElement = $('[data-valmsg-for="Command.' + messages.PropertyName + '"]');
        if (errorElement.length > 0) {
            errorElement.append(`<span id=\"Command_${messages.PropertyName}-error\">${messages.ErrorMessage}</span>`);
            errorElement.addClass("field-validation-error");
        }
    });
}

onSuccessedDataPopup = (form, xhr, resetHiddenField = false, isBindData = true, isResetForm = false) => {
    try {
        var url = $(form).data('ajax-success-url');
        var elementId = $(form).data('ajax-success-id');
        var elementIdList = $(form).data('ajax-success-idlist');
        var elementHidden = $(form).data('hidden');
        if (xhr.responseJSON.Succeeded) {

            if ($(form).find('input[type="checkbox"][name*="AddMoreData"]:checked').length || isResetForm) {
                resetForm(form);
                //$(form).find('input[name*="IsAddOtherSource"]').prop('checked', true);
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
        console.log(xhr.responseJSON.Messages)
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
        stickyHeaderTableInit()
        stickyHeaderTableModalInit(modalDataPopup)

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
                modalData2Popup.modal('hide')
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
                // bindData(xhr.responseJSON.Id);
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

onBindDataSelectSuccessed = (element, xhr, isRelate = false) => {
    try {
        //console.log(xhr)
        var resetElement = $(element).data('ajax-reset') || false;
        removeLoading(element)
        resetTableActions()
        if (resetElement) {
            resetForm($(element))
        }
        if (xhr.responseText) {

            const response = $(xhr.responseText)

            const isEmpty = response.filter('table').
                find('.not-found').first().length > 0

            if (isRelate) {
                if (response.filter('tr.not-found').first().length > 0)
                    toastMessage(false, "Không tìm thấy văn bản phù hợp");
            }
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
        //dành riêng cho docRelate
        if (isRelate) {
            $("#tblDocSearch .not-found").remove();
            var tableRows = $('#tblDocSearch tr');
            if (tableRows.length > 0) {
                tableRows.each(function (index) {
                    console.log(index)
                    $(this).find('.row-number').text(index + 1);
                })
            }
            console.log(tableRows);
        }
        if (xhr.Messages) {
            toastMessage(xhr.Succeeded, xhr.Messages.join('<br />'))
        }
        stickyHeaderTableInit()
        stickyHeaderTableModalInit(modalDataPopup)


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
onUploadFileSuccessed = (element, xhr) => {
    try {
        removeLoading(element);
        const btnUpFile = $('.btn-up-file');
        if (btnUpFile.length) {
            removeLoading(btnUpFile);
        }
        var btnUpFileTextbox = $('.btn-up-file-to-textbox');
        if (btnUpFileTextbox.length) {
            removeLoading(btnUpFileTextbox);
        }
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
        //refresh
        var dataType, fileTypeId, dataId, elementResultId;
        if ($('#fileUploadCreateCommand_DataType').length) {
            dataType = $('#fileUploadCreateCommand_DataType').val();
        }
        if ($('#fileUploadCreateCommand_FileTypeId').length) {
            fileTypeId = $('#fileUploadCreateCommand_FileTypeId').val();
        }
        if ($('#fileUploadCreateCommand_DataId').length) {
            dataId = $('#fileUploadCreateCommand_DataId').val();
        }
        if ($('#fileUploadCreateCommand_ElementResultId').length) {
            elementResultId = $('#fileUploadCreateCommand_ElementResultId').val();
        }
        if (dataType != undefined && fileTypeId != undefined && dataId != undefined) {
            //Nếu là box uploadfile
            removeLoading(element);
            if (elementResultId != undefined && elementResultId.length && $('#' + elementResultId).length) {
                var filePath = xhr.responseJSON.Data[0].FilePath;
                $('#' + elementResultId).val(filePath);
                if ($('#' + elementResultId + '_Preview').length) {
                    $('#' + elementResultId + '_Preview').attr('src', filePath);
                }
            }
            else //bảng danh sách file
            {
                var idTable = 'table_' + dataType + "_" + dataId;
                var table = $('#' + idTable);
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
        }

    } catch (e) {
        console.log(e)
    }
}
onUploadDocumentSuccessed = (element, xhr) => {
    try {
        removeLoading(element);
        const btnUpFile = $('.btn-up-document');
        if (btnUpFile.length) {
            removeLoading(btnUpFile);
        }
        var btnUpFileTextbox = $('.btn-up-file-to-textbox');
        if (btnUpFileTextbox.length) {
            removeLoading(btnUpFileTextbox);
        }
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
        //refresh
        var DocumentTypeId, LawerId, FirmId, elementResultId;
        if ($('#lawDocumentCreateCommand_DocumentTypeId').length) {
            DocumentTypeId = $('#lawDocumentCreateCommand_DocumentTypeId').val();
        }
        if ($('#lawDocumentCreateCommand_LawerId').length) {
            LawerId = $('#lawDocumentCreateCommand_LawerId').val();
        }
        if ($('#lawDocumentCreateCommand_FirmId').length) {
            FirmId = $('#lawDocumentCreateCommand_FirmId').val();
        }
        if ($('#lawDocumentCreateCommand_ElementResultId').length) {
            elementResultId = $('#lawDocumentCreateCommand_ElementResultId').val();
        }
        if (LawerId != undefined || FirmId != undefined) {
            //Nếu là box uploadfile
            removeLoading(element);
            if (elementResultId != undefined && elementResultId.length && $('#' + elementResultId).length) {
                var filePath = xhr.responseJSON.Data[0].FilePath;
                $('#' + elementResultId).val(filePath);
                if ($('#' + elementResultId + '_Preview').length) {
                    $('#' + elementResultId + '_Preview').attr('src', filePath);
                }
            }
            else //bảng danh sách file
            {
                if (FirmId) {
                    var idTable = 'table_' + FirmId;
                    var table = $('#' + idTable);
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
                } else {
                    var idTable = 'table_' + LawerId;
                    var table = $('#' + idTable);
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

            }
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

flatpickrInit = (element) => {
    element = element || '.datepicker';
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
    element = element || '.datepicker-time';

    $(element).each((index, item) => {
        const format = $(item).data('format') || 'd/m/Y H:i',
            enableSeconds = $(item).data('enable-seconds') || false,
            nocalendar = $(item).data('nocalendar') || false,
            time24h = $(item).data('time-24h') || false;
        if (typeof $.fn.flatpickr !== 'undefined') {
            $(item).flatpickr({
                dateFormat: format,
                enableTime: true,
                enableSeconds: enableSeconds,
                allowInput: true,
                disableMobile: true,
                time_24hr: time24h,
                noCalendar: nocalendar,
                'locale': 'vn'
            });
        }
    })
}
sortableBlock = element => {
    var arraySortable = new Array()
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
                arraySortable = []

                $.map($(this).find('.sortable-item'), function (el) {
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
        bindDataParamsElement = $('.bulk-select-replace-element')

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
            stickyHeaderTableInit()
            stickyHeaderTableModalInit()

            if (id.length > 0) {
                hightLightRow(id, element)
            }

            initTooltip()
            initCkEditorInline()
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
modalGet = (element, path, title, callback) => {
    modalSetSize(element)
    let timeout
    elementAction = element
    const elementHtml = $(element).html()
    const autoClose = $(element).data('auto-close') !== false

    modalPopup.attr('data-auto-close', autoClose)

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

            if (modalPopupContent.find('[data-bs-content]').length > 0) {
                docReady(popoverInit)
            }

            const jsonViewElement = modalPopupContent.find('.json-view').first()

            if (jsonViewElement.length > 0) {
                modalPopup.find('.modal-body').first().jsonView(jsonViewElement.text())
            }

            docReady(tooltipInit)

            initViewMaxLength()

            initCkEditor()

            initSelect2AutoComplete($('.select2-auto-complete-modal'))

            initAutocomplete()

            initTooltip()
            
            initTooltipDocProperties()

            initTooltipDocProperties2()

            select2Init()
            if (modalPopupContent.find('.smart-select').length > 0) {
                modalPopupContent.find('.smart-select').each(function () {
                    const container = $(this);
                    if (container) {
                        initSelectBox(container);
                    }
                });
            }

            stickyHeaderTableModalInit(modalPopup)

            $('#tooltip-doc-props').hide()

            modalPopup.modal('show')

            if ($("#Command_LawJudgTypeId")) {
                var lawJudgType = $("#Command_LawJudgTypeId").val()
                if (parseInt(lawJudgType) == 3) {
                    $(".is-ban-an").hide()
                    $(".is-an-le").show()
                }
                else {
                    $(".is-ban-an").show()
                    $(".is-an-le").hide()
                    if (parseInt(lawJudgType) == 1) {

                        $(".is-quyet-dinh").hide()
                    }
                    else {
                        $(".is-quyet-dinh").show()
                    }

                }
            }

            if (callback && typeof callback === 'function') {
                callback()
            }

        }), 1000
    )

}

modalLoad = (element) => {
    const self = $(element),
        { title, className } = element,
        { modalSize, path, callbacks, refresh, closeCurrent } = element.dataset,
        modalId = (Math.random() + 1).toString(36).substring(2);

    let timeout
    elementAction = element
    const elementHtml = $(element).html(), parent = self.closest('.modal'), parentId = parent.attr('id');
    if (closeCurrent === 'true' && parent.length > 0) {
        // 🔒 Gắn 1 flag để đảm bảo modalLoad sẽ được gọi lại sau khi đóng
        element.dataset.closeCurrent = 'false'; // Đặt lại để không lặp vô hạn

        // 👉 Đóng modal hiện tại
        parent.on('hidden.bs.modal', function () {
            $(this).remove();
            modalLoad(element);
        });

        parent.modal('hide');
        return;
    }

    $("body").append(`<div id="modalDataPopup_${modalId}" class="modal fade" data-bs-focus="false" data-bs-backdrop="static" data-keyboard="false" tabindex="-3" style="z-index:1051" ${(refresh ? `data-path="${path}"` : '')} ${(refresh && parentId ? `data-pid="${parentId}"` : '')} ${(callbacks ? `data-callbacks='${callbacks}'` : '')}>
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

    let modalDataLoad = $(`#modalDataPopup_${modalId}`)

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
            let modalDataLoadContent = modalDataLoad.find('.modal-content').first(),
                modalDataLoadTitle = modalDataLoad.find('.modal-title').first()

            modalDataLoadContent.html(response)

            modalDataLoadTitle.text(title)

            if (!modalDataLoadContent.hasClass('fs-0')) {
                modalDataLoadContent.addClass('fs-0')
            }

            $.validator.unobtrusive.parse(modalDataLoad.find('form').first())

            if (modalDataLoadContent.find('.scrollbar-overlay').length > 0) {
                docReady(scrollbarInit)
            }

            if (modalDataLoadContent.find('[data-bs-content]').length > 0) {
                docReady(popoverInit)
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
            initSelect2AutoComplete($('select2-auto-complete-modal'))
            initViewMaxLength()

            if (modalDataLoadContent.find('.select-picker-modal').length > 0) {
                docReady(select2Init(modalDataLoad))
            }

            modalDataLoad.modal('show')
        }), 1000
    )

    modalDataLoad.on('hidden.bs.modal', function () {
        if (currentAbortController) {
            currentAbortController.abort();
            currentAbortController = null;
            console.log('⛔ Stream bị hủy do đóng modal.');
        }
        $(this).remove()
    })
}

refreshModal = (modalId) => {
    const element = document.getElementById(modalId),
        modalDataPopup = $(element),
        { title, path, callbacks, refresh } = element.dataset
    if (path) {
        debounce(
            fetchData({
                url: path,
                type: 'GET',
                dataType: 'html'
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

                if (modalDataPopupContent.find('[data-bs-content]').length > 0) {
                    docReady(popoverInit)
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

                initCkEditor()
            }), 1000
        )

        modalDataPopup.on('hidden.bs.modal', function () {
            $(this).remove()
        })
    }
}

updateDocSummaryLayout = (docId, languageId) => {
    const docContentCol = $('#doc-content-col');
    const toggleLink = $('#toggle-doc-content');
    //const contentCols = $('.content-col');
    const rowParent = toggleLink.parent().closest('.row');
    const contentCols = rowParent.find('.content-col');
    const newsContentCol = $('#news-root-content-col');

    const isVisible = docContentCol.hasClass('d-none');

    const isDocStreamLoaded = docContentCol.data('doc-loaded') === true;

    if (isVisible && !isDocStreamLoaded) {
        loadDocStream(docId, languageId);
        docContentCol.data('doc-loaded', true);
    }

    if (isVisible) {
        toggleLink.text('Ẩn nội dung văn bản');
        docContentCol.removeClass('d-none').addClass('col-md-4');
        contentCols.removeClass('col-md-6').addClass('col-md-4');
    } else {
        toggleLink.text('Hiển thị nội dung văn bản');
        docContentCol.addClass('d-none').removeClass('col-md-4');

        if (newsContentCol.length && newsContentCol.hasClass('d-none')) {
            contentCols.removeClass('col-md-4').addClass('col-md-6');
        }
    }
}

let blinkInterval = null;
let dotAnimationInterval = null;

const setDocStreamStatus = (status, type = 'info', autoClear = false, autoClearMs = 3000) => {
    const label = document.querySelector('.doc-stream-label');
    if (!label) return;

    if (blinkInterval) {
        clearInterval(blinkInterval);
        blinkInterval = null;
        label.classList.remove('blink');
    }

    if (dotAnimationInterval) {
        clearInterval(dotAnimationInterval);
        dotAnimationInterval = null;
    }

    const icons = {
        info: '⏳',
        inprocess: '🔄',
        success: '✅',
        error: '🚫',
        warning: '⚠️',
    };

    const colors = {
        info: 'text-muted',
        inprocess: 'text-primary',
        success: 'text-success',
        error: 'text-danger',
        warning: 'text-warning',
    };

    label.classList.remove('fade-out');

    label.className = 'doc-stream-label small';
    label.classList.add(colors[type]);

    const baseText = `${icons[type]} ${status}`;

    if (type === 'inprocess') {
        label.innerHTML = `${baseText}<span class="dot-loader"></span>`;
        const dotSpan = label.querySelector('.dot-loader');

        let dotCount = 0;
        const dotStates = ['', '.', '..', '...'];

        dotAnimationInterval = setInterval(() => {
            dotSpan.textContent = dotStates[dotCount];
            dotCount = (dotCount + 1) % dotStates.length;
        }, 500);
    } else {
        label.innerText = baseText;
    }

    if (autoClear) {

        blinkInterval = setInterval(() => {
            label.classList.toggle('blink');
        }, 500);

        setTimeout(() => {
            clearInterval(blinkInterval);
            blinkInterval = null;
            label.classList.remove('blink');
            label.innerText = '';
        }, autoClearMs);
    }
}

loadDocStream = async (docId, languageId) => {
    const container = document.getElementById('doc-stream-container');

    try {
        // Nếu có một yêu cầu trước đó, hủy nó
        if (currentAbortController) {
            currentAbortController.abort();
        }

        // Tạo mới AbortController
        currentAbortController = new AbortController();
        const signal = currentAbortController.signal;

        if (container) container.innerHTML = '';

        setDocStreamStatus('Đang stream nội dung văn bản', 'inprocess');

        // Gọi API để stream dữ liệu
        const response = await fetch(`/LuatVietNamDoc/Docs?handler=StreamDocument&docId=${docId}&languageId=${languageId}`, { signal });

        if (!response.ok || !response.body) {

            setDocStreamStatus('Không thể tải dữ liệu stream từ máy chủ.', 'error');

            return;
        }

        const reader = response.body.getReader();
        const decoder = new TextDecoder('utf-8');

        while (true) {
            const { value, done } = await reader.read();
            if (done) {
                // Decode phần còn lại nếu có
                const chunkLeft = decoder.decode();
                if (chunkLeft) {
                    container.insertAdjacentHTML('beforeend', chunkLeft);
                }
                break;
            }

            const chunk = decoder.decode(value, { stream: true });

            container.insertAdjacentHTML('beforeend', chunk);
        }

        setDocStreamStatus('Tải xong nội dung văn bản', 'success', 5000);

        console.log('✅ Stream nội dung văn bản đã hoàn tất!');

    } catch (error) {

        if (error.name === 'AbortError') {
            setDocStreamStatus('Đã hủy stream', 'warning');
        } else {
            setDocStreamStatus('Lỗi khi stream nội dung ăn bản', 'error');
            console.error('❌ Lỗi khi stream nội dung ăn bản:', error);
        }
    }
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
modalChildGet = (element, path, title) => {
    modalSetSize(element, $('#modalDataPopup'))
    let timeout
    elementAction = element
    const elementHtml = $(element).html();


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
                stickyHeaderTableModalInit(modalDataPopup)
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
            stickyHeaderTableModalInit(modalDataPopup)
            docReady(tooltipInit);

            initViewMaxLength()

            initCkEditor()

            initSelect2AutoComplete($('.select3-auto-complete-modal'))

            initAutocomplete()

            initTooltip()

            modalDataPopup.modal('show')
        })
    )
}
modalChild2Get = (element, path, title) => {
    modalSetSize(element, $('#modalData2Popup'))
    let timeout
    elementAction = element
    const elementHtml = $(element).html();


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
            modalData2PopupContent.html(response)

            const isEmpty = $(response).filter('table').
                find('.not-found').first().length > 0

            if (!isEmpty) {
                stickyHeaderTableModalInit(modalData2Popup)
            }

            modalData2Popup.find('.modal-title').first().text(title)

            $.validator.unobtrusive.parse(modalData2Popup.find('form').first())


            if (!modalData2PopupContent.hasClass('fs-0')) {
                modalData2PopupContent.addClass('fs-0')
            }
            if (modalData2PopupContent.find('input.datepicker').length > 0) {
                flatpickrInit()
            }

            if (modalData2PopupContent.find('input.datepicker-time').length > 0) {
                flatpickrTimeInit()
            }
            if (modalData2PopupContent.find('input.datepicker-time2').length > 0) {
                flatpickrTime2Init()
            }
            if (modalData2PopupContent.find('.select-picker-modal2').length > 0) {
                docReady(select3Init($('#modalData2Popup')))
            }
            if (modalData2PopupContent.find('.scrollbar-overlay').length > 0) {
                docReady(scrollbarInit)
            }

            const jsonViewElement = modalData2PopupContent.find('.json-view').first()

            if (jsonViewElement.length > 0) {
                modalData2Popup.find('.modal-body').first().jsonView(jsonViewElement.text())
            }

            docReady(tooltipInit);

            initViewMaxLength()
            if ($('.ckeditor-input-modal').length > 0) {
                initCkEditor($('.ckeditor-input-modal'))
            }
            if ($('.ckeditor-input').length > 0) {
                initCkEditor($('.ckeditor-input'))
            }
            initSelect2AutoComplete($('.select3-auto-complete-modal'))

            initAutocomplete()

            initTooltip()

            modalData2Popup.modal('show')
        })
    )
}
selectVOHVoice = (element) => {
    const profileSelected = $('#Command_HostVoices option:selected'),
        profileId = profileSelected.val() || 0
    //voicTypeSelected = $('#VOHVoiceType option:selected'),
    //voiceTypeId = voicTypeSelected.val() || 0

    //if (voiceTypeId <= 0) {

    //    showMessage({ message : 'Chưa chọn giọng đọc.' })

    //    return false
    //}

    if (profileId <= 0) {

        showMessage({ message: 'Vui lòng chọn biên tập viên.' })

        return false
    }

    return true
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

modalMediaGet = (element, key, mediaTypeId = 1, title = 'Chọn ảnh') => {
    modalSetSize(element)
    let timeout
    elementAction = element
    const elementHtml = $(element).html(),
        mediaTypeParam = `?Query.MediaTypeId=${mediaTypeId}`,
        path = `/LuatVietNamCms/Medias/Select${mediaTypeParam}`

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
                stickyHeaderTableModalInit(modalDataPopup)
                $('#sticky-table-modal').bootstrapTable('resetView');
            }

            modalDataPopup.find('.modal-title').first().text(title)

            if (!modalDataPopupContent.hasClass('fs-0')) {
                modalDataPopupContent.addClass('fs-0')
            }

            modalDataPopup.modal('show')
        })
    )
}

modalMediaEditorGet = (editor, element) => {
    let timeout
    let key, mediaTypeId = 1, title = 'Chọn ảnh'
    let elementId = $(`#${element.uiItems[0]._.id}`)
    elementAction = $(elementId)
    const elementHtml = $(elementId).html(),
        mediaTypeParam = `?Query.MediaTypeId=${mediaTypeId}`,
        path = `/ICAds/Medias/EditorSelect${mediaTypeParam}`

    modalDataPopup.removeAttr('data-key')
    modalDataPopup.removeAttr('data-type')

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(elementId)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(elementId).removeAttr('disabled')
                    $(elementId).html(elementHtml)
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
                stickyHeaderTableModalInit(modalDataPopup)
                $('#sticky-table-modal').bootstrapTable('resetView');
            }

            modalDataPopup.find('.modal-title').first().text(title)

            if (!modalDataPopupContent.hasClass('fs-0')) {
                modalDataPopupContent.addClass('fs-0')
            }

            modalDataPopup.modal('show')
        })
    )
}

modalNewsEditorGet = (editor, element, keywords = '', newsTypeId = 1, searchByDate = 1, title = "Chọn bài viết") => {
    let timeout
    let elementId = $(`#${element.uiItems[0]._.id}`)
    elementAction = $(elementId)
    const elementHtml = $(elementId).html(),
        newsTypeParam = `?Query.NewType=${newsTypeId}&Query.Keywords=${keywords}&Query.SearchByDate=${searchByDate}`,
        path = `/News/News/EditorSelect${newsTypeParam}`

    modalDataPopup.removeAttr('data-key')
    modalDataPopup.removeAttr('data-type')

    debounce(
        fetchData({
            url: path,
            type: 'GET',
            dataType: 'html',
            beforeSend: () => {
                onBegin(elementId)
            },
            callback: () => {
                if (timeout) {
                    clearTimeout(timeout)
                }

                timeout = setTimeout(() => {
                    $(elementId).removeAttr('disabled')
                    $(elementId).html(elementHtml)
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

                initTooltip(modalDataPopupContent)
            }

            var selectModal = modalDataPopupContent.find('.select-editor-modal')

            selectModal.length && selectModal.each(function (index, value) {
                var $this = $(value);
                var options = $.extend({
                    dropdownParent: $('#modalDataPopup'),
                }, $this.data('options'))
                $this.select2(options)
            })

            if (modalDataPopupContent.find('input.datepicker').length > 0) {
                flatpickrInit()
            }

            modalDataPopup.find('.modal-title').first().text(title)

            if (!modalDataPopupContent.hasClass('fs-0')) {
                modalDataPopupContent.addClass('fs-0')
            }


            modalDataPopup.modal('show')
        })
    )
}
modalNewsGet = (element, key, keywords = '', newsTypeId = 1, title = "Chọn văn bản", searchByDate = 1) => {
    element.attr('data-modal', 'modal-xl')
    modalSetSize(element, $('#modalDataChildPopup'))
    let timeout
    elementAction = element
    const elementHtml = $(element).html(),
        path = `/LuatVietNamCMS/Articles/SelectArticles`

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
modalDocsGet = (element, key, keywords = '', newsTypeId = 1, title = "Chọn văn bản", searchByDate = 1) => {
    element.attr('data-modal', 'modal-xl')
    modalSetSize(element, $('#modalDataChildPopup'))
    let timeout
    elementAction = element
    const elementHtml = $(element).html(),
        path = `/LuatVietNamDoc/Docs/SelectDocs`

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

selectChildData = (element, path, title = '') => {
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

selectMedia = async (element, mediaType = 1, mediaPath, duration = null, fileSize = 0, mediaName = '') => {
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
        }
        else {
            const selectMediaInput = $(`#SelectMediaInput${mediaKey}`)

            if (selectMediaInput.length > 0) {
                selectMediaInput.val(mediaPath)
                selectMediaInput.trigger('change')
                selectMediaInput.trigger('blur')
            }
        }
    } else if (mediaTypeName == 'audio') {

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
}

selectData = (element, path, title = '') => {
    if (typeof path == 'undefined' || path.trim().length <= 0) {
        return;
    }

    modalDataPopup.modal('hide')

    let dataKey = modalDataPopup.attr('data-key') || ''

    if (dataKey.trim().length > 0) {

        if (dataKey.includes('key_cke_')) {
            $(`.${dataKey}`).find('input').val(path)
            $($(`.${dataKey}`).find('input')[0]).focus()
            $(`.${dataKey}.alternative-information`).find('input').val(title)
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

initDocReviewStatusCount = () => {

    var container = $('#docReviewStatusCount')
    var url = container.data('url') || '/LuatVietNamDoc/Docs?handler=DocReviewStatusCount'
    if (!container.length) return

    // Lấy các tham số từ URL
    var params = Object.fromEntries(new URLSearchParams(window.location.search))

    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'html',
        data: params
    })
        .done(function (response) {
            container.find('.placeholder').fadeOut(300).promise().done(function () {
                container.empty().html(response).css('opacity', '0').animate({ opacity: 1 }, 300)
            })
        })
        .fail(function (error) {
            console.error(error)
        })
}

initDocReviewReviewStatusCount = () => {
    var container = $('#docReview_ReviewStatusCount')
    var url = container.data('url') || '/LuatVietNamDoc/Docs/DocsReview?handler=DocReviewStatusCount'
    if (!container.length) return

    // Lấy các tham số từ URL
    var params = Object.fromEntries(new URLSearchParams(window.location.search))

    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'html',
        data: params
    })
        .done(function (response) {
            container.find('.placeholder').fadeOut(300).promise().done(function () {
                container.empty().html(response).css('opacity', '0').animate({ opacity: 1 }, 300)
            })
        })
        .fail(function (error) {
            console.error(error)
        })
}

initDocStandardReviewStatusCount = () => {

    var container = $('#docStandardStatusCount')

    if (!container.length) return

    // Lấy các tham số từ URL
    var params = Object.fromEntries(new URLSearchParams(window.location.search))

    $.ajax({
        url: '/LuatVietNamDoc/DocStandardFulls?handler=DocReviewStatusCount',
        type: 'GET',
        dataType: 'html',
        data: params
    })
        .done(function (response) {
            container.find('.placeholder').fadeOut(300).promise().done(function () {
                container.empty().html(response).css('opacity', '0').animate({ opacity: 1 }, 300)
            })
        })
        .fail(function (error) {
            console.error(error)
        })
}

initTooltipDocProperties = (tooltip = $('#tooltip-doc-props'),
    hoverSelector = '.doc-property-item') => {
    const cache = {};
    let currentRequest = null;

    const adjustTooltipPosition = (x, y) => {
        const offset = 10;
        const tooltipWidth = tooltip.outerWidth();
        const tooltipHeight = tooltip.outerHeight();
        const windowWidth = $(window).width();
        const windowHeight = $(window).height();

        // Điều chỉnh vị trí ngang
        if (x + tooltipWidth + offset > windowWidth) {
            x = windowWidth - tooltipWidth * 2 - offset;
        } else {
            x += offset;
        }

        // Điều chỉnh vị trí dọc
        if (y + tooltipHeight + offset > windowHeight - 10) {
            y = y - tooltipHeight - offset;
        } else {
            y += offset;
        }

        tooltip.css({ left: x, top: y });
    };

    $(document)
        .on('mouseenter', hoverSelector, function (e) {
            const element = $(this);
            const docId = element.data('id');
            const languageId = element.data('lang');
            console.log(1234)
            tooltip.text('Đang tải dữ liệu...').show();
            adjustTooltipPosition(e.pageX, e.pageY);

            // Kiểm tra cache
            if (cache[docId]) {
                tooltip.html(cache[docId]);
                return;
            }

            // Hủy request đang chờ nếu có
            if (currentRequest) {
                currentRequest.abort();
            }

            currentRequest = $.ajax({
                url: '/LuatVietNamDoc/Docs?handler=GetTooltipDocProperties',
                data: { docId, languageId },
                method: 'GET',
                success: function (content) {
                    cache[docId] = content;
                    tooltip.html(content);
                    adjustTooltipPosition(e.pageX, e.pageY);
                },
                error: function () {
                    tooltip.html('').fadeOut(2000);
                }
            });
        })
        .on('mouseleave', hoverSelector, function () {
            tooltip.stop(true, true).hide();
            if (currentRequest) {
                currentRequest.abort();
            }
        });

    $(document).mousemove(function (e) {
        if (tooltip.is(':visible')) {
            adjustTooltipPosition(e.pageX, e.pageY);
        }
    });
}

initTooltipDocProperties2 = (tooltip = $('#tooltip-doc-props'),
    hoverSelector = '.doc-property-item2') => {
    const cache = {};
    let currentRequest = null;

    const adjustTooltipPosition = (x, y) => {
        const offset = 10;
        const tooltipWidth = tooltip.outerWidth();
        const tooltipHeight = tooltip.outerHeight();
        const windowWidth = $(window).width();
        const windowHeight = $(window).height();

        // Điều chỉnh vị trí ngang
        if (x + tooltipWidth + offset > windowWidth) {
            x = windowWidth - tooltipWidth * 2 - offset;
        } else {
            x += offset;
        }

        // Điều chỉnh vị trí dọc
        if (y + tooltipHeight + offset > windowHeight - 10) {
            y = y - tooltipHeight - offset;
        } else {
            y += offset;
        }

        tooltip.css({ left: x, top: y });
    };

    $(document)
        .on('mouseenter', hoverSelector, function (e) {
            const element = $(this);
            const docId = element.data('id');
            const languageId = element.data('lang');

            tooltip.text('Đang tải dữ liệu...').show();
            adjustTooltipPosition(e.pageX, e.pageY);

            // Kiểm tra cache
            if (cache[docId]) {
                tooltip.html(cache[docId]);
                return;
            }

            // Hủy request đang chờ nếu có
            if (currentRequest) {
                currentRequest.abort();
            }

            currentRequest = $.ajax({
                url: '/LuatVietNamDoc/DocsConsolidation?handler=GetTooltipDocProperties',
                data: { docId, languageId },
                method: 'GET',
                success: function (content) {
                    cache[docId] = content;
                    tooltip.html(content);
                    adjustTooltipPosition(e.pageX, e.pageY);
                },
                error: function () {
                    tooltip.html('').fadeOut(2000);
                }
            });
        })
        .on('mouseleave', hoverSelector, function () {
            tooltip.stop(true, true).hide();
            if (currentRequest) {
                currentRequest.abort();
            }
        });

    $(document).mousemove(function (e) {
        if (tooltip.is(':visible')) {
            adjustTooltipPosition(e.pageX, e.pageY);
        }
    });
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
const select2CacheStore = {};
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
                                keywords: params.term
                            }

                            return query;
                        },
                        // Dùng transport để kiểm tra cache theo key (URL + từ khóa 'default' nếu rỗng)
                        transport: function (params, success, failure) {

                            const term = params.data.keywords ? params.data.keywords.trim() : '';
                            const key = urlRequest + '_' + (term === "" ? 'default' : term);

                            // Nếu term rỗng và có cache thì sử dụng cache
                            if (term === "" && select2CacheStore[key]) {
                                success(select2CacheStore[key]);
                                return;
                            }
                            // Gọi ajax bình thường
                            var $request = $.ajax(params);
                            $request.then(function (data) {
                                // Nếu term rỗng thì lưu vào cache
                                if (term === "") {
                                    select2CacheStore[key] = data;
                                }
                                success(data);
                            }, failure);
                            return $request;
                        },
                        processResults: function (response) {

                            if (isSearch && !response.Data.some(item => item.id === '0')) {
                                response.Data.unshift({
                                    id: '0',
                                    text: '...'
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

            if (urlRequest.trim().length == 0 || dropdownParent.length > 0 && selected.length > 0) {

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
AddNewOnSelect = (element) => {
    var ul = $(element).closest("ul");
    var parentDiv = $(element).closest("div.modal");
    var idParentDiv = '';
    if (parentDiv.length) {
        idParentDiv = $(parentDiv).attr('id');
        if (idParentDiv.length) {
            if (ul.length) {
                var ulid = $(ul).attr('id');
                if (ulid != undefined) {
                    ulid = ulid.replaceAll("select2-", "").replaceAll("-results", "");
                    var select = $('#' + ulid);
                    if (select.length) {
                        var dataUrl = $(select).attr('data-url-add');
                        var dataTitle = $(select).attr('data-title-add');
                        if (dataUrl.length) {
                            var $button = $('<button/>', {
                                type: 'button',
                                'class': 'btn btn-primary no-focus ms-2',
                                'data-modal': 'modal-lg',
                                text: 'Thêm'
                            });
                            modalGet(null, dataUrl, "");
                            //window.open(dataUrl+'?layout=_LayoutBlank', "popupWindow", "width=600,height=800,scrollbars=yes");
                            //modalChild3Get($button, dataUrl, dataTitle);
                        }
                    }
                }
            }
        }
    }
}
exportFile = (element, handlerName = 'Export') => {
    console.log(window.location.href)
    let url = new URL(window.location.href),
        searchParams = url.searchParams

    searchParams.set('handler', handlerName)
    buttonAction = $(element)
    url.search = searchParams.toString()
    let urlRequest = url.toString()

    buttonAction.attr('disabled', 'disabled')
    buttonAction.data('og', buttonAction.html())
    if (buttonAction.hasClass('cke_element')) {
        buttonAction.html('<span style="padding: 0 5px;">Vui lòng đợi...</span>')
    } else if (buttonAction.hasClass('cke_button')) {
        buttonAction.html('<span style="padding: 0 2px;">...</span>')
    } else {
        buttonAction.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>')
    }
    window.location.href = urlRequest
    setTimeout(removeLoading(element), 5000)

}
const popupCenter = ({ url, title, w, h }) => {
    const dualScreenLeft = window.screenLeft !== undefined ? window.screenLeft : window.screenX;
    const dualScreenTop = window.screenTop !== undefined ? window.screenTop : window.screenY;

    const width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
    const height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

    const systemZoom = width / window.screen.availWidth;
    const left = (width - w) / 2 / systemZoom + dualScreenLeft
    const top = (height - h) / 2 / systemZoom + dualScreenTop
    const newWindow = window.open(url, title,
        `
      scrollbars=yes,
      width=${w / systemZoom}, 
      height=${h / systemZoom}, 
      top=${top}, 
      left=${left}
      `
    )

    if (window.focus) newWindow.focus();
}
removeItemAutocomplete = (element, id) => {
    const self = $(element), parent = self.closest('.row')

    if (parent) {
        let buttons = {
            'confirm': {
                text: "Đồng ý",
                btnClass: 'btn btn-danger',
                keys: ['enter'],
                action: function () {
                    parent.remove()
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

    return flase
}

initAutocomplete = () => {
    const element = $('.input-auto-complete')

    if (element.length > 0) {

        $(document).on('click', (event) => {
            console.log(1)
            if (!$(event.target).closest('.did-floating-label-content').length) {
                $('.autocomplete-result').addClass('d-none')
            }
        })

        element.on('click keyup', debounce((e) => {
            let self = $(e.target), urlRequest = self.data('url'),
                itemsLimit = self.data('items') || 3, inputName = self.data('name') || '',
                inputValue = self.val().trim(), parent = self.closest('.auto-complete'),
                iconContainer = self.next('.icon-container'),
                resultElement = parent.find('.autocomplete-result').first();


            // get keycode of current keypress event
            var code = (e.keyCode || e.which)

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
                        //<a href="${response.Data[index].slug}" target="_blank" title="${response.Data[index].title}">${response.Data[index].slug}</a>
                        htmlAppend = `<div data-id="${response.Data[index].id}" class="row g-3 align-items-center text-center text-md-start py-1 border-bottom border-200">
						    <div class="col px-x1 py-1">
							    <div class="row">
								    <div class="col-12">
									    <h6 class="fs-0">${response.Data[index].title}</h6>
								    </div>
							    </div>
						    </div>
                            <div class="col-md-auto d-flex justify-content-center">`
                        if (response.Data[index].isrelated) {
                            htmlAppend += `<a href="javascript:void(0)" class="btn btn-falcon-default icon-item" title="Bài viết đã được chọn làm tin liên quan"><i class="fas fa-check-circle text-success"></i> </a>`
                        } else {
                            htmlAppend += `<a href="javascript:void(0)" class="btn btn-falcon-default icon-item focus-bg-primary" onclick="return addItemAutocomplete(this, ${response.Data[index].id}, '${response.Data[index].title.replaceAll('"', '&quot;').replaceAll('\'', '&quot;')}', '${response.Data[index].slug}', '${inputName}', ${itemsLimit})" title="Thêm vào danh sách"><i class="fas fa-plus"></i> </a>`
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
                console.log(error)
                resultElement.addClass('d-none')
                showMessage({
                    message: 'Vui lòng thử lại sau.',
                })
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
        console.log(formData);
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

    return data.text ?? data.title;
}
function formatRepoSelection(data, container) {

    if (data.type) {
        $(container).addClass(`profile-selected-${data.type}`)
    }

    return data.text;
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
        .not(':button, :submit, :reset, :hidden, :input[name="Command.Id"], :input[name*="AddMoreData"], select, textarea,iframe')
        .val('')
        .prop('checked', false)

    $(form).find('img')
        .attr('src', '')
        .css('border', 'none')

    $(form).find('.profile-pic')
        .attr('src', '/img/avatar.png')
    $(form).find('select[class*=form-select]:not([class*="form-select-submit"])').val(0).trigger("change");
    $('textarea').val('')

    if (typeof CKEDITOR != 'undefined') {

        for (instance in CKEDITOR.instances) {

            CKEDITOR.instances[instance].setData(" ")

        }

    }
}

fetchData = options => {
    return new Promise(function (resolve, reject) {
        $.ajax(options).done(resolve).fail(reject).always(options.callback || {})
    })
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
    console.log(chkActionIds)

    $('.ChkActionIds').attr('value', chkActionIds)
}

function selectAllRow2() {
    var chkActionIds = '';
    $('.main-table>tbody>tr').each(function (index) {

        var chk = $(this).find('.form-check2:first')

        if (chk.prop('disabled') == false) {

            $(chk).prop('checked', $(event.target).is(":checked"))

            if ($(event.target).is(":checked")) {

                if (chkActionIds.length > 0) chkActionIds += ','

                chkActionIds += $(chk).attr('value')

            }
        }
    });
    console.log(chkActionIds)

    $('.ChkActionIds').attr('value', chkActionIds)
}

function editTextContent(e) {
    var target = $(e);
    var id = target.data('id')
    if (id > 0) {
        console.log($('#' + id))
        $('#row' + id).removeClass('d-none')
        $('#quit' + id).removeClass('d-none')
        $('#save' + id).removeClass('d-none')
        $('#text' + id).addClass('d-none')
    }
}
function quitEditTextContent(e) {
    var target = $(e);
    var id = target.data('id')
    if (id > 0) {
        console.log($('#' + id))
        $('#text' + id).removeClass('d-none')
        $('#row' + id).addClass('d-none')
        $('#quit' + id).addClass('d-none')
        $('#save' + id).addClass('d-none')
    }
}
function saveTextContent(e) {
    var target = $(e);
    var id = target.data('id');
    var textContent = $('#row' + id).val();
    var url = target.data('url');
    var command = {
        Id: parseInt(id),
        TextContent: textContent
    };
    var urlSuccess = target.data('ajax-success-url');
    var elementId = target.data('ajax-success-id');
    //console.log(command)
    $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(command),
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
        },
        success: function (response) {
            if (response.Succeeded) {
                fetchData({
                    type: 'GET',
                    url: urlSuccess,
                    dataType: 'html'
                }).then(response => {

                    $('#' + elementId).html(response);
                })
            }
            toastMessage(response.Succeeded, response.Messages.join('<br />'))
            initSelect2AutoComplete($('.select2-auto-complete'))
        },
        error: function (xhr, status, error) {
            toastMessage(false, "Vui lòng thử lại");
        }
    });
}
function selectRow() {
    var allChecked = true;
    var hasCheck = false;
    var chkActionIds = '';
    $('.main-table>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check-input:first');
        if (chk.is(":checked")) {
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
function selectRow2() {
    var allChecked = true;
    var hasCheck = false;
    var chkActionIds = '';
    $('.main-table>tbody>tr').each(function (index) {
        var chk = $(this).find('.form-check2:first');
        if (chk.is(":checked")) {
            hasCheck = true;
            if (chkActionIds.length > 0) chkActionIds += ',';
            chkActionIds += $(chk).attr('value');
        }
    });
    $('.ChkActionIds').attr('value', chkActionIds);
    if (hasCheck) {
        $('.bulk-select-actions').removeClass('d-none');
    }

    if (allChecked) {
        $('.main-table>thead>tr').each(function (index) {
            var chk = $(this).find('.form-check2:first');
            if (chk) {
                chk.prop('checked', true);
            }
        });
    }
    else {
        $('.main-table>thead>tr').each(function (index) {
            var chk = $(this).find('.form-check2:first');
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
    $('.ChkActionIds').attr('value', chkActionIds)
}

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
    $('.ChkActionIds').attr('value', chkActionIds);
    if (hasCheck) {
        $('.bulk-select-actions').removeClass('d-none');
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
function selectRadioModal(e) {
    var ckId = '';
    var docLink = $(".doc-url").val();
    var id = $(e).val()
    if (id > 0) {
        ckId = id;
    }
    //console.log(1234, id)
    if (ckId > 0) {
        $(".doc-link").val('')
    }
    else {
        $(".doc-link").val(docLink)
    }
    $("#Command_LinkDocItemId").val(ckId)
    $("#Item3_LinkDocItemId").val(ckId)
    //console.log(12345, $("#Command_LinkDocItemId").val())
}
function docFormatData(data) {
    if (!data.id || data.id <= 0) { return data.text; }

    let itemHtml = `<div class="row profile align-items-center text-center text-md-start p-1 border-bottom border-200">
                            <div class="col px-x1- py-2-">
                                <div class="row">
                                    <div class="col-12 mb-0 mt-0 ">
                                        <h6 class="fs--1">${data.text} (<i>${data.docNumber} - ${data.issueDate}</i>)</h6>
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
    if (data.title == null) {
        data.title = data.text;
    }
    if (data.id) {
        $("#Command_Link").val(data.docUrl)
        $(".doc-link").val(data.docUrl)
        $(".doc-link-modal").val(data.docUrl)
    }
    if (data.docNumber && data.title.trim() != data.docNumber.trim()) {
        return `${data.docNumber.trim()}`
    }


    return data.title
}
function resultFormatData(data) {
    const name = data?.text || data?.name;
    if (!data.id || data.id <= 0) { return name; }
    return name;
    //let itemHtml = `<div class="row align-items-center text-center text-md-start p-1 border-bottom border-200">
    //                        <div class="col px-x1- py-2-">
    //                            <div class="row">
    //                                <div class="col-12">
    //                                    <h6 class="fs-0">${name}</h6>
    //                                </div>
    //                            </div>
    //                        </div>
    //                    </div>`

    //return $(
    //    itemHtml
    //)
}
function resultFormatDataSelection(data) {
    if (data.text == null || data.text == '') {
        data.text = data.title ? data.title : '';
    }
    if (data.name && data.text.trim() != data.name.trim()) {
        return `${data.text} ${data.name.trim()}`
    }
    return data.text
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
function importContent(element) {
    var target = $(element);
    var url = target.data('url')
    var lawJudgId = target.data('value');
    data = {
        lawJudgId: lawJudgId
    }
    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json',
        data: data,
        dataType: 'json',
        beforeSend: () => {
            target.attr('disabled', 'disabled')
            target.data('og', target.html())
            if (target.hasClass('cke_element')) {
                target.html('<span style="padding: 0 5px;">Vui lòng đợi...</span>')
            } else if (target.hasClass('cke_button')) {
                target.html('<span style="padding: 0 2px;">...</span>')
            } else {
                target.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>')
            }
        },

        success: (response) => {
            console.log(response.Data)
            if (response.Data) {
                console.log(response.Data)
                var editors = $('.ckeditor-input')
                if (editors.length > 0) {
                    console.log(1)
                    for (var key in CKEDITOR.instances)
                        CKEDITOR.instances[key].insertHtml(response.Data);
                }
            }
            else if (response.Messages) {
                toastMessage(response.Succeeded, response.Messages.join('<br />'))
            }
            removeLoading(element)
        },
        error: function (xhr, status, error) {
            alert('Error: ' + error + status);
        }
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

            //root.find('a').unwrap()

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

    if (modalMedia.length > 0) {

        modalMedia.modal('hide')

        let mediaKey = modalMedia.attr('data-key') || ''

        if (mediaKey.trim().length > 0) {
            if (mediaKey.includes('key_cke_')) {
                $(`.${mediaKey}`).find('input').val(mediaPath)
                $($(`.${mediaKey}`).find('input')[0]).focus().blur()
                $(`.${mediaKey}.alt-input`).find('input').val(mediaName)
            }
            else {
                const selectMediaInput = $(`#SelectMediaInput${mediaKey}`),
                    mediaControl = $(`#SelectMediaView${mediaKey}`)

                if (selectMediaInput.length > 0) {

                    selectMediaInput.val(mediaPath)

                    mediaControl.attr('src', mediaPath)
                }
            }
        }
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
        //console.log(1, selectboxShow)
        if (selectboxShow) {
            selectboxShow.attr('disabled', 'disabled')
        }
    }
    else {
        selectboxShow.removeAttr('disabled')
    }
    if ($(elementHide).hasClass('d-none')) {
        //console.log(2, selectboxHide)
        if (selectboxHide) {
            selectboxHide.attr('disabled', 'disabled')
        }
    }
    else {
        selectboxHide.removeAttr('disabled')
    }
})
$(document).on('change', 'table .row-checkbox', function () {
    let table = $(this).closest('table')
    selectRowRelate(table)
})
selectRowRelate = table => {
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
onUploadDocsFileSuccessed = (element, xhr) => {
    try {
        removeLoading(element);
        const btnUpFile = $('.btn-up-docfile');
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
        var dataType, fileTypeId, dataId, elementResultId;

        if ($('#createFileCommand_DocId').length) {
            dataId = $('#createFileCommand_DocId').val();
        }
        if ($('#fcreateFileCommand_ElementResultId').length) {
            elementResultId = $('#createFileCommand_ElementResultId').val();
        }
        if (dataId != undefined) {
            //Nếu là box uploadfile
            removeLoading(element);
            if (elementResultId != undefined && elementResultId.length && $('#' + elementResultId).length) {
                var filePath = xhr.responseJSON.Data[0].FilePath;
                $('#' + elementResultId).val(filePath);
                if ($('#' + elementResultId + '_Preview').length) {
                    $('#' + elementResultId + '_Preview').attr('src', filePath);
                }
            }
            else //bảng danh sách file
            {
                var idTable = 'table_'/* + dataType + "_" */ + dataId;
                var table = $('#' + idTable);
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
        }

    } catch (e) {
        console.log(e)
    }
}

function importDocContent(element) {
    const target = $(element);
    const url = target.data('url');
    const docId = target.data('value');

    $.ajax({
        url: url,
        type: 'GET',
        data: { docId: docId },
        dataType: 'json',
        beforeSend: () => {
            target.attr('disabled', 'disabled').data('og', target.html());

            if (target.hasClass('cke_element')) {
                target.html('<span style="padding: 0 5px;">Vui lòng đợi...</span>');
            } else if (target.hasClass('cke_button')) {
                target.html('<span style="padding: 0 2px;">...</span>');
            } else {
                target.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
            }
        },
        success: (response) => {
            if (response.Data) {
                for (let key in CKEDITOR.instances) {
                    CKEDITOR.instances[key].insertHtml(response.Data);
                    CKEDITOR.instances[key].updateElement();
                }
            } else if (response.Messages) {
                toastMessage(response.Succeeded, response.Messages.join('<br />'));
                removeLoading(element);
                return;
            }
            $('#doc_content').trigger('submit');
            removeLoading(element);

        },
        error: function (xhr, status, error) {
            alert('Error: ' + error + ' - ' + status);
        }

    });
}
//function WorkflowResultToDoc(element) {
//    $('#Command_IsWorkflowResultToDoc').val(true);
//    $('#Command_AddMoreData').val(false);
//    $('#doc_summary').trigger('submit');
//}

onDocContentSuccessed = (form, xhr) => {
    try {
        elementAction = form

        if (xhr.responseJSON.Succeeded) {
            removeLoading(form)
        }

        if (xhr.responseJSON.Messages) {
            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages)
        }


    } catch (e) {
        console.log(e)
    }
}

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
$(document).on('change', 'table thead input[class*="form-check-input"]', function () {
    let table = $(this).closest('table'),
        isChecked = $(this).prop('checked')

    table.find('.row-checkbox').prop('checked', isChecked)
    selectRowRelate(table)
})
//onDocContentSuccessed = (form, xhr) => {
//    try {
//        if (!xhr.responseJSON.Succeeded) {
//            removeLoading(form);
//        }

//        if (xhr.responseJSON.Messages) {
//            toastMessage(xhr.responseJSON.Succeeded, xhr.responseJSON.Messages);
//        }

//        if (xhr.responseJSON.Succeeded) {
//            let docId = form.querySelector("#Command_DocId")?.value;
//            let formUrl = form.getAttribute('data-ajax-url') + "?docId=" + docId;

//            if (formUrl != null) {
//                fetchData({
//                    type: 'GET',
//                    url: formUrl,
//                    dataType: 'html'
//                }).then(response => {
//                    let parsedHtml = $('<div>').append($.parseHTML(response));
//                    let newContent = parsedHtml.find("#doc_content").html();

//                    if (newContent) {
//                        $("#doc_content").html(newContent);
//                    } else {
//                        $("#doc_content").html(response);
//                    }
//                    $("#FormFiles").val("");

//                    setTimeout(() => {
//                        $("#doc_content textarea").each(function () {
//                            let editorId = $(this).attr("id");

//                            if (editorId) {
//                                if (CKEDITOR.instances[editorId]) {
//                                    CKEDITOR.instances[editorId].destroy(true);
//                                }

//                                CKEDITOR.replace(editorId, {
//                                    allowedContent: true
//                                });
//                            }
//                        });
//                    }, 100);

//                });

//            }
//        }

//    } catch (e) {
//        console.log(e);
//    }
//};
CKEDITOR.on('instanceReady', function (ev) {
    ev.editor.dataProcessor.htmlFilter.addRules({
        elements: {
            a: function (element) {
                if (element.attributes.name && element.attributes.name.startsWith('bookmark')) {
                    return false; // Loại bỏ thẻ <a> có name="bookmarkX"
                }
            }
        }
    });
});
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
$('.form-select-search').change(function () {
    var formData = $('#form_create').serialize();
    $.ajax({
        url: '/LuatVietNamDoc/Docs/Create?handler=Session',
        type: 'POST',
        data: formData,
        success: function (response) {
            location.reload();
        },
        error: function (xhr, status, error) {
            console.error("Error submitting form:", error);
        }
    });
});
$('.form-select-searchEdit').change(function () {
    var formData = $('#form_edit').serialize();
    $.ajax({
        url: '/LuatVietNamDoc/Docs/Edit?handler=Session',
        type: 'POST',
        data: formData,
        success: function (response) {
            location.reload();
        },
        error: function (xhr, status, error) {
            console.error("Error submitting form:", error);
        }
    });
});
$(document).on('keyup', '.numberInput', (e) => {
    var sanitized = AddDotToText($(e.target).val());
    $(e.target).val(sanitized);
    CalulateMoneyVAT();
});
AddDotToText = (value) => {
    if (value === null || value === undefined) return "";

    let str = String(value).trim();
    if (!str) return "";

    // Giữ dấu âm nếu có
    let sign = '';
    if (str[0] === '-' || str[0] === '+') {
        if (str[0] === '-') sign = '-';
        str = str.slice(1);
    }

    // Chỉ giữ số và dấu chấm
    str = str.replace(/[^\d,]/g, '');

    // Tách phần nguyên & thập phân
    let [intPart, fracPart] = str.split(',') || [];
    intPart = intPart || '0';

    // Xóa số 0 thừa đầu
    intPart = intPart.replace(/^0+(?=\d)/, '') || '0';

    // Thêm dấu phẩy ngăn cách nghìn
    intPart = intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ".");

    return sign + intPart + (fracPart ? "." + fracPart : "");
}
$(document).on('change', '#Command_VATPercent', function () {
    CalulateMoneyVAT();
})
$(document).on('change', '#Command_CSKHOrderValueText', function () {
    CalulateMoneyVAT();
})
$(document).on('change', '#Command_SalePercent', function () {
    CalulateMoneyVAT();
})
$(document).on('change', '#Command_ProductParentGroupId', function () {
    var VATPersent = $("#Command_VATPercent");
    var salePersent = $("#Command_SalePercent");
    var productGroup = $('input[name="Command.ProductGroupId"]').val();
    var productGroupParent = parseInt($(this).val());
    var id = parseInt(productGroup);
    if (productGroup <= 0 && productGroupParent > 0) {
        id = productGroupParent;
    }
    if (id > 0) {
        $.ajax({
            url: '/LuatVietNamCms/CSKHOrders/Index?handler=SearchProductGroupById',
            type: 'GET',
            data: {
                id: id
            },
            success: function (response) {
                if (response.Succeeded) {
                    VATPersent.val(response.Data.VATPercent).prop('selected', true);
                    salePersent.val(response.Data.SalePercent);
                    CalulateMoneyVAT();
                }
            },
            error: function (xhr, status, error) {
                console.error("Error submitting form:", error);
            }
        });
    }

})
//$(document).on('change', 'input[name="Command.ProductGroupId"]', function () {
//    var VATPersent = $("#Command_VATPercent");
//    var salePersent = $("#Command_SalePercent");
//    var productGroup = $(this).val();
//    var productGroupParent = parseInt($('#Command_ProductParentGroupId').val());
//    var id = parseInt(productGroup);
//    if (productGroup <= 0 && productGroupParent > 0) {
//        id = productGroupParent;
//    }
//    if (id > 0) {
//        $.ajax({
//            url: '/LuatVietNamCms/CSKHOrders/Index?handler=SearchProductGroupById',
//            type: 'GET',
//            data: {
//                id: id
//            },
//            success: function (response) {
//                if (response.Succeeded) {
//                    console.log(response.Data)
//                    VATPersent.val(response.Data.VATPercent).prop('selected', true);
//                    salePersent.val(response.Data.SalePercent);
//                    CalulateMoneyVAT();
//                }
//            },
//            error: function (xhr, status, error) {
//                console.error("Error submitting form:", error);
//            }
//        });
//    }

//})
const formatNumberRound3 = (value) => {
    if (value === null || value === undefined) return "";

    let num = Number(value);
    if (isNaN(num)) return "";

    return num.toLocaleString("en-US", {
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    });
};
CalulateMoneyVAT = () => {
    var VATPersent = $("#Command_VATPercent");
    var CSKHorderAmount = $("#Command_CSKHOrderAmountText");
    var CSKHorderVAT = $("#Command_CSKHOrderVATText");
    var CSKHOrderValue = $("#Command_CSKHOrderValueText");
    var SaleValueText = $("#Command_SaleValueText");
    var salePersent = $("#Command_SalePercent");
    var VATPersentValue = parseInt(VATPersent.val());
    var CSKHorderAmountValue = parseInt(CSKHorderAmount.val().replaceAll(".", ""));
    var SaleValue = parseInt(SaleValueText.val().replaceAll(".", ""));
    var salePersentValue = parseInt(salePersent.val());
    console.log(CSKHorderAmount.val())
   var orderValue = CSKHorderAmountValue;
    if (CSKHorderAmountValue != undefined && CSKHorderAmountValue > 0) {
        if (VATPersentValue > 0) {
            orderValue = (CSKHorderAmountValue / (1 + (VATPersentValue / 100)));
        }
        var VATValue = CSKHorderAmountValue - orderValue
        CSKHorderVAT.val(AddDotToText(Math.round(VATValue)))
        CSKHOrderValue.val(AddDotToText((Math.round(orderValue))))
    }

    console.log(salePersentValue, orderValue)
    if (orderValue > 0) {
        SaleValueText.val(AddDotToText(Math.round((orderValue * salePersentValue) / 100)))
    }
}
$(document).on('change',
    '.block-list-tracuu input[type="checkbox"]',
    function (e) {
        var self = $(this),
            docgroupIds = $('input[name="DocGroupIds"]').filter(':checked').map(function () {
                return this.value
            }).get(),
            docTypeIds = $('input[name="DocTypeIds"]').filter(':checked').map(function () {
                return this.value
            }).get(),
            fieldIds = $('input[name="FieldIds"]').filter(':checked').map(function () {
                return this.value
            }).get(),
            effectStatusIds = $('input[name="EffectStatusIds"]').filter(':checked').map(function () {
                return this.value
            }).get(),
            organIds = $('input[name="OrganIds"]').filter(':checked').map(function () {
                return this.value
            }).get(),
            isueYear = $('input[name="IsueYear"]').filter(':checked').map(function () {
                return this.value
            }).get();
        var valueDocGroupIds = docgroupIds.length > 0 ? docgroupIds.join(',') : ''
        $('#Query_DocGroupIdStrings').val(valueDocGroupIds);
        var valuedocTypeIds = docTypeIds.length > 0 ? docgroupIds.join(',') : ''
        $('#Query_DocTypeIdStrings').val(valuedocTypeIds);
        var valueeffectStatusIds = effectStatusIds.length > 0 ? effectStatusIds.join(',') : ''
        $('#Query_EffectStatusIdStrings').val(valueeffectStatusIds);
        var valueorganIds = organIds.length > 0 ? organIds.join(',') : ''
        $('#Query_OrganIdStrings').val(valueorganIds);
        var valueisueYear = isueYear.length > 0 ? isueYear.join(',') : ''
        $('#Query_IsueYearString').val(valueisueYear);
        var valueFieldIds = fieldIds.length > 0 ? fieldIds.join(',') : ''
        $('#Query_FieldIdString').val(valueFieldIds);
    })
function getProductGroupChild() {

}
function checkAndOpenModal(button) {
    const id = button.getAttribute("data-id");

    fetch(`/LuatVietNamDoc/DocTemps/Edit?handler=Check&id=${id}`)
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                const url = `/LuatVietNamDoc/DocTemps/Edit?id=${id}`;
                modalGet(button, url, "Cập file PDF và trạng thái");
            } else {
                toastMessage(false, data.message);
            }
        })
        .catch(err => {
            toastMessage(false, "Có lỗi xảy ra khi kiểm tra dữ liệu");
        });

    return false;
}
function toggleFullscreen(btn) {
    const icon = btn.querySelector('#toggleIcon');
    const viewer = document.getElementById('docViewerWrapper');
    const content = document.querySelector('.contentItem');

    if (!viewer || !content) return;

    const isExpanded = icon.classList.contains('fa-expand');

    if (isExpanded) {
        // Đổi icon + tooltip
        icon.classList.replace('fa-expand', 'fa-compress');
        icon.setAttribute('title', 'Thu nhỏ');

        // Ẩn viewer và mở rộng content
        viewer.classList.add('d-none');
        content.classList.remove('col-6');
        content.classList.add('col-12');
    } else {
        // Đổi icon + tooltip
        icon.classList.replace('fa-compress', 'fa-expand');
        icon.setAttribute('title', 'Mở rộng');

        // Hiện viewer và thu hẹp content
        viewer.classList.remove('d-none');
        content.classList.remove('col-12');
        content.classList.add('col-6');
    }
}
