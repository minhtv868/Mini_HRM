CKEDITOR.plugins.add('selectnews',
    {
        init: function (editor) {

            var pluginName = 'selectnews', newsTypeId = 0, searchByDate = 2;

            editor.ui.addButton(pluginName,
                {
                    label: 'Chèn bài viết gợi ý',
                    command: 'OpenWindowSelectNews',
                    icon: CKEDITOR.plugins.getPath(pluginName) + 'image.png'
                });

            editor.addCommand('OpenWindowSelectNews', {
                exec: function (editor) {

                    modalNewsEditorGet(editor, this, '', newsTypeId, searchByDate);
                }
            });
        }
    });