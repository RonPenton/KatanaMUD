/// <reference path="jquery.d.ts" />
var KMud;
(function (KMud) {
    $(function () {
        $(".reset").click(function (evt) {
            var row = $(evt.target).closest("div");
            var input = row.find("input[type='text']");
            input.val(row.find(".initial").val());
            calculateCps();
        });
        $(".down").click(function (evt) {
            var row = $(evt.target).closest("div");
            var input = row.find("input[type='text']");
            var initial = row.find(".initial").val();
            var val = input.val() - 1;
            if (val < initial)
                val = initial;
            input.val(val.toString());
            calculateCps();
        });
        $(".up").click(function (evt) {
        });
    });
    function calculateCps() {
    }
})(KMud || (KMud = {}));
