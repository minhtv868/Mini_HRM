CKEDITOR.editorConfig = function (config) {
    config.language = 'vi';
    config.versionCheck = false;
    config.disableNativeSpellChecker = true;
    config.removePlugins = 'a11ychecker,indentblock';
    config.toolbarGroups = [
        { name: 'document', groups: ['mode', 'document', 'doctools'] },
        { name: 'clipboard', groups: ['clipboard', 'undo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
        { name: 'forms', groups: ['forms'] },
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
        { name: 'insert', groups: ['insert'] },
        { name: 'styles', groups: ['styles'] },
        { name: 'colors', groups: ['colors'] },
        { name: 'tools', groups: ['tools'] },
        { name: 'others', groups: ['others'] },
        { name: 'about', groups: ['about'] }
    ];
    config.extraPlugins = 'image2,listtable,pastecode,indentblock2';
    config.allowedContent = true;
    config.extraAllowedContent =
        "*(*);*{*};*[*];*;object[id,name,width,height]; param[name,value]; " +
        "embed[src,type,allowscriptaccess,allowfullscreen,wmode,width,height]; audio[src,controls];video[id,class,controls,preload,width,height,data-setup,poster]; source[src,type];style";

    config.removeButtons = 'Save,NewPage,Preview,Print,Templates,Cut,Copy,Undo,Redo,Find,SelectAll,Scayt,Form,Checkbox,Radio,TextField,Select,Textarea,Button,ImageButton,Replace,HiddenField,Strike,Subscript,CopyFormatting,CreateDiv,BidiLtr,Language,BidiRtl,Flash,HorizontalRule,SpecialChar,PageBreak,Iframe,ShowBlocks,About';

    config.height = '600px';
};