CKEDITOR.dialog.add('html5video', function (editor) {
    var tableId = `key_${CKEDITOR.tools.getNextId()}`,
        posterId = `key_${CKEDITOR.tools.getNextId()}`,
        imageType = 1,
        videoType = 2,
        lang = editor.lang.html5video
    return {
        title: lang.title,
        minWidth: 500,
        minHeight: 100,
        contents: [ {
            id: 'info',
            label: lang.infoLabel,
            elements: [ 
            {
                type: 'vbox',
                padding: 0,
                children: [
                    {
                    type: 'hbox',
                    widths: [ '365px', '110px' ],
                    align: 'right',
                    children: [ {
                        type: 'text',
                        id: 'url',
                        label: lang.url,
                        required: true,
                        className: `cke_dialog_video_url ${tableId}`,
                        validate: CKEDITOR.dialog.validate.notEmpty( lang.urlMissing ),
                        setup: function( widget ) {
                            this.setValue( widget.data.src );
                        },
                        commit: function( widget ) {
                            widget.setData( 'src', this.getValue() );
                        }
                    },
                    {
                        type: 'button',
                        id: 'browse',
                        // v-align with the 'txtUrl' field.
                        // TODO: We need something better than a fixed size here.
                        style: 'display:inline-block;margin-top:23px;',
                        align: 'center',
                        label: lang.selectFile,
                        className: 'cke_element cke_select_video',
                        onClick: () => {
                            return modalMediaEditorGet($('.cke_select_video'), `${tableId}`, videoType)
                        }
                        //hidden: true,
                        //filebrowser: 'info:url'
                    } ]
                } ]
            },
            {
                type: 'checkbox',
                id: 'responsive',
                label: lang.responsive,
                setup: function( widget ) {
                    this.setValue( widget.data.responsive );
                },
                commit: function( widget ) {
                    widget.setData( 'responsive', this.getValue()?'true':'' );
                }
            },
            {
                type: 'vbox',
                padding: 0,
                padding: 0,
                
                children: [{
                    type: 'hbox',
                    widths: [ '365px', '110px' ],
                    align: 'right',
                    children: [ {
                        type: 'text',
                        id: 'poster',
                        label: lang.poster,
                        className: `cke_dialog_poster_url ${posterId}`,
                        setup: function( widget ) {
                            this.setValue( widget.data.poster );
                        },
                        commit: function( widget ) {
                            widget.setData( 'poster', this.getValue() );
                        }
                    },
                    {
                        type: 'button',
                        id: 'browseposter',
                        // v-align with the 'txtUrl' field.
                        // TODO: We need something better than a fixed size here.
                        style: 'display:inline-block;margin-top:20px;',
                        align: 'center',
                        label: lang.buttonPoster,
                        className: 'cke_element cke_select_poster',
                        onClick: () => {
                            return modalMediaEditorGet($('.cke_select_poster'), `${posterId}`, imageType)
                        }
                        //hidden: true,
                        //filebrowser:{action:"Browse",target:"info:poster",url:editor.config.filebrowserImageBrowseUrl}
                    } ]
                }]
                },
                {
                    type: 'hbox',
                    children: [{
                        type: "text",
                        id: 'caption',
                        label: lang.caption,
                        'default': '',
                        className: `${tableId} alternative-information`,
                        setup: function (widget) {
                            this.setValue(widget.data.caption);
                        },
                        commit: function (widget) {
                            widget.setData('caption', this.getValue());
                        }
                    }]
                },
            {
                type: 'checkbox',
                id: 'controls',
                label: lang.controls,
                setup: function (widget) {
                    this.setValue(widget.data.controls);
                },
                commit: function (widget) {
                    widget.setData('controls', this.getValue() ? 'true' : '');
                }
            },
            {
                type: 'hbox',
                id: 'size',
                children: [ {
                    type: 'text',
                    id: 'width',
                    label: editor.lang.common.width,
                    setup: function( widget ) {
                        if ( widget.data.width ) {
                            this.setValue( widget.data.width );
                        }
                    },
                    commit: function( widget ) {
                        widget.setData( 'width', this.getValue() );
                    }
                },
                {
                    type: 'text',
                    id: 'height',
                    label: editor.lang.common.height,
                    setup: function( widget ) {
                        if ( widget.data.height ) {
                            this.setValue( widget.data.height );
                        }
                    },
                    commit: function( widget ) {
                        widget.setData( 'height', this.getValue() );
                    }
                },
                ]
            },

            {
                type: 'hbox',
                id: 'alignment',
                children: [ {
                    type: 'radio',
                    id: 'align',
                    label: editor.lang.common.align,
                    items: [
                        [editor.lang.common.alignCenter, 'center'],
                        [editor.lang.common.alignLeft, 'left'],
                        [editor.lang.common.alignRight, 'right'],
                        [editor.lang.common.alignNone, 'none']
                    ],
                    'default': 'center',
                    setup: function( widget ) {
                        if ( widget.data.align ) {
                            this.setValue( widget.data.align );
                        }
                    },
                    commit: function( widget ) {
                        widget.setData( 'align', this.getValue() );
                    }
                } ]
            } ]
        },
        {
            id: 'Upload',
            hidden: true,
            filebrowser: 'uploadButton',
            label: lang.upload,
            elements: [ {
                type: 'file',
                id: 'upload',
                label: lang.btnUpload,
                style: 'height:40px',
                size: 38
            },
            {
                type: 'fileButton',
                id: 'uploadButton',
                filebrowser: 'info:url',
                label: lang.btnUpload,
                'for': [ 'Upload', 'upload' ]
            } ]
        },
        {
            id: 'advanced',
            label: lang.advanced,
            elements: [ {
                type: 'vbox',
                padding: 10,
                children: [ {
                    type: 'hbox',
                    widths: ["33%", "33%", "33%"],
                    children: [ {
                        type: 'radio',
                        id: 'autoplay',
                        label: lang.autoplay,
                        items: [
                            [lang.yes, 'yes'],
                            [lang.no, 'no']
                        ],
                        'default': 'no',
                        setup: function( widget ) {
                            if ( widget.data.autoplay ) {
                                this.setValue( widget.data.autoplay );
                            }
                        },
                        commit: function( widget ) {
                            widget.setData( 'autoplay', this.getValue() );
                        }
                    }, 
                    {
                        type: 'radio',
                        id: 'loop',
                        label: lang.loop,
                        items: [
                            [lang.yes, 'yes'],
                            [lang.no, 'no']
                        ],
                        'default': 'no',
                        setup: function( widget ) {
                            if ( widget.data.loop ) {
                                this.setValue( widget.data.loop );
                            }
                        },
                        commit: function( widget ) {
                            widget.setData( 'loop', this.getValue() );
                        }
                    },
                    {
                        type: 'radio',
                        id: 'allowdownload',
                        label: lang.allowdownload,
                        items: [
                            [lang.yes, 'yes'],
                            [lang.no, 'no']
                        ],
                        'default': 'no',
                        setup: function( widget ) {
                            if ( widget.data.allowdownload ) {
                                this.setValue(widget.data.allowdownload);
                            }
                        },
                        commit: function( widget ) {
                            widget.setData( 'allowdownload', this.getValue() );
                        }
                    } ]
                }, 
                {
                    type: 'hbox',
                    children: [ {
                        type: "text",
                        id: 'advisorytitle',
                        label: lang.advisorytitle,
                        'default': '',
                        className: `${tableId} alternative-information`,
                        setup: function( widget ) {
                            if ( widget.data.advisorytitle ) {
                                this.setValue(widget.data.advisorytitle);
                            }
                        },
                        commit: function( widget ) {
                            widget.setData( 'advisorytitle', this.getValue() );
                        }
                    } ]
                } ]
            } ]
        } ]
    };
} );
