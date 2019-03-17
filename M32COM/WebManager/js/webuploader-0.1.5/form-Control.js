var uploadArray = new Array();//记录所有初始化过的上传组件
var uploadIsInit = new Array();//记录组件是否已经被初始化
var allMaxSize = 300;
var FileHandle =
   {
       initOpts: function (options) {
           var defaults = {
               onAllComplete: function (event) { }, // 当所有file都上传后执行的回调函数
               onComplete: function (event) { },// 每上传一个file的回调函数
               onUploadSuccess: function (file, response) {
                   var oPath_obj = document.getElementById("WUOPath_" + file.id);
                   var tPath_obj = document.getElementById("WUTPath_" + file.id);
                   var f_Description = document.getElementById("WUTDescription_" + file.id);
                   if (oPath_obj != null) {
                       $("#WUOPath_" + file.id).val($("#WUimg_" + file.id).attr("filevalue"));
                       $("#WUimg_" + file.id).attr("src", response.OPath);
                       $("#WUimg_" + file.id).show();
                   }
               },//每个文件上传 Success调用
               //如果有：点击了Edit图片时要执行的方法
               onEditImage: function () { },
               innerOptions: {},
               fileNumLimit: undefined,
               multiple: options.multiple,
               fileSizeLimit: allMaxSize * 1024 * 1024,//undefined,
               fileSingleSizeLimit: allMaxSize * 1024 * 1024,
               chunkSize: allMaxSize * 1024 * 1024,
               FileName: "",
               FileType: "",
               imgIDCard: false,//是否识图片InfoMation
               Startbtn: "",
               FileAccept: {},
               OPath: "",//原路径（Name）
               TPath: "",//缩略图路径（如果有【Name】）
               FileSort: "",//图片的排序字段
               FDescription: "",//文件描述(如果有)【Name】
               EnableEdit: false,//是否启用Edit
               filetips: "",//提示内容，类似于（建议上传600*300之类的）
               defaultUrl: "",//Edit时显示的图片路径（只正对于单个文件）
               imgStyle: ""
           };
           var opts = $.extend({}, defaults, options);
           var fileType = opts.FileType;
           var FileAccept = {};
           FileAccept = {
               title: '图片文件',
               extensions: 'jpg,jpeg,bmp,png',
               mimeTypes: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/msword,application/vnd.ms-excel,application/x-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-powerpoint'
           };
           opts.FileAccept = FileAccept;
           return opts;
       },
       DisableSubmit: function () {
           $("body").find("button[type='submit']").attr("disabled", "disabled");
           $("body").find("button[type='submit']").removeClass("btn-default").addClass("btn-reset").addClass("disabled");
           $("body").find("input[type='submit']").attr("disabled", "disabled");
           $("body").find("input[type='submit']").removeClass("btn-default").addClass("btn-reset").addClass("disabled");
       },
       EnableSubmit: function () {

           $("body").find("button[type='submit']").removeAttr("disabled");
           $("body").find("button[type='submit']").removeClass("btn-reset").removeClass("disabled").addClass("btn-default");
           $("body").find("input[type='submit']").removeAttr("disabled");
           $("body").find("input[type='submit']").removeClass("btn-reset").removeClass("disabled").addClass("btn-default");
       },
       delFile: function (uploader, fileid, identity) {
           var fileObj = uploader.getFile(fileid);
           $.post("/UpLoad/delimg", { 'filename': fileid, 'filepathname': $("#WUimg_" + fileid).attr("filevalue") }, function (returndata) {
               if (returndata.result == "true") {
                   $("ul.forminfo").find("#child_" + fileid).remove();
                   $.alert("Delete Success！");
               }
               $ele.parent().remove();
           });
           if (fileObj) {
               uploader.removeFile(fileObj, true);
           }
           $("ul.forminfo").find("#child_" + fileid).remove();

           FileHandle.DelCorrect(identity);
           FileHandle.checkNotCompleted();
       },
       delFileEdit: function (fileid, url) {
           var $ele = $(this);
           //$("#WUimg_" + fileid).attr("filevalue");
           $.post("/UpLoad/delimg", { 'filename': fileid, 'filepathname': url }, function (returndata) {
               if (returndata.result == "true") {
                   $("ul.forminfo").find("#child_" + fileid).remove();
                   $.alert("Delete Success！");
               }
               $ele.parent().remove();
           });
           FileHandle.DelCorrect(identity);
       },
       EditFile: function (fileid, FDescription) {

       },
       DelCorrect: function (identity) {
           //修正Delete后的List
           var hdInputParent = $("div[filelist='" + identity + "']").find(".dz-preview");
           $(hdInputParent).each(function (p) {
               var $parentObj = $(this);
               var hiddenObj = $(this).find("input[type='hidden']");
               $(hiddenObj).each(function (k) {
                   var $inputObj = $(this);
                   var oldName = $inputObj.attr("name");
                   //var newName = oldName.replace(identity + "["+/(^\d$)/+"]", "123");
                   if (oldName.toUpperCase().indexOf(identity.toUpperCase()) >= 0) {
                       var newName = oldName.replace(/\d+/g, p);
                       $inputObj.attr("name", newName);
                   }
                   // $inputObj.attr("name",)
               });
           });

       },
       checkNotCompleted: function () {
           var isOk = 0;//表示所有文件已经完成上传
           for (var fileState = 0; fileState < uploadArray.length; fileState++) {
               var queueNum = uploadArray[fileState].getStats().queueNum;
               if (queueNum > 0) {
                   isOk++;
               }
           }
           if (isOk > 0) {
               FileHandle.DisableSubmit();
           } else {
               FileHandle.EnableSubmit();
           }
       },
       DefaultUrl: function (contentId, opts) {

           if (opts.multiple.toString() !== "true") {
               if (opts.defaultUrl != null) {
                   if (opts.defaultUrl !== "") {
                       var FileHtml = "";
                       $("#up_father_" + contentId).html("");

                       if (opts.OPath != "") {
                           var temp = $("WUimg_" + contentId).attr("filevalue");
                           FileHtml += "<input type=\"hidden\" id=\"WUOPath_" + contentId + "\" title=\"" + opts.FileName + "\" name=\"" + opts.OPath + "\" value=\"" + opts.defaultUrl + "\"/>";
                       }
                       if (opts.TPath != "") {
                           FileHtml += "<input type=\"hidden\" id=\"WUTPath_" + contentId + "\" name=\"" + opts.TPath + "\"/>";
                       }
                       if (opts.FDescription != "") {
                           FileHtml += "<input type=\"hidden\" id=\"WUTDescription_" + contentId + "\" name=\"" + opts.FDescription + "\"/>";
                       }
                       if (opts.FileSort != "") {
                           FileHtml += "<input type=\"hidden\" id=\"WUSort_" + contentId + "\" name=\"" + opts.FileSort + "\"/>";
                       }
                       var lens = opts.defaultUrl.toLowerCase().split(".");
                       var filenames = lens[lens.length - 1];// '<div class="dz-filename"><span data-dz-name="">' + opts.FileName + '</span><span style="display: block;margin-top: 2px;color: #128C11;"></span><span></span></div><div class="dz-size" data-dz-size=""><strong></strong></div>';
                       filenames = filenames.substr(filenames.length - 1, 1) == "x" ? filenames.substr(0, filenames.length - 1) : filenames;
                       var html = "<div id=\"child_" + contentId + "\" class=\"dz-preview dz-file-preview\">"
                           + "<div class=\"dz-details\">"
                           + (opts.FileName.toLowerCase().indexOf("jpg") >= 0 || opts.FileName.toLowerCase().indexOf("bmp") >= 0 || opts.FileName.toLowerCase().indexOf("png") >= 0 || opts.FileName.toLowerCase().indexOf("jpeg") >= 0 ? "<img style='display:block;' data-dz-thumbnail=\"\" id=\"WUimg_" + contentId + "\" src=\"" + opts.defaultUrl + "\">" : '<div class="dz-filename"><span data-dz-name="">' + opts.FileName + '</span><span style="display: block;margin-top: 2px;color: #128C11;"></span><span></span></div><div class="dz-size" data-dz-size=""><strong></strong></div><img data-dz-thumbnail="" id="WUimg_WU_FILE_' + contentId + '" src="/images/' + filenames + '.jpg" title="' + opts.defaultUrl + '" filevalue="' + opts.defaultUrl + '" style="display: inline;">')
                           + "</div>"
                           + "<div class=\"tools\">" + (!opts.EnableEdit ? "" : "<a  id=\"WUFileEdit_" + contentId + "\">Edit</a>&nbsp;&nbsp;") + "<a onclick=\"FileHandle.delFileEdit('" + contentId + "','" + opts.defaultUrl + "');\" id=\"WUFileDel_" + contentId + "\" style=\"cursor: pointer\" >Delete</a></div>"
                           + FileHtml
                           + "</div>";
                       $("#up_father_" + contentId + "").append(html);
                   } else {
                       $("#up_father_" + contentId).html("");
                   }
               } else {
                   $("#up_father_" + contentId).html("");
               }
           }
       }
   };
(function ($, window) {
    var Null_Convert = function (val, defval) {
        try {
            if (val == null || val == "") {
                if (defval != null) {
                    if (defval != "") {
                        return defval;
                    }
                    else {
                        return "";
                    }
                }
                else {
                    return "";
                }
            }
            else {
                return val;
            }
        }
        catch (e) {
            if (defval != null) {
                if (defval != "") {
                    return defval;
                }
                else {
                    return "";
                }
            }
            else {
                return "";
            }
        }

    }
    $.fn.def_select2 = function () {
        var $ThisObject = $(this);
        var empty_val = $(this).attr("select2-empty");
        if (empty_val != null) {
            if (empty_val != "") {
                $ThisObject.append("<option selected  value=\"\">" + empty_val + "</option>");
            }
        }
        var select_val = $(this).attr("select2-val");
        if (select_val != null) {
            if (select_val != "") {
                $(this).val(select_val);
            }
        }
        $ThisObject.select2({
            placeholder: "\u8bf7\u9009\u62e9...",
            allowClear: true

        });
    }
    $.fn.ajax_select2 = function (param, funchange) {
        var url = $(this).attr("select2-url");
        var multiple = $(this).attr("multiple");
        var method = $(this).attr("select2-method");
        var change_to = $(this).attr("change-to");

        var select2_val = $(this).attr("select2-val");
        var empty_val = $(this).attr("select2-empty");
        var select2_change = $(this).attr("select2-change");
        var $ThisObject = $(this);
        if (method == null || method == "") { method = "get"; }
        $.AjaxSend(method, url, param, function (data) {

            $ThisObject.html("");
            // $ThisObject.val(null).trigger("change");
            data = data == null ? "" : data;
            if (empty_val != null) {
                if (empty_val != "" && multiple != "multiple") {
                    $ThisObject.append("<option selected  value=\"\">" + empty_val + "</option>");
                }
            }
            if (data != "") {
                if (data.group) {
                    var group_html = "";
                    for (var g = 0; g < data.group.length; g++) {
                        group_html += "<optgroup label=\"" + data.group[g].item + "\">";
                        for (var g_i = 0; g_i < data.group[g].selectItem.length; g_i++) {
                            if (("," + select2_val + ",").indexOf(("," + data.group[g].selectItem[g_i].value + ",")) >= 0) {


                                if (!data.group[g].selectItem[g_i].disabled) {
                                    group_html += "<option selected  value=\"" + data.group[g].selectItem[g_i].value + "\">" + data.group[g].selectItem[g_i].item + "</option>";
                                }
                                else {
                                    group_html += "<option selected disabled=\"disabled\" value=\"" + data.group[g].selectItem[g_i].value + "\">" + data.group[g].selectItem[g_i].item + "</option>";
                                }
                            }
                            else {
                                if (data.group[g].selectItem[g_i].value == "" && multiple == "multiple") {
                                    group_html += "<option disabled=\"disabled\" value=\"\">" + data.group[g].selectItem[g_i].item + "</option>";
                                }
                                else {
                                    if (!data.group[g].selectItem[g_i].disabled) {
                                        if (!data.group[g].selectItem[g_i].selected) {
                                            group_html += "<option  value=\"" + data.group[g].selectItem[g_i].value + "\">" + data.group[g].selectItem[g_i].item + "</option>";
                                        }
                                        else {
                                            group_html += "<option selected  value=\"" + data.group[g].selectItem[g_i].value + "\">" + data.group[g].selectItem[g_i].item + "</option>";
                                        }

                                    }
                                    else {
                                        if (!data[j].selected) {
                                            group_html += "<option  value=\"" + data.group[g].selectItem[g_i].value + "\">" + data.group[g].selectItem[g_i].item + "</option>";
                                        }
                                        else {
                                            group_html += "<option selected=\"" + data.group[g].selectItem[g_i].selected + "\" disabled=\"disabled\" value=\"" + data.group[g].selectItem[g_i].value + "\">" + data.group[g].selectItem[g_i].item + "</option>";
                                        }
                                    }
                                }
                            }
                        }
                        group_html += "</optgroup>";
                    }
                    $ThisObject.append(group_html);
                }
                else {
                    for (var j = 0; j < data.length; j++) {

                        if (("," + select2_val + ",").indexOf(("," + data[j].value + ",")) >= 0) {


                            if (!data[j].disabled) {
                                $ThisObject.append("<option selected  value=\"" + data[j].value + "\">" + data[j].item + "</option>");
                            }
                            else {
                                $ThisObject.append("<option selected disabled=\"disabled\" value=\"" + data[j].value + "\">" + data[j].item + "</option>");
                            }
                        }
                        else {

                            if (data[j].value == "" && multiple == "multiple") {
                                $ThisObject.append("<option disabled=\"disabled\" value=\"\">" + data[j].item + "</option>");
                            }
                            else {
                                if (!data[j].disabled) {
                                    if (!data[j].selected) {
                                        $ThisObject.append("<option  value=\"" + data[j].value + "\">" + data[j].item + "</option>");
                                    }
                                    else {
                                        $ThisObject.append("<option selected  value=\"" + data[j].value + "\">" + data[j].item + "</option>");
                                    }

                                }
                                else {
                                    if (!data[j].selected) {
                                        $ThisObject.append("<option  value=\"" + data[j].value + "\">" + data[j].item + "</option>");
                                    }
                                    else {
                                        $ThisObject.append("<option selected=\"" + data[j].selected + "\" disabled=\"disabled\" value=\"" + data[j].value + "\">" + data[j].item + "</option>");
                                    }
                                }
                            }
                        }
                    }
                }




            }
            if (change_to != null && change_to != "") {

                $ThisObject.unbind("change");

                $ThisObject.on("change", function () {

                    var changeval = $(this).val();
                    if (change_to.indexOf(",") >= 0) {
                        for (var i = 0; i < change_to.split(",").length; i++) {

                            $("#" + change_to.split(",")[i]).ajax_select2({ "parentId": changeval });
                        }
                    }
                    else {
                        $("#" + change_to).ajax_select2({ "parentId": changeval });
                    }
                    if (typeof (funchange) == "function") {
                        funchange($ThisObject);
                    }
                    if (select2_change === "true") {
                        select2change($ThisObject);
                    }
                    // }
                });


            }
            $ThisObject.trigger("change");
            //初始化select2组件，统一初始化有问题
            if ($ThisObject.select2) {
                $ThisObject.select2({
                    placeholder: "\u8bf7\u9009\u62e9...",
                    allowClear: true

                });
            }


        }, "json");

    }
    $.fn.ajax_CheckBox = function (param, backfun) {
        var url = $(this).attr("ck-url");
        var method = $(this).attr("ck-method");
        // var change_to = $(this).attr("change-to");
        var ck_val = $(this).attr("ck-val");
        var ck_name = $(this).attr("ck-name");
        var ck_style = $(this).attr("ck-style");

        // $("#" + ck_name).remove();
        var $ThisObject = $(this);
        ck_style = ck_style == null ? "" : ck_style;
        if (method == null || method == "") { method = "get"; }
        $.AjaxSend(method, url, param, function (data) {

            //$ThisObject.html("");
            data = data == null ? "" : data;

            if (data != "") {

                for (var j = 0; j < data.length; j++) {
                    if (("," + ck_val + ",").indexOf(("," + data[j].value + ",")) >= 0) {


                        if (!data[j].disabled) {
                            if (j == 0) {
                                if ($ThisObject.find("input[type=checkbox]").length != 0) {

                                    $ThisObject.find("input[type=checkbox]").eq(0).attr("checked", "checked");
                                    $ThisObject.find("input[type=checkbox]").eq(0).val(data[j].value);
                                    $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                }
                                else {

                                    $ThisObject.append("<input  type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\" checked class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }
                            }
                            else {
                                $ThisObject.append("<input type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\" checked class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                            }
                        }
                        else {
                            if (j == 0) {
                                if ($ThisObject.find("input[type=checkbox]").length != 0) {
                                    $ThisObject.find("input[type=checkbox]").eq(0).attr("checked", "checked");
                                    $ThisObject.find("input[type=checkbox]").eq(0).attr("disabled", "disabled");
                                    $ThisObject.find("input[type=checkbox]").eq(0).val(data[j].value);
                                    $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                }
                                else {

                                    $ThisObject.append("<input  type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\" disabled checked class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }
                            }
                            else {
                                $ThisObject.append("<input type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\" disabled checked class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                            }

                        }
                    }
                    else {

                        if (!data[j].disabled) {
                            if (!data[j].selected) {

                                if (j == 0) {
                                    if ($ThisObject.find("input[type=checkbox]").length != 0) {
                                        $ThisObject.find("input[type=checkbox]").eq(0).val(data[j].value);
                                        $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                    }
                                    else {

                                        $ThisObject.append("<input  type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\"  class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"> <label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                    }
                                }
                                else {
                                    $ThisObject.append("<input type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\"  class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"> <label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }
                            }
                            else {
                                if (j == 0) {
                                    if ($ThisObject.find("input[type=checkbox]").length != 0) {
                                        $ThisObject.find("input[type=checkbox]").eq(0).attr("checked", "checked");
                                        $ThisObject.find("input[type=checkbox]").eq(0).val(data[j].value);
                                        $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                    }
                                    else {

                                        $ThisObject.append("<input  type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\" checked class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"> <label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                    }
                                }
                                else {
                                    $ThisObject.append("<input type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\" checked class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"> <label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }

                            }

                        }
                        else {
                            if (!data[j].selected) {
                                if (j == 0) {
                                    if ($ThisObject.find("input[type=checkbox]").length != 0) {
                                        $ThisObject.find("input[type=checkbox]").eq(0).val(data[j].value);
                                        $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                    }
                                    else {

                                        $ThisObject.append("<input  type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\"  class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                    }
                                }
                                else {
                                    $ThisObject.append("<input type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\"  class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }
                            }
                            else {
                                if (j == 0) {
                                    if ($ThisObject.find("input[type=checkbox]").length != 0) {
                                        $ThisObject.find("input[type=checkbox]").eq(0).attr("disabled", "disabled");
                                        $ThisObject.find("input[type=checkbox]").eq(0).val(data[j].value);
                                        $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                    }
                                    else {
                                        $ThisObject.append("<input type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\" disabled class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                    }
                                }
                                else {
                                    $ThisObject.append("<input type=\"checkbox\" id=\"" + ck_name + "_" + j + "\" name=\"" + ck_name + "\" disabled class=\"form-control\" data-checkbox=\"icheckbox_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + ck_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }

                            }
                        }

                    }
                }

            }
            if (ck_style == "dt") {
                var thisHtml = $ThisObject.html();
                $ThisObject.html("<div class=\"rule-multi-checkbox\">" + thisHtml + "</div>");
                $ThisObject.ruleMultiCheckbox();
            }
            else {
                //初始化Icheck组件
                $ThisObject.find("input[type=checkbox]").iCheck({
                    checkboxClass: 'icheckbox_square-orange',
                    radioClass: 'iradio_square-blue',
                    increaseArea: '20%'
                });
                $ThisObject.find("div").addClass("validates");
                $ThisObject.find(".validates input[type=checkbox]").on('ifChecked', function (event) { //如果是选中，点击后则为不选中 
                    if ($("form").validate != null) {
                        $("form").validate().element($("#" + this.id));
                    }
                });
                $ThisObject.find(".validates input[type=checkbox]").on('ifUnchecked', function (event) { //如果是选中，点击后则为不选中 
                    if ($("form").validate != null) {
                        $("form").validate().element($("#" + this.id));
                    }

                });
            }
            if (typeof (backfun) == "function") {
                backfun();
            }
        }, "json");

    }
    $.fn.ajax_Radio = function (param) {
        var url = $(this).attr("radio-url");
        var method = $(this).attr("radio-method");
        var radio_val = $(this).attr("radio-val");
        var radio_name = $(this).attr("radio-name");
        var radio_style = $(this).attr("radio-style");
        var $ThisObject = $(this);
        radio_style = radio_style == null ? "" : radio_style;

        if (method == null || method == "") { method = "get"; }
        $.AjaxSend(method, url, param, function (data) {

            //$ThisObject.html("");
            data = data == null ? "" : data;

            if (data != "") {

                for (var j = 0; j < data.length; j++) {
                    if (("," + radio_val + ",").indexOf(("," + data[j].value + ",")) >= 0) {


                        if (!data[j].disabled) {
                            if (j == 0) {
                                if ($ThisObject.find("input[type=radio]").length != 0) {
                                    $ThisObject.find("input[type=radio]").eq(0).attr("checked", "checked");
                                    $ThisObject.find("input[type=radio]").eq(0).val(data[j].value);
                                    $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                }
                                else {
                                    $ThisObject.append("<input type=\"radio\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\" checked class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }
                            }
                            else {
                                $ThisObject.append("<input type=\"radio\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\" checked class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                            }
                        }
                        else {
                            if (j == 0) {
                                if ($ThisObject.find("input[type=radio]").length != 0) {
                                    $ThisObject.find("input[type=radio]").eq(0).attr("checked", "checked");
                                    $ThisObject.find("input[type=radio]").eq(0).attr("disabled", "disabled");
                                    $ThisObject.find("input[type=radio]").eq(0).val(data[j].value);
                                    $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                }
                                else {
                                    $ThisObject.append("<input type=\"radio\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\" disabled checked class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }
                            }
                            else {
                                $ThisObject.append("<input type=\"radio\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\" disabled checked class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                            }

                        }
                    }
                    else {

                        if (!data[j].disabled) {
                            if (!data[j].selected) {
                                if (j == 0) {
                                    if ($ThisObject.find("input[type=radio]").length != 0) {
                                        $ThisObject.find("input[type=radio]").eq(0).val(data[j].value);
                                        $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                    }
                                    else {
                                        $ThisObject.append("<input type=\"radio\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\"  class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"> <label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                    }
                                }
                                else {
                                    $ThisObject.append("<input type=\"radio\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\"  class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"> <label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }
                            }
                            else {
                                if (j == 0) {
                                    if ($ThisObject.find("input[type=radio]").length != 0) {
                                        $ThisObject.find("input[type=radio]").eq(0).attr("checked", "checked");
                                        $ThisObject.find("input[type=radio]").eq(0).val(data[j].value);
                                        $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                    }
                                    else {
                                        $ThisObject.append("<input type=\"checkbox\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\" checked class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"> <label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                    }
                                }
                                else {
                                    $ThisObject.append("<input type=\"checkbox\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\" checked class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"> <label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }

                            }

                        }
                        else {
                            if (!data[j].selected) {
                                if (j == 0) {
                                    if ($ThisObject.find("input[type=radio]").length != 0) {
                                        $ThisObject.find("input[type=radio]").eq(0).val(data[j].value);
                                        $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                    }
                                    else {
                                        $ThisObject.append("<input type=\"radio\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\"  class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                    }
                                }
                                else {
                                    $ThisObject.append("<input type=\"radio\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\"  class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }
                            }
                            else {
                                if (j == 0) {
                                    if ($ThisObject.find("input[type=radio]").length != 0) {
                                        $ThisObject.find("input[type=radio]").eq(0).attr("disabled", "disabled");
                                        $ThisObject.find("input[type=radio]").eq(0).val(data[j].value);
                                        $ThisObject.find("label").eq(0).text(" " + data[j].item);
                                    }
                                    else {
                                        $ThisObject.append("<input type=\"checkbox\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\" disabled class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                    }
                                }
                                else {
                                    $ThisObject.append("<input type=\"checkbox\" id=\"" + radio_name + "_" + j + "\" name=\"" + radio_name + "\" disabled class=\"form-control\" data-radio=\"radio_flat-blue\" value=\"" + data[j].value + "\"><label for=\"" + radio_name + "_" + j + "\"> " + data[j].item + "</label>");
                                }

                            }
                        }

                    }
                }

            }
            if (radio_style == "dt") {
                var thisHtml = $ThisObject.html();
                $ThisObject.html("<div class=\"rule-multi-radio\">" + thisHtml + "</div>");
                $ThisObject.ruleMultiRadio();
            }
            else {
                //初始化Icheck组件
                $ThisObject.find("input[type=radio]").iCheck({
                    checkboxClass: 'icheckbox_square-blue',
                    radioClass: 'iradio_square-orange',
                    increaseArea: '20%'
                });
                $ThisObject.find("div").addClass("validates");
                $ThisObject.find(".validates input[type=radio]").on('ifChecked', function (event) { //如果是选中，点击后则为不选中 
                    var this_Val = $(this).val();
                    if (typeof (radioChecked) == "function") {
                        radioChecked($ThisObject, this_Val);
                    }

                    if ($("form").validate != null) {
                        $("form").validate().element($("#" + this.id));
                    }
                });
                $ThisObject.find(".validates input[type=radio]").on('ifUnchecked', function (event) { //如果是选中，点击后则为不选中 
                    if ($("form").validate != null) {
                        $("form").validate().element($("#" + this.id));
                    }
                });
            }

        }, "json");
    }

    //全局变量
    var applicationPath = window.applicationPath === "" ? "" : window.applicationPath || "../..";
    function newGuid() {
        var guid = "";
        for (var i = 1; i <= 32; i++) {
            var n = Math.floor(Math.random() * 16.0).toString(16);
            guid += n;
            if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                guid += "-";
        }
        return guid;
    }


    function initWebUpload(target, options) {
        //判断浏览器是否支持
        if (!WebUploader.Uploader.support()) {
            var error = "上传控件不支持您的浏览器！请尝试升级flash版本或者使用Chrome引擎的浏览器。<a target='_blank' href='http://se.360.cn'>下载页面</a>";

            if (window.console) {
                window.console.log(error);
            }
            $(target).text(error);
            return;
        }

        //文件MD5记录器
        var ArrMD5 = new Array();

        var opts = FileHandle.initOpts(options);
        var hdFileData = $("#" + opts.hiddenInputId);

        var $btn = target.find('#btn-upload'),//上传
            state = 'pending',
            uploader;
        var jsonData = {
            fileList: []
        };
        var $ThisId = target.attr("id");
        var webuploaderoptions = $.extend({
            chunked: true,
            method: "post",
            auto: (opts.Startbtn == "" ? true : false),
            swf: '/js/webuploader-0.1.5/Uploader.swf?k=' + Math.random(),//解决IE下缓存swf文件的问题
            // 文件接收服务端。
            //  server: '/BlogAdmin/WebUploader/Upload' + (opts.innerOptions.chunked == true ? "Chunked" : ""),
            server: '/UpLoad/UpLoadFile',
            // 选择文件的按钮。可选。
            // 内部根据当前运行是创建，可能是input元素，也可能是flash.
            pick: { id: '#' + $ThisId, multiple: opts.multiple },
            // 不压缩image, Default如果是jpeg，文件上传前会压缩一把再上传！
            threads: 1,
            accept: opts.FileAccept,
            resize: false,
            multiple: opts.multiple,
            chunkSize: opts.chunkSize,
            fileNumLimit: opts.fileNumLimit,
            fileSizeLimit: opts.fileSizeLimit,
            fileSingleSizeLimit: opts.fileSingleSizeLimit,
            formData: { fileMd5: newGuid(), wepuploadFileName: opts.FileName, imgIDCard: opts.imgIDCard, fujianname: opts.fujianname, chunkcount: opts.chunkcount }
        },
        opts.innerOptions);
        var uploader = WebUploader.create(webuploaderoptions);
        if (document.getElementById("up_father_" + $ThisId) == null) {
            var imgstyle = opts.imgStyle;
            //gstyle = imgstyle === "" ? "margin-left: 12%;" : imgstyle;
            //mgstyle = imgstyle == null ? "margin-left: 12%;" : imgstyle;
            target.parent().after("<div class =\"col-md-12 dropzone\" filelist=\"" + $ThisId + "\" style=\"" + imgstyle + "\" id=\"up_father_" + $ThisId + "\"></div>");
        }

        if (document.getElementById("up_father_tips_" + $ThisId) == null) {
            if (opts.filetips != null) {
                if (opts.filetips !== "") {
                    $("#" + $ThisId).append("<div class='filetips'>" + opts.filetips + "</div>");
                }
            }

        }
        FileHandle.DefaultUrl($ThisId, opts);

        //当有文件被Add进队列的时候触发
        uploader.on('fileQueued', function (file) {
            //禁用提交
            FileHandle.DisableSubmit();
            if ($.inArray(uploader, uploadArray) < 0) {
                uploadArray.push(uploader);
            }

            $thisuploader = uploader;
            /*保存源文件，压缩文件，文件描述*/
            var FileHtml = "";
            if (opts.multiple) {

                var ThisFileCount = $("#up_father_" + $ThisId).find(".dz-preview").length;
                if (opts.OPath != "") {
                    FileHtml += "<input type=\"hidden\" id=\"WUOPath_" + file.id + "\"  name=\"" + $ThisId + "[" + ThisFileCount + "]." + opts.OPath + "\"/>";
                }
                if (opts.TPath != "") {
                    FileHtml += "<input type=\"hidden\" id=\"WUTPath_" + file.id + "\" name=\"" + $ThisId + "[" + ThisFileCount + "]." + opts.TPath + "\"/>";
                }
                if (opts.FDescription != "") {
                    FileHtml += "<input type=\"hidden\" id=\"WUTDescription_" + file.id + "\" name=\"" + $ThisId + "[" + ThisFileCount + "]." + opts.FDescription + "\"/>";
                }
                if (opts.FileSort != "") {
                    FileHtml += "<input type=\"hidden\" id=\"WUSort_" + file.id + "\" name=\"" + $ThisId + "[" + ThisFileCount + "]." + opts.FileSort + "\"/>";
                }
            }
            else {
                $("#up_father_" + $ThisId).html("");

                if (opts.OPath != "") {
                    FileHtml += "<input type=\"hidden\" id=\"WUOPath_" + file.id + "\" name=\"" + opts.OPath + "\"/>";
                }
                if (opts.TPath != "") {
                    FileHtml += "<input type=\"hidden\" id=\"WUTPath_" + file.id + "\" name=\"" + opts.TPath + "\"/>";
                }
                if (opts.FDescription != "") {
                    FileHtml += "<input type=\"hidden\" id=\"WUTDescription_" + file.id + "\" name=\"" + opts.FDescription + "\"/>";
                }
                if (opts.FileSort != "") {
                    FileHtml += "<input type=\"hidden\" id=\"WUSort_" + file.id + "\" name=\"" + opts.FileSort + "\"/>";
                }
            }
            var html = "<div id=\"child_" + file.id + "\" class=\"dz-preview dz-file-preview dz-processing\">"
                       + "<div class=\"dz-details\">"
                       + "<div class=\"dz-filename\">"
                       + "<span data-dz-name=\"\">" + file.name + "</span>"
                       + "<span style=\"display: block;margin-top: 2px;color: #128C11;\">等待上传...</span>"
                       + "<span></span>"
                       + "</div>"
                       + "<div class=\"dz-size\" data-dz-size=\"\">"
                       + "<strong>" + WebUploader.formatSize(file.size) + "</strong>"
                       + "</div>"
                       + "<img data-dz-thumbnail=\"\" id=\"WUimg_" + file.id + "\" src=\"\">"
                       + "</div>"
                       + "<div class=\"dz-progress\">"
                       + "<span class=\"dz-upload\" data-dz-uploadprogress=\"\">"
                       + "</span>"
                       + "</div>"
                       + "<div class=\"dz-success-mark\">"
                       + "<span style=\"color: #0EDE0B;font-size: 30px;\">✔</span>"
                       + "</div>"
                       + "<div class=\"dz-success-message\">"
                       + "<span data-dz-successmessage=\"\"></span>"
                       + "</div>"
                       + "<div class=\"dz-error-mark\">"
                       + "<span style=\"color: #F10A06;font-size: 30px;\">✘</span>"
                       + "</div>"
                       + "<div class=\"dz-error-message\">"
                       + "<span data-dz-errormessage=\"\"></span>"
                       + "</div>"
                       + "<div class=\"tools\">" + (!opts.EnableEdit ? "" : "<a  id=\"WUFileEdit_" + file.id + "\">Edit</a>&nbsp;&nbsp;") + "<a id=\"WUFileDel_" + file.id + "\" style=\"cursor:pointer\" >Delete</a></div>"
                       + FileHtml
            + "</div>";
            $("#up_father_" + $ThisId + "").append(html);
            $("#WUFileDel_" + file.id).unbind("click");
            $("#WUFileDel_" + file.id).on("click", function () {
                FileHandle.delFile(uploader, file.id, $ThisId);

            });
            $("#WUFileEdit_" + file.id).unbind("click");
            $("#WUFileEdit_" + file.id).on("click", function () {
                if (typeof (opts.onEditImage) == "function") {
                    opts.onEditImage(uploader, $ThisId, file);
                }
                if (typeof (onEditImage) == "function") {
                    onEditImage(uploader, $ThisId, file);
                }
            });
            // $(".dz-success-message").css("display","block");
            //文件MD5记录器
            ArrMD5[file.id] = newGuid();

        });
        // 文件上传过程中创建进度条实时显示。
        uploader.on("uploadProgress", function (file, percentage) {
            $("#child_" + file.id + " .dz-filename").find("span").eq(1).html("正在上传文件");
            $("#child_" + file.id + " .dz-filename").find("span").eq(2).html(WebUploader.formatSize(file.size * percentage));
            $("#child_" + file.id + " .dz-progress .dz-upload").css("width", percentage * 100 + "%");

        });
        //  验证大小
        uploader.on("error", function (type) {
            if (type == "F_DUPLICATE") {
                win.alert("System 提示", "请不要重复选择文件！");
            } else if (type == "Q_EXCEED_SIZE_LIMIT") {
                $.alert("System 提示: 所选附件总大小不可超过" + allMaxSize + "M 哦！");
            }

        });
        uploader.on("uploadBeforeSend", function (o, d, h) {
            if (d["chunk"] == null) {
                //如果文件没有分块，则写入Default的参数InfoMation
                d["chunk"] = "0";
                d["chunks"] = "1";


            }
            d["fileMd5"] = ArrMD5[d["id"]];

        });
        //上传 Success
        uploader.on('uploadSuccess', function (file, response) {
            if ($("#child_" + file.id + " .dz-filename").find("span").eq(1).text() != "极速秒传") {
                $("#child_" + file.id + " .dz-filename").find("span").eq(1).html("上传 Success！");
                $("#child_" + file.id + " .dz-filename").find("span").eq(2).html("");
                $("#WUimg_" + file.id).attr("src", response.filePath);
                $("#WUimg_" + file.id).attr("title", response.filename);
                $("#WUOPath_" + file.id).attr("title", response.filename);
                $("#WUOPath_" + file.id).val(response.filevalue);
                $("#WUimg_" + file.id).attr("filevalue", response.filevalue);
            }
            //移除错误样式
            $("#child_" + file.id + "").removeClass("dz-error");
            $("#child_" + file.id + " .dz-error-message").find("span").eq(0).html("");
            //Add上传 Success样式
            $("#child_" + file.id + "").addClass("dz-success");
            var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串  
            var isIE = userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1; //判断是否IE<11浏览器  
            var isEdge = userAgent.indexOf("Edge") > -1 && !isIE; //判断是否IE的Edge浏览器  
            var isIE11 = userAgent.indexOf('Trident') > -1 && userAgent.indexOf("rv:11.0") > -1;
            if (isIE) {
                var reIE = new RegExp("MSIE (\\d+\\.\\d+);");
                reIE.test(userAgent);
                var fIEVersion = parseFloat(RegExp["$1"]);
                if (fIEVersion > 9) {
                    $(".col-md-12").html("");
                }
            }
            if (isIE11) {
                $(".col-md-12").html("");
            }
            if ($(".col-md-12").html() == "") {
                var mm = "<div id=\"child_WU_FILE_0\" class=\"dz-preview dz-file-preview dz-processing dz-success\"><div class=\"dz-details\"><div class=\"dz-filename\"><span data-dz-name=\"\">" + response.filename + "</span><span style=\"display: block;margin-top: 2px;color: #128C11;\">上传 Success！</span><span></span></div><div class=\"dz-size\" data-dz-size=\"\"><strong>" + WebUploader.formatSize(file.size) + "</strong></div><img data-dz-thumbnail=\"\" id=\"WUimg_WU_FILE_0\" src=\"" + response.filePath + "\" title=\"" + response.filename + "\" filevalue=\"" + response.filevalue + "\" style=\"display: inline;\"></div><div class=\"dz-progress\"><span class=\"dz-upload\" data-dz-uploadprogress=\"\" style=\"width: 100%;\"></span></div><div class=\"dz-success-mark\"><span style=\"color: #0EDE0B;font-size: 30px;\">✔</span></div><div class=\"dz-success-message\"><span data-dz-successmessage=\"\"></span></div><div class=\"dz-error-mark\"><span style=\"color: #F10A06;font-size: 30px;\">✘</span></div><div class=\"dz-error-message\"><span data-dz-errormessage=\"\"></span></div><div class=\"tools\"><a id=\"WUFileDel_WU_FILE_0\" style=\"cursor:pointer\">Delete</a></div><input type=\"hidden\" id=\"WUOPath_WU_FILE_0\" name=\"ImgUrl_1\" title=\"" + response.filename + "\" value=\"" + response.filevalue + "\"></div>";
                $(".col-md-12").html(mm);
            }
            opts.onUploadSuccess(file, response);
            if (typeof (onUploadSuccess) == "function") {
                onUploadSuccess(uploader, $ThisId, file, response);
            }
            if (response.chunkcount > 1 && response.filename != undefined && response.filevalue != "") {
                $.get("/UpLoad/Combine?uploadDir=" + response.filevalue, function (a) {
                    if (a != "true") {
                        $.alert(a);
                    }
                })
            }
        });
        //上传出错
        uploader.on('uploadError', function (file, reason) {
            $("#child_" + file.id + "").removeClass("dz-success");
            $("#child_" + file.id + "").addClass("dz-error");
            $("#child_" + file.id + " .dz-error-message").find("span").eq(0).html('上传出错');
            //$('#' + file.id).find('p.state').text('上传出错');
        });
        uploader.on('uploadComplete', function (file) {
            opts.onComplete(file);
        });

        uploader.on('all', function (type) {
            if (type === 'startUpload') {
                state = 'uploading';
            } else if (type === 'stopUpload') {
                state = 'paused';
            } else if (type === 'uploadFinished') {
                state = 'done';
                opts.onAllComplete($ThisId);
                FileHandle.checkNotCompleted();
                if (typeof (onAllComplete) == "function") {
                    onAllComplete(uploader, $ThisId);
                }
            }
            if (state === 'uploading') {
                $("#" + opts.Startbtn).text('暂停上传');
                $("#" + opts.Startbtn).val('暂停上传');
            } else {
                $("#" + opts.Startbtn).text('开始上传');
                $("#" + opts.Startbtn).val('开始上传');
            }
        });
        //手动上传
        $("#" + opts.Startbtn).on('click', function () {
            //if (uploader.getStats().queueNum <= 0)
            //{
            //    alert("无可上传的文件！");
            //}
            //else
            //{
            if (state === 'uploading') {
                uploader.stop();
            } else {
                uploader.upload();
            }
            // }
        });
        //Delete
        //$list.on("click", ".del", function () {
        //    var $ele = $(this);
        //    var id = $ele.parent().attr("id");
        //    var deletefile = {};
        //    $.each(jsonData.fileList, function (index, item) {
        //        if (item && item.queueId === id) {
        //            uploader.removeFile(uploader.getFile(id));//不要遗漏
        //            deletefile = jsonData.fileList.splice(index, 1)[0];
        //            $("#" + opts.hiddenInputId).val(JSON.stringify(jsonData));
        //            $.post(applicationi + "/usercenter/delimg", { 'filepathname': deletefile.filePath }, function (returndata) {
        //                alert(returndata);
        //                $.alert(returndata.result);
        //                $ele.parent().remove();
        //            });
        //            return;
        //        }
        //    });
        //});
    }

    $.fn.myUpFile = function (options) {
        var $initObj = $(this);
        var objId = $initObj.attr("id");
        if ($.inArray(objId, uploadIsInit) < 0) {
            uploadIsInit.push(objId);
            initWebUpload($initObj, options);
        }
        var opts = FileHandle.initOpts(options);
        FileHandle.DefaultUrl(objId, opts);
    }



    $(function () {
        load();
        $(".select2").each(function (i) {
            $(this).def_select2("");

        });
        $(".select2_ajax_load").each(function (i) {
            $(this).ajax_select2("");

        });
        $(".checkbox_ajax_load").each(function (i) {
            $(this).ajax_CheckBox("");

        });
        $(".radio_ajax_load").each(function (i) {
            $(this).ajax_Radio("");

        });
        //初始化文件上传控件
        $(".myupload").on("click", function () {
            commonfileload($(this).attr("id"));
        })
        //初始化文件上传控件
        function load() {
            $(".myupload").each(function (i) {
                commonfileload($(this).attr("id"));
            });
        }
        function commonfileload(id) {
            var $ThisObject = $("#" + id);
            var $ThisId = $ThisObject.attr("id");
            var multi = $ThisObject.attr("multi") != undefined && $ThisObject.attr("multi") != "" && $ThisObject.attr("multi") != "null" && $ThisObject.attr("multi") != null ? true : false;
            var filenamest = $ThisObject.attr("fujianname") != undefined && $ThisObject.attr("fujianname") != "" && $ThisObject.attr("fujianname") != "null" && $ThisObject.attr("fujianname") != null ? $ThisObject.attr("fujianname") : "";
            var startbtn = Null_Convert($ThisObject.attr("startbtn"), "");
            var chunksize = Null_Convert($ThisObject.attr("chunksize"), 10 * 1024 * 1024);
            var OPath = Null_Convert($ThisObject.attr("opath"), "");
            var TPath = Null_Convert($ThisObject.attr("tpath"), "");
            var FDescription = Null_Convert($ThisObject.attr("fdescription"), "");
            var FileName = Null_Convert($ThisObject.attr("filename"), "");
            var para = Null_Convert($ThisObject.attr("para"), "");
            var FileType = Null_Convert($ThisObject.attr("fileType"), "");
            var EnableEdit = Null_Convert($ThisObject.attr("enableedit"), false);
            if (EnableEdit === "true") {
                EnableEdit = true;
            }
            var EnableEdit = Null_Convert($ThisObject.attr("enableedit"), false);
            var defaultUrl = Null_Convert($ThisObject.attr("defaulturl"), "");
            var FileSort = Null_Convert($ThisObject.attr("filesort"), "");
            var imgIDCard = Null_Convert($ThisObject.attr("imgidcard"), false);
            var filetips = Null_Convert($ThisObject.attr("filetips"), "");
            var imgStyle = Null_Convert($ThisObject.attr("imgStyle"), "");
            var ops = { multiple: multi, chunkSize: chunksize, Startbtn: startbtn, OPath: OPath, TPath: TPath, FDescription: FDescription, FileName: FileName, FileType: FileType, EnableEdit: EnableEdit, FileSort: FileSort, defaultUrl: defaultUrl, imgIDCard: imgIDCard, filetips: filetips, imgStyle: imgStyle, fujianname: filenamest, chunkcount: 0 };
            $ThisObject.myUpFile(ops);
        }
    });

})(jQuery, window);