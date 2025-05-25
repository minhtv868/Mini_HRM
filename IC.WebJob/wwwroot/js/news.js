$(function () {
    $(document).on('keyup', '#Command_VoiceContent', (e) => {
        getVoiceContentLengh()
    })

    $(document).on('input change', '#Command_Title', (e) => {
        let self = $(e.target), label = self.next(),
            className = '', inputLength = self.val().trim().length
        
        if (inputLength <= 75) {
            className = 'success'
        } else if (inputLength >= 76 && inputLength <= 85) {
            className = 'warning'
        } else if (inputLength >= 86 && inputLength <= 100) {
            className = 'danger'
        }

        self.removeClass('title-success').removeClass('title-warning').removeClass('title-danger')
        label.removeClass('label-success').removeClass('label-warning').removeClass('label-danger')

        if (inputLength > 0 && className.length > 0) {
            self.addClass(`title-${className}`)
            label.addClass(`label-${className}`)
        }
    })

    $(document).on('input change', '#Command_Intro', (e) => {
        let self = $(e.target), label = self.next(),
            className = '', inputLength = self.val().trim().length
        
        if (inputLength < 165) {
            className = 'success'
        } else if (inputLength >= 166 && inputLength <= 185) {
            className = 'warning'
        } else if (inputLength >= 186 && inputLength <= 200) {
            className = 'danger'
        }

        self.removeClass('title-success').removeClass('title-warning').removeClass('title-danger')
        label.removeClass('label-success').removeClass('label-warning').removeClass('label-danger')
        
        if (inputLength > 0 && className.length > 0) {
            self.addClass(`title-${className}`)
            label.addClass(`label-${className}`)
        }
    })

    $(document).on('click', '.edit-slug', (e) => {
        $('#slug-container').addClass('d-none')
        $('#edit-slug-container').removeClass('d-none')
    })

    $(document).on('click', '.cancel-change-slug', (e) => {
        let slugContainer = $('#slug-container'), slugInput = slugContainer.attr('data-slug') || ''
        slugContainer.removeClass('d-none')
        $('#edit-slug-container').addClass('d-none')
        $('#Command_Slug').val(slugInput)
    })

    $(document).on('click', '.confirm-slug-change', (e) => {
        let slugContainer = $('#slug-container'),
            commandSlug = $('#Command_Slug'),
            commandTitle = $('#Command_Title'),
            slugTag = $('.slug-tag'),
            slugValue = toSlug(commandSlug.val(), '-')

        if (slugValue.length == 0) {
            slugValue = toSlug(commandTitle.val(), '-')
        }

        commandSlug.val(slugValue)

        slugContainer.attr('data-slug', slugValue)

        if (slugValue.length > 0) {
            slugTag.text(slugValue)
        }
        else {
            slugTag.text('')
        }

        slugContainer.removeClass('d-none')

        $('#edit-slug-container').addClass('d-none')
    })

    $(document).on('click', '.edit-slug-used', (e) => {
        $('#slug-used-container').addClass('d-none')
        $('#edit-slug-used-container').removeClass('d-none')
    })

    $(document).on('click', '.cancel-change-slug-used', (e) => {
        let slugUsedContainer = $('#slug-used-container'),
            slugUsedInput = slugUsedContainer.attr('data-slug-used') || ''
        slugUsedContainer.removeClass('d-none')
        $('#edit-slug-used-container').addClass('d-none')
        $('#Command_NewsExtInput_SlugUsed').val(slugUsedInput)
    })

    $(document).on('click', '.confirm-slug-change-used', (e) => {
        let slugUsedContainer = $('#slug-used-container'),
            commandTitle = $('#Command_Title'),
            titleSlug = toSlug(commandTitle.val(), '-'),
            commantCate = $('#Command_CateID option:selected'),
            cateSlug = toSlug(commantCate.text(), '-'),
            firstPartSlugUsed = slugUsedContainer.attr('data-first-part-slug-used') || '',
            lastPartSlugUsed = slugUsedContainer.attr('data-last-part-slug-used') || '',
            commandSlugUsed = $('#Command_NewsExtInput_SlugUsed'),
            metaUrl = $('#js-seo-preview__google-url'),
            socialUrl = $('#js-seo-preview__facebook-url'),
            fullSlug = '',
            slugUsedTag = $('.slug-used-tag'),
            slugUsedValue = toSlug(commandSlugUsed.val(), '-') 

        if (firstPartSlugUsed.length == 0) {
            firstPartSlugUsed = `/tin-tuc/${cateSlug}/`
        }

        if (lastPartSlugUsed.length == 0) {
            lastPartSlugUsed = '-xxxxxxxxxxxxxxxxx.html'
        }

        slugUsedContainer.attr('data-slug-used', slugUsedValue)

        if (slugUsedValue.length > 0) {
            fullSlug = `${firstPartSlugUsed}${slugUsedValue}${lastPartSlugUsed}`
            commandSlugUsed.val(`${slugUsedValue}`)
            slugUsedTag.text(fullSlug)
            fullSlug = truncate(fullSlug, 72)
            metaUrl.text(`www.voh.com.vn${fullSlug}`)
            socialUrl.text(`www.voh.com.vn${fullSlug}`)
        } else {
            slugUsedTag.text('')
            commandSlugUsed.val('')
            metaUrl.text('')
            socialUrl.text('')
        }
        
        slugUsedContainer.removeClass('d-none')

        $('#edit-slug-used-container').addClass('d-none')
    })
})

var autoSaveInterval

var formatDate = (date) => {

    if (typeof date == 'string')
        date = new Date(date)

    return ("00" + (date.getMonth() + 1)).slice(-2)
        + "/" + ("00" + date.getDate()).slice(-2)
        + "/" + date.getFullYear() + " "
        + ("00" + date.getHours()).slice(-2) + ":"
        + ("00" + date.getMinutes()).slice(-2)
        + ":" + ("00" + date.getSeconds()).slice(-2)
}

var autoSave = (isClose = false) => {

    var form = document.getElementById('news-create-form'),
        formClone = form.cloneNode(true)

    formClone.action = `/News/NewsDraft/Edit${ isClose ? '?isClose=true' : '' }`

    delete formClone.dataset.ajaxFailure
    delete formClone.dataset.ajaxLoading
    delete formClone.dataset.ajaxSuccess
    delete formClone.dataset.ajaxUpdate

    var dataTags = $(formClone).find('#Command_Tags').select2('data')

    if (dataTags.length > 0) {
        $.each(dataTags, function (key, item) {
            $(formClone).append(`<input name="Command.Tags[]" value="${item.id}" />`)
        })
    }

    var dataHosts = $(formClone).find('#Command_Hosts').select2('data')

    if (dataHosts.length > 0) {
        $.each(dataHosts, function (key, item) {
            $(formClone).append(`<input name="Command.Hosts[]" value="${item.id}" />`)
        })
    }

    $(formClone).find('#Command_Status').val($('#Command_Status').val())

    $(formClone).find('#Command_NewType').val($('#Command_NewType').val())

    $(formClone).find('#Command_NewDetailInput_Content').val(CKEDITOR.instances.Command_NewDetailInput_Content.getData())

    fetchData({
        url: formClone.action,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
        },
        type: 'post',
        dataType: 'json',
        data: $(formClone).serialize(),
    }).then(response => {

        $('#auto-save-time').text(`Lưu nháp lần cuối lúc: ${formatDate(new Date())}`)

        bindData()
    })

    delete formClone
}

const closeAutoSave = () => {
    if (autoSaveInterval) {
        clearInterval(autoSaveInterval)
    }

    autoSave(true)

    return false
}

const closeDeleteNewsDraft = (id) => {
    if (autoSaveInterval) {
        clearInterval(autoSaveInterval)
    }

    fetchData({
        type: 'post',
        url: '/News/News/Create?handler=NewsDraftDelete',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
        },
        type: 'post',
        dataType: 'json',
        data: { id: id },
    }).then(response => {

        if (response.Messages) {
            toastMessage(response.Succeeded, response.Messages.join('<br />'))
        }

        bindData()
    })

    return false
}

getVoiceContentLengh = () => {
    const voiceContentLength = $('#Command_VoiceContent').val().trim().length,
        element = $('#VoiceContent_Length')

    if (voiceContentLength > 0) {
        if (voiceContentLength > 5000) {
            element.addClass('text-danger').text(`${voiceContentLength.toLocaleString()} ký tự.`)
        } else {
            element.remove('text-danger').text(`${voiceContentLength.toLocaleString()} ký tự.`)
        }
    } else {
        element.html('')
    }
}

genVoiceContent = (element, id) => {
    const voiceContentElement = $('#Command_VoiceContent')

    let buttons = {
        'confirm': {
            text: "Đồng ý",
            btnClass: 'btn btn-danger',
            keys: ['enter'],
            action: function () {
                fetchData({
                    type: 'GET',
                    url: '/News/News?handler=GenVoiceContent',
                    data: { id: id },
                    dataType: 'json',
                    beforeSend: () => {
                        $(element).html('Gen nội dung đọc <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>')
                    },
                    callback: () => {
                        $(element).text('Gen nội dung đọc')
                    }
                }).then(response => {
                    if (response.Data) {
                        voiceContentElement.val(response.Data)
                        getVoiceContentLengh()
                    }
                    else {
                        voiceContentElement.val('')
                    }
                })
            }
        }
    }

    showMessage({
        title: 'Xác nhận gen nội dung đọc?',
        icon: 'fa fa-question-circle',
        columnClass: 'col-md-6 col-md-offset-3',
        message: 'Nội dung đọc sẽ được gen từ tiêu đề, giới thiệu và nội dung bài viết.',
        buttons
    })
    
    return false
}

deleteVoiceContent = (element) => {
    let buttons = {
        'confirm': {
            text: "Đồng ý",
            btnClass: 'btn btn-danger',
            keys: ['enter'],
            action: function () {
                $('#Command_VoiceContent').val('')
                $('#VoiceContent_Length').text('')
            }
        }
    }

    showMessage({
        title: 'Xác nhận xóa nội dung đọc?',
        icon: 'fa fa-question-circle',
        columnClass: 'col-md-6 col-md-offset-3',
        message: 'Dữ liệu đã xóa sẽ không thể phục hồi !',
        buttons
    })

    return false
}

voiceConversionAPICallback = (element, id) => {
    let buttons = {
        'confirm': {
            text: "Đồng ý",
            btnClass: 'btn btn-danger',
            keys: ['enter'],
            action: function () {
                let timeout
                const elementHtml = $(element).html()
                fetchData({
                    type: 'POST',
                    url: '/News/News/Edit?handler=VoiceAPICallback',
                    data: { id: id },
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('RequestVerificationToken', $('input:hidden[name="__RequestVerificationToken"]').val())
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
                    if (response.Messages) {
                        toastMessage(response.Succeeded, response.Messages.join('<br />'))
                    }
                })
            }
        }
    }

    showMessage({
        icon: 'fa fa-question-circle',
        columnClass: 'col-md-6 col-md-offset-3',
        message: 'Xác nhận chuyển nội dung tin bài thành giọng nói ?',
        buttons
    })

    return false;
}

newsOnSuccessed = (form, xhr, resetHiddenField = false, isBindData = true) => {
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

            if (xhr.responseJSON.Data != null && xhr.responseJSON.Data == 'PreviewMobile') {
                let previewIframe = document.getElementById('preview-mobile-iframe')

                previewIframe.classList.add('iframe-placeholder')

                previewIframe.src = xhr.responseJSON.ReturnUrl

                var sectionOffset = $('#cke_Command_NewDetailInput_Content'),
                    iframeMobilePreview = $('#preview-mobile-iframe')

                if (sectionOffset.length > 0) {
                    modalPopup.find('.modal-body').first().animate({
                        scrollTop: sectionOffset.position().top + 130
                    }, 'slow')
                }

                $('#preview-mobile-iframe').contents().find("html, body").animate({ scrollTop: 500 }, { duration: 'medium', easing: 'swing' })
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

newsPreviewSubmit = (element) => {
    var form = $(element).closest('form')
    if (form) {

        form.find('#Command_PreviewMobile').val(false)

        form.find('#Command_Preview').val(true)

        form.find('#Command_NewDetailInput_Content').val(CKEDITOR.instances.Command_NewDetailInput_Content.getData())

        return true
    }

    return false
}

newsPreviewMobileSubmit = (element) => {
    var form = $(element).closest('form')

    if (form) {

        elementAction = form

        document.getElementById('preview-mobile-iframe').classList.add('iframe-placeholder')

        document.getElementById('preview-mobile-iframe').src = ''

        form.find('#Command_Preview').val(false)

        form.find('#Command_PreviewMobile').val(true)

        form.find('#Command_NewDetailInput_Content').val(CKEDITOR.instances.Command_NewDetailInput_Content.getData())

        form.submit()
    }

    return false
}

publishNews = (element) => {
    let form = $(element).closest('form'), minCount = 300,
        newsSelected = $('input[name="ChkActionIds"]:checked', $('#table-ticket-body'))

    if (newsSelected.length == 1) {
        let textCount = newsSelected.first().attr('data-text-count') || 0

        if (textCount < minCount) {

            $.confirm({
                title: 'Xác nhận?',
                columnClass: 'col-md-6 col-md-offset-3',
                content: `Bài viết này ngắn hơn ${minCount} từ.<br/> Nội dung quá ngắn có thể không cung cấp đủ thông tin cho người đọc.<br/> Kiểm tra lại trước khi publish để tránh xuất bản nội dung kém chất lượng.`,
                type: 'white',
                onClose: function () {
                    
                },
                buttons: {
                    ok: {
                        text: "Vẫn publish",
                        btnClass: 'btn btn-danger',
                        keys: ['enter'],
                        action: function () {
                            form.submit()
                        }
                    },
                    addcontent: {
                        text: 'Kiểm tra lại',
                        btnClass: 'btn-blue',
                        keys: ['enter'],
                        action: function () {

                        }
                    }
                },
            })

            return false
        }
    }

    return true
}

newsSubmit = (element) => {
    let form = $(element).closest('form'),
        wordCountElement = $('#cke_wordcount_Command_NewDetailInput_Content'),
        wordCount = wordCountElement.attr('data-word-count') || 0,
        paragraphs = wordCountElement.attr('data-paragraphs') || 0,
        imageCount = wordCountElement.attr('data-image-count') || 0,
        mediaCount = wordCountElement.attr('data-media-count') || 0,
        minCount = 300

    if (form) {
        form.find('.paragraphs-count-input').first().val(paragraphs)

        form.find('.word-count-input').first().val(wordCount)

        form.find('.image-count-input').first().val(imageCount)

        form.find('.media-count-input').first().val(mediaCount)

        form.find('#Command_Preview').val(false)
        form.find('#Command_PreviewMobile').val(false)
        form.find('#Command_NewDetailInput_Content').val(CKEDITOR.instances.Command_NewDetailInput_Content.getData())

        if (wordCount < minCount) {
            let isSubmit = false
            //toastMessage(false, `Bài viết này ngắn hơn ${minCount} từ,<br/> Nội dung quá ngắn có thể không cung cấp đủ thông tin cho người đọc.<br/> Cân nhắc bổ sung thêm nội dung để hoàn thiện bài viết.`)
            $.confirm({
                title: 'Xác nhận?',
                columnClass: 'col-md-6 col-md-offset-3',
                content: `Bài viết này ngắn hơn ${minCount} từ,<br/> Nội dung quá ngắn có thể không cung cấp đủ thông tin cho người đọc.<br/> Cân nhắc bổ sung thêm nội dung để hoàn thiện bài viết.`,
                type: 'white',
                onClose: function () {
                    return isSubmit
                },
                buttons: {
                    ok: {
                        text: "Bỏ qua và lưu bài",
                        btnClass: 'btn btn-danger',
                        keys: ['enter'],
                        action: function () {
                            form.submit()
                        }
                    },
                    addcontent: {
                        text: 'Bổ sung nội dung',
                        btnClass: 'btn-blue',
                        keys: ['enter'],
                        action: function () {
                           
                        }
                    }
                },
            })

            return isSubmit
        }

        if (form.valid()) {
            return true
        }
    }

    return false
}

truncate = (original_text, limit) => {
    var truncated_text = original_text

    if (original_text.length > limit) {
        truncated_text = original_text.substring(0, limit)
        truncated_text = truncated_text + " ..."
    }

    return truncated_text
}