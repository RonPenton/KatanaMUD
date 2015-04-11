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
        $(".edit").focus(function (evt) {
            var row = new Row($(evt.target).closest("div"));
            row.input[0].previousValue = row.val();
        }).change(function (evt) {
            var row = new Row($(evt.target).closest("div"));
            var start = row.input[0].previousValue;
            var end = row.val();
            var points = row.cpsCost(start, end);
            if (points == NaN || points > cps()) {
                row.val(start);
                return;
            }
            cps(-points);
        }).click(function (evt) {
            $(evt.target).select();
        });
        if ($("#Name").is(":disabled"))
            $("#Strength").select();
        else
            $("#Name").select();
    });
    function down(input) {
        var row = input.closest("div");
        var stats = new Row(row);
        if (stats.val() == stats.current)
            return false;
        var newVal = stats.val() - 1;
        var points = stats.cpsPerPoint(newVal + 1);
        stats.val(newVal);
        cps(points);
        return true;
    }
    function up(input) {
        var row = input.closest("div");
        var stats = new Row(row);
        var newVal = stats.val() + 1;
        var points = stats.cpsPerPoint(newVal);
        if (points > cps() || newVal > stats.max)
            return false;
        stats.val(newVal);
        cps(-points);
        return true;
    }
    function cps(val) {
        if (val !== undefined) {
            $("#cps").val(String(parseInt($("#cps").val()) + val));
        }
        return parseInt($("#cps").val());
    }
    var Row = (function () {
        function Row(row) {
            this.initial = parseInt(row.find(".initial").val());
            this.current = parseInt(row.find(".current").val());
            this.max = parseInt(row.find(".maxval").val());
            this.input = row.find("input[type='text']");
        }
        Row.prototype.val = function (points) {
            if (points !== undefined) {
                this.input.val(String(points));
            }
            return parseInt(this.input.val());
        };
        Row.prototype.cpsPerPoint = function (point) {
            var difference = point - this.initial;
            if (difference < 0)
                throw new Error();
            // 0 -> 0
            // 1-10 -> 1
            // 11-20 -> 2
            // 21->30 -> 3
            // etc
            return Math.floor((difference + 9) / 10);
        };
        Row.prototype.cpsCost = function (start, end) {
            if (end < this.current || end > this.max)
                return NaN;
            var cost = 0;
            while (end < start) {
                cost -= this.cpsPerPoint(start);
                start--;
            }
            while (end > start) {
                cost += this.cpsPerPoint(start + 1);
                start++;
            }
            return cost;
        };
        return Row;
    })();
})(KMud || (KMud = {}));
