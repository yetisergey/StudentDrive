var ViewModel = {
    actionText: ko.observable(),
    My_Drive: function () {
        $("#labelupload").show();
        $("#share").show();
        $("#download").show();
        $("#delete").show();
        $("#deleteShares").hide();
        $("#recovery").hide();
        $("#deleteShareFromFile").show();
        ViewModel.ListFiles.visibleShared(true);
        ViewModel.ListFiles.visibleRewrite(false);

        GetFiles();
    },
    Share: function () {
        $("#labelupload").hide();
        $("#share").hide();
        $("#download").show();
        $("#delete").hide();
        $("#deleteShares").show();
        $("#recovery").hide();
        $("#deleteShareFromFile").hide();
        ViewModel.ListFiles.visibleShared(false);
        ViewModel.ListFiles.visibleRewrite(true);

        GetSharedFiles();
    },
    Recycle_bin: function () {
        $("#labelupload").hide();
        $("#share").hide();
        $("#download").hide();
        $("#delete").show();
        $("#deleteShares").hide();
        $("#recovery").show();
        $("#deleteShareFromFile").hide();
        ViewModel.ListFiles.visibleShared(false);
        ViewModel.ListFiles.visibleRewrite(false);

        GetDeletedFiles();
    },
    ListFiles: new ListFilesImplementation(ko),
    ListPlayers: new ListPlayersImplementation(ko),
    SettingsPlayer: new SettingsPlayerImplementation(ko),
    ListPlayersForShare: new ListPlayersForShareImplementation(ko)
};

function GetFiles() {
    $.ajax({
        url: '/api/file',
        type: 'GET',
        success: function (data) {
            $(".visibleShared").show()
            $(".visibleRewrite").hide()

            ViewModel.ListFiles.Array(data);
            ActionForSettingShare()
        },
        error: function (e) {
            console.log(e.message);
            RefreshTableFiles()
        }
    });
};

function ActionForSettingShare() {
    $('.sharePlayerModalSettings').click(function () {
        var get_cls = $(this).attr("class");
        var answer = get_cls.split(" ").pop();
        $("#shareModalSettings").modal("show");
        $.ajax({
            url: '/api/sharesettings/?id=' + answer,
            type: 'GET',
            success: function (data) {
                ViewModel.ListPlayersForShare.CurrentIdFile(answer);
                ViewModel.ListPlayersForShare.Array(data);
            },
            error: function (e) {
                console.log(e);
                RefreshTableFiles()
            }
        });
    });
}
function ActionForOverWriteFile() {
    $(".reloadFileBtn").change(function () {
        var get_cls = $(this).attr("class");
        var answer = get_cls.split(" ").pop();
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        if (numFiles > 0) {
            var fd = new FormData();
            $.each(input.prop('files'), function (it, val) {
                fd.append('file' + it, val);
            });
            $.ajax({
                url: '/api/ShareSettings/?fileId=' + answer,
                data: fd,
                processData: false,
                contentType: false,
                type: 'PUT',
                success: function (data) {
                    GetSharedFiles()
                    RefreshTableFiles()
                },
                error: function (e) {
                    console.log(e.message);
                    RefreshTableFiles()
                }
            });
        }
    });
}

function GetSharedFiles() {
    $.ajax({
        url: '/api/shared',
        type: 'GET',
        success: function (data) {
            ViewModel.ListFiles.Array(data);
            $(".visibleShared").hide()
            $(".visibleRewrite").show()
            ActionForOverWriteFile();
            RefreshTableFiles();
        },
        error: function (e) {
            console.log(e);
            RefreshTableFiles()
        }
    });
};

function GetDeletedFiles() {
    $.ajax({
        url: '/api/deleted',
        type: 'GET',
        success: function (data) {

            ViewModel.ListFiles.Array(data);
            $(".visibleShared").hide()
            $(".visibleRewrite").hide()

            RefreshTableFiles()
        },
        error: function (e) {
            console.log(e.message);
            RefreshTableFiles()
        }
    });
};


function ListPlayersForShareImplementation(ko) {

    this.CurrentIdFile = ko.observable();
    this.Id = ko.observable();
    this.FirstName = ko.observable();
    this.SecondName = ko.observable();
    this.checkedShare = ko.observable();
    this.Array = ko.observableArray();

    this.onSubmit = function () {
        $.ajax({
            url: '/api/ShareSettings/?fileId=' + ViewModel.ListPlayersForShare.CurrentIdFile(),
            data: JSON.stringify(ViewModel.ListPlayersForShare.Array()),
            type: 'POST',
            contentType: "application/json",
            success: function (data) {
                ViewModel.My_Drive();
                RefreshTableFiles()
            },
            error: function (e) {
                console.log(e.message);
                RefreshTableFiles()
            }
        });
    };
    this.removeFromList = function (obj, elem) {
        var isChecked = $("#" + elem.target.id).is(':checked');
        ViewModel.ListPlayersForShare.Array.remove(obj);
        $('.' + elem.target.className).remove();
        return true;
    };
}

function ListPlayersImplementation(ko) {
    this.Id = ko.observable();
    this.FirstName = ko.observable();
    this.SecondName = ko.observable();
    this.Login = ko.observable();
    this.Password = ko.observable();
    this.Space = ko.observable();
    this.Array = ko.observableArray();
    this.ArrayChecked = ko.observableArray();
    this.ArrayRewriter = ko.observableArray();

    this.onSubmit = function () {
        if (ViewModel.ListPlayers.ArrayChecked().length > 0) {
            var url = "";

            $.each(ViewModel.ListPlayers.ArrayChecked(), function (it, val) {
                if (ViewModel.ListPlayers.ArrayRewriter.indexOf(val) < 0) {
                    url += "ids[" + it + "][id]=" + val.Id + "&ids[" + it + "][isRew]=false&";
                } else {
                    url += "ids[" + it + "][id]=" + val.Id + "&ids[" + it + "][isRew]=true&";
                }
            });

            var dataTransfer = $.map(ViewModel.ListFiles.ArrayChecked(), function (val) {
                return val.Id;
            })
            $.ajax({
                url: '/api/file/ids?' + url,
                data: JSON.stringify(dataTransfer),
                type: 'PUT',
                contentType: "application/json",
                success: function (data) {
                    ViewModel.My_Drive();
                    RefreshTableFiles()
                },
                error: function (e) {
                    console.log(e.message);
                    RefreshTableFiles()
                }
            });
        }
    };
    this.addPlayerToChecked = function (obj, elem) {
        var isChecked = $("#" + elem.target.id).is(':checked');

        if (isChecked && ViewModel.ListPlayers.ArrayChecked.indexOf(obj) < 0) {
            ViewModel.ListPlayers.ArrayChecked.push(obj);
        }
        else if (!isChecked && ViewModel.ListPlayers.ArrayChecked.indexOf(obj) >= 0) {
            ViewModel.ListPlayers.ArrayChecked.remove(obj);
        }

        return true;
    };
    this.addPlayerToReWrite = function (obj, elem) {
        var isChecked = $("." + elem.target.className).is(':checked');

        if (isChecked && ViewModel.ListPlayers.ArrayRewriter.indexOf(obj) < 0) {
            ViewModel.ListPlayers.ArrayRewriter.push(obj);
        }
        else if (!isChecked && ViewModel.ListPlayers.ArrayRewriter.indexOf(obj) >= 0) {
            ViewModel.ListPlayers.ArrayRewriter.remove(obj);
        }

        return true;
    };
}

function ListFilesImplementation(ko) {

    this.Id = ko.observable();
    this.Name = ko.observable();
    this.Size = ko.observable();
    this.UserId = ko.observable();
    this.Check = ko.observable(false);
    this.Array = ko.observableArray();
    this.ArrayChecked = ko.observableArray();
    this.parseToMB = function (by) {
        var MB = by / 1048576
        return MB.toFixed(2) + " MB";
    };
    this.addFileToChecked = function (obj, elem) {
        var isChecked = $("#" + elem.target.id).is(':checked');

        if (isChecked && ViewModel.ListFiles.ArrayChecked.indexOf(obj) < 0) {
            ViewModel.ListFiles.ArrayChecked.push(obj);
        }
        else if (!isChecked && ViewModel.ListFiles.ArrayChecked.indexOf(obj) >= 0) {
            ViewModel.ListFiles.ArrayChecked.remove(obj);
        }

        return true;
    }
    this.addAllFileToChecked = function () {
        $.each(ViewModel.ListFiles.Array(), function (it, val) {
            if ($("#" + val.Id).is(':checked') == false) {
                if (ViewModel.ListFiles.ArrayChecked.indexOf(val) < 0) {
                    ViewModel.ListFiles.ArrayChecked.push(val);
                }
            } else {
                if (ViewModel.ListFiles.ArrayChecked.indexOf(val) >= 0) {
                    ViewModel.ListFiles.ArrayChecked.remove(val);
                }
            }
            $("#" + val.Id).click();

        });
        return true;
    }
    this.DateOfUpload = ko.observable();
    this.parseToDate = function (obj) {
        var date = new Date(obj);
        return date.toLocaleDateString();
    }

    this.Share = ko.observable();
    this.parseToShare = function (obj, Id) {
        if (obj === true) {
            return "<button class='btn btn-sm btn-default sharePlayerModalSettings " + Id + "' >Свойства</button>";
        } else {
            return "<span class='glyphicon glyphicon-thumbs-down'></span>";
        }
    }
    this.visibleShared = ko.observable(true);
    this.parseToRewrite = function (obj, id) {
        if (obj === true) {
            //"<label class='btn btn-md btn-success btn-file'><span class='glyphicon glyphicon-cloud-upload'></span> Перезаписать <input type='file'  class='reloadFileBtn " + id + "' multiple style='display: none;'></label>"
            return "<label class='btn btn-md btn-success btn-file'><span class='glyphicon glyphicon-cloud-upload'></span> Перезаписать <input type='file'  class='reloadFileBtn " + id + "' multiple style='display: none;'></label>"//"<button class='btn btn-sm btn-success reloadFileBtn "+id+"'>Перезаписать</button>";
        } else {
            return "";
        }
    }
    this.Rewrite = ko.observable();
    this.visibleRewrite = ko.observable(false);
}

function SettingsPlayerImplementation(ko) {
    this.FirstName = ko.observable("");
    this.SecondName = ko.observable("");
    this.Login = ko.observable("");
    this.Password = ko.observable("");
    this.Space = ko.observable("");
    this.VkId = ko.observable("");
    this.onSubmit = function () {
        var search = window.location.href;
        var splited = search.split("/");
        var guid = splited[splited.length - 1];
        var value = {
            FirstName: ViewModel.SettingsPlayer.FirstName(),
            SecondName: ViewModel.SettingsPlayer.SecondName(),
            Login: ViewModel.SettingsPlayer.Login(),
            Password: (ViewModel.SettingsPlayer.Password() == "" ? ViewModel.SettingsPlayer.FirstName() : ViewModel.SettingsPlayer.Password()),
            Space: ViewModel.SettingsPlayer.Space(),
            VkId: ViewModel.SettingsPlayer.VkId(),
        }

        if (value.FirstName != "" &&
            value.SecondName != "" &&
            value.Password != "" &&
            value.Login != "") {
            $.ajax({
                url: '/api/user/?id=' + guid,
                data: JSON.stringify(value),
                type: 'PUT',
                contentType: "application/json",
                success: function (data) {
                    console.log(data);
                },
                error: function (e) {
                    console.log(e.message);
                }
            });
        }
    };
    this.onDelete = function () {
        $.ajax({
            url: '/api/user',
            type: 'DELETE',
            success: function (data) {
                window.location.reload()
            },
            error: function (e) {
                console.log(e.message);
            }
        });
    };
}

function RefreshTableFiles() {
    $("#selectall").prop("checked", false);
    $.each(ViewModel.ListFiles.Array(), function (it, val) {
        if ($("#" + val.Id).is(':checked') == true) {
            $("#" + val.Id).click();
        }

    });
    ViewModel.ListFiles.ArrayChecked([]);
    $("#upload").val("");
    GetSizeDrivePlayer();

}

function RefreshTablePlayers() {
    $.each(ViewModel.ListPlayers.Array(), function (it, val) {
        if ($("#" + val.Id).is(':checked') == true) {
            $("#" + val.Id).click();
        }

    });
    ViewModel.ListPlayers.ArrayChecked([]);
}

function GetNamePlayer() {
    $.ajax({
        url: '/api/user',
        type: 'GET',
        success: function (data) {
            $("#namePlayer").html(data.f + " " + data.i);
            CreateDiskUsage((data.s / 262144000) * 100)
            textUsageDisk(data.s / 1000000 + " МБ");

            CreateStatistic(data.uc, data.dc, data.sc)
        },
        error: function (e) {
            console.log(e);
        }
    });
}
function GetSizeDrivePlayer() {
    $.ajax({
        url: '/api/user',
        type: 'GET',
        success: function (data) {
            CreateDiskUsage((data.s / 262144000) * 100);
            textUsageDisk(data.s / 1000000);
            myChart.destroy();
            CreateStatistic(data.uc, data.dc, data.sc)
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function textUsageDisk(s) {
    $('#textUsageDisk').html(s + " MB")
}

var myChart;
function CreateStatistic(uc, dc, sc) {
    var ctx = document.getElementById("myChart");
    myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["Скачано", "Загружено", "Расшарено"],
            datasets: [{
                label: ["Статистика по файлам"],
                data: [dc, uc, sc],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }],

        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}

function CreateDiskUsage(val) {
    var myCircle = Circles.create({
        id: 'circles-1',
        radius: 75,
        value: val,
        maxValue: 100,
        width: 10,
        text: function (value) { return value + '%'; },
        colors: ['#D3B6C6', '#00ffff'],
        duration: 800,
        wrpClass: 'circles-wrp',
        textClass: 'circles-text',
        valueStrokeClass: 'circles-valueStroke',
        maxValueStrokeClass: 'circles-maxValueStroke',
        styleWrapper: true,
        styleText: true
    });
}

$(function () {
    $("#deleteShares").hide();
    $("#recovery").hide();
    $('body').loading({
        message: "Загрузка..."
    });

    $(document).ajaxStart(function () {
        $('body').loading('start');
    });

    $(document).ajaxComplete(function () {
        $('body').loading('stop');
    });

    GetFiles();
    GetNamePlayer();

    ko.applyBindings(ViewModel);

    //загрузка файлов
    $("#upload").change(function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        if (numFiles > 0) {
            var fd = new FormData();
            $.each(input.prop('files'), function (it, val) {
                fd.append('file' + it, val);
            });
            $.ajax({
                url: '/api/file',
                data: fd,
                processData: false,
                contentType: false,
                type: 'POST',
                success: function (data) {
                    $.each(data, function (it, val) {
                        ViewModel.ListFiles.Array.push(val);
                    });
                    RefreshTableFiles()
                },
                error: function (e) {
                    console.log(e.message);
                    RefreshTableFiles()
                }
            });
        }
    });

    //удаление файлов
    $("#delete").click(function () {
        if (ViewModel.ListFiles.ArrayChecked().length > 0) {
            var url = "";
            $.each(ViewModel.ListFiles.ArrayChecked(), function (it, val) {
                url += "ids=" + val.Id + "&";
            });
            $.ajax({
                url: '/api/file/?' + url,
                type: 'DELETE',
                success: function (data) {
                    $.each(ViewModel.ListFiles.ArrayChecked(), function (it, val) {
                        ViewModel.ListFiles.Array.remove(val);
                    });
                    RefreshTableFiles();
                },
                error: function (e) {
                    console.log(e.message);
                    RefreshTableFiles();
                }
            });
        }
    });

    //восстановление файлов
    $("#recovery").click(function () {
        if (ViewModel.ListFiles.ArrayChecked().length > 0) {
            var url = "";
            $.each(ViewModel.ListFiles.ArrayChecked(), function (it, val) {
                url += "id=" + val.Id + "&";
            });
            $.ajax({
                url: '/api/deleted/?' + url,
                type: 'PUT',
                contentType: "application/json",
                success: function (data) {
                    $.each(ViewModel.ListFiles.ArrayChecked(), function (it, val) {
                        ViewModel.ListFiles.Array.remove(val);
                    });
                    RefreshTableFiles();
                },
                error: function (e) {
                    console.log(e.message);
                    RefreshTableFiles();
                }
            });
        }
    });

    //распространение файлов
    $("#share").click(function () {
        if (ViewModel.ListFiles.ArrayChecked().length > 0) {
            $.ajax({
                url: '/api/users',
                type: 'GET',
                success: function (data) {
                    ViewModel.ListPlayers.Array(data);
                },
                error: function (e) {
                    console.log(e.message);
                }
            });
            $("#playersModalShare").modal("show");
        }
    });

    //скачивание
    $("#download").click(function () {
        if (ViewModel.ListFiles.ArrayChecked().length > 0) {
            var url = "";
            $.each(ViewModel.ListFiles.ArrayChecked(), function (it, val) {
                url += "id=" + val.Id + "&name=" + val.Name + "&";
            });
            window.open("/Home/File/?" + url);
        }
        RefreshTableFiles()
    });

    //удаление расшаренных файлов
    $("#deleteShares").click(function () {
        if (ViewModel.ListFiles.ArrayChecked().length > 0) {
            var url = "";
            $.each(ViewModel.ListFiles.ArrayChecked(), function (it, val) {
                url += "ids=" + val.Id + "&";
            });
            $.ajax({
                url: '/api/shared/?' + url,
                type: 'DELETE',
                success: function (data) {
                    $.each(ViewModel.ListFiles.ArrayChecked(), function (it, val) {
                        ViewModel.ListFiles.Array.remove(val);
                    });
                    RefreshTableFiles()
                },
                error: function (e) {
                    console.log(e.message);
                    RefreshTableFiles()
                }
            });
        }
    });

    //настройки пользователя
    $("#settingsPlayer").click(function () {
        var search = window.location.href;
        var splited = search.split("/");
        var guid = splited[splited.length - 1];
        $.ajax({
            url: '/api/user/?id=' + guid,
            type: 'GET',
            success: function (data) {
                ViewModel.SettingsPlayer.FirstName(data.FirstName);
                ViewModel.SettingsPlayer.SecondName(data.SecondName);
                ViewModel.SettingsPlayer.Login(data.Login);
                ViewModel.SettingsPlayer.Password(data.Password);
                ViewModel.SettingsPlayer.VkId(data.VkId);
                ViewModel.SettingsPlayer.Space(data.Space);
            },
            error: function (e) {
                console.log(e.message);
            }
        });
        $("#playerModalSettings").modal("show");
    });

    $('#playersModalShare').on('hide.bs.modal', function () {
        RefreshTablePlayers();
    });

    $("#deleteShareFromFile").click(function () {
        if (ViewModel.ListFiles.ArrayChecked().length > 0) {
            var url = "";
            $.each(ViewModel.ListFiles.ArrayChecked(), function (it, val) {
                url += "ids=" + val.Id + "&";
            });
            $.ajax({
                url: '/api/shared/?' + url,
                type: 'PUT',
                contentType: "application/json",
                success: function (data) {
                    ViewModel.My_Drive();
                    RefreshTableFiles();

                },
                error: function (e) {
                    ViewModel.My_Drive();

                    console.log(e.message);
                    RefreshTableFiles();
                }
            });
        }
    });

    $('#statisticPlayer').click(function () {
        $("#statisticModalSettings").modal("show");
    });

});