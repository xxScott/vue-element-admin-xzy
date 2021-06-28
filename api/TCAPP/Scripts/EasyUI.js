function format_state(value) {
    if (value == success_code) {
        return success_name;
    }
    else {
        return fail_name;
    }
}

function format_check(value) {
    if (value == "1") {
        return '<div class="icon-ok">&nbsp;</div>';
    }
    else if (value == "0") {
        return '<div class="icon-cancel">&nbsp;</div>';
    }
    else {
        return '';
    }
}

function config_dialog(id, title, icon) {
    $("#" + id).dialog({
        title: title,
        iconCls: icon,
        modal: true
    });
}

function config_window(id, title, icon) {
    $("#" + id).window({
        title: title,
        iconCls: icon,
        modal: true
    });
}

function messager_show(title, content) {
    $.messager.show({
        title: title,
        msg: content,
        timeout: 1000,
        height: 30,
        showType: 'show'
    });
}

function messager_slide(title, content) {
    $.messager.show({
        title: title,
        msg: content,
        timeout: 500,
        height: 30,
        showType: 'slide'
    });
}

function messager_fade(title, content) {
    $.messager.show({
        title: title,
        msg: content,
        timeout: 1000,
        height: 30,
        showType: 'fade'
    });
}
function messager_progress(title, content) {
    var win = $.messager.progress({
        title: title,
        msg: content
    });
    setTimeout(function() {
        $.messager.progress('close');
    }, 5000)
}

$.extend($.fn.validatebox.defaults.rules, {
    confirmPass: {
        validator: function(value, param) {
            var pass = $(param[0]).passwordbox('getValue');
            return value == pass;
        },
        message: '两次输入的密码不一致'
    }
})

function loading() {
    //$("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: "100%", height: $(window).height() }).appendTo("body");
    //$("<div class=\"datagrid-mask-msg\"></div>").html("正在处理，请稍候...").appendTo("body").css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2 });
    $('#loading-mask').show();
}
function loaded() {
    //$(".datagrid-mask").remove();
    //$(".datagrid-mask-msg").remove();
    $('#loading-mask').hide();
}

//全选反选
//参数:selected:传入this,表示当前点击的组件
//treeMenu:要操作的tree的id；如：id="userTree"
function treeChecked(selected, treeMenu) {
    var roots = $('#' + treeMenu).tree('getRoots'); //返回tree的所有根节点数组
    if (selected) {
        for (var i = 0; i < roots.length; i++) {
            var node = $('#' + treeMenu).tree('find', roots[i].id); //查找节点
            $('#' + treeMenu).tree('check', node.target); //将得到的节点选中
        }
    } else {
        for (var i = 0; i < roots.length; i++) {
            var node = $('#' + treeMenu).tree('find', roots[i].id);
            $('#' + treeMenu).tree('uncheck', node.target);
        }
    }
}

/**  
 * layout方法扩展  
 * @param {Object} jq  
 * @param {Object} region  
 */
$.extend($.fn.layout.methods, {
    /**  
* 面板是否存在和可见  
 * @param {Object} jq  
 * @param {Object} params  
 */
    isVisible: function (jq, params) {
        var panels = $.data(jq[0], 'layout').panels;
        var pp = panels[params];
        if (!pp) {
            return false;
        }
        if (pp.length) {
            return pp.panel('panel').is(':visible');
        } else {
            return false;
        }
    },
    /**  
     * 隐藏除某个region，center除外。  
     * @param {Object} jq  
     * @param {Object} params  
     */
    hidden: function (jq, params) {
        return jq.each(function () {
            var opts = $.data(this, 'layout').options;
            var panels = $.data(this, 'layout').panels;
            if (!opts.regionState) {
                opts.regionState = {};
            }
            var region = params;
            function hide(dom, region, doResize) {
                var first = region.substring(0, 1);
                var others = region.substring(1);
                var expand = 'expand' + first.toUpperCase() + others;
                if (panels[expand]) {
                    if ($(dom).layout('isVisible', expand)) {
                        opts.regionState[region] = 1;
                        panels[expand].panel('close');
                    } else if ($(dom).layout('isVisible', region)) {
                        opts.regionState[region] = 0;
                        panels[region].panel('close');
                    }
                } else {
                    panels[region].panel('close');
                }
                if (doResize) {
                    $(dom).layout('resize');
                }
            };
            if (region.toLowerCase() == 'all') {
                hide(this, 'east', false);
                hide(this, 'north', false);
                hide(this, 'west', false);
                hide(this, 'south', true);
            } else {
                hide(this, region, true);
            }
        });
    },
    /**  
     * 显示某个region，center除外。  
     * @param {Object} jq  
     * @param {Object} params  
     */
    show: function (jq, params) {
        return jq.each(function () {
            var opts = $.data(this, 'layout').options;
            var panels = $.data(this, 'layout').panels;
            var region = params;

            function show(dom, region, doResize) {
                var first = region.substring(0, 1);
                var others = region.substring(1);
                var expand = 'expand' + first.toUpperCase() + others;
                if (panels[expand]) {
                    if (!$(dom).layout('isVisible', expand)) {
                        if (!$(dom).layout('isVisible', region)) {
                            if (opts.regionState[region] == 1) {
                                panels[expand].panel('open');
                            } else {
                                panels[region].panel('open');
                            }
                        }
                    }
                } else {
                    panels[region].panel('open');
                }
                if (doResize) {
                    $(dom).layout('resize');
                }
            };
            if (region.toLowerCase() == 'all') {
                show(this, 'east', false);
                show(this, 'north', false);
                show(this, 'west', false);
                show(this, 'south', true);
            } else {
                show(this, region, true);
            }
        });
    }
});
