/*
 图片上传组件
 */
tinymce.PluginManager.add('imageupload', function (editor) {

    function selectLocalImages() {
        var dom = editor.dom;
        var input_f = $('<input type="file" name="file" accept="image/jpg,image/jpeg,image/png,image/gif">');
        input_f.on('change', function () {
            var form = $("<form/>",
                {
                    action: editor.settings.upload_image_url, //设置上传图片的路由，配置在初始化时
                    style: 'display:none',
                    method: 'post',
                    enctype: 'multipart/form-data'
                }
            );
            form.append(input_f);
            //ajax提交表单
            form.ajaxSubmit({
                beforeSubmit: function () {
                    return true;
                },
                success: function (data) {
                    editor.selection.setContent(dom.createHTML('img', { src: data }));
                }
            });
        });

        input_f.click();
    }

    editor.addCommand("mceUploadImageEditor", selectLocalImages);

    editor.ui.registry.addButton('imageupload', {
        icon: 'image',
        tooltip: '图片上传',
        onAction: selectLocalImages
    });

    editor.ui.registry.addMenuItem('imageupload', {
        icon: 'image',
        text: '图片上传',
        context: 'tools',
        onAction: selectLocalImages
    });
});