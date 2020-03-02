//主题点赞
function AddThemePlus(themeId, title, toAccountId, htmlId) {
    if (isAccountLogin) {
        $.ajax({
            type: 'post',
            url: '/Docs/Plus/Theme',
            data: 'ThemeId=' + themeId + '&Title=' + title + '&ToAccountId=' + toAccountId,
            success: function (res) {
                console.log(res);
                var plus = parseInt($(htmlId).text());
                if (res.data == 0) {
                    layer.msg("点赞异常,请稍后尝试");
                }
                else if (res.data == 1) {
                    plus = plus + 1;
                    $(htmlId).text(plus);
                    layer.msg("点赞成功");
                }
                else if (res.data == -1) {
                    plus = plus - 1;
                    $(htmlId).text(plus);
                    layer.msg("取消点赞成功");
                }
            }
        });
    }
    else {
        layer.msg("请您先登录");
    }
}
//文档点赞
function AddDocsPlus(docsId,themeId, title, toAccountId, htmlId) {
    if (isAccountLogin) {
        $.ajax({
            type: 'post',
            url: '/Docs/Plus/Docs',
            data: 'DocsId=' + docsId + '&ThemeId=' + themeId + '&Title=' + title + '&ToAccountId=' + toAccountId,
            success: function (res) {
                console.log(res);
                var plus = parseInt($(htmlId).text());
                if (res.data == 0) {
                    layer.msg("点赞异常,请稍后尝试");
                }
                else if (res.data == 1) {
                    plus = plus + 1;
                    $(htmlId).text(plus);
                    layer.msg("点赞成功");
                }
                else if (res.data == -1) {
                    plus = plus - 1;
                    $(htmlId).text(plus);
                    layer.msg("取消点赞成功");
                }
            }
        });
    }
    else {
        layer.msg("请您先登录");
    }
}