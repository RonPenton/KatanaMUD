/// <reference path="jquery.d.ts" />
var KMud;
(function (KMud) {
    $(function () {
        $(".reset").click(function (evt) {
            var input = $(evt.target);
            while (down(input)) {
            }
        });
        $(".down").click(function (evt) {
            down($(evt.target));
        });
        $(".up").click(function (evt) {
            up($(evt.target));
        });
        $(".max").click(function (evt) {
            var input = $(evt.target);
            while (up(input)) {
            }
        });
    });
    function down(input) {
        var row = input.closest("div");
        var input = row.find("input[type='text']");
        var current = parseInt(row.find(".current").val());
        var val = parseInt(input.val()) - 1;
        if (val < current)
            return false;
        var points = cpsPerPoint(row, val + 1);
        input.val(val.toString());
        $("#cps").val(String(parseInt($("#cps").val()) + points));
        return true;
    }
    function up(input) {
        var row = input.closest("div");
        var input = row.find("input[type='text']");
        var val = parseInt(input.val()) + 1;
        var points = cpsPerPoint(row, val);
        var cps = $("#cps").val();
        if (points > cps)
            return false;
        input.val(val.toString());
        $("#cps").val(String(parseInt($("#cps").val()) - points));
        return true;
    }
    function cpsPerPoint(row, point) {
        var initial = parseInt(row.find(".initial").val());
        var difference = point - initial;
        if (difference < 0)
            throw new Error();
        // 0 -> 0
        // 1-10 -> 1
        // 11-20 -> 2
        // 21->30 -> 3
        // etc
        return Math.floor((difference + 9) / 10);
    }
})(KMud || (KMud = {}));
