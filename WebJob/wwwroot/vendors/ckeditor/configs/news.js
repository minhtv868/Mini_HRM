/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    config.language = 'vi';
    config.versionCheck = false;
    config.disableNativeSpellChecker = true;
    config.removePlugins = 'a11ychecker,indentblock';
	//config.toolbarGroups = [
	//	{ name: 'document', groups: ['mode', 'document', 'doctools'] },
	//	{ name: 'clipboard', groups: ['clipboard', 'undo'] },
	//	{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
	//	{ name: 'forms', groups: ['forms'] },
	//	{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
	//	{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
	//	{ name: 'links', groups: ['links'] },
	//	{ name: 'insert', groups: ['insert'] },
	//	{ name: 'styles', groups: ['styles'] },
	//	{ name: 'colors', groups: ['colors'] },
	//	{ name: 'tools', groups: ['tools'] },
 //       { name: 'others', groups: ['others'] },
 //       { name: 'paragraph', groups: ['texttransform'] }
 //   ];

    config.toolbar = [
        {
            name: 'clipboard', items: ['Cut', 'Copy', 'SelectAll', '-', 'CopyFormatting', 'Anchor', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Footnotes', 'Detail', '-',
                '-', 'Outdent', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', 'ShowBlocks', 'Superscript', 'Subscript',
            ]
        },
        { name: 'document', items: ['Undo', 'Redo', '-', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Find', '-', 'Source', 'Maximize', '-', 'Link', 'Unlink', '-', 'tocinsert', 'tocdelete', 'selectnews', '-', 'Table', '-', 'Html5audio', 'Html5video', 'MediaEmbed','Image', 'Templates'] },
        { name: 'editing', items: ['Styles', 'Format', 'Font', 'FontSize', '-', 'indent', 'BulletedList', 'NumberedList', '-', 'Bold', 'Italic', 'Underline', 'TransformTextSwitcher', '-', 'TextColor', 'BGColor', 'RemoveFormat', 'Blockquote'] },
    ]
    
    //config.removeButtons = 'Scayt,Form,Checkbox,Radio,TextField,Select,Textarea,Button,ImageButton,Replace,HiddenField,Strike,CreateDiv,BidiLtr,Language,BidiRtl,Flash,About';

    config.format_tags = "p;h1;h2;h3;h4;pre;div";

    config.allowedContent = true;

    config.extraAllowedContent =
        "*(*);*{*};*[*];*;object[id,name,width,height]; param[name,value]; " +
		"embed[src,type,allowscriptaccess,allowfullscreen,wmode,width,height]; audio[src,controls];video[id,class,controls,preload,width,height,data-setup,poster]; source[src,type];style";

    config.extraPlugins = 'autogrow,mediaembed,image2,html5audio,html5video,wordcount,footnotes,toc,videodetector,detail,selectnews,texttransform,pastecode,indentblock2'; 

    config.autoGrow_onStartup = true;

    config.autoGrow_minHeight = 250;

    config.autoGrow_maxHeight = 600;

    config.wordcount = {

        // Whether or not you Show Remaining Count (if Maximum Word/Char/Paragraphs Count is set)
        showRemaining: false,

        // Whether or not you want to show the Paragraphs Count
        showParagraphs: true,

        // Whether or not you want to show the Word Count
        showWordCount: true,

        // Whether or not you want to show the Char Count
        showCharCount: false,

        // Whether or not you want to Count Bytes as Characters (needed for Multibyte languages such as Korean and Chinese)
        countBytesAsChars: false,

        // Whether or not you want to count Spaces as Chars
        countSpacesAsChars: false,

        // Whether or not to include Html chars in the Char Count
        countHTML: false,

        // Whether or not to include Line Breaks in the Char Count
        countLineBreaks: false,

        // Whether or not to prevent entering new Content when limit is reached.
        hardLimit: true,

        // Whether or not to to Warn only When limit is reached. Otherwise content above the limit will be deleted on paste or entering
        warnOnLimitOnly: false,

        // Maximum allowed Word Count, -1 is default for unlimited
        maxWordCount: -1,

        // Maximum allowed Char Count, -1 is default for unlimited
        maxCharCount: -1,

        // Maximum allowed Paragraphs Count, -1 is default for unlimited
        maxParagraphs: -1,

        // How long to show the 'paste' warning, 0 is default for not auto-closing the notification
        pasteWarningDuration: 0,

        // Add filter to add or remove element before counting (see CKEDITOR.htmlParser.filter), Default value : null (no filter)
        filter: new CKEDITOR.htmlParser.filter({
            elements: {
                div: function (element) {
                    if (element.attributes.class == 'mediaembed') {
                        return false;
                    }
                }
            }
        })
    };
};