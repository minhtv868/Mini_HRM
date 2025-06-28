/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

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
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup','-','RemoveFormat'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'about', groups: ['about'] }
	];
	config.allowedContent = true;

	config.extraAllowedContent =
		"*(*);*{*};*[*];*;object[id,name,width,height]; param[name,value]; " +
		"embed[src,type,allowscriptaccess,allowfullscreen,wmode,width,height]; audio[src,controls];video[id,class,controls,preload,width,height,data-setup,poster]; source[src,type];style";
	config.extraPlugins = 'image2,pastecode,indentblock2';
	config.removeButtons = 'Save,NewPage,Preview,Print,Templates,Cut,Scayt,Form,Checkbox,Radio,TextField,Select,Textarea,Button,ImageButton,Replace,HiddenField,Strike,CopyFormatting,CreateDiv,BidiLtr,Language,BidiRtl,Flash,HorizontalRule,SpecialChar,PageBreak,Iframe,ShowBlocks,About';
	
};