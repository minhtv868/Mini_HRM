CKEDITOR.plugins.add('selectimages',
    {
        init: function (editor) {

            var pluginName = 'selectimages';

            editor.ui.addButton(pluginName,
                {
                    label: 'Chèn nhiều ảnh',
                    command: 'OpenWindowSelectImages',
                    icon: CKEDITOR.plugins.getPath(pluginName) + 'image.png'
                });

            editor.addCommand('OpenWindowSelectImages', {
                exec: function (editor) {

                    modalMediaEditorGet(editor, this);
                }
            });
        }
    });