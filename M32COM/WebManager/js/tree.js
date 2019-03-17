// 保存选中的数据
var selectData;

//为模态对话框Add拖拽

	
/**
 * 使用递归遍历json字符串，生成树结构
 * @param  {[type]} tData json字符串
 * @return {[type]}       Return树结构
 */
function loadTree(tData){
	var ul = $('<ul class="myul">');
	for(var i=0; i<tData.length; i++){
		 var  id=tData[i].id.split("_")[0];
	     id=tData[i].id.replace(id,"");
		 var menu="checkevent('checkmenu"+id+"')";
		 var li =tData[i].children != undefined?  $("<li class='li_all'>").appendTo(ul) : $("<li class='myli'>").appendTo(ul);
		 if(tData[i].children != undefined){
			 var check=tData[i].checked=="true"? "checked='true'":"";
			 $("<label class='labeltt' style='position:absolute;left: 15px;'><input onclick=\""+menu+"\" "+check+" type='checkbox'  id='"+tData[i].id+"' ><i>✓</i></label>").appendTo(li);
		 }
		 var node = $("<a class='tree-node'>").appendTo(li);
		 var div =tData[i].children == undefined?  $('<div class="div1">').appendTo(node) : $('<div class="div2">').appendTo(node);      
	 	 var icon = $('<i>').addClass("fa fa-file-text-o div3").appendTo(div);
		 if(tData[i].children == undefined){
			 var check=tData[i].checked=="true"? "checked='true'":"";
             var checkbox = $("<label class='labeltt'><input type='checkbox' "+check+"  onclick=\""+menu+"\"  id='"+tData[i].id+"' ><i>✓</i>"+tData[i].title+"</label><br/>").appendTo(div);
		 }else{
           var checkbox = $("<span>&nbsp;&nbsp;"+tData[i].title+"</span>").appendTo(div);
		 }
		//处理有子节点的
		if(tData[i].children != undefined){
			// Add图标样式
			icon.addClass('fa fa-minus-square-o');
			var ic = $('<i>').addClass('fa fa-folder-open-o');
			icon.after(ic).addClass('status');
			node.addClass('tree-node');
			// Add标记节点是否打开
			$('<input>').addClass('open').val(tData[i].open).css('display','none').appendTo(node);
			// 递归遍历子节点
			loadTree(tData[i].children).appendTo(li);
		} else{
			 var div = $("<div class='div_item'>").appendTo(node);
			 if(tData[i].btn!=undefined){
				 for(var j=0;j<tData[i].btn.length;j++){
					 var check=tData[i].btn[j].checked=="true"? "checked='true'":"";
					 var checkbox = $("<label class='labeltt'><input type=\"checkbox\"  "+check+"  onclick=\"checkevent('"+tData[i].btn[j].id+"')\" id='"+tData[i].btn[j].id+"' ><i>✓</i>"+tData[i].btn[j].title+"</label>").appendTo(div);
					 var div = $("<div class='div_item'>").appendTo(node);   
				 }
			 }
		}
	}
	return ul;
}
/**
 * 节点点击事件
 * @param  {[type]} box 存在菜单树的盒子
 */
//function nodeClick(box){
//	 $(".tree-node").find(".div_item:last").remove();
//     box.find('.tree-node').click(function(){
//		// 判断该节点是否开启
//		if($.trim($(this).find('.open').val()) == 'true'){
//			// 已开启，则关闭节点
//			$(this).next().fadeOut();
//			$(this).find('.open').val('false');
//			$(this).find('.status').removeClass('fa-minus-square-o').addClass('fa-plus-square-o');
//		} else{
//			// 开启前关闭节点下的所有节点
//			$(this).next().find('.tree-node').each(function(){
//				$(this).next().css('display','none');
//				$(this).find('.open').val('false');
//				$(this).find('.status').removeClass('fa-minus-square-o').addClass('fa-plus-square-o');
//			})

//			// 已关闭，则开启节点
//			$(this).find('.open').val('true');
//			$(this).find('.status').removeClass('fa-plus-square-o').addClass('fa-minus-square-o');
//			// 开启节点下的节点
			
//			$(this).next().fadeIn();
//		}
//	})
//}
////开启所有节点
//function load(obj){
//	var obj=obj!=undefined ? $(obj):obj;
//	$(obj).next().find('.tree-node').each(function(){
//		$(this).next().css('display','none');
//		$(this).find('.open').val('false');
//		$(this).find('.status').removeClass('fa-minus-square-o').addClass('fa-plus-square-o');
//		load(this);
//		$(".fa-minus-square-o").removeClass('fa-minus-square-o').addClass('fa-plus-square-o');
//	})
//	// 已关闭，则开启节点
//	$(this).find('.open').val('true');
//	$array=$(".div2 i");
//	for(var i=0;i<$array.length;i++){
//		if($array.eq(i).hasClass('fa-plus-square-o')){
//			$array.eq(i).removeClass('fa-plus-square-o').addClass('fa-minus-square-o');
//		}
//	}
//	$(this).find('.status').removeClass('fa-plus-square-o').addClass('fa-minus-square-o');
//	// 开启节点下的节点	
//	$(this).next().fadeIn();
//}

//开启所有节点
function load(obj) {
    var obj = obj != undefined ? $(obj) : obj;
    $(obj).next().find('.tree-node').each(function () {
        //$(this).next().css('display', 'none');
        $(this).find('.open').val('false');
        $(this).find('.status').removeClass('fa-minus-square-o').addClass('fa-plus-square-o');
        load(this);
        $(".fa-minus-square-o").removeClass('fa-minus-square-o').addClass('fa-plus-square-o');
    })
    // 已关闭，则开启节点
    $(this).find('.open').val('true');
    $array = $(".div2 i");
    for (var i = 0; i < $array.length; i++) {
        if ($array.eq(i).hasClass('fa-plus-square-o')) {
            $array.eq(i).removeClass('fa-plus-square-o').addClass('fa-minus-square-o');
        }
    }
    $(this).find('.status').removeClass('fa-plus-square-o').addClass('fa-minus-square-o');
    // 开启节点下的节点	
    $(this).next().fadeIn();
}
/**
* 节点点击事件
* param  {[type]} box 存在菜单树的盒子
*/
function nodeClick(box) {
    $(".tree-node").find(".div_item:last").remove();
    box.find('.tree-node').click(function () {
        var id = $(this).attr("id").split("_")[1];
        $.getcommstr("/Expenditures/Index?types=" + id, "", "#list");
        $("#listtree span").removeClass("current");
        $(this).find("span").addClass("current");
        // 判断该节点是否开启
        if ($.trim($(this).find('.open').val()) == 'true') {
            // 已开启，则关闭节点
            $(this).next().fadeOut();
            $(this).next().find(".tree-node").fadeOut();
            $(this).find('.open').val('false');
            $(this).find('.status').removeClass('fa-minus-square-o').addClass('fa-plus-square-o');
        } else {
            // 开启前关闭节点下的所有节点
            $(this).next().find('.tree-node').each(function () {
                $(this).next().fadeOut();
                $(this).next().find(".tree-node").fadeOut();
                $(this).find('.open').val('false');
                $(this).find('.status').removeClass('fa-minus-square-o').addClass('fa-plus-square-o');
            })
            // 已关闭，则开启节点
            $(this).find('.open').val('true');
            $(this).find('.status').removeClass('fa-plus-square-o').addClass('fa-minus-square-o');
            // 开启节点下的节点
            $(this).next().fadeIn();
            $(this).next().find(".tree-node").fadeIn();
        }
    })
}

 function checkevent(obj) {
      var id = obj.replace(obj.split("_")[0]+"_", "");
      id = id.replace(obj.split("_")[1], "");
      if (obj.split("_")[1] == "btnmenu") {
          if ($("#"+obj).prop("checked")) {
        	  $("#checkmenu"+id).prop("checked", true);
              $("#"+obj).prop("checked", true);
          }
		  if ($("[id$=_btnmenu" + id+"]:checked").length==0) {
              $("#checkmenu" + id).prop("checked", false);
          }
              var $parent = ("checkmenu" + id).split("_");
			  var s="";
			  for (var j = $parent.length - 1; j >1; j--) {
				  s = s == "" ? "_" + $parent[j] : "_" + $parent[j] + s;
				  var temp=id.substr(0,id.lastIndexOf(s));
				  var tt = "checkmenu" + temp;
				  if ($("[id^="+tt+"_]:checked").length==0){
					   $("#" + tt).prop("checked", false);
				  } else {
					   $("#" + tt).prop("checked", true);
				  }
			  }
      }
      else {
              if (obj.split("_")[0] == "checkmenu") {
            	  id = obj.replace(obj.split("_")[0], "");
            	  //alert(obj+"   "+id);
                  if ($("#" + obj).prop('checked')) {
                	  $("#" + obj).prop("checked", true);
                      //$("#checkshowbtn" + id).prop("checked", true);
                      //$("#checkaddbtn" + id).prop("checked", true);
                      //$("#checkeditbtn" + id).prop("checked", true);
                      //$("#checkdelbtn" + id).prop("checked", true);
                      $("[id$=_btnmenu" + id+"]").prop("checked", true);
                      $("[id*=_btnmenu" + id+"_]").prop("checked", true);
                      $("input[id*=checkmenu" + id + "_]").prop("checked", true);
                      //$("input[id^=checkshowbtn" + id + "_]").prop("checked", true);
                      //$("input[id^=checkaddbtn" + id + "_]").prop("checked", true);
                      //$("input[id^=checkeditbtn" + id + "_]").prop("checked", true);
                      //$("input[id^=checkdelbtn" + id + "_]").prop("checked", true);
                  } else {
                	  $("#" + obj).prop("checked", false);
                      //$("#checkshowbtn" + id).prop("checked", false);
                      //$("#checkaddbtn" + id).prop("checked", false);
                      //$("#checkeditbtn" + id).prop("checked", false);
                      //$("#checkdelbtn" + id).prop("checked", false);
                	  $("[id$=_btnmenu" + id+"]").prop("checked", false);
                      $("[id*=_btnmenu" + id+"_]").prop("checked", false);
                      $("input[id*=checkmenu" + id + "_]").prop("checked", false);
                      //$("input[id^=checkshowbtn" + id + "_]").prop("checked", false);
                      //$("input[id^=checkmenu" + id + "_]").prop("checked", false);
                      //$("input[id^=checkaddbtn" + id + "_]").prop("checked", false);
                      //$("input[id^=checkeditbtn" + id + "_]").prop("checked", false);
                      //$("input[id^=checkdelbtn" + id + "_]").prop("checked", false);
                  }
              }
			  var $parent = obj.split("_");
              var temp = ""; var s = "";
              for (var j = $parent.length - 1; j >1; j--) {
                  s = s == "" ? "_" + $parent[j] : "_" + $parent[j] + s;
				  var tt=obj.substr(0,obj.lastIndexOf(s));
                  //var tt = obj.replace(s, "");
				  //alert(tt+"    "+$("[id^=" + tt + "_]:checked").length);
                  if (!$("#" + obj).prop('checked')) {
                      //alert(obj + "   " + s + "    " + tt + "   " + $("[id^=" + tt + "_]").length);
                      if ($("[id^=" + tt + "_]:checked").length == 0) {
                          $("#" + tt).prop("checked", false);
                      }
                  } else {
                      $("#" + tt).prop("checked", true);
                  }
              }
          }
}
function editrole(uid) {
    var $array = $("[id^=checkmenu_]:checked");
    var str = "";
    if ($array.length > 0) {
        for (var i = 0; i < $array.length; i++) {
            var id = $array.eq(i).attr("id");
            id = id.replace(id.split("_")[0], "");
            if ($("[id^=" + $array.eq(i).attr("id")+ "_]").length == 0) {
            	str += "checkmenu" + id;
                var $btn=$("[id$=_btnmenu" + id+"]:checked");
                for(var j=0;j<$btn.length;j++){
                	  str +="|"+$btn.eq(j).attr("id");
                }
                str+=",";
            }else{
            	 str += "checkmenu" + id+",";
            }
        }
        str=str!=""? str.substr(0,str.length-1):str;
        if (str == "") {
                $.alert("您没有选中任何菜单！");
        } else {
        	    $.setcommstr("/role/MenuRelationRoleForm", "{id:'" + uid + "',title:'" + str + "'}","/role/index");
        }
    } else {
           $.alert("抱歉，您没有选中任何菜单！");
    }
}