/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    //config.toolbar_Basic =
    //['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink'];

    //config.extraPlugins = 'image2,html5audio,html5video,videodetector';
    config.versionCheck = false;
    config.disableNativeSpellChecker = true;
    config.removePlugins = 'a11ychecker,indentblock';
    config.indentOffset = 0.1;
    config.indentUnit = 'in'; 
};
