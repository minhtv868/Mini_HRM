(function () {
    CKEDITOR.plugins.add('toc', {

        // Register the icons. They must match command names.
        icons: 'tocinsert,tocdelete',
        lang: ['de', 'en', 'vi'],
        // The plugin initialization logic goes inside this method.
        init: function (editor) {

            // Define the editor command that inserts a timestamp.
            editor.addCommand('insertToc', {

                allowedContent: '*[id,name,class]{margin-left}',
                // Define the function that will be fired when the command is executed.
                exec: function (editor) {
                    //remove already exisiting tocs...
                    if (editor.document.getById("tableOfContents"))
                        editor.document.getById("tableOfContents").remove();

                    var text = editor.getData();;
                    var toc = "";
                    var level = text.includes("<h1") ? 0 : text.includes("<h2") ? 1 : 2;

                    text = text.replace(/<h([\d])(.*\s?.*)>(?!\<)(.+?)(?=\<\/.+?(?=))<\/(.*)h([\d])>/gmi,
                        function (str, openLevel, attributes1, titleText, attributes2, closeLevel) {
                            if (openLevel != closeLevel) {
                                c.log(openLevel)
                                return str + ' - ' + openLevel;
                            }

                            if (titleText && titleText.trim() == 'Nguồn tham khảo') {
                                return;
                            }

                            if (openLevel > level) {
                                toc += (new Array(openLevel - level + 1)).join("<ol>");
                            } else if (openLevel < level) {
                                toc += (new Array(level - openLevel + 1)).join("</ol>");
                            }

                            level = parseInt(openLevel);
                            var toctitleText = titleText.replace("&nbsp;", " ");
                            toctitleText = toctitleText.replace(/^\s*(?:[\d]*?([\da-zA-Z]*[.\/)\\])*|•|[\da-zA-Z]?([.][\da-zA-Z]?.))\s+/, "");
                            var anchor = ChangeToSlug(toctitleText);
                            toc += "<li><a href=\"#" + anchor + "\">" + toctitleText + "</a></li>";

                            return "<h" + openLevel + " id =\"" + anchor + "\"" + attributes1 + ">" + titleText + "</" + attributes2 + "h" + closeLevel + ">";
                        }
                    );
                    if (level) {
                        toc += (new Array(level + 1)).join("</ol>");
                    }

                    var wrapper = document.createElement("div");
                    wrapper.classList.add("wrapper")
                    wrapper.setAttribute("id", "tableOfContents");
                    var tablelabel = document.createElement("span");
                    tablelabel.innerHTML = "Mục lục";
                    tablelabel.id = "toc-title"
                    wrapper.appendChild(tablelabel)
                    var content = document.createElement("div");
                    content.classList.add("content")
                    content.id = "toc-content"
                    content.innerHTML = toc;
                    wrapper.appendChild(content)
                    text = wrapper.outerHTML + text;
                    editor.setData(text);
                }
            });

            // Define the editor command that inserts a timestamp.
            editor.addCommand('deleteToc', {

                // Define the function that will be fired when the command is executed.
                exec: function (editor) {
                    //remove already exisiting tocs...
                    if (editor.document.getById("tableOfContents"))
                        editor.document.getById("tableOfContents").remove();
                }
            });

            // Create the toolbar button that executes the above command.
            editor.ui.addButton('tocinsert', {
                label: editor.lang.toc.tooltip,
                command: 'insertToc',
                icon: this.path + 'icons/tocinsert.png',
                toolbar: 'links'
            });

            // Create the toolbar button that executes the above command.
            editor.ui.addButton('tocdelete', {
                label: "Xóa mục lục",
                command: 'deleteToc',
                icon: this.path + 'icons/tocdelete.png',
                toolbar: 'links'
            });
        }
    }
    )
})
    ();

function ChangeToSlug(text) {
    var title, slug;

    //Lấy text từ thẻ input title 
    title = text;

    //Đổi chữ hoa thành chữ thường
    slug = title.toLowerCase();

    //Đổi ký tự có dấu thành không dấu
    slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
    slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
    slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
    slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
    slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
    slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
    slug = slug.replace(/đ/gi, 'd');
    //Xóa các ký tự đặt biệt
    slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
    //Đổi khoảng trắng thành ký tự gạch ngang
    slug = slug.replace(/ /gi, "-");
    //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
    //Phòng trường hợp người nhập vào quá nhiều ký tự trắng
    slug = slug.replace(/\-\-\-\-\-/gi, '-');
    slug = slug.replace(/\-\-\-\-/gi, '-');
    slug = slug.replace(/\-\-\-/gi, '-');
    slug = slug.replace(/\-\-/gi, '-');
    //Xóa các ký tự gạch ngang ở đầu và cuối
    slug = '@' + slug + '@';
    slug = slug.replace(/\@\-|\-\@|\@/gi, '');
    //In slug ra textbox có id “slug”
    return slug;
}
