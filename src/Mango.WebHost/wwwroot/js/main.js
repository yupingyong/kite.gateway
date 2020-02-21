//判断用户是否登录的标识
var isUserLogin = false;
//用户点赞操作
function AddUserPlus(objectId, title, plusType, toUserId, id,htmlId) {
    if (isUserLogin) {
        $.ajax({
            type: 'post',
            url: '/Main/AddUserPlus',
            data: 'objectId=' + objectId + '&title=' + title + '&plusType=' + plusType + '&toUserId=' + toUserId+'&id='+id,
            success: function (res) {
                var plus = parseInt($(htmlId).text());
                if (res == 0) {
                    layer.msg("点赞异常,请稍后尝试");
                }
                else if (res == 1) {
                    plus = plus + 1;
                    $(htmlId).text(plus);
                    layer.msg("点赞成功");
                }
                else if (res == -1) {
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